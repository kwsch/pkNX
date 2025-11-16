using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Numerics;
using System.Text.Json;
using System.Windows.Forms;
using pkNX.Game;
using pkNX.Structures;
using pkNX.Structures.FlatBuffers;
using static pkNX.WinForms.LumioseFieldIndex;
using Point = pkNX.WinForms.TransformScale;

namespace pkNX.WinForms.Subforms;

public partial class MapViewer9a : Form
{
    private readonly GameManager9a ROM;
    private readonly LumioseMap Map;
    private readonly World9a Maps;
    private readonly SpawnRipper9a Spawners;
    private bool Loading;

    private bool HasWarnedUserNoFilesFound;

    private readonly List<SceneSpawner> RenderedSpawns = [];
    private CoordinateInfo9a? Current;

    private LumioseFieldIndex MapIndex => CB_Map.SelectedIndex != -1 ? (LumioseFieldIndex)CB_Map.SelectedIndex : 0;

    private MapTransform CurrentTransform => TransformUI9a.GetMapTransform(MapIndex);

    public MapViewer9a(GameManager9a rom, string language)
    {
        ROM = rom;
        Maps = new(rom);
        Spawners = new(rom, language, "", Maps);
        Map = Maps.Overworld;

        InitializeComponent();

        var species = GetSpeciesList(rom, language);

        Loading = true;
        CB_Species.DisplayMember = nameof(ComboItem.Text);
        CB_Species.ValueMember = nameof(ComboItem.Value);
        CB_Species.DataSource = new BindingSource(species, string.Empty);
        CB_Map.Items.AddRange("Lumiose", "Lysandre Labs", "The Sewers Ch5", "The Sewers Ch6");
        ReloadAreaIndexes(CB_Map.SelectedIndex = 0);
        CB_Species.SelectedValue = -1; // None
        Loading = false;

        PB_Map.Click += ClickMap;

        LoadMap();
    }

    private List<ComboItem> GetSpeciesList(GameManager9a rom, string language)
    {
        var cfg = new TextConfig(rom.Game);
        var speciesNames = GetCommonText("monsname", language, cfg);
        var species = speciesNames.Select((z, i) => new ComboItem(z, i)).ToList();

        var valid = new HashSet<int>(Spawners.Shared.Encounters.Select(z => (int)z.Value.DevNo));
        species.RemoveAll(z => !valid.Contains(z.Value));

        species.Insert(0, new ComboItem("(All)", 0));
        species.Insert(0, new ComboItem("(None)", -1));
        return species;
    }

    private sealed record ComboItem(string Text, int Value);

    private string[] GetCommonText(string name, string lang, TextConfig cfg)
    {
        var data = ROM.GetPackedFile($"ik_message/dat/{lang}/common/{name}.dat");
        return new TextFile(data, cfg).Lines;
    }

    private void ChangeMap(object sender, EventArgs e)
    {
        if (Loading)
            return;
        Loading = true;
        ReloadAreaIndexes(CB_Map.SelectedIndex);
        Loading = false;
    }

    private void ReloadAreaIndexes(int map)
    {
        CB_AreaIndex.Items.Clear();
        if (map == 0)
            CB_AreaIndex.Items.AddRange(Map.AreaNames.Select(z => z).ToArray());
        else
            CB_AreaIndex.Items.Add("Dungeon");
        CB_AreaIndex.SelectedIndex = 0;
    }

    private void ChangeAreaIndex(object sender, EventArgs e)
    {
        if (Loading)
            return;
        LoadMap();
    }

    private const string mapFolderName = "map_za"; // folder we expect the user to place map png's in

    private static string GetMapImagePath(LumioseFieldIndex field) => field switch
    {
        Overworld => $@"{mapFolderName}\lumiose.png",
        LysandreLabs => $@"{mapFolderName}\lysandre_labs.png",
        SewersCh5 => $@"{mapFolderName}\the_sewers_ch5.png",
        SewersCh6 => $@"{mapFolderName}\the_sewers_ch6.png",
        _ => throw new ArgumentException($"Unknown field {field}"),
    };

    private void B_ExportImage_Click(object sender, EventArgs e)
    {
        if (ModifierKeys == Keys.Alt)
        {
            // Open folder
            var path = System.IO.Path.GetFullPath(GetMapImagePath(MapIndex));
            var folder = System.IO.Path.GetDirectoryName(path);
            if (folder != null && System.IO.Directory.Exists(folder))
                System.Diagnostics.Process.Start("explorer.exe", folder);
            return;
        }

        var index = CB_AreaIndex.SelectedIndex;
        var suffix = MapIndex switch
        {
            Overworld => Map.AreaNames[index],
            _ => "Dungeon",
        };
        if (CHK_ExportImages.Checked)
            ExportImage(suffix);
    }

    private void ExportImage(string suffix)
    {
        var img = PB_Map.BackgroundImage;
        var path = GetMapImagePath(MapIndex);
        var fileName = $"{path.Replace(".png", "")}_{suffix}.png";
        img?.Save(fileName);
    }

    private int AreaIndex => CB_AreaIndex.SelectedIndex;

    private void LoadMap()
    {
        var tr = CurrentTransform;
        var mapImagePath = System.IO.Path.GetFullPath(GetMapImagePath(MapIndex));
        var img = GetBackground(mapImagePath, tr);
        PB_Map.BackgroundImage = img;

        using var gr = Graphics.FromImage(img);
        string fileSuffix;

        var coords = GetCoordinatesToRender(MapIndex);
        RenderPoints(gr, tr, Brushes.Red, coords);

        var areaIndex = AreaIndex;
        if (areaIndex < 0)
            areaIndex = 0;

        if (MapIndex == 0) // Overworld
        {
            var areaName = Map.AreaNames[areaIndex];
            if (Map.AreaCollisionTrees.TryGetValue(areaName, out var colTree))
                RenderTree(tr, gr, colTree);
            else if (Map.AreaCollisionBoxes.TryGetValue(areaName, out var colBox))
                RenderBox(tr, gr, colBox);

            fileSuffix = areaName;
            if (CHK_ExportImages.Checked)
                ExportImage(areaName);
        }
        else
        {
            // Nothing rendered at this point.
            fileSuffix = "Dungeon";
        }

        RenderSpawns(gr, tr);

        if (CHK_ExportImages.Checked)
            ExportImage(fileSuffix);
    }

    private Image GetBackground(string mapImagePath, MapTransform tr)
    {
        if (System.IO.File.Exists(mapImagePath))
            return Image.FromFile(mapImagePath);

        if (!HasWarnedUserNoFilesFound)
        {
            WinFormsUtil.Error($"Unable to find map image at path: {mapImagePath}", "Automatic extraction of map image is not yet supported.\nYou will have to extract it manually and place them at the location above.");
            HasWarnedUserNoFilesFound = true;
        }
        return new Bitmap((int)tr.Texture.X, (int)tr.Texture.Z);
    }

    private void RenderBox(MapTransform tr, Graphics gr, BoxCollision9 colBox)
    {
        var center_x = colBox.Position.X;
        var center_z = colBox.Position.Z;
        var size_x = colBox.Size.X;
        var size_z = colBox.Size.Z;

        using var brush = new SolidBrush(Color.FromArgb((int)NUD_CollisionTransparency.Value, Color.Cyan));
        var x = (float)tr.ConvertX(center_x - (tr.Dir.X * (size_x / 2.0f)));
        var z = (float)tr.ConvertZ(center_z - (tr.Dir.Z * (size_z / 2.0f)));
        var w = (float)tr.ConvertWidth(size_x);
        var d = (float)tr.ConvertHeight(size_z);
        gr.FillRectangle(brush, new RectangleF(x, z, w, d));
    }

    private void RenderTree(MapTransform tr, Graphics gr, MultiAABBTree colTree)
    {
        foreach (var tree in colTree)
        {
            foreach (var node in tree.Nodes)
            {
                if (!node.IsLeaf)
                    continue;

                for (var i = 0; i < HavokCollision.hkcdSimdTreeNode.NodeCount; i++)
                {
                    if (!node.IsBound(i))
                        continue;
                    var lo = new Vector3(node.LoX[i], node.LoY[i], node.LoZ[i]);
                    var hi = new Vector3(node.HiX[i], node.HiY[i], node.HiZ[i]);

                    var yh = node.HiY[i] + 6;
                    var y = node.LoY[i] + 6;
                    var color1 = Color.FromArgb((int)NUD_CollisionTransparency.Value, 0, Math.Clamp((int)(y  / 30.0f * 255), 0, 255), Math.Clamp(255 - (int)(y / 30.0f * 255), 0, 255));
                    var color2 = Color.FromArgb((int)NUD_CollisionTransparency.Value, 0, Math.Clamp((int)(yh / 01.0f * 255), 0, 255), Math.Clamp(255 - (int)(yh / 1.0f * 255), 0, 255));

                    using var brush = new HatchBrush(HatchStyle.BackwardDiagonal, color1, color2);
                    var rect = new HavokCollision.Rectangle3D(lo, hi);
                    var x = (float)tr.ConvertX(rect.X + rect.Width);
                    var z = (float)tr.ConvertZ(rect.Z + rect.Depth);
                    var w = (float)tr.ConvertWidth(rect.Width);
                    var d = (float)tr.ConvertHeight(rect.Depth);

                    gr.FillRectangle(brush, new RectangleF(x, z, w, d));
                }
            }

            for (var i = 0; i < tree.BoundingBoxRectangles.Length; i++)
            {
                var rect = tree.BoundingBoxRectangles[i];
            }
        }
    }

    public static Color GetSpeciesColor(int species)
    {
        var mash = (0x41C64E6D * (uint)species) + 0x00006073;
        var r = (byte)((mash >> 16) & 0xFF);
        var g = (byte)((mash >> 8) & 0xFF);
        var b = (byte)(mash & 0xFF);
        return Color.FromArgb(r, g, b);
    }

    private void RenderSpawns(Graphics gr, MapTransform tr)
    {
        RenderedSpawns.Clear();
        var speciesFilter = WinFormsUtil.GetIndex(CB_Species);
        if (speciesFilter == -1)
            return;

        var nameFilter = TB_ObjectName.Text.Trim();
        var pointFilter = TB_PointName.Text.Trim();

        var areaIndex = AreaIndex;
        var mapIndex = (int)MapIndex;

        var map = Spawners.RippedMaps[mapIndex];
        foreach (var (name, point) in map.SpawnerPositions)
        {
            if (pointFilter.Length != 0 && !name.Contains(pointFilter))
                continue;

            if (!Spawners.TryGetSpawner(name, out var spawn))
                continue;

            bool rendered = false;
            var position = point.Position;
            foreach (var edi in spawn.EncountDataInfoList)
            {
                if (edi.EncountDataId is not { } id)
                    continue;
                if (!Spawners.TryGetEncounter(id, out var slot))
                    continue;

                var species = slot.DevNo;
                if (speciesFilter != 0 && speciesFilter != (int)species)
                    continue;

                var color = GetSpeciesColor((int)species);
                using var brush = new SolidBrush(color);
                RenderPoints(gr, tr, brush, (position.X, position.Z));
                rendered = true;
            }
            if (rendered)
                RenderedSpawns.Add(point);
        }
    }

    private static void RenderPoints(Graphics gr, MapTransform tr, Brush brush, params ReadOnlySpan<Point> coords)
    {
        foreach (var pt in coords)
        {
            const int length = (10 * 2) + 1; // Diameter of the circle
            var x = (float)tr.ConvertX(pt.X) - (length / 2.0f);
            var y = (float)tr.ConvertZ(pt.Z) - (length / 2.0f);
            gr.FillEllipse(brush, x, y, length, length);
        }
    }

    private static Point[] GetCoordinatesToRender(LumioseFieldIndex mapIndex) => mapIndex switch
    {
        LysandreLabs => [(-11.009529, 92.041016), (21.560417, 94.00054), (-67.99764, 52.998158)],
        SewersCh5 => [(-94.78022, -129.65295), (89.97959, -49.889454), (76.90937, -149.78218)],
        SewersCh6 => [(-74.71503, -17.543823), (-65.31848, -42.548283), (-83.20368, -90.00267)],
        _ => [new(420, 420)],
    };

    private void Map_MouseMove(object sender, MouseEventArgs e)
    {
        var snap = false;
        if (ModifierKeys == Keys.Alt)
            snap = true;
        else if (ModifierKeys != Keys.None)
            return;

        var tr = CurrentTransform;
        var (x, z) = tr.ScreenToWorld(e.X, e.Y, PB_Map.Width, PB_Map.Height);

        var info = GetCoordinateInfo(x, z);
        var title = $"{info.LocationName} X: {x:F2}, Z: {z:F2}";
        if (info.Spawner is { } spawner)
            title += $" - Spawner: {spawner.Name} ({spawner.Type})";
        Text = title;

        RTB_Analysis.Text = JsonSerializer.Serialize(info, SpawnExporter.Options);

        Current = info;
        if (snap)
            SnapAreaToCurrent();
    }

    private CoordinateInfo9a GetCoordinateInfo(float x, float z)
    {
        var info = new CoordinateInfo9a
        {
            X = x,
            Z = z,
        };
        info.PlaceName = Spawners.Overworld.GetPlaceName(x, z, out var areaName);
        info.AreaName = areaName;
        info.LocationIndex = Spawners.Resolver.GetLocationIndex(info.PlaceName);
        info.LocationName = info.LocationIndex == -1 ? "Out of Bounds" : Spawners.GetLocationName(info.LocationIndex);
        info.Collisions = Map.GetCollisions(x, z, Spawners.Resolver)
            .OrderByDescending(a => a.Valid)
            .ThenByDescending(tree => tree.Layer)
            .ToArray();

        if (TryGetSceneSpawner((x, z), out var spawner))
            info.Spawner = spawner;
        return info;
    }

    public record CoordinateInfo9a
    {
        public required float X { get; init; }
        public required float Z { get; init; }

        public string AreaName { get; set; } = "";
        public string PlaceName { get; set; } = "";
        public string LocationName { get; set; } = "";
        public SceneSpawner? Spawner { get; set; }
        public int LocationIndex { get; set; }
        public IntersectingTree[] Collisions { get; set; } = [];

        public string? GetArea()
        {
            if (Collisions.Length == 0)
                return null;
            var info = Collisions[0].GetAreaInternal();
            return info;
        }
    }

    private void B_ExportSpawnsToClipboard_Click(object sender, EventArgs e) => WinFormsUtil.Alert("Not implemented.");

    private void ChangeSpawnFilter(object sender, EventArgs e)
    {
        if (Loading)
            return;
        LoadMap();
    }

    private bool TryGetSceneSpawner(Point position, [NotNullWhen(true)] out SceneSpawner? result)
    {
        const float minResult = 5.0f;
        result = null;
        var minDistance = float.MaxValue;
        foreach (var spawner in RenderedSpawns)
        {
            var sp = spawner.Position;
            var distance = sp.DistanceTo((float)position.X, (float)position.Z);
            if (distance > minResult)
                continue;
            if (distance >= minDistance)
                continue;
            minDistance = distance;
            result = spawner;
        }
        return result != null;
    }

    private void ClickMap(object? sender, EventArgs e) => SnapAreaToCurrent();

    private void SnapAreaToCurrent()
    {
        if (MapIndex != 0)
            return;

        if (Current is not { } c)
            return;

        // Set the current area to the clicked location's area
        var areaInfo = c.GetArea();
        if (areaInfo is null)
            return;

        var index = Math.Max(0, Map.AreaNames.IndexOf(areaInfo));
        if (index != CB_AreaIndex.SelectedIndex)
            CB_AreaIndex.SelectedIndex = index;
    }
}
