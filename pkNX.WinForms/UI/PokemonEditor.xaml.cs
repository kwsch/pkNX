using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
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

public record CheckedMove(int Index, string Name, bool IsChecked);

/// <summary>
/// Interaction logic for PokemonEditor.xaml
/// </summary>
public partial class PokemonEditor
{
    public static readonly DependencyProperty SelectedFormProperty = DependencyProperty.Register(nameof(SelectedForm), typeof(byte), typeof(PokemonEditor), new PropertyMetadata((byte)0, OnSelectedSpeciesChanged));

    public static readonly DependencyProperty SelectedSpeciesProperty = DependencyProperty.Register(nameof(SelectedSpecies), typeof(ushort), typeof(PokemonEditor), new PropertyMetadata((ushort)0, OnSelectedSpeciesChanged));

    private static void OnSelectedSpeciesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var self = (PokemonEditor)d;
        if (self.Loaded)
            self.SaveCurrent();
        self.LoadSelectedSpecies();
    }

    private new readonly bool Loaded;
    private readonly GameData8a Data;

    public readonly GameManager ROM;
    public readonly PokeEditor8a Editor;
    public bool Modified { get; set; }

    public IPersonalInfoPLA cPersonal;
    public EvolutionSet cEvos;

    public byte SelectedForm
    {
        get => (byte)GetValue(SelectedFormProperty);
        set => SetValue(SelectedFormProperty, value);
    }

    public ushort SelectedSpecies
    {
        get => (ushort)GetValue(SelectedSpeciesProperty);
        set => SetValue(SelectedSpeciesProperty, value);
    }

    public PokemonEditor(PokeEditor8a editor, GameManager rom, GameData8a data)
    {
        ROM = rom;
        Editor = editor;
        Data = data;
        InitializeComponent();

        B_NextPokemon.Focus();

        var pt = Data.PersonalData;
        cPersonal = (IPersonalInfoPLA)pt[0];
        cEvos = Editor.Evolve[0];

        SelectedSpecies = 1;
        Loaded = true;
    }

    private void LoadSelectedSpecies()
    {
        var pt = Data.PersonalData;
        var index = pt.GetFormIndex(SelectedSpecies, SelectedForm);

        //TB_FormName.Text = UIStaticSources.FormsList[index];

        LoadPersonal((IPersonalInfoPLA)Data.PersonalData[index]);
        LoadMisc(Editor.PokeMisc.Root.GetEntry(SelectedSpecies, SelectedForm));

        LoadDexResearch(Editor.DexResearch.Root.GetEntries(SelectedSpecies));

        var pkm = Editor.Learn.Root.Table.FirstOrDefault(x => x is not null && x.Species == SelectedSpecies && x.Form == SelectedForm, default);
        DG_LevelUp.ItemsSource = pkm?.Arceus.ToList();

        IC_EvolutionsItems.ItemsSource = Editor.Evolve.Root.Table.FirstOrDefault(x => x is not null && x.Species == SelectedSpecies && x.Form == SelectedForm, default)?.Table;

        Bitmap rawImg = SpriteUtil.GetSprite(SelectedSpecies, SelectedForm, 0, 0, 0, false, PKHeX.Core.Shiny.Never);
        Bitmap bigImg = Utils.ResizeBitmap(rawImg, rawImg.Width * 2, rawImg.Height * 2);

        IMG_MonSprite.Source = Utils.ImageSourceFromBitmap(bigImg);
    }

    public bool SaveCurrent()
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
        //pkm.EvoStage = Util.ToInt32(TB_Stage.Text);

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
        //pkm.Form = Convert.ToUInt16(TB_Form.Text);
        //pkm.Color = (byte)CB_Color.SelectedIndex | (Util.ToInt32(TB_RawColor.Text) & 0xF0);

        pkm.BaseEXP = Convert.ToUInt16(TB_BaseExp.Text);

        if (double.TryParse(TB_Height.Text, out double h))
            pkm.Height = (int)(h * 100.0);

        if (double.TryParse(TB_Weight.Text, out double w))
            pkm.Weight = (int)(w * 10.0);

        //pkm.DexIndexNational = Convert.ToUInt16(TB_NationalDex.Text);
        pkm.DexIndexRegional = Convert.ToUInt16(TB_HisuianDex.Text);
        pkm.DexIndexLocal1 = CHK_InArea1.IsChecked is true ? pkm.DexIndexRegional : (ushort)0;
        pkm.DexIndexLocal2 = CHK_InArea2.IsChecked is true ? pkm.DexIndexRegional : (ushort)0;
        pkm.DexIndexLocal3 = CHK_InArea3.IsChecked is true ? pkm.DexIndexRegional : (ushort)0;
        pkm.DexIndexLocal4 = CHK_InArea4.IsChecked is true ? pkm.DexIndexRegional : (ushort)0;
        pkm.DexIndexLocal5 = CHK_InArea5.IsChecked is true ? pkm.DexIndexRegional : (ushort)0;

        TB_LocalFormIndex.Text = pkm.LocalFormIndex.ToString("##0");
        TB_RegionalFormIndex.Text = pkm.RegionalFormIndex.ToString("##0");
        TB_HatchCycles.Text = pkm.HatchCycles.ToString("##0");

        pkm.IsPresentInGame = CHK_IsPresentInGame.IsChecked is true;
        pkm.IsRegionalForm = CHK_IsRegionalForm.IsChecked is true;

        foreach (var item in CLB_TM.Items)
        {
            var move = (CheckedMove)item;
            pkm.TMHM[move.Index] = move.IsChecked;
        }

        foreach (var item in CLB_TR.Items)
        {
            var move = (CheckedMove)item;
            pkm.TR[move.Index] = move.IsChecked;
        }

        foreach (var item in CLB_TypeTutor.Items)
        {
            var move = (CheckedMove)item;
            pkm.TypeTutors[move.Index] = move.IsChecked;
        }

        foreach (var item in CLB_SpecialTutor.Items)
        {
            var move = (CheckedMove)item;
            pkm.SpecialTutors[move.Index] = move.IsChecked;
        }

        return true;
    }

    private void LoadPersonal(IPersonalInfoPLA pkm)
    {
        cPersonal = pkm;
        UpdateSpeciesButtonStates();

        TB_BaseHP.Value = pkm.HP;
        TB_BaseATK.Value = pkm.ATK;
        TB_BaseDEF.Value = pkm.DEF;
        TB_BaseSPE.Value = pkm.SPE;
        TB_BaseSPA.Value = pkm.SPA;
        TB_BaseSPD.Value = pkm.SPD;
        TB_HPEVs.Value = pkm.EV_HP;
        TB_ATKEVs.Value = pkm.EV_ATK;
        TB_DEFEVs.Value = pkm.EV_DEF;
        TB_SPEEVs.Value = pkm.EV_SPE;
        TB_SPAEVs.Value = pkm.EV_SPA;
        TB_SPDEVs.Value = pkm.EV_SPD;
        TB_BST.Value = pkm.GetBaseStatTotal();

        TB_Classification.Text = UIStaticSources.SpeciesClassificationsList[pkm.DexIndexNational];

        CB_Type1.SelectedIndex = (int)pkm.Type1;
        CB_Type2.SelectedIndex = (int)pkm.Type2;

        TB_CatchRate.Value = pkm.CatchRate;
        //TB_Stage.Text = pkm.EvoStage.ToString("##0");

        CB_HeldItem1.SelectedIndex = pkm.Item1;
        CB_HeldItem2.SelectedIndex = pkm.Item2;
        CB_HeldItem3.SelectedIndex = pkm.Item3;

        TB_Gender.Value = pkm.Gender;
        L_GenderDetails.Content = GenderRatioToString();

        TB_Field_18.Value = pkm.Field_18;
        TB_Friendship.Value = pkm.BaseFriendship;

        CB_EXPGroup.SelectedIndex = pkm.EXPGrowth;

        CB_EggGroup1.SelectedIndex = pkm.EggGroup1;
        CB_EggGroup2.SelectedIndex = pkm.EggGroup2;
        CB_HatchSpecies.SelectedIndex = pkm.HatchedSpecies;

        CB_Ability1.SelectedIndex = pkm.Ability1;
        CB_Ability2.SelectedIndex = pkm.Ability2;
        CB_Ability3.SelectedIndex = pkm.AbilityH;

        TB_FormCount.Value = pkm.FormCount;

        CB_Color.SelectedIndex = pkm.Color & 0xF;

        TB_BaseExp.Value = pkm.BaseEXP;

        TB_Height.Value = pkm.Height / 100.0;
        TB_Weight.Value = pkm.Weight / 10.0;

        TB_HisuianDex.Value = pkm.DexIndexRegional;

        bool hasRegionalDexIndex = pkm.DexIndexRegional != 0;
        CHK_InArea1.IsChecked = hasRegionalDexIndex && pkm.DexIndexLocal1 == pkm.DexIndexRegional;
        CHK_InArea2.IsChecked = hasRegionalDexIndex && pkm.DexIndexLocal2 == pkm.DexIndexRegional;
        CHK_InArea3.IsChecked = hasRegionalDexIndex && pkm.DexIndexLocal3 == pkm.DexIndexRegional;
        CHK_InArea4.IsChecked = hasRegionalDexIndex && pkm.DexIndexLocal4 == pkm.DexIndexRegional;
        CHK_InArea5.IsChecked = hasRegionalDexIndex && pkm.DexIndexLocal5 == pkm.DexIndexRegional;

        TB_LocalFormIndex.Value = pkm.LocalFormIndex;
        TB_RegionalFormIndex.Value = pkm.RegionalFormIndex;
        TB_HatchCycles.Value = pkm.HatchCycles;

        CHK_IsPresentInGame.IsChecked = pkm.IsPresentInGame;
        CHK_IsRegionalForm.IsChecked = pkm.IsRegionalForm;

        CLB_TM.ItemsSource = Legal.TMHM_SWSH.Select((moveID, i) => new CheckedMove(i, $"TM{i:00} {UIStaticSources.MovesList[moveID]}", pkm.TMHM[i]));
        CLB_TR.ItemsSource = Legal.TR_SWSH.Select((moveID, i) => new CheckedMove(i, $"TR{i:00} {UIStaticSources.MovesList[moveID]}", pkm.TR[i]));
        CLB_TypeTutor.ItemsSource = Legal.TypeTutor8.Select((moveID, i) => new CheckedMove(i, UIStaticSources.MovesList[moveID], pkm.TypeTutors[i]));
        CLB_SpecialTutor.ItemsSource = Legal.MoveShop8_LA.Select((moveID, i) => new CheckedMove(i, UIStaticSources.MovesList[moveID], pkm.SpecialTutors[i]));
    }

    private void LoadMisc(PokeMisc pokeMisc8a)
    {
        PG_PokeMisc.SelectedObject = pokeMisc8a;
        TB_MiscSpecies.Text = pokeMisc8a.Species.ToString("##0");
        TB_MiscForm.Text = pokeMisc8a.Form.ToString("##0");

        TB_MiscScale.Text = pokeMisc8a.ScaleFactor.ToString("#0.0");
        TB_MiscAlphaScale.Text = pokeMisc8a.AlphaScaleFactor.ToString("#0.0");

        pokeMisc8a.DropTable = Editor.FieldDropTables.Table.FirstOrDefault(drops => drops.Hash == pokeMisc8a.DropTableRef);
        pokeMisc8a.AlphaDropTable = Editor.FieldDropTables.Table.FirstOrDefault(drops => drops.Hash == pokeMisc8a.AlphaDropTableRef);
    }

    private void LoadDexResearch(PokedexResearchTask[] pokedexResearchTask)
    {
        DG_DexResearchTasks.ItemsSource = pokedexResearchTask;
    }

    private string GenderRatioToString()
    {
        switch (cPersonal.GetFixedGenderType())
        {
            case FixedGenderType.OnlyMale:
                return "♂100%";
            case FixedGenderType.OnlyFemale:
                return "♀100%";
            case FixedGenderType.Genderless:
                return "⚥100%";
            case FixedGenderType.DualGender:
                var femaleRatio = (cPersonal.Gender - 1) / 253.0;
                int m = (int)Math.Round((1.0 - femaleRatio) * 100);
                int f = (int)Math.Round(femaleRatio * 100);
                return $"♂{m}% / ♀{f}%";
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void UpdateSpeciesButtonStates()
    {
        B_PreviousPokemon.IsEnabled = cPersonal.DexIndexNational > 0;
        B_PreviousForm.IsEnabled = cPersonal.Form > 0;
        B_NextForm.IsEnabled = (cPersonal.Form + 1) < cPersonal.FormCount;
        B_NextPokemon.IsEnabled = (cPersonal.DexIndexNational + 1) <= Data.PersonalData.MaxSpeciesID;
    }

    private bool ValidateRegionalDexIndex()
    {
        var pt = Data.PersonalData;

        var form0 = pt.Table.Cast<IPersonalInfoPLA>().Where(x => x.Form == 0);
        var regionalDex = form0.Select(x => x.DexIndexRegional).Where(x => x != 0).ToImmutableHashSet();

        var dexIndex = Convert.ToUInt16(TB_HisuianDex.Text);
        // Allow index 0 only if the Pokémon is not in the game
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
            if (evoSet.Table is not { Count: > 0 })
                continue;

            var species = evoSet.Species;
            var form = evoSet.Form;

            if (species > Legal.MaxSpeciesID_8)
                continue;

            int index = swshPersonal.GetFormIndex(species, (byte)form);
            if (index == 0)
                continue; // Assume the form doesn't exist in the game

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
            evoSet.Table = entries.ToArray();
        }
    }

    private void B_DumpTable_Click(object sender, RoutedEventArgs e)
    {
        var pt = Editor.Personal;
        var altForms = pt.GetFormList(UIStaticSources.SpeciesList);
        string[] entryNames = pt.GetPersonalEntryList(altForms, UIStaticSources.SpeciesList, out _, out _);

        var result = TableUtil.GetNamedTypeTable(pt.Table, entryNames, "Species");
        Clipboard.SetText(result);
        System.Media.SystemSounds.Asterisk.Play();
    }

    private void B_RandPersonal_Click(object sender, RoutedEventArgs e)
    {
        /*SaveCurrent();
        var settings = (PersonalRandSettings)PG_Personal.SelectedObject;
        var rand = new PersonalRandomizer(Editor.Personal, ROM.Info, Editor.Evolve.LoadAll()) { Settings = settings };
        rand.Execute();
        LoadIndex(CB_Species.SelectedIndex);
        System.Media.SystemSounds.Asterisk.Play();*/
    }

    private void B_AmpExperience_Click(object sender, RoutedEventArgs e)
    {
        /*SaveCurrent();
        decimal rate = NUD_AmpEXP.Value;
        foreach (var p in Editor.Personal.Table)
            p.BaseEXP = (ushort)Math.Max(0, Math.Min(byte.MaxValue, p.BaseEXP * rate));
        LoadIndex(CB_Species.SelectedIndex);
        System.Media.SystemSounds.Asterisk.Play();*/
    }

    private void B_RandEvo_Click(object sender, RoutedEventArgs e)
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

    private void B_TradeEvo_Click(object sender, RoutedEventArgs e)
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

    private void B_EvolveEveryLevel_Click(object sender, RoutedEventArgs e)
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

    private void B_RandLearn_Click(object sender, RoutedEventArgs e)
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

    private void B_LearnExpand_Click(object sender, RoutedEventArgs e)
    {
        /*var settings = (LearnSettings)PG_Learn.SelectedObject;
        if (!settings.Expand)
        {
            WinFormsUtil.Error("Expand moves not selected. Please double-check settings.",
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

    private void B_LearnMetronome_Click(object sender, RoutedEventArgs e)
    {
        /*var settings = (LearnSettings)PG_Learn.SelectedObject;
        var moveset = (MovesetRandSettings)PG_Move.SelectedObject;
        var rand = new LearnsetRandomizer(ROM.Info, Editor.Learn.LoadAll(), Editor.Personal);
        rand.Initialize(Data.MoveData.LoadAll(), settings, moveset);
        rand.ExecuteMetronome();
        LoadIndex(CB_Species.SelectedIndex);
        System.Media.SystemSounds.Asterisk.Play();*/
    }

    private void B_AufoFill_Click(object sender, RoutedEventArgs e)
    {
        // Make sure any modifications are saved before forcing to reload everything
        SaveCurrent();

        AutoFillPersonal();
        AutoFillEvolutions();

        // Reload selected
        LoadSelectedSpecies();
        Modified = true;
    }

    private void B_NextPokemon_Click(object sender, RoutedEventArgs e)
    {
        Debug.Assert(cPersonal.DexIndexNational < Data.PersonalData.MaxSpeciesID);
        SelectedForm = 0;
        SelectedSpecies++;
    }

    private void B_NextForm_Click(object sender, RoutedEventArgs e)
    {
        Debug.Assert(cPersonal.Form < cPersonal.FormCount);
        SelectedForm++;
    }

    private void B_PreviousForm_Click(object sender, RoutedEventArgs e)
    {
        Debug.Assert(cPersonal.Form > 0);
        SelectedForm--;
    }

    private void B_PreviousPokemon_Click(object sender, RoutedEventArgs e)
    {
        Debug.Assert(cPersonal.DexIndexNational > 0);
        SelectedForm = 0;
        SelectedSpecies--;
    }

    private void B_AddTask_Click(object sender, RoutedEventArgs e)
    {
        int species = cPersonal.DexIndexNational;
        Data.DexResearch.Root.AddTask(species);

        LoadDexResearch(Editor.DexResearch.Root.GetEntries(species));
    }

    private void B_CloneTask_Click(object sender, RoutedEventArgs e)
    {
        Debug.Assert(DG_DexResearchTasks.SelectedValue is PokedexResearchTask);
        var task = (PokedexResearchTask)DG_DexResearchTasks.SelectedValue;
        Data.DexResearch.Root.AddTask(task);

        LoadDexResearch(Editor.DexResearch.Root.GetEntries(task.Species));
    }

    private void B_DeleteTask_Click(object sender, RoutedEventArgs e)
    {
        Debug.Assert(DG_DexResearchTasks.SelectedValue is PokedexResearchTask);
        var task = (PokedexResearchTask)DG_DexResearchTasks.SelectedValue;
        Data.DexResearch.Root.RemoveTask(task);

        LoadDexResearch(Editor.DexResearch.Root.GetEntries(task.Species));
    }

    private void PG_DexResearchTasks_SelectedGridItemChanged(object sender, object e)
    {
        /*if (e.NewSelection == null)
            return;
        var obj = e.NewSelection.Value;
        bool enable = obj is PokedexResearchTask;
        B_CloneTask.IsEnabled = enable;
        B_DeleteTask.IsEnabled = enable;*/
    }

    private void TB_Gender_TextChanged(object sender, TextChangedEventArgs e)
    {
        cPersonal.Gender = Util.ToInt32(TB_Gender.Text);
        L_GenderDetails.Content = GenderRatioToString();
    }

    private void CHK_IsPresentInGame_Changed(object sender, RoutedEventArgs e)
    {
        if (CHK_IsPresentInGame.IsChecked != null)
            cPersonal.IsPresentInGame = CHK_IsPresentInGame.IsChecked.Value;
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

    private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
        e.Cancel = !SaveCurrent();
        Modified = true;
    }
}
