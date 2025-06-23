using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using pkNX.Containers;
using pkNX.Structures.FlatBuffers.SV;
using pkNX.Structures.FlatBuffers.SV.Trinity;

namespace pkNX.Structures.FlatBuffers;

public enum PaldeaPointPivot
{
    Overworld,
    AreaZero,
}

public class PaldeaSceneModel
{
    // 3 maps = Paldea, Kitakami, Blueberry
    public List<string>[] AreaNames { get; } = [[], [], []];
    public Dictionary<string, AreaInfo>[] AreaInfos { get; } = [[], [], []];
    public Dictionary<string, HavokCollision.AABBTree>[] AreaCollisionTrees { get; } = [[], [], []];
    public Dictionary<string, BoxCollision9>[] AreaCollisionBoxes { get; } = [[], [], []];
    public Dictionary<string, PaldeaPointPivot>[] PaldeaType { get; } = [[], [], []];

    public PaldeaSceneModel(IFileInternal ROM, PaldeaFieldModel field)
    {
        // NOTE: Safe to only use _0 because _1 is identical.
        var area_collision = FlatBufferConverter.DeserializeFrom<TrinitySceneObjectTemplate>(ROM.GetPackedFile("world/scene/parts/field/resident_event/resident_area_collision_/resident_area_collision_0.trscn"));
        Debug.Assert(area_collision.ObjectTemplateName == "resident_area_collision");

        // NOTE: Safe to only use _0 because _1 is identical.
        var a0 = ROM.GetPackedFile("world/scene/parts/field/room/a_w23_field/a_w23_field_area_col_/a_w23_field_area_col_0.trscn");
        var a_w23_field_area_col = FlatBufferConverter.DeserializeFrom<TrinitySceneObjectTemplate>(a0);
        Debug.Assert(a_w23_field_area_col.ObjectTemplateName == "a_w23_field_area_col");

        foreach (var obj in area_collision.Objects.Concat(a_w23_field_area_col.Objects))
            AddIfAppropriate(ROM, field, PaldeaFieldIndex.Paldea, a_w23_field_area_col.Objects, obj);

        // NOTE: Safe to only use _0 because _1 is identical.
        var area_collision_su1 = FlatBufferConverter.DeserializeFrom<TrinitySceneObjectTemplate>(ROM.GetPackedFile("world/scene/parts/field/main/field_1/field_main/resident_event/resident_area_collision_/resident_area_collision_0.trscn"));
        Debug.Assert(area_collision_su1.ObjectTemplateName == "resident_area_collision");

        foreach (var obj in area_collision_su1.Objects)
            AddIfAppropriate(ROM, field, PaldeaFieldIndex.Kitakami, [], obj);

        // NOTE: Safe to only use _0 because _1 is identical.
        var area_collision_su2 = FlatBufferConverter.DeserializeFrom<TrinitySceneObjectTemplate>(ROM.GetPackedFile("world/scene/parts/field/main/field_2/field_main/resident_event/resident_area_collision_/resident_area_collision_0.trscn"));
        Debug.Assert(area_collision_su2.ObjectTemplateName == "resident_area_collision");

        foreach (var obj in area_collision_su2.Objects)
            AddIfAppropriate(ROM, field, PaldeaFieldIndex.Terarium, [], obj);
    }

    private void AddIfAppropriate(IFileInternal ROM, PaldeaFieldModel field, PaldeaFieldIndex index,
        IList<TrinitySceneObjectTemplateEntry> a0, TrinitySceneObjectTemplateEntry obj)
    {
        if (!(obj.SubObjects.Count > 0 && obj.SubObjects[0].Type == "trinity_CollisionComponent"))
            return;

        var inner = obj.SubObjects[0].Data;
        var tcom = FlatBufferConverter.DeserializeFrom<TrinityComponent>(inner);
        var collision = tcom.Component.CollisionComponent.Shape;

        var isAtlantisObj = GetPaldeaType(a0, obj);
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
                PaldeaType[(int)index][sObj.ObjectTemplateName] = isAtlantisObj;
                break;
            }
            case "trinity_SceneObject":
            {
                var sceneObject = FlatBufferConverter.DeserializeFrom<TrinitySceneObject>(obj.Data);
                AddSceneObject(index, sceneObject.ObjectName, sceneObject, collision, field, ROM);
                PaldeaType[(int)index][sceneObject.ObjectName] = isAtlantisObj;
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

    private PaldeaPointPivot GetPaldeaType(IList<TrinitySceneObjectTemplateEntry> a0, TrinitySceneObjectTemplateEntry obj)
    {
        if (a0.Contains(obj))
            return PaldeaPointPivot.AreaZero;
        return PaldeaPointPivot.Overworld;
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

    public bool TryGetParentAreaName(PaldeaFieldIndex fieldIndex, string areaName, PackedVec3f point, Func<string, bool> criteria, [NotNullWhen(true)] out string? parent)
    {
        var areas = AreaNames[(int)fieldIndex];
        for (var i = areas.Count - 1; i >= 0; i--)
        {
            var name = areas[i];
            if (name == areaName)
                continue;

            if (!criteria(name))
                continue;

            if (!TryGetContainsCheck(fieldIndex, name, out var outer))
                continue;

            if (!outer.ContainsPoint(point.X, point.Y, point.Z))
                continue;

            //Debug.WriteLine($"Remapped {areaName} to {name}");
            parent = name;
            return true;
        }

        parent = null;
        return false;
    }

    public bool TryGetParentAreaName(PaldeaFieldIndex fieldIndex, string areaName, IContainsV3f inner, Func<string, bool> criteria, [NotNullWhen(true)] out string? parent)
    {
        var areas = AreaNames[(int)fieldIndex];
        for (var i = areas.Count - 1; i >= 0; i--)
        {
            var name = areas[i];
            if (name == areaName)
                continue;

            if (!criteria(name))
                continue;

            if (!TryGetContainsCheck(fieldIndex, name, out var outer))
                continue;

            if (!inner.ContainedBy(outer))
                continue;

            //Debug.WriteLine($"Remapped {areaName} to {name}");
            parent = name;
            return true;
        }

        parent = null;
        return false;
    }
}
