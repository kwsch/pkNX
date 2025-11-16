using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using pkNX.Game;

namespace pkNX.WinForms;

public sealed partial class TypeChart : Form
{
    private readonly string[] TypeNames;
    private readonly TypeChartEditor Editor;

    public Span<byte> Chart => Editor.Data;

    public bool Modified { get; set; }
    private const int TypeWidth = 32; // px
    private int TypeCount => TypeNames.Length;

    public TypeChart(TypeChartEditor editor, string[] typeNames)
    {
        InitializeComponent();

        Editor = editor;
        TypeNames = typeNames;
        LoadChart();
    }

    private void B_Save_Click(object sender, EventArgs e)
    {
        Modified = true;
        Close();
    }

    private void B_RTM_Click(object sender, EventArgs e)
    {
        Editor.Randomize();
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
        if (value >= effects.Length || X >= TypeNames.Length || Y >= TypeNames.Length)
            return; // clicking and moving outside the box has invalid values
        L_Hover.Text = $"[{X:00}x{Y:00}: {value:00}] {TypeNames[Y]} attacking {TypeNames[X]} {effects[value]}";
    }

    private readonly string[] effects =
    [
        "has no effect!",
        "",
        "is not very effective.",
        "",
        "does regular damage.",
        "",
        "",
        "",
        "is super effective!",
    ];

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
        byte[] vals = [0, 2, 4, 8];
        int curIndex = Array.IndexOf(vals, currentValue);
        if (curIndex < 0)
            return currentValue;

        uint shift = (uint)(curIndex + (increase ? 1 : -1));
        var newIndex = shift % vals.Length;
        return vals[newIndex];
    }

    public static Bitmap GetGrid(ReadOnlySpan<byte> vals, int itemsize, int itemsPerRow)
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
