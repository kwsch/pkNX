using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using pkNX.Game;
using pkNX.Structures;
using pkNX.Structures.FlatBuffers;
using pkNX.Structures.FlatBuffers.ZA;
using pkNX.Structures.FlatBuffers.ZA.Trinity;

namespace pkNX.WinForms;

public sealed class LumioseMap
{
    // Public surface for consumers of a single map
    public List<string> AreaNames { get; } = [];
    public OrderedDictionary<string, MultiAABBTree> AreaCollisionTrees { get; } = [];
    public OrderedDictionary<string, BoxCollision9> AreaCollisionBoxes { get; } = [];

    private readonly IList<FieldMainArea> MainAreas;
    private readonly IList<FieldSubArea> SubAreas;
    private readonly IList<FieldBattleZone> BattleZoneAreas;
    private readonly IList<FieldWildZone> WildZoneAreas;
    private readonly IList<FieldSceneArea> SceneAreas;
    private readonly IList<FieldLocation> FieldLocations;

    public LumioseMap(
        IList<FieldMainArea> main,
        IList<FieldSubArea> sub,
        IList<FieldBattleZone> battle,
        IList<FieldWildZone> wild,
        IList<FieldSceneArea> scene,
        IList<FieldLocation> locations)
    {
        MainAreas = main;
        SubAreas = sub;
        BattleZoneAreas = battle;
        WildZoneAreas = wild;
        SceneAreas = scene;
        FieldLocations = locations;
    }

    public void LoadScenes(GameManager9a ROM, IEnumerable<string> scenePaths)
    {
        foreach (var scene in scenePaths)
        {
            var data = ROM.GetPackedFile(scene);
            LoadScene(ROM, data, scene);
        }
    }

    private void LoadScene(GameManager9a rom, Memory<byte> rootData, string fileName)
    {
        var root = FlatBufferConverter.DeserializeFrom<TrinitySceneObjectTemplate>(rootData);
        var subScenes = root.Field05;

        // Recurse for sub-scenes.
        foreach (var sub in subScenes)
        {
            // Convert from relative path to actual romfs path.
            // Ensure it actually is configured as a relative path
            System.Diagnostics.Debug.Assert(sub.StartsWith("../area_collision"));

            // Find parent directory, as we only load fully qualified file paths
            var start = fileName.AsSpan().IndexOf(sub.AsSpan(2, "/area_collision".Length));
            System.Diagnostics.Debug.Assert(start != -1);

            // Get actual file name (with _0 appended -- why? dunno!)
            var subFileName = $"{fileName[..start]}{sub[2..].Replace(".trscn", "_0.trscn")}";
            var file = rom.GetPackedFile(subFileName);

            LoadScene(rom, file, subFileName);
        }

        foreach (var obj in root.Objects)
        {
            // "SubScene" are already handled at start. We only care about collision components at this point.

            // Main map immediately has trinity Scene objects.
            if (obj is { Type: "trinity_SceneObject", SubObjects: { Count: not 0 } so })
            {
                var inner = so[0];
                if (inner.Type is not "trinity_CollisionComponent")
                    continue;

                var scene = FlatBufferConverter.DeserializeFrom<TrinitySceneObject>(obj.Data);
                var name = scene.Name;
                var trc = FlatBufferConverter.DeserializeFrom<TrinityComponent>(inner.Data);
                var coll = trc.Component.CollisionComponent;
                LoadSceneCollision(rom, name, scene, coll);
            }

            // Zone scene containers store them in nested object templates.
            if (obj is { Type: "trinity_ObjectTemplate", SubObjects: { Count: not 0 } ot })
            {
                var inner = ot[0];
                if (inner.Type is not "trinity_CollisionComponent")
                    continue;

                var sObj = FlatBufferConverter.DeserializeFrom<TrinitySceneObjectTemplateData>(obj.Data);
                if (sObj.Type is not "trinity_SceneObject")
                    continue;

                var scene = FlatBufferConverter.DeserializeFrom<TrinitySceneObject>(sObj.Data);
                var name = sObj.ObjectTemplateName;
                var trc = FlatBufferConverter.DeserializeFrom<TrinityComponent>(inner.Data);
                var coll = trc.Component.CollisionComponent;
                LoadSceneCollision(rom, name, scene, coll);
            }
        }
    }

    private void LoadSceneCollision(GameManager9a rom, string name, TrinitySceneObject sceneObject, CollisionComponent collisionComponent)
    {
        AreaNames.Add(name);

        // Verified unused booleans
        // if (sceneObject.Field02 || sceneObject.Field05)

        var shape = collisionComponent.Shape;
        if (shape.TryGet(out Box? box))
        {
            // Box collision, obj.ObjectPosition.Transform is pos, box.Size is size of box
            // Box rotation/transform is unused in this game
            // if (!box.Transform.IsZero() || !box.Rotation.IsZero())
            var value = new BoxCollision9
            {
                Position = sceneObject.Position.Translation,
                Size = box.Size,
            };
            AreaCollisionBoxes.Add(name, value);
        }
        else if (shape.TryGet(out Havok? havok))
        {
            var havokData = rom.GetPackedFile(havok.TrcolFilePath);
            var tree = HavokCollision.ParseAABBTree(havokData);

            // if (sceneObject.ApplySRT) -- no idea?
            {
                var srt = sceneObject.Position;
                var s = srt.Scale;
                var r = srt.Rotation;
                var t = srt.Translation;

                // Havok shape SRT is unused in this game
                // if (!havok.Transform.IsZero() || !havok.Scale.IsOne() || !havok.Rotation.IsZero())

                if (!s.IsOne())
                    tree.Scale(s.X, s.Y, s.Z);
                if (!r.IsZero())
                    tree.Rotate(r.X, r.Y, r.Z);
                if (!t.IsZero())
                    tree.Translate(t.X, t.Y, t.Z);
            }
            if (AreaCollisionTrees.TryGetValue(name, out var exist))
                exist.Add(tree);
            else
                AreaCollisionTrees.Add(name, new MultiAABBTree(name, tree));
        }
        else
        {
            System.Diagnostics.Debug.Fail("Unhandled shape.");
        }
    }

    public IAreaLayer FindAreaInfo(string name)
    {
        foreach (var area in MainAreas)
        {
            if (IsAreaName(area, name))
                return area;
        }
        foreach (var area in SubAreas)
        {
            if (IsAreaName(area, name))
                return area;
        }
        foreach (var area in SceneAreas)
        {
            if (IsAreaName(area, name))
                return area;
        }
        foreach (var area in BattleZoneAreas)
        {
            if (IsAreaName(area, name))
                return area;
        }
        foreach (var area in WildZoneAreas)
        {
            if (IsAreaName(area, name))
                return area;
        }
        foreach (var area in FieldLocations)
        {
            if (IsAreaName(area, name))
                return area;
        }
        throw new ArgumentException($"Unknown area {name}");
    }

    private static bool IsAreaName<T>(T area, string name) where T : IAreaLayer => area.AreaInfo.AreaName == name;

    public IEnumerable<IntersectingTree> GetCollisions(float x, float z, SpawnNameResolver resolver) => GetCollisions(x, 100, z, true, resolver);
    public IEnumerable<IntersectingTree> GetCollisions(float x, float y, float z, SpawnNameResolver resolver) => GetCollisions(x, y, z, false, resolver);
    public string GetPlaceName(float x, float z, out string areaKey) => GetPlaceName(x, 100, z, true, out areaKey);
    public string GetPlaceName(float x, float y, float z, out string areaKey) => GetPlaceName(x, y, z, false, out areaKey);

    private IEnumerable<IntersectingTree> GetCollisions(float x, float y, float z, bool noY, SpawnNameResolver resolver)
    {
        foreach (var area in AreaCollisionTrees)
        {
            var coll = area.Value;
            if (noY ? !coll.ContainsPoint(x, z) : !coll.ContainsPoint(x, y, z))
                continue;

            var key = area.Key;
            var info = FindAreaInfo(key);

            var layer = info.Layer;
            var type = info.AreaInfo.AreaType;
            var valid = IsValidLocation(info.AreaInfo);
            var placeName = GetPlaceName(info);
            var locationID = resolver.GetLocationIndex(placeName);
            var locationName = locationID == -1 ? "Out of Bounds" : resolver.LocationNames[locationID];
            yield return new IntersectingTree
            {
                Layer = layer,
                Name = key,
                Valid = valid,
                Type = type,
                AreaInfo = info.AreaInfo,
                Collision = coll,
                PlaceName = placeName,
                LocationID = locationID,
                LocationName = locationName,
            };
        }
    }

    private string GetPlaceName(float x, float y, float z, bool noY, out string areaKey)
    {
        areaKey = string.Empty;
        IAreaLayer? result = null;
        var best = (AreaLayer.Scene, AreaType.Default);
        float bestFloorDistance = float.PositiveInfinity;
        // The highest value of tuple is preferred.
        // Prefer better Layer first, then AreaType second.

        foreach (var area in AreaCollisionBoxes)
        {
            var coll = area.Value;
            if (!coll.ContainsPoint(x, y, z))
                continue;

            var key = area.Key;
            var info = FindAreaInfo(key);

            var layer = info.Layer;
            var type = info.AreaInfo.AreaType;
            var candidate = (layer, type);
            if (!IsZoneBetter(candidate, best) || !IsValidLocation(info.AreaInfo))
                continue;

            areaKey = key;
            result = info;
            best = candidate;
            bestFloorDistance = 0f; // boxes don't provide floor; treat as perfect fit if they win layer/type
        }
        foreach (var area in AreaCollisionTrees)
        {
            var coll = area.Value;
            if (noY ? !coll.ContainsPoint(x, z) : !coll.ContainsPoint(x, y, z))
                continue;

            var key = area.Key;
            var info = FindAreaInfo(key);

            var layer = info.Layer;
            var type = info.AreaInfo.AreaType;
            var candidate = (layer, type);
            if (!IsValidLocation(info.AreaInfo))
                continue;

            if (IsZoneBetter(candidate, best))
            {
                result = info;
                best = candidate;
                bestFloorDistance = coll.DistanceToFloor(x, y, z);
                continue;
            }

            // Final tie-breaker: same priority, prefer closest floor under (x,y,z)
            if (candidate != best) // only check if same layer
                continue;

            var dist = coll.DistanceToFloor(x, y, z);
            if ((dist > bestFloorDistance))
                continue;

            areaKey = key;
            result = info;
            bestFloorDistance = dist;
        }

        if (result is null)
        {
            System.Diagnostics.Debug.WriteLine($"Unable to place {x:R},{z:R}");
            return string.Empty;
        }

        return GetPlaceName(result);
    }

    private static string GetPlaceName(IAreaLayer result)
    {
        var ai = result.AreaInfo;
        if (!string.IsNullOrWhiteSpace(ai.SubAreaName))
            return ai.SubAreaName;
        if (!string.IsNullOrWhiteSpace(ai.MainAreaName))
            return ai.MainAreaName;
        return ai.AreaName; // should never hit, filtered via quality check below
    }

    private static bool IsValidLocation(FieldAreaInfo info) =>
        !string.IsNullOrWhiteSpace(info.SubAreaName) || !string.IsNullOrWhiteSpace(info.MainAreaName);

    private static bool IsZoneBetter((AreaLayer Layer, AreaType Type) candidate, (AreaLayer Layer, AreaType Type) best)
    {
        if (candidate.Layer is AreaLayer.BattleZone)
            return false; // No wild encounters; don't care.
        if (candidate.Type is AreaType.Room)
            return false; // No wild encounters; don't care.

        if (candidate.Layer < best.Layer)
            return false;

        if (candidate.Layer > best.Layer)
            return true;
        return candidate.Type > best.Type;
    }
}

public sealed record MultiAABBTree(string Name, HavokCollision.AABBTree Tree1) : IEnumerable<HavokCollision.AABBTree>
{
    public MultiAABBTree? Next { get; set; }

    public void Add(HavokCollision.AABBTree tree)
    {
        if (Next is null)
            Next = new MultiAABBTree(Name, tree);
        else
            Next.Add(tree);
    }

    public bool ContainsPoint(float x, float z)
    {
        if (Tree1.ContainsPoint(x, 100, z)) // drop floor of Y axis
            return true;
        if (Next is not null)
            return Next.ContainsPoint(x, z);
        return false;
    }

    public bool ContainsPoint(float x, float y, float z)
    {
        if (Tree1.ContainsPointDirect(x, y + 1, z)) // don't drop floor of Y axis
            return true;
        if (Next is not null)
            return Next.ContainsPoint(x, y, z);
        return false;
    }

    /// <summary>
    /// Attempts to find the highest floor (LoY) beneath the given point across all chained trees.
    /// </summary>
    public bool TryGetFloorY(float x, float y, float z, out float floorY)
    {
        var found = Tree1.TryGetFloorY(x, y, z, out floorY);

        // check if any Next set found a better floor
        if (Next is null)
            return found;
        if (!Next.TryGetFloorY(x, y, z, out var ny))
            return found;
        if (found && !(ny > floorY))
            return found;

        // next found a better floor
        floorY = ny;
        found = true;
        return found;
    }

    /// <summary>
    /// Returns the distance from y to the closest floor below (y - floorY). Returns PositiveInfinity if none found.
    /// </summary>
    public float DistanceToFloor(float x, float y, float z)
    {
        return TryGetFloorY(x, y, z, out var fy) ? (y - fy) : float.PositiveInfinity;
    }

    public IEnumerator<HavokCollision.AABBTree> GetEnumerator()
    {
        yield return Tree1;
        if (Next is null)
            yield break;
        foreach (var tree in Next)
            yield return tree;
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

public sealed record IntersectingTree
{
    // Displayed via Json reflection
    public required AreaLayer Layer { get; init; }
    public required string Name { get; init; }
    public required bool Valid { get; init; }
    public required AreaType Type { get; init; }
    public required string PlaceName { get; init; }
    public required int LocationID { get; init; }
    public required string LocationName { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.Always)] public FieldAreaInfo AreaInfo { get; init; } = null!;
    [JsonIgnore(Condition = JsonIgnoreCondition.Always)] public MultiAABBTree Collision { get; init; } = null!;
    public string GetAreaInternal() => AreaInfo.AreaName;
}
