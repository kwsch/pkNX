using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using pkNX.Game;
using pkNX.Structures;
using pkNX.Sprites;

namespace pkNX.WinForms
{
    public partial class BTTE : Form
    {
        private string[][] AltForms;
        private int entry = -1;
        private PictureBox[] pba;

        private readonly PersonalTable Personal;
        private readonly GameManager Game;
        private readonly TrainerEditor Trainers;

        private readonly string[] abilitylist;
        private readonly string[] movelist;
        private readonly string[] itemlist;
        private readonly string[] specieslist;
        private readonly string[] types;
        private readonly string[] natures;
        private readonly string[] trName;
        private readonly string[] trClass;

        public BTTE(GameManager game, TrainerEditor editor)
        {
            InitializeComponent();

            Stats.Personal = Personal = game.Data.PersonalData;
            Game = game;
            Trainers = editor;

            trClass = Game.GetStrings(TextName.TrainerClasses);
            trName = Game.GetStrings(TextName.TrainerClasses);

            abilitylist = Game.GetStrings(TextName.AbilityNames);
            movelist = Game.GetStrings(TextName.MoveNames);
            itemlist = Game.GetStrings(TextName.ItemNames);
            specieslist = Game.GetStrings(TextName.SpeciesNames);
            types = Game.GetStrings(TextName.Types);
            natures = Game.GetStrings(TextName.Natures);
            trName = Game.GetStrings(TextName.TrainerNames);
            trClass = Game.GetStrings(TextName.TrainerClasses);

            mnuView.Click += ClickView;
            mnuSet.Click += ClickSet;
            mnuDelete.Click += ClickDelete;
            Setup();
            foreach (var pb in pba)
                pb.Click += ClickSlot;

            CB_TrainerID.SelectedIndex = 0;
        }

        public bool Modified { get; set; }

        private int GetSlot(object sender)
        {
            var send = ((sender as ToolStripItem)?.Owner as ContextMenuStrip)?.SourceControl ?? sender as PictureBox;
            return Array.IndexOf(pba, send);
        }

        private void ClickSlot(object sender, EventArgs e)
        {
            switch (ModifierKeys)
            {
                case Keys.Control: ClickView(sender, e); break;
                case Keys.Shift: ClickSet(sender, e); break;
                case Keys.Alt: ClickDelete(sender, e); break;
            }
        }

        private void ClickView(object sender, EventArgs e)
        {
            int slot = GetSlot(sender);
            if (pba[slot].Image == null)
            {
                System.Media.SystemSounds.Exclamation.Play();
                return;
            }

            // Load the PKM
            var pk = Trainers[entry].Team[slot];
            if (pk.Species != 0)
            {
                try { PopulateFields(pk); }
                catch { }
                // Visual to display what slot is currently loaded.
                GetSlotColor(slot, Sprites.Properties.Resources.slotView);
            }
            else
            {
                System.Media.SystemSounds.Exclamation.Play();
            }
        }

        private void ClickSet(object sender, EventArgs e)
        {
            int slot = GetSlot(sender);
            if (CB_Species.SelectedIndex == 0)
            { WinFormsUtil.Alert("Can't set empty slot."); return; }

            var pk = PreparePKM();
            var tr = Trainers[entry];
            if (slot < tr.Team.Count)
            {
                tr.Team[slot] = pk;
            }
            else
            {
                tr.Team.Add(pk);
                slot = tr.Team.Count - 1;
            }

            GetQuickFiller(pba[slot], pk);
            GetSlotColor(slot, Sprites.Properties.Resources.slotSet);
        }

        private void ClickDelete(object sender, EventArgs e)
        {
            int slot = GetSlot(sender);

            if (slot < Trainers[entry].Team.Count)
                Trainers[entry].Team.RemoveAt(slot);

            PopulateTeam(Trainers[entry].Team);
            GetSlotColor(slot, Sprites.Properties.Resources.slotDel);
        }

        private void PopulateTeam(IList<TrainerPoke> team)
        {
            for (int i = 0; i < team.Count; i++)
                GetQuickFiller(pba[i], team[i]);
            for (int i = team.Count; i < 6; i++)
                pba[i].Image = null;
        }

        private void GetSlotColor(int slot, Image color)
        {
            foreach (PictureBox t in pba)
                t.BackgroundImage = null;

            pba[slot].BackgroundImage = color;
        }

        private static void GetQuickFiller(PictureBox pb, TrainerPoke pk)
        {
            var rawImg = SpriteBuilder.GetSprite(pk.Species, pk.Form, pk.Gender, pk.HeldItem, false, pk.Shiny);
            pb.Image = ImageUtil.ScaleImage((Bitmap)rawImg, 2);
        }

        // Top Level Functions
        private void RefreshFormAbility(object sender, EventArgs e)
        {
            if (entry < 0)
                return;
            pkm.Form = CB_Forme.SelectedIndex;
            RefreshPKMSlotAbility();

            if (!Stats.UpdatingFields)
                Stats.UpdateStats();
        }

        private void RefreshSpeciesAbility(object sender, EventArgs e)
        {
            if (entry < 0)
                return;
            pkm.Species = (ushort)CB_Species.SelectedIndex;
            FormUtil.SetForms(CB_Species.SelectedIndex, CB_Forme, AltForms);
            RefreshPKMSlotAbility();

            if (!Stats.UpdatingFields)
                Stats.UpdateStats();
        }

        private void RefreshPKMSlotAbility()
        {
            int previousAbilityIndex = CB_Ability.SelectedIndex;

            int species = CB_Species.SelectedIndex;
            int formnum = CB_Forme.SelectedIndex;
            int index = Personal[species].FormeIndex(species, formnum);

            CB_Ability.Items.Clear();
            CB_Ability.Items.Add("Any (1 or 2)");
            CB_Ability.Items.Add(abilitylist[Personal[index].Abilities[0]] + " (1)");
            CB_Ability.Items.Add(abilitylist[Personal[index].Abilities[1]] + " (2)");
            CB_Ability.Items.Add(abilitylist[Personal[index].Abilities[2]] + " (H)");

            CB_Ability.SelectedIndex = previousAbilityIndex;
        }

        private static string GetEntryTitle(string str, int i) => $"{str} - {i:000}";

        private void Setup()
        {
            AltForms = new byte[Personal.TableLength].Select(_ => Enumerable.Range(0, 32).Select(i => i.ToString()).ToArray()).ToArray();
            CB_TrainerID.Items.Clear();
            for (int i = 0; i < Trainers.Length; i++)
                CB_TrainerID.Items.Add(GetEntryTitle(trName[i] ?? "UNKNOWN", i));

            CB_Trainer_Class.Items.Clear();
            for (int i = 0; i < trClass.Length; i++)
                CB_Trainer_Class.Items.Add(GetEntryTitle(trClass[i], i));

            specieslist[0] = "---";
            abilitylist[0] = itemlist[0] = movelist[0] = "(None)";
            pba = new[] { PB_Team1, PB_Team2, PB_Team3, PB_Team4, PB_Team5, PB_Team6 };

            CB_Species.Items.Clear();
            foreach (string s in specieslist)
                CB_Species.Items.Add(s);

            CB_Move1.Items.Clear();
            CB_Move2.Items.Clear();
            CB_Move3.Items.Clear();
            CB_Move4.Items.Clear();
            foreach (string s in movelist)
            {
                CB_Move1.Items.Add(s);
                CB_Move2.Items.Add(s);
                CB_Move3.Items.Add(s);
                CB_Move4.Items.Add(s);
            }

            Stats.Initialize(types);

            CB_Nature.Items.Clear();
            CB_Nature.Items.AddRange(natures.Take(25).ToArray());

            CB_Item.Items.Clear();
            foreach (string s in itemlist)
                CB_Item.Items.Add(s);

            CB_Gender.Items.Clear();
            CB_Gender.Items.Add("- / Genderless/Random");
            CB_Gender.Items.Add("♂ / Male");
            CB_Gender.Items.Add("♀ / Female");

            CB_Forme.Items.Add("");

            CB_Species.SelectedIndex = 0;
            CB_Item_1.Items.Clear();
            CB_Item_2.Items.Clear();
            CB_Item_3.Items.Clear();
            CB_Item_4.Items.Clear();
            foreach (string s in itemlist)
            {
                CB_Item_1.Items.Add(s);
                CB_Item_2.Items.Add(s);
                CB_Item_3.Items.Add(s);
                CB_Item_4.Items.Add(s);
            }

            CB_Money.Items.Clear();
            for (int i = 0; i < 256; i++)
            { CB_Money.Items.Add(i.ToString()); }

            CHK_CanMega.CheckedChanged += (s, e) => NUD_MegaForm.Visible = CHK_CanMega.Checked;
            NUD_MegaForm.Visible = false;

            CB_TrainerID.SelectedIndex = 0;
            entry = 0;
            pkm = new TrainerPoke7b();
            PopulateFields(pkm);
        }

        private void ChangeTrainerIndex(object sender, EventArgs e)
        {
            SaveEntry();
            LoadEntry();
            if (TC_trdata.SelectedIndex == TC_trdata.TabCount - 1) // last
                TC_trdata.SelectedIndex = 0;
        }

        private void SaveEntry()
        {
            if (entry < 0)
                return;
            var tr = Trainers[entry];
            PrepareTrainer(tr.Self);
        }

        private bool loading;

        private void LoadEntry()
        {
            entry = CB_TrainerID.SelectedIndex;
            var tr = Trainers[entry];

            loading = true;

            PopulateFieldsTrainer(tr.Self);
            PopulateTeam(tr.Team);
            loading = false;
        }

        private void UpdateTrainerName(object sender, EventArgs e)
        {
            if (loading)
                return;
            string str = TB_TrainerName.Text;
            CB_TrainerID.Items[entry] = GetEntryTitle(str, entry);
        }

        private TrainerPoke pkm;

        private void PopulateFields(TrainerPoke pk)
        {
            pkm = pk.Clone();

            Stats.UpdatingFields = true;

            CB_Species.SelectedIndex = pkm.Species;
            CB_Forme.SelectedIndex = pkm.Form;
            CB_Ability.SelectedIndex = pkm.Ability;
            CB_Nature.SelectedIndex = pkm.Nature;
            NUD_Level.Value = Math.Min(NUD_Level.Maximum, pkm.Level);
            CB_Item.SelectedIndex = pkm.HeldItem;
            CHK_Shiny.Checked = pkm.Shiny;
            CB_Gender.SelectedIndex = pkm.Gender;

            CB_Move1.SelectedIndex = pkm.Move1;
            CB_Move2.SelectedIndex = pkm.Move2;
            CB_Move3.SelectedIndex = pkm.Move3;
            CB_Move4.SelectedIndex = pkm.Move4;

            if (pk is TrainerPoke7b b)
            {
                CHK_CanMega.Checked = b.CanMegaEvolve;
                NUD_MegaForm.Value = b.MegaFormChoice;
            }
            else
            {
                flowLayoutPanel8.Visible = flowLayoutPanel9.Visible = false;
            }

            Stats.LoadStats(pk);
        }

        private TrainerPoke PreparePKM()
        {
            var pk = pkm.Clone();
            pk.Species = CB_Species.SelectedIndex;
            pk.Form = CB_Forme.SelectedIndex;
            pk.Level = (byte)NUD_Level.Value;
            pk.Ability = CB_Ability.SelectedIndex;
            pk.HeldItem = CB_Item.SelectedIndex;
            pk.Shiny = CHK_Shiny.Checked;
            pk.Nature = CB_Nature.SelectedIndex;
            pk.Gender = CB_Gender.SelectedIndex;

            pk.Move1 = CB_Move1.SelectedIndex;
            pk.Move2 = CB_Move2.SelectedIndex;
            pk.Move3 = CB_Move3.SelectedIndex;
            pk.Move4 = CB_Move4.SelectedIndex;

            if (pk is TrainerPoke7b b)
            {
                b.CanMegaEvolve = CHK_CanMega.Checked;
                b.MegaFormChoice = (int) NUD_MegaForm.Value;
            }

            return pk;
        }

        private void PopulateFieldsTrainer(TrainerData tr)
        {
            // Load Trainer Data
            CB_Trainer_Class.SelectedIndex = tr.Class;
            CB_Item_1.SelectedIndex = tr.Item1;
            CB_Item_2.SelectedIndex = tr.Item2;
            CB_Item_3.SelectedIndex = tr.Item3;
            CB_Item_4.SelectedIndex = tr.Item4;
            CB_Money.SelectedIndex = tr.Money;
            NUD_AI.Value = (int)tr.AI;
        }

        private void PrepareTrainer(TrainerData tr)
        {
            tr.Class = (byte)CB_Trainer_Class.SelectedIndex;
            tr.Item1 = CB_Item_1.SelectedIndex;
            tr.Item2 = CB_Item_2.SelectedIndex;
            tr.Item3 = CB_Item_3.SelectedIndex;
            tr.Item4 = CB_Item_4.SelectedIndex;
            tr.Money = CB_Money.SelectedIndex;
            tr.AI = (uint)NUD_AI.Value;
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            SaveEntry();
            base.OnFormClosing(e);
        }

        private void DumpTxt(object sender, EventArgs e)
        {
            using (var sfd = new SaveFileDialog())
            {
                sfd.FileName = "Trainers.txt";
                if (sfd.ShowDialog() != DialogResult.OK)
                    return;
                var sb = new StringBuilder();
                for (int i = 0; i < Trainers.Length; i++)
                    sb.Append(GetTrainerString(Trainers[i]));
                File.WriteAllText(sfd.FileName, sb.ToString());
            }
        }

        private string GetTrainerString(VsTrainer Trainer)
        {
            var file = Trainer.ID;
            var tr = Trainer.Self;
            var name = Trainer.Name;
            var team = Trainer.Team;
            var sb = new StringBuilder();
            sb.AppendLine("======");
            sb.Append(file).Append(" - ").Append(trClass[tr.Class]).Append(" ").AppendLine(name);
            sb.AppendLine("======");
            sb.Append("Pokemon: ").Append(tr.NumPokemon).AppendLine();
            for (int i = 0; i < tr.NumPokemon; i++)
            {
                if (team[i].Shiny)
                    sb.Append("Shiny ");
                sb.Append(specieslist[team[i].Species]);
                sb.Append(" (Lv. ").Append(team[i].Level).Append(") ");
                if (team[i].HeldItem > 0)
                    sb.Append("@").Append(itemlist[team[i].HeldItem]);

                if (team[i].Nature != 0)
                    sb.Append(" (Nature: ").Append(natures[team[i].Nature]).Append(")");

                sb.Append(" (Moves: ").Append(string.Join("/", team[i].Moves.Select(m => m == 0 ? "(None)" : movelist[m]))).Append(")");
                sb.Append(" IVs: ").Append(string.Join("/", team[i].IVs));
                sb.Append(" EVs: ").Append(string.Join("/", team[i].EVs));
                sb.AppendLine();
            }
            return sb.ToString();
        }

        private void UpdateStats(object sender, EventArgs e)
        {
            if (Stats.UpdatingFields)
                return;
            if (sender == CB_Nature)
                pkm.Nature = WinFormsUtil.GetIndex(CB_Nature);
            else if (sender == NUD_Level)
                pkm.Level = (int) NUD_Level.Value;
            else if (sender == NUD_Friendship)
                pkm.Friendship = (int) NUD_Friendship.Value;

            Stats.UpdateStats();
        }

        private void B_HighAttack_Click(object sender, EventArgs e)
        {
            pkm.Species = CB_Species.SelectedIndex;
            pkm.Level = (int)NUD_Level.Value;
            pkm.Form = CB_Forme.SelectedIndex;
            //var moves = learn.GetHighPoweredMoves(pkm.Species, pkm.Form, 4);
            //SetMoves(moves);
        }

        private void B_CurrentAttack_Click(object sender, EventArgs e)
        {
            pkm.Species = CB_Species.SelectedIndex;
            pkm.Level = (int)NUD_Level.Value;
            pkm.Form = CB_Forme.SelectedIndex;
            //var moves = learn.GetCurrentMoves(pkm.Species, pkm.Form, pkm.Level, 4);
            //SetMoves(moves);
        }

        private void B_Clear_Click(object sender, EventArgs e) => SetMoves(new int[4]);

        private void SetMoves(IList<int> moves)
        {
            var mcb = new[] { CB_Move1, CB_Move2, CB_Move3, CB_Move4 };
            for (int i = 0; i < mcb.Length; i++)
                mcb[i].SelectedIndex = moves[i];
        }
    }

    public static class FormUtil
    {
        // Utility (Shared)
        internal static void SetForms(int species, ComboBox cb, string[][] AltForms)
        {
            cb.Items.Clear();
            string[] forms = AltForms[species];
            if (forms.Length < 2)
            {
                cb.Items.Add("");
                cb.Enabled = false;
            }
            else
            {
                foreach (string s in forms)
                    cb.Items.Add(s);
                cb.Enabled = true;
            }
            cb.SelectedIndex = 0;
        }
    }
}
