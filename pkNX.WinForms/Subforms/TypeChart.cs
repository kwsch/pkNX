using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using pkNX.Game;

namespace pkNX.WinForms;

public sealed partial class TypeChart : Form
{
    private readonly string[] types;
    private readonly TypeChartEditor Editor;

    public byte[] Chart;

    public bool Modified { get; set; }
    private const int TypeWidth = 32; // px
    private int TypeCount => types.Length;

    public TypeChart(TypeChartEditor editor, string[] types)
    {
        Editor = editor;
        InitializeComponent();
        this.types = types;
        Chart = editor.Data;
        LoadChart();
    }

    private void B_Save_Click(object sender, EventArgs e)
    {
        Modified = true;
        SaveChart();
        Close();
    }

    private void SaveChart() => Chart = Editor.Data;

    private void B_RTM_Click(object sender, EventArgs e)
    {
        Editor.Randomize();
        Chart = Editor.Data;
        LoadChart();
        System.Media.SystemSounds.Asterisk.Play();
    }

    private void LoadChart() => PB_Chart.Image = GetGrid(Chart, TypeWidth, TypeCount);

    // gui logic below

    private void MoveMouse(object sender, MouseEventArgs e)
    {
        GetCoordinate((PictureBox)sender, e, out int X, out int Y);
        int index = (Y * TypeCount) + X;
        if (index >= Chart.Length)
            return;
        UpdateLabel(X, Y, Chart[index]);
    }

    private void ClickMouse(object sender, MouseEventArgs e)
    {
        GetCoordinate((PictureBox)sender, e, out int X, out int Y);
        int index = (Y * TypeCount) + X;
        if (index >= Chart.Length)
            return;

        Chart[index] = ToggleEffectiveness(Chart[index], e.Button == MouseButtons.Left);

        UpdateLabel(X, Y, Chart[index]);
        LoadChart();
    }

    private void UpdateLabel(int X, int Y, int value)
    {
        if (value >= effects.Length || X >= types.Length || Y >= types.Length)
            return; // clicking and moving outside the box has invalid values
        L_Hover.Text = $"[{X:00}x{Y:00}: {value:00}] {types[Y]} attacking {types[X]} {effects[value]}";
    }

    private readonly string[] effects =
    {
        "has no effect!",
        "",
        "is not very effective.",
        "",
        "does regular damage.",
        "",
        "",
        "",
        "is super effective!",
    };

    public static void GetCoordinate(Control sender, MouseEventArgs e, out int X, out int Y)
    {
        X = e.X / TypeWidth;
        Y = e.Y / TypeWidth;
        if (e.X == sender.Width - 1 - 2) // tweak because the furthest pixel is unused for transparent effect, and 2 px are used for border
            X--;
        if (e.Y == sender.Height - 1 - 2)
            Y--;
    }

    public static byte ToggleEffectiveness(byte currentValue, bool increase)
    {
        byte[] vals = { 0, 2, 4, 8 };
        int curIndex = Array.IndexOf(vals, currentValue);
        if (curIndex < 0)
            return currentValue;

        uint shift = (uint)(curIndex + (increase ? 1 : -1));
        var newIndex = shift % vals.Length;
        return vals[newIndex];
    }

    public static Bitmap GetGrid(byte[] vals, int itemsize, int itemsPerRow)
    {
        // set up image
        var bmpData = TypeChartEditor.GetTypeChartImageData(itemsize, itemsPerRow, vals, out int width, out int height);
        return CreateImage(width, height, bmpData);
    }

    private static Bitmap CreateImage(int width, int height, byte[] bmpData)
    {
        // assemble image
        var b = new Bitmap(width, height, PixelFormat.Format32bppArgb);
        var bData = b.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
        System.Runtime.InteropServices.Marshal.Copy(bmpData, 0, bData.Scan0, bmpData.Length);
        b.UnlockBits(bData);
        return b;
    }
}
