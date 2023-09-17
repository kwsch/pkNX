using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using pkNX.Containers;
using pkNX.Structures.FlatBuffers.SV;
using pkNX.Structures.FlatBuffers.SV.Trinity;

namespace pkNX.Structures.FlatBuffers;

public class PaldeaSceneModel
{
    public List<string>[] AreaNames { get; } = { new(), new() };
    public Dictionary<string, AreaInfo>[] AreaInfos { get; } = { new(), new() };
    public Dictionary<string, HavokCollision.AABBTree>[] AreaCollisionTrees { get; } = { new(), new() };
    public Dictionary<string, BoxCollision9>[] AreaCollisionBoxes { get; } = { new(), new() };
    public Dictionary<string, bool>[] IsAtlantis { get; } = { new(), new() };

    public PaldeaSceneModel(IFileInternal ROM, PaldeaFieldModel field)
    {
        // NOTE: Safe to only use _0 because _1 is identical.
        var area_collision = FlatBufferConverter.DeserializeFrom<TrinitySceneObjectTemplate>(ROM.GetPackedFile("world/scene/parts/field/resident_event/resident_area_collision_/resident_area_collision_0.trscn"));
        Debug.Assert(area_collision.ObjectTemplateName == "resident_area_collision");

        // NOTE: Safe to only use _0 because _1 is identical.
        var a_w23_field_area_col = FlatBufferConverter.DeserializeFrom<TrinitySceneObjectTemplate>(ROM.GetPackedFile("world/scene/parts/field/room/a_w23_field/a_w23_field_area_col_/a_w23_field_area_col_0.trscn"));
        Debug.Assert(a_w23_field_area_col.ObjectTemplateName == "a_w23_field_area_col");

        foreach (var obj in area_collision.Objects.Concat(a_w23_field_area_col.Objects))
            AddIfAppropriate(ROM, field, PaldeaFieldIndex.Paldea, a_w23_field_area_col.Objects, obj);

        // TODO: is this _0 or _1? Identical? Name?
        var area_collision_su1 = FlatBufferConverter.DeserializeFrom<TrinitySceneObjectTemplate>(ROM.GetPackedFile(0x441FE0A17C85BEAA));
        Debug.Assert(area_collision_su1.ObjectTemplateName == "resident_area_collision");

        foreach (var obj in area_collision_su1.Objects)
            AddIfAppropriate(ROM, field, PaldeaFieldIndex.Kitakami, new List<TrinitySceneObjectTemplateEntry>(), obj);
    }

    private void AddIfAppropriate(IFileInternal ROM, PaldeaFieldModel field, PaldeaFieldIndex index, IList<TrinitySceneObjectTemplateEntry> atlantis, TrinitySceneObjectTemplateEntry obj)
    {
        if (!(obj.SubObjects.Count > 0 && obj.SubObjects[0].Type == "trinity_CollisionComponent"))
            return;

        var inner = obj.SubObjects[0].Data;
        var tcom = FlatBufferConverter.DeserializeFrom<TrinityComponent>(inner);
        var collision = tcom.Component.CollisionComponent.Shape;

        var isAtlantisObj = atlantis.Contains(obj);
        switch (obj.Type)
        {
            case "trinity_ObjectTemplate":
            {
                var sObj = FlatBufferConverter.DeserializeFrom<TrinitySceneObjectTemplateData>(obj.Data);
                if (sObj.Type != "trinity_SceneObject")
                    return;
                var sceneObject = FlatBufferConverter.DeserializeFrom<TrinitySceneObject>(sObj.Data);
                Debug.Assert(sceneObject.ObjectName == sObj.ObjectTemplateExtra);

                AddSceneObject(index, sObj.ObjectTemplateName, sceneObject, collision, field, ROM);
                IsAtlantis[(int)index][sObj.ObjectTemplateName] = isAtlantisObj;
                break;
            }
            case "trinity_SceneObject":
            {
                var sceneObject = FlatBufferConverter.DeserializeFrom<TrinitySceneObject>(obj.Data);
                AddSceneObject(index, sceneObject.ObjectName, sceneObject, collision, field, ROM);
                IsAtlantis[(int)index][sceneObject.ObjectName] = isAtlantisObj;
                break;
            }
            // ReSharper disable once RedundantEmptySwitchSection
            default:
            {
                // Ignored.
                break;
            }
        }
    }

    private void AddSceneObject(PaldeaFieldIndex index, string name, TrinitySceneObject sceneObject, CollisionUnion shape, PaldeaFieldModel field, IFileInternal ROM)
    {
        AreaNames[(int)index].Add(name);
        AreaInfos[(int)index][name] = field.FindAreaInfo(index, name);

        Debug.Assert(shape.Discriminator is 2 or 4);

        if (shape.TryGet(out Box? box))
        {
            // Box collision, obj.ObjectPosition.Field02 is pos, box.Field02 is size of box
            AreaCollisionBoxes[(int)index][name] = new BoxCollision9
            {
                Position = sceneObject.ObjectPosition.Field02,
                Size = box!.Field02,
            };
        }
        else if (shape.TryGet(out Havok? havok))
        {
            var havokData = ROM.GetPackedFile(havok!.TrcolFilePath);
            AreaCollisionTrees[(int)index][name] = HavokCollision.ParseAABBTree(havokData);
        }
    }

    public bool IsPointContained(PaldeaFieldIndex index, string areaName, float x, float y, float z)
    {
        if (AreaCollisionTrees[(int)index].TryGetValue(areaName, out var tree))
            return tree.ContainsPoint(x, y, z);
        if (AreaCollisionBoxes[(int)index].TryGetValue(areaName, out var box))
            return box.ContainsPoint(x, y, z);
        return false;
    }

    public bool TryGetContainsCheck(PaldeaFieldIndex index, string areaName, [NotNullWhen(true)] out IContainsV3f? result)
    {
        if (AreaCollisionTrees[(int)index].TryGetValue(areaName, out var tree))
        {
            result = tree;
            return true;
        }

        if (AreaCollisionBoxes[(int)index].TryGetValue(areaName, out var box))
        {
            result = box;
            return true;
        }

        result = null;
        return false;
    }
}
