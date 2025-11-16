using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using pkNX.Game;

namespace pkNX.WinForms.Subforms;

public partial class MapViewer9 : Form
{
    private readonly GameManagerSV ROM;
    private readonly PaldeaMap Map;
    private bool Loading;

    private int FieldIndex => CB_Field.SelectedIndex != -1 ? CB_Field.SelectedIndex : 0;

    public MapViewer9(GameManagerSV rom)
    {
        Map = new(ROM = rom);

        InitializeComponent();
        Loading = true;
        CB_Field.Items.AddRange("Paldea", "Kitakami", "Terarium");
        Loading = false;
        CB_Field.SelectedIndex = 0;
    }

    private double ScaleX => FieldIndex switch
    {
        0 => 5000,
        1 => 2000,
        2 => 2000,
        _ => throw new ArgumentOutOfRangeException(nameof(FieldIndex)),
    };

    private double ScaleZ => FieldIndex switch
    {
        0 => 5000,
        1 => 2000,
        2 => 2000,
        _ => throw new ArgumentOutOfRangeException(nameof(FieldIndex)),
    };

    private double OffsetZ => FieldIndex switch
    {
        0 => 5500,
        1 => 2000,
        2 => 2000,
        _ => throw new ArgumentOutOfRangeException(nameof(FieldIndex)),
    };

    private double ConvertWidth(double s) => (4096.0 / ScaleX) * s;
    private double ConvertHeight(double s) => (4096.0 / ScaleZ) * s;
    private double ConvertX(double x) => (4096.0 / ScaleX) * x;
    private double ConvertZ(double z) => (4096.0 / ScaleZ) * (z + OffsetZ);

    private void CB_Field_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Loading)
            return;
        Loading = true;
        CB_Map.Items.Clear();
        CB_Map.Items.AddRange(Map.AreaNames[FieldIndex].Select(z => z).ToArray());
        Loading = false;
        CB_Map.SelectedIndex = 0;
    }

    private void CB_Map_SelectedIndexChanged(object sender, EventArgs e)
    {
        UpdateMap(CB_Map.SelectedIndex);
    }

    private static string GetMapImagePath(int field) => field switch
    {
        0 => @"map_sv\paldea.png",
        1 => @"map_sv\kitakami.png",
        2 => @"map_sv\terarium.png",
        _ => throw new ArgumentException($"Unknown field {field}"),
    };

    private void UpdateMap(int map)
    {
        if (Loading)
            return;

        var areaName = Map.AreaNames[FieldIndex][map];

        var mapImagePath = System.IO.Path.GetFullPath(GetMapImagePath(FieldIndex));
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
        // var r = new SolidBrush(Color.FromArgb(100, 255, 0, 0));
        // var g = new SolidBrush(Color.FromArgb(20, 20, 255, 10));
        // var b = new SolidBrush(Color.FromArgb(100, 10, 0, 255));
        // var c = new SolidBrush(Color.FromArgb(100, 0, 255, 255));
        // var rs = new Pen(Color.FromArgb(255, 255, 0, 0)) { Width = 3 };
        // var gs = new Pen(Color.FromArgb(255, 20, 255, 10)) { Width = 3 };
        // var bs = new Pen(Color.FromArgb(255, 10, 0, 255)) { Width = 3 };
        // var cs = new Pen(Color.FromArgb(255, 10, 255, 255)) { Width = 3 };
        if (Map.AreaCollisionTrees[FieldIndex].TryGetValue(areaName, out var colTree))
        {
            using var brush = new SolidBrush(Color.FromArgb(25, 0, 0, 255));

            foreach (var rect in colTree.BoundingBoxRectangles)
            {
                gr.FillRectangle(brush, new RectangleF((float)ConvertX(rect.X), (float)ConvertZ(rect.Z), (float)ConvertWidth(rect.Width), (float)ConvertHeight(rect.Depth)));
            }
        }
        else if (Map.AreaCollisionBoxes[FieldIndex].TryGetValue(areaName, out var colBox))
        {
            var center_x = colBox.Position.X;
            var center_z = colBox.Position.Z;
            var size_x = colBox.Size.X;
            var size_z = colBox.Size.Z;

            using var brush = new SolidBrush(Color.FromArgb(25, 0, 0, 255));
            gr.FillRectangle(brush, new RectangleF((float)ConvertX(center_x - (size_x / 2.0)), (float)ConvertZ(center_z - (size_z / 2.0)), (float)ConvertWidth(size_x), (float)ConvertHeight(size_z)));
        }
        pictureBox1.BackgroundImage.Save($"{GetMapImagePath(FieldIndex).Replace(".png", "")}_{areaName}.png");
    }

    private void Map_MouseMove(object sender, MouseEventArgs e)
    {
        var x = ((e.X * ScaleX) / pictureBox1.Width);
        var z = ((e.Y * ScaleZ) / pictureBox1.Height) - OffsetZ;
        Text = $"X: {x}, Z: {z}";
    }
}
