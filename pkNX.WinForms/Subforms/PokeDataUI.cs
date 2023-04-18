using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using pkNX.Game;
using pkNX.Randomization;
using pkNX.Structures;
using PKHeX.Drawing.PokeSprite;
using Util = pkNX.Randomization.Util;

namespace pkNX.WinForms;

public partial class PokeDataUI : Form
{
    private readonly bool Loaded;
    private readonly GameData Data;

    public PokeDataUI(PokeEditor editor, GameManager rom, GameData data)
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
        abilities = ROM.GetStrings(TextName.AbilityNames);
        types = ROM.GetStrings(TextName.TypeNames);
        movelist = EditorUtil.SanitizeMoveList(movelist);

        species[0] = "---";
        abilities[0] = items[0] = movelist[0] = "";

        var pt = Data.PersonalData;
        cPersonal = pt[0];
        cLearnset = Editor.Learn[0];
        cEvos = Editor.Evolve[0];
        cMega = Editor.Mega != null ? Editor.Mega[0] : Array.Empty<MegaEvolutionSet>();

        var altForms = pt.GetFormList(species);
        entryNames = pt.GetPersonalEntryList(altForms, species, out baseForms, out formVal);

        InitPersonal();
        InitLearn();

        InitEvo(Editor.Evolve[0].PossibleEvolutions.Length);

        Megas = Editor.Mega != null ? InitMega(2) : Array.Empty<MegaEvoEntry>();

        CB_Species.SelectedIndex = 1;
        Loaded = true;

        PG_Personal.SelectedObject = EditUtil.Settings.Personal;
        PG_Evolution.SelectedObject = EditUtil.Settings.Species;
        PG_Learn.SelectedObject = EditUtil.Settings.Learn;
        PG_Move.SelectedObject = EditUtil.Settings.Move;
    }

    public readonly GameManager ROM;
    public readonly PokeEditor Editor;
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

    public IPersonalInfo cPersonal;
    public Learnset cLearnset;
    public EvolutionSet cEvos;
    public MegaEvolutionSet[] cMega;
    private readonly MegaEvoEntry[] Megas;

    public void InitPersonal()
    {
        var TMs = Editor.TMHM;
        if (TMs.Count == 0) // No ExeFS to grab TMs from.
        {
            for (int i = 0; i < 100; i++)
                CLB_TM.Items.Add($"TM{i + 1:00}");
        }
        else // Use TM moves.
        {
            if (GameVersion.SWSH.Contains(ROM.Game))
            {
                var TRs = Editor.TR;
                for (int i = 0; i < TMs.Count; i++)
                    CLB_TM.Items.Add($"TM{i:00} {movelist[TMs[i]]}");
                for (int i = 0; i < TRs.Count; i++)
                    CLB_TM.Items.Add($"TR{i:00} {movelist[TRs[i]]}");

                foreach (var move in Legal.TypeTutor8)
                    CLB_TypeTutor.Items.Add(movelist[move]);
                foreach (var move in Legal.Tutors_SWSH_1)
                    CLB_SpecialTutor.Items.Add(movelist[move]);
            }
            else
            {
                for (int i = 0; i < TMs.Count; i++)
                    CLB_TM.Items.Add($"TM{i + 1:00} {movelist[TMs[i]]}");
            }
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
        dgv.Columns.Add(dgvMove);
    }

    private static EvolutionRow[] EvoRows = Array.Empty<EvolutionRow>();

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

    public MegaEvoEntry[] InitMega(int count)
    {
        var result = new MegaEvoEntry[count];
        MegaEvoEntry.items = items;

        for (int i = 0; i < count; i++)
        {
            var row = new MegaEvoEntry();
            flowLayoutPanel1.Controls.Add(row);
            result[i] = row;
        }

        return result;
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
        LoadPersonal(Editor.Personal[index]);
        LoadLearnset(Editor.Learn[index]);
        LoadEvolutions(Editor.Evolve[index]);
        if (Editor.Mega != null)
            LoadMegas(Editor.Mega[index], spec);
        Bitmap rawImg = (Bitmap)SpriteUtil.GetSprite((ushort)spec, (byte)form, 0, 0, 0, false, PKHeX.Core.Shiny.Never);
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
        SaveMegas();
    }

    public void LoadPersonal(IPersonalInfo pkm)
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

        CB_Type1.SelectedIndex = (int)pkm.Type1;
        CB_Type2.SelectedIndex = (int)pkm.Type2;

        TB_CatchRate.Text = pkm.CatchRate.ToString("000");
        TB_Stage.Text = pkm.EvoStage.ToString("0");

        CB_HeldItem1.SelectedIndex = pkm.Item1;
        CB_HeldItem2.SelectedIndex = pkm.Item2;
        CB_HeldItem3.SelectedIndex = pkm.Item3;

        TB_Gender.Text = pkm.Gender.ToString("000");

        if (pkm is IPersonalEgg_1 eggInfo)
            TB_HatchCycles.Text = eggInfo.HatchCycles.ToString("000");

        TB_Friendship.Text = pkm.BaseFriendship.ToString("000");

        CB_EXPGroup.SelectedIndex = pkm.EXPGrowth;

        CB_EggGroup1.SelectedIndex = pkm.EggGroup1;
        CB_EggGroup2.SelectedIndex = pkm.EggGroup2;

        CB_Ability1.SelectedIndex = pkm.Ability1;
        CB_Ability2.SelectedIndex = pkm.Ability2;
        CB_Ability3.SelectedIndex = pkm.AbilityH;

        TB_FormCount.Text = pkm.FormCount.ToString("000");
        TB_FormSprite.Text = pkm.FormSprite.ToString("000");

        TB_RawColor.Text = pkm.Color.ToString("000");
        CB_Color.SelectedIndex = pkm.Color & 0xF;

        TB_BaseExp.Text = pkm.BaseEXP.ToString("000");
        TB_BST.Text = pkm.GetBaseStatTotal().ToString("000");

        TB_Height.Text = ((decimal)pkm.Height / 100).ToString("00.00");
        TB_Weight.Text = ((decimal)pkm.Weight / 10).ToString("000.0");

        if (pkm is IPersonalInfoSM sm)
        {
            TB_CallRate.Text = sm.EscapeRate.ToString("000");
            CB_ZItem.SelectedIndex = sm.SpecialZ_Item;
            CB_ZBaseMove.SelectedIndex = sm.SpecialZ_BaseMove;
            CB_ZMove.SelectedIndex = sm.SpecialZ_ZMove;
            CHK_Variant.Checked = sm.IsRegionalForm;
            CHK_IsPresentInGame.Visible = CHK_CanNotDynamax.Visible =
                L_RegionalDex.Visible = L_ArmorDex.Visible = L_CrownDex.Visible = TB_RegionalDex.Visible = TB_ArmorDex.Visible = TB_CrownDex.Visible = false;
        }
        if (pkm is IPersonalInfoGG gg)
        {
            MT_GoID.Text = gg.GoSpecies.ToString("000");
            CHK_Variant.Checked = gg.IsRegionalForm;
            GB_ZMove.Visible = CHK_IsPresentInGame.Visible = CHK_CanNotDynamax.Visible = L_TypeTutors.Visible = CLB_TypeTutor.Visible = CLB_SpecialTutor.Visible =
                L_RegionalDex.Visible = L_ArmorDex.Visible = L_CrownDex.Visible = TB_RegionalDex.Visible = TB_ArmorDex.Visible = TB_CrownDex.Visible = false;
        }
        if (pkm is IPersonalInfoSWSH swsh)
        {
            L_TM.Text = "TMs/TRs:";
            MT_GoID.Text = swsh.DexIndexNational.ToString("000");
            TB_RegionalDex.Text = swsh.DexIndexRegional.ToString("000");
            TB_ArmorDex.Text = swsh.ArmorDexIndex.ToString("000");
            TB_CrownDex.Text = swsh.CrownDexIndex.ToString("000");
            CHK_IsPresentInGame.Checked = swsh.IsPresentInGame;
            CHK_Variant.Checked = swsh.IsRegionalForm;
            CHK_CanNotDynamax.Checked = swsh.CanNotDynamax;
            L_CallRate.Visible = TB_CallRate.Visible = GB_ZMove.Visible = false;
        }

        if (pkm is IMovesInfo_1 mi)
        {
            int halfList = CLB_TM.Items.Count / 2;
            int fullList = CLB_TM.Items.Count;
        
            for (int i = 0; i < halfList; i++)
                CLB_TM.SetItemChecked(i, mi.TMHM[i]); // Bitflags for TM
                
            if (pkm is IMovesInfo_SWSH mitr) // if SWSH, the second half is just TRs
            {
                for (int i = 0; i < halfList; i++)
                    CLB_TM.SetItemChecked((i + halfList), mitr.TR[i]); // Bitflags for TR
            }
            else
            {
                for (int i = halfList; i < fullList; i++) // not SWSH, finish the remaining TMs
                    CLB_TM.SetItemChecked(i, mi.TMHM[i]);
            }
            
            for (int i = 0; i < CLB_TypeTutor.Items.Count; i++)
                CLB_TypeTutor.SetItemChecked(i, mi.TypeTutors[i]);
        }

        if (pkm is IMovesInfo_2 mi2)
        {
            for (int i = 0; i < CLB_SpecialTutor.Items.Count; i++)
                CLB_SpecialTutor.SetItemChecked(i, mi2.SpecialTutors[0][i]);
        }
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

        pkm.Type1 = (Types)CB_Type1.SelectedIndex;
        pkm.Type2 = (Types)CB_Type2.SelectedIndex;
        pkm.Item1 = CB_HeldItem1.SelectedIndex;
        pkm.Item2 = CB_HeldItem2.SelectedIndex;
        pkm.Item3 = CB_HeldItem3.SelectedIndex;

        pkm.Gender = Util.ToInt32(TB_Gender.Text);

        if (pkm is IPersonalEgg_1 eggInfo)
            eggInfo.HatchCycles = Util.ToInt32(TB_HatchCycles.Text);

        pkm.BaseFriendship = Util.ToInt32(TB_Friendship.Text);
        pkm.EXPGrowth = (byte)CB_EXPGroup.SelectedIndex;
        pkm.EggGroup1 = CB_EggGroup1.SelectedIndex;
        pkm.EggGroup2 = CB_EggGroup2.SelectedIndex;
        pkm.Ability1 = CB_Ability1.SelectedIndex;
        pkm.Ability2 = CB_Ability2.SelectedIndex;
        pkm.AbilityH = CB_Ability3.SelectedIndex;

        pkm.FormSprite = Convert.ToUInt16(TB_FormSprite.Text);
        pkm.FormCount = Convert.ToByte(TB_FormCount.Text);
        pkm.Color = (byte)(CB_Color.SelectedIndex) | (Util.ToInt32(TB_RawColor.Text) & 0xF0);
        pkm.BaseEXP = Convert.ToUInt16(TB_BaseExp.Text);

        if (decimal.TryParse(TB_Height.Text, out decimal h))
            pkm.Height = (int)(h * 100);

        if (decimal.TryParse(TB_Weight.Text, out decimal w))
            pkm.Weight = (int)(w * 10);

        if (pkm is IPersonalInfoSM sm)
        {
            pkm.EscapeRate = Util.ToInt32(TB_CallRate.Text);
            sm.SpecialZ_Item = CB_ZItem.SelectedIndex;
            sm.SpecialZ_BaseMove = CB_ZBaseMove.SelectedIndex;
            sm.SpecialZ_ZMove = CB_ZMove.SelectedIndex;
            sm.IsRegionalForm = CHK_Variant.Checked;
        }
        if (pkm is IPersonalInfoGG gg)
        {
            gg.GoSpecies = Convert.ToUInt16(MT_GoID.Text);
        }
        if (pkm is IPersonalInfoSWSH swsh)
        {
            swsh.DexIndexRegional = Convert.ToUInt16(TB_RegionalDex.Text);
            swsh.ArmorDexIndex = Convert.ToUInt16(TB_ArmorDex.Text);
            swsh.CrownDexIndex = Convert.ToUInt16(TB_CrownDex.Text);
            swsh.IsPresentInGame = CHK_IsPresentInGame.Checked;
            swsh.IsRegionalForm = CHK_Variant.Checked;
            swsh.CanNotDynamax = CHK_CanNotDynamax.Checked;
        }

        if (pkm is IMovesInfo_1 mi)
        {
            int halfList = CLB_TM.Items.Count / 2;
            int fullList = CLB_TM.Items.Count;
            
            for (int i = 0; i < halfList; i++)
                mi.TMHM[i] = CLB_TM.GetItemChecked(i);
                
            if (pkm is IMovesInfo_SWSH mitr)
            {
                for (int i = 0; i < halfList; i++)
                    mitr.TR[i] = CLB_TM.GetItemChecked(i + halfList);
            }
            else
            {
                for (int i = halfList; i < fullList; i++)
                    mi.TMHM[i] = CLB_TM.GetItemChecked(i);
            }
            
            for (int i = 0; i < CLB_TypeTutor.Items.Count; i++)
                mi.TypeTutors[i] = CLB_TypeTutor.GetItemChecked(i);
        }

        if (pkm is IMovesInfo_2 mi2)
        {
            for (int i = 0; i < CLB_SpecialTutor.Items.Count; i++)
                mi2.SpecialTutors[0][i] = CLB_SpecialTutor.GetItemChecked(i);
        }
    }

    public void LoadLearnset(Learnset pkm)
    {
        cLearnset = pkm;
        dgv.Rows.Clear();
        if (pkm.Count == 0)
        {
            dgv.CancelEdit();
            return;
        }
        dgv.Rows.Add(pkm.Count);

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
        List<int> moves = new();
        List<int> levels = new();
        for (int i = 0; i < dgv.Rows.Count - 1; i++)
        {
            var cells = dgv.Rows[i].Cells;
            int move = Array.IndexOf(movelist, cells[1].Value);
            if (move < 1)
                continue;

            moves.Add((short)move);
            string level = cells[0].Value?.ToString() ?? "0";
            _ = int.TryParse(level, out var lv);
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
        if (Editor.Mega == null)
            return;
        cMega = m;
        Debug.Assert(Megas.Length == m.Length);
        for (int i = 0; i < Megas.Length; i++)
        {
            var entry = Megas[i];
            entry.LoadEvolution(m[i], spec);
        }
    }

    public void SaveMegas()
    {
        if (Editor.Mega == null)
            return;
        var m = cMega;
        Debug.Assert(Megas.Length == m.Length);
        foreach (var row in Megas)
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
        SaveCurrent();
        var settings = (PersonalRandSettings)PG_Personal.SelectedObject;
        var rand = new PersonalRandomizer(Editor.Personal, ROM.Info, Editor.Evolve.LoadAll()) { Settings = settings };
        rand.Execute();
        LoadIndex(CB_Species.SelectedIndex);
        System.Media.SystemSounds.Asterisk.Play();
    }

    private void B_AmpExperience_Click(object sender, EventArgs e)
    {
        SaveCurrent();
        decimal rate = NUD_AmpEXP.Value;
        foreach (var p in Editor.Personal.Table)
            p.BaseEXP = (int)Math.Max(0, Math.Min(byte.MaxValue, p.BaseEXP * rate));
        LoadIndex(CB_Species.SelectedIndex);
        System.Media.SystemSounds.Asterisk.Play();
    }

    private void B_RandEvo_Click(object sender, EventArgs e)
    {
        SaveCurrent();
        var settings = (SpeciesSettings)PG_Evolution.SelectedObject;
        if (ROM.Info.GG)
            settings.Gen2 = settings.Gen3 = settings.Gen4 = settings.Gen5 = settings.Gen6 = settings.Gen7 = settings.Gen8 = false;
        var rand = new EvolutionRandomizer(ROM.Info, Editor.Evolve.LoadAll(), Editor.Personal);
        int[] ban = Array.Empty<int>();

        if (ROM.Info.SWSH)
        {
            var pt = Data.PersonalData;
            ban = pt.Table.Take(ROM.Info.MaxSpeciesID + 1)
                .Select((z, i) => new { Species = i, Present = ((IPersonalInfoSWSH)z).IsPresentInGame })
                .Where(z => !z.Present).Select(z => z.Species).ToArray();
        }

        rand.RandSpec.Initialize(settings, ban);
        rand.Execute();
        LoadIndex(CB_Species.SelectedIndex);
        System.Media.SystemSounds.Asterisk.Play();
    }

    private void B_TradeEvo_Click(object sender, EventArgs e)
    {
        SaveCurrent();
        var settings = (SpeciesSettings)PG_Evolution.SelectedObject;
        if (ROM.Info.GG)
            settings.Gen2 = settings.Gen3 = settings.Gen4 = settings.Gen5 = settings.Gen6 = settings.Gen7 = settings.Gen8 = false;
        var rand = new EvolutionRandomizer(ROM.Info, Editor.Evolve.LoadAll(), Editor.Personal);
        rand.RandSpec.Initialize(settings);
        rand.ExecuteTrade();
        LoadIndex(CB_Species.SelectedIndex);
        System.Media.SystemSounds.Asterisk.Play();
    }

    private void B_EvolveEveryLevel_Click(object sender, EventArgs e)
    {
        SaveCurrent();
        var settings = (SpeciesSettings)PG_Evolution.SelectedObject;
        if (ROM.Info.GG)
            settings.Gen2 = settings.Gen3 = settings.Gen4 = settings.Gen5 = settings.Gen6 = settings.Gen7 = settings.Gen8 = false;
        var rand = new EvolutionRandomizer(ROM.Info, Editor.Evolve.LoadAll(), Editor.Personal);
        int[] ban = Array.Empty<int>();

        if (ROM.Info.SWSH)
        {
            var pt = Data.PersonalData;
            ban = pt.Table.Take(ROM.Info.MaxSpeciesID + 1)
                .Select((z, i) => new { Species = i, Present = ((IPersonalInfoSWSH)z).IsPresentInGame })
                .Where(z => !z.Present).Select(z => z.Species).ToArray();
        }

        rand.RandSpec.Initialize(settings, ban);
        rand.ExecuteEvolveEveryLevel();
        rand.Execute(); // randomize right after
        LoadIndex(CB_Species.SelectedIndex);
        System.Media.SystemSounds.Asterisk.Play();
    }

    private void B_RandLearn_Click(object sender, EventArgs e)
    {
        SaveCurrent();
        var settings = (LearnSettings)PG_Learn.SelectedObject;
        var moveset = (MovesetRandSettings)PG_Move.SelectedObject;
        var rand = new LearnsetRandomizer(ROM.Info, Editor.Learn.LoadAll(), Editor.Personal);
        var moves = Data.MoveData.LoadAll();
        int[] banned = Legal.GetBannedMoves(ROM.Info.Game, moves.Length);
        rand.Initialize(moves, settings, moveset, banned);
        rand.Execute();
        LoadIndex(CB_Species.SelectedIndex);
        System.Media.SystemSounds.Asterisk.Play();
    }

    private void B_LearnExpand_Click(object sender, EventArgs e)
    {
        var settings = (LearnSettings)PG_Learn.SelectedObject;
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
        System.Media.SystemSounds.Asterisk.Play();
    }

    private void B_LearnMetronome_Click(object sender, EventArgs e)
    {
        var settings = (LearnSettings)PG_Learn.SelectedObject;
        var moveset = (MovesetRandSettings)PG_Move.SelectedObject;
        var rand = new LearnsetRandomizer(ROM.Info, Editor.Learn.LoadAll(), Editor.Personal);
        rand.Initialize(Data.MoveData.LoadAll(), settings, moveset);
        rand.ExecuteMetronome();
        LoadIndex(CB_Species.SelectedIndex);
        System.Media.SystemSounds.Asterisk.Play();
    }

    private void B_Save_Click(object sender, EventArgs e)
    {
        SaveCurrent();
        Modified = true;
        Close();
    }
}
