using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using pkNX.Game;
using pkNX.Structures;
using pkNX.Structures.FlatBuffers;
using pkNX.Structures.FlatBuffers.SV;
using pkNX.Structures.FlatBuffers.SV.Trinity;

namespace pkNX.WinForms.Subforms;

public partial class MapViewer9 : Form
{
    private readonly GameManagerSV ROM;
    private readonly bool Loading;
    private readonly PaldeaMap Map;

    public MapViewer9(GameManagerSV rom)
    {
        Map = new(ROM = rom);

        InitializeComponent();
        Loading = true;
        CB_Map.Items.AddRange(Map.AreaNames.Select(z => z).ToArray());
        Loading = false;
        CB_Map.SelectedIndex = 0;
    }

    private class ComboItem
    {
        public ComboItem(string text, int value)
        {
            Text = text;
            Value = value;
        }

        public string Text { get; }
        public int Value { get; }
    }

    private void CB_Map_SelectedIndexChanged(object sender, EventArgs e)
    {
        UpdateMap(CB_Map.SelectedIndex);
    }

    private static string GetMapImagePath()
    {
        return @"map_sv\paldea.png";
    }

    private void UpdateMap(int map)
    {
        if (Loading)
            return;

        var areaName = Map.AreaNames[map];

        var mapImagePath = System.IO.Path.GetFullPath(GetMapImagePath());
        if (System.IO.File.Exists(mapImagePath))
        {
            pictureBox1.BackgroundImage = Image.FromFile(mapImagePath);
        }
        else
        {
            WinFormsUtil.Error($"Unable to find map image at path: {mapImagePath}", "Automatic extraction of map image is not yet supported.\nYou will have to extract it manually and place them at the location above.");
            pictureBox1.BackgroundImage = new Bitmap(4096, 4096);
        }

        using var gr = Graphics.FromImage(pictureBox1.BackgroundImage);
        var r = new SolidBrush(Color.FromArgb(100, 255, 0, 0));
        var g = new SolidBrush(Color.FromArgb(20, 20, 255, 10));
        var b = new SolidBrush(Color.FromArgb(100, 10, 0, 255));
        var c = new SolidBrush(Color.FromArgb(100, 0, 255, 255));

        var rs = new Pen(Color.FromArgb(255, 255, 0, 0)) { Width = 3 };
        var gs = new Pen(Color.FromArgb(255, 20, 255, 10)) { Width = 3 };
        var bs = new Pen(Color.FromArgb(255, 10, 0, 255)) { Width = 3 };
        var cs = new Pen(Color.FromArgb(255, 10, 255, 255)) { Width = 3 };

        double ConvertX(double x) => (4096.0 / 5000.0) * x;
        double ConvertZ(double z) => (4096.0 / 5000.0) * (z + 5500.0);

        if (Map.AreaCollisionTrees.TryGetValue(areaName, out var colTree))
        {
            var brush = new SolidBrush(Color.FromArgb(25, 0, 0, 255));

            foreach (var rect in colTree.BoundingBoxRectangles)
            {
                gr.FillRectangle(brush, new RectangleF((float)ConvertX(rect.Left), (float)ConvertZ(rect.Top), (float)ConvertX(rect.Width), (float)ConvertX(rect.Height)));
            }
        }
        else if (Map.AreaCollisionBoxes.TryGetValue(areaName, out var colBox))
        {
            var center_x = colBox.Position.X;
            var center_z = colBox.Position.Z;
            var size_x = colBox.Size.X;
            var size_z = colBox.Size.Z;

            var brush = new SolidBrush(Color.FromArgb(25, 0, 0, 255));
            gr.FillRectangle(brush, new RectangleF((float)ConvertX(center_x - (size_x / 2.0)), (float)ConvertZ(center_z - (size_z / 2.0)), (float)ConvertX(size_x), (float)ConvertX(size_z)));
        }
    }

    private void Map_MouseMove(object sender, MouseEventArgs e)
    {
        var x = ((e.X * 5000.0f) / pictureBox1.Width);
        var z = ((e.Y * 5000.0f) / pictureBox1.Height) - 5500f;
        Text = $"X: {x}, Z: {z}";
    }
}

public class PaldeaMap
{
    public readonly List<string> AreaNames = new();
    private readonly Dictionary<string, AreaDef9> Areas = new();
    public readonly Dictionary<string, HavokCollision.AABBTree> AreaCollisionTrees = new();
    public readonly Dictionary<string, BoxCollision9> AreaCollisionBoxes = new();

    private readonly IList<FieldMainArea> MainAreas;
    private readonly IList<FieldSubArea> SubAreas;
    private readonly IList<FieldInsideArea> InsideAreas;
    private readonly IList<FieldDungeonArea> DungeonAreas;
    private readonly IList<FieldLocation> FieldLocations;

    public PaldeaMap(GameManagerSV ROM)
    {
        MainAreas = FlatBufferConverter.DeserializeFrom<FieldMainAreaArray>(ROM.GetPackedFile("world/data/field/area/field_main_area/field_main_area_array.bin")).Table;
        SubAreas = FlatBufferConverter.DeserializeFrom<FieldSubAreaArray>(ROM.GetPackedFile("world/data/field/area/field_sub_area/field_sub_area_array.bin")).Table;
        InsideAreas = FlatBufferConverter.DeserializeFrom<FieldInsideAreaArray>(ROM.GetPackedFile("world/data/field/area/field_inside_area/field_inside_area_array.bin")).Table;
        DungeonAreas = FlatBufferConverter.DeserializeFrom<FieldDungeonAreaArray>(ROM.GetPackedFile("world/data/field/area/field_dungeon_area/field_dungeon_area_array.bin")).Table;
        FieldLocations = FlatBufferConverter.DeserializeFrom<FieldLocationArray>(ROM.GetPackedFile("world/data/field/area/field_location/field_location_array.bin")).Table;

        LoadScene(ROM);
    }

    private void LoadScene(GameManagerSV ROM)
    {
        var area_management = FlatBufferConverter.DeserializeFrom<TrinitySceneObjectTemplate>(ROM.GetPackedFile("world/scene/parts/field/resident_event/resident_area_collision_/resident_area_collision_0.trscn"));

        foreach (var obj in area_management.Objects)
        {
            if (obj is not { Type: "trinity_SceneObject", SubObjects.Count: > 0 })
                continue;
            if (obj.SubObjects[0].Type != "trinity_CollisionComponent")
                continue;

            var sceneObject = FlatBufferConverter.DeserializeFrom<TrinitySceneObject>(obj.Data);
            var trcData = obj.SubObjects[0].Data;
            var trc = FlatBufferConverter.DeserializeFrom<TrinityComponent>(trcData);
            var collisionComponent = trc.Component.CollisionComponent;

            AreaNames.Add(sceneObject.ObjectName);
            var areaInfo = FindAreaInfo(sceneObject.ObjectName);

            var shape = collisionComponent.Shape;
            if (shape.TryGet(out Box? box))
            {
                // Box collision, obj.ObjectPosition.Field_02 is pos, box.Field_01 is size of box
                AreaCollisionBoxes[sceneObject.ObjectName] = new BoxCollision9
                {
                    Position = sceneObject.ObjectPosition.Field02,
                    Size = box!.Field01,
                };
            }

            if (shape.TryGet(out Havok? havok))
            {
                var havokData = ROM.GetPackedFile(havok!.TrcolFilePath);
                AreaCollisionTrees[sceneObject.ObjectName] = HavokCollision.ParseAABBTree(havokData);
            }

            Areas[sceneObject.ObjectName] = new AreaDef9
            {
                SceneObject = sceneObject,
                CollisionComponent = collisionComponent,
                Info = areaInfo,
            };
        }
    }

    public AreaInfo FindAreaInfo(string name)
    {
        foreach (var area in MainAreas)
        {
            if (area.Name == name)
                return area.Info;
        }
        foreach (var area in SubAreas)
        {
            if (area.Name == name)
                return area.Info;
        }
        foreach (var area in InsideAreas)
        {
            if (area.Name == name)
                return area.Info;
        }
        foreach (var area in DungeonAreas)
        {
            if (area.Name == name)
                return area.Info;
        }
        foreach (var area in FieldLocations)
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
