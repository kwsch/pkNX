using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using pkNX.Game;
using PKHeX.Drawing.PokeSprite;
using pkNX.Structures;
using pkNX.Structures.FlatBuffers.Arceus;
using Clipboard = System.Windows.Forms.Clipboard;
using EvolutionMethod = pkNX.Structures.EvolutionMethod;
using EvolutionSet = pkNX.Structures.FlatBuffers.Arceus.EvolutionSet;
using EvolutionType = pkNX.Structures.EvolutionType;
using Legal = pkNX.Structures.Legal;
using Util = pkNX.Randomization.Util;

namespace pkNX.WinForms;

public partial class PokeDataUI8a : Form
{
    private static EvolutionRow8a[] EvoRows = [];

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
    private readonly string[] speciesNames;
    private readonly string[] classifications;
    private readonly string[] abilities;
    private readonly string[] types;

    private readonly string[] entryNames;
    private readonly int[] baseForms, formVal;

    public IPersonalInfoPLA cPersonal;
    public LearnsetMeta cLearnset;
    public EvolutionSet cEvos;

    public PokeDataUI8a(PokeEditor8a editor, GameManager rom, GameData8a data)
    {
        ROM = rom;
        Editor = editor;
        Data = data;
        InitializeComponent();

        B_NextPokemon.Focus();

        helditem_boxes = [CB_HeldItem1, CB_HeldItem2, CB_HeldItem3];
        ability_boxes = [CB_Ability1, CB_Ability2, CB_Ability3];
        typing_boxes = [CB_Type1, CB_Type2];
        eggGroup_boxes = [CB_EggGroup1, CB_EggGroup2];

        items = ROM.GetStrings(TextName.ItemNames);
        movelist = ROM.GetStrings(TextName.MoveNames);
        speciesNames = ROM.GetStrings(TextName.SpeciesNames);
        classifications = ROM.GetStrings(TextName.SpeciesClassifications);
        abilities = ROM.GetStrings(TextName.AbilityNames);
        types = ROM.GetStrings(TextName.TypeNames);
        movelist = EditorUtil.SanitizeMoveList(movelist);

        speciesNames[0] = "---";
        abilities[0] = items[0] = movelist[0] = "---";

        var pt = Data.PersonalData;
        cPersonal = (IPersonalInfoPLA)pt[0];
        cLearnset = Editor.Learn[0];
        cEvos = Editor.Evolve[0];

        var altForms = pt.GetFormList(speciesNames);
        entryNames = pt.GetPersonalEntryList(altForms, speciesNames, out baseForms, out formVal);

        InitPersonal();
        InitLearn();
        InitEvo();

        CB_HatchSpecies.Items.AddRange([.. speciesNames]);

        CB_Species.SelectedIndex = 1;
        Loaded = true;

        PG_Personal.SelectedObject = EditUtil.Settings.Personal;
        PG_Evolution.SelectedObject = EditUtil.Settings.Species;
        PG_Learn.SelectedObject = EditUtil.Settings.Learn;
        PG_Move.SelectedObject = EditUtil.Settings.Move;
    }

    private void InitPersonal()
    {
        /*for (int i = 0; i < TMs.Count / 2; i++)
            CLB_TM.Items.Add($"TM{i:00} {movelist[TMs[i]]}");
        for (int i = TMs.Count / 2; i < TMs.Count; i++)
            CLB_TM.Items.Add($"TR{i - 100:00} {movelist[TMs[i]]}");
        for (int i = 0; i < Legal.TypeTutor8.Length; i++)
            CLB_TypeTutor.Items.Add(movelist[Legal.TypeTutor8[i]]);*/
        foreach (ushort move in Legal.MoveShop8_LA)
            CLB_SpecialTutor.Items.Add(movelist[move]);

        var entries = entryNames.Select((z, i) => $"{z} - {i:0000}");
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

    private void InitLearn()
    {
        DataGridViewColumn dgvLevel = new DataGridViewTextBoxColumn()
        {
            HeaderText = "Level",
            DisplayIndex = 0,
            Width = 60,
            MinimumWidth = 44,
            Resizable = DataGridViewTriState.True,
            ValueType = typeof(ushort),
        };

        DataGridViewColumn dgvLevelMastery = new DataGridViewTextBoxColumn
        {
            HeaderText = "Mastery Level",
            DisplayIndex = 1,
            Width = 130,
            MinimumWidth = 44,
            Resizable = DataGridViewTriState.True,
            ValueType = typeof(ushort),
        };

        DataGridViewComboBoxColumn dgvMove = new()
        {
            HeaderText = "Move",
            DisplayIndex = 2,
            AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
            Resizable = DataGridViewTriState.True,
            FlatStyle = FlatStyle.Flat,
        };
        dgvMove.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
        dgvMove.Items.AddRange([.. movelist]); // add only the Names

        dgv.Columns.Add(dgvLevel);
        dgv.Columns.Add(dgvLevelMastery);
        dgv.Columns.Add(dgvMove);
    }

    private void InitEvo()
    {
        EvolutionRow8a.species = speciesNames;
        EvolutionRow8a.items = items;
        EvolutionRow8a.movelist = movelist;
        EvolutionRow8a.types = types;
    }

    private void UpdateIndex(object sender, EventArgs e)
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
        LoadMisc(Editor.PokeMisc.Root.GetEntry((ushort)spec, (ushort)form));

        LoadDexResearch(Editor.DexResearch.Root.GetEntries(spec));

        var learnsetTable = Editor.Learn.Root.Table;
        LoadLearnset(learnsetTable.First(x => x.Species == spec && x.Form == form));

        var evoTable = Editor.Evolve.Root.Table;
        LoadEvolutions(evoTable.First(x => x.Species == spec && x.Form == form));

        Bitmap rawImg = SpriteUtil.GetSprite((ushort)spec, (byte)form, 0, 0, 0, false, PKHeX.Core.Shiny.Never);
        Bitmap bigImg = ResizeBitmap(rawImg, rawImg.Width * 2, rawImg.Height * 2);
        PB_MonSprite.Image = bigImg;
    }

    private static Bitmap ResizeBitmap(Image sourceBMP, int width, int height)
    {
        var result = new Bitmap(width, height);
        using Graphics g = Graphics.FromImage(result);
        g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
        g.DrawImage(sourceBMP, 0, 0, width, height);
        return result;
    }

    public bool SaveCurrent()
    {
        bool success = false;
        success |= SavePersonal();
        success |= SaveLearnset();
        success |= SaveEvolutions();

        return success;
    }

    private void LoadPersonal(IPersonalInfoPLA pkm)
    {
        cPersonal = pkm;
        UpdateSpeciesButtonStates();

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

        TB_Classification.Text = classifications[pkm.DexIndexNational];

        CB_Type1.SelectedIndex = (int)pkm.Type1;
        CB_Type2.SelectedIndex = (int)pkm.Type2;

        TB_CatchRate.Text = pkm.CatchRate.ToString(TB_CatchRate.Mask);
        TB_Stage.Text = pkm.EvoStage.ToString(TB_Stage.Mask);

        CB_HeldItem1.SelectedIndex = pkm.Item1;
        CB_HeldItem2.SelectedIndex = pkm.Item2;
        CB_HeldItem3.SelectedIndex = pkm.Item3;

        TB_Gender.Text = pkm.Gender.ToString(TB_Gender.Mask);
        UpdateGenderDetailLabel();

        TB_Field_18.Text = pkm.Field_18.ToString(TB_Field_18.Mask);
        TB_Friendship.Text = pkm.BaseFriendship.ToString(TB_Friendship.Mask);

        CB_EXPGroup.SelectedIndex = pkm.EXPGrowth;

        CB_EggGroup1.SelectedIndex = pkm.EggGroup1;
        CB_EggGroup2.SelectedIndex = pkm.EggGroup2;
        CB_HatchSpecies.SelectedIndex = pkm.HatchedSpecies;

        CB_Ability1.SelectedIndex = pkm.Ability1;
        CB_Ability2.SelectedIndex = pkm.Ability2;
        CB_Ability3.SelectedIndex = pkm.AbilityH;

        TB_FormCount.Text = pkm.FormCount.ToString(TB_FormCount.Mask);
        TB_Form.Text = pkm.Form.ToString(TB_Form.Mask);

        TB_RawColor.Text = pkm.Color.ToString(TB_RawColor.Mask);
        CB_Color.SelectedIndex = pkm.Color & 0xF;

        TB_BaseExp.Text = pkm.BaseEXP.ToString(TB_BaseExp.Mask);

        TB_Height.Text = (pkm.Height / 100.0).ToString("#0.0");
        TB_Weight.Text = (pkm.Weight / 10.0).ToString("#0.0");

        TB_NationalDex.Text = pkm.DexIndexNational.ToString(TB_NationalDex.Mask);
        TB_HisuianDex.Text = pkm.DexIndexRegional.ToString(TB_HisuianDex.Mask);

        bool hasRegionalDexIndex = pkm.DexIndexRegional != 0;
        CHK_InArea1.Checked = hasRegionalDexIndex && pkm.DexIndexLocal1 == pkm.DexIndexRegional;
        CHK_InArea2.Checked = hasRegionalDexIndex && pkm.DexIndexLocal2 == pkm.DexIndexRegional;
        CHK_InArea3.Checked = hasRegionalDexIndex && pkm.DexIndexLocal3 == pkm.DexIndexRegional;
        CHK_InArea4.Checked = hasRegionalDexIndex && pkm.DexIndexLocal4 == pkm.DexIndexRegional;
        CHK_InArea5.Checked = hasRegionalDexIndex && pkm.DexIndexLocal5 == pkm.DexIndexRegional;

        TB_LocalFormIndex.Text = pkm.LocalFormIndex.ToString(TB_LocalFormIndex.Mask);
        TB_RegionalFormIndex.Text = pkm.RegionalFormIndex.ToString(TB_RegionalFormIndex.Mask);
        TB_HatchCycles.Text = pkm.HatchCycles.ToString(TB_HatchCycles.Mask);

        //TB_MoveShop1.Text = pkm.MoveShop1.ToString(TB_MoveShop1.Mask);
        //TB_MoveShop2.Text = pkm.MoveShop2.ToString(TB_MoveShop2.Mask);

        CHK_IsPresentInGame.Checked = pkm.IsPresentInGame;
        CHK_IsRegionalForm.Checked = pkm.IsRegionalForm;

        /*for (int i = 0; i < CLB_TM.Items.Count; i++)
            CLB_TM.SetItemChecked(i, pkm.TMHM[i]); // Bitflags for TM
        for (int i = 0; i < CLB_TypeTutor.Items.Count; i++)
            CLB_TypeTutor.SetItemChecked(i, pkm.TypeTutors[i]);*/
        for (int i = 0; i < CLB_SpecialTutor.Items.Count; i++)
            CLB_SpecialTutor.SetItemChecked(i, pkm.SpecialTutors[i]);

        // For some reason editing the combobox value causes these 4 to get selected ???
        CB_EXPGroup.SelectionLength = 0;
        CB_Color.SelectionLength = 0;
        CB_Type1.SelectionLength = 0;
        CB_Type2.SelectionLength = 0;
    }

    private void LoadMisc(PokeMisc pokeMisc8a)
    {
        PG_PokeMisc.SelectedObject = pokeMisc8a;
        TB_MiscSpecies.Text = pokeMisc8a.Species.ToString(TB_MiscSpecies.Mask);
        TB_MiscForm.Text = pokeMisc8a.Form.ToString(TB_MiscForm.Mask);

        TB_MiscScale.Text = pokeMisc8a.ScaleFactor.ToString("#0.0");
        TB_MiscAlphaScale.Text = pokeMisc8a.AlphaScaleFactor.ToString("#0.0");

        pokeMisc8a.DropTable = Editor.FieldDropTables.Table.FirstOrDefault(drops => drops.Hash == pokeMisc8a.DropTableRef);
        pokeMisc8a.AlphaDropTable = Editor.FieldDropTables.Table.FirstOrDefault(drops => drops.Hash == pokeMisc8a.AlphaDropTableRef);
    }

    private void LoadDexResearch(PokedexResearchTask[] pokedexResearchTask)
    {
        PG_DexResearchTasks.SelectedObject = pokedexResearchTask;
    }

    private void UpdateGenderDetailLabel()
    {
        switch (cPersonal.GetFixedGenderType())
        {
            case FixedGenderType.OnlyMale:
                L_GenderDetails.Text = "(100% Male)";
                break;
            case FixedGenderType.OnlyFemale:
                L_GenderDetails.Text = "(100% Female)";
                break;
            case FixedGenderType.Genderless:
                L_GenderDetails.Text = "(Genderless)";
                break;
            case FixedGenderType.DualGender:
                var female = (cPersonal.Gender - 1) / 253.0;
                L_GenderDetails.Text = $"({1.0 - female:P} Male, {female:P} Female)";
                break;
        }
    }

    private void UpdateSpeciesButtonStates()
    {
        B_PreviousPokemon.Enabled = cPersonal.DexIndexNational > 0;
        B_PreviousForm.Enabled = cPersonal.Form > 0;
        B_NextForm.Enabled = (cPersonal.Form + 1) < cPersonal.FormCount;
        B_NextPokemon.Enabled = (cPersonal.DexIndexNational + 1) <= Data.PersonalData.MaxSpeciesID;
    }

    private bool ValidateRegionalDexIndex()
    {
        var pt = Data.PersonalData;

        var form0 = pt.Table.Cast<IPersonalInfoPLA>().Where(x => x.Form == 0);
        var regionalDex = form0.Select(x => x.DexIndexRegional).Where(x => x != 0).ToImmutableHashSet();

        var dexIndex = Convert.ToUInt16(TB_HisuianDex.Text);
        // Allow index 0 only if the pokemon is not in the game
        if ((dexIndex == 0 && cPersonal.IsPresentInGame) || dexIndex == cPersonal.DexIndexRegional)
            return true;

        if (!regionalDex.Contains(dexIndex))
            return false;
        var prompt = WinFormsUtil.Prompt(MessageBoxButton.YesNo, $"Regional Dex Index '{dexIndex}' is already in use.", "Would you like the index to be automatically updated to a valid one?");
        if (prompt != MessageBoxResult.Yes)
            return false;

        TB_HisuianDex.Text = (regionalDex.Max() + 1).ToString();
        return true;
    }

    private bool SavePersonal()
    {
        bool allValid = false;
        allValid |= ValidateRegionalDexIndex();

        if (!allValid)
            return false;

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

        pkm.FormCount = Convert.ToByte(TB_FormCount.Text);
        pkm.Form = Convert.ToUInt16(TB_Form.Text);
        pkm.Color = (byte)(CB_Color.SelectedIndex) | (Util.ToInt32(TB_RawColor.Text) & 0xF0);

        pkm.BaseEXP = Convert.ToUInt16(TB_BaseExp.Text);

        if (double.TryParse(TB_Height.Text, out double h))
            pkm.Height = (int)(h * 100.0);

        if (double.TryParse(TB_Weight.Text, out double w))
            pkm.Weight = (int)(w * 10.0);

        pkm.DexIndexNational = Convert.ToUInt16(TB_NationalDex.Text);
        pkm.DexIndexRegional = Convert.ToUInt16(TB_HisuianDex.Text);
        pkm.DexIndexLocal1 = CHK_InArea1.Checked ? pkm.DexIndexRegional : (ushort)0;
        pkm.DexIndexLocal2 = CHK_InArea2.Checked ? pkm.DexIndexRegional : (ushort)0;
        pkm.DexIndexLocal3 = CHK_InArea3.Checked ? pkm.DexIndexRegional : (ushort)0;
        pkm.DexIndexLocal4 = CHK_InArea4.Checked ? pkm.DexIndexRegional : (ushort)0;
        pkm.DexIndexLocal5 = CHK_InArea5.Checked ? pkm.DexIndexRegional : (ushort)0;

        TB_LocalFormIndex.Text = pkm.LocalFormIndex.ToString(TB_LocalFormIndex.Mask);
        TB_RegionalFormIndex.Text = pkm.RegionalFormIndex.ToString(TB_RegionalFormIndex.Mask);
        TB_HatchCycles.Text = pkm.HatchCycles.ToString(TB_HatchCycles.Mask);

        //pkm.MoveShop1 = Convert.ToUInt16(TB_MoveShop1.Text);
        //pkm.MoveShop2 = Convert.ToUInt16(TB_MoveShop2.Text);

        pkm.IsPresentInGame = CHK_IsPresentInGame.Checked;
        pkm.IsRegionalForm = CHK_IsRegionalForm.Checked;

        /*for (int i = 0; i < CLB_TM.Items.Count; i++)
            pkm.TMHM[i] = CLB_TM.GetItemChecked(i);
        for (int i = 0; i < CLB_TypeTutor.Items.Count; i++)
            pkm.TypeTutors[i] = CLB_TypeTutor.GetItemChecked(i);*/
        for (int i = 0; i < CLB_SpecialTutor.Items.Count; i++)
            pkm.SpecialTutors[i] = CLB_SpecialTutor.GetItemChecked(i);

        return true;
    }

    private void LoadLearnset(LearnsetMeta pkm)
    {
        dgv.SuspendLayout();
        cLearnset = pkm;
        dgv.Rows.Clear();
        if (pkm.Arceus.Count == 0)
        {
            dgv.CancelEdit();
            return;
        }
        dgv.Rows.Add(pkm.Arceus.Count);

        // Fill Entries
        for (int i = 0; i < pkm.Arceus.Count; i++)
        {
            LearnsetEntry entry = pkm.Arceus[i];
            dgv.Rows[i].Cells[0].Value = entry.Level;
            dgv.Rows[i].Cells[1].Value = entry.LevelMaster;
            dgv.Rows[i].Cells[2].Value = movelist[entry.Move];
        }

        dgv.CancelEdit();
        dgv.ResumeLayout();
    }

    private bool SaveLearnset()
    {
        var pkm = cLearnset;
        int rowCount = dgv.Rows.Count - 1;

        var entries = new List<LearnsetEntry>();
        for (int i = 0; i < rowCount; i++)
        {
            var cells = dgv.Rows[i].Cells;

            int move = Array.IndexOf(movelist, cells[2].Value);
            if (move < 0)
                continue; // Remove all entries that have invalid moves

            _ = ushort.TryParse(cells[0].Value?.ToString(), out var lvl);
            _ = ushort.TryParse(cells[1].Value?.ToString(), out var lvlMastry);

            entries.Add(new LearnsetEntry
            {
                Move = (ushort)move,
                Level = Math.Clamp(lvl, (ushort)0, (ushort)100),
                LevelMaster = Math.Clamp(lvlMastry, (ushort)0, (ushort)100),
            });
        }

        pkm.Arceus = [.. entries];

        return true;
    }

    private void LoadEvolutions(EvolutionSet s)
    {
        cEvos = s;

        var numPossibleEvos = cEvos.Table.Count;

        flowLayoutPanel1.SuspendLayout();
        flowLayoutPanel1.Controls.Clear();
        EvoRows = new EvolutionRow8a[numPossibleEvos];

        for (int i = 0; i < numPossibleEvos; i++)
        {
            var row = new EvolutionRow8a();
            flowLayoutPanel1.Controls.Add(row);
            flowLayoutPanel1.SetFlowBreak(row, true);
            EvoRows[i] = row;

            row.LoadEvolution(cEvos.Table[i]);
        }
        flowLayoutPanel1.ResumeLayout();
    }

    private bool SaveEvolutions()
    {
        var s = cEvos;
        Debug.Assert(EvoRows.Length == s.Table.Count);
        foreach (var row in EvoRows)
            row.SaveEvolution();

        return true;
    }

    private void AutoFillPersonal()
    {
        Debug.Assert(Data.PersonalData is PersonalTable8LA, "This function is build for PLA data. It needs to be updated if more data is added.");

        var la = (PersonalTable8LA)Data.PersonalData;
        la.FixMissingData();
    }

    private void AutoFillEvolutions()
    {
        var usum = ResourcesUtil.USUM_Evolutions;
        var swsh = ResourcesUtil.SWSH_Evolutions;
        var swshPersonal = ResourcesUtil.SWSH;

        for (int i = 0; i < usum.Count; i++)
        {
            EvolutionMethod[] evoSet = swsh[i];
            EvolutionMethod[] usumEvos = usum[i];

            if (evoSet[0].Method != EvolutionType.None || usumEvos[0].Method == EvolutionType.None)
                continue;

            for (int j = 0; j < usumEvos.Length; j++)
            {
                if (usumEvos[j].Method == EvolutionType.None)
                    continue;

                var usumEntry = usumEvos[j];
                var evoEntry = evoSet[j];
                evoEntry.Species = usumEntry.Species;
                evoEntry.Form = usumEntry.Form;
                evoEntry.Argument = usumEntry.Argument;
                evoEntry.Method = usumEntry.Method;
                evoEntry.Level = usumEntry.Level;
            }
        }

        var la = Data.EvolutionData;
        for (int i = 0; i < la.Length; i++)
        {
            var evoSet = la[i];
            if (evoSet.Table != null && evoSet.Table.Count != 0)
                continue;

            var species = evoSet.Species;
            var form = evoSet.Form;

            if (species > Legal.MaxSpeciesID_8)
            {
                continue;
            }

            int index = swshPersonal.GetFormIndex(species, (byte)form);
            if (index == 0)
            {
                // Assume the form doesn't exsist in the game
                continue;
            }

            var swshEvos = swsh[index];
            var entries = new List<Structures.FlatBuffers.Arceus.EvolutionEntry>();
            foreach (var evo in swshEvos)
            {
                if (evo.Method == EvolutionType.None)
                    continue;

                entries.Add(new()
                {
                    Species = evo.Species,
                    Form = evo.Form,
                    Argument = evo.Argument,
                    Method = (ushort)evo.Method,
                    Level = evo.Level,
                });
            }
            evoSet.Table = [.. entries];
        }
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

    private void B_AufoFill_Click(object sender, EventArgs e)
    {
        // Make sure any modifications are saved before forcing to reload everything
        SaveCurrent();

        AutoFillPersonal();
        AutoFillEvolutions();

        // Reload selected
        LoadIndex(CB_Species.SelectedIndex);
        Modified = true;
    }

    private void B_NextPokemon_Click(object sender, EventArgs e)
    {
        Debug.Assert(cPersonal.DexIndexNational < Data.PersonalData.MaxSpeciesID);
        CB_Species.SelectedIndex = cPersonal.DexIndexNational + 1;
    }

    private void B_NextForm_Click(object sender, EventArgs e)
    {
        Debug.Assert(cPersonal.Form < cPersonal.FormCount);

        var pt = Data.PersonalData;
        CB_Species.SelectedIndex = pt.GetFormIndex(cPersonal.DexIndexNational, (byte)(cPersonal.Form + 1));
    }

    private void B_PreviousForm_Click(object sender, EventArgs e)
    {
        Debug.Assert(cPersonal.Form > 0);

        var pt = Data.PersonalData;
        CB_Species.SelectedIndex = pt.GetFormIndex(cPersonal.DexIndexNational, (byte)(cPersonal.Form - 1));
    }

    private void B_PreviousPokemon_Click(object sender, EventArgs e)
    {
        Debug.Assert(cPersonal.DexIndexNational > 0);
        CB_Species.SelectedIndex = cPersonal.DexIndexNational - 1;
    }

    private void B_AddTask_Click(object sender, EventArgs e)
    {
        int species = cPersonal.DexIndexNational;
        Data.DexResearch.Root.AddTask(species);

        LoadDexResearch(Editor.DexResearch.Root.GetEntries(species));
    }

    private void B_CloneTask_Click(object sender, EventArgs e)
    {
        Debug.Assert(PG_DexResearchTasks.SelectedGridItem?.Value is PokedexResearchTask);
        var task = (PokedexResearchTask)PG_DexResearchTasks.SelectedGridItem.Value;
        Data.DexResearch.Root.AddTask(task);

        LoadDexResearch(Editor.DexResearch.Root.GetEntries(task.Species));
    }

    private void B_DeleteTask_Click(object sender, EventArgs e)
    {
        Debug.Assert(PG_DexResearchTasks.SelectedGridItem?.Value is PokedexResearchTask);
        var task = (PokedexResearchTask)PG_DexResearchTasks.SelectedGridItem.Value;
        Data.DexResearch.Root.RemoveTask(task);

        LoadDexResearch(Editor.DexResearch.Root.GetEntries(task.Species));
    }

    private void PG_DexResearchTasks_SelectedGridItemChanged(object sender, SelectedGridItemChangedEventArgs e)
    {
        if (e.NewSelection == null)
            return;
        var obj = e.NewSelection.Value;
        bool enable = obj is PokedexResearchTask;
        B_CloneTask.Enabled = enable;
        B_DeleteTask.Enabled = enable;
    }

    private void PokeDataUI8a_FormClosing(object sender, FormClosingEventArgs e)
    {
        e.Cancel = !SaveCurrent();
        Modified = true;
    }

    private void CHK_IsPresentInGame_CheckedChanged(object sender, EventArgs e)
    {
        cPersonal.IsPresentInGame = CHK_IsPresentInGame.Checked;
        if (!cPersonal.IsPresentInGame)
            return;

        ValidateRegionalDexIndex();

        var rMisc = Editor.PokeMisc.Root;
        if (!rMisc.HasEntry(cPersonal.DexIndexNational, cPersonal.Form))
        {
            var entry = rMisc.AddEntry(cPersonal.DexIndexNational, cPersonal.Form);

            // Reload entry with correct misc data
            LoadMisc(entry);
        }

        var rBehave = Editor.SymbolBehave.Root;
        if (!rBehave.HasEntry(cPersonal.DexIndexNational, cPersonal.Form, false))
        {
            rBehave.AddEntry(cPersonal.DexIndexNational, cPersonal.Form, false);
            rBehave.AddEntry(cPersonal.DexIndexNational, cPersonal.Form, true);
        }

        var rResL = Editor.PokeResourceList.Root;
        if (!rResL.HasEntry(cPersonal.DexIndexNational))
            rResL.AddEntry(cPersonal.DexIndexNational, cPersonal.FormCount);

        var rResT = Editor.PokeResourceTable.Root;
        if (!rResT.HasEntry(cPersonal.DexIndexNational, cPersonal.Form, 0))
            rResT.AddEntry(cPersonal.DexIndexNational, cPersonal.Form, 0);

        var rEnc = Editor.EncounterRateTable.Root;
        if (!rEnc.HasEntry(cPersonal.DexIndexNational, cPersonal.Form))
            rEnc.AddEntry(cPersonal.DexIndexNational, cPersonal.Form);

        var rCap = Editor.CaptureCollisionTable.Root;
        if (!rCap.HasEntry(cPersonal.DexIndexNational, cPersonal.Form))
            rCap.AddEntry(cPersonal.DexIndexNational, cPersonal.Form);
    }

    private void TB_Gender_TextChanged(object sender, EventArgs e)
    {
        cPersonal.Gender = Util.ToInt32(TB_Gender.Text);
        UpdateGenderDetailLabel();
    }

    private void B_Save_Click(object sender, EventArgs e)
    {
        SaveCurrent();
        Modified = true;
        System.Media.SystemSounds.Asterisk.Play();
    }
}
