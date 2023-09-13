using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
using System.Windows.Forms;
using pkNX.Game;
using pkNX.Structures;
using pkNX.Structures.FlatBuffers;
using pkNX.Structures.FlatBuffers.SV;
using pkNX.Structures.FlatBuffers.SV.Trinity;

namespace pkNX.WinForms.Subforms
{
    public class PaldeaMap
    {
        public readonly List<string>[] AreaNames = new List<string>[] { new(), new() };
        private readonly Dictionary<string, AreaDef9>[] Areas = new Dictionary<string, AreaDef9>[] { new(), new() };
        public readonly Dictionary<string, HavokCollision.AABBTree>[] AreaCollisionTrees = new Dictionary<string, HavokCollision.AABBTree>[] { new(), new() };
        public readonly Dictionary<string, BoxCollision9>[] AreaCollisionBoxes = new Dictionary<string, BoxCollision9>[] { new(), new() };

        private readonly IList<FieldMainArea>[] MainAreas = new IList<FieldMainArea>[2];
        private readonly IList<FieldSubArea>[] SubAreas = new IList<FieldSubArea>[2];
        private readonly IList<FieldInsideArea>[] InsideAreas = new IList<FieldInsideArea>[2];
        private readonly IList<FieldDungeonArea>[] DungeonAreas = new IList<FieldDungeonArea>[2];
        private readonly IList<FieldLocation>[] FieldLocations = new IList<FieldLocation>[2];

        public PaldeaMap(GameManagerSV ROM)
        {
            MainAreas[0] = FlatBufferConverter.DeserializeFrom<FieldMainAreaArray>(ROM.GetPackedFile("world/data/field/area/field_main_area/field_main_area_array.bin")).Table;
            SubAreas[0] = FlatBufferConverter.DeserializeFrom<FieldSubAreaArray>(ROM.GetPackedFile("world/data/field/area/field_sub_area/field_sub_area_array.bin")).Table;
            InsideAreas[0] = FlatBufferConverter.DeserializeFrom<FieldInsideAreaArray>(ROM.GetPackedFile("world/data/field/area/field_inside_area/field_inside_area_array.bin")).Table;
            DungeonAreas[0] = FlatBufferConverter.DeserializeFrom<FieldDungeonAreaArray>(ROM.GetPackedFile("world/data/field/area/field_dungeon_area/field_dungeon_area_array.bin")).Table;
            FieldLocations[0] = FlatBufferConverter.DeserializeFrom<FieldLocationArray>(ROM.GetPackedFile("world/data/field/area/field_location/field_location_array.bin")).Table;

            MainAreas[1] = FlatBufferConverter.DeserializeFrom<FieldMainAreaArray>(ROM.GetPackedFile("world/data/field/area_su1/field_main_area_su1/field_main_area_su1_array.bin")).Table;
            SubAreas[1] = FlatBufferConverter.DeserializeFrom<FieldSubAreaArray>(ROM.GetPackedFile("world/data/field/area_su1/field_sub_area_su1/field_sub_area_su1_array.bin")).Table;
            InsideAreas[1] = FlatBufferConverter.DeserializeFrom<FieldInsideAreaArray>(ROM.GetPackedFile("world/data/field/area_su1/field_inside_area_su1/field_inside_area_su1_array.bin")).Table;
            DungeonAreas[1] = FlatBufferConverter.DeserializeFrom<FieldDungeonAreaArray>(ROM.GetPackedFile("world/data/field/area_su1/field_dungeon_area_su1/field_dungeon_area_su1_array.bin")).Table;
            FieldLocations[1] = FlatBufferConverter.DeserializeFrom<FieldLocationArray>(ROM.GetPackedFile("world/data/field/area_su1/field_location_su1/field_location_su1_array.bin")).Table;

            LoadScenes(ROM);
        }

        private void LoadScenes(GameManagerSV ROM)
        {
            // Load default scenes
            LoadScene(ROM, ROM.GetPackedFile("world/scene/parts/field/resident_event/resident_area_collision_/resident_area_collision_0.trscn"), 0);

            // Load default scenes
            LoadScene(ROM, ROM.GetPackedFile(0x441FE0A17C85BEAA), 1);
        }

        private void LoadScene(GameManagerSV ROM, byte[] collisionFile, int index)
        {
            var area_collision = FlatBufferConverter.DeserializeFrom<TrinitySceneObjectTemplate>(collisionFile);

            foreach (var obj in area_collision.Objects)
            {
                if (obj is not { Type: "trinity_SceneObject", SubObjects.Count: > 0 })
                    continue;
                if (obj.SubObjects[0].Type != "trinity_CollisionComponent")
                    continue;

                var sceneObject = FlatBufferConverter.DeserializeFrom<TrinitySceneObject>(obj.Data);
                var trcData = obj.SubObjects[0].Data;
                var trc = FlatBufferConverter.DeserializeFrom<TrinityComponent>(trcData);
                var collisionComponent = trc.Component.CollisionComponent;

                AreaNames[index].Add(sceneObject.ObjectName);
                var areaInfo = FindAreaInfo(index, sceneObject.ObjectName);

                var shape = collisionComponent.Shape;
                if (shape.TryGet(out Box? box))
                {
                    // Box collision, obj.ObjectPosition.Field_02 is pos, box.Field_01 is size of box
                    AreaCollisionBoxes[index][sceneObject.ObjectName] = new BoxCollision9
                    {
                        Position = sceneObject.ObjectPosition.Field02,
                        Size = box!.Field01,
                    };
                }

                if (shape.TryGet(out Havok? havok))
                {
                    var havokData = ROM.GetPackedFile(havok!.TrcolFilePath);
                    AreaCollisionTrees[index][sceneObject.ObjectName] = HavokCollision.ParseAABBTree(havokData);
                }

                Areas[index][sceneObject.ObjectName] = new AreaDef9
                {
                    SceneObject = sceneObject,
                    CollisionComponent = collisionComponent,
                    Info = areaInfo,
                };
            }
        }

        public AreaInfo FindAreaInfo(int index, string name)
        {
            foreach (var area in MainAreas[index])
            {
                if (area.Name == name)
                    return area.Info;
            }
            foreach (var area in SubAreas[index])
            {
                if (area.Name == name)
                    return area.Info;
            }
            foreach (var area in InsideAreas[index])
            {
                if (area.Name == name)
                    return area.Info;
            }
            foreach (var area in DungeonAreas[index])
            {
                if (area.Name == name)
                    return area.Info;
            }
            foreach (var area in FieldLocations[index])
            {
                if (area.Name == name)
                    return area.Info;
            }
            throw new ArgumentException($"Unknown area {name}");
        }
    }

    public class AreaDef9
    {
        public required TrinitySceneObject SceneObject;
        public required CollisionComponent CollisionComponent;
        public required AreaInfo Info;
    }
}
