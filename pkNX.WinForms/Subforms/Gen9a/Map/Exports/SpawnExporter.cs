using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using pkNX.Containers;
using pkNX.Structures.FlatBuffers;

namespace pkNX.WinForms;

public sealed class SpawnExporter(string folder)
{
    /// <summary>
    /// Allows location ID to name mapping to be provided by the caller.
    /// </summary>
    public required Func<int, string> GetLocationName { get; init; }


    public static readonly JsonSerializerOptions Options = new()
    {
        WriteIndented = true,
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping, // japanese text in some Encounter tags; don't bother escaping for safety
        Converters = { new PackedVec3fConverter(true) },
    };

    public void ExportJson<T>(T obj, string name, string map)
    {
        var json = JsonSerializer.Serialize(obj, Options);
        File.WriteAllText(Path.Combine(folder, $"{map}_{name}.json"), json);
    }

    public void ExportJson(Dictionary<int, List<FakeSpawner9a>> pointSpawners, string map)
    {
        var array = pointSpawners.SelectMany(x => x.Value);
        ExportJson(array, "point_spawners_a", map);
    }

    public void ExportJson(IEnumerable<EncounterArea9a> slotSets, string map)
    {
        var dict = slotSets.ToDictionary(z => $"{GetLocationName(z.Location)} ({z.Location})", z => z.Slots);
        ExportJson(dict, "slot_sets", map);
    }

    public void ExportJson(IEnumerable<SpawnSceneSimulation> maps)
    {
        foreach (var result in maps)
        {
            // Export info for inspection/loading into downstream tools
            ExportJson(result.SpawnerPositions, nameof(result.SpawnerPositions), result.Name);

            // Flatten the spawner positions to a string, as passing in a FlatBuffer object doesn't serialize nicely.
            var transform = result.SpawnerPositions
                .OrderBy(z => GetLocationName(result.LocationNameFetch(z.Value.Position)))
                .ThenBy(z => z.Key);
            var points = transform.Select(z => GetPositionString(result, z.Key, z.Value)).Order();
            ExportJson(points, "point_spawners", result.Name);
        }
    }

    public void ExportAll(SpawnRipper9a all)
    {
        ExportJson(all.RipInfo.SpawnerInfo, nameof(MapSpawnerSet.SpawnerInfo), "all");
        ExportJson(all.RippedMaps.SelectMany(z => z.SpawnerPositions).ToArray(), "point_spawners", "all");
    }

    private string GetPositionString(SpawnSceneSimulation result, string internalName, SceneSpawner point)
    {
        var location = result.LocationNameFetch(point.Position);
        var name = GetLocationName(location);
        var hash = FnvHash.HashFnv1a_64(internalName);
        return $"{name} - {hash:X16} - {internalName} @ {point.Position}";
    }

    // Custom converter so PackedVec3f serializes to a string via ToString().
    private sealed class PackedVec3fConverter(bool concise) : JsonConverter<PackedVec3f>
    {
        public override PackedVec3f Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            // Deserialization is not required for current usage; avoid lossy parsing.
            throw new NotSupportedException("Deserializing PackedVec3f from JSON is not supported.");
        }

        public override void Write(Utf8JsonWriter writer, PackedVec3f value, JsonSerializerOptions options)
        {
            if (concise)
                writer.WriteStringValue(value.ToShortString());
            else
                writer.WriteStringValue(value.ToString());
        }
    }
}
