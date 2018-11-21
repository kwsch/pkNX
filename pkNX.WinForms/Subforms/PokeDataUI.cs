using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using pkNX.Game;
using pkNX.Sprites;
using pkNX.Structures;
using Util = pkNX.Randomization.Util;

namespace pkNX.WinForms
{
    public partial class PokeDataUI : Form
    {
        public PokeDataUI(PokeEditor editor, GameManager ROM)
        {
            Editor = editor;
            InitializeComponent();

            helditem_boxes = new[] { CB_HeldItem1, CB_HeldItem2, CB_HeldItem3 };
            ability_boxes = new[] { CB_Ability1, CB_Ability2, CB_Ability3 };
            typing_boxes = new[] { CB_Type1, CB_Type2 };
            eggGroup_boxes = new[] { CB_EggGroup1, CB_EggGroup2 };

            items = ROM.GetStrings(TextName.ItemNames);
            movelist = ROM.GetStrings(TextName.MoveNames);
            species = ROM.GetStrings(TextName.SpeciesNames);
            abilities = ROM.GetStrings(TextName.AbilityNames);
            types = ROM.GetStrings(TextName.Types);

            string[] ps = { "P", "S" }; // Distinguish Physical/Special Z Moves
            for (int i = 622; i < 658; i++)
                movelist[i] += $" ({ps[i % 2]})";

            species[0] = "---";
            abilities[0] = items[0] = movelist[0] = "";

            var pt = ROM.Data.PersonalData;

            var altForms = pt.GetFormList(species, pt.MaxSpeciesID);
            entryNames = pt.GetPersonalEntryList(altForms, species, pt.MaxSpeciesID, out baseForms, out formVal);

            InitPersonal();
            InitLearn();
            InitEvo(8);
            InitMega(2);

            CB_Species.SelectedIndex = 1;
        }

        public PokeEditor Editor { get; set; }
        public bool Modified { get; set; }

        private readonly ComboBox[] helditem_boxes;
        private readonly ComboBox[] ability_boxes;
        private readonly ComboBox[] typing_boxes;
        private readonly ComboBox[] eggGroup_boxes;

        private readonly string[] items;
        private readonly string[] movelist;
        private readonly string[] species;
        private readonly string[] abilities;
        private readonly string[] types;

        private readonly string[] entryNames;
        private readonly int[] baseForms, formVal;

        public PersonalInfo cPersonal;
        public Learnset cLearnset;
        public EvolutionSet cEvos;
        public MegaEvolutionSet[] cMega;

        public void InitPersonal()
        {
            var TMs = GetTMList();
            if (TMs.Length == 0) // No ExeFS to grab TMs from.
            {
                for (int i = 0; i < TMs.Length; i++)
                    CLB_TM.Items.Add($"TM{i+1:00}");
            }
            else // Use TM moves.
            {
                for (int i = 0; i < 100; i++)
                    CLB_TM.Items.Add($"TM{i+1:00} {movelist[TMs[i]]}");
            }

            var entries = entryNames.Select((z, i) => $"{z} - {i:000}");
            CB_Species.Items.AddRange(entries.ToArray());

            foreach (ComboBox cb in helditem_boxes)
                cb.Items.AddRange(items);

            CB_ZItem.Items.AddRange(items);
            CB_ZBaseMove.Items.AddRange(movelist);
            CB_ZMove.Items.AddRange(movelist);

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
            DataGridViewComboBoxColumn dgvMove = new DataGridViewComboBoxColumn();
            {
                dgvMove.HeaderText = "Move";
                dgvMove.DisplayIndex = 1;
                for (int i = 0; i < movelist.Length; i++)
                    dgvMove.Items.Add(sortedmoves[i]); // add only the Names

                dgvMove.Width = 135;
                dgvMove.FlatStyle = FlatStyle.Flat;
            }
            dgv.Columns.Add(dgvLevel);
            dgv.Columns.Add(dgvMove);
        }

        private static EvolutionRow[] EvoRows;

        public void InitEvo(int rows)
        {
            EvoRows = new EvolutionRow[rows];
            EvolutionRow.species = species;
            EvolutionRow.items = items;
            EvolutionRow.movelist = movelist;
            EvolutionRow.types = types;

            for (int i = 0; i < rows; i++)
            {
                var row = new EvolutionRow();
                flowLayoutPanel1.Controls.Add(row);
                flowLayoutPanel1.SetFlowBreak(row, true);
                EvoRows[i] = row;
            }
        }

        private MegaEvoEntry[] Megas;

        public void InitMega(int count)
        {
            Megas = new MegaEvoEntry[count];
            MegaEvoEntry.items = items;

            for (int i = 0; i < count; i++)
            {
                var row = new MegaEvoEntry();
                flowLayoutPanel1.Controls.Add(row);
                Megas[i] = row;
            }
        }

        private static int[] GetTMList()
        {
            return new[]
            {
                029, 269, 270, 100, 156, 113, 182, 164, 115, 091,
                261, 263, 280, 019, 069, 086, 525, 369, 231, 399,
                492, 157, 009, 404, 127, 398, 092, 161, 503, 339,
                007, 605, 347, 406, 008, 085, 053, 087, 200, 094,
                089, 120, 247, 583, 076, 126, 057, 063, 276, 355,
                059, 188, 072, 430, 058, 446, 006, 529, 138, 224,
                // rest are same as SM, unused

                // No HMs
            };
        }

        public void UpdateIndex(object sender, EventArgs e)
        {
            SaveCurrent();
            LoadIndex(CB_Species.SelectedIndex);
        }

        private void LoadIndex(int index)
        {
            int spec = baseForms[index];
            var form = formVal[index];
            LoadPersonal(Editor.Personal[index]);
            LoadLearnset(Editor.Learn[index]);
            LoadEvolutions(Editor.Evolve[index]);
            LoadMegas(Editor.Mega[index], spec);
            PB_MonSprite.Image = SpriteBuilder.GetSprite(spec, form, 0, 0, false, false);
        }

        private void SaveCurrent()
        {
            SavePersonal();
            SaveLearnset();
            SaveEvolutions();
            SaveMegas();
        }

        public void LoadPersonal(PersonalInfo pkm)
        {
            cPersonal = pkm;
            TB_BaseHP.Text = pkm.HP.ToString("000");
            TB_BaseATK.Text = pkm.ATK.ToString("000");
            TB_BaseDEF.Text = pkm.DEF.ToString("000");
            TB_BaseSPE.Text = pkm.SPE.ToString("000");
            TB_BaseSPA.Text = pkm.SPA.ToString("000");
            TB_BaseSPD.Text = pkm.SPD.ToString("000");
            TB_HPEVs.Text = pkm.EV_HP.ToString("0");
            TB_ATKEVs.Text = pkm.EV_ATK.ToString("0");
            TB_DEFEVs.Text = pkm.EV_DEF.ToString("0");
            TB_SPEEVs.Text = pkm.EV_SPE.ToString("0");
            TB_SPAEVs.Text = pkm.EV_SPA.ToString("0");
            TB_SPDEVs.Text = pkm.EV_SPD.ToString("0");

            CB_Type1.SelectedIndex = pkm.Types[0];
            CB_Type2.SelectedIndex = pkm.Types[1];

            TB_CatchRate.Text = pkm.CatchRate.ToString("000");
            TB_Stage.Text = pkm.EvoStage.ToString("0");

            CB_HeldItem1.SelectedIndex = pkm.Items[0];
            CB_HeldItem2.SelectedIndex = pkm.Items[1];
            CB_HeldItem3.SelectedIndex = pkm.Items[2];

            TB_Gender.Text = pkm.Gender.ToString("000");
            TB_HatchCycles.Text = pkm.HatchCycles.ToString("000");
            TB_Friendship.Text = pkm.BaseFriendship.ToString("000");

            CB_EXPGroup.SelectedIndex = pkm.EXPGrowth;

            CB_EggGroup1.SelectedIndex = pkm.EggGroups[0];
            CB_EggGroup2.SelectedIndex = pkm.EggGroups[1];

            CB_Ability1.SelectedIndex = pkm.Abilities[0];
            CB_Ability2.SelectedIndex = pkm.Abilities[1];
            CB_Ability3.SelectedIndex = pkm.Abilities[2];

            TB_FormeCount.Text = pkm.FormeCount.ToString("000");
            TB_FormeSprite.Text = pkm.FormeSprite.ToString("000");

            TB_RawColor.Text = pkm.Color.ToString("000");
            CB_Color.SelectedIndex = pkm.Color & 0xF;

            TB_BaseExp.Text = pkm.BaseEXP.ToString("000");
            TB_BST.Text = pkm.BST.ToString("000");

            TB_Height.Text = ((decimal)pkm.Height / 100).ToString("00.00");
            TB_Weight.Text = ((decimal)pkm.Weight / 10).ToString("000.0");

            if (pkm is PersonalInfoSM sm)
            {
                TB_CallRate.Text = sm.EscapeRate.ToString("000");
                CB_ZItem.SelectedIndex = sm.SpecialZ_Item;
                CB_ZBaseMove.SelectedIndex = sm.SpecialZ_BaseMove;
                CB_ZMove.SelectedIndex = sm.SpecialZ_ZMove;
                CHK_Variant.Checked = sm.LocalVariant;
            }
            if (pkm is PersonalInfoGG gg)
            {
                MT_GoID.Text = gg.GoSpecies.ToString("000");
            }

            for (int i = 0; i < CLB_TM.Items.Count; i++)
                CLB_TM.SetItemChecked(i, pkm.TMHM[i]); // Bitflags for TM
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

            pkm.CatchRate = Util.ToInt32(TB_CatchRate.Text);
            pkm.EvoStage = Util.ToInt32(TB_Stage.Text);

            pkm.Types = new[] { CB_Type1.SelectedIndex, CB_Type2.SelectedIndex };
            pkm.Items = new[] { CB_HeldItem1.SelectedIndex, CB_HeldItem2.SelectedIndex, CB_HeldItem3.SelectedIndex };

            pkm.Gender = Util.ToInt32(TB_Gender.Text);
            pkm.HatchCycles = Util.ToInt32(TB_HatchCycles.Text);
            pkm.BaseFriendship = Util.ToInt32(TB_Friendship.Text);
            pkm.EXPGrowth = (byte)CB_EXPGroup.SelectedIndex;
            pkm.EggGroups = new[] { CB_EggGroup1.SelectedIndex, CB_EggGroup2.SelectedIndex };
            pkm.Abilities = new[] { CB_Ability1.SelectedIndex, CB_Ability2.SelectedIndex, CB_Ability3.SelectedIndex };

            pkm.FormeSprite = Convert.ToUInt16(TB_FormeSprite.Text);
            pkm.FormeCount = Util.ToInt32(TB_FormeCount.Text);
            pkm.Color = (byte)(CB_Color.SelectedIndex) | (Util.ToInt32(TB_RawColor.Text) & 0xF0);
            pkm.BaseEXP = Convert.ToUInt16(TB_BaseExp.Text);

            if (decimal.TryParse(TB_Height.Text, out decimal h))
                pkm.Height = (int)(h * 100);

            if (decimal.TryParse(TB_Weight.Text, out decimal w))
                pkm.Weight = (int)(w * 10);

            if (pkm is PersonalInfoSM sm)
            {
                pkm.EscapeRate = Util.ToInt32(TB_CallRate.Text);
                sm.SpecialZ_Item = CB_ZItem.SelectedIndex;
                sm.SpecialZ_BaseMove = CB_ZBaseMove.SelectedIndex;
                sm.SpecialZ_ZMove = CB_ZMove.SelectedIndex;
                sm.LocalVariant = CHK_Variant.Checked;
            }
            if (pkm is PersonalInfoGG gg)
            {
                gg.GoSpecies = Convert.ToUInt16(MT_GoID.Text);
            }

            for (int i = 0; i < CLB_TM.Items.Count; i++)
                pkm.TMHM[i] = CLB_TM.GetItemChecked(i);
        }

        public void LoadLearnset(Learnset pkm)
        {
            cLearnset = pkm;
            while (pkm.Count < dgv.Rows.Count)
                dgv.Rows.RemoveAt(dgv.Rows.Count - 1);
            int diff = pkm.Count - dgv.Rows.Count;
            if (diff != 0)
                dgv.Rows.Add(diff);

            // Fill Entries
            for (int i = 0; i < pkm.Count; i++)
            {
                dgv.Rows[i].Cells[0].Value = pkm.Levels[i];
                dgv.Rows[i].Cells[1].Value = movelist[pkm.Moves[i]];
            }

            dgv.CancelEdit();
        }

        public void SaveLearnset()
        {
            var pkm = cLearnset;
            List<int> moves = new List<int>();
            List<int> levels = new List<int>();
            for (int i = 0; i < dgv.Rows.Count - 1; i++)
            {
                int move = Array.IndexOf(movelist, dgv.Rows[i].Cells[1].Value);
                if (move < 1)
                    continue;

                moves.Add((short)move);
                string level = (dgv.Rows[i].Cells[0].Value ?? 0).ToString();
                int.TryParse(level, out var lv);
                levels.Add(Math.Min(100, lv));
            }
            pkm.Update(moves.ToArray(), levels.ToArray());
        }

        public void LoadEvolutions(EvolutionSet s)
        {
            cEvos = s;
            Debug.Assert(EvoRows.Length == s.PossibleEvolutions.Length);
            for (int i = 0; i < EvoRows.Length; i++)
            {
                var row = EvoRows[i];
                row.LoadEvolution(s.PossibleEvolutions[i]);
            }
        }

        public void SaveEvolutions()
        {
            var s = cEvos;
            Debug.Assert(EvoRows.Length == s.PossibleEvolutions.Length);
            foreach (var row in EvoRows)
                row.SaveEvolution();
        }

        public void LoadMegas(MegaEvolutionSet[] m, int spec)
        {
            cMega = m;
            Debug.Assert(Megas.Length == m.Length);
            for (int i = 0; i < EvoRows.Length; i++)
            {
                var entry = Megas[i];
                entry.LoadEvolution(m[i], spec);
            }
        }

        public void SaveMegas()
        {
            var m = cMega;
            Debug.Assert(Megas.Length == m.Length);
            foreach (var row in Megas)
                row.SaveEvolution();
        }
    }
}
