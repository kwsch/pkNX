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
    public readonly List<string> areaNames = new();
    public readonly Dictionary<string, AreaInfo> AreaInfos = new();
    public readonly Dictionary<string, HavokCollision.AABBTree> areaColTrees = new();
    public readonly Dictionary<string, BoxCollision9> areaColBoxes = new();
    public readonly Dictionary<string, bool> isAtlantis = new();

    public PaldeaSceneModel(IFileInternal ROM, PaldeaFieldModel field)
    {
        // NOTE: Safe to only use _0 because _1 is identical.
        var area_management = FlatBufferConverter.DeserializeFrom<TrinitySceneObjectTemplate>(ROM.GetPackedFile("world/scene/parts/field/field_system/area_management_/area_management_0.trscn"));
        Debug.Assert(area_management.ObjectTemplateName == "area_management");

        // NOTE: Safe to only use _0 because _1 is identical.
        var a_w23_field_area_col = FlatBufferConverter.DeserializeFrom<TrinitySceneObjectTemplate>(ROM.GetPackedFile("world/scene/parts/field/room/a_w23_field/a_w23_field_area_col_/a_w23_field_area_col_0.trscn"));
        Debug.Assert(a_w23_field_area_col.ObjectTemplateName == "a_w23_field_area_col");

        foreach (var obj in area_management.Objects.Concat(a_w23_field_area_col.Objects))
            AddIfAppropriate(ROM, field, a_w23_field_area_col, obj);
    }

    private void AddIfAppropriate(IFileInternal ROM, PaldeaFieldModel field, TrinitySceneObjectTemplate w23, TrinitySceneObjectTemplateEntry obj)
    {
        if (!(obj.SubObjects.Count > 0 && obj.SubObjects[0].Type == "trinity_CollisionComponent"))
            return;

        var inner = obj.SubObjects[0].Data;
        var tcom = FlatBufferConverter.DeserializeFrom<TrinityComponent>(inner);
        var collision = tcom.Component.CollisionComponent.Shape;

        var isAtlantisObj = w23.Objects.Contains(obj);
        switch (obj.Type)
        {
            case "trinity_ObjectTemplate":
            {
                var sObj = FlatBufferConverter.DeserializeFrom<TrinitySceneObjectTemplateData>(obj.Data);
                if (sObj.Type != "trinity_SceneObject")
                    return;
                var sceneObject = FlatBufferConverter.DeserializeFrom<TrinitySceneObject>(sObj.Data);
                Debug.Assert(sceneObject.ObjectName == sObj.ObjectTemplateExtra);

                AddSceneObject(sObj.ObjectTemplateName, sceneObject, collision, field, ROM);
                isAtlantis[sObj.ObjectTemplateName] = isAtlantisObj;
                break;
            }
            case "trinity_SceneObject":
            {
                var sceneObject = FlatBufferConverter.DeserializeFrom<TrinitySceneObject>(obj.Data);
                AddSceneObject(sceneObject.ObjectName, sceneObject, collision, field, ROM);
                isAtlantis[sceneObject.ObjectName] = isAtlantisObj;
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

    private void AddSceneObject(string name, TrinitySceneObject sceneObject, CollisionUnion shape, PaldeaFieldModel field, IFileInternal ROM)
    {
        areaNames.Add(name);
        AreaInfos[name] = field.FindAreaInfo(name);

        Debug.Assert(shape.Discriminator is 2 or 4);

        if (shape.TryGet(out Box? box))
        {
            // Box collision, obj.ObjectPosition.Field_02 is pos, box.Field_01 is size of box
            areaColBoxes[name] = new BoxCollision9
            {
                Position = sceneObject.ObjectPosition.Field02,
                Size = box.Field01,
            };
        }
        else if (shape.TryGet(out Havok? havok))
        {
            var havokData = ROM.GetPackedFile(havok.TrcolFilePath);
            areaColTrees[name] = HavokCollision.ParseAABBTree(havokData);
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

    public bool TryGetContainsCheck(string areaName, [NotNullWhen(true)] out IContainsV3f? result)
    {
        if (areaColTrees.TryGetValue(areaName, out var tree))
        {
            result = tree;
            return true;
        }

        if (areaColBoxes.TryGetValue(areaName, out var box))
        {
            result = box;
            return true;
        }

        result = null;
        return false;
    }
}
