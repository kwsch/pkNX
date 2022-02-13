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

namespace pkNX.WinForms.Controls;

internal class EditorPLA : EditorBase
{
    protected internal EditorPLA(GameManager rom) : base(rom) => CheckOodleDllPresence();

    private static void CheckOodleDllPresence()
    {
        const string file = $"{Oodle.OodleLibraryPath}.dll";
        var dir = Application.StartupPath;
        var path = Path.Combine(dir, file);
        if (!File.Exists(path))
            WinFormsUtil.Alert($"{file} not found in the executable folder", "Some decompression functions may cause errors.");
    }

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

    public void EditTrainers()
    {
        var folder = ROM.GetFilteredFolder(GameFile.TrainerData).FilePath;
        var files = Directory.GetFiles(folder);
        var data = files.Select(FlatBufferConverter.DeserializeFrom<TrData8a>).ToArray();
        var names = files.Select(Path.GetFileNameWithoutExtension).ToArray();
        var cache = new DataCache<TrData8a>(data);
        using var form = new GenericEditor<TrData8a>(cache, names, "Trainers", canSave: false);
        form.ShowDialog();
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

    public void EditThrowable_Param()
    {
        var itemNames = ROM.GetStrings(TextName.ItemNames);
        PopFlat<ThrowableParamTable8a, ThrowableParam8a>(GameFile.ThrowableParam, "Throwable Param Editor", z => itemNames[z.ItemID]);
    }

    public void EditThrow_Param()
    {
        PopFlat<ThrowParamTable8a, ThrowParam8a>(GameFile.ThrowParam, "Throw Param Editor", z => z.Hash.ToString("X16"));
    }

    public void EditThrow_ResourceSet_Dictionary()
    {
        PopFlat<ThrowableResourceSetDictionary8a, ThrowableResourceSetEntry8a>(GameFile.ThrowableResourceSet, "Throwable ResourceSet Dictionary Editor", z => z.Hash_00.ToString("X16"));
    }

    public void EditThrow_Resource_Dictionary()
    {
        PopFlat<ThrowableResourceDictionary8a, ThrowableResourceEntry8a>(GameFile.ThrowableResource, "Throwable Resource Dictionary Editor", z => z.Hash_00.ToString("X16"));
    }

    public void EditThrow_PermissionSet_Param()
    {
        PopFlat<ThrowPermissionSetDictionary8a, ThrowPermissionSetEntry8a>(GameFile.ThrowPermissionSet, "Throw Permission Editor", z => z.Hash_00.ToString("X16"));
    }

    public void EditHa_Shop_Data()
    {
        var itemNames = ROM.GetStrings(TextName.ItemNames);
        PopFlat<HaShopTable8a, HaShopItem8a>(GameFile.HaShop, "ha_shop_data Editor", z => itemNames[z.ItemID]);
    }

    public void EditApp_Config_List()
    {
        PopFlat<AppConfigList8a, AppconfigEntry8a>(GameFile.AppConfigList, "App Config List", z => z.OriginalPath);
    }

    public void EditStatic()
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
            return $"{names[s.Species]}{(s.Form == 0 ? "" : $"-{s.Form}")} @ lv {s.Level}";
        }

        void Randomize(IEnumerable<EventEncount8a> arr)
        {
            var pt = ROM.Data.PersonalData;
            int[] ban = pt.Table.Take(ROM.Info.MaxSpeciesID + 1)
                .Select((z, i) => new { Species = i, Present = ((PersonalInfoLA)z).IsPresentInGame })
                .Where(z => !z.Present).Select(z => z.Species).ToArray();

            var spec = EditUtil.Settings.Species;
            var srand = new SpeciesRandomizer(ROM.Info, ROM.Data.PersonalData);
            var frand = new FormRandomizer(ROM.Data.PersonalData);
            srand.Initialize(spec, ban);
            foreach (var entry in arr)
            {
                if (entry.Table is not { Length: > 0 } x)
                    continue;
                var t = x[0];
                if (t.Form != 0 || t.Species is (int)Species.Arceus) // Keep boss battles same?
                    continue;
                t.Species = srand.GetRandomSpecies(t.Species);
                t.Form = (byte)frand.GetRandomForme(t.Species, false, false, true, true, ROM.Data.PersonalData.Table);
                t.Nature = (int)Nature.Serious;
                t.Gender = (int)FixedGender.Random;
                t.ShinyLock = ShinyType8a.Random;
                t.Move1 = t.Move2 = t.Move3 = t.Move4 = 0;
                t.Mastered1 = t.Mastered2 = t.Mastered3 = t.Mastered4 = true;
                t.IV_HP = t.IV_ATK = t.IV_DEF = t.IV_SPA = t.IV_SPD = t.IV_SPE = 31;
                t.GV_HP = t.GV_ATK = t.GV_DEF = t.GV_SPA = t.GV_SPD = t.GV_SPE = 31;
            }
        }
    }

    public void EditGift()
    {
        var names = ROM.GetStrings(TextName.SpeciesNames);
        var obj = ROM.GetFile(GameFile.EncounterGift);
        var data = obj[0];
        var root = FlatBufferConverter.DeserializeFrom<PokeAdd8aArchive>(data);
        var entries = root.Table;
        var result = PopFlat(entries, "Gift Encounter Editor", z => $"{names[z.Species]} @ lv {z.Level}", () => Randomize(entries));
        if (result)
            obj[0] = FlatBufferConverter.SerializeFrom(root);

        void Randomize(IEnumerable<PokeAdd8a> arr)
        {
            var pt = ROM.Data.PersonalData;
            int[] ban = pt.Table.Take(ROM.Info.MaxSpeciesID + 1)
                .Select((z, i) => new { Species = i, Present = ((PersonalInfoLA)z).IsPresentInGame })
                .Where(z => !z.Present).Select(z => z.Species).ToArray();

            var spec = EditUtil.Settings.Species;
            var srand = new SpeciesRandomizer(ROM.Info, ROM.Data.PersonalData);
            var frand = new FormRandomizer(ROM.Data.PersonalData);
            srand.Initialize(spec, ban);
            foreach (var t in arr)
            {
                if (t.Form != 0 || t.Species is (int)Species.Arceus) // Keep boss battles same?
                    continue;
                t.Species = srand.GetRandomSpecies(t.Species);
                t.Form = (byte)frand.GetRandomForme(t.Species, false, false, true, true, ROM.Data.PersonalData.Table);
                t.Nature = NatureType8a.Random;
                t.Gender = (int)FixedGender.Random;
                t.ShinyLock = ShinyType8a.Random;
                t.Move1 = t.Move2 = t.Move3 = t.Move4 = 0;
            }
        }
    }

    public void EditPersonal_Raw()
    {
        var names = ROM.GetStrings(TextName.SpeciesNames);
        PopFlat<PersonalTableLA, PersonalInfoLAfb>(GameFile.PersonalStats, "Personal Info Editor (Raw)", z => $"{names[z.Species]}{(z.Form == 0 ? "" : $"-{z.Form}")}");
    }

    public void EditLearnset_Raw()
    {
        var names = ROM.GetStrings(TextName.SpeciesNames);
        PopFlat<Learnset8a, Learnset8aMeta>(GameFile.Learnsets, "Learnset Editor (Raw)", z => $"{names[z.Species]}{(z.Form == 0 ? "" : $"-{z.Form}")}");
    }

    public void EditMiscSpeciesInfo()
    {
        var names = ROM.GetStrings(TextName.SpeciesNames);
        PopFlat<PokeMiscTable8a, PokeMisc8a>(GameFile.PokeMisc, "Misc Species Info Editor", z => $"{names[z.Species]}{(z.Form == 0 ? "" : $"-{z.Form}")} ~ {z.Value}");
    }

    public void EditSpawns()
    {
        var residentpak = ROM.GetFile(GameFile.Resident)[0];
        var resident = new GFPack(residentpak);
        using var form = new MapViewer8a(ROM, resident);
        form.ShowDialog();
    }

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
        ROM.Data.MoveData.ClearAll(); // force reload if used again
    }

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

    public void EditShinyRate() => PopFlatConfig(GameFile.ShinyRolls, "Shiny Rate Editor");
    public void EditWormholeRate() => PopFlatConfig(GameFile.WormholeConfig, "Wormhole Config Editor");
    public void EditCapture_Config() => PopFlatConfig(GameFile.CaptureConfig, "CaptureConfig Editor");
    public void EditBattle_Logic_Config() => PopFlatConfig(GameFile.BattleLogicConfig, "Battle Logic Config Editor");
    public void EditEvent_Farm_Config() => PopFlatConfig(GameFile.EventFarmConfig, "Event Farm Config Editor");
    public void EditPlayer_Config() => PopFlatConfig(GameFile.PlayerConfig, "Player Config Editor");
    public void EditField_Landmark_Config() => PopFlatConfig(GameFile.FieldLandmarkConfig, "Field Landmark Config Editor");
    public void EditBattle_View_Config() => PopFlatConfig(GameFile.BattleViewConfig, "Battle View Config Editor");
    public void EditAICommon_Config() => PopFlatConfig(GameFile.AICommonConfig, "AI Common Config Editor");
    public void EdiField_Spawner_Config() => PopFlatConfig(GameFile.FieldSpawnerConfig, "Field Spawner Config Editor");
    public void EditOutbreak_Config() => PopFlatConfig(GameFile.OutbreakConfig, "Outbreak Config Editor");
    public void EditEvolution_Config() => PopFlatConfig(GameFile.EvolutionConfig, "Evolution Config Editor");
    public void EditBall_Throw_Config() => PopFlatConfig(GameFile.BallThrowConfig, "Ball Throw Config Editor");
    public void EditSize_Scale_Config() => PopFlatConfig(GameFile.SizeScaleConfig, "Size Scale Config Editor");

    public void EditOutbreakDetail()
    {
        PopFlat<MassOutbreakTable8a, MassOutbreak8a>(GameFile.Outbreak, "Outbreak Proc Editor", z => z.WorkValueName);
    }

    public void EditSymbolBehave()
    {
        var names = ROM.GetStrings(TextName.SpeciesNames);
        PopFlat<PokeAIArchive8a, PokeAI8a>(GameFile.SymbolBehave, "Symbol Behavior Editor",             z => $"{names[z.Species]}{(z.Form != 0 ? $"-{z.Form}" : "")}", canSave: false);
    }

    public void EditMasterDump()
    {
        using var md = new DumperPLA((GameManagerPLA)ROM);
        md.ShowDialog();
    }
}
