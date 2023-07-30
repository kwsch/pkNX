using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using FlatSharp;
using pkNX.Containers;
using pkNX.Game;
using pkNX.Randomization;
using pkNX.Structures;
using pkNX.Structures.FlatBuffers;
using pkNX.Structures.FlatBuffers.Arceus;
using pkNX.WinForms.Subforms;
using static pkNX.Structures.Species;
using EvolutionSet = pkNX.Structures.FlatBuffers.Arceus.EvolutionSet;
using Learnset = pkNX.Structures.FlatBuffers.Arceus.Learnset;
using TrainerData = pkNX.Structures.FlatBuffers.Arceus.TrainerData;
using Util = pkNX.Randomization.Util;

// ReSharper disable UnusedMember.Global

namespace pkNX.WinForms.Controls;

internal class EditorPLA : EditorBase
{
    protected override GameManagerPLA ROM { get; }
    private GameData8a Data => ROM.Data;
    protected internal EditorPLA(GameManagerPLA rom)
    {
        ROM = rom;
        CheckOodleDllPresence();
    }

    private static void CheckOodleDllPresence()
    {
        const string file = $"{Oodle.OodleLibraryPath}.dll";
        var dir = Application.StartupPath;
        var path = Path.Combine(dir, file);
        if (!File.Exists(path))
            WinFormsUtil.Alert($"{file} not found in the executable folder", "Some decompression functions may cause errors.");
    }

    public void PopFlat<T1, T2>(GameFile file, string title, Func<T1, IList<T2>> sel, Func<T2, int, string> getName, Action<IEnumerable<T2>>? rand = null, Action<T1>? addEntryCallback = null, bool canSave = true)
        where T1 : class, IFlatBufferSerializable<T1>
        where T2 : class
    {
        var obj = ROM.GetFile(file);
        var tableCache = new TableCache<T1, T2>(obj, sel);

        DataCache<T2> loader(GenericEditor<T2> form)
        {
            if (form.Modified)
                tableCache.Save();

            tableCache = new TableCache<T1, T2>(obj, sel);
            return tableCache.Cache;
        }

        Action? cb = null;
        if (addEntryCallback != null)
            cb = () => addEntryCallback.Invoke(tableCache.Root);

        using var form = new GenericEditor<T2>(loader, getName, title, rand, cb, canSave);
        form.ShowDialog();
        if (form.Modified)
        {
            tableCache.Save();
        }
    }

    private static bool PopFlat<T2>(IList<T2> arr, string title, Func<T2, int, string> getName, Action<IEnumerable<T2>>? rand = null, bool canSave = true) where T2 : class
    {
        using var form = new GenericEditor<T2>(_ => new DataCache<T2>(arr!), getName, title, rand, null, canSave);
        form.ShowDialog();
        return form.Modified;
    }

    public void PopFlatConfig(GameFile file, string title)
    {
        PopFlat<ConfigArchive, ConfigEntry>(file, title, z => z.Table, (z, _) => z.Name);
    }

    public int[] GetSpeciesBanlist()
    {
        var pt = Data.PersonalData;
        var hasForm = new HashSet<int>();
        var banned = new HashSet<int>();
        foreach (var pi in pt.Table.Cast<IPersonalMisc_SWSH>())
        {
            if (pi.IsPresentInGame)
            {
                banned.Remove(pi.DexIndexNational);
                hasForm.Add(pi.DexIndexNational);
            }
            else if (!hasForm.Contains(pi.DexIndexNational))
            {
                banned.Add(pi.DexIndexNational);
            }
        }
        return banned.ToArray();
    }

    public int GetRandomForm(int spec)
    {
        var pt = Data.PersonalData;
        var formRand = pt.Table.Cast<IPersonalMisc_SWSH>()
            .Where(z => z.IsPresentInGame && !(Legal.BattleExclusiveForms.Contains(z.DexIndexNational) || Legal.BattleFusions.Contains(z.DexIndexNational)))
            .GroupBy(z => z.DexIndexNational)
            .ToDictionary(z => z.Key, z => z.ToList());

        if (!formRand.TryGetValue((ushort)spec, out var entries))
            return 0;
        var count = entries.Count;

        return (Species)spec switch
        {
            Growlithe or Arcanine or Voltorb or Electrode or Typhlosion or Qwilfish or Samurott or Lilligant or Zorua or Zoroark or Braviary or Sliggoo or Goodra or Avalugg or Decidueye => 1,
            Basculin => 2,
            Kleavor => 0,
            _ => Util.Random.Next(0, count),
        };
    }

    [EditorCallable(EditorCategory.None)]
    public void EditMasterDump()
    {
        using var md = new DumperPLA(ROM);
        md.ShowDialog();
    }

    #region Dialog Editors
    [EditorCallable(EditorCategory.Dialog)]
    public void EditCommon()
    {
        var text = ROM.GetFilteredFolder(GameFile.GameText, z => Path.GetExtension(z) == ".dat");
        var config = new TextConfig(ROM.Game);
        var tc = new TextContainer(text, config);
        using var form = new TextEditor(tc, TextEditor.TextEditorMode.Common);
        form.ShowDialog();
        if (!form.Modified)
            text.CancelEdits();
    }

    [EditorCallable(EditorCategory.Dialog)]
    public void EditScript()
    {
        var text = ROM.GetFilteredFolder(GameFile.StoryText, z => Path.GetExtension(z) == ".dat");
        var config = new TextConfig(ROM.Game);
        var tc = new TextContainer(text, config);
        using var form = new TextEditor(tc, TextEditor.TextEditorMode.Script);
        form.ShowDialog();
        if (!form.Modified)
            text.CancelEdits();
    }
    #endregion

    #region Pokemon Editors
    [EditorCallable(EditorCategory.Pokemon)]
    public void EditPokemon()
    {
        var editor = new PokeEditor8a
        {
            Personal = Data.PersonalData,
            PokeMisc = new(ROM.GetFile(GameFile.PokeMisc), z => z.Table),
            SymbolBehave = new(ROM.GetFile(GameFile.SymbolBehave), z => z.Table),
            Evolve = Data.EvolutionData,
            Learn = Data.LevelUpData,
            FieldDropTables = Data.FieldDrops,
            BattleDropTabels = Data.BattleDrops,
            DexResearch = Data.DexResearch,
            PokeResourceList = new(ROM.GetFile(GameFile.PokemonResourceList), z => z.Table),
            PokeResourceTable = new(ROM.GetFile(GameFile.PokemonResourceTable), z => z.Table),
            EncounterRateTable = new(ROM.GetFile(GameFile.EncounterRateTable), z => z.Table),
            CaptureCollisionTable = new(ROM.GetFile(GameFile.PokeCaptureCollision), z => z.Table),
        };
        using var form = new PokeDataUI8a(editor, ROM, Data);
        form.ShowDialog();
        if (!form.Modified)
            editor.CancelEdits();
        else
            editor.Save();
    }

    [EditorCallable(EditorCategory.Pokemon)]
    public void EditMiscSpeciesInfo()
    {
        var names = ROM.GetStrings(TextName.SpeciesNames);
        PopFlat<PokeMiscTable, PokeMisc>(GameFile.PokeMisc, "Misc Species Info Editor", z => z.Table, (z, _) => $"{names[z.Species]}{(z.Form == 0 ? "" : $"-{z.Form}")} ~ {z.Value}");
    }

    [EditorCallable(EditorCategory.Pokemon)]
    public void EditSymbolBehave()
    {
        var names = ROM.GetStrings(TextName.SpeciesNames);
        PopFlat<PokeAIArchive, PokeAI>(GameFile.SymbolBehave, "Symbol Behavior Editor", z => z.Table, (z, _) => $"{names[z.Species]}{(z.IsAlpha ? "*" : "")}{(z.Form != 0 ? $"-{z.Form}" : "")}");
    }

    [EditorCallable(EditorCategory.Pokemon)]
    public void EditEvolutions()
    {
        var names = ROM.GetStrings(TextName.SpeciesNames);
        PopFlat<EvolutionTable, EvolutionSet>(GameFile.Evolutions, "Evolution Editor", z => z.Table,
            (z, _) => $"{names[z.Species]}{(z.Form != 0 ? $"-{z.Form}" : "")}");
    }

    [EditorCallable(EditorCategory.Pokemon)]
    public void EditEvolutionConfig() => PopFlatConfig(GameFile.EvolutionConfig, "Evolution Config Editor");

    [EditorCallable(EditorCategory.Pokemon)]
    public void EditLearnsetRaw()
    {
        var names = ROM.GetStrings(TextName.SpeciesNames);
        PopFlat<Learnset, LearnsetMeta>(GameFile.Learnsets, "Learnset Editor (Raw)", z => z.Table, (z, _) => $"{names[z.Species]}{(z.Form == 0 ? "" : $"-{z.Form}")}");
    }

    [EditorCallable(EditorCategory.Pokemon)]
    public void EditPersonalRaw()
    {
        var names = ROM.GetStrings(TextName.SpeciesNames);
        PopFlat<PersonalTable, PersonalInfo>(GameFile.PersonalStats, "Personal Info Editor (Raw)", z => z.Table, (z, _) => $"{names[z.DexIndexNational]}{(z.Form == 0 ? "" : $"-{z.Form}")}");
    }

    [EditorCallable(EditorCategory.Pokemon)]
    public void EditGift()
    {
        var names = ROM.GetStrings(TextName.SpeciesNames);
        PopFlat<PokeAddArchive, PokeAdd>(GameFile.EncounterTableGift, "Gift Encounter Editor", z => z.Table, (z, _) => $"{names[z.Species]}{(z.Form == 0 ? "" : $"-{z.Form}")} @ Lv. {z.Level}", entries => Randomize(entries));

        void Randomize(IEnumerable<PokeAdd> arr)
        {
            var settings = EditUtil.Settings.Species;
            var rand = new SpeciesRandomizer(ROM.Info, Data.PersonalData);
            settings.Legends = false;
            rand.Initialize(settings, GetSpeciesBanlist());
            foreach (var t in arr)
            {
                t.Species = rand.GetRandomSpecies(t.Species);
                t.Form = (byte)GetRandomForm(t.Species);
                t.Nature = NatureType.Random;
                t.Gender = (int)FixedGender.Random;
                t.ShinyLock = ShinyType.Random;
                t.Ball = Util.Random.Next(27, 37); // [Strange, Origin]
                t.Move1 = t.Move2 = t.Move3 = t.Move4 = 0;
                t.Height = t.Weight = -1;
            }
        }
    }

    [EditorCallable(EditorCategory.Pokemon)]
    public void EditPokeCaptureCollision()
    {
        var names = ROM.GetStrings(TextName.SpeciesNames);
        PopFlat<PokeCaptureCollisionArchive, PokeCaptureCollision>(GameFile.PokeCaptureCollision, "Pokemon Capture Collision Editor", z => z.Table, (z, _) => $"{names[z.Species]}{(z.Form == 0 ? "" : $"-{z.Form}")}");
    }

    [EditorCallable(EditorCategory.Pokemon)] public void EditEventRestrictionBattle() => PopFlatConfig(GameFile.EventRestrictionBattle, "Event Restriction Battle Editor");
    [EditorCallable(EditorCategory.Pokemon)] public void EditBuddyConfig() => PopFlatConfig(GameFile.BuddyConfig, "Buddy Config Editor");
    [EditorCallable(EditorCategory.Pokemon)] public void EditBuddyDirectItemConfig() => PopFlatConfig(GameFile.BuddyDirectItemConfig, "Buddy Direct Item Config Editor");
    [EditorCallable(EditorCategory.Pokemon)] public void EditBuddyGroupTalkConfig() => PopFlatConfig(GameFile.BuddyGroupTalkConfig, "Buddy Group Talk Config Editor");
    [EditorCallable(EditorCategory.Pokemon)] public void EditBuddyLandmarkConfig() => PopFlatConfig(GameFile.BuddyLandmarkConfig, "Buddy Landmark Config Editor");
    [EditorCallable(EditorCategory.Pokemon)] public void EditBuddyNPCReactionConfig() => PopFlatConfig(GameFile.BuddyNPCReactionConfig, "Buddy NPC Reaction Config Editor");
    [EditorCallable(EditorCategory.Pokemon)] public void EditBuddyPlayerModeConfig() => PopFlatConfig(GameFile.BuddyPlayerModeConfig, "Buddy Player Mode Config Editor");
    [EditorCallable(EditorCategory.Pokemon)] public void EditBuddyWarpConfig() => PopFlatConfig(GameFile.BuddyWarpConfig, "Buddy Warp Config Editor");
    [EditorCallable(EditorCategory.Pokemon)] public void EditCaptureConfig() => PopFlatConfig(GameFile.CaptureConfig, "Capture Config Editor");
    [EditorCallable(EditorCategory.Pokemon)] public void EditPokemonConfig() => PopFlatConfig(GameFile.PokemonConfig, "Pokemon Config");
    [EditorCallable(EditorCategory.Pokemon)] public void EditPokemonControllerConfig() => PopFlatConfig(GameFile.PokemonControllerConfig, "Pokemon Controller Config");
    [EditorCallable(EditorCategory.Pokemon)] public void EditPokemonFriendshipConfig() => PopFlatConfig(GameFile.PokemonFriendshipConfig, "Pokemon Friendship Config");
    [EditorCallable(EditorCategory.Pokemon)] public void EditSizeScaleConfig() => PopFlatConfig(GameFile.SizeScaleConfig, "Size Scale Config");

    #endregion

    #region AI Editors
    [EditorCallable(EditorCategory.AI)] public void EditAICommonConfig() => PopFlatConfig(GameFile.AICommonConfig, "AI Common Config Editor");
    [EditorCallable(EditorCategory.AI)] public void EditAIExcitingConfig() => PopFlatConfig(GameFile.AIExcitingConfig, "AI Exciting Config Editor");
    [EditorCallable(EditorCategory.AI)] public void EditAISemiLegendConfig() => PopFlatConfig(GameFile.AISemiLegendConfig, "AI Semi Legend Config Editor");
    [EditorCallable(EditorCategory.AI)] public void EditAITirednessConfig() => PopFlatConfig(GameFile.AITirednessConfig, "AI Tiredness Config Editor");
    [EditorCallable(EditorCategory.AI)] public void EditNPC_AIConfig() => PopFlatConfig(GameFile.NPCAIConfig, "NPC AI Config");
    [EditorCallable(EditorCategory.AI)] public void EditNPCPokemonAIConfig() => PopFlatConfig(GameFile.NPCPokemonAIConfig, "NPC Pokemon AI Config");

    #endregion

    #region Field Editors
    [EditorCallable(EditorCategory.Field)]
    public void EditAreaWeather()
    {
        var gfp = (GFPack)ROM.GetFile(GameFile.Resident);
        var data = gfp[2065];
        var obj = FlatBufferConverter.DeserializeFrom<AreaWeatherTable>(data);
        var result = PopFlat(obj.Table, "Area Weather Editor", (z, _) => z.Hash.ToString("X16"));
        if (!result)
            return;
        gfp[2065] = obj.SerializeFrom();
    }

    [EditorCallable(EditorCategory.Field)]
    public void EditStaticEncounter()
    {
        var names = ROM.GetStrings(TextName.SpeciesNames);
        PopFlat<EventEncountArchive, EventEncount>(GameFile.EncounterTableStatic, "Static Encounter Editor", z => z.Table, (z, _) => $"{z.EncounterName} ({GetDetail(z, names)})", entries => Randomize(entries));

        static string GetDetail(EventEncount z, string[] names)
        {
            if (z.Table is not { Count: not 0 } x)
                return "No Entries";
            var s = x[0];
            return $"{names[s.Species]}{(s.Form == 0 ? "" : $"-{s.Form}")} @ Lv. {s.Level}";
        }

        void Randomize(IEnumerable<EventEncount> arr)
        {
            var settings = EditUtil.Settings.Species;
            var rand = new SpeciesRandomizer(ROM.Info, Data.PersonalData);
            settings.Legends = false;
            rand.Initialize(settings, GetSpeciesBanlist());
            foreach (var entry in arr)
            {
                if (entry.Table is not { Count: > 0 } x)
                    continue;
                foreach (var t in x)
                {
                    bool isBoss = t.Species is (int)Kleavor or (int)Lilligant or (int)Arcanine or (int)Electrode or (int)Avalugg or (int)Arceus;
                    if (isBoss) // don't randomize boss battles
                        continue;
                    if (Legal.Legendary_8a.Contains(t.Species)) // don't randomize legendaries
                        continue;

                    t.Species = rand.GetRandomSpecies(t.Species);
                    t.Form = (byte)GetRandomForm(t.Species);
                    t.Nature = (NatureType)Nature.Random;
                    t.Gender = (int)FixedGender.Random;
                    t.ShinyLock = ShinyType.Random;
                    t.Move1 = t.Move2 = t.Move3 = t.Move4 = 0;
                    t.Mastered1 = t.Mastered2 = t.Mastered3 = t.Mastered4 = true;
                    t.IVHP = t.IVATK = t.IVDEF = t.IVSPA = t.IVSPD = t.IVSPE = 31;
                    t.GVHP = t.GVATK = t.GVDEF = t.GVSPA = t.GVSPD = t.GVSPE = 10;
                    t.Height = t.Weight = -1;
                }
            }
        }
    }

    [EditorCallable(EditorCategory.Field)]
    public void EditEncounterRate()
    {
        var names = ROM.GetStrings(TextName.SpeciesNames);
        PopFlat<EncounterMultiplierArchive, EncounterMultiplier>(GameFile.EncounterRateTable, "Encounter Rate Editor", z => z.Table, (z, _) => $"{names[z.Species]}{(z.Form == 0 ? "" : $"-{z.Form}")}");
    }

    [EditorCallable(EditorCategory.Field)]
    public void EditMapViewer()
    {
        var resident = (GFPack)ROM.GetFile(GameFile.Resident);
        using var form = new MapViewer8a(ROM, resident);
        form.ShowDialog();
    }

    [EditorCallable(EditorCategory.Field)]
    public void EditAreas()
    {
        using var form = new AreaEditor8a(ROM);
        form.ShowDialog();
    }

    [EditorCallable(EditorCategory.Field)]
    public void EditShinyRate() => PopFlatConfig(GameFile.ShinyRolls, "Shiny Rate Editor");

    [EditorCallable(EditorCategory.Field)]
    public void EditWormholeRate() => PopFlatConfig(GameFile.WormholeConfig, "Wormhole Config Editor");

    [EditorCallable(EditorCategory.Field)]
    public void EditOutbreakConfig() => PopFlatConfig(GameFile.OutbreakConfig, "Outbreak Configuration Editor");

    [EditorCallable(EditorCategory.Field)]
    public void EditOutbreakDetail()
        => PopFlat<MassOutbreakTable, MassOutbreak>(GameFile.Outbreak, "Outbreak Proc Editor", z => z.Table, (z, _) => z.WorkValueName);

    [EditorCallable(EditorCategory.Field)]
    public void EditNewOutbreakGroup()
        => PopFlat<NewHugeOutbreakGroupArchive, NewHugeOutbreakGroup>(GameFile.NewHugeGroup, "New Outbreak Group Editor", z => z.Table, (z, _) => z.Group.ToString("X16"));

    [EditorCallable(EditorCategory.Field)]
    public void EditNewOutbreakGroupLottery()
        => PopFlat<NewHugeOutbreakGroupLotteryArchive, NewHugeOutbreakGroupLottery>(GameFile.NewHugeGroupLottery, "New Outbreak Group Lottery Editor", z => z.Table, (z, _) => z.LotteryGroup.ToString("X16"));

    [EditorCallable(EditorCategory.Field)]
    public void EditNewOutbreakLottery()
        => PopFlat<NewHugeOutbreakLotteryArchive, NewHugeOutbreakLottery>(GameFile.NewHugeLottery, "New Outbreak Lottery Editor", z => z.Table, (z, _) => z.LotteryGroupString);

    [EditorCallable(EditorCategory.Field)]
    public void EditNewOutbreakTimeLimit()
        => PopFlat<NewHugeOutbreakTimeLimitArchive, NewHugeOutbreakTimeLimit>(GameFile.NewHugeTimeLimit, "New Outbreak Time Limit Editor", z => z.Table, (z, _) => z.Duration.ToString());

    [EditorCallable(EditorCategory.Field)]
    public void EditFieldAttackConfig() => PopFlatConfig(GameFile.AIFieldWazaConfig, "AI Field Attack Config Editor");

    [EditorCallable(EditorCategory.Field)] public void EditFieldWeatheringConfig() => PopFlatConfig(GameFile.FieldWeatheringConfig, "Field Weathering Config");
    [EditorCallable(EditorCategory.Field)] public void EditFieldWildPokemonConfig() => PopFlatConfig(GameFile.FieldWildPokemonConfig, "Field Wild Pokemon Config");
    [EditorCallable(EditorCategory.Field)] public void EditFieldLandmarkConfig() => PopFlatConfig(GameFile.FieldLandmarkConfig, "Field Landmark Config Editor");
    [EditorCallable(EditorCategory.Field)] public void EditField_Spawner_Config() => PopFlatConfig(GameFile.FieldSpawnerConfig, "Field Spawner Config Editor");
    [EditorCallable(EditorCategory.Field)] public void EditFieldAreaSpeed() => PopFlatConfig(GameFile.FieldAreaSpeedConfig, "Field Area Speed Config");
    [EditorCallable(EditorCategory.Field)] public void EditFieldCameraConfig() => PopFlatConfig(GameFile.FieldCameraConfig, "Field Camera Config");
    [EditorCallable(EditorCategory.Field)] public void EditFieldCaptureDirectorConfig() => PopFlatConfig(GameFile.FieldCaptureDirectorConfig, "Field Capture Director Config");
    [EditorCallable(EditorCategory.Field)] public void EditFieldCharaViewerConfig() => PopFlatConfig(GameFile.FieldCharaViewerConfig, "Field Chara Viewer Config");
    [EditorCallable(EditorCategory.Field)] public void EditFieldCommonConfig() => PopFlatConfig(GameFile.FieldCommonConfig, "Field Common Config");
    [EditorCallable(EditorCategory.Field)] public void EditFieldDirectItemConfig() => PopFlatConfig(GameFile.FieldDirectItemConfig, "Field Direct Item Config");
    [EditorCallable(EditorCategory.Field)] public void EditFieldEnvConfig() => PopFlatConfig(GameFile.FieldEnvConfig, "Field Env Config");
    [EditorCallable(EditorCategory.Field)] public void EditFieldItem() => PopFlatConfig(GameFile.FieldItem, "Field Item");
    [EditorCallable(EditorCategory.Field)] public void EditFieldItemRespawn() => PopFlatConfig(GameFile.FieldItemRespawn, "Field Item Respawn");
    [EditorCallable(EditorCategory.Field)] public void EditFieldLandmarkInciteConfig() => PopFlatConfig(GameFile.FieldLandmarkInciteConfig, "Field Landmark Incite Config");
    [EditorCallable(EditorCategory.Field)] public void EditFieldMyPokeBallHitNoneTargetConfig() => PopFlatConfig(GameFile.FieldBallMissedConfig, "Field My Poke Ball Hit None Target Config");
    [EditorCallable(EditorCategory.Field)] public void EditFieldObstructionWazaConfig() => PopFlatConfig(GameFile.FieldObstructionWazaConfig, "Field Obstruction Waza Config");
    [EditorCallable(EditorCategory.Field)] public void EditFieldPokemonSlopeConfig() => PopFlatConfig(GameFile.FieldPokemonSlopeConfig, "Field Pokemon Slope Config");
    [EditorCallable(EditorCategory.Field)] public void EditFieldQuestDestinationConfig() => PopFlatConfig(GameFile.FieldQuestDestinationConfig, "Field Quest Destination Config");
    [EditorCallable(EditorCategory.Field)] public void EditFieldThrowConfig() => PopFlatConfig(GameFile.FieldThrowConfig, "Field Throw Config");
    [EditorCallable(EditorCategory.Field)] public void EditFieldThrowableAfterHitConfig() => PopFlatConfig(GameFile.FieldThrowableAfterHitConfig, "Field Throwable After Hit Config");

    #endregion

    #region Battle Editors
    [EditorCallable(EditorCategory.Battle)]
    public void EditTrainers()
    {
        var folder = ROM.GetFilteredFolder(GameFile.TrainerSpecData);
        var names = folder.GetPaths().Select(Path.GetFileNameWithoutExtension).ToArray();

        var cache = new DataCache<TrainerData>(folder)
        {
            Create = FlatBufferConverter.DeserializeFrom<TrainerData>,
            Write = FlatBufferConverter.SerializeFrom,
        };

        using var form = new GenericEditor<TrainerData>(_ => cache, (_, i) => names[i] ?? string.Empty, "Trainers", Randomize, canSave: true);

        form.ShowDialog();

        void Randomize(IEnumerable<TrainerData> arr)
        {
            var settings = EditUtil.Settings.Species;
            var rand = new SpeciesRandomizer(ROM.Info, Data.PersonalData);
            rand.Initialize(settings, GetSpeciesBanlist());

            foreach (var t in arr)
            {
                foreach (var pk in t.Team)
                {
                    pk.Species = rand.GetRandomSpecies(pk.Species);
                    pk.Form = GetRandomForm(pk.Species);
                    pk.Gender = (int)FixedGender.Random;
                    pk.Nature = NatureType.Random;
                    pk.Move01.Move = pk.Move02.Move = pk.Move03.Move = pk.Move04.Move = 0;
                    pk.Move01.Mastered = pk.Move02.Mastered = pk.Move03.Mastered = pk.Move04.Mastered = true;
                    pk.Shiny = Util.Random.Next(0, 100 + 1) < 3;
                    pk.IsOybn = Util.Random.Next(0, 100 + 1) < 3;
                }
            }
        }

        if (!form.Modified)
            cache.CancelEdits();
        else
            cache.Save();
    }

    [EditorCallable(EditorCategory.Battle)]
    public void EditMoves()
    {
        var obj = ROM[GameFile.MoveStats]; // folder
        var cache = new DataCache<Waza>(obj)
        {
            Create = FlatBufferConverter.DeserializeFrom<Waza>,
            Write = FlatBufferConverter.SerializeFrom,
        };
        var names = ROM.GetStrings(TextName.MoveNames);
        using var form = new GenericEditor<Waza>(_ => cache, (_, i) => names[i], "Move Editor");
        form.ShowDialog();
        if (!form.Modified)
        {
            cache.CancelEdits();
            return;
        }

        cache.Save();
        Data.MoveData.ClearAll(); // force reload if used again
    }

    [EditorCallable(EditorCategory.Battle)] public void EditBattleCommonConfig() => PopFlatConfig(GameFile.BattleCommonConfig, "Battle Common Config Editor");
    [EditorCallable(EditorCategory.Battle)] public void EditBattleEndConfig() => PopFlatConfig(GameFile.BattleEndConfig, "Battle End Config Editor");
    [EditorCallable(EditorCategory.Battle)] public void EditBattleInConfig() => PopFlatConfig(GameFile.BattleInConfig, "Battle In Config Editor");
    [EditorCallable(EditorCategory.Battle)] public void EditBattleLogicConfig() => PopFlatConfig(GameFile.BattleLogicConfig, "Battle Logic Config Editor");
    [EditorCallable(EditorCategory.Battle)] public void EditBattleStartConfig() => PopFlatConfig(GameFile.BattleStartConfig, "Battle Start Config Editor");
    [EditorCallable(EditorCategory.Battle)] public void EditBattleViewConfig() => PopFlatConfig(GameFile.BattleViewConfig, "Battle View Config Editor");
    [EditorCallable(EditorCategory.Battle)] public void EditBattleVsnsConfig() => PopFlatConfig(GameFile.BattleVsnsConfig, "Battle Vsns Config Editor");

    #endregion

    #region Shop Editors
    [EditorCallable(EditorCategory.Shops)]
    public void EditMoveShop()
    {
        var names = ROM.GetStrings(TextName.MoveNames);
        PopFlat<MoveShopTable, MoveShopIndex>(GameFile.MoveShop, "Move Shop Editor", z => z.Table, (z, _) => names[z.Move]);
    }

    [EditorCallable(EditorCategory.Shops)]
    public void EditHaShopData()
    {
        var names = ROM.GetStrings(TextName.ItemNames);
        PopFlat<HaShopTable, HaShopItem>(GameFile.HaShop, "ha_shop_data Editor", z => z.Table, (z, _) => names[z.ItemID], Randomize);

        static void Randomize(IEnumerable<HaShopItem> arr)
        {
            foreach (var t in arr)
            {
                if (Legal.Pouch_Recipe_LA.Contains((ushort)t.ItemID)) // preserve recipes
                    continue;
                t.ItemID = Legal.Pouch_Items_LA[Util.Random.Next(Legal.Pouch_Items_LA.Length)];
            }
        }
    }

    #endregion

    #region Graphics Editors

    //[EditorCallable(EditorCategory.Graphics)]
    //public void EditPokemonModels()
    //{
        /*var pokemonModelDir = (FolderContainer)ROM[GameFile.PokemonArchiveFolder];
        pokemonModelDir.Initialize();
        CB_Species.Items.AddRange(PokemonModelDir.GetFileNames().Where(x => x != "pokeconfig.gfpak").ToArray());
        using var form = new ModelConverter(ROM);
        form.ShowDialog();
        var gfp = (GFPack)ROM.GetFile(GameFile.Resident);
        var result = PopFlat(obj.Table, "NPC Model Set Editor", (z, _) => z.NPCModelHash.ToString());
        if (!result)
            return;
        gfp[index] = FlatBufferConverter.SerializeFrom(obj);*/


        /*var pokemonModelDir = (FolderContainer)ROM[GameFile.PokemonArchiveFolder];
        pokemonModelDir.Initialize();
        var fileNames = pokemonModelDir.GetFileNames().Where(x => x != "pokeconfig.gfpak");
        var pack = new GFPack(pokemonModelDir.GetFileData(selectedFile) ?? Array.Empty<byte>());
        PokemonModelArchive archive = new(pack);
        var cache = new DataCache<PokemonModelArchive>(obj)
        {
            Create = PokemonModelArchive.new(GFPack),
            Write = FlatBufferConverter.SerializeFrom,
        };
        var tableCache = new TableCache<T1, T2>(obj);
        DataCache<PokemonModelArchive> loader(GenericEditor<PokemonModelArchive> form)
        {
            if (form.Modified)
                tableCache.Save();
            tableCache = new TableCache<T1, T2>(obj);
            return tableCache.Cache;
        }
        using var form = new GenericEditor<PokemonModelArchive>(loader, (z, _) => z.NPCModelHash.ToString(), "Pokemon Model Editor");
        form.ShowDialog();
        if (form.Modified)
        {
            tableCache.Save();
        }*/
    //}

    [EditorCallable(EditorCategory.Graphics)]
    public void EditNPCModelSet()
    {
        var gfp = (GFPack)ROM.GetFile(GameFile.Resident);
        var index = gfp.GetIndexFull("bin/field/param/placement/common/npc_model_set.bin");

        var obj = FlatBufferConverter.DeserializeFrom<NPCModelSet>(gfp[index]);
        var result = PopFlat(obj.Table, "NPC Model Set Editor", (z, _) => z.NPCModelHash.ToString());
        if (!result)
            return;
        gfp[index] = obj.SerializeFrom();
    }

    [EditorCallable(EditorCategory.Graphics)]
    public void EditPokemonModelSet()
    {
        var gfp = (GFPack)ROM.GetFile(GameFile.Resident);
        var index = gfp.GetIndexFull("bin/field/param/placement/common/pokemon_model_set.bin");

        var obj = FlatBufferConverter.DeserializeFrom<PokeModelSet>(gfp[index]);
        var names = ROM.GetStrings(TextName.SpeciesNames);

        var result = PopFlat(obj.Table, "Pokemon Model Set Editor", (z, _) => $"{names[z.Species]}{(z.Form == 0 ? "" : $"-{z.Form}")} ({z.VariantDesc})");
        if (!result)
            return;
        gfp[index] = obj.SerializeFrom();
    }

    [EditorCallable(EditorCategory.Graphics)]
    public void EditPokeResourceTable()
    {
        var names = ROM.GetStrings(TextName.SpeciesNames);
        PopFlat<PokeResourceTable, PokeModelConfig>(GameFile.PokemonResourceTable, "Pokemon Resource Table", z => z.Table, (z, _) => $"{names[z.SpeciesInfo.Species]}{(z.SpeciesInfo.Form == 0 ? "" : $"-{z.SpeciesInfo.Form}")}{(z.SpeciesInfo.Gender == 0 ? "" : $" ({z.SpeciesInfo.Gender})")}");
    }

    [EditorCallable(EditorCategory.Graphics)]
    public void EditPokemonResourceList()
    {
        var names = ROM.GetStrings(TextName.SpeciesNames);

        PopFlat<PokeInfoList, PokeInfo>(GameFile.PokemonResourceList, "Pokemon Resource List", z => z.Table, (z, _) => $"{names[z.Species]}");
    }

    [EditorCallable(EditorCategory.Graphics)]
    public void EditPokeBodyParticle()
    {
        var names = ROM.GetStrings(TextName.SpeciesNames);
        PopFlat<PokeBodyParticleArchive, PokeBodyParticle>(GameFile.PokeBodyParticle, "Pokemon Body Particle Editor", z => z.Table, (z, _) => $"{names[z.Species]}{(z.Form == 0 ? "" : $"-{z.Form}")}");
    }

    [EditorCallable(EditorCategory.Graphics)] public void EditFieldAnimationFramerate() => PopFlatConfig(GameFile.FieldAnimationFramerateConfig, "Field Anime Framerate Config");
    [EditorCallable(EditorCategory.Graphics)] public void EditWaterMotion() => PopFlatConfig(GameFile.WaterMotion, "Water Motion Configuration");
    [EditorCallable(EditorCategory.Graphics)] public void EditAppliHudConfig() => PopFlatConfig(GameFile.AppliHudConfig, "Appli Hud Config Editor");
    [EditorCallable(EditorCategory.Graphics)] public void EditAppliTipsConfig() => PopFlatConfig(GameFile.AppliTipsConfig, "Appli Tips Config Editor");
    [EditorCallable(EditorCategory.Graphics)] public void EditEventCullingConfig() => PopFlatConfig(GameFile.EventCullingConfig, "Event Culling Config Editor");
    [EditorCallable(EditorCategory.Graphics)] public void EditEventDitherConfig() => PopFlatConfig(GameFile.EventDitherConfig, "Event Dither Config Editor");
    [EditorCallable(EditorCategory.Graphics)] public void EditFieldShadowConfig() => PopFlatConfig(GameFile.FieldShadowConfig, "Field Shadow Config");
    #endregion

    #region Items Editors

    [EditorCallable(EditorCategory.Items)]
    public void EditThrowParam()
    {
        PopFlat<ThrowParamTable, ThrowParam>(GameFile.ThrowParam, "Throw Param Editor", z => z.Table, (z, _) => z.ThrowParamType.ToString());
    }

    [EditorCallable(EditorCategory.Items)]
    public void EditThrowPermissionSetParam()
    {
        PopFlat<ThrowPermissionSetDictionary, ThrowPermissionSetEntry>(GameFile.ThrowPermissionSet, "Throw Permission Editor", z => z.Table, (z, _) => z.ThrowPermissionSet.ToString());
    }

    [EditorCallable(EditorCategory.Items)]
    public void EditThrowableParam()
    {
        var itemNames = ROM.GetStrings(TextName.ItemNames);
        PopFlat<ThrowableParamTable, ThrowableParam>(GameFile.ThrowableParam, "Throwable Param Editor", z => z.Table, (z, _) => $"{itemNames[z.ItemID]} ({z.ItemID})", null, t => { t.AddEntry(0); });
    }

    [EditorCallable(EditorCategory.Items)]
    public void EditThrowResourceDictionary()
    {
        PopFlat<ThrowableResourceDictionary, ThrowableResourceEntry>(GameFile.ThrowableResource, "Throwable Resource Dictionary Editor", z => z.Table, (z, _) => z.Hash00.ToString("X16"));
    }

    [EditorCallable(EditorCategory.Items)]
    public void EditThrowResourceSetDictionary()
    {
        PopFlat<ThrowableResourceSetDictionary, ThrowableResourceSetEntry>(GameFile.ThrowableResourceSet, "Throwable Resource Set Dictionary Editor", z => z.Table, (z, _) => z.ItemType.ToString());
    }

    [EditorCallable(EditorCategory.Items)]
    public void EditItems()
    {
        var obj = ROM[GameFile.ItemStats]; // mini
        var data = obj[0];
        var items = Item8a.GetArray(data);
        var cache = new DataCache<Item8a>(items);

        var itemNames = ROM.GetStrings(TextName.ItemNames);
        using var form = new GenericEditor<Item8a>(_ => cache, (z, _) => $"{itemNames[z.ItemID]} ({z.ItemID})", "Item Editor");
        form.ShowDialog();
        if (!form.Modified)
        {
            cache.CancelEdits();
            return;
        }

        obj[0] = Item8a.SetArray(items, data);
    }

    [EditorCallable(EditorCategory.Items)] public void EditCommonItemConfig() => PopFlatConfig(GameFile.CommonItemConfig, "Common Item Config Editor");
    [EditorCallable(EditorCategory.Items, true)] public void EditEventItemConfig() => PopFlatConfig(GameFile.EventItemConfig, "Event Item Config Editor");
    #endregion

    #region NPC Editors
    [EditorCallable(EditorCategory.NPC)] public void EditNPCControllerConfig() => PopFlatConfig(GameFile.NPCControllerConfig, "NPC Controller Config");
    [EditorCallable(EditorCategory.NPC)] public void EditNPCCreaterConfig() => PopFlatConfig(GameFile.NPCCreaterConfig, "NPC Creater Config");
    [EditorCallable(EditorCategory.NPC)] public void EditNPCPopupConfig() => PopFlatConfig(GameFile.NPCPopupConfig, "NPC Popup Config");
    [EditorCallable(EditorCategory.NPC)] public void EditNPCTalkTableConfig() => PopFlatConfig(GameFile.NPCTalkTableConfig, "NPC Talk Table Config");
    [EditorCallable(EditorCategory.NPC)] public void EditEventBanditConfig() => PopFlatConfig(GameFile.EventBanditConfig, "Event Bandit Config Editor");

    #endregion

    #region Player Editors
    [EditorCallable(EditorCategory.Player)] public void EditBallThrowConfig() => PopFlatConfig(GameFile.BallThrowConfig, "Ball Throw Config Editor");
    [EditorCallable(EditorCategory.Player)] public void EditFieldLockonConfig() => PopFlatConfig(GameFile.FieldLockonConfig, "Field Lockon Config");
    [EditorCallable(EditorCategory.Player)] public void EditCharacterBipedIkConfig() => PopFlatConfig(GameFile.CharacterBipedIkConfig, "Character Biped Ik Config Editor");
    [EditorCallable(EditorCategory.Player)] public void EditCharacterBlinkConfig() => PopFlatConfig(GameFile.CharacterBlinkConfig, "Character Blink Config Editor");
    [EditorCallable(EditorCategory.Player)] public void EditCharacterControllerConfig() => PopFlatConfig(GameFile.CharacterControllerConfig, "Character Controller Config Editor");
    [EditorCallable(EditorCategory.Player)] public void EditCharacterLookAtConfig() => PopFlatConfig(GameFile.CharacterLookAtConfig, "Character Look At Config Editor");
    [EditorCallable(EditorCategory.Player)] public void EditPlayerCameraShakeConfig() => PopFlatConfig(GameFile.PlayerCameraShakeConfig, "Player Camera Shake Config");
    [EditorCallable(EditorCategory.Player)] public void EditPlayerCollisionConfig() => PopFlatConfig(GameFile.PlayerCollisionConfig, "Player Collision Config");
    [EditorCallable(EditorCategory.Player)] public void EditPlayerConfig() => PopFlatConfig(GameFile.PlayerConfig, "Player Config Editor");
    [EditorCallable(EditorCategory.Player)] public void EditPlayerControllerConfig() => PopFlatConfig(GameFile.PlayerControllerConfig, "Player Controller Config");
    [EditorCallable(EditorCategory.Player)] public void EditPlayerFaceConfig() => PopFlatConfig(GameFile.PlayerFaceConfig, "Player Face Config");
    [EditorCallable(EditorCategory.Player)] public void EditPlayer1DressupTable() => PopFlat<DressUpTable, DressUpEntry>(GameFile.Player1DressupTable, "Player 1 DressUp Table", z => z.Table, (z, _) => z.EntryName);
    [EditorCallable(EditorCategory.Player)] public void EditPlayer2DressupTable() => PopFlat<DressUpTable, DressUpEntry>(GameFile.Player2DressupTable, "Player 2 DressUp Table", z => z.Table, (z, _) => z.EntryName);

    #endregion

    #region Rides Editors
    [EditorCallable(EditorCategory.Rides)] public void EditRideBasuraoCollisionConfig() => PopFlatConfig(GameFile.RideBasuraoCollisionConfig, "Ride Basurao Collision Config");
    [EditorCallable(EditorCategory.Rides)] public void EditRideBasuraoConfig() => PopFlatConfig(GameFile.RideBasuraoConfig, "Ride Basurao Config");
    [EditorCallable(EditorCategory.Rides)] public void EditRideChangeConfig() => PopFlatConfig(GameFile.RideChangeConfig, "Ride Change Config");
    [EditorCallable(EditorCategory.Rides)] public void EditRideCommonConfig() => PopFlatConfig(GameFile.RideCommonConfig, "Ride Common Config");
    [EditorCallable(EditorCategory.Rides)] public void EditRideNyuuraCollisionConfig() => PopFlatConfig(GameFile.RideNyuuraCollisionConfig, "Ride Nyuura Collision Config");
    [EditorCallable(EditorCategory.Rides)] public void EditRideNyuuraConfig() => PopFlatConfig(GameFile.RideNyuuraConfig, "Ride Nyuura Config");
    [EditorCallable(EditorCategory.Rides)] public void EditRideNyuuraControllerConfig() => PopFlatConfig(GameFile.RideNyuuraControllerConfig, "Ride Nyuura Controller Config");
    [EditorCallable(EditorCategory.Rides)] public void EditRideOdoshishiCollisionConfig() => PopFlatConfig(GameFile.RideOdoshishiCollisionConfig, "Ride Odoshishi Collision Config");
    [EditorCallable(EditorCategory.Rides)] public void EditRideOdoshishiConfig() => PopFlatConfig(GameFile.RideOdoshishiConfig, "Ride Odoshishi Config");
    [EditorCallable(EditorCategory.Rides)] public void EditRideRingumaCollisionConfig() => PopFlatConfig(GameFile.RideRingumaCollisionConfig, "Ride Ringuma Collision Config");
    [EditorCallable(EditorCategory.Rides)] public void EditRideRingumaConfig() => PopFlatConfig(GameFile.RideRingumaConfig, "Ride Ringuma Config");
    [EditorCallable(EditorCategory.Rides)] public void EditRideRingumaControllerConfig() => PopFlatConfig(GameFile.RideRingumaControllerConfig, "Ride Ringuma Controller Config");
    [EditorCallable(EditorCategory.Rides)] public void EditRideWhooguruCollisionConfig() => PopFlatConfig(GameFile.RideWhooguruCollisionConfig, "Ride Whooguru Collision Config");
    [EditorCallable(EditorCategory.Rides)] public void EditRideWhooguruConfig() => PopFlatConfig(GameFile.RideWhooguruConfig, "Ride Whooguru Config");
    [EditorCallable(EditorCategory.Rides)] public void EditRideWhooguruControllerConfig() => PopFlatConfig(GameFile.RideWhooguruControllerConfig, "Ride Whooguru Controller Config");

    #endregion

    #region Audio Editors
    [EditorCallable(EditorCategory.Audio)] public void EditSoundConfig() => PopFlatConfig(GameFile.SoundConfig, "Sound Config");
    [EditorCallable(EditorCategory.Audio)] public void EditFieldVigilanceBgmConfig() => PopFlatConfig(GameFile.FieldVigilanceBgmConfig, "Field Vigilance Bgm Config");
    [EditorCallable(EditorCategory.Audio)] public void EditEnvPokeVoiceConfig() => PopFlatConfig(GameFile.EnvPokeVoiceConfig, "Env Poke Voice Config Editor");

    #endregion

    #region Gameplay Editors

    [EditorCallable(EditorCategory.Gameplay)]
    public void EditPokedexRankTable()
    {
        PopFlat<PokedexRankTable, PokedexRankLevel>(GameFile.DexRank, "Pokedex Rank Table Editor", z => z.Table, (z, _) => z.Rank.ToString());
    }

    [EditorCallable(EditorCategory.Gameplay)] public void EditEventBalloonRunConfig() => PopFlatConfig(GameFile.EventBalloonrunConfig, "Event Balloon Run Config Editor");
    [EditorCallable(EditorCategory.Gameplay)] public void EditEventBalloonThrowConfig() => PopFlatConfig(GameFile.EventBalloonthrowConfig, "Event Balloon Throw Config Editor");
    [EditorCallable(EditorCategory.Gameplay)] public void EditEventMkrgRewardConfig() => PopFlatConfig(GameFile.EventMkrgRewardConfig, "Event Mkrg Reward Config Editor");
    [EditorCallable(EditorCategory.Gameplay)] public void EditFarmConfig() => PopFlatConfig(GameFile.EventFarmConfig, "Farm Config Editor");
    [EditorCallable(EditorCategory.Gameplay)] public void EditEventGameOverConfig() => PopFlatConfig(GameFile.EventGameOverConfig, "Event Game Over Config Editor");
    [EditorCallable(EditorCategory.Gameplay, true)] public void EditEventQuestBoardConfig() => PopFlatConfig(GameFile.EventQuestBoardConfig, "Event Quest Board Config Editor");
    #endregion

    #region Physics Editors
    [EditorCallable(EditorCategory.Physics)] public void EditBuddyBattleConfig() => PopFlatConfig(GameFile.BuddyBattleConfig, "Buddy Battle Config Editor");
    #endregion

    #region Misc Editors

    /*[EditorCallable()]
    public void ModelConverter()
    {
        using var form = new ModelConverter(ROM);
        form.ShowDialog();
    }*/

    [EditorCallable()]
    public void EditPokeEatingHabits()
    {
        PopFlat<PokeEatingHabitsArchive, PokeEatingHabits>(GameFile.PokeEatingHabits, "Pokemon Eating Habits Editor", z => z.Table, (z, _) => z.ID.ToString());
    }

    [EditorCallable()]
    public void EditMoveObstructionLegend()
    {
        PopFlat<PokeFieldObstructionWazaNsLegendArchive, PokeFieldObstructionWazaNsLegend>(GameFile.MoveObstructionLegend, "PokeFieldObstructionWazaNsLegendArchive Editor", z => z.Table, (z, _) => "error");
    }

    [EditorCallable()]
    public void EditMoveObstructionLegendEffect()
    {
        PopFlat<PokeFieldObstructionWazaNsLegendEffectArchive, PokeFieldObstructionWazaNsLegendEffect>(GameFile.MoveObstructionLegendEffect, "PokeFieldObstructionWazaNsLegendEffectArchive Editor", z => z.Table, (z, _) => "error");
    }

    [EditorCallable()]
    public void EditMoveObstructionSE()
    {
        PopFlat<PokeFieldObstructionWazaSeArchive, PokeFieldObstructionWazaSe>(GameFile.MoveObstructionSE, "PokeFieldObstructionWazaSeArchive Editor", z => z.Table, (z, _) => "error");
    }

    [EditorCallable()]
    public void EditMoveObstructionWild()
    {
        PopFlat<PokeFieldObstructionWazaWildArchive, PokeFieldObstructionWaza>(GameFile.MoveObstructionWild, "Poke Field Obstruction Waza Wild Editor", z => z.Table, (z, _) => z.Field00);
    }

    [EditorCallable()]
    public void EditMoveObstructionWildEffect()
    {
        PopFlat<PokeFieldObstructionWazaWildEffectArchive, PokeFieldObstructionWazaWildEffect>(GameFile.MoveObstructionWildEffect, "PokeFieldObstructionWazaWildEffectArchive Editor", z => z.Table, (z, _) => "error");
    }

    [EditorCallable()]
    public void EditMoveObstructionWater()
    {
        PopFlat<PokeFieldObstructionWazaWildWaterArchive, PokeFieldObstructionWaza>(GameFile.MoveObstructionWater, "PokeFieldObstructionWazaWildWaterArchive Editor", z => z.Table, (z, _) => z.Field00);
    }

    [EditorCallable()]
    public void EditMoveObstructionWaterEffect()
    {
        PopFlat<PokeFieldObstructionWazaWildWaterEffectArchive, PokeFieldObstructionWazaWildWaterEffect>(GameFile.MoveObstructionWaterEffect, "PokeFieldObstructionWazaWildWaterEffectArchive Editor", z => z.Table, (z, _) => "error");
    }

    [EditorCallable(EditorCategory.Misc, true)]
    public void EditAppConfigList()
    {
        PopFlat<AppConfigList, AppConfigEntry>(GameFile.AppConfigList, "App Config List", z => z.Table, (z, _) => z.OriginalPath);
    }

    [EditorCallable(EditorCategory.Misc, true)]
    public void EditArchiveContents()
    {
        var archiveFolder = ROM.GetFilteredFolder(GameFile.ArchiveFolder);

        var paths = archiveFolder.GetPaths()
            .Select(p => Path.GetRelativePath(ROM.PathRomFS, p).Replace('\\', '/'))
            .ToDictionary(p => FnvHash.HashFnv1a_64(p));

        PopFlat<ArchiveContents, ArchiveContent>(GameFile.archive_contents, "Archive Contents Editor", z => z.Table,
            (z, _) => paths.GetValueOrDefault(z.ArchivePathHash) ?? z.ArchivePathHash.ToString("X16"),
            addEntryCallback: x => x.AddEntry());
    }

    //[EditorCallable(EditorCategory.Misc, true)]
    //public void EditPokedexFormStorage()
    //{
    //    //PopFlat<PokedexRankTable, PokedexRankLevel>(GameFile.DexFormStorage, "Pokedex Form Storage Editor", z => z.Rank.ToString());
    //}

    [EditorCallable(EditorCategory.Misc, true)]
    public void EditPokeDefaultLocator()
    {
        PopFlat<PokeDefaultLocatorArchive, PokeDefaultLocator>(GameFile.PokeDefaultLocator, "Poke Default Locator Editor", z => z.Table, (z, _) => z.Locator);
    }

    [EditorCallable()] public void EditAppliStaffrollConfig() => PopFlatConfig(GameFile.AppliStaffrollConfig, "Appli Staffroll Config Editor");
    [EditorCallable(EditorCategory.Misc, true)] public void EditCommonGeneralConfig() => PopFlatConfig(GameFile.CommonGeneralConfig, "Common General Config Editor");
    [EditorCallable(EditorCategory.Misc, true)] public void EditDemoConfig() => PopFlatConfig(GameFile.DemoConfig, "Demo Config Editor");
    [EditorCallable()] public void EditEventWork() => PopFlatConfig(GameFile.EventWork, "Event Work Editor");
    #endregion
}
