using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using pkNX.Game;
using pkNX.Randomization;
using pkNX.Structures;
using pkNX.Sprites;

namespace pkNX.WinForms
{
    public partial class BTTE : Form
    {
        private readonly LearnsetRandomizer learn;
        private readonly string[][] AltForms;
        private readonly PictureBox[] pba;

        private int entry = -1;
        private TrainerPoke pkm = new TrainerPoke7b();
        private bool loadingPKM;

        private readonly PersonalTable Personal;
        private readonly GameManager Game;
        private readonly GameData Data;
        private readonly TrainerEditor Trainers;

        private readonly string[] abilitylist;
        private readonly string[] movelist;
        private readonly string[] itemlist;
        private readonly string[] specieslist;
        private readonly string[] types;
        private readonly string[] natures;
        private readonly string[] trName;
        private readonly string[] trClass;

        private readonly CheckBox[] AIBits;

        public BTTE(GameData data, TrainerEditor editor, GameManager game)
        {
            Game = game;
            Data = data;
            Trainers = editor;
            InitializeComponent();
            pba = new[] { PB_Team1, PB_Team2, PB_Team3, PB_Team4, PB_Team5, PB_Team6 };

            Stats.Personal = Personal = data.PersonalData;
            learn = new LearnsetRandomizer(game.Info, data.LevelUpData.LoadAll(), Personal);

            AltForms = new byte[Personal.TableLength]
                .Select(_ => Enumerable.Range(0, 32).Select(i => i.ToString()).ToArray()).ToArray();

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
            movelist = EditorUtil.SanitizeMoveList(movelist);

            AIBits = Game.Info.SWSH ? new[] { CHK_AI_Basic, CHK_AI_Strong, CHK_AI_Expert, CHK_AI_Double, CHK_AI_Raid, CHK_AI_Allowance, CHK_AI_PokeChange, CHK_AI_FireGym1, CHK_AI_FireGym2, CHK_AI_Unused1, CHK_AI_Item, CHK_AI_FireGym3, CHK_AI_Unused2 }
                                    : new[] { CHK_AI_Basic, CHK_AI_Strong, CHK_AI_Expert, CHK_AI_Double, CHK_AI_Allowance, CHK_AI_Item, CHK_AI_PokeChange, CHK_AI_Unused1 };

            mnuView.Click += ClickView;
            mnuSet.Click += ClickSet;
            mnuDelete.Click += ClickDelete;
            Setup();
            foreach (var pb in pba)
                pb.Click += ClickSlot;

            CB_TrainerID.SelectedIndex = 0;

            PG_Moves.SelectedObject = EditUtil.Settings.Move;
            PG_RTrainer.SelectedObject = EditUtil.Settings.Trainer;
            PG_Species.SelectedObject = EditUtil.Settings.Species;

            L_Gift.Visible = CB_Gift.Visible = NUD_GiftCount.Visible = Game.Info.GG;
            GB_Additional_AI.Visible = Game.Info.SWSH;
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
                PopulateFields(pk);
                // Visual to display what slot is currently loaded.
                GetSlotColor(slot, Sprites.Properties.Resources.slotView68);
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
            GetSlotColor(slot, Sprites.Properties.Resources.slotSet68);
        }

        private void ClickDelete(object sender, EventArgs e)
        {
            int slot = GetSlot(sender);

            if (slot < Trainers[entry].Team.Count)
                Trainers[entry].Team.RemoveAt(slot);

            PopulateTeam(Trainers[entry].Team);
            GetSlotColor(slot, Sprites.Properties.Resources.slotDel68);
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
            if (pk is TrainerPoke8 c)
                pb.Image = SpriteUtil.GetSprite(c.Species, c.Form, c.Gender, c.HeldItem, false, c.Shiny, c.CanGigantamax);
            else
                pb.Image = SpriteUtil.GetSprite(pk.Species, pk.Form, pk.Gender, pk.HeldItem, false, pk.Shiny, false);
        }

        // Top Level Functions
        private void RefreshFormAbility(object sender, EventArgs e)
        {
            if (entry < 0)
                return;
            RefreshPKMSlotAbility();
            if (loadingPKM)
                return;
            pkm.Form = CB_Forme.SelectedIndex;

            if (!Stats.UpdatingFields)
                Stats.UpdateStats();
        }

        private void RefreshSpeciesAbility(object sender, EventArgs e)
        {
            if (entry < 0)
                return;
            FormUtil.SetForms(CB_Species.SelectedIndex, CB_Forme, AltForms);
            if (loadingPKM)
                return;
            pkm.Species = (ushort)CB_Species.SelectedIndex;
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

            var abilities = Personal[index].Abilities;
            CB_Ability.Items.Clear();
            CB_Ability.Items.Add("Any (1 or 2)");
            CB_Ability.Items.Add(abilitylist[abilities[0]] + " (1)");
            CB_Ability.Items.Add(abilitylist[abilities[1]] + " (2)");
            CB_Ability.Items.Add(abilitylist[abilities[2]] + " (H)");

            CB_Ability.SelectedIndex = previousAbilityIndex;
        }

        private static string GetEntryTitle(string str, int i) => $"{str} - {i:000}";

        private void Setup()
        {
            CB_TrainerID.Items.Clear();
            for (int i = 0; i < Trainers.Length; i++)
                CB_TrainerID.Items.Add(GetEntryTitle(trName[i], i));

            CB_Trainer_Class.Items.Clear();
            for (int i = 0; i < trClass.Length; i++)
                CB_Trainer_Class.Items.Add(GetEntryTitle(trClass[i], i));

            specieslist[0] = "---";
            abilitylist[0] = itemlist[0] = movelist[0] = "(None)";

            CB_Species.Items.AddRange(specieslist);

            CB_Move1.Items.AddRange(movelist);
            CB_Move2.Items.AddRange(movelist);
            CB_Move3.Items.AddRange(movelist);
            CB_Move4.Items.AddRange(movelist);

            Stats.Initialize(types);
            CB_Nature.Items.Clear();
            CB_Nature.Items.AddRange(natures.Take(25).ToArray());
            CB_Item.Items.AddRange(itemlist);

            CB_Gender.Items.Add("- / Genderless/Random");
            CB_Gender.Items.Add("♂ / Male");
            CB_Gender.Items.Add("♀ / Female");

            CB_Forme.Items.Add("");

            CB_Species.SelectedIndex = 0;
            CB_Item_1.Items.AddRange(itemlist);
            CB_Item_2.Items.AddRange(itemlist);
            CB_Item_3.Items.AddRange(itemlist);
            CB_Item_4.Items.AddRange(itemlist);
            CB_Gift.Items.AddRange(itemlist);

            CB_Money.Items.AddRange(Enumerable.Range(0, 256).Select(z => z.ToString()).ToArray());
            CHK_CanMega.CheckedChanged += (s, e) => NUD_MegaForm.Visible = CHK_CanMega.Checked;
            NUD_MegaForm.Visible = false;

            CB_TrainerID.SelectedIndex = 0;
            entry = 0;
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

        private void PopulateFields(TrainerPoke pk)
        {
            pkm = pk.Clone();

            Stats.UpdatingFields = loadingPKM = true;

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

            if (pkm is TrainerPoke7b b)
            {
                CHK_CanMega.Checked = b.CanMegaEvolve;
                NUD_MegaForm.Value = b.MegaFormChoice;
                NUD_Friendship.Value = b.Friendship;
                FLP_Friendship.Visible = FLP_Mega.Visible = true;
                FLP_HeldItem.Visible = FLP_Ability.Visible = FLP_CanDynamax.Visible = false;
            }
            else if (pkm is TrainerPoke8 c)
            {
                CHK_CanDynamax.Checked = c.CanDynamax;
                Stats.CB_DynamaxLevel.SelectedIndex = c.DynamaxLevel;
                Stats.CHK_Gigantamax.Checked = c.CanGigantamax;
                FLP_Friendship.Visible = FLP_Mega.Visible = false;
                FLP_HeldItem.Visible = FLP_Ability.Visible = FLP_CanDynamax.Visible = true;
            }

            Stats.LoadStats(pkm);
            loadingPKM = false;
        }

        private TrainerPoke PreparePKM()
        {
            var pk = pkm.Clone();
            pk.Species = CB_Species.SelectedIndex;
            pk.Form = CB_Forme.SelectedIndex;
            pk.Level = (int)NUD_Level.Value;
            pk.Ability = CB_Ability.SelectedIndex;
            pk.HeldItem = CB_Item.SelectedIndex;
            pk.Shiny = CHK_Shiny.Checked;
            pk.Nature = CB_Nature.SelectedIndex;
            pk.Gender = CB_Gender.SelectedIndex;

            pk.Move1 = CB_Move1.SelectedIndex;
            pk.Move2 = CB_Move2.SelectedIndex;
            pk.Move3 = CB_Move3.SelectedIndex;
            pk.Move4 = CB_Move4.SelectedIndex;

            switch (pk)
            {
                case TrainerPoke7b b:
                    b.CanMegaEvolve = CHK_CanMega.Checked;
                    b.MegaFormChoice = (int) NUD_MegaForm.Value;
                    b.Friendship = (int) NUD_Friendship.Value;
                    break;
                case TrainerPoke8 c:
                    c.CanDynamax = CHK_CanDynamax.Checked;
                    c.DynamaxLevel = (byte) Stats.CB_DynamaxLevel.SelectedIndex;
                    c.CanGigantamax = Stats.CHK_Gigantamax.Checked;
                    break;
            }

            return pk;
        }

        private void PopulateFieldsTrainer(TrainerData tr)
        {
            // some trainers have trclasses without corresponding trnames in the text, so add them
            if (Game.Info.SWSH)
            {
                var classes = CB_Trainer_Class.Items;
                for (int i = classes.Count; i <= 253; i++)
                    classes.Add($"{trClass[1]} - {i} *");
            }

            // Load Trainer Data
            CB_Trainer_Class.SelectedIndex = tr.Class;
            CB_Item_1.SelectedIndex = tr.Item1;
            CB_Item_2.SelectedIndex = tr.Item2;
            CB_Item_3.SelectedIndex = tr.Item3;
            CB_Item_4.SelectedIndex = tr.Item4;
            CB_Money.SelectedIndex = tr.Money;
            CB_Mode.SelectedIndex = (int)tr.Mode;
            LoadAIBits(tr.AI);
            if (tr is TrainerData7b b)
            {
                CB_Gift.SelectedIndex = b.Gift;
                NUD_GiftCount.Value = b.GiftQuantity;
            }
        }

        private void PrepareTrainer(TrainerData tr)
        {
            tr.Class = CB_Trainer_Class.SelectedIndex;
            tr.Item1 = CB_Item_1.SelectedIndex;
            tr.Item2 = CB_Item_2.SelectedIndex;
            tr.Item3 = CB_Item_3.SelectedIndex;
            tr.Item4 = CB_Item_4.SelectedIndex;
            tr.Money = CB_Money.SelectedIndex;
            tr.Mode = (BattleMode)CB_Mode.SelectedIndex;
            tr.AI = SaveAIBits(tr.AI);
            if (tr is TrainerData7b b)
            {
                b.Gift = CB_Gift.SelectedIndex;
                b.GiftQuantity = (int) NUD_GiftCount.Value;
            }
        }

        private void LoadAIBits(uint val)
        {
            for (int i = 0; i < AIBits.Length; i++)
                AIBits[i].Checked = ((val >> i) & 1) == 1;
        }

        private uint SaveAIBits(uint oldval)
        {
            uint val = oldval;
            for (int i = 0; i < AIBits.Length; i++)
            {
                if (AIBits[i].Checked)
                    val |= 1u << i;
                else
                    val &= ~(1u << i);
            }
            return val;
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            SaveEntry();
            base.OnFormClosing(e);
        }

        private void DumpTxt(object sender, EventArgs e)
        {
            using var sfd = new SaveFileDialog {FileName = "Trainers.txt"};
            if (sfd.ShowDialog() != DialogResult.OK)
                return;
            var sb = new StringBuilder();
            for (int i = 0; i < Trainers.Length; i++)
            {
                var tr = Trainers[i];
                tr.Name = trName[i];
                sb.Append(GetTrainerString(tr));
            }
            File.WriteAllText(sfd.FileName, sb.ToString());
        }

        private string GetTrainerString(VsTrainer Trainer)
        {
            var file = Trainer.ID;
            var tr = Trainer.Self;
            var name = Trainer.Name;
            var team = Trainer.Team;
            var sb = new StringBuilder();
            if (tr.Class > trClass.Length) // Klara and Avery out of bounds trclass edge case
                tr.Class = 1;

            sb.AppendLine("======");
            sb.Append(file).Append(" - ").Append(trClass[tr.Class]).Append(" ").AppendLine(name);
            sb.AppendLine("======");
            sb.Append("Pokémon: ").Append(tr.NumPokemon).AppendLine();
            for (int i = 0; i < tr.NumPokemon; i++)
            {
                var pk = team[i];
                if (pk.Shiny)
                    sb.Append("Shiny ");
                sb.Append(specieslist[pk.Species]);
                if (pk.Form > 0)
                    sb.Append('-').Append(pk.Form);
                sb.Append(" (Lv. ").Append(pk.Level).Append(") ");
                if (pk.HeldItem > 0)
                    sb.Append("@ ").Append(itemlist[pk.HeldItem]);

                if (pk.Nature != 0)
                    sb.Append(" (Nature: ").Append(natures[pk.Nature]).Append(")");

                sb.Append(" (Moves: ").Append(string.Join("/", pk.Moves.Select(m => m == 0 ? "(None)" : movelist[m]))).Append(")");

                var ivs = pk.IVs;
                sb.Append(" IVs: ").Append(string.Join("/", ivs));
                var evs = pk.EVs;
                if (evs.Any(z => z != 0))
                    sb.Append(" EVs: ").Append(string.Join("/", pk.EVs));

                if (pk is IAwakened a)
                {
                    var avs = a.AVs();
                    if (avs.Any(z => z != 0))
                        sb.Append(" AVs: ").Append(string.Join("/", avs));
                }
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
            var movedata = Data.MoveData.LoadAll();
            var moves = learn.GetHighPoweredMoves(movedata, pkm.Species, pkm.Form, 4);
            SetMoves(moves);
        }

        private void B_CurrentAttack_Click(object sender, EventArgs e)
        {
            pkm.Species = CB_Species.SelectedIndex;
            pkm.Level = (int)NUD_Level.Value;
            pkm.Form = CB_Forme.SelectedIndex;
            var moves = learn.GetCurrentMoves(pkm.Species, pkm.Form, pkm.Level, 4);
            SetMoves(moves);
        }

        private void B_Clear_Click(object sender, EventArgs e) => SetMoves(new int[4]);

        private void SetMoves(IList<int> moves)
        {
            var mcb = new[] { CB_Move1, CB_Move2, CB_Move3, CB_Move4 };
            for (int i = 0; i < mcb.Length; i++)
                mcb[i].SelectedIndex = moves[i];
        }

        private void B_Save_Click(object sender, EventArgs e)
        {
            SaveEntry();
            Modified = true;
            Close();
        }

        private void B_Randomize_Click(object sender, EventArgs e)
        {
            SaveEntry();
            var trand = GetRandomizer();
            trand.Execute();
            LoadEntry();
            System.Media.SystemSounds.Asterisk.Play();
        }

        private TrainerRandomizer GetRandomizer()
        {
            var moves = Data.MoveData.LoadAll();
            var rmove = new MoveRandomizer(Game.Info, moves, Personal);
            int[] banned = Legal.GetBannedMoves(Game.Info.Game, moves.Length);
            rmove.Initialize((MovesetRandSettings)PG_Moves.SelectedObject, banned);
            int[] ban = Array.Empty<int>();

            if (Game.Info.SWSH)
            {
                var pt = Data.PersonalData;
                ban = pt.Table.Take(Game.Info.MaxSpeciesID + 1)
                    .Select((z, i) => new {Species = i, Present = ((PersonalInfoSWSH)z).IsPresentInGame})
                    .Where(z => !z.Present).Select(z => z.Species).ToArray();
            }

            var rspec = new SpeciesRandomizer(Game.Info, Personal);
            var rform = new FormRandomizer(Personal);
            rspec.Initialize((SpeciesSettings)PG_Species.SelectedObject, ban);
            learn.Moves = moves;
            var evos = Data.EvolutionData;
            var trand = new TrainerRandomizer(Game.Info, Personal, Trainers.LoadAll(), evos.LoadAll())
            {
                ClassCount = CB_Trainer_Class.Items.Count,
                Learn = learn,
                RandMove = rmove,
                RandSpec = rspec,
                RandForm = rform,
                GetBlank = () => Game.Info.SWSH ? new TrainerPoke8() : new TrainerPoke7b(), // this should probably be less specific
            };
            trand.Initialize((TrainerRandSettings)PG_RTrainer.SelectedObject, (SpeciesSettings)PG_Species.SelectedObject);
            return trand;
        }

        private void B_Boost_Click(object sender, EventArgs e)
        {
            SaveEntry();
            var trand = GetRandomizer();
            var settings = (TrainerRandSettings)PG_RTrainer.SelectedObject;
            trand.ModifyAllPokemon(pk => TrainerRandomizer.BoostLevel(pk, settings.LevelBoostRatio));
            LoadEntry();
            System.Media.SystemSounds.Asterisk.Play();
        }

        private void B_MaxAI_Click(object sender, EventArgs e)
        {
            SaveEntry();
            var trand = GetRandomizer();
            trand.ModifyAllTrainers(TrainerRandomizer.MaximizeAIFlags);
            LoadEntry();
            System.Media.SystemSounds.Asterisk.Play();
        }
    }

    public static class FormUtil
    {
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
