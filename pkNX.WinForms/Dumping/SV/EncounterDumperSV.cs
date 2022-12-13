using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using pkNX.Containers;
using pkNX.Structures.FlatBuffers;

namespace pkNX.WinForms;

public class EncounterDumperSV
{
    private readonly IFileInternal ROM;
    private const float tolX = 30f;
    private const float tolY = 30f;
    private const float tolZ = 30f;

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
        var csym = new PaldeaCoinSymbolModel(ROM);
        var mlEncPoints = FlatBufferConverter.DeserializeFrom<PointDataArray>(ROM.GetPackedFile("world/data/encount/point_data/point_data/encount_data_100000.bin"));
        var alEncPoints = FlatBufferConverter.DeserializeFrom<PointDataArray>(ROM.GetPackedFile("world/data/encount/point_data/point_data/encount_data_atlantis.bin"));
        var pokeData = FlatBufferConverter.DeserializeFrom<EncountPokeDataArray>(ROM.GetPackedFile("world/data/encount/pokedata/pokedata/pokedata_array.bin"));

        var db = new LocationDatabase();

        // Overall Logic Flow:
        // 1 - At every point, spawn everything that can exist at that point.
        // 2 - At each area, absorb native points into local points.
        // 3 - At each area, absorb crossover points into crossover points.
        // 4 - Consolidate the encounters from both point lists.
        // Just compute everything (big memory!) then crunch it all down.

        // Points can be used by multiple areas as crossover sources. Need to be able to "belong" to multiple areas, and indicate their parent area.
        var pointMain = ReformatPoints(mlEncPoints);
        var pointAtlantis = ReformatPoints(alEncPoints);

        // Fill the point lists for each area, then spawn everything into those points.
        foreach (var areaName in scene.areaNames)
        {
            var areaInfo = scene.AreaInfos[areaName];

            // Determine potential spawners
            if (!scene.TryGetContainsCheck(areaName, out var collider))
            {
                Console.WriteLine($"No collider for {areaName}");
                continue;
            }

            // Locations that do not spawn encounters can still have crossovers bleed into them.
            // We'll have empty local encounter lists for them.
            var name = areaInfo.LocationNameMain;
            if (string.IsNullOrEmpty(name))
                continue;
            var storage = db.Get(placeNameMap[name].Index, areaName, areaInfo);
            if (areaInfo.Tag is AreaTag.NG_Encount or AreaTag.NG_All)
                continue;

            var points = scene.isAtlantis[areaName] ? pointAtlantis : pointMain;
            storage.LoadPoints(points, collider, areaInfo.ActualMinLevel, areaInfo.ActualMaxLevel);
            storage.GetEncounters(pokeData, scene);
        }

        // For each area, we need to peek at the other areas to see if they have any crossover points.
        // For each of those crossover points, we need to see if they are in the current area's collider.
        // If they are, we need to add them to the current area's list of crossover points.
        foreach (var areaName in scene.areaNames)
        {
            // Same sanity checking as above iteration.
            var areaInfo = scene.AreaInfos[areaName];

            // Determine potential spawners
            if (!scene.TryGetContainsCheck(areaName, out var collider))
            {
                Console.WriteLine($"No collider for {areaName}");
                continue;
            }

            var name = areaInfo.LocationNameMain;
            if (string.IsNullOrEmpty(name))
                continue;
            //if (areaInfo.Tag is AreaTag.NG_Encount or AreaTag.NG_All)
            //    continue;

            var storage = db.Get(placeNameMap[name].Index, areaName, areaInfo);
            if (storage.Location == 124) // Area Zero
                continue;

            // Here's where the fun begins. Iterate over areas inside this loop so we can look for all possible adjacent areas.
            foreach (var otherName in scene.areaNames)
            {
                // Skip self
                if (otherName == areaName)
                    continue;
                // Skip areas that don't have a location name
                var otherAreaInfo = scene.AreaInfos[otherName];
                var otherNameMain = otherAreaInfo.LocationNameMain;
                if (string.IsNullOrEmpty(otherNameMain))
                    continue;

                // Skip areas that don't have a collider
                if (!scene.TryGetContainsCheck(otherName, out _))
                    continue;

                // Iterate over all crossover points in the other area.
                var cross = db.Get(placeNameMap[otherNameMain].Index, otherName, otherAreaInfo);
                if (cross.Location == 124) // Area Zero
                    continue;
                foreach (var point in cross.Local)
                {
                    // If the crossover point is close enough to the current area's collider, add it to the current area's list of crossover points.
                    if (collider.ContainsPoint(point.X, point.Y, point.Z, tolX, tolY, tolZ))
                        storage.Nearby.Add(point);
                }
            }
        }

        // Each area and their local / crossover points have been aggregated.
        // Integrate the points' slots into a single list, and consolidate entries with same level ranges to as few objects as possible.
        foreach (var storage in db.Locations.Values)
        {
            // Consolidate encounters
            storage.Integrate();
            storage.Consolidate();
        }

        // Output to stream if available.
        if (writeText)
        {
            using var sw = File.CreateText(Path.Combine(path, "titan_enc.txt"));
            foreach (var s in db.Locations.Values)
                WriteLocation(sw, specNames, placeNameMap, s.AreaName, s.AreaInfo, s.Slots);
            using var tw = File.CreateText(Path.Combine(path, "titan_loc_enc.txt"));
            WriteLocEncList(tw, specNames, placeNameMap, db);

            using var pw = File.CreateText(Path.Combine(path, "titan_loc_point.txt"));
            WriteLocPointList(pw, placeNameMap, db);
        }
        if (writePickle)
        {
            string binPath = Path.Combine(path, "encounter_wild_paldea.pkl");
            SerializeEncounters(binPath, db);
        }

        // Fixed symbols
        List<byte[]> serialized = new();
        int[] bannedIndexes =
        {
            31, // Lighthouse Wingull
        };
        var fsymData = FlatBufferConverter.DeserializeFrom<FixedSymbolTableArray>(ROM.GetPackedFile("world/data/field/fixed_symbol/fixed_symbol_table/fixed_symbol_table_array.bin"));
        var eventBattle = FlatBufferConverter.DeserializeFrom<EventBattlePokemonArray>(ROM.GetPackedFile("world/data/battle/eventBattlePokemon/eventBattlePokemon_array.bin"));
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
                        gw.WriteLine("    Moves:   Random");
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

                areas.Clear();
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

                    if (!scene.TryGetContainsCheck(areaName, out var collider))
                        continue;

                    if (points.Any(p => collider.ContainsPoint(p.Position.X, p.Position.Y, p.Position.Z, tolX, tolY, tolZ)))
                        areas.Add(areaName);
                }

                locs = areas.Select(a => placeNameMap[scene.AreaInfos[a].LocationNameMain].Index).Distinct().ToList();
                WriteFixedSymbol(serialized, entry, locs);
            }
        }

        using var cw = File.CreateText(Path.Combine(path, "titan_coin_symbol.txt"));
        foreach (var entry in csym.Points)
        {
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

                if (scene.IsPointContained(areaName, entry.Position.X, entry.Position.Y, entry.Position.Z))
                    areas.Add(areaName);
            }

            var locs = areas.Select(a => placeNameMap[scene.AreaInfos[a].LocationNameMain].Index).Distinct().ToList();

            cw.WriteLine("===");
            cw.WriteLine(entry.Name);
            cw.WriteLine("===");
            cw.WriteLine($"  First Num:   {entry.FirstNum}");
            cw.WriteLine($"  Coordinates: ({entry.Position.X}, {entry.Position.Y}, {entry.Position.Z})");

            if (entry.IsBox)
            {
                cw.WriteLine($"  Box Label:   {entry.BoxLabel}");
                cw.WriteLine("  PokeData:");
                var pd = Array.Find(eventBattle.Table, e => e.Label == entry.BoxLabel)!.PokeData;

                cw.WriteLine($"    Species: {specNames[(int)pd.DevId]}");
                cw.WriteLine($"    Form:    {pd.FormId}");
                cw.WriteLine($"    Level:   {pd.Level}");
                cw.WriteLine($"    Sex:     {new[] { "Random", "Male", "Female" }[(int)pd.Sex]}");
                cw.WriteLine($"    Shiny:   {new[] { "Random", "Never", "Always" }[(int)pd.RareType]}");

                var talentStr = pd.TalentType switch
                {
                    TalentType.RANDOM => "Random",
                    TalentType.V_NUM => $"{pd.TalentVnum} Perfect",
                    TalentType.VALUE => $"{pd.TalentValue.HP}/{pd.TalentValue.ATK}/{pd.TalentValue.DEF}/{pd.TalentValue.SPA}/{pd.TalentValue.SPD}/{pd.TalentValue.SPE}",
                    _ => "Invalid",
                };
                cw.WriteLine($"    IVs:     {talentStr}");
                cw.WriteLine($"    Ability: {new[] { "1/2", "1/2/3", "1", "2", "3" }[(int)pd.Tokusei]}");
                switch (pd.WazaType)
                {
                    case WazaType.DEFAULT:
                        cw.WriteLine($"    Moves:   Random");
                        break;
                    case WazaType.MANUAL:
                        cw.WriteLine($"    Moves:   {moveNames[(int)pd.Waza1.WazaId]}/{moveNames[(int)pd.Waza2.WazaId]}/{moveNames[(int)pd.Waza3.WazaId]}/{moveNames[(int)pd.Waza4.WazaId]}");
                        break;
                }

                cw.WriteLine($"    Scale:   {new[] { "Random", "XS", "S", "M", "L", "XL", $"{pd.ScaleValue}" }[(int)pd.ScaleType]}");
                cw.WriteLine($"    GemType: {(int)pd.GemType}");
            }

            cw.WriteLine("  Areas:");
            foreach (var areaName in areas)
            {
                var areaInfo = scene.AreaInfos[areaName];
                var loc = areaInfo.LocationNameMain;
                (string name, int index) = placeNameMap[loc];
                cw.WriteLine($"    - {areaName} - {loc} - {name} ({index})");
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

    private static LocationPointDetail[] ReformatPoints(PointDataArray all)
    {
        var arr = all.Table;
        var result = new LocationPointDetail[arr.Length];
        for (int i = 0; i < arr.Length; i++)
            result[i] = new LocationPointDetail(arr[i]);
        return result;
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
        bw.Write((byte)(enc.Sex - 1));
        bw.Write((byte)0); // reserved

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

    private static void SerializeEncounters(string binPath, LocationDatabase db)
    {
        var tables = db.Locations.Values;
        int ctr = 0;
        var result = new byte[tables.Count + tables.Sum(z => z.SlotsCrossover.Count)][];
        foreach (var x in tables)
        {
            result[ctr++] = SerializeLocationSet((ushort)x.Location, 0, x.Slots);
            foreach (var sub in x.SlotsCrossover)
                result[ctr++] = SerializeLocationSet((ushort)x.Location, (ushort)sub.Key, sub.Value.Slots);
        }
        var mini = MiniUtil.PackMini(result, "sv");
        File.WriteAllBytes(binPath, mini);
    }

    private static byte[] SerializeLocationSet(ushort loc, ushort crossover, IReadOnlyList<PaldeaEncounter> slots)
    {
        using var ms = new MemoryStream();
        using var bw = new BinaryWriter(ms);
        bw.Write(loc);
        bw.Write(crossover);
        foreach (var slot in slots)
        {
            bw.Write((ushort)slot.Species);
            bw.Write((byte)slot.Form);
            bw.Write((byte)slot.Gender);

            bw.Write((byte)slot.MinLevel);
            bw.Write((byte)slot.MaxLevel);
            bw.Write((byte)slot.Time);
            bw.Write((byte)0);
        }
        return ms.ToArray();
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
        LocationDatabase db)
    {
        foreach (var place in placeNameMap.Keys.OrderBy(p => placeNameMap[p].Index))
        {
            (string name, int index) = placeNameMap[place];
            if (!db.Locations.TryGetValue(index, out var encounts))
                continue;

            var heading = $"{name} ({index})";
            WriteEncounts(tw, specNames, heading, encounts.Slots);
        }
    }

    private static void WriteLocPointList(TextWriter tw,
        IReadOnlyDictionary<string, (string Name, int Index)> placeNameMap,
        LocationDatabase db)
    {
        foreach (var place in placeNameMap.Keys.OrderBy(p => placeNameMap[p].Index))
        {
            (string name, int index) = placeNameMap[place];
            if (!db.Locations.TryGetValue(index, out var loc))
                continue;

            var heading = $"{name} ({index})";
            WriteBiomes(tw, heading, loc.Local);
        }

        foreach (var place in placeNameMap.Keys.OrderBy(p => placeNameMap[p].Index))
        {
            (string name, int index) = placeNameMap[place];
            if (!db.Locations.TryGetValue(index, out var loc))
                continue;

            var heading = $"{name} ({index})";
            WritePoints(tw, heading, loc.Local);
        }
    }

    private static void WriteBiomes(TextWriter tw, string heading, List<LocationPointDetail> points)
    {
        var biomes = points.Select(z => z.Point.Biome).Distinct().Select(z => z.ToString()).OrderBy(z => z);
        var btext = string.Join(',', biomes);
        tw.WriteLine($"{heading}\t{btext}");
    }

    private static void WritePoints(TextWriter tw, string heading, List<LocationPointDetail> points)
    {
        tw.WriteLine("===");
        tw.WriteLine(heading);
        tw.WriteLine("===");
        foreach (var e in points)
            tw.WriteLine($" - {e.GetString()}");
        tw.WriteLine();
    }

    private static void WriteEncounts(TextWriter tw, IReadOnlyList<string> specNames, string heading, IEnumerable<PaldeaEncounter> encounts)
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
