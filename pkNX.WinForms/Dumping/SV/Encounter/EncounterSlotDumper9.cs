using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using pkNX.Containers;
using pkNX.Structures.FlatBuffers.SV;

namespace pkNX.Structures.FlatBuffers;

public static class EncounterSlotDumper9
{
    public static void DumpSlots(IFileInternal rom, EncounterDumpConfigSV config, PaldeaSceneModel scene)
    {
        var spawns = new PaldeaSpawnModel(rom);
        var db = new LocationDatabase();

        // Process all field indices
        foreach (var index in EncounterDumperSV.AllMaps)
            ProcessAreas(index, scene, db, spawns, config.PlaceNameMap);

        // Each area and their local / crossover points have been aggregated.
        // Integrate the points' slots into a single list, and consolidate entries with same level ranges to as few objects as possible.
        foreach (var storage in db.Locations.Values)
        {
            // Consolidate encounters
            storage.Integrate();
            storage.Consolidate();
            if (storage.AreaName == "a_su0104")
                storage.Consolidate();
        }

        // Output to stream if available.
        if (config.WriteText)
        {
            using var sw = File.CreateText(Path.Combine(config.Path, "titan_enc.txt"));
            foreach (var s in db.Locations.Values)
                WriteLocation(sw, config, s.AreaName, s.AreaInfo, s.Slots);
            using var tw = File.CreateText(Path.Combine(config.Path, "titan_loc_enc.txt"));
            WriteLocEncList(tw, config, db);

            using var pw = File.CreateText(Path.Combine(config.Path, "titan_loc_point.txt"));
            WriteLocPointList(pw, config, db);
        }
        if (config.WritePickle)
        {
            string binPath = Path.Combine(config.Path, "encounter_wild_paldea.pkl");
            SerializeEncounters(binPath, db);
        }
    }

    private static void ProcessAreas(PaldeaFieldIndex fieldIndex, PaldeaSceneModel scene, LocationDatabase db, PaldeaSpawnModel map, Dictionary<string, (string Name, int Index)> placeNameMap)
    {
        // Overall Logic Flow:
        // 1 - At every point, spawn everything that can exist at that point.
        // 2 - At each area, absorb native points into local points.
        // 3 - At each area, absorb crossover points into crossover points.
        // 4 - Consolidate the encounters from both point lists.
        // Just compute everything (big memory!) then crunch it all down.

        // Fill the point lists for each area, then spawn everything into those points.
        var areaNames = scene.AreaNames[(int)fieldIndex];
        var areas = scene.AreaInfos[(int)fieldIndex];
        var types = scene.PaldeaType[(int)fieldIndex];
        var pd = map.GetPokeData(fieldIndex);
        for (var i = areaNames.Count - 1; i >= 0; i--)
        {
            var areaName = areaNames[i];

            // Determine potential spawners
            if (!scene.TryGetContainsCheck(fieldIndex, areaName, out var collider))
            {
                Console.WriteLine($"No collider for {areaName}");
                continue;
            }

            var areaInfo = areas[areaName];
            // Areas without a location name can't be reversed into location ID.
            if (!EncounterDumperSV.TryGetPlaceName(ref areaName, areaInfo, collider, placeNameMap, areas, scene, fieldIndex, out var placeName))
                continue;

            // Locations that do not spawn encounters can still have crossovers bleed into them.
            // We'll have empty local encounter lists for them.
            var storage = db.Get(placeNameMap[placeName].Index, fieldIndex, areaName, areas[areaName]);
            if (areaInfo.Tag is AreaTag.NG_Encount or AreaTag.NG_All)
                continue;

            var type = types[areaName];
            var points = map.GetPoints(fieldIndex, type);
            storage.LoadPoints(points, collider, areaInfo.ActualMinLevel, areaInfo.ActualMaxLevel, areaInfo.AdjustEncLv);
            storage.GetEncounters(pd, scene);
        }

        // Add in the locations that each point can be activated, that are not the current area.
        // Player crossing into the target area; can spawn encounters with weather foreign to the target area.
        foreach (var areaName in areaNames)
        {
            var areaInfo = areas[areaName];

            // Determine potential spawners
            if (!scene.TryGetContainsCheck(fieldIndex, areaName, out _))
                continue;

            var name = areaInfo.LocationNameMain;
            if (string.IsNullOrEmpty(name))
                continue;

            var storage = db.Get(placeNameMap[name].Index, fieldIndex, areaName, areaInfo);
            if (!IsCrossoverAllowed(storage))
                continue;

            // From our spawning area, iterate through all adjacent areas.
            foreach (var otherName in areaNames)
            {
                // Skip self
                if (otherName == areaName)
                    continue;
                // Skip areas that don't have a location name -- subzones were the initial spawn spot if so.
                var otherAreaInfo = areas[otherName];
                var otherNameMain = otherAreaInfo.LocationNameMain;
                if (string.IsNullOrEmpty(otherNameMain))
                    continue;

                // Skip areas that don't have a collider
                if (!scene.TryGetContainsCheck(fieldIndex, otherName, out var otherCollider))
                    continue;

                var cross = db.Get(placeNameMap[otherNameMain].Index, fieldIndex, otherName, otherAreaInfo);
                if (!IsCrossoverAllowed(cross))
                    continue;

                foreach (var point in storage.Local)
                {
                    // If the crossover point is close enough to the current area's collider, add it to the current area's list of crossover points.
                    if (EncounterDumperSV.IsCloseEnoughDistance(otherCollider, point))
                        point.Activate(cross.Location);
                }
            }
        }

        // For each area, we need to peek at the other areas to see if they have any crossover points.
        // For each of those crossover points, we need to see if they are in the current area's collider.
        // If they are, we need to add them to the current area's list of crossover points.
        foreach (var areaName in areaNames)
        {
            // Same sanity checking as above iteration.
            var areaInfo = areas[areaName];

            // Determine potential spawners
            if (!scene.TryGetContainsCheck(fieldIndex, areaName, out var collider))
            {
                Console.WriteLine($"No collider for {areaName}");
                continue;
            }

            var name = areaInfo.LocationNameMain;
            if (string.IsNullOrEmpty(name))
                continue;
            //if (areaInfo.Tag is AreaTag.NG_Encount or AreaTag.NG_All)
            //    continue;

            var storage = db.Get(placeNameMap[name].Index, fieldIndex, areaName, areaInfo);
            if (!IsCrossoverAllowed(storage))
                continue;

            // Here's where the fun begins. Iterate over areas inside this loop so we can look for all possible adjacent areas.
            foreach (var otherName in areaNames)
            {
                // Skip self
                if (otherName == areaName)
                    continue;
                // Skip areas that don't have a location name -- subzones were the initial spawn spot if so.
                var otherAreaInfo = areas[otherName];
                var otherNameMain = otherAreaInfo.LocationNameMain;
                if (string.IsNullOrEmpty(otherNameMain))
                    continue;

                // Skip areas that don't have a collider
                if (!scene.TryGetContainsCheck(fieldIndex, otherName, out _))
                    continue;

                // Iterate over all crossover points in the other area.
                var cross = db.Get(placeNameMap[otherNameMain].Index, fieldIndex, otherName, otherAreaInfo);
                if (!IsCrossoverAllowed(cross))
                    continue;
                foreach (var point in cross.Local)
                {
                    // If the crossover point is close enough to the current area's collider, add it to the current area's list of crossover points.
                    if (EncounterDumperSV.IsContainedBy(collider, point.Point.Position))
                        storage.Nearby.Add(point);
                }
            }
        }
    }

    /// <summary>
    /// Allow encounters into and out of this location
    /// </summary>
    public static bool IsCrossoverAllowed(LocationStorage storage)
    {
        var loc = storage.Location;
        return EncounterDumperSV.IsCrossoverAllowed(loc);
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
            ushort species = slot.Species;
            byte form = slot.Species switch
            {
                (ushort)Species.Vivillon or (ushort)Species.Spewpa or (ushort)Species.Scatterbug => 30,
                (ushort)Species.Minior => 31,
                _ => slot.Form,
            };

            // ReSharper disable RedundantCast
            bw.Write(species);
            bw.Write(form);
            bw.Write((byte)slot.Gender);

            bw.Write((byte)slot.MinLevel);
            bw.Write((byte)slot.MaxLevel);
            bw.Write((byte)slot.Time);
            bw.Write((byte)slot.Weather);
            // ReSharper restore RedundantCast
        }
        return ms.ToArray();
    }

    #region Writing

    private static void WriteLocation(TextWriter tw, EncounterDumpConfigSV config, string areaName, AreaInfo areaInfo, List<PaldeaEncounter> encounts)
    {
        var loc = areaInfo.LocationNameMain;
        if (!config.PlaceNameMap.TryGetValue(loc, out var value))
            return;
        var (name, index) = value;
        var heading = $"{areaName} - {loc} - {name} ({index})";
        WriteEncounts(tw, config.SpecNamesInternal, heading, encounts);
    }

    private static void WriteLocEncList(TextWriter tw, EncounterDumpConfigSV config, LocationDatabase db)
    {
        var names = config.PlaceNameMap;
        foreach (var place in names.Keys.OrderBy(p => names[p].Index))
        {
            var (name, index) = names[place];
            if (!db.Locations.TryGetValue(index, out var encounts))
                continue;

            var heading = $"{name} ({index})";
            WriteEncounts(tw, config.SpecNamesInternal, heading, encounts.Slots);
        }
    }

    private static void WriteEncounts(TextWriter tw, ReadOnlySpan<string> specNamesInternal, string heading, IEnumerable<PaldeaEncounter> encounts)
    {
        tw.WriteLine("===");
        tw.WriteLine(heading);
        tw.WriteLine("===");
        foreach (var e in encounts)
            tw.WriteLine($" - {e.GetEncountString(specNamesInternal)}");
        tw.WriteLine();
    }

    private static void WriteLocPointList(TextWriter tw, EncounterDumpConfigSV config, LocationDatabase db)
    {
        var names = config.PlaceNameMap;
        foreach (var place in names.Keys.OrderBy(p => names[p].Index))
        {
            var (name, index) = names[place];
            if (!db.Locations.TryGetValue(index, out var loc))
                continue;

            var heading = $"{name} ({index})";
            WriteBiomes(tw, heading, loc.Local);
        }

        foreach (var place in names.Keys.OrderBy(p => names[p].Index))
        {
            var (name, index) = names[place];
            if (!db.Locations.TryGetValue(index, out var loc))
                continue;

            var heading = $"{name} ({index})";
            WritePoints(tw, heading, loc.Local);
        }
    }

    private static void WriteBiomes(TextWriter tw, string heading, List<LocationPointDetail> points)
    {
        var biomes = points.Select(z => z.Point.Biome).Distinct().Select(z => z.ToString()).Order();
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
    #endregion
}
