using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using pkNX.Containers;
using pkNX.Structures;
using pkNX.Structures.FlatBuffers;
using pkNX.WinForms.Subforms;

namespace pkNX.WinForms;

public class EncounterDumperSV
{
    private readonly IFileInternal ROM;

    public EncounterDumperSV(IFileInternal rom) => ROM = rom;

    public void DumpTo(string path, IReadOnlyList<string> specNames, IReadOnlyList<string> moveNames,
        Dictionary<string, (string Name, int Index)> placeNameMap,
        bool writeText = true, bool writePickle = true)
    {
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);

        var field = new PaldeaFieldModel(ROM);
        var scene = new PaldeaSceneModel(ROM, field);
        var fsym = new PaldeaFixedSymbolModel(ROM);
        var mlEncPoints = FlatBufferConverter.DeserializeFrom<PointDataArray>(ROM.GetPackedFile("world/data/encount/point_data/point_data/encount_data_100000.bin"));
        var alEncPoints = FlatBufferConverter.DeserializeFrom<PointDataArray>(ROM.GetPackedFile("world/data/encount/point_data/point_data/encount_data_atlantis.bin"));
        var pokeData = FlatBufferConverter.DeserializeFrom<EncountPokeDataArray>(ROM.GetPackedFile("world/data/encount/pokedata/pokedata/pokedata_array.bin"));
        var fsymData = FlatBufferConverter.DeserializeFrom<FixedSymbolTableArray>(ROM.GetPackedFile("world/data/field/fixed_symbol/fixed_symbol_table/fixed_symbol_table_array.bin"));

        var result = new Dictionary<int, List<PaldeaEncounter>>();
        using var sw = File.CreateText(Path.Combine(path, "titan_enc.txt"));
        foreach (var areaName in scene.areaNames)
        {
            var areaInfo = scene.AreaInfos[areaName];
            var name = areaInfo.LocationNameMain;
            if (string.IsNullOrEmpty(name))
                continue;
            if (areaInfo.Tag is AreaTag.NG_Encount or AreaTag.NG_All)
                continue;

            // Determine potential spawners
            var areaEncPoints = GetAreaPoints(scene, areaName, alEncPoints, mlEncPoints, areaInfo);
            var slots = GetEncounters(areaEncPoints, pokeData, areaName, scene);

            // Consolidate encounters
            ConsolidateEncounters(slots);

            // Add to loc id
            var locationID = placeNameMap[name].Index;
            AddSlots(result, locationID, slots);

            // Output to stream if available.
            if (writeText)
                WriteLocation(sw, specNames, placeNameMap, areaName, areaInfo, slots);
        }

        if (writeText)
        {
            using var tw = File.CreateText(Path.Combine(path, "titan_loc_enc.txt"));
            WriteLocEncList(tw, specNames, placeNameMap, result);
        }
        if (writePickle)
        {
            string binPath = Path.Combine(path, "encounter_wild_paldea.pkl");
            SerializeEncounters(binPath, result);
        }

        // Fixed symbols
        List<byte[]> serialized = new();
        int[] bannedIndexes = { 31 }; // Lighthouse Wingull
        foreach (var (game, gamePoints) in new[] { ("sl", fsym.scarletPoints), ("vl", fsym.violetPoints)})
        {
            using var gw = File.CreateText(Path.Combine(path, $"titan_fixed_{game}.txt"));
            for (var i = 0; i < fsymData.Table.Length; i++)
            {
                FixedSymbolTable? entry = fsymData.Table[i];
                var tableKey = entry.TableKey;

                var points = gamePoints.Where(p => p.TableKey == tableKey).ToList();
                if (points.Count == 0)
                    continue;

                var areas = new List<string>();
                foreach (var areaName in scene.areaNames)
                {
                    if (scene.isAtlantis[areaName])
                        continue;

                    var areaInfo = scene.AreaInfos[areaName];
                    var name = areaInfo.LocationNameMain;
                    if (string.IsNullOrEmpty(name))
                        continue;
                    if (areaInfo.Tag is AreaTag.NG_Encount or AreaTag.NG_All)
                        continue;

                    if (points.Any(p => scene.IsPointContained(areaName, p.Position.X, p.Position.Y, p.Position.Z)))
                        areas.Add(areaName);
                }

                var locs = areas.Select(a => placeNameMap[scene.AreaInfos[a].LocationNameMain].Index).Distinct().ToList();

                gw.WriteLine("===");
                gw.WriteLine(entry.TableKey);
                gw.WriteLine("===");
                gw.WriteLine("  PokeData:");
                var pd = entry.PokeDataSymbol;
                gw.WriteLine($"    Species: {specNames[(int)pd.DevId]}");
                gw.WriteLine($"    Form:    {pd.FormId}");
                gw.WriteLine($"    Level:   {pd.Level}");
                gw.WriteLine($"    Sex:     {new[] { "Random", "Male", "Female" }[(int)pd.Sex]}");
                gw.WriteLine($"    Shiny:   {new[] { "Random", "Never", "Always" }[(int)pd.RareType]}");

                var talentStr = pd.TalentType switch
                {
                    TalentType.RANDOM => "Random",
                    TalentType.V_NUM => $"{pd.TalentVNum} Perfect",
                    TalentType.VALUE => $"{pd.TalentValue.HP}/{pd.TalentValue.ATK}/{pd.TalentValue.DEF}/{pd.TalentValue.SPA}/{pd.TalentValue.SPD}/{pd.TalentValue.SPE}",
                    _ => "Invalid",
                };
                gw.WriteLine($"    IVs:     {talentStr}");
                gw.WriteLine($"    Ability: {new[] { "1/2", "1/2/3", "1", "2", "3" }[(int)pd.TokuseiIndex]}");
                switch (pd.WazaType)
                {
                    case WazaType.DEFAULT:
                        gw.WriteLine($"    Moves:   Random");
                        break;
                    case WazaType.MANUAL:
                        gw.WriteLine($"    Moves:   {moveNames[(int)pd.Waza1.WazaId]}/{moveNames[(int)pd.Waza2.WazaId]}/{moveNames[(int)pd.Waza3.WazaId]}/{moveNames[(int)pd.Waza4.WazaId]}");
                        break;
                }

                gw.WriteLine($"    Scale:   {new[] { "Random", "XS", "S", "M", "L", "XL", $"{pd.ScaleValue}" }[(int)pd.ScaleType]}");
                gw.WriteLine($"    GemType: {(int)pd.GemType}");


                gw.WriteLine("  Points:");
                foreach (var point in points)
                {
                    gw.WriteLine($"    - ({point.Position.X}, {point.Position.Y}, {point.Position.Z})");
                }
                gw.WriteLine("  Areas:");
                foreach (var areaName in areas)
                {
                    var areaInfo = scene.AreaInfos[areaName];
                    var loc = areaInfo.LocationNameMain;
                    (string name, int index) = placeNameMap[loc];
                    gw.WriteLine($"    - {areaName} - {loc} - {name} ({index})");
                }

                // Serialize
                if (locs.Count == 0)
                    continue;
                if (bannedIndexes.Contains(i))
                    continue;
                WriteFixedSymbol(serialized, entry, locs);
            }
        }

        var pathPickle = Path.Combine(path, "encounter_fixed_paldea.pkl");
        var ordered = serialized
                .OrderBy(z => BinaryPrimitives.ReadUInt16LittleEndian(z)) // Species
                .ThenBy(z => z[2]) // Form
                .ThenBy(z => z[3]) // Level
            ;
        File.WriteAllBytes(pathPickle, ordered.SelectMany(z => z).ToArray());
    }

    private static void WriteFixedSymbol(ICollection<byte[]> exist, FixedSymbolTable entry, IReadOnlyList<int> locs)
    {
        var enc = entry.PokeDataSymbol;
        using var ms = new MemoryStream();
        using var bw = new BinaryWriter(ms);
        
        bw.Write((ushort)enc.DevId);
        bw.Write((byte)enc.FormId);
        bw.Write((byte)enc.Level);

        bw.Write((byte)enc.TalentVNum);
        bw.Write((byte)enc.GemType);
        bw.Write((ushort)0); // reserved

        bw.Write((ushort)enc.Waza1.WazaId);
        bw.Write((ushort)enc.Waza2.WazaId);

        bw.Write((ushort)enc.Waza3.WazaId);
        bw.Write((ushort)enc.Waza4.WazaId);

        // At most 3 locations, but just use 4 bytes.
        Span<byte> temp = stackalloc byte[4];
        if (locs.Count > temp.Length)
            throw new ArgumentException("Too many locations??", nameof(locs));
        for (int i = 0; i < locs.Count; i++)
            temp[i] = (byte)locs[i];
        bw.Write(temp);

        var result = ms.ToArray();
        if (!exist.Any(x => x.SequenceEqual(result)))
            exist.Add(result);
    }

    private static void AddSlots(IDictionary<int, List<PaldeaEncounter>> encounters, int locationID, List<PaldeaEncounter> slots)
    {
        if (encounters.TryGetValue(locationID, out var existingSlots))
        {
            existingSlots.AddRange(slots);
            ConsolidateEncounters(existingSlots);
        }
        else
        {
            encounters[locationID] = slots;
        }
    }

    private static void SerializeEncounters(string binPath, Dictionary<int, List<PaldeaEncounter>> tables)
    {
        int ctr = 0;
        var result = new byte[tables.Count][];
        foreach (var (loc, slots) in tables)
        {
            using var ms = new MemoryStream();
            using var bw = new BinaryWriter(ms);
            bw.Write((ushort)loc);
            foreach (var slot in slots)
            {
                bw.Write((ushort)slot.Species);
                bw.Write((byte)slot.Form);
                bw.Write((byte)slot.Gender);
                bw.Write((byte)slot.MinLevel);
                bw.Write((byte)slot.MaxLevel);
            }
            result[ctr++] = ms.ToArray();
        }
        var mini = MiniUtil.PackMini(result, "sv");
        File.WriteAllBytes(binPath, mini);
    }

    private static void ConsolidateEncounters(List<PaldeaEncounter> encounters)
    {
        // Merge and remove future indexes if they can be combined with current
        // Pre-sort so we can just iterate.
        encounters.Sort();

        for (int i = 0; i < encounters.Count; )
        {
            var enc = encounters[i];
            int count = 1;
            for (int j = i + 1; j < encounters.Count; j++)
            {
                if (!encounters[j].IsSameSpecFormGender(enc))
                    break;
                count++;
            }

            // For i to i+count, merge within
            // Remove all indexes that have Absorb return true with a later element
            while (true)
            {
                if (TryAbsorbFromRange(encounters, i, count))
                    count--;
                else
                    break;
            }
            i += count;
        }
    }

    private static bool TryAbsorbFromRange(List<PaldeaEncounter> encounters, int start, int count)
    {
        // Need to compare all elements with each-other; return true if any absorb.
        var end = start + count - 1;
        for (int i = start; i < end; i++)
        {
            for (int j = i + 1; j <= end; j++)
            {
                if (!encounters[i].Absorb(encounters[j]))
                    continue;
                encounters.RemoveAt(j);
                return true;
            }
        }
        return false;
    }

    private static List<PaldeaEncounter> GetEncounters(List<PointData> areaEncPoints, EncountPokeDataArray pokeData, string areaName, PaldeaSceneModel scene)
    {
        var added = new HashSet<ulong>();
        var result = new List<PaldeaEncounter>();
        foreach (var ep in areaEncPoints)
        {
            foreach (var pd in pokeData.Table)
            {
                // Check area
                if (!string.IsNullOrEmpty(pd.Area) && !IsInArea(pd.Area, ep.AreaNo))
                    continue;

                // Check loc
                if (!string.IsNullOrEmpty(pd.LocationName) && !IsInArea(pd.LocationName, areaName, scene, ep))
                    continue;

                // Check biome
                if (!HasBiome(pd, (Biome)(int)ep.Biome))
                    continue;

                // check level range overlap
                if (!LevelWithinRange(pd, ep))
                    continue;

                // check area level range overlap
                // Assume flag, enable table, timetable are fine
                // Assume version is fine -- union wireless sessions can share encounters.

                // Add encount
                var e = PaldeaEncounter.GetNew(pd, ep);
                if (added.Add(e.GetHash()))
                    result.Add(e);

                if (pd.BandPoke != 0) // Add band encount
                {
                    var b = PaldeaEncounter.GetBand(pd, ep);
                    if (added.Add(b.GetHash()))
                        result.Add(b);
                }
            }
        }
        return result;
    }

    private static bool LevelWithinRange(EncountPokeData pd, PointData clamp)
    {
        return clamp.LevelRange.X <= pd.MaxLevel && pd.MinLevel <= clamp.LevelRange.Y;
    }

    private static bool HasBiome(EncountPokeData pd, Biome biome)
    {
        if (biome == pd.Biome1)
            return true;
        if (biome == pd.Biome2)
            return true;
        if (biome == pd.Biome3)
            return true;
        if (biome == pd.Biome4)
            return true;
        return false;
    }

    private static bool IsInArea(ReadOnlySpan<char> areaName, int areaNo)
    {
        int start = 0;
        for (int i = 0; i < areaName.Length; i++)
        {
            if (areaName[i] != ',')
                continue;
            var name = areaName[start..i];
            if (int.TryParse(name, out var tmp) && areaNo == tmp)
                return true;
            start = i + 1;
        }
        return int.TryParse(areaName[start..], out var x) && areaNo == x;
    }

    private static bool IsInArea(string locName, string areaName, PaldeaSceneModel scene, PointData ep)
    {
        var split = locName.Split(",");
        foreach (string a in split)
        {
            if (a == areaName)
                return true;
            
            if (!scene.IsPointContained(a, ep.Position.X, ep.Position.Y, ep.Position.Z))
                continue;

            return true;
        }
        return false;
    }

    private static List<PointData> GetAreaPoints(PaldeaSceneModel scene, string areaName, PointDataArray alEncPoints,
        PointDataArray mlEncPoints, AreaInfo areaInfo)
    {
        var areaEncPoints = new List<PointData>();
        foreach (var ep in scene.isAtlantis[areaName] ? alEncPoints.Table : mlEncPoints.Table)
        {
            var areaMin = areaInfo.MinEncLv != 0 ? areaInfo.MinEncLv : 1;
            var areaMax = areaInfo.MaxEncLv != 0 ? areaInfo.MaxEncLv : 100;
            if (!(ep.LevelRange.X <= areaMax && areaMin <= ep.LevelRange.Y))
                continue;
            if (!scene.IsPointContained(areaName, ep.Position.X, ep.Position.Y, ep.Position.Z))
                continue;
            var composite = GetCompositePoint(ep, areaMin, areaMax);
            areaEncPoints.Add(composite);
        }

        return areaEncPoints;
    }

    private static List<PokeDataSymbol> GetFixedSymbols(PaldeaSceneModel scene, PaldeaFixedSymbolModel fsym, FixedSymbolTableArray fsymTable, string areaName)
    {
        var symbols = new List<PokeDataSymbol>();
        var added = new HashSet<string>();
        // add Scarlet
        foreach (var point in fsym.scarletPoints.Concat(fsym.violetPoints))
        {
            if (!added.Contains(point.TableKey) && scene.IsPointContained(areaName, point.Position.X, point.Position.Y, point.Position.Z) )
            {
                added.Add(point.TableKey);
                symbols.Add(GetPokeDataSymbol(fsymTable, point.TableKey));
            }
        }

        return symbols;
    }

    private static PokeDataSymbol GetPokeDataSymbol(FixedSymbolTableArray fsymTable, string tableKey)
    {
        foreach (var entry in fsymTable.Table)
        {
            if (entry.TableKey == tableKey)
                return entry.PokeDataSymbol;
        }
        throw new ArgumentException($"TableKey not found ({tableKey})");
    }

    private static PointData GetCompositePoint(PointData ep, int areaMin, int areaMax)
    {
        var newX = Math.Max(ep.LevelRange.X, areaMin);
        var newY = Math.Min(ep.LevelRange.Y, areaMax);
        var composite = new PointData
        {
            Position = ep.Position,
            LevelRange = new SVector2 { X = newX, Y = newY },
            Biome = ep.Biome,
            Substance = ep.Substance,
            AreaNo = ep.AreaNo,
        };
        return composite;
    }

    private static void WriteLocation(TextWriter tw, IReadOnlyList<string> specNames,
        IReadOnlyDictionary<string, (string Name, int Index)> placeNameMap,
        string areaName, AreaInfo areaInfo, List<PaldeaEncounter> encounts)
    {
        var loc = areaInfo.LocationNameMain;
        (string name, int index) = placeNameMap[loc];
        var heading = $"{areaName} - {loc} - {name} ({index})";
        WriteEncounts(tw, specNames, heading, encounts);
    }

    private static void WriteLocEncList(TextWriter tw, IReadOnlyList<string> specNames,
        IReadOnlyDictionary<string, (string Name, int Index)> placeNameMap,
        IReadOnlyDictionary<int, List<PaldeaEncounter>> locIdEncs)
    {
        foreach (var place in placeNameMap.Keys.OrderBy(p => placeNameMap[p].Index))
        {
            (string name, int index) = placeNameMap[place];
            if (!locIdEncs.TryGetValue(index, out var encounts))
                continue;

            var heading = $"{name} ({index})";
            WriteEncounts(tw, specNames, heading, encounts);
        }
    }

    private static void WriteEncounts(TextWriter tw, IReadOnlyList<string> specNames, string heading, List<PaldeaEncounter> encounts)
    {
        tw.WriteLine("===");
        tw.WriteLine(heading);
        tw.WriteLine("===");
        foreach (var e in encounts)
            tw.WriteLine($" - {e.GetEncountString(specNames)}");
        tw.WriteLine();
    }

    public static Dictionary<string, (string Name, int Index)> GetPlaceNameMap(string[] text, AHTB ahtb)
    {
        var result = new Dictionary<string, (string Name, int Index)>();
        for (var i = 0; i < text.Length; i++)
            result[ahtb.Entries[i].Name] = (text[i], i);
        return result;
    }
}

public record PaldeaEncounter(int Species, int Form, int Sex, int MinLevel, int MaxLevel) : IComparable<PaldeaEncounter>
{
    public int MinLevel { get; private set; } = MinLevel;
    public int MaxLevel { get; private set; } = MaxLevel;

    public int Gender => Sex switch
    {
        1 => 0,
        2 => 1,
        _ => -1,
    };


    public static PaldeaEncounter GetNew(EncountPokeData pd, PointData ep)
    {
        var min = (int)Math.Max(ep.LevelRange.X, pd.MinLevel);
        var max = (int)Math.Min(ep.LevelRange.Y, pd.MaxLevel);
        return new((int)pd.DevId, pd.Form, (int)pd.Sex, min, max);
    }
    
    public static PaldeaEncounter GetBand(EncountPokeData pd, PointData ep)
    {
        var min = (int)Math.Max(ep.LevelRange.X, pd.MinLevel);
        var max = (int)Math.Min(ep.LevelRange.Y, pd.MaxLevel);
        return new((int)pd.BandPoke, pd.BandForm, (int)pd.BandSex, min, max);
    }

    public string GetEncountString(IReadOnlyList<string> specNames)
    {
        var species = specNames[Species];
        return GetString(species);
    }

    public override string ToString()
    {
        return GetString(((PKHeX.Core.Species)Species).ToString());
    }

    private string GetString(string species)
    {
        var form = Form == 0 ? "" : $"-{Form}";
        var sex = Sex == 0 ? "" : $" (sex={Sex})";
        return $"{species}{form}{sex} Lv. {MinLevel}-{MaxLevel}";
    }

    public bool Absorb(PaldeaEncounter other)
    {
        if (other.MinLevel == MinLevel && other.MaxLevel == MaxLevel)
            return true;

        if (!IsLevelRangeOverlap(other) && !other.IsLevelRangeOverlap(this))
            return false;

        MinLevel = Math.Min(MinLevel, other.MinLevel);
        MaxLevel = Math.Max(MaxLevel, other.MaxLevel);
        return true;
    }

    public bool IsSameSpecFormGender(PaldeaEncounter other)
    {
        if (Species != other.Species || Form != other.Form)
            return false;
        if (Sex != other.Sex)
            return false;
        return true;
    }

    private bool IsLevelRangeOverlap(PaldeaEncounter other)
    {
        // If our level range overlaps with the other (with +/- 1 tolerance), return true.
        return MaxLevel + 1 >= other.MinLevel && MinLevel + 1 <= other.MaxLevel;
    }

    public int CompareTo(PaldeaEncounter? other)
    {
        if (ReferenceEquals(this, other)) return 0;
        if (ReferenceEquals(null, other)) return 1;
        int speciesComparison = Species.CompareTo(other.Species);
        if (speciesComparison != 0) return speciesComparison;
        int formComparison = Form.CompareTo(other.Form);
        if (formComparison != 0) return formComparison;
        int sexComparison = Sex.CompareTo(other.Sex);
        if (sexComparison != 0) return sexComparison;
        int minLevelComparison = MinLevel.CompareTo(other.MinLevel);
        if (minLevelComparison != 0) return minLevelComparison;
        return MaxLevel.CompareTo(other.MaxLevel);
    }

    public ulong GetHash()
    {
        ulong result = (ulong)Species;
        result = (result << 16) | (byte)Form;
        result = (result << 8) | (byte)Sex;
        result = (result << 8) | (byte)MinLevel;
        result = (result << 8) | (byte)MaxLevel;
        return result;
    }
}

public class PaldeaFixedSymbolPoint
{
    public string TableKey;
    public Vector3f Position;

    public PaldeaFixedSymbolPoint(string key, Vector3f pos)
    {
        TableKey = key;
        Position = new Vector3f
        {
            X = pos.X,
            Y = pos.Y,
            Z = pos.Z,
        };
    }
}

public class PaldeaSceneModel
{
    public readonly List<string> areaNames;
    public readonly Dictionary<string, AreaInfo> AreaInfos;
    public readonly Dictionary<string, HavokCollision.AABBTree> areaColTrees;
    public readonly Dictionary<string, BoxCollision9> areaColBoxes;
    public readonly Dictionary<string, bool> isAtlantis;

    public PaldeaSceneModel(IFileInternal ROM, PaldeaFieldModel field)
    {
        // NOTE: Safe to only use _0 because _1 is identical.
        var area_management = FlatBufferConverter.DeserializeFrom<TrinitySceneObjectTemplateSV>(ROM.GetPackedFile("world/scene/parts/field/field_system/area_management_/area_management_0.trscn"));
        Debug.Assert(area_management.ObjectTemplateName == "area_management");

        // NOTE: Safe to only use _0 because _1 is identical.
        var a_w23_field_area_col = FlatBufferConverter.DeserializeFrom<TrinitySceneObjectTemplateSV>(ROM.GetPackedFile("world/scene/parts/field/room/a_w23_field/a_w23_field_area_col_/a_w23_field_area_col_0.trscn"));
        Debug.Assert(a_w23_field_area_col.ObjectTemplateName == "a_w23_field_area_col");

        areaNames = new List<string>();
        AreaInfos = new Dictionary<string, AreaInfo>();
        areaColTrees = new Dictionary<string, HavokCollision.AABBTree>();
        areaColBoxes = new Dictionary<string, BoxCollision9>();
        isAtlantis = new Dictionary<string, bool>();

        void AddSceneObject(string name, TrinitySceneObjectSV sceneObject, TrinityCollisionComponent1SV collisionComponent)
        {
            areaNames.Add(name);
            AreaInfos[name] = field.FindAreaInfo(name);

            Debug.Assert(collisionComponent.CollisionShape.Discriminator is 2 or 4);

            if (collisionComponent.CollisionShape.TryGet(out TrinityCollisionShapeBoxSV? box))
            {
                // Box collision, obj.ObjectPosition.Field_02 is pos, box.Field_01 is size of box
                areaColBoxes[name] = new BoxCollision9
                {
                    Position = sceneObject.ObjectPosition.Field_02,
                    Size = box.Field_01,
                };
            }
            else if (collisionComponent.CollisionShape.TryGet(out TrinityCollisionShapeHavokSV? havok))
            {
                var havokData = ROM.GetPackedFile(havok.TrcolFilePath);
                areaColTrees[name] = HavokCollision.ParseAABBTree(havokData);
            }
        }

        foreach (var obj in area_management.Objects.Concat(a_w23_field_area_col.Objects))
        {
            var isAtlantisObj = a_w23_field_area_col.Objects.Contains(obj);
            if (!(obj.SubObjects.Length > 0 && obj.SubObjects[0].Type == "trinity_CollisionComponent"))
                continue;
            var collisionComponent = FlatBufferConverter.DeserializeFrom<TrinityCollisionComponentSV>(obj.SubObjects[0].Data).Component.Item1;

            switch (obj.Type)
            {
                case "trinity_ObjectTemplate":
                {
                    var sObj = FlatBufferConverter.DeserializeFrom<TrinitySceneObjectTemplateDataSV>(obj.Data);
                    if (sObj.Type != "trinity_SceneObject")
                        continue;
                    var sceneObject = FlatBufferConverter.DeserializeFrom<TrinitySceneObjectSV>(sObj.Data);
                    Debug.Assert(sceneObject.ObjectName == sObj.ObjectTemplateExtra);

                    AddSceneObject(sObj.ObjectTemplateName, sceneObject, collisionComponent);
                    isAtlantis[sObj.ObjectTemplateName] = isAtlantisObj;
                    break;
                }
                case "trinity_SceneObject":
                {
                    var sceneObject = FlatBufferConverter.DeserializeFrom<TrinitySceneObjectSV>(obj.Data);
                    AddSceneObject(sceneObject.ObjectName, sceneObject, collisionComponent);
                    isAtlantis[sceneObject.ObjectName] = isAtlantisObj;
                    break;
                }
            }
        }
    }

    public bool IsPointContained(string areaName, float x, float y, float z)
    {
        if (areaColTrees.TryGetValue(areaName, out var tree))
            return tree.ContainsPoint(x, y, z);
        if (areaColBoxes.TryGetValue(areaName, out var box))
            return box.ContainsPoint(x, y, z);
        return false;
    }
}

public class PaldeaFieldModel
{
    private readonly FieldMainArea[] mainAreas;
    private readonly FieldSubArea[] subAreas;
    private readonly FieldInsideArea[] insideAreas;
    private readonly FieldDungeonArea[] dungeonAreas;
    private readonly FieldLocation[] fieldLocations;

    public PaldeaFieldModel(IFileInternal ROM)
    {
        mainAreas = FlatBufferConverter.DeserializeFrom<FieldMainAreaArray>(ROM.GetPackedFile("world/data/field/area/field_main_area/field_main_area_array.bin")).Table;
        subAreas = FlatBufferConverter.DeserializeFrom<FieldSubAreaArray>(ROM.GetPackedFile("world/data/field/area/field_sub_area/field_sub_area_array.bin")).Table;
        insideAreas = FlatBufferConverter.DeserializeFrom<FieldInsideAreaArray>(ROM.GetPackedFile("world/data/field/area/field_inside_area/field_inside_area_array.bin")).Table;
        dungeonAreas = FlatBufferConverter.DeserializeFrom<FieldDungeonAreaArray>(ROM.GetPackedFile("world/data/field/area/field_dungeon_area/field_dungeon_area_array.bin")).Table;
        fieldLocations = FlatBufferConverter.DeserializeFrom<FieldLocationArray>(ROM.GetPackedFile("world/data/field/area/field_location/field_location_array.bin")).Table;
    }

    public AreaInfo FindAreaInfo(string name)
    {
        foreach (var area in mainAreas)
        {
            if (area.Name == name)
                return area.AreaInfo;
        }
        foreach (var area in subAreas)
        {
            if (area.Name == name)
                return area.AreaInfo;
        }
        foreach (var area in insideAreas)
        {
            if (area.Name == name)
                return area.AreaInfo;
        }
        foreach (var area in dungeonAreas)
        {
            if (area.Name == name)
                return area.AreaInfo;
        }
        foreach (var area in fieldLocations)
        {
            if (area.Name == name)
                return area.AreaInfo;
        }
        throw new ArgumentException($"Unknown area {name}");
    }
}

public class PaldeaFixedSymbolModel
{
    public readonly List<PaldeaFixedSymbolPoint> scarletPoints;
    public readonly List<PaldeaFixedSymbolPoint> violetPoints;

    public PaldeaFixedSymbolModel(IFileInternal ROM)
    {
        scarletPoints = new List<PaldeaFixedSymbolPoint>();
        violetPoints = new List<PaldeaFixedSymbolPoint>();

        var p0Data = ROM.GetPackedFile("world/scene/parts/field/streaming_event/world_fixed_placement_symbol_/world_fixed_placement_symbol_0.trscn");
        var p1Data = ROM.GetPackedFile("world/scene/parts/field/streaming_event/world_fixed_placement_symbol_/world_fixed_placement_symbol_1.trscn");

        var p0 = FlatBufferConverter.DeserializeFrom<TrinitySceneObjectTemplateSV>(p0Data);
        var p1 = FlatBufferConverter.DeserializeFrom<TrinitySceneObjectTemplateSV>(p1Data);

        scarletPoints.AddRange(GetObjectTemplateSymbolPoints(p0));
        violetPoints.AddRange(GetObjectTemplateSymbolPoints(p1));
    }

    private IEnumerable<PaldeaFixedSymbolPoint> GetObjectTemplateSymbolPoints(TrinitySceneObjectTemplateSV template)
    {
        foreach (var obj in template.Objects)
        {
            switch (obj.Type)
            {
                case "trinity_ObjectTemplate":
                    {
                        var sObj = FlatBufferConverter.DeserializeFrom<TrinitySceneObjectTemplateDataSV>(obj.Data);
                        if (sObj.Type != "trinity_ScenePoint")
                            continue;
                        var scenePoint = FlatBufferConverter.DeserializeFrom<TrinityScenePointSV>(sObj.Data);

                        foreach (var f in GetScenePointSymbolPoints(scenePoint, obj.SubObjects))
                            yield return f;
                        break;
                    }
                case "trinity_ScenePoint":
                    {
                        var scenePoint = FlatBufferConverter.DeserializeFrom<TrinityScenePointSV>(obj.Data);

                        foreach (var f in GetScenePointSymbolPoints(scenePoint, obj.SubObjects))
                            yield return f;
                        break;
                    }
            }
        }
    }

    private IEnumerable<PaldeaFixedSymbolPoint> GetScenePointSymbolPoints(TrinityScenePointSV scenePoint, TrinitySceneObjectTemplateEntrySV[] subObjects)
    {
        // Handle SubObjects
        for (var i = 0; i < subObjects.Length; i++)
        {
            var sobj = subObjects[i];
            switch (sobj.Type)
            {
                case "trinity_PropertySheet":
                    {
                        var propSheet = FlatBufferConverter.DeserializeFrom<TrinityPropertySheetSV>(sobj.Data);
                        if (propSheet.Name == "fixed_symbol_point")
                        {
                            var tableKey = GetTableKey(propSheet);
                            if (!string.IsNullOrEmpty(tableKey))
                            {
                                yield return new PaldeaFixedSymbolPoint(tableKey, scenePoint.Position);
                            }
                        }
                        break;
                    }
                case "trinity_ScenePoint":
                    {
                        var subScenePoint = FlatBufferConverter.DeserializeFrom<TrinityScenePointSV>(sobj.Data);
                        foreach (var f in GetScenePointSymbolPoints(subScenePoint, sobj.SubObjects))
                            yield return f;
                        break;
                    }
                default:
                    throw new ArgumentException($"Unknown SubObject {sobj.Type}");
            }
        }
    }

    public static string GetTableKey(TrinityPropertySheetSV propSheet)
    {
        if (propSheet.Name != "fixed_symbol_point")
            throw new ArgumentException($"Invalid PropertySheet {propSheet.Name}");

        if (propSheet.Properties[0].Fields[1].Name != "tableKey")
            throw new ArgumentException("Invalid PropertySheet field layout");

        if (!propSheet.Properties[0].Fields[1].Data.TryGet(out TrinityPropertySheetFieldStringValueSV? sv))
            throw new ArgumentException("Could not get PropertySheet Table Key");

        return sv.Value;
    }
}
