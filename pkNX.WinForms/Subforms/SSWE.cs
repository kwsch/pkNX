using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using pkNX.Game;
using pkNX.Randomization;
using pkNX.Structures;
using pkNX.Structures.FlatBuffers;

namespace pkNX.WinForms
{
    public sealed partial class SSWE : Form
    {
        private readonly EncounterArchive8 Symbols;
        private readonly EncounterArchive8 Hidden;
        private readonly GameManagerSWSH ROM;
        private ulong entry;

        private readonly EncounterList8[] SL;

        public SSWE(GameManagerSWSH rom, EncounterArchive8 sym, EncounterArchive8 hid)
        {
            InitializeComponent();
            Symbols = sym;
            Hidden = hid;
            ROM = rom;

            var spec = rom.GetStrings(TextName.SpeciesNames);
            var species = (string[])spec.Clone();
            species[0] = "";
            EncounterList8.species = species;

            SL = new[] { SL_0, SL_1, SL_2, SL_3, SL_4, SL_5, SL_6, SL_7, SL_8, SL_9, SL_10 };
            foreach (var z in SL)
                z.Initialize();

            PG_Species.SelectedObject = EditUtil.Settings.Species;

            LoadLocations();
        }

        private class LocationHash
        {
            public ulong Hash { get; }
            public string LocationName { get; }

            public LocationHash(ulong hash, string loc)
            {
                Hash = hash;
                LocationName = loc;
            }
        }

        public bool Modified { get; private set; }

        public void LoadLocations()
        {
            static string GetLocationName(ulong id) => SWSHInfo.Zones.TryGetValue(id, out var zoneName) ? zoneName : id.ToString("X16");
            var sl = Symbols.EncounterTables
                .Select(area => new LocationHash(area.ZoneID, GetLocationName(area.ZoneID) + " [S]"));
            var hl = Hidden.EncounterTables
                .Select(area => new LocationHash(area.ZoneID, GetLocationName(area.ZoneID) + " [H]"));
            var locs = sl.Concat(hl)
                .OrderBy(z => z.LocationName)
                .ToArray();

            CB_Location.ValueMember = nameof(LocationHash.Hash);
            CB_Location.DisplayMember = nameof(LocationHash.LocationName);
            CB_Location.DataSource = new BindingSource(locs, null);

            CB_Location.SelectedIndex = 0;
        }

        private void CB_Location_SelectedIndexChanged(object sender, EventArgs e)
        {
            SaveEntry(entry);
            var item = (LocationHash)CB_Location.SelectedItem;
            entry = item.Hash;
            Debug.WriteLine($"Loading area data for [0x{entry:X16}] {item.LocationName}");
            L_Hash.Text = entry.ToString("X16");
            LoadEntry(entry);
        }

        private void LoadEntry(ulong zone)
        {
            Load(SL, Symbols, "Symbols");
            Load(SL, Hidden, "Hidden");
            void Load(EncounterList8[] arr, EncounterArchive8 arc, string name)
            {
                var table = Array.Find(arc.EncounterTables, z => z.ZoneID == zone);
                if (table == null)
                    return;

                L_Type.Text = name;

                var subs = table.SubTables;
                for (int i = 0; i < subs.Length; i++)
                {
                    var t = subs[i];
                    arr[i].NUD_Max.Value = t.LevelMax;
                    arr[i].NUD_Min.Value = t.LevelMin;
                    arr[i].LoadSlots(subs[i].Slots);
                    arr[i].Visible = true;
                }

                // some tables don't have tree/fish
                for (int i = subs.Length; i < arr.Length; i++)
                    arr[i].Visible = false;
            }
        }

        private void SaveEntry(ulong zone)
        {
            if (zone == 0)
                return;

            Save(SL, Symbols);
            Save(SL, Hidden);
            void Save(EncounterList8[] arr, EncounterArchive8 arc)
            {
                var table = Array.Find(arc.EncounterTables, z => z.ZoneID == zone);
                if (table == null)
                    return;

                var subs = table.SubTables;
                for (int i = 0; i < subs.Length; i++)
                {
                    var t = subs[i];
                    t.LevelMax = (byte)arr[i].NUD_Max.Value;
                    t.LevelMin = (byte)arr[i].NUD_Min.Value;
                    arr[i].SaveCurrent();
                }
            }
        }

        private void B_Save_Click(object sender, EventArgs e)
        {
            SaveEntry(entry);
            Modified = true;
            Close();
        }

        private void B_RandAll_Click(object sender, EventArgs e)
        {
            SaveEntry(entry);
            var settings = (SpeciesSettings)PG_Species.SelectedObject;
            var rand = new SpeciesRandomizer(ROM.Info, ROM.Data.PersonalData);

            var pt = ROM.Data.PersonalData;
            var ban = pt.Table.Take(ROM.Info.MaxSpeciesID + 1)
                .Select((z, i) => new {Species = i, Present = ((PersonalInfoSWSH) z).IsPresentInGame})
                .Where(z => !z.Present).Select(z => z.Species).ToArray();

            rand.Initialize(settings, ban);
            RandomizeWild(rand, CHK_FillEmpty.Checked, CHK_Level.Checked);
            LoadEntry(entry);
            System.Media.SystemSounds.Asterisk.Play();
        }

        private void RandomizeWild(SpeciesRandomizer rand, bool fill, bool boost)
        {
            var pt = ROM.Data.PersonalData;
            var fr = new FormRandomizer(pt);
            foreach (var area in Symbols.EncounterTables.Concat(Hidden.EncounterTables))
            {
                foreach (var sub in area.SubTables)
                {
                    if (boost)
                    {
                        sub.LevelMin = (byte)Legal.GetModifiedLevel(sub.LevelMin, (double)NUD_LevelBoost.Value);
                        sub.LevelMax = (byte)Legal.GetModifiedLevel(sub.LevelMax, (double)NUD_LevelBoost.Value);
                    }
                    ApplyRand(sub.Slots);
                }
            }

            void ApplyRand(IList<EncounterSlot8> slots)
            {
                if (slots[0].Species == 0)
                    return;

                for (int i = 0; i < slots.Count; i++)
                {
                    var s = slots[i];
                    if (s.Species == 0)
                    {
                        if (!fill)
                            continue;
                        s.Species = slots.FirstOrDefault(z => z.Species != 0)?.Species ?? rand.GetRandomSpecies();
                        s.Form = 0; // ensure it's not junk
                    }

                    s.Species = rand.GetRandomSpecies(s.Species);
                    s.Form = (byte)fr.GetRandomForme(s.Species, false, false, true, true, ROM.Data.PersonalData.Table);
                    if (fill)
                        s.Probability = RandomScaledRates[slots.Count][i];
                }
            }
        }

        public static readonly Dictionary<int, byte[]> RandomScaledRates = new()
        {
            [01] = new byte[] {100},
            [04] = new byte[] {60, 30, 7, 3},
            [05] = new byte[] {40, 30, 18, 10, 2},
            [10] = new byte[] {20, 15, 15, 10, 10, 10, 10, 5, 4, 1},
        };

        private void TC_Tables_DrawItem(object sender, DrawItemEventArgs e)
        {
            var tc = (TabControl) sender;
            Graphics g = e.Graphics;

            // Get the item from the collection.
            TabPage _tabPage = tc.TabPages[e.Index];

            // Get the real bounds for the tab rectangle.
            Rectangle _tabBounds = tc.GetTabRect(e.Index);

            Brush _textBrush;
            if (e.State == DrawItemState.Selected)
            {
                // Draw a different background color, and don't paint a focus rectangle.
                _textBrush = new SolidBrush(Color.Red);
                g.FillRectangle(Brushes.Beige, e.Bounds);
            }
            else
            {
                _textBrush = new SolidBrush(e.ForeColor);
                e.DrawBackground();
            }

            // Use our own font.
            Font _tabFont = new("Arial", (float)10.0, FontStyle.Bold, GraphicsUnit.Pixel);

            // Draw string. Center the text.
            StringFormat _stringFlags = new()
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };
            g.DrawString(_tabPage.Text, _tabFont, _textBrush, _tabBounds, new StringFormat(_stringFlags));
        }
    }
}
