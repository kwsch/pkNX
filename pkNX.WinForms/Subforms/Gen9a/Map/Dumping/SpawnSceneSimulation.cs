using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using pkNX.Structures.FlatBuffers;
using pkNX.Structures.FlatBuffers.ZA.Trinity;

namespace pkNX.WinForms;

public sealed class SpawnSceneSimulation
{
    public required string Name { get; init; }
    public required string[] Paths { get; init; }
    public required Func<PackedVec3f, int> LocationNameFetch { get; init; }
    public required FlatBufferSource Game { get; init; }

    public readonly Dictionary<string, SceneSpawner> SpawnerPositions = []; // scraped from Scene

    public void ScrapePoints()
    {
        // For every scene, scrape out every v3f point we can find that is of a valid spawner object shape.
        for (var i = 0; i < Paths.Length; i++)
        {
            Debug.WriteLine($"Processing Scene {i + 1}/{Paths.Length}: {Paths[i]}");
            var file = Paths[i];
            var scene = Game.Get<TrinitySceneObjectTemplate>(file);
            RipScenePoints(scene);
        }
    }

    private void RipScenePoints(TrinitySceneObjectTemplate scene)
    {
        Debug.WriteLine($"Dumping {scene.Objects.Count} from {scene.ObjectTemplateName}");
        foreach (var obj in scene.Objects)
        {
            var name = obj.Type;
            if (name is not ObjectTemplate)
                continue;

            var so = Game.Get<TrinitySceneObjectTemplateData>(obj.Data);
            if (!RipScenePoints(so, out var point))
                continue;

            SpawnerPositions.Add(point.Name, point);
            for (var i = 0; i < obj.SubObjects.Count; i++)
            {
                var sub = obj.SubObjects[i];
                name = sub.Type;
                if (name is not SceneObject)
                    continue;
                var s = Game.Get<TrinitySceneObject>(sub.Data);
                if (RipScenePoints(s, $"subObject[{i}]", out var result))
                    point.Inner.Add(result);
            }
        }
    }

    private bool RipScenePoints(TrinitySceneObjectTemplateData obj, [NotNullWhen(true)] out SceneSpawner? result)
    {
        result = null;
        var name = obj.Type;
        if (name is not SceneObject)
            return false;

        var objName = obj.ObjectTemplateName;

        var so = Game.Get<TrinitySceneObject>(obj.Data);
        return RipScenePoints(so, objName, out result);
    }

    private static bool RipScenePoints(TrinitySceneObject so, string objName, [NotNullWhen(true)] out SceneSpawner? result)
    {
        result = null;
        var type = so.Name;
        if (!IsValidSpawnerShape(type))
            return false;

        var scale = so.Position.Scale;
        var rotation = so.Position.Rotation;
        var position = so.Position.Translation;
        result = new SceneSpawner(scale, rotation, position, type, objName);
        return true;
    }

    private const string SceneObject = "trinity_SceneObject";
    private const string ObjectTemplate = "trinity_ObjectTemplate";

    private const string SpawnerSphere = "pokemon_spawner_sphere_object";
    private const string SpawnerBox = "pokemon_spawner_box_object";
    private const string SpawnerPoint = "pokemon_spawner_point_object";

    private static bool IsValidSpawnerShape(string name) => name is SpawnerSphere or SpawnerBox or SpawnerPoint;
}
