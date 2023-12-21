using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using FlatSharp;
using pkNX.Containers;
using pkNX.Game;
using pkNX.Structures;
using pkNX.Structures.FlatBuffers;
using pkNX.Structures.FlatBuffers.Arceus;
using EncounterSlot = pkNX.Structures.FlatBuffers.Arceus.EncounterSlot;
using Species = pkNX.Structures.Species;

namespace pkNX.WinForms.Subforms;

public partial class MapViewer8a : Form
{
    private readonly GameManagerPLA ROM;
    private readonly GFPack Resident;
    private readonly AreaSettingsTable Settings;

    public readonly AreaInstance[] Areas;
    private readonly bool Loading = true;

    public MapViewer8a(GameManagerPLA rom, GFPack resident)
    {
        ROM = rom;
        Resident = resident;
        var bin_settings = resident.GetDataFullPath("bin/field/resident/AreaSettings.bin");
        Settings = FlatBufferConverter.DeserializeFrom<AreaSettingsTable>(bin_settings);

        InitializeComponent();

        Areas = ResidentAreaSet.AreaNames.Select(z => AreaInstance.Create(Resident, z, Settings)).ToArray();
        CB_Map.Items.AddRange(Areas.Select(z => z.ParentArea?.FriendlyAreaName ?? z.FriendlyAreaName).ToArray());

        var speciesNames = ROM.GetStrings(TextName.SpeciesNames);
        var pt = rom.Data.PersonalData;
        var nameList = new List<ComboItem>();
        foreach (var e in pt.Table.Cast<IPersonalMisc_SWSH>())
        {
            if (!e.IsPresentInGame)
                continue;

            var species = e.DexIndexNational;
            if (nameList.All(z => z.Value != species))
                nameList.Add(new(speciesNames[species], species));
        }

        nameList.Insert(0, new("(All)", -1));
        nameList.Sort((x, y) => string.Compare(x.Text, y.Text, StringComparison.InvariantCulture));

        CB_Species.DisplayMember = nameof(ComboItem.Text);
        CB_Species.ValueMember = nameof(ComboItem.Value);
        CB_Species.DataSource = new BindingSource(nameList, null);

        CB_Species.SelectedValue = -1;
        Loading = false;
        CB_Map.SelectedIndex = 1;
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
        var index = WinFormsUtil.GetIndex(CB_Species);
        UpdateMap(CB_Map.SelectedIndex, index);
    }

    private void CB_Species_SelectedIndexChanged(object sender, EventArgs e)
    {
        var index = WinFormsUtil.GetIndex(CB_Species);
        UpdateMap(CB_Map.SelectedIndex, index);
    }

    private List<AreaDef> Defs = [];

    private static string GetMapImagePath(AreaInstance area)
    {
        string mapName = MapName(area.AreaName);
        return $"map_pla\\{mapName}.png";
    }

    private static string MapName(string name) => name switch
    {
        "ha_area00" => "map_lmap_pic_05_05",
        "ha_area01" => "map_lmap_pic_00",
        "ha_area02" => "map_lmap_pic_01",
        "ha_area03" => "map_lmap_pic_02",
        "ha_area04" => "map_lmap_pic_03",
        "ha_area05" => "map_lmap_pic_04",
        "ha_area06" => "map_lmap_pic_06",
        _ => name,
    };

    private void UpdateMap(int map, int species)
    {
        if (Loading)
            return;

        var area = Areas[map];
        var mapImagePath = System.IO.Path.GetFullPath(GetMapImagePath(area));
        if (System.IO.File.Exists(mapImagePath))
        {
            pictureBox1.BackgroundImage = Image.FromFile(mapImagePath);
        }
        else
        {
            WinFormsUtil.Error($"Unable to find map image at path: {mapImagePath}", "Automatic extraction of the map images is not yet supported.\nYou will have to extract them manually and place them at the location above. (You can use Switch-Toolbox for this.)");
            pictureBox1.BackgroundImage = new Bitmap(1024, 1024);
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

        var coordinates = Defs = GetSpawnerInfo(species, area);
        foreach (var o in coordinates)
        {
            var brush = o.Type switch
            {
                SpawnerType.Spawner => r,
                SpawnerType.Wormhole => g,
                SpawnerType.Landmark => b,
                SpawnerType.Unown => c,
                _ => throw new ArgumentOutOfRangeException(nameof(o.Type)),
            };
            var penS = o.Type switch
            {
                SpawnerType.Spawner => rs,
                SpawnerType.Wormhole => gs,
                SpawnerType.Landmark => bs,
                SpawnerType.Unown => cs,
                _ => throw new ArgumentOutOfRangeException(nameof(o.Type)),
            };

            var center = o.Position;
            var radius = o.Radius;

            var x = center.X - radius;
            var y = center.Z - radius;
            var d = radius * 2;
            gr.FillEllipse(brush, x, y, d, d);
            gr.DrawEllipse(penS, x, y, d, d);
        }
    }

    private static List<AreaDef> GetSpawnerInfo(int species, AreaInstance area)
    {
        var result = new List<AreaDef>();

        foreach (var s in area.Spawners.Concat(area.SubAreas.SelectMany(z => z.Spawners)))
        {
            var table = s.Field20Value.EncounterTableID;
            var slots = area.Encounters.BinarySearchByFlatBufferKey(table);
            if (slots == null)
                continue;

            if (species != -1 && slots.Table.All(z => z.Species != species))
                continue;

            result.Add(new(slots.TableName, s.MinSpawnCount, s.MaxSpawnCount, s.Parameters.Coordinates, SpawnerType.Spawner, slots.Table, s.Scalar));
        }

        foreach (var s in area.Wormholes.Concat(area.SubAreas.SelectMany(z => z.Wormholes)))
        {
            var table = s.Field20Value.EncounterTableID;
            var slots = area.Encounters.BinarySearchByFlatBufferKey(table);
            if (slots == null)
                continue;

            if (species != -1 && slots.Table.All(z => z.Species != species))
                continue;

            result.Add(new(slots.TableName, s.MinSpawnCount, s.MaxSpawnCount, s.Parameters.Coordinates, SpawnerType.Wormhole, slots.Table, s.Scalar));
        }

        foreach (var a in area.LandMarks.Concat(area.SubAreas.SelectMany(z => z.LandMarks)))
        {
            var table = a.LandmarkItemSpawnTableID;
            foreach (var l in area.LandItems.Concat(area.SubAreas.SelectMany(z => z.LandItems)))
            {
                if (l.LandmarkItemSpawnTableID != table)
                    continue;
                var st = l.EncounterTableID;
                var slots = area.Encounters.BinarySearchByFlatBufferKey(st);
                if (slots == null)
                    continue;

                if (species != -1 && slots.Table.All(z => z.Species != species))
                    continue;

                result.Add(new(slots.TableName, 1, 1, a.Parameters.Coordinates, SpawnerType.Landmark, slots.Table, Math.Max(a.Scalar, 1) * 4));
            }
        }

        if (species is not 201)
            return result;

        foreach (var u in area.Unown.Concat(area.SubAreas.SelectMany(z => z.Unown)))
        {
            var slots = Unown;
            result.Add(new("Unown", 1, 1, u.Parameters.Coordinates, SpawnerType.Unown, slots, u.Number * 2));
        }

        return result;
    }

    private static readonly EncounterSlot[] Unown = { new()
        {
            Species = 201,
            Behavior1 = string.Empty,
            Behavior2 = string.Empty,
            Unused = string.Empty,
            Eligibility = new EncounterEligiblityTraits
            {
                ConditionArg1 = string.Empty,
                ConditionArg2 = string.Empty,
                ConditionArg3 = string.Empty,
                ConditionArg4 = string.Empty,
                ConditionArg5 = string.Empty,
            },
            Oybn = new(),
        },
    };

    private void MapViewer8a_MouseMove(object sender, MouseEventArgs e)
    {
        SizeF imageSize = pictureBox1.BackgroundImage!.Size;
        SizeF controlSize = pictureBox1.Size;
        float scaleX = imageSize.Width / controlSize.Width;
        float scaleY = imageSize.Height / controlSize.Height;

        var (x, z) = (e.X * scaleX, e.Y * scaleY);

        L_CoordinateMouse.Text = $"{x}, {z}";

        var dist = (int)NUD_Tolerance.Value;
        var spawners = Defs
            .Select(s => (Spawner: s, Distance: s.Position.DistanceTo(new(x, s.Position.Y, z))))
            .Where(s => s.Distance <= s.Spawner.Radius + dist)
            .OrderByDescending(s => s.Distance).ToArray();

        if (spawners.Length == 0)
        {
            L_SpawnDump.Text = "";
            return;
        }

        L_SpawnDump.Text = string.Join(Environment.NewLine, spawners.Select(s => s.Spawner.GetLine()));
    }
}

public class AreaDef
{
    public readonly string NameSummary;
    public readonly int Min;
    public readonly int Max;
    public readonly pkNX.Structures.FlatBuffers.Vec3f Position;
    public readonly SpawnerType Type;
    public readonly IList<EncounterSlot> Slots;
    public readonly float Radius;

    public AreaDef(string NameSummary, int min, int max, pkNX.Structures.FlatBuffers.Vec3f position, SpawnerType type, IList<EncounterSlot> slots, float radius)
    {
        this.NameSummary = NameSummary;
        Min = min;
        Max = max;
        Position = position;
        Type = type;
        Slots = slots;
        Radius = radius;
    }

    public string GetLine()
    {
        var species = string.Join(",", Slots.Select(x => (Species)x.Species));
        return $"{NameSummary}\r\n{Position.ToTriple()} {Min}-{Max}: {species}";
    }
}
