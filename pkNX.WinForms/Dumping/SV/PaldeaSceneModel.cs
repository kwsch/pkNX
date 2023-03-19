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
    public readonly List<string> areaNames;
    public readonly Dictionary<string, AreaInfo> AreaInfos;
    public readonly Dictionary<string, HavokCollision.AABBTree> areaColTrees;
    public readonly Dictionary<string, BoxCollision9> areaColBoxes;
    public readonly Dictionary<string, bool> isAtlantis;

    public PaldeaSceneModel(IFileInternal ROM, PaldeaFieldModel field)
    {
        // NOTE: Safe to only use _0 because _1 is identical.
        var area_management = FlatBufferConverter.DeserializeFrom<TrinitySceneObjectTemplate>(ROM.GetPackedFile("world/scene/parts/field/field_system/area_management_/area_management_0.trscn"));
        Debug.Assert(area_management.ObjectTemplateName == "area_management");

        // NOTE: Safe to only use _0 because _1 is identical.
        var a_w23_field_area_col = FlatBufferConverter.DeserializeFrom<TrinitySceneObjectTemplate>(ROM.GetPackedFile("world/scene/parts/field/room/a_w23_field/a_w23_field_area_col_/a_w23_field_area_col_0.trscn"));
        Debug.Assert(a_w23_field_area_col.ObjectTemplateName == "a_w23_field_area_col");

        areaNames = new List<string>();
        AreaInfos = new Dictionary<string, AreaInfo>();
        areaColTrees = new Dictionary<string, HavokCollision.AABBTree>();
        areaColBoxes = new Dictionary<string, BoxCollision9>();
        isAtlantis = new Dictionary<string, bool>();

        void AddSceneObject(string name, TrinitySceneObject sceneObject, CollisionComponent collisionComponent)
        {
            areaNames.Add(name);
            AreaInfos[name] = field.FindAreaInfo(name);

            var shape = collisionComponent.CollisionShape;
            Debug.Assert(shape.Discriminator is 2 or 4);

            if (shape is { Discriminator: 2, Item2: { } box })
            {
                // Box collision, obj.ObjectPosition.Field_02 is pos, box.Field_01 is size of box
                areaColBoxes[name] = new BoxCollision9
                {
                    Position = sceneObject.ObjectPosition.Field02,
                    Size = box.Field01,
                };
            }
            else if (shape is { Discriminator: 4, Item4: { } havok })
            {
                var havokData = ROM.GetPackedFile(havok.TrcolFilePath);
                areaColTrees[name] = HavokCollision.ParseAABBTree(havokData);
            }
        }

        foreach (var obj in area_management.Objects.Concat(a_w23_field_area_col.Objects))
        {
            var isAtlantisObj = a_w23_field_area_col.Objects.Contains(obj);
            if (!(obj.SubObjects.Count > 0 && obj.SubObjects[0].Type == "trinity_CollisionComponent"))
                continue;
            var collisionComponent = FlatBufferConverter.DeserializeFrom<CollisionComponent>(obj.SubObjects[0].Data);

            switch (obj.Type)
            {
                case "trinity_ObjectTemplate":
                {
                    var sObj = FlatBufferConverter.DeserializeFrom<TrinitySceneObjectTemplateData>(obj.Data);
                    if (sObj.Type != "trinity_SceneObject")
                        continue;
                    var sceneObject = FlatBufferConverter.DeserializeFrom<TrinitySceneObject>(sObj.Data);
                    Debug.Assert(sceneObject.ObjectName == sObj.ObjectTemplateExtra);

                    AddSceneObject(sObj.ObjectTemplateName, sceneObject, collisionComponent);
                    isAtlantis[sObj.ObjectTemplateName] = isAtlantisObj;
                    break;
                }
                case "trinity_SceneObject":
                {
                    var sceneObject = FlatBufferConverter.DeserializeFrom<TrinitySceneObject>(obj.Data);
                    AddSceneObject(sceneObject.ObjectName, sceneObject, collisionComponent);
                    isAtlantis[sceneObject.ObjectName] = isAtlantisObj;
                    break;
                }
            }
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
