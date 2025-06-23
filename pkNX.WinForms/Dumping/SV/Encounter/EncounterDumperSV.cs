using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using pkNX.Containers;
using pkNX.Structures.FlatBuffers.SV;

namespace pkNX.Structures.FlatBuffers;

public static class EncounterDumperSV
{
    private const float tolX = 30f;
    private const float tolY = 30f;
    private const float tolZ = 30f;

    public static bool IsContainedBy<T>(T collider, LocationPointDetail point) where T : IContainsV3f
        => collider.ContainsPoint(point.X, point.Y, point.Z, tolX, tolY, tolZ);

    public static bool IsContainedBy<T>(T collider, PackedVec3f point) where T : IContainsV3f
        => collider.ContainsPoint(point.X, point.Y, point.Z, tolX, tolY, tolZ);

    public static bool IsCloseEnoughDistance<T>(T collider, LocationPointDetail point) where T : IContainsV3f
        => collider.ContainsPoint(point.X, point.Y, point.Z, tolX, tolY, tolZ);

    public static ReadOnlySpan<PaldeaFieldIndex> AllMaps =>
    [
        PaldeaFieldIndex.Paldea,
        PaldeaFieldIndex.Kitakami,
        PaldeaFieldIndex.Terarium,
    ];

    public static void Dump(IFileInternal rom, EncounterDumpConfigSV config)
    {
        if (!Directory.Exists(config.Path))
            Directory.CreateDirectory(config.Path);

        var field = new PaldeaFieldModel(rom);
        var scene = new PaldeaSceneModel(rom, field);
        EncounterSlotDumper9.DumpSlots(rom, config, scene);
        FixedSymbolDumper9.Dump(rom, config, scene);
        GimmighoulDumper.Dump(rom, config, scene);

        // Raw dumps for inspection
        DumpScene(scene, config.Path);
        DumpField(field, config.Path);
    }

    public static bool TryGetPlaceName(ref string areaName, AreaInfo areaInfo,
        IContainsV3f collider, Dictionary<string, (string Name, int Index)> placeNameMap,
        IReadOnlyDictionary<string, AreaInfo> areas,
        PaldeaSceneModel scene, PaldeaFieldIndex fieldIndex, out string placeName)
    {
        placeName = areaInfo.LocationNameMain;
        if (!string.IsNullOrEmpty(placeName))
            return true;

        // Maybe this is a sub-area? Try to get the parent area name.
        bool IsValidParentAreaName(string aName)
        {
            if (!areas.TryGetValue(aName, out var info))
                return false;
            var n = info.LocationNameMain;
            if (string.IsNullOrEmpty(n))
                return false;
            return placeNameMap.ContainsKey(n);
        }

        if (!scene.TryGetParentAreaName(fieldIndex, areaName, collider, IsValidParentAreaName, out var parentAreaName))
        {
            Console.WriteLine($"No parent area for {areaName}");
            return false;
        }

        areaName = parentAreaName;
        placeName = areas[parentAreaName].LocationNameMain;
        return true;
    }

    public static bool TryGetPlaceName(ref string areaName, AreaInfo areaInfo,
        PackedVec3f point, Dictionary<string, (string Name, int Index)> placeNameMap,
        IReadOnlyDictionary<string, AreaInfo> areas,
        PaldeaSceneModel scene, PaldeaFieldIndex fieldIndex, out string placeName)
    {
        placeName = areaInfo.LocationNameMain;
        if (!string.IsNullOrEmpty(placeName))
            return true;

        // Maybe this is a sub-area? Try to get the parent area name.
        bool IsValidParentAreaName(string aName)
        {
            if (!areas.TryGetValue(aName, out var info))
                return false;
            var n = info.LocationNameMain;
            if (string.IsNullOrEmpty(n))
                return false;
            return placeNameMap.ContainsKey(n);
        }

        if (!scene.TryGetParentAreaName(fieldIndex, areaName, point, IsValidParentAreaName, out var parentAreaName))
        {
            Console.WriteLine($"No parent area for {areaName}");
            return false;
        }

        areaName = parentAreaName;
        placeName = areas[parentAreaName].LocationNameMain;
        return true;
    }

    private static void DumpScene(PaldeaSceneModel scene, string path)
    {
        // Dump each property to json.
        var dest = Path.Combine(path, "paldea_scene.json");
        var json = JsonSerializer.Serialize(scene, new JsonSerializerOptions { WriteIndented = true, IncludeFields = true });
        File.WriteAllText(dest, json);
    }

    private static void DumpField(PaldeaFieldModel field, string path)
    {
        // Dump each property to json.
        var dest = Path.Combine(path, "paldea_field.json");
        var json = JsonSerializer.Serialize(field, new JsonSerializerOptions { WriteIndented = true, IncludeFields = true });
        File.WriteAllText(dest, json);
    }

    public static bool IsCrossoverAllowed(int MetLocation) => MetLocation switch
    {
        8 => false, // Mesagoza
        124 => false, // Area Zero
        _ => true,
    };

    public static Dictionary<string, (string Name, int Index)> GetInternalStringLookup(ReadOnlySpan<string> text, AHTB ahtb)
    {
        var result = new Dictionary<string, (string Name, int Index)>();
        for (var i = 0; i < text.Length; i++)
        {
            var entry = ahtb.Entries[i];
            var name = entry.Name;
            var value = (text[i], i);
            result.Add(name, value);
        }
        return result;
    }
}
