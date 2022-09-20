using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using pkNX.Game;
using pkNX.Randomization;
using pkNX.Sprites;
using pkNX.Structures;
using pkNX.Structures.FlatBuffers;
using Util = pkNX.Randomization.Util;

namespace pkNX.WinForms
{
    public partial class PokeDataUI8a : Form
    {
        private static EvolutionRow8a[] EvoRows = Array.Empty<EvolutionRow8a>();

        private readonly bool Loaded;
        private readonly GameData8a Data;

        public readonly GameManager ROM;
        public readonly PokeEditor8a Editor;
        public bool Modified { get; set; }

        private readonly ComboBox[] helditem_boxes;
        private readonly ComboBox[] ability_boxes;
        private readonly ComboBox[] typing_boxes;
        private readonly ComboBox[] eggGroup_boxes;

        private readonly string[] items;
        private readonly string[] movelist;
        private readonly string[] species;
        private readonly string[] classifications;
        private readonly string[] abilities;
        private readonly string[] types;

        private readonly string[] entryNames;
        private readonly int[] baseForms, formVal;

        public IPersonalInfoPLA cPersonal;
        public Learnset8aMeta cLearnset;
        public EvolutionSet8a cEvos;

        public PokeDataUI8a(PokeEditor8a editor, GameManager rom, GameData8a data)
        {
            ROM = rom;
            Editor = editor;
            Data = data;
            InitializeComponent();

            helditem_boxes = new[] { CB_HeldItem1, CB_HeldItem2, CB_HeldItem3 };
            ability_boxes = new[] { CB_Ability1, CB_Ability2, CB_Ability3 };
            typing_boxes = new[] { CB_Type1, CB_Type2 };
            eggGroup_boxes = new[] { CB_EggGroup1, CB_EggGroup2 };

            items = ROM.GetStrings(TextName.ItemNames);
            movelist = ROM.GetStrings(TextName.MoveNames);
            species = ROM.GetStrings(TextName.SpeciesNames);
            classifications = ROM.GetStrings(TextName.SpeciesClassifications);
            abilities = ROM.GetStrings(TextName.AbilityNames);
            types = ROM.GetStrings(TextName.Types);
            movelist = EditorUtil.SanitizeMoveList(movelist);

            species[0] = "---";
            abilities[0] = items[0] = movelist[0] = "---";

            var pt = Data.PersonalData;
            cPersonal = (IPersonalInfoPLA)pt[0];
            cLearnset = Editor.Learn[0];
            cEvos = Editor.Evolve[0];

            var altForms = GetFormList(pt, species);
            entryNames = GetPersonalEntryList(pt, altForms, species, out baseForms, out formVal);

            InitPersonal();
            InitLearn();

            CB_HatchSpecies.Items.AddRange(species.ToArray());

            CB_Species.SelectedIndex = 1;
            Loaded = true;

            PG_Personal.SelectedObject = EditUtil.Settings.Personal;
            PG_Evolution.SelectedObject = EditUtil.Settings.Species;
            PG_Learn.SelectedObject = EditUtil.Settings.Learn;
            PG_Move.SelectedObject = EditUtil.Settings.Move;
        }

        /// <summary>
        /// Gets form names for every species.
        /// </summary>
        /// <param name="species">Raw string resource (Species) for the corresponding table.</param>
        /// <param name="MaxSpecies">Max Species ID (Species ID)</param>
        /// <returns>Array of species containing an array of form names for that species.</returns>
        public string[][] GetFormList(IPersonalTable pt, string[] species)
        {
            string[][] FormList = new string[pt.MaxSpeciesID + 1][];
            for (int i = 0; i < FormList.Length; i++)
            {
                int FormCount = pt[i].FormCount;
                FormList[i] = new string[FormCount];
                if (FormCount <= 0) continue;
                FormList[i][0] = species[i];
                for (int j = 1; j < FormCount; j++)
                    FormList[i][j] = $"{species[i]} {j}";
            }

            return FormList;
        }

        /// <summary>
        /// Gets an arranged list of Form names and indexes for use with the individual <see cref="PersonalInfo"/> AltForm ID values.
        /// </summary>
        /// <param name="AltForms">Raw string resource (Forms) for the corresponding table.</param>
        /// <param name="species">Raw string resource (Species) for the corresponding table.</param>
        /// <param name="MaxSpecies">Max Species ID (Species ID)</param>
        /// <param name="baseForm">Pointers for base form IDs</param>
        /// <param name="formVal">Pointers for table indexes for each form</param>
        /// <returns>Sanitized list of species names, and outputs indexes for various lookup purposes.</returns>
        public string[] GetPersonalEntryList(IPersonalTable pt, string[][] AltForms, string[] species, out int[] baseForm, out int[] formVal)
        {
            string[] result = new string[pt.Table.Length];
            baseForm = new int[result.Length];
            formVal = new int[result.Length];
            for (int i = 0; i <= pt.MaxSpeciesID; i++)
            {
                result[i] = species[i];
                if (AltForms[i].Length == 0) continue;
                int altformpointer = pt[i].FormStatsIndex;
                if (altformpointer <= 0) continue;
                for (int j = 1; j < AltForms[i].Length; j++)
                {
                    int ptr = altformpointer + j - 1;
                    baseForm[ptr] = i;
                    formVal[ptr] = j;
                    result[ptr] = AltForms[i][j];
                }
            }
            return result;
        }

        public void InitPersonal()
        {
            /*{
                if (GameVersion.SWSH.Contains(ROM.Game))
                {
                    for (int i = 0; i < TMs.Count / 2; i++)
                        CLB_TM.Items.Add($"TM{i:00} {movelist[TMs[i]]}");
                    for (int i = TMs.Count / 2; i < TMs.Count; i++)
                        CLB_TM.Items.Add($"TR{i - 100:00} {movelist[TMs[i]]}");
                    for (int i = 0; i < Legal.TypeTutor8.Length; i++)
                        CLB_TypeTutor.Items.Add(movelist[Legal.TypeTutor8[i]]);
                    for (int i = 0; i < Legal.Tutors_SWSH_1.Length; i++)
                        CLB_SpecialTutor.Items.Add(movelist[Legal.Tutors_SWSH_1[i]]);
                }
                else
                {
                    for (int i = 0; i < TMs.Count; i++)
                        CLB_TM.Items.Add($"TM{i + 1:00} {movelist[TMs[i]]}");
                }
            }*/

            var entries = entryNames.Select((z, i) => $"{z} - {i:000}");
            CB_Species.Items.AddRange(entries.ToArray());

            foreach (ComboBox cb in helditem_boxes)
                cb.Items.AddRange(items);

            foreach (ComboBox cb in ability_boxes)
                cb.Items.AddRange(abilities);

            foreach (ComboBox cb in typing_boxes)
                cb.Items.AddRange(types);

            foreach (ComboBox cb in eggGroup_boxes)
                cb.Items.AddRange(Enum.GetNames(typeof(EggGroup)));

            CB_Color.Items.AddRange(Enum.GetNames(typeof(PokeColor)));
            CB_EXPGroup.Items.AddRange(Enum.GetNames(typeof(EXPGroup)));
        }

        public void InitLearn()
        {
            string[] sortedmoves = (string[])movelist.Clone();

            Array.Sort(sortedmoves);
            DataGridViewColumn dgvLevel = new DataGridViewTextBoxColumn();
            {
                dgvLevel.HeaderText = "Level";
                dgvLevel.DisplayIndex = 0;
                dgvLevel.Width = 45;
                dgvLevel.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
            DataGridViewColumn dgvLevelMastery = new DataGridViewTextBoxColumn();
            {
                dgvLevel.HeaderText = "Level Mastery";
                dgvLevel.DisplayIndex = 0;
                dgvLevel.Width = 45;
                dgvLevel.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
            DataGridViewComboBoxColumn dgvMove = new();
            {
                dgvMove.HeaderText = "Move";
                dgvMove.DisplayIndex = 1;
                for (int i = 0; i < movelist.Length; i++)
                    dgvMove.Items.Add(sortedmoves[i]); // add only the Names

                dgvMove.Width = 135;
                dgvMove.FlatStyle = FlatStyle.Flat;
            }
            dgv.Columns.Add(dgvLevel);
            dgv.Columns.Add(dgvLevelMastery);
            dgv.Columns.Add(dgvMove);
        }

        public void InitEvo(int rows)
        {
            EvoRows = new EvolutionRow8a[rows];
            EvolutionRow8a.species = species;
            EvolutionRow8a.items = items;
            EvolutionRow8a.movelist = movelist;
            EvolutionRow8a.types = types;

            for (int i = 0; i < rows; i++)
            {
                var row = new EvolutionRow8a();
                flowLayoutPanel1.Controls.Add(row);
                flowLayoutPanel1.SetFlowBreak(row, true);
                EvoRows[i] = row;
            }
        }

        public void UpdateIndex(object sender, EventArgs e)
        {
            if (Loaded)
                SaveCurrent();
            LoadIndex(CB_Species.SelectedIndex);
        }

        private void LoadIndex(int index)
        {
            int spec = baseForms[index];
            if (spec == 0)
                spec = index;
            var form = formVal[index];
            LoadPersonal((IPersonalInfoPLA)Data.PersonalData[index]);
            LoadLearnset(Editor.Learn[index]);
            LoadEvolutions(Editor.Evolve[index]);

            Bitmap rawImg = (Bitmap)SpriteUtil.GetSprite(spec, form, 0, 0, false, false, false);
            Bitmap bigImg = new(rawImg.Width * 2, rawImg.Height * 2);
            for (int x = 0; x < rawImg.Width; x++)
            {
                for (int y = 0; y < rawImg.Height; y++)
                {
                    Color c = rawImg.GetPixel(x, y);
                    bigImg.SetPixel(2 * x, 2 * y, c);
                    bigImg.SetPixel((2 * x) + 1, 2 * y, c);
                    bigImg.SetPixel(2 * x, (2 * y) + 1, c);
                    bigImg.SetPixel((2 * x) + 1, (2 * y) + 1, c);
                }
            }
            PB_MonSprite.Image = bigImg;
        }

        private void SaveCurrent()
        {
            SavePersonal();
            SaveLearnset();
            SaveEvolutions();
        }

        public void LoadPersonal(IPersonalInfoPLA pkm)
        {
            cPersonal = pkm;

            TB_BaseHP.Text = pkm.HP.ToString(TB_BaseHP.Mask);
            TB_BaseATK.Text = pkm.ATK.ToString(TB_BaseATK.Mask);
            TB_BaseDEF.Text = pkm.DEF.ToString(TB_BaseDEF.Mask);
            TB_BaseSPE.Text = pkm.SPE.ToString(TB_BaseSPE.Mask);
            TB_BaseSPA.Text = pkm.SPA.ToString(TB_BaseSPA.Mask);
            TB_BaseSPD.Text = pkm.SPD.ToString(TB_BaseSPD.Mask);
            TB_HPEVs.Text = pkm.EV_HP.ToString(TB_HPEVs.Mask);
            TB_ATKEVs.Text = pkm.EV_ATK.ToString(TB_ATKEVs.Mask);
            TB_DEFEVs.Text = pkm.EV_DEF.ToString(TB_DEFEVs.Mask);
            TB_SPEEVs.Text = pkm.EV_SPE.ToString(TB_SPEEVs.Mask);
            TB_SPAEVs.Text = pkm.EV_SPA.ToString(TB_SPAEVs.Mask);
            TB_SPDEVs.Text = pkm.EV_SPD.ToString(TB_SPDEVs.Mask);
            TB_BST.Text = pkm.GetBaseStatTotal().ToString(TB_BST.Mask);

            TB_Classification.Text = classifications[pkm.Species];

            CB_Type1.SelectedIndex = (int)pkm.Type1;
            CB_Type2.SelectedIndex = (int)pkm.Type2;

            TB_CatchRate.Text = pkm.CatchRate.ToString(TB_CatchRate.Mask);
            TB_Stage.Text = pkm.EvoStage.ToString(TB_Stage.Mask);

            CB_HeldItem1.SelectedIndex = pkm.Item1;
            CB_HeldItem2.SelectedIndex = pkm.Item2;
            CB_HeldItem3.SelectedIndex = pkm.Item3;

            TB_Gender.Text = pkm.Gender.ToString(TB_Gender.Mask);
            TB_Field_18.Text = pkm.Field_18.ToString(TB_Field_18.Mask);
            TB_Friendship.Text = pkm.BaseFriendship.ToString(TB_Friendship.Mask);

            CB_EXPGroup.SelectedIndex = pkm.EXPGrowth;

            CB_EggGroup1.SelectedIndex = pkm.EggGroup1;
            CB_EggGroup2.SelectedIndex = pkm.EggGroup2;
            CB_HatchSpecies.SelectedIndex = pkm.HatchedSpecies;

            CB_Ability1.SelectedIndex = pkm.Ability1;
            CB_Ability2.SelectedIndex = pkm.Ability2;
            CB_Ability3.SelectedIndex = pkm.AbilityH;

            TB_FormeCount.Text = pkm.FormCount.ToString(TB_FormeCount.Mask);
            TB_FormeSprite.Text = pkm.FormSprite.ToString(TB_FormeSprite.Mask);

            TB_RawColor.Text = pkm.Color.ToString(TB_RawColor.Mask);
            CB_Color.SelectedIndex = pkm.Color & 0xF;

            TB_BaseExp.Text = pkm.BaseEXP.ToString(TB_BaseExp.Mask);

            TB_Height.Text = (pkm.Height / 100.0).ToString("#0.0");
            TB_Weight.Text = (pkm.Weight / 10.0).ToString("#0.0");

            TB_NationalDex.Text = pkm.DexIndexNational.ToString(TB_NationalDex.Mask);
            TB_HisuianDex.Text = pkm.DexIndexRegional.ToString(TB_HisuianDex.Mask);
            TB_Region1Dex.Text = pkm.DexIndexLocal1.ToString(TB_Region1Dex.Mask);
            TB_Region2Dex.Text = pkm.DexIndexLocal2.ToString(TB_Region2Dex.Mask);
            TB_Region3Dex.Text = pkm.DexIndexLocal3.ToString(TB_Region3Dex.Mask);
            TB_Region4Dex.Text = pkm.DexIndexLocal4.ToString(TB_Region4Dex.Mask);
            TB_Region5Dex.Text = pkm.DexIndexLocal5.ToString(TB_Region5Dex.Mask);

            TB_LocalFormIndex.Text = pkm.LocalFormIndex.ToString(TB_LocalFormIndex.Mask);
            TB_Field_46.Text = pkm.Field_46.ToString(TB_Field_46.Mask);
            TB_Field_47.Text = pkm.Field_47.ToString(TB_Field_47.Mask);

            //TB_MoveShop1.Text = pkm.MoveShop1.ToString(TB_MoveShop1.Mask);
            //TB_MoveShop2.Text = pkm.MoveShop2.ToString(TB_MoveShop2.Mask);

            CHK_IsPresentInGame.Checked = pkm.IsPresentInGame;
            CHK_Field_45.Checked = pkm.Field_45;
            //CHK_Variant.Checked = pkm.IsRegionalForm;

            /*for (int i = 0; i < CLB_TM.Items.Count; i++)
                CLB_TM.SetItemChecked(i, pkm.TMHM[i]); // Bitflags for TM
            for (int i = 0; i < CLB_TypeTutor.Items.Count; i++)
                CLB_TypeTutor.SetItemChecked(i, pkm.TypeTutors[i]);
            for (int i = 0; i < CLB_SpecialTutor.Items.Count; i++)
                CLB_SpecialTutor.SetItemChecked(i, pkm.SpecialTutors[0][i]);*/
        }

        public void SavePersonal()
        {
            var pkm = cPersonal;
            pkm.HP = Util.ToInt32(TB_BaseHP.Text);
            pkm.ATK = Util.ToInt32(TB_BaseATK.Text);
            pkm.DEF = Util.ToInt32(TB_BaseDEF.Text);
            pkm.SPE = Util.ToInt32(TB_BaseSPE.Text);
            pkm.SPA = Util.ToInt32(TB_BaseSPA.Text);
            pkm.SPD = Util.ToInt32(TB_BaseSPD.Text);

            pkm.EV_HP = Util.ToInt32(TB_HPEVs.Text);
            pkm.EV_ATK = Util.ToInt32(TB_ATKEVs.Text);
            pkm.EV_DEF = Util.ToInt32(TB_DEFEVs.Text);
            pkm.EV_SPE = Util.ToInt32(TB_SPEEVs.Text);
            pkm.EV_SPA = Util.ToInt32(TB_SPAEVs.Text);
            pkm.EV_SPD = Util.ToInt32(TB_SPDEVs.Text);

            pkm.Type1 = (Types)CB_Type1.SelectedIndex;
            pkm.Type2 = (Types)CB_Type2.SelectedIndex;

            pkm.CatchRate = Util.ToInt32(TB_CatchRate.Text);
            pkm.EvoStage = Util.ToInt32(TB_Stage.Text);

            pkm.Item1 = CB_HeldItem1.SelectedIndex;
            pkm.Item2 = CB_HeldItem2.SelectedIndex;
            pkm.Item3 = CB_HeldItem3.SelectedIndex;

            pkm.Gender = Util.ToInt32(TB_Gender.Text);
            pkm.Field_18 = Convert.ToByte(TB_Field_18.Text);
            pkm.BaseFriendship = Util.ToInt32(TB_Friendship.Text);

            pkm.EXPGrowth = (byte)CB_EXPGroup.SelectedIndex;

            pkm.EggGroup1 = CB_EggGroup1.SelectedIndex;
            pkm.EggGroup2 = CB_EggGroup2.SelectedIndex;

            pkm.HatchedSpecies = (ushort)CB_HatchSpecies.SelectedIndex;

            pkm.Ability1 = CB_Ability1.SelectedIndex;
            pkm.Ability2 = CB_Ability2.SelectedIndex;
            pkm.AbilityH = CB_Ability3.SelectedIndex;

            pkm.FormCount = Convert.ToByte(TB_FormeCount.Text);
            pkm.FormSprite = Convert.ToUInt16(TB_FormeSprite.Text);
            pkm.Color = (byte)(CB_Color.SelectedIndex) | (Util.ToInt32(TB_RawColor.Text) & 0xF0);

            pkm.BaseEXP = Convert.ToUInt16(TB_BaseExp.Text);

            if (double.TryParse(TB_Height.Text, out double h))
                pkm.Height = (int)(h * 100.0);

            if (double.TryParse(TB_Weight.Text, out double w))
                pkm.Weight = (int)(w * 10.0);

            pkm.DexIndexNational = Convert.ToUInt16(TB_NationalDex.Text);
            pkm.DexIndexRegional = Convert.ToUInt16(TB_HisuianDex.Text);
            pkm.DexIndexLocal1 = Convert.ToUInt16(TB_Region1Dex.Text);
            pkm.DexIndexLocal2 = Convert.ToUInt16(TB_Region2Dex.Text);
            pkm.DexIndexLocal3 = Convert.ToUInt16(TB_Region3Dex.Text);
            pkm.DexIndexLocal4 = Convert.ToUInt16(TB_Region4Dex.Text);
            pkm.DexIndexLocal5 = Convert.ToUInt16(TB_Region5Dex.Text);

            TB_LocalFormIndex.Text = pkm.LocalFormIndex.ToString(TB_LocalFormIndex.Mask);
            TB_Field_46.Text = pkm.Field_46.ToString(TB_Field_46.Mask);
            TB_Field_47.Text = pkm.Field_47.ToString(TB_Field_47.Mask);

            //pkm.MoveShop1 = Convert.ToUInt16(TB_MoveShop1.Text);
            //pkm.MoveShop2 = Convert.ToUInt16(TB_MoveShop2.Text);

            pkm.IsPresentInGame = CHK_IsPresentInGame.Checked;
            pkm.Field_45 = CHK_Field_45.Checked;
            //pkm.IsRegionalForm = CHK_Variant.Checked;

            /*for (int i = 0; i < CLB_TM.Items.Count; i++)
                pkm.TMHM[i] = CLB_TM.GetItemChecked(i);
            for (int i = 0; i < CLB_TypeTutor.Items.Count; i++)
                pkm.TypeTutors[i] = CLB_TypeTutor.GetItemChecked(i);
            for (int i = 0; i < CLB_SpecialTutor.Items.Count; i++)
                pkm.SpecialTutors[0][i] = CLB_SpecialTutor.GetItemChecked(i);*/
        }

        public void LoadLearnset(Learnset8aMeta pkm)
        {
            cLearnset = pkm;
            dgv.Rows.Clear();
            if (pkm.Arceus.Length == 0)
            {
                dgv.CancelEdit();
                return;
            }
            dgv.Rows.Add(pkm.Arceus.Length);

            // Fill Entries
            for (int i = 0; i < pkm.Arceus.Length; i++)
            {
                dgv.Rows[i].Cells[0].Value = pkm.Arceus[i].Level;
                dgv.Rows[i].Cells[1].Value = movelist[pkm.Arceus[i].LevelMaster];
                dgv.Rows[i].Cells[2].Value = movelist[pkm.Arceus[i].Move];
            }

            dgv.CancelEdit();
        }

        public void SaveLearnset()
        {
            // TODO
            var pkm = cLearnset;
            List<int> moves = new();
            List<int> levelMaster = new();
            List<int> levels = new();
            for (int i = 0; i < dgv.Rows.Count - 1; i++)
            {
                int move = Array.IndexOf(movelist, dgv.Rows[i].Cells[2].Value);
                if (move < 1)
                    continue;

                moves.Add((short)move);
                string level = (dgv.Rows[i].Cells[0].Value ?? 0).ToString();
                int.TryParse(level, out var lv);
                levels.Add(Math.Min(100, lv));
            }
            //pkm.Update(moves.ToArray(), levels.ToArray());
        }

        public void LoadEvolutions(EvolutionSet8a s)
        {
            cEvos = s;

            if (cEvos.Table == null)
                return;

            var numPossibleEvos = cEvos.Table.Length;
            InitEvo(numPossibleEvos);

            for (int i = 0; i < numPossibleEvos; i++)
            {
                var row = EvoRows[i];
                row.LoadEvolution(cEvos.Table[i]);
            }
        }

        public void SaveEvolutions()
        {
            var s = cEvos;
            Debug.Assert(s.Table != null && EvoRows.Length == s.Table.Length);
            foreach (var row in EvoRows)
                row.SaveEvolution();
        }


        private void B_PDumpTable_Click(object sender, EventArgs e)
        {
            var arr = Editor.Personal.Table;
            var result = TableUtil.GetNamedTypeTable(arr, entryNames, "Species");
            Clipboard.SetText(result);
            System.Media.SystemSounds.Asterisk.Play();
        }

        private void B_RandPersonal_Click(object sender, EventArgs e)
        {
            /*SaveCurrent();
            var settings = (PersonalRandSettings)PG_Personal.SelectedObject;
            var rand = new PersonalRandomizer(Editor.Personal, ROM.Info, Editor.Evolve.LoadAll()) { Settings = settings };
            rand.Execute();
            LoadIndex(CB_Species.SelectedIndex);
            System.Media.SystemSounds.Asterisk.Play();*/
        }

        private void B_AmpExperience_Click(object sender, EventArgs e)
        {
            SaveCurrent();
            decimal rate = NUD_AmpEXP.Value;
            foreach (var p in Editor.Personal.Table)
                p.BaseEXP = (ushort)Math.Max(0, Math.Min(byte.MaxValue, p.BaseEXP * rate));
            LoadIndex(CB_Species.SelectedIndex);
            System.Media.SystemSounds.Asterisk.Play();
        }

        private void B_RandEvo_Click(object sender, EventArgs e)
        {
            /*SaveCurrent();
            var settings = (SpeciesSettings)PG_Evolution.SelectedObject;
            if (ROM.Info.GG)
                settings.Gen2 = settings.Gen3 = settings.Gen4 = settings.Gen5 = settings.Gen6 = settings.Gen7 = settings.Gen8 = false;
            var rand = new EvolutionRandomizer(ROM.Info, Editor.Evolve.LoadAll(), Editor.Personal);
            int[] ban = Array.Empty<int>();

            if (ROM.Info.SWSH)
            {
                var pt = Data.PersonalData;
                ban = pt.Table.Take(ROM.Info.MaxSpeciesID + 1)
                    .Select((z, i) => new { Species = i, Present = ((PersonalInfoSWSH)z).IsPresentInGame })
                    .Where(z => !z.Present).Select(z => z.Species).ToArray();
            }

            rand.RandSpec.Initialize(settings, ban);
            rand.Execute();
            LoadIndex(CB_Species.SelectedIndex);
            System.Media.SystemSounds.Asterisk.Play();*/
        }

        private void B_TradeEvo_Click(object sender, EventArgs e)
        {
            /*SaveCurrent();
            var settings = (SpeciesSettings)PG_Evolution.SelectedObject;
            if (ROM.Info.GG)
                settings.Gen2 = settings.Gen3 = settings.Gen4 = settings.Gen5 = settings.Gen6 = settings.Gen7 = settings.Gen8 = false;
            var rand = new EvolutionRandomizer(ROM.Info, Editor.Evolve.LoadAll(), Editor.Personal);
            rand.RandSpec.Initialize(settings);
            rand.ExecuteTrade();
            LoadIndex(CB_Species.SelectedIndex);
            System.Media.SystemSounds.Asterisk.Play();*/
        }

        private void B_EvolveEveryLevel_Click(object sender, EventArgs e)
        {
            /*SaveCurrent();
            var settings = (SpeciesSettings)PG_Evolution.SelectedObject;
            if (ROM.Info.GG)
                settings.Gen2 = settings.Gen3 = settings.Gen4 = settings.Gen5 = settings.Gen6 = settings.Gen7 = settings.Gen8 = false;
            var rand = new EvolutionRandomizer(ROM.Info, Editor.Evolve.LoadAll(), Editor.Personal);
            int[] ban = Array.Empty<int>();

            if (ROM.Info.SWSH)
            {
                var pt = Data.PersonalData;
                ban = pt.Table.Take(ROM.Info.MaxSpeciesID + 1)
                    .Select((z, i) => new { Species = i, Present = ((PersonalInfoSWSH)z).IsPresentInGame })
                    .Where(z => !z.Present).Select(z => z.Species).ToArray();
            }

            rand.RandSpec.Initialize(settings, ban);
            rand.ExecuteEvolveEveryLevel();
            rand.Execute(); // randomize right after
            LoadIndex(CB_Species.SelectedIndex);
            System.Media.SystemSounds.Asterisk.Play();*/
        }

        private void B_RandLearn_Click(object sender, EventArgs e)
        {
            /*SaveCurrent();
            var settings = (LearnSettings)PG_Learn.SelectedObject;
            var moveset = (MovesetRandSettings)PG_Move.SelectedObject;
            var rand = new LearnsetRandomizer(ROM.Info, Editor.Learn.LoadAll(), Editor.Personal);
            var moves = Data.MoveData.LoadAll();
            int[] banned = Legal.GetBannedMoves(ROM.Info.Game, moves.Length);
            rand.Initialize(moves, settings, moveset, banned);
            rand.Execute();
            LoadIndex(CB_Species.SelectedIndex);
            System.Media.SystemSounds.Asterisk.Play();*/
        }

        private void B_LearnExpand_Click(object sender, EventArgs e)
        {
            /*var settings = (LearnSettings)PG_Learn.SelectedObject;
            if (!settings.Expand)
            {
                WinFormsUtil.Error("Expand moves not selected. Please double check settings.",
                    "Not expanding learnsets.");
                return;
            }
            var moveset = (MovesetRandSettings)PG_Move.SelectedObject;
            var rand = new LearnsetRandomizer(ROM.Info, Editor.Learn.LoadAll(), Editor.Personal);
            rand.Initialize(Data.MoveData.LoadAll(), settings, moveset);
            rand.ExecuteExpandOnly();
            LoadIndex(CB_Species.SelectedIndex);
            System.Media.SystemSounds.Asterisk.Play();*/
        }

        private void B_LearnMetronome_Click(object sender, EventArgs e)
        {
            /*var settings = (LearnSettings)PG_Learn.SelectedObject;
            var moveset = (MovesetRandSettings)PG_Move.SelectedObject;
            var rand = new LearnsetRandomizer(ROM.Info, Editor.Learn.LoadAll(), Editor.Personal);
            rand.Initialize(Data.MoveData.LoadAll(), settings, moveset);
            rand.ExecuteMetronome();
            LoadIndex(CB_Species.SelectedIndex);
            System.Media.SystemSounds.Asterisk.Play();*/
        }

        private void B_Save_Click(object sender, EventArgs e)
        {
            SaveCurrent();
            Modified = true;
            Close();
        }
    }
}
