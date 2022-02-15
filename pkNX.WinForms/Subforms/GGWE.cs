using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using pkNX.Containers;
using pkNX.Game;
using pkNX.Randomization;
using pkNX.Structures;
using pkNX.Structures.FlatBuffers;

namespace pkNX.WinForms
{
    public sealed partial class GGWE : Form
    {
        private readonly EncounterArchive7b Tables;
        private readonly GameManagerGG ROM;
        private int entry = -1;

        public GGWE(GameManagerGG rom, EncounterArchive7b obj)
        {
            InitializeComponent();
            if (obj.EncounterTables.Length == 0 || obj.EncounterTables[0].GroundTable.Length == 0)
            {
                WinFormsUtil.Error("Bad data provided.", $"Unable to parse to {nameof(EncounterArchive7b)} data.");
                Close();
            }

            ROM = rom;

            var spec = rom.GetStrings(TextName.SpeciesNames);
            var species = (string[]) spec.Clone();
            species[0] = "";
            EncounterList.species = species;
            var locs = rom.GetStrings(TextName.metlist_00000);

            EL_Ground.Initialize();
            EL_Water.Initialize();
            EL_Old.Initialize();
            EL_Good.Initialize();
            EL_Super.Initialize();
            EL_Sky.Initialize();

            TC_Tables.Controls.Remove(Tab_Old);
            TC_Tables.Controls.Remove(Tab_Good);
            TC_Tables.Controls.Remove(Tab_Super);
            EL_Old.OverworldSpawn = EL_Good.OverworldSpawn = EL_Super.OverworldSpawn = false;
            L_Rank.Visible = NUD_RankMin.Visible = NUD_RankMax.Visible = false;

            PG_Species.SelectedObject = EditUtil.Settings.Species;

            Tables = obj;
            LoadFile(locs);

            EL_Ground.ShowForm = false;
            EL_Water.ShowForm = false;
            EL_Old.ShowForm = false;
            EL_Good.ShowForm = false;
            EL_Super.ShowForm = false;
            EL_Sky.ShowForm = false;
            CB_Location.SelectedIndex = 0;
        }

        public void LoadFile(string[] locationNames)
        {
            var locs = Tables.EncounterTables.Select(z => z.ZoneID);
            var names = GetNames(locs, locationNames);
            var dupeNamed = GetScreenedNames(names);

            CB_Location.Items.Clear();
            CB_Location.Items.AddRange(dupeNamed.ToArray());
        }

        private static IEnumerable<string> GetNames(IEnumerable<ulong> locs, string[] locationNames)
        {
            return locs
                .Select(z => DictHash[z]) // loc internal name
                .Select(z => LocIDTable[z]) // loc ID
                .Select(z => locationNames[z]); // loc external name (pkmdata)
        }

        private static IEnumerable<string> GetScreenedNames(IEnumerable<string> names)
        {
            int ctr = 0;
            string? prev = null;
            foreach (var name in names)
            {
                if (name != prev)
                {
                    ctr = 1;
                    yield return name;
                }
                else
                {
                    ctr++;
                    yield return $"{name} ({ctr})";
                }
                prev = name;
            }
        }

        private static readonly Dictionary<string, int> LocIDTable = new()
        {
            ["forest001"] = 39,
            ["r004d0101"] = 40,
            ["r004d0102"] = 40,
            ["r004d0103"] = 40,
            ["r010d0101"] = 41,
            ["r010d0102"] = 41,
            ["r010r0101"] = 42,
            ["r011d0101"] = 43,

            ["r020d0101"] = 44,
            ["r020d0102"] = 44,
            ["r020d0103"] = 44,
            ["r020d0104_1"] = 44,
            ["r020d0104_2"] = 44,
            ["r020d0105_1"] = 44,
            ["r020d0105_2"] = 44,
            ["r023d0101"] = 45,
            ["r023d0102"] = 45,
            ["r023d0103"] = 45,
            ["road001"] = 3,
            ["road002_1"] = 4,
            ["road002_2"] = 4,
            ["road003"] = 5,
            ["road004_1"] = 6,
            ["road004_2"] = 6,
            ["road005"] = 7,
            ["road006"] = 8,
            ["road007"] = 9,
            ["road008"] = 10,
            ["road009"] = 11,
            ["road010_1"] = 12,
            ["road010_2"] = 12,
            ["road011_1"] = 13,
            ["road011_2"] = 13,
            ["road012"] = 14,
            ["road013"] = 15,
            ["road014"] = 16,
            ["road015_1"] = 17,
            ["road015_2"] = 17,
            ["road016_1"] = 18,
            ["road016_2"] = 18,
            ["road017"] = 19,
            ["road018_1"] = 20,
            ["road018_2"] = 20,
            ["road019"] = 21,
            ["road020"] = 22,
            ["road020_2"] = 22,
            ["road021"] = 23,
            ["road022"] = 24,
            ["road023"] = 25,
            ["road024"] = 26,
            ["road025"] = 27,
            ["t004d0101"] = 46,
            ["t004d0102"] = 46,
            ["t004d0103"] = 46,
            ["t005r0303"] = 47,
            ["t005r0304"] = 47,
            ["t005r0305"] = 47,
            ["t005r0306"] = 47,
            ["t009r0201"] = 51,
            ["t009r0202"] = 51,
            ["t009r0203"] = 51,
            ["t009r0204"] = 51,
            ["town001"] = 28,
            ["town002"] = 29,
            ["town003"] = 30,
            ["town004"] = 31,
            ["town005"] = 32,
            ["town006"] = 33,
            ["town007"] = 34,
            ["town008"] = 35,
            ["town009"] = 36,
        };

        private static readonly Dictionary<ulong, string> DictHash = LocIDTable
            .Select(z => new KeyValuePair<ulong, string>(FnvHash.HashFnv1a_64(z.Key), z.Key))
            .ToDictionary(z => z.Key, z => z.Value);

        private void CB_Location_SelectedIndexChanged(object sender, EventArgs e)
        {
            SaveEntry(entry);
            entry = CB_Location.SelectedIndex;
            var id = Tables.EncounterTables[entry].ZoneID;
            var iname = LocIDTable.First(z => FnvHash.HashFnv1a_64(z.Key) == id).Key;
            L_Hash.Text = Tables.EncounterTables[entry].ZoneID.ToString("X16") + " " + iname;
            LoadEntry(entry);
        }

        private void LoadEntry(int i)
        {
            var arr = Tables.EncounterTables[i];

            NUD_RankMin.Value = arr.TrainerRankMin;
            NUD_RankMax.Value = arr.TrainerRankMax;

            EL_Ground.LoadSlots(arr.GroundTable);
            EL_Water.LoadSlots(arr.WaterTable);
            EL_Old.LoadSlots(arr.OldRodTable);
            EL_Good.LoadSlots(arr.GoodRodTable);
            EL_Super.LoadSlots(arr.SuperRodTable);
            EL_Sky.LoadSlots(arr.SkyTable);

            EL_Ground.NUD_Min.Value = arr.GroundTableLevelMin;
            EL_Ground.NUD_Max.Value = arr.GroundTableLevelMax;
            EL_Ground.NUD_SpawnRate.Value = arr.GroundTableEncounterRate;
            EL_Ground.NUD_Count.Value = arr.GroundSpawnCountMax;
            EL_Ground.NUD_Duration.Value = arr.GroundSpawnDuration;

            EL_Water.NUD_Min.Value = arr.WaterTableLevelMin;
            EL_Water.NUD_Max.Value = arr.WaterTableLevelMax;
            EL_Water.NUD_SpawnRate.Value = arr.WaterTableEncounterRate;
            EL_Water.NUD_Count.Value = arr.WaterSpawnCountMax;
            EL_Water.NUD_Duration.Value = arr.WaterSpawnDuration;

            EL_Old.NUD_Min.Value = arr.OldRodTableLevelMin;
            EL_Old.NUD_Max.Value = arr.OldRodTableLevelMax;
            EL_Old.NUD_SpawnRate.Value = arr.OldRodTableEncounterRate;

            EL_Good.NUD_Min.Value = arr.GoodRodTableLevelMin;
            EL_Good.NUD_Max.Value = arr.GoodRodTableLevelMax;
            EL_Good.NUD_SpawnRate.Value = arr.GoodRodTableEncounterRate;

            EL_Super.NUD_Min.Value = arr.SuperRodTableLevelMin;
            EL_Super.NUD_Max.Value = arr.SuperRodTableLevelMax;
            EL_Super.NUD_SpawnRate.Value = arr.SuperRodTableEncounterRate;

            EL_Sky.NUD_Min.Value = arr.SkyTableLevelMin;
            EL_Sky.NUD_Max.Value = arr.SkyTableLevelMax;
            EL_Sky.NUD_SpawnRate.Value = arr.SkyTableEncounterRate;
            EL_Sky.NUD_Count.Value = arr.SkySpawnCountMax;
            EL_Sky.NUD_Duration.Value = arr.SkySpawnDuration;
        }

        private void SaveEntry(int i)
        {
            if (i < 0)
                return;
            var arr = Tables.EncounterTables[i];

            arr.TrainerRankMin = (int)NUD_RankMin.Value;
            arr.TrainerRankMax = (int)NUD_RankMax.Value;

            arr.GroundTableLevelMin = (int)EL_Ground.NUD_Min.Value;
            arr.GroundTableLevelMax = (int)EL_Ground.NUD_Max.Value;
            arr.GroundTableEncounterRate = (int)EL_Ground.NUD_SpawnRate.Value;
            arr.GroundSpawnCountMax = (int)EL_Ground.NUD_Count.Value;
            arr.GroundSpawnDuration = (int)EL_Ground.NUD_Duration.Value;

            arr.WaterTableLevelMin = (int)EL_Water.NUD_Min.Value;
            arr.WaterTableLevelMax = (int)EL_Water.NUD_Max.Value;
            arr.WaterTableEncounterRate = (int)EL_Water.NUD_SpawnRate.Value;
            arr.WaterSpawnCountMax = (int)EL_Water.NUD_Count.Value;
            arr.WaterSpawnDuration = (int)EL_Water.NUD_Duration.Value;

            arr.OldRodTableLevelMin = (int)EL_Old.NUD_Min.Value;
            arr.OldRodTableLevelMax = (int)EL_Old.NUD_Max.Value;
            arr.OldRodTableEncounterRate = (int)EL_Old.NUD_SpawnRate.Value;

            arr.GoodRodTableLevelMin = (int)EL_Good.NUD_Min.Value;
            arr.GoodRodTableLevelMax = (int)EL_Good.NUD_Max.Value;
            arr.GoodRodTableEncounterRate = (int)EL_Good.NUD_SpawnRate.Value;

            arr.SuperRodTableLevelMin = (int)EL_Super.NUD_Min.Value;
            arr.SuperRodTableLevelMax = (int)EL_Super.NUD_Max.Value;
            arr.SuperRodTableEncounterRate = (int)EL_Super.NUD_SpawnRate.Value;

            arr.SkyTableLevelMin = (int)EL_Sky.NUD_Min.Value;
            arr.SkyTableLevelMax = (int)EL_Sky.NUD_Max.Value;
            arr.SkyTableEncounterRate = (int)EL_Sky.NUD_SpawnRate.Value;
            arr.SkySpawnCountMax = (int)EL_Sky.NUD_Count.Value;
            arr.SkySpawnDuration = (int)EL_Sky.NUD_Duration.Value;

            EL_Ground.SaveCurrent();
            EL_Water.SaveCurrent();
            EL_Old.SaveCurrent();
            EL_Good.SaveCurrent();
            EL_Super.SaveCurrent();
            EL_Sky.SaveCurrent();
        }

        private void B_Save_Click(object sender, EventArgs e)
        {
            SaveEntry(entry);
            EL_Ground.SaveCurrent();
            EL_Water.SaveCurrent();
            EL_Old.SaveCurrent();
            EL_Good.SaveCurrent();
            EL_Super.SaveCurrent();
            EL_Sky.SaveCurrent();

            DialogResult = DialogResult.OK;

            Close();
        }

        private void B_ModCount_Click(object sender, EventArgs e)
        {
            SaveEntry(entry);
            foreach (var area in Tables.EncounterTables)
            {
                if (area.GroundSpawnCountMax != 0)
                    area.GroundSpawnCountMax = (int)NUD_ModCount.Value;
                if (area.WaterSpawnCountMax != 0)
                    area.WaterSpawnCountMax = (int)NUD_ModCount.Value;
                if (area.SkySpawnCountMax != 0)
                    area.SkySpawnCountMax = (int)NUD_ModCount.Value;
            }
            LoadEntry(entry);
            System.Media.SystemSounds.Asterisk.Play();
        }

        private void B_ModRate_Click(object sender, EventArgs e)
        {
            SaveEntry(entry);
            foreach (var area in Tables.EncounterTables)
            {
                if (area.GroundTableEncounterRate != 0)
                    area.GroundTableEncounterRate = (int)NUD_ModCount.Value;
                if (area.WaterTableEncounterRate != 0)
                    area.WaterTableEncounterRate = (int)NUD_ModCount.Value;
                if (area.SkyTableEncounterRate != 0)
                    area.SkyTableEncounterRate = (int)NUD_ModCount.Value;
            }
            LoadEntry(entry);
            System.Media.SystemSounds.Asterisk.Play();
        }

        private void B_ModDuration_Click(object sender, EventArgs e)
        {
            SaveEntry(entry);
            foreach (var area in Tables.EncounterTables)
            {
                if (area.GroundSpawnDuration != 0)
                    area.GroundSpawnDuration = (int)NUD_ModDuration.Value;
                if (area.WaterSpawnDuration != 0)
                    area.WaterSpawnDuration = (int)NUD_ModDuration.Value;
                if (area.SkySpawnDuration != 0)
                    area.SkySpawnDuration = (int)NUD_ModDuration.Value;
            }
            LoadEntry(entry);
            System.Media.SystemSounds.Asterisk.Play();
        }

        private void B_RandAll_Click(object sender, EventArgs e)
        {
            SaveEntry(entry);
            var settings = (SpeciesSettings)PG_Species.SelectedObject;
            settings.Gen2 = settings.Gen3 = settings.Gen4 = settings.Gen5 = settings.Gen6 = settings.Gen7 = false;
            var rand = new SpeciesRandomizer(ROM.Info, ROM.Data.PersonalData);
            rand.Initialize(settings, 808, 809);
            RandomizeWild(rand, CHK_FillEmpty.Checked, CHK_Level.Checked);
            LoadEntry(entry);
            System.Media.SystemSounds.Asterisk.Play();
        }

        private void RandomizeWild(SpeciesRandomizer rand, bool fill, bool boost)
        {
            var pt = ROM.Data.PersonalData;
            bool IsGrassOrWater(int s) => pt[s].IsType((int)Types.Water) || pt[s].IsType((int)Types.Grass);

            foreach (var area in Tables.EncounterTables)
            {
                if (boost)
                {
                    area.GroundTableLevelMin = Legal.GetModifiedLevel(area.GroundTableLevelMin, (double)NUD_LevelBoost.Value);
                    area.GroundTableLevelMax = Legal.GetModifiedLevel(area.GroundTableLevelMax, (double)NUD_LevelBoost.Value);

                    area.WaterTableLevelMin = Legal.GetModifiedLevel(area.WaterTableLevelMin, (double)NUD_LevelBoost.Value);
                    area.WaterTableLevelMax = Legal.GetModifiedLevel(area.WaterTableLevelMax, (double)NUD_LevelBoost.Value);

                    area.SkyTableLevelMin = Legal.GetModifiedLevel(area.SkyTableLevelMin, (double)NUD_LevelBoost.Value);
                    area.SkyTableLevelMax = Legal.GetModifiedLevel(area.SkyTableLevelMax, (double)NUD_LevelBoost.Value);
                }
                ApplyRand(area.GroundTable);
                ApplyRand(area.WaterTable);
                ApplyRand(area.SkyTable);

                ApplyRand(area.OldRodTable);
                ApplyRand(area.GoodRodTable);
                ApplyRand(area.SuperRodTable);
            }

            void ApplyRand(IList<EncounterSlot7b> slots)
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
                    }

                    s.Species = rand.GetRandomSpecies(s.Species);
                    s.Form = 0; // mega & alolan forms don't spawn :(
                    if (fill)
                        s.Probability = RandomScaledRates[slots.Count][i];
                }
            }

            if (CHK_ForceType.Checked)
            {
                var table = Tables.EncounterTables[0];
                var slots = table.GroundTable;
                while (!slots.Any(z => IsGrassOrWater(z.Species)))
                    ApplyRand(slots);
            }
        }

        public static readonly Dictionary<int, int[]> RandomScaledRates = new()
        {
            [01] = new[] {100},
            [04] = new[] {60, 30, 7, 3},
            [05] = new[] {40, 30, 18, 10, 2},
            [10] = new[] {20, 15, 15, 10, 10, 10, 10, 5, 4, 1},
        };

        private void B_Dump_Click(object sender, EventArgs e)
        {
            var strings = GetEncounterTableSummary(ROM, Tables);
            var result = string.Join(Environment.NewLine, strings);
            Clipboard.SetText(result);
            System.Media.SystemSounds.Asterisk.Play();
        }

        private static IEnumerable<string> GetEncounterTableSummary(GameManager rom, EncounterArchive7b table)
        {
            var locationNames = rom.GetStrings(TextName.metlist_00000);
            var specs = rom.GetStrings(TextName.SpeciesNames);

            var locs = table.EncounterTables.Select(z => z.ZoneID);
            var names = GetNames(locs, locationNames);
            var dupeNamed = GetScreenedNames(names).ToArray();
            return EncounterTable7bUtil.GetLines(table, dupeNamed, specs);
        }
    }
}
