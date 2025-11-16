using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using pkNX.Containers;
using pkNX.Structures.FlatBuffers.SV;

namespace pkNX.Structures.FlatBuffers;

public static class FixedSymbolDumper9
{
    private record struct AppearTuple(string PlaceName, string AreaName, int Adjust, PackedVec3f Point);

    public static void Dump(IFileInternal rom, EncounterDumpConfigSV config, PaldeaSceneModel scene)
    {
        List<byte[]> serialized = [];
        var fsym = new PaldeaFixedSymbolModel(rom);
        var array = rom.GetPackedFile("world/data/field/fixed_symbol/fixed_symbol_table/fixed_symbol_table_array.bin");
        var fsymData = FlatBufferConverter.DeserializeFrom<FixedSymbolTableArray>(array);
        foreach (var (game, gamePoints) in new[] { ("sl", points: fsym.PointsScarlet), ("vl", points: fsym.PointsViolet) })
        {
            using var gw = File.CreateText(Path.Combine(config.Path, $"titan_fixed_{game}.txt"));
            foreach (var fieldIndex in EncounterDumperSV.AllMaps)
            {
                var areaNames = scene.AreaNames[(int)fieldIndex];
                var areas = scene.AreaInfos[(int)fieldIndex];
                var atlantis = scene.PaldeaType[(int)fieldIndex];
                var allPoints = gamePoints[(int)fieldIndex];

                if (fieldIndex is PaldeaFieldIndex.Kitakami)
                {
                    areas = areas.Where(z => z.Value.AdjustEncLv != 0)
                        .ToDictionary(z => z.Key, z => z.Value);
                    areaNames = [.. areas.Keys];
                }

                for (var i = 0; i < fsymData.Table.Count; i++)
                {
                    var entry = fsymData.Table[i];
                    var tableKey = entry.TableKey;
                    if (tableKey.StartsWith("su2_w23d10_"))
                        continue; // handle later, manually
                    var points = allPoints.FindAll(p => p.TableKey == tableKey);
                    if (points.Count == 0)
                        continue;

                    var appearAreas = new List<AppearTuple>();

                    if (!FindArea(AreaType.Cave))
                        FindArea(AreaType.Default);

                    bool FindArea(AreaType type)
                    {
                        for (var x = areaNames.Count - 1; x >= 0; x--)
                        {
                            var areaName = areaNames[x];
                            if (atlantis[areaName] != PaldeaPointPivot.Overworld)
                                continue;

                            var areaInfo = areas[areaName];
                            if (type != AreaType.Default && areaInfo.Type != type)
                                continue;
                            if (areaInfo.Tag is AreaTag.NG_Encount)
                                continue;
                            if (!scene.TryGetContainsCheck(fieldIndex, areaName, out var collider))
                                continue;
                            for (int p = 0; p < points.Count; p++)
                            {
                                var point = points[p];
                                var pt = point.Position;
                                if (!collider.ContainsPoint(pt.X, pt.Y, pt.Z))
                                    continue;
                                var tmp = areaName;
                                if (!EncounterDumperSV.TryGetPlaceName(ref tmp, areaInfo, pt, config.PlaceNameMap, areas, scene, fieldIndex, out var placeName))
                                    continue;
                                if (!appearAreas.Exists(z => z.Point == pt && z.AreaName == tmp))
                                    appearAreas.Add(new(placeName, tmp, areaInfo.AdjustEncLv, pt));
                                points.RemoveAt(p);
                                p--;
                            }

                            if (points.Count == 0)
                                return true;
                        }
                        return false;
                    }

                    WriteFixedSpawn(config, gw, tableKey, i, entry, appearAreas);

                    // Serialize
                    if (entry.PokeAI.ActionId.IsAvoidantAction())
                        continue; // not actually encounter-able, they're just for spectacle
                    // var locs = appearAreas.Select(a => placeNameMap[a.PlaceName].Index);
                    if (appearAreas.Count == 0)
                        continue;

                    // If not stationary, allow some tolerance.
                    var wanderAreas = new List<AppearTuple>(0);
                    if (!entry.PokeAI.ActionId.IsStationary())
                    {
                        var allInfos = scene.AreaInfos[(int)fieldIndex];
                        var allNames = scene.AreaNames[(int)fieldIndex];
                        for (var a = 0; a < allNames.Count; a++)
                        {
                            var areaName = allNames[a];
                            if (atlantis[areaName] != PaldeaPointPivot.Overworld)
                                continue;

                            var areaInfo = allInfos[areaName];
                            if (!scene.TryGetContainsCheck(fieldIndex, areaName, out var collider))
                                continue;
                            if (areaInfo.Tag is AreaTag.NG_Encount)
                                continue;
                            if (!TryGetBleedArea(collider, appearAreas, scene, fieldIndex, out var bledFrom))
                                continue; // can't bleed into this area

                            // Bleeding from zones should start from the sub-zone, so we don't need to check other sub-zones.
                            if (!EncounterDumperSV.TryGetPlaceName(ref areaName, areaInfo, collider, config.PlaceNameMap, allInfos, scene, fieldIndex, out var placeName))
                                continue;
                            wanderAreas.Add(bledFrom with { PlaceName = placeName, AreaName = allNames[a] });
                        }
                    }

                    foreach (var area in appearAreas.GroupBy(z => z.Adjust))
                    {
                        var adjust = area.Key;
                        var wander = wanderAreas.Where(z => z.Adjust == adjust);
                        var allLocations = area.Concat(wander);
                        var locationIDs = allLocations.Select(z => config.PlaceNameMap[z.PlaceName].Index).Distinct().ToList();
                        if (fieldIndex == PaldeaFieldIndex.Paldea && entry.PokeAI.ActionId == PokemonActionID.FS_POP_AREA22_DRAGONITE) // Flies around not using tolerance.
                            locationIDs.Add(46); // North Province (Area One)
                        locationIDs.Sort();

                        WriteFixedSymbol(serialized, entry, locationIDs);
                        if (adjust != 0)
                            WriteFixedSymbol(serialized, entry, locationIDs, adjust);
                    }
                }
            }

            var underDepths = Array.FindIndex(fsymData.Table.ToArray(), x => x.TableKey == "su2_w23d10_01");
            for (int i = underDepths; i < underDepths + 23; i++)
            {
                var entry = fsymData.Table[i];
                var tableKey = entry.TableKey;
                var appearAreas = new List<AppearTuple> { new("PLACENAME_a_w23_d10_01", "a_w23_d10_subarea", 0, new()) };
                WriteFixedSpawn(config, gw, tableKey, i, entry, appearAreas);
                WriteFixedSymbol(serialized, entry, [196]);
            }
        }

        var pathPickle = Path.Combine(config.Path, "encounter_fixed_paldea.pkl");
        var ordered = serialized
                .OrderBy(z => BinaryPrimitives.ReadUInt16LittleEndian(z)) // Species
                .ThenBy(z => z[2]) // Form
                .ThenBy(z => z[3]) // Level
            ;
        File.WriteAllBytes(pathPickle, ordered.SelectMany(z => z).ToArray());
    }

    private static void WriteFixedSpawn(EncounterDumpConfigSV config, StreamWriter gw, string tableKey, int i, FixedSymbolTable entry, List<AppearTuple> appearAreas)
    {
        gw.WriteLine("===");
        gw.WriteLine($"{tableKey} - {i}");
        gw.WriteLine("===");
        gw.WriteLine("  PokeData:");
        var pd = entry.Symbol;
        gw.WriteLine($"    Species: {config.SpecNamesInternal[(int)pd.DevId]}");
        gw.WriteLine($"    Form:    {pd.FormId}");
        gw.Write($"    Level:   {pd.Level}");
        foreach (var adj in appearAreas.Select(a => a.Adjust).Where(lv => lv != 0).Distinct().Order())
        {
            gw.Write($", {pd.Level + adj}");
        }
        gw.WriteLine();
        gw.WriteLine($"    Sex:     {pd.Sex.Humanize()}");
        gw.WriteLine($"    Shiny:   {pd.RareType.Humanize()}");

        var talentStr = pd.TalentType switch
        {
            TalentType.RANDOM => "Random",
            TalentType.V_NUM => $"{pd.TalentVNum} Perfect",
            TalentType.VALUE => $"{pd.TalentValue.HP}/{pd.TalentValue.ATK}/{pd.TalentValue.DEF}/{pd.TalentValue.SPA}/{pd.TalentValue.SPD}/{pd.TalentValue.SPE}",
            _ => "Invalid",
        };
        gw.WriteLine($"    IVs:     {talentStr}");
        gw.WriteLine($"    Ability: {pd.TokuseiIndex.Humanize()}");
        switch (pd.WazaType)
        {
            case WazaType.DEFAULT:
                gw.WriteLine("    Moves:   Random");
                break;
            case WazaType.MANUAL:
                gw.WriteLine($"    Moves:   {config[pd.Waza1]}/{config[pd.Waza2]}/{config[pd.Waza3]}/{config[pd.Waza4]}");
                break;
        }

        gw.WriteLine($"    Scale:   {pd.ScaleType.Humanize(pd.ScaleValue)}");
        gw.WriteLine($"    GemType: {(int)pd.GemType}");

        gw.WriteLine($"    Probability: {entry.PokeGeneration.RepopProbability}");
        gw.WriteLine($"    Pattern:     {entry.PokeGeneration.GenerationPattern}");
        gw.WriteLine($"    Scenario:    {entry.PokeGeneration.RequireScenarioId}");
        gw.WriteLine($"    Action:      {entry.PokeAI.ActionId}");
        gw.WriteLine($"    Trigger:     {entry.PokeAI.TriggerActionId}");

        gw.WriteLine("  Areas:");
        foreach (var area in appearAreas)
        {
            var loc = area.PlaceName;
            var (name, index) = config.PlaceNameMap[loc];
            gw.WriteLine($"    - {area.PlaceName} - {loc} - {name} ({index})");
            var point = area.Point;
            gw.WriteLine($"    @ ({point.X}, {point.Y}, {point.Z})");
        }
    }

    private static bool TryGetBleedArea(IContainsV3f collider, List<AppearTuple> appearAreas, PaldeaSceneModel scene, PaldeaFieldIndex fieldIndex, out AppearTuple bledFrom)
    {
        foreach (var a in appearAreas)
        {
            var p = a.Point;
            if (!EncounterDumperSV.IsContainedBy(collider, p))
                continue;
            // Get the original area it bled from.
            bledFrom = appearAreas.First(z =>
                scene.TryGetContainsCheck(fieldIndex, z.AreaName, out var c) &&
                c.ContainsPoint(p.X, p.Y, p.Z));
            return true;
        }
        bledFrom = default;
        return false;
    }

    private static void WriteFixedSymbol(ICollection<byte[]> exist, FixedSymbolTable entry, IReadOnlyList<int> locs, int adjustLevel = 0)
    {
        var enc = entry.Symbol;
        using var ms = new MemoryStream();
        using var bw = new BinaryWriter(ms);

        ushort species = SpeciesConverterSV.GetNational9((ushort)enc.DevId);
        byte form = (byte)enc.FormId;
        if (species == (int)Species.Minior)
        {
            // Not form random -- keep original form ID.
            // Encounter Slots themselves are form random, but the fixed symbols aren't.
            form += 7; // Out of battle form ID (without the rocky shields up)
        }

        bw.Write(species);
        bw.Write(form);
        bw.Write((byte)(enc.Level + adjustLevel));

        bw.Write(enc.TalentType == TalentType.RANDOM ? (byte)0 : (byte)enc.TalentVNum);
        bw.Write((byte)enc.GemType);
        bw.Write((byte)(enc.Sex - 1));
        bw.Write((byte)(enc.TokuseiIndex switch
        {
            TokuseiType.RANDOM_12 => PKHeX.Core.AbilityPermission.Any12,
            TokuseiType.RANDOM_123 => PKHeX.Core.AbilityPermission.Any12H,
            TokuseiType.SET_1 => PKHeX.Core.AbilityPermission.OnlyFirst,
            TokuseiType.SET_2 => PKHeX.Core.AbilityPermission.OnlySecond,
            TokuseiType.SET_3 => PKHeX.Core.AbilityPermission.OnlyHidden,
            _ => throw new ArgumentOutOfRangeException(nameof(TokuseiType), enc.TokuseiIndex, null),
        }));

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
}
