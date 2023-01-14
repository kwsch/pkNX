using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using pkNX.Game;
using pkNX.Structures;
using pkNX.Structures.FlatBuffers;

namespace pkNX.WinForms.Subforms;

public partial class MapViewer9 : Form
{
    private readonly GameManagerSV ROM;
    private readonly bool Loading = true;
    private readonly List<string> AreaNames;
    private readonly Dictionary<string, AreaDef9> Areas;
    private readonly Dictionary<string, HavokCollision.AABBTree> AreaCollisionTrees;
    private readonly Dictionary<string, BoxCollision9> AreaCollisionBoxes;

    private readonly FieldMainArea[] MainAreas;
    private readonly FieldSubArea[] SubAreas;
    private readonly FieldInsideArea[] InsideAreas;
    private readonly FieldDungeonArea[] DungeonAreas;
    private readonly FieldLocation[] FieldLocations;

    public MapViewer9(GameManagerSV rom)
    {
        ROM = rom;

        MainAreas = FlatBufferConverter.DeserializeFrom<FieldMainAreaArray>(ROM.GetPackedFile("world/data/field/area/field_main_area/field_main_area_array.bin")).Table;
        SubAreas = FlatBufferConverter.DeserializeFrom<FieldSubAreaArray>(ROM.GetPackedFile("world/data/field/area/field_sub_area/field_sub_area_array.bin")).Table;
        InsideAreas = FlatBufferConverter.DeserializeFrom<FieldInsideAreaArray>(ROM.GetPackedFile("world/data/field/area/field_inside_area/field_inside_area_array.bin")).Table;
        DungeonAreas = FlatBufferConverter.DeserializeFrom<FieldDungeonAreaArray>(ROM.GetPackedFile("world/data/field/area/field_dungeon_area/field_dungeon_area_array.bin")).Table;
        FieldLocations = FlatBufferConverter.DeserializeFrom<FieldLocationArray>(ROM.GetPackedFile("world/data/field/area/field_location/field_location_array.bin")).Table;

        var area_management = FlatBufferConverter.DeserializeFrom<TrinitySceneObjectTemplateSV>(ROM.GetPackedFile(0x0573CA323061A2D3));

        AreaNames = new();
        Areas = new();
        AreaCollisionTrees = new();
        AreaCollisionBoxes = new();

        InitializeComponent();

        foreach (var obj in area_management.Objects)
        {
            if (obj.Type == "trinity_SceneObject" && obj.SubObjects.Length > 0 && obj.SubObjects[0].Type == "trinity_CollisionComponent")
            {
                var sceneObject = FlatBufferConverter.DeserializeFrom<TrinitySceneObjectSV>(obj.Data);
                var collisionComponent = FlatBufferConverter.DeserializeFrom<TrinityCollisionComponentSV>(obj.SubObjects[0].Data).Component.Item1;

                AreaNames.Add(sceneObject.ObjectName);
                var areaInfo = FindAreaInfo(sceneObject.ObjectName);

                if (collisionComponent.CollisionShape.TryGet(out TrinityCollisionShapeBoxSV? box))
                {
                    // Box collision, obj.ObjectPosition.Field_02 is pos, box.Field_01 is size of box
                    AreaCollisionBoxes[sceneObject.ObjectName] = new BoxCollision9
                    {
                        Position = sceneObject.ObjectPosition.Field_02,
                        Size = box.Field_01,
                    };
                }
                else if (collisionComponent.CollisionShape.TryGet(out TrinityCollisionShapeHavokSV? havok))
                {
                    var havokData = ROM.GetPackedFile(havok.TrcolFilePath);
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

        CB_Map.Items.AddRange(AreaNames.Select(z => z).ToArray());

        Loading = false;
        CB_Map.SelectedIndex = 0;
    }

    private AreaInfo FindAreaInfo(string name)
    {
        foreach (var area in MainAreas)
        {
            if (area.Name == name)
                return area.AreaInfo;
        }
        foreach (var area in SubAreas)
        {
            if (area.Name == name)
                return area.AreaInfo;
        }
        foreach (var area in InsideAreas)
        {
            if (area.Name == name)
                return area.AreaInfo;
        }
        foreach (var area in DungeonAreas)
        {
            if (area.Name == name)
                return area.AreaInfo;
        }
        foreach (var area in FieldLocations)
        {
            if (area.Name == name)
                return area.AreaInfo;
        }
        throw new ArgumentException($"Unknown area {name}");
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

        var areaName = AreaNames[map];

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

        if (AreaCollisionTrees.TryGetValue(areaName, out var colTree))
        {
            var brush = new SolidBrush(Color.FromArgb(25, 0, 0, 255));

            foreach (var rect in colTree.BoundingBoxRectangles)
            {
                gr.FillRectangle(brush, new RectangleF((float)ConvertX(rect.Left), (float)ConvertZ(rect.Top), (float)ConvertX(rect.Width), (float)ConvertX(rect.Height)));
            }
        }
        else if (AreaCollisionBoxes.TryGetValue(areaName, out var colBox))
        {
            var center_x = colBox.Position.X;
            var center_z = colBox.Position.Z;
            var size_x = colBox.Size.X;
            var size_z = colBox.Size.Z;

            var brush = new SolidBrush(Color.FromArgb(25, 0, 0, 255));
            gr.FillRectangle(brush, new RectangleF((float)ConvertX(center_x - (size_x / 2.0)), (float)ConvertZ(center_z - (size_z / 2.0)), (float)ConvertX(size_x), (float)ConvertX(size_z)));
        }
    }

    private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
    {
        var x = ((e.X * 5000.0f) / pictureBox1.Width);
        var z = ((e.Y * 5000.0f) / pictureBox1.Height) - 5500f;
        Text = $"X: {x}, Z: {z}";
    }
}

public class AreaDef9
{
    public required TrinitySceneObjectSV SceneObject;
    public required TrinityCollisionComponent1SV CollisionComponent;
    public required AreaInfo Info;
}
