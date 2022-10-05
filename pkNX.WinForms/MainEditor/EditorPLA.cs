using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using pkNX.Containers;
using pkNX.Game;
using pkNX.Randomization;
using pkNX.Structures;
using pkNX.Structures.FlatBuffers;
using pkNX.WinForms.Subforms;
using static pkNX.Structures.Species;

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

    public void PopFlat<T1, T2>(GameFile file, string title, Func<T2, string> getName, Action? rand = null, bool canSave = true) where T1 : class, IFlatBufferArchive<T2> where T2 : class
    {
        var obj = ROM.GetFile(file);
        var data = obj[0];
        var root = FlatBufferConverter.DeserializeFrom<T1>(data);
        var arr = root.Table;
        if (!PopFlat(arr, title, getName, rand, canSave))
            return;
        obj[0] = FlatBufferConverter.SerializeFrom(root);
    }

    private static bool PopFlat<T2>(T2[] arr, string title, Func<T2, string> getName, Action? rand = null, bool canSave = true) where T2 : class
    {
        var names = arr.Select(getName).ToArray();
        var cache = new DataCache<T2>(arr);
        using var form = new GenericEditor<T2>(cache, names, title, randomize: rand, canSave: canSave);
        form.ShowDialog();
        return form.Modified;
    }

    public void PopFlatConfig(GameFile file, string title)
    {
        var obj = ROM.GetFile(file); // flatbuffer
        var data = obj[0];
        var root = FlatBufferConverter.DeserializeFrom<ConfigureTable8a>(data);
        var cache = new DataCache<Configure8aEntry>(root.Table);
        var names = root.Table.Select(z => z.Name).ToArray();
        using var form = new GenericEditor<Configure8aEntry>(cache, names, title);
        form.ShowDialog();
        if (!form.Modified)
            return;
        obj[0] = FlatBufferConverter.SerializeFrom(root);
    }

    public int[] GetSpeciesBanlist()
    {
        var pt = Data.PersonalData;
        var hasForm = new HashSet<int>();
        var banned = new HashSet<int>();
        foreach (var pi in pt.Table.Cast<IPersonalMisc_1>())
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
        var formRand = pt.Table.Cast<IPersonalMisc_1>()
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
            _ => Randomization.Util.Random.Next(0, count),
        };
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
            PokeMisc = Data.PokeMiscData,
            Evolve = Data.EvolutionData,
            Learn = Data.LevelUpData,
            FieldDropTables = Data.FieldDrops,
            BattleDropTabels = Data.BattleDrops,
            DexResearch = Data.DexResearch
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
        PopFlat<PokeMiscTable8a, PokeMisc8a>(GameFile.PokeMisc, "Misc Species Info Editor", z => $"{names[z.Species]}{(z.Form == 0 ? "" : $"-{z.Form}")} ~ {z.Value}");
    }

    [EditorCallable(EditorCategory.Pokemon)]
    public void EditSymbolBehave()
    {
        var names = ROM.GetStrings(TextName.SpeciesNames);
        PopFlat<PokeAIArchive8a, PokeAI8a>(GameFile.SymbolBehave, "Symbol Behavior Editor", z => $"{names[z.Species]}{(z.Form != 0 ? $"-{z.Form}" : "")}");
    }

    [EditorCallable(EditorCategory.Pokemon)]
    public void EditEvolutions()
    {
        var names = ROM.GetStrings(TextName.SpeciesNames);
        PopFlat<EvolutionTable8, EvolutionSet8a>(GameFile.Evolutions, "Evolution Editor",
            z => $"{names[z.Species]}{(z.Form != 0 ? $"-{z.Form}" : "")}");
    }

    [EditorCallable(EditorCategory.Pokemon)]
    public void EditEvolutionConfig() => PopFlatConfig(GameFile.EvolutionConfig, "Evolution Config Editor");

    [EditorCallable(EditorCategory.Pokemon)]
    public void EditLearnsetRaw()
    {
        var names = ROM.GetStrings(TextName.SpeciesNames);
        PopFlat<Learnset8a, Learnset8aMeta>(GameFile.Learnsets, "Learnset Editor (Raw)", z => $"{names[z.Species]}{(z.Form == 0 ? "" : $"-{z.Form}")}");
    }

    [EditorCallable(EditorCategory.Pokemon)]
    public void EditPersonalRaw()
    {
        var names = ROM.GetStrings(TextName.SpeciesNames);
        PopFlat<PersonalTableLAfb, PersonalInfoLAfb>(GameFile.PersonalStats, "Personal Info Editor (Raw)", z => $"{names[z.DexIndexNational]}{(z.Form == 0 ? "" : $"-{z.Form}")}");
    }

    [EditorCallable(EditorCategory.Pokemon)]
    public void EditGift()
    {
        var names = ROM.GetStrings(TextName.SpeciesNames);
        var obj = ROM.GetFile(GameFile.EncounterGift);
        var data = obj[0];
        var root = FlatBufferConverter.DeserializeFrom<PokeAdd8aArchive>(data);
        var entries = root.Table;
        var result = PopFlat(entries, "Gift Encounter Editor", z => $"{names[z.Species]}{(z.Form == 0 ? "" : $"-{z.Form}")} @ Lv. {z.Level}", () => Randomize(entries));
        if (result)
            obj[0] = FlatBufferConverter.SerializeFrom(root);

        void Randomize(IEnumerable<PokeAdd8a> arr)
        {
            var settings = EditUtil.Settings.Species;
            var rand = new SpeciesRandomizer(ROM.Info, Data.PersonalData);
            settings.Legends = false;
            rand.Initialize(settings, GetSpeciesBanlist());
            foreach (var t in arr)
            {
                t.Species = rand.GetRandomSpecies(t.Species);
                t.Form = (byte)GetRandomForm(t.Species);
                t.Nature = NatureType8a.Random;
                t.Gender = (int)FixedGender.Random;
                t.ShinyLock = ShinyType8a.Random;
                t.Ball = Randomization.Util.Random.Next(27, 37); // [Strange, Origin]
                t.Move1 = t.Move2 = t.Move3 = t.Move4 = 0;
                t.Height = t.Weight = -1;
            }
        }
    }

    [EditorCallable(EditorCategory.Pokemon)] public void EditBuddyBattleConfig() => PopFlatConfig(GameFile.BuddyBattleConfig, "Buddy Battle Config Editor");
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
        var obj = FlatBufferConverter.DeserializeFrom<AreaWeatherTable8a>(data);
        var result = PopFlat(obj.Table, "Area Weather Editor", z => z.Hash.ToString("X16"));
        if (!result)
            return;
        gfp[2065] = FlatBufferConverter.SerializeFrom(obj);
    }

    [EditorCallable(EditorCategory.Field)]
    public void EditStaticEncounter()
    {
        var names = ROM.GetStrings(TextName.SpeciesNames);
        var obj = ROM.GetFile(GameFile.EncounterStatic);
        var data = obj[0];
        var root = FlatBufferConverter.DeserializeFrom<EventEncount8aArchive>(data);
        var entries = root.Table;
        var result = PopFlat(entries, "Static Encounter Editor", z => $"{z.EncounterName} ({GetDetail(z, names)})", () => Randomize(entries));
        if (result)
            obj[0] = FlatBufferConverter.SerializeFrom(root);

        static string GetDetail(EventEncount8a z, string[] names)
        {
            if (z.Table is not { Length: not 0 } x)
                return "No Entries";
            var s = x[0];
            return $"{names[s.Species]}{(s.Form == 0 ? "" : $"-{s.Form}")} @ Lv. {s.Level}";
        }

        void Randomize(IEnumerable<EventEncount8a> arr)
        {
            var settings = EditUtil.Settings.Species;
            var rand = new SpeciesRandomizer(ROM.Info, Data.PersonalData);
            settings.Legends = false;
            rand.Initialize(settings, GetSpeciesBanlist());
            foreach (var entry in arr)
            {
                if (entry.Table is not { Length: > 0 } x)
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
                    t.Nature = (int)Nature.Random;
                    t.Gender = (int)FixedGender.Random;
                    t.ShinyLock = ShinyType8a.Random;
                    t.Move1 = t.Move2 = t.Move3 = t.Move4 = 0;
                    t.Mastered1 = t.Mastered2 = t.Mastered3 = t.Mastered4 = true;
                    t.IV_HP = t.IV_ATK = t.IV_DEF = t.IV_SPA = t.IV_SPD = t.IV_SPE = 31;
                    t.GV_HP = t.GV_ATK = t.GV_DEF = t.GV_SPA = t.GV_SPD = t.GV_SPE = 10;
                    t.Height = t.Weight = -1;
                }
            }
        }
    }

    [EditorCallable(EditorCategory.Field)]
    public void EditMapViewer()
    {
        var resident = (GFPack)ROM.GetFile(GameFile.Resident);
        using var form = new MapViewer8a((GameManagerPLA)ROM, resident);
        form.ShowDialog();
    }

    [EditorCallable(EditorCategory.Field)]
    public void EditAreas()
    {
        using var form = new AreaEditor8a((GameManagerPLA)ROM);
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
        => PopFlat<MassOutbreakTable8a, MassOutbreak8a>(GameFile.Outbreak, "Outbreak Proc Editor", z => z.WorkValueName);

    [EditorCallable(EditorCategory.Field)]
    public void EditNewOutbreakGroup()
        => PopFlat<NewHugeOutbreakGroupArchive8a, NewHugeOutbreakGroup8a>(GameFile.NewHugeGroup, "New Outbreak Group Editor", z => z.Group.ToString("X16"));

    [EditorCallable(EditorCategory.Field)]
    public void EditNewOutbreakGroupLottery()
        => PopFlat<NewHugeOutbreakGroupLotteryArchive8a, NewHugeOutbreakGroupLottery8a>(GameFile.NewHugeGroupLottery, "New Outbreak Group Lottery Editor", z => z.LotteryGroup.ToString("X16"));

    [EditorCallable(EditorCategory.Field)]
    public void EditNewOutbreakLottery()
        => PopFlat<NewHugeOutbreakLotteryArchive8a, NewHugeOutbreakLottery8a>(GameFile.NewHugeLottery, "New Outbreak Lottery Editor", z => z.LotteryGroupString);

    [EditorCallable(EditorCategory.Field)]
    public void EditNewOutbreakTimeLimit()
        => PopFlat<NewHugeOutbreakTimeLimitArchive8a, NewHugeOutbreakTimeLimit8a>(GameFile.NewHugeTimeLimit, "New Outbreak Time Limit Editor", z => z.Duration.ToString());

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
    [EditorCallable(EditorCategory.Field)] public void EditFieldMyPokeBallHitNoneTargetConfig() => PopFlatConfig(GameFile.FieldMyPokeBallHitNoneTargetConfig, "Field My Poke Ball Hit None Target Config");
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
        var folder = ROM.GetFilteredFolder(GameFile.TrainerData);
        var cache = new DataCache<TrData8a>(folder)
        {
            Create = FlatBufferConverter.DeserializeFrom<TrData8a>,
            Write = FlatBufferConverter.SerializeFrom,
        };
        var names = folder.GetPaths().Select(Path.GetFileNameWithoutExtension).ToArray();
        using var form = new GenericEditor<TrData8a>(cache, names, "Trainers", Randomize, canSave: true);
        form.ShowDialog();

        void Randomize()
        {
            var settings = EditUtil.Settings.Species;
            var rand = new SpeciesRandomizer(ROM.Info, Data.PersonalData);
            rand.Initialize(settings, GetSpeciesBanlist());

            for (int i = 0; i < cache.Length; i++)
            {
                foreach (var t in cache[i].Team)
                {
                    t.Species = rand.GetRandomSpecies(t.Species);
                    t.Form = GetRandomForm(t.Species);
                    t.Gender = (int)FixedGender.Random;
                    t.Nature = NatureType8a.Random;
                    t.Move_01.Move = t.Move_02.Move = t.Move_03.Move = t.Move_04.Move = 0;
                    t.Move_01.Mastered = t.Move_02.Mastered = t.Move_03.Mastered = t.Move_04.Mastered = true;
                    t.Shiny = Randomization.Util.Random.Next(0, 100 + 1) < 3;
                    t.IsOybn = Randomization.Util.Random.Next(0, 100 + 1) < 3;
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
        var cache = new DataCache<Waza8a>(obj)
        {
            Create = FlatBufferConverter.DeserializeFrom<Waza8a>,
            Write = FlatBufferConverter.SerializeFrom,
        };
        using var form = new GenericEditor<Waza8a>(cache, ROM.GetStrings(TextName.MoveNames), "Move Editor");
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
        var data = ROM.GetFile(GameFile.MoveShop);
        var obj = FlatBufferConverter.DeserializeFrom<MoveShopTable8a>(data[0]);
        var result = PopFlat(obj.Table, "Move Shop Editor", z => names[z.Move]);
        if (!result)
            return;
        data[0] = FlatBufferConverter.SerializeFrom(obj);
    }

    [EditorCallable(EditorCategory.Shops)]
    public void EditHaShopData()
    {
        var names = ROM.GetStrings(TextName.ItemNames);
        var data = ROM.GetFile(GameFile.HaShop);
        var obj = FlatBufferConverter.DeserializeFrom<HaShopTable8a>(data[0]);
        var result = PopFlat(obj.Table, "ha_shop_data Editor", z => names[z.ItemID], Randomize);
        if (!result)
            return;

        void Randomize()
        {
            foreach (var t in obj.Table)
            {
                if (Legal.Pouch_Recipe_LA.Contains((ushort)t.ItemID)) // preserve recipes
                    continue;
                t.ItemID = Legal.Pouch_Items_LA[Randomization.Util.Random.Next(Legal.Pouch_Items_LA.Length)];
            }
        }

        data[0] = FlatBufferConverter.SerializeFrom(obj);
    }

    #endregion

    #region Graphics Editors
    [EditorCallable(EditorCategory.Graphics)]
    public void EditFieldAnimationFramerate() => PopFlatConfig(GameFile.FieldAnimationFramerateConfig, "Field Anime Framerate Config");

    [EditorCallable(EditorCategory.Graphics)]
    public void EditWaterMotion() => PopFlatConfig(GameFile.WaterMotion, "Water Motion Configuration");
    [EditorCallable(EditorCategory.Graphics)] public void EditAppliHudConfig() => PopFlatConfig(GameFile.AppliHudConfig, "Appli Hud Config Editor");
    [EditorCallable(EditorCategory.Graphics)] public void EditAppliTipsConfig() => PopFlatConfig(GameFile.AppliTipsConfig, "Appli Tips Config Editor");

    [EditorCallable(EditorCategory.Graphics)] public void EditFieldShadowConfig() => PopFlatConfig(GameFile.FieldShadowConfig, "Field Shadow Config");

    [EditorCallable(EditorCategory.Graphics)]
    public void EditNPCModelSet()
    {
        var gfp = (GFPack)ROM.GetFile(GameFile.Resident);
        var index = gfp.GetIndexFull("bin/field/param/placement/common/npc_model_set.bin");

        var obj = FlatBufferConverter.DeserializeFrom<NPCModelSet8a>(gfp[index]);
        var result = PopFlat(obj.Table, "NPC Model Set Editor", z => z.NPCModelHash.ToString());
        if (!result)
            return;
        gfp[index] = FlatBufferConverter.SerializeFrom(obj);
    }

    #endregion

    #region Items Editors
    [EditorCallable(EditorCategory.Items)] public void EditCommonItemConfig() => PopFlatConfig(GameFile.CommonItemConfig, "Common Item Config Editor");

    [EditorCallable(EditorCategory.Items)]
    public void EditThrowParam()
    {
        PopFlat<ThrowParamTable8a, ThrowParam8a>(GameFile.ThrowParam, "Throw Param Editor", z => z.Hash.ToString("X16"));
    }

    [EditorCallable(EditorCategory.Items)]
    public void EditThrowPermissionSetParam()
    {
        PopFlat<ThrowPermissionSetDictionary8a, ThrowPermissionSetEntry8a>(GameFile.ThrowPermissionSet, "Throw Permission Editor", z => z.Hash_00.ToString("X16"));
    }

    [EditorCallable(EditorCategory.Items)]
    public void EditThrowableParam()
    {
        var itemNames = ROM.GetStrings(TextName.ItemNames);
        PopFlat<ThrowableParamTable8a, ThrowableParam8a>(GameFile.ThrowableParam, "Throwable Param Editor", z => itemNames[z.ItemID]);
    }
    [EditorCallable(EditorCategory.Items)]
    public void EditThrowResourceDictionary()
    {
        PopFlat<ThrowableResourceDictionary8a, ThrowableResourceEntry8a>(GameFile.ThrowableResource, "Throwable Resource Dictionary Editor", z => z.Hash_00.ToString("X16"));
    }
    [EditorCallable(EditorCategory.Items)]
    public void EditThrowResourceSetDictionary()
    {
        PopFlat<ThrowableResourceSetDictionary8a, ThrowableResourceSetEntry8a>(GameFile.ThrowableResourceSet, "Throwable Resource Set Dictionary Editor", z => z.Hash_00.ToString("X16"));
    }

    [EditorCallable(EditorCategory.Items)]
    public void EditItems()
    {
        var obj = ROM[GameFile.ItemStats]; // mini
        var data = obj[0];
        var items = Item8a.GetArray(data);
        var cache = new DataCache<Item8a>(items);
        using var form = new GenericEditor<Item8a>(cache, ROM.GetStrings(TextName.ItemNames), "Item Editor");
        form.ShowDialog();
        if (!form.Modified)
        {
            cache.CancelEdits();
            return;
        }

        obj[0] = Item8a.SetArray(items, data);
    }
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

    #region Misc Editors

    [EditorCallable(EditorCategory.Misc)]
    public void EditAppConfigList()
    {
        PopFlat<AppConfigList8a, AppconfigEntry8a>(GameFile.AppConfigList, "App Config List", z => z.OriginalPath);
    }
    [EditorCallable(EditorCategory.Misc)] public void EditAppliStaffrollConfig() => PopFlatConfig(GameFile.AppliStaffrollConfig, "Appli Staffroll Config Editor");
    [EditorCallable(EditorCategory.Misc)] public void EditCommonGeneralConfig() => PopFlatConfig(GameFile.CommonGeneralConfig, "Common General Config Editor");
    [EditorCallable(EditorCategory.Misc)] public void EditDemoConfig() => PopFlatConfig(GameFile.DemoConfig, "Demo Config Editor");
    [EditorCallable(EditorCategory.Misc)] public void EditEventBalloonrunConfig() => PopFlatConfig(GameFile.EventBalloonrunConfig, "Event Balloonrun Config Editor");
    [EditorCallable(EditorCategory.Misc)] public void EditEventBalloonthrowConfig() => PopFlatConfig(GameFile.EventBalloonthrowConfig, "Event Balloonthrow Config Editor");
    [EditorCallable(EditorCategory.Misc)] public void EditEventCullingConfig() => PopFlatConfig(GameFile.EventCullingConfig, "Event Culling Config Editor");
    [EditorCallable(EditorCategory.Misc)] public void EditEventDitherConfig() => PopFlatConfig(GameFile.EventDitherConfig, "Event Dither Config Editor");
    [EditorCallable(EditorCategory.Misc)] public void EditFarmConfig() => PopFlatConfig(GameFile.EventFarmConfig, "Farm Config Editor");
    [EditorCallable(EditorCategory.Misc)] public void EditEventGameOverConfig() => PopFlatConfig(GameFile.EventGameOverConfig, "Event Game Over Config Editor");
    [EditorCallable(EditorCategory.Misc)] public void EditEventItemConfig() => PopFlatConfig(GameFile.EventItemConfig, "Event Item Config Editor");
    [EditorCallable(EditorCategory.Misc)] public void EditEventMkrgRewardConfig() => PopFlatConfig(GameFile.EventMkrgRewardConfig, "Event Mkrg Reward Config Editor");
    [EditorCallable(EditorCategory.Misc)] public void EditEventQuestBoardConfig() => PopFlatConfig(GameFile.EventQuestBoardConfig, "Event Quest Board Config Editor");
    [EditorCallable(EditorCategory.Misc)] public void EditEventRestrictionBattle() => PopFlatConfig(GameFile.EventRestrictionBattle, "Event Restriction Battle Editor");
    [EditorCallable(EditorCategory.Misc)] public void EditEventWork() => PopFlatConfig(GameFile.EventWork, "Event Work Editor");

    public void EditMasterDump()
    {
        using var md = new DumperPLA(ROM);
        md.ShowDialog();
    }
    #endregion
}
