using FlatSharp;
using pkNX.Containers;
using pkNX.Game;
using pkNX.Structures;
using pkNX.Structures.FlatBuffers;
using pkNX.Structures.FlatBuffers.ZA;
using pkNX.Structures.FlatBuffers.ZA.Trinity;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Serialization;
using System.Windows;
using Schema = pkNX.Structures.FlatBuffers.Reflection;
using Trigger = pkNX.Structures.FlatBuffers.ZA.Trigger;

// ReSharper disable StringLiteralTypo

namespace pkNX.WinForms;

public class GameDumper9a(GameManager9a rom)
{
    public string DumpFolder
    {
        get
        {
            var parent = Directory.GetParent(rom.PathRomFS) ?? throw new DirectoryNotFoundException($"Unable to find parent directory of {rom.PathRomFS}");
            return Path.Combine(parent.FullName, "Dump");
        }
    }

    private const string message = "ik_message"; // ik_messagedat
    private const string DumpArchive = "archive";
    private const string DumpArchiveExtracted = TrinityPakExtractor.DumpArchiveExtracted;

    /// <summary>
    /// Message string language names/folders which contain the text localizations.
    /// </summary>
    private static readonly string[] LanguageEnglishNames =
    [
        "JPN",
        "JPN_KANJI",
        "English",
        "French",
        "Italian",
        "German",
        "Spanish",
        "Korean",
        "Simp_Chinese",
        "Trad_Chinese",
        "LATAM",
    ];

    private string[] GetCommonText(string name, string lang, TextConfig cfg)
    {
        var data = rom.GetPackedFile($"{message}/dat/{lang}/common/{name}.dat");
        return new TextFile(data, cfg).Lines;
    }

    private ushort[] GetCommonTextFlags(string name, string lang, TextConfig cfg)
    {
        var data = rom.GetPackedFile($"{message}/dat/{lang}/common/{name}.dat");
        return new TextFile(data, cfg).Flags;
    }

    private AHTB GetCommonAHTB(string name, string lang)
    {
        var data = rom.GetPackedFile($"{message}/dat/{lang}/common/{name}.tbl");
        return new AHTB(data);
    }

    private string GetPath(params ReadOnlySpan<string> folders)
    {
        Directory.CreateDirectory(DumpFolder);
        var result = Path.Combine(DumpFolder, Path.Combine(folders));
        var parent = Directory.GetParent(result) ?? throw new DirectoryNotFoundException($"Unable to get parent directory of {result}");
        Directory.CreateDirectory(parent.FullName); // double check :(
        return result;
    }

    public void DumpAHTB()
    {
        var arcPath = GetPath(DumpArchive, DumpArchiveExtracted);
        if (!Directory.Exists(arcPath))
        {
            WinFormsUtil.Alert("Archive not extracted yet. Please extract the archive first.");
            return;
        }
        var files = Directory.EnumerateFiles(arcPath, "*.ahtb", SearchOption.AllDirectories);

        var result = new HashSet<string>();
        var list = new List<string>();
        foreach (var f in files)
        {
            try
            {
                var bytes = File.ReadAllBytes(f);
                var tbl = new AHTB(bytes);
                var summaries = tbl.Summary;
                foreach (var t in tbl.ShortSummary)
                    result.Add(t);
                var fn = Path.GetFileName(f);
                list.Add(fn);
                list.AddRange(summaries);
            }
            catch
            {
            }
        }

        var outname = GetPath("ahtb.txt");
        var outname2 = GetPath("ahtblist.txt");
        File.WriteAllLines(outname, result);
        File.WriteAllLines(outname2, list);
    }

    public void DumpStrings()
    {
        var textConfig = new TextConfig(GameVersion.ZA);
        ReadOnlySpan<string> groups = ["ik_messagedat"];
        ReadOnlySpan<string> types = ["script", "common", "sk"];
        foreach (var group in groups)
        {
            foreach (var type in types)
                GetStrings(textConfig, type, group);
        }
    }

    private void GetStrings(TextConfig textConfig, string type, string messageGroup)
    {
        var arcPath = Path.Combine(GetPath(DumpArchive, DumpArchiveExtracted), "arc");
        foreach (var lang in LanguageEnglishNames)
            RipLanguage(textConfig, type, lang, arcPath, messageGroup);
    }

    private void RipLanguage(TextConfig textConfig, string type, string lang, string arcPath, string messageGroup)
    {
        var prefix = $"{messageGroup}{lang}";
        const string suffix = ".trpak";
        var pattern = $"{prefix}*{suffix}";
        List<(string File, string[] Lines)> text = [];
        List<(string File, string[] Lines)> full = [];
        var folders = Directory.EnumerateDirectories(arcPath, pattern, SearchOption.TopDirectoryOnly);
        foreach (var folder in folders)
        {
            // Get the file name out from the pattern we specified above.
            var name = Path.GetFileName(folder).AsSpan();
            var textFile = name.Slice(prefix.Length, name.Length - prefix.Length - suffix.Length);
            if (!textFile.StartsWith(type))
                continue;
            var fn = Path.ChangeExtension(textFile[type.Length..].ToString(), null);
            var (shortText, fullText) = GetTextTuple(textConfig, lang, folder, type, fn);
            text.Add(new(fn, shortText));
            full.Add(new(fn, fullText));
        }

        DumpTotalFiles(type, lang, text, full);
    }

    private (string[] Text, string[] Full) GetTextTuple(TextConfig textConfig, string lang, string dumped, string folder, string file)
    {
        var files = Directory.GetFiles(dumped);
        if (files.Length != 2)
        {
            if (files.Length == 1 && !files[0].EndsWith("ahtb"))
            {
                var dat = files.Single(z => !z.EndsWith("ahtb"));
                var dest = Path.Combine(lang, folder, $"{file}.txt");
                var pathLines = GetPath("text", dest);
                var textData = File.ReadAllBytes(dat);
                var lines = TextFile.GetStrings(textData, textConfig)!;
                File.WriteAllLines(pathLines, lines);
                return (lines, lines);
            }
            return (Array.Empty<string>(), Array.Empty<string>());
        }

        {
            var dat = files.Single(z => !z.EndsWith("ahtb"));
            var ahtb = files.Single(z => z.EndsWith("ahtb"));

            var dest = Path.Combine(lang, folder, $"{file}.txt");
            var pathLines = GetPath("text", dest);
            var textData = File.ReadAllBytes(dat);
            var lines = TextFile.GetStrings(textData, textConfig)!;
            File.WriteAllLines(pathLines, lines);

            var ahtbData = File.ReadAllBytes(ahtb);
            var table = new AHTB(ahtbData);
            var detailed = table.MergeFlat(lines);

            var pathAHTB = GetPath("textFull", dest);
            File.WriteAllLines(pathAHTB, detailed);
            return (lines, detailed);
        }
    }

    private void DumpTotalFiles(string type, string lang,
        IReadOnlyList<(string File, string[] Lines)> text,
        IReadOnlyList<(string File, string[] Lines)> full)
    {
        var outText = GetPath("text", Path.Combine(lang, $"{type}.txt"));
        using var swt = File.CreateText(outText);
        var outFull = GetPath("text", Path.Combine(lang, $"{type}Full.txt"));
        using var swf = File.CreateText(outFull);
        for (var i = 0; i < text.Count; i++)
        {
            static void WriteFileHeader(string file, TextWriter s)
            {
                s.WriteLine("~~~~~~~~~~~~~~~");
                s.WriteLine($"Text File : {file}.dat");
                s.WriteLine("~~~~~~~~~~~~~~~");
            }

            var (fn, lines) = full[i];
            WriteFileHeader(fn, swf);
            foreach (var line in lines)
                swf.WriteLine(line);

            (fn, lines) = text[i];
            WriteFileHeader(fn, swt);
            foreach (var line in lines)
                swt.WriteLine(line);
        }
    }

    private void Dump<TTable, TEntry>(ulong hash, Func<TTable, IList<TEntry>> sel)
        where TTable : class, IFlatBufferSerializable<TTable>
        where TEntry : notnull
    {
        var flat = Get<TTable>(hash);
        var path = GetPath(hash.ToString("X16"));
        Dump(sel, flat, path);
    }

    private TTable Dump<TTable, TEntry>(string f, Func<TTable, IList<TEntry>> sel)
        where TTable : class, IFlatBufferSerializable<TTable>
        where TEntry : notnull
    {
        var flat = Get<TTable>(f);
        var path = GetRawPath(f);
        Dump(sel, flat, path);
        return flat;
    }

    private string GetRawPath(string fileName) => GetPath("raw", fileName.Replace('/', Path.DirectorySeparatorChar));

    private static void Dump<TTable, TEntry>(Func<TTable, IList<TEntry>> sel, TTable flat, string path)
        where TTable : class, IFlatBufferSerializable<TTable> where TEntry : notnull
    {
        DumpJson(flat, path);
        var table = sel(flat);
        var dump = TableUtil.GetTable(table);

        var fileName = Path.ChangeExtension(path, ".txt");
        File.WriteAllText(fileName, dump);
    }

    private TTable Get<TTable>(ulong hash)
        where TTable : class, IFlatBufferSerializable<TTable>
    {
        var bin = rom.GetPackedFile(hash);
        return Get<TTable>(bin);
    }

    private TTable Get<TTable>(string f)
        where TTable : class, IFlatBufferSerializable<TTable>
    {
        var bin = rom.GetPackedFile(f.Replace("bfbs", "bin"));
        return Get<TTable>(bin);
    }

    private static TTable Get<TTable>(Memory<byte> bin) where TTable : class, IFlatBufferSerializable<TTable> => FlatBufferConverter.DeserializeFrom<TTable>(bin);

    private void DumpSel<TTable, TEntry>(string f, Func<TTable, IEnumerable<TEntry>> sel, string prefix = "sel")
        where TTable : class, IFlatBufferSerializable<TTable>
        where TEntry : notnull
    {
        var bin = rom.GetPackedFile(f.Replace("bfbs", "bin"));
        var path = GetPath("raw", f.Replace('/', Path.DirectorySeparatorChar));
        DumpSel(sel, prefix, bin, path);
    }

    private static void DumpSel<TTable, TEntry>(Func<TTable, IEnumerable<TEntry>> sel, string prefix, Memory<byte> bin, string path)
        where TTable : class, IFlatBufferSerializable<TTable> where TEntry : notnull
    {
        var flat = FlatBufferConverter.DeserializeFrom<TTable>(bin);
        var table = sel(flat);
        var dump = TableUtil.GetTable(table);

        var fileName = Path.ChangeExtension(path, $".{prefix}.txt");
        try
        {
            File.WriteAllText(fileName, dump);
        }
        catch
        {
            // Ignore. File likely already exists and is open in an editor.
        }
    }

    private void DumpX<TTable, TEntry, TSub>(string f, Func<TTable, IList<TEntry>> sel, Func<TEntry, IList<TSub>> sel2)
        where TTable : class, IFlatBufferSerializable<TTable>
        where TEntry : class
        where TSub : notnull
    {
        var bin = rom.GetPackedFile(f.Replace("bfbs", "bin"));
        var flat = FlatBufferConverter.DeserializeFrom<TTable>(bin);

        var path = GetPath("raw", f.Replace('/', Path.DirectorySeparatorChar));
        DumpJson(flat, path);

        var arr = sel(flat);
        for (int i = 0; i < arr.Count; i++)
        {
            var entry = sel2(arr[i]);
            var dump = TableUtil.GetTable(entry);

            var folder = Path.GetDirectoryName(path);
            var fileName = Path.Combine(folder!, $"{typeof(TEntry).Name}_{i}.txt");
            File.WriteAllText(fileName, dump);
        }
    }

    private T DumpJson<T>(string f) where T : class, IFlatBufferSerializable<T>
    {
        var bin = rom.GetPackedFile(f.Replace("bfbs", "bin"));
        var flat = FlatBufferConverter.DeserializeFrom<T>(bin);

        var path = GetPath("raw", f.Replace('/', Path.DirectorySeparatorChar));
        DumpJson(flat, path);
        return flat;
    }


    private static readonly System.Text.Json.JsonSerializerOptions Options = new()
    {
        WriteIndented = true,
        IncludeFields = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        NumberHandling = JsonNumberHandling.AllowNamedFloatingPointLiterals,
    };

    private static void DumpJson<T>(T flat, string filePath) where T : class
    {
        var json = System.Text.Json.JsonSerializer.Serialize(flat, Options);

        var fileName = Path.ChangeExtension(filePath, ".json");
        File.WriteAllText(fileName, json);
    }

    public void DumpSpecific()
    {
        string[] files =
        [
            "world/ik_data/ui/pokedex/black_list_main/black_list_main_array.bfbs",
            "world/ik_data/rank_battle/reward/reward_array.bfbs",

            "world/ik_data/field/area_jump/field_area_jump_point/field_area_jump_point_array.bfbs",

            "world/ik_data/field/area/field_main_area/field_main_area_array.bfbs",
            "world/ik_data/field/area/field_sub_area/field_sub_area_array.bfbs",
            "world/ik_data/field/area/field_location/field_location_array.bfbs",
            "world/ik_data/field/area/field_battle_zone/field_battle_zone_array.bfbs",
            "world/ik_data/field/area/field_wild_zone/field_wild_zone_array.bfbs",
            "world/ik_data/field/area/field_scene_area/field_scene_area_array.bfbs",

            "world/ik_data/field/area/area_config/area_config_data.bfbs",
            "world/ik_data/field/config/field_config/field_config_data.bfbs",

            "world/ik_data/field/map_replace/map_replace_data/map_replace_data_array.bfbs",

            "world/ik_data/field/data/field_data_config/field_data_config_array.bfbs",

            "world/ik_data/field/pokemon_spawner/pokemon_spawner_data/pokemon_spawner_data_array.bfbs",
            "world/ik_data/field/spawner_transform_data/pokemon_spawner_transform/pokemon_spawner_transform/pokemon_spawner_transform_array.bfbs",

            "world/ik_data/field/weather/field_weather_table/field_weather_table.bfbs",

            "world/ik_data/playreport/playreport_types/key_to_data_types.bfbs",
            "world/ik_data/field/landscape/field_landscape_terrain_layer_table/field_landscape_terrain_layer_table_array.bfbs",
            "world/ik_data/audio/engine/poke_type_to_audio_group/poke_type_to_audio_group_array.bfbs",
            "world/ik_data/ui/ruby_description/ruby_description_data.bfbs",
            "world/ik_data/playreport/playreport_events/playreport_events_array.bfbs",
            "world/ik_data/field/landscape/field_landscape_attribute_table/field_landscape_attribute_table_array.bfbs",
            "world/ik_data/audio/engine/init_settings/init_settings_data.bfbs",
            "world/ik_data/audio/engine/group_settings/group_settings_array.bfbs",

            "world/ik_data/field/pokemon/pokemon_data/pokemon_data/pokemon_data_array.bfbs",
            "world/ik_data/field/pokemon/encount_data/encount_data/encount_data_array.bfbs",

            "world/ik_data/field/oyabun/oyabun_waza/oyabun_waza.bfbs",

            "world/ik_data/field/item_ball/item_ball_spawner_data/item_ball_spawner_data/item_ball_spawner_data_array.bfbs",
            "world/ik_data/field/spawner_transform_data/pokemon_spawner_transform/pokemon_spawner_transform/pokemon_spawner_transform_array.bfbs",

            "world/ik_data/field/npc/npc_asset_data/npc_asset_data/npc_asset_data.bfbs",

            "world/ik_data/field/random_pop_item/random_pop_item_table/random_pop_item_table_data/random_pop_item_table_data_array.bfbs",
            "world/ik_data/field/random_pop_item_spawner/random_pop_item_spawner_data/random_pop_item_spawner_data_array.bfbs",
            "world/ik_data/field/field_wazagimmick_spawner/wazagimmick_spawner_data/wazagimmick_spawner_data_array.bfbs",

            "ik_event/bin/flag/temp_work.bin",
            "ik_event/bin/flag/quest_work.bin",
            "ik_event/bin/flag/system_flag.bin",
            "ik_event/bin/flag/momiji_work.bin",
            "ik_event/bin/flag/system_work.bin",
            "ik_event/bin/flag/event_flag.bin",
            "ik_event/bin/flag/temp_flag.bin",
            "ik_event/bin/momiji_count/momiji_count.bin",

            "ik_event/bin/mission_card/mission_card.bin",
            "ik_event/bin/xconst/event_xconst.bin",
            "ik_event/bin/quest/main_quest.bin",
            "ik_event/bin/quest/sub_quest.bin",
            "ik_event/bin/quest/momiji_quest.bin",
            "ik_event/bin/phase/phase.bin",
            "ik_event/bin/flower/flower.bin",
            "ik_event/bin/event_control/event_control.bin",

            "world/exl/dungeon_map_ui_data/dungeon_map_ui_data/dungeon_map_ui_data.bin",

            "world/ik_data/field/weather/weather_schedule_table/weather_schedule_table/weather_schedule_table.bfbs",
            "world/ik_data/field/weather/field_weather_table/field_weather_table.bfbs",
            "world/ik_data/field/weather/weather_happening/weather_happening_array.bfbs",
        ];
        foreach (var f in files)
            RipBFBS(f);
    }

    public void RipBFBS(string f)
    {
        DumpPackedFile(f);
        DumpPackedFile(Path.ChangeExtension(f, ".bin"));
    }

    public void DumpFromTextFile(string path)
    {
        var files = File.ReadAllLines(path);
        foreach (var f in files)
        {
            if (!rom.HasFile(f))
                continue;
            DumpPackedFile(f);
        }
    }

    private void DumpPackedFile(string f)
    {
        var path = GetPath("raw", f.Replace('/', Path.DirectorySeparatorChar));
        var dir = Path.GetDirectoryName(path)!;
        if (!Directory.Exists(dir))
            Directory.CreateDirectory(dir);

        var data = rom.GetPackedFile(f);
        File.WriteAllBytes(path, data);

        var ext = Path.GetExtension(f);
        if (ext == ".bfbs")
        {
            var settings = new Schema.SchemaDumpSettings { FileNamespace = "pkNX.Structures.FlatBuffers.ZA", EmitGeneratedCountComments = false };
            var schema = Schema.SchemaDump.HandleReflection(data, path, dir, settings);
            DumpJson(schema, Path.ChangeExtension(path, ".schema.json"));
        }
    }

    public void DumpHashReflectionBFBS()
    {
        ulong[] files =
        [
            // This isn't really needed as the game has very leaky file names. Leave in for future reference.
        ];
        foreach (var f in files)
        {
            if (!rom.HasFile(f))
                continue;

            var path = GetPath("hashFile", f.ToString("X16"));
            var dir = Path.GetDirectoryName(path)!;
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            var data = rom.GetPackedFile(f);
            File.WriteAllBytes(path, data);
            try
            {
                var settings = new Schema.SchemaDumpSettings();
                var schema = Schema.SchemaDump.HandleReflection(data, path, dir, settings);
                DumpJson(schema, Path.ChangeExtension(path, ".schema.json"));
            }
            catch
            {
            }
        }
    }

    public void DumpArchives()
    {
        var dirRootDump = GetPath(DumpArchive);
        if (Directory.Exists(dirRootDump))
        {
            var dr = WinFormsUtil.Prompt(MessageBoxButton.YesNo, "Archive dump folder already exists -- are you sure you want to dump again?");
            if (dr != MessageBoxResult.Yes)
                return;
        }
        TrinityPakExtractor.DumpArchives(rom.PathRomFS, dirRootDump);
        Debug.WriteLine("Loaded");
    }

    public void DumpMisc()
    {
        Dump<ItemTableArray, ItemTable>("world/ik_data/battle/plib_item_conversion/plib_item_conversion_array.bfbs", z => z.Table);
        Dump<RankBattleRewardArray, RankBattleReward>("world/ik_data/rank_battle/reward/reward_array.bfbs", z => z.Table);
        Dump<PokedexBlackListMainArray, PokedexBlackListMain>("world/ik_data/ui/pokedex/black_list_main/black_list_main_array.bfbs", z => z.Table);

        DumpJson<CaptureBallData>("world/ik_data/capture/capture_ball_data/capture_ball_data.bin");
        DumpJson<CaptureData>("world/ik_data/capture/capture_data/capture_data.bin");
        DumpJson<CaptureZARankData>("world/ik_data/capture/capture_zarank_data/capture_zarank_data.bin");

        Dump<ZARewardItemDataArray, ZARewardItemData>("ik_event/bin/za_reward_item_data/za_reward_item_data.bin", z => z.Table);
        Dump<NpcAssetDataDBArray, NpcAssetData>("world/ik_data/field/npc/npc_asset_data/npc_asset_data/npc_asset_data.bfbs", z => z.Table);
        Dump<TrainerBattleGlobalDBArray, TrainerBattleData>("ik_event/bin/event_trainer_battle_data/trainer_btl_data_global.bin", z => z.Table);
        Dump<ShopLineupArray, ShopLineup>("world/exl/shop/shop_item_lineup/shop_item_lineup.bin", z => z.Table);
        Dump<ItemTableDataDBArray, ItemTableDataDB>("world/ik_data/field/random_pop_item/random_pop_item_table/random_pop_item_table_data/random_pop_item_table_data_array.bfbs", z => z.Table);
        Dump<RandomPopItemSpawnerDataDBArray, RandomPopItemSpawnerDataDB>("world/ik_data/field/random_pop_item_spawner/random_pop_item_spawner_data/random_pop_item_spawner_data_array.bfbs", z => z.Table);
        Dump<MedalRateDefeatBonusArray, MedalRateDefeatBonus>("ik_event/bin/medal_rate/defeat_bonus.bin", z => z.Table);

        Dump<HudLineupArray, HudLineup>("world/exl/hologram/hud_shopping/hud_shopping.bin", z => z.Table);

        Dump<EventConstArray, EventConst>("ik_event/bin/xconst/event_xconst.bin", z => z.Table);
        Dump<EventControlArray, EventControl>("ik_event/bin/event_control/event_control.bin", z => z.Table);

        Dump<TitleArray, Title>("ik_event/bin/title/title.bin", z => z.Table);
        Dump<TitleCountArray, TitleCount>("ik_event/bin/title_count/title_count.bin", z => z.Table);

        DumpJson<FieldWeatherTable>("world/ik_data/field/weather/field_weather_table/field_weather_table.bin");
        Dump<WeatherHappeningParamArray, WeatherHappeningParam>("world/ik_data/field/weather/weather_happening/weather_happening_array.bin", z => z.Table);

        DumpDressUp();

        const string pst = "world/ik_data/field/spawner_transform_data/placement_npc_spawner_transform/placement_npc_spawner_transform/placement_npc_spawner_transform_array.bfbs";
        var spawnerT = Dump<SpawnerTransformDataDBArray, SpawnerTransformDataDB>(pst, z => z.Table);
        for (var i = 0; i < spawnerT.Table.Count; i++)
        {
            var table = spawnerT.Table[i];
            var lines = TableUtil.GetTable(table.Table);
            var path = GetRawPath(pst);
            var fileName = Path.ChangeExtension(path, $".{i:00}.txt");
            File.WriteAllText(fileName, lines);
        }

        DumpJson<ConditionRareDBArray>("param_ai/data/ai/trigger/condition_rare/condition_rare_array.bin");

        Dump<DonutFlavorArray, DonutFlavor>("world/exl/donut/flavor/flavor.bin", z => z.Table);
        Dump<DonutRecipeArray, DonutRecipe>("world/exl/donut/donut_recipe/donut_recipe.bin", z => z.Table);
        Dump<BerryDetailArray, BerryDetail>("world/exl/donut/berry/berry.bin", z => z.Table);
        Dump<DonutDetailArray, DonutDetail>("world/exl/donut/donut/donut.bin", z => z.Table);
        Dump<DonutFlavorLotteryArray, DonutFlavorLottery>("world/exl/donut/flavor_lottery/flavor_lottery.bin", z => z.Table);
        Dump<DonutFlavorParameterArray, DonutFlavorParameter>("world/exl/donut/flavor_parameter/flavor_parameter.bin", z => z.Table);
        Dump<DonutOneWordArray, DonutOneWord>("world/exl/donut/oneword/oneword.bin", z => z.Table);
    }

    private void DumpDressUp()
    {
        var groups = Dump<DressUpGroupDataArray, DressUpGroupData>("world/exl/dress_up_data/dress_up_group_data/dress_up_group_data.bin", z => z.Table);
        const string ens = "world/exl/dress_up_data/dress_up_ensemble_data/dress_up_ensemble_data.bin";
        Dump<DressUpEnsembleDataArray, DressUpEnsembleData>(ens, z => z.Table);
        DumpSel<DressUpEnsembleDataArray, DressUpEnsemble>(ens, z => z.Table.SelectMany(e => e.Ensemble), "ens");
        var dress = Dump<DressUpDataArray, DressUpData>("world/exl/dress_up_data/dress_up_data/dress_up_data.bin", z => z.Table);
        foreach (var language in LanguageEnglishNames)
            DumpLabels(dress.Table, groups.Table, language);

        var hm = Dump<HairMakeDataArray, HairMakeData>("world/exl/hair_make_data/hair_make_data/hair_make_data.bin", z => z.Table);
    }

    private void DumpLabels(IList<DressUpData> dress, IList<DressUpGroupData> groups, string language)
    {
        const string textFileNameLabel = "dressup_item_name"; // .dat
        var config = new TextConfig(GameVersion.ZA);
        var text = GetCommonText(textFileNameLabel, language, config);
        var ahtb = GetCommonAHTB(textFileNameLabel, language);
        var dictionary = ahtb.ToDictionary(text);

        // Name->Group
        var groupGet = new Dictionary<string, (string Group, string Localized)>(groups.Count);
        foreach (var group in groups)
        {
            var hash = FnvHash.HashFnv1a_64(group.Group);
            var localized = dictionary[hash];
            var tuple = (group.Group, localized);
            groupGet.Add(group.Name, tuple);
        }

        var result = new string[dress.Count];
        for (int i = 0; i < dress.Count; i++)
        {
            var d = dress[i];
            var name = d.Name;
            var color1 = d.Color1;
            var color2 = d.Color2;
            bool hasColor2 = color2 != "dressup_item_color_null";
            var colorString = dictionary[FnvHash.HashFnv1a_64(color1)];
            if (hasColor2)
                colorString += $", {dictionary[FnvHash.HashFnv1a_64(color2)]}";

            var (_, localized) = groupGet[name];

            var nameString = $"{localized} ({colorString})";
            var line = $"{d.MagicValue:D8}\t{nameString}";
            result[i] = line;
        }

        var file = GetPath(language, "dressup_labels.txt");
        File.WriteAllLines(file, result);
    }

    public void DumpScrubbedInfo()
    {
        DumpScrubbedPokedexEntries();
        DumpScrubbedItemInfo();
        DumpScrubbedMoveInfo();
        DumpScrubbedPokedexCategories();
        DumpScrubbedDressupItemNames();
    }

    public void DumpFieldItems()
    {
        var files = new string[]
        {
            "world/ik_scene/field/area/t1/sub_scene/t1_gimmick/t1_item_ball_spawner_/t1_item_ball_spawner_0.trscn",
            "world/ik_scene/field/area/t1_i004a/sub_scene/t1_i004a_gimmick/t1_i004a_item_ball_spawner_/t1_i004a_item_ball_spawner_0.trscn",
            "world/ik_scene/field/area/t1_i004b/sub_scene/t1_i004b_gimmick/t1_i004b_item_ball_spawner_/t1_i004b_item_ball_spawner_0.trscn",
            "world/ik_scene/field/area/t1_i004c/sub_scene/t1_i004c_gimmick/t1_i004c_item_ball_spawner_/t1_i004c_item_ball_spawner_0.trscn",
            "world/ik_scene/field/area/t1_i004d/sub_scene/t1_i004d_gimmick/t1_i004d_item_ball_spawner_/t1_i004d_item_ball_spawner_0.trscn",
            "world/ik_scene/field/area/t1_i006_2/sub_scene/t1_i006_2_gimmick/t1_i006_2_item_ball_spawner_/t1_i006_2_item_ball_spawner_0.trscn",
            "world/ik_scene/field/area/t1_i006_3/sub_scene/t1_i006_3_gimmick/t1_i006_3_item_ball_spawner_/t1_i006_3_item_ball_spawner_0.trscn",
            "world/ik_scene/field/area/t1_i007a/sub_scene/t1_i007a_gimmick/t1_i007a_item_ball_spawner_/t1_i007a_item_ball_spawner_0.trscn",
            "world/ik_scene/field/area/t1_i008_1/sub_scene/t1_i008_1_gimmick/t1_i008_1_item_ball_spawner_/t1_i008_1_item_ball_spawner_0.trscn",
            "world/ik_scene/field/area/t1_i011/sub_scene/t1_i011_gimmick/t1_i011_item_ball_spawner_/t1_i011_item_ball_spawner_0.trscn",
            "world/ik_scene/field/area/t1_i012_1/sub_scene/t1_i012_1_gimmick/t1_i012_1_item_ball_spawner_/t1_i012_1_item_ball_spawner_0.trscn",
            "world/ik_scene/field/area/t1_i012_2/sub_scene/t1_i012_2_gimmick/t1_i012_2_item_ball_spawner_/t1_i012_2_item_ball_spawner_0.trscn",
            "world/ik_scene/field/area/t1_i014_1/sub_scene/t1_i014_1_gimmick/t1_i014_1_item_ball_spawner_/t1_i014_1_item_ball_spawner_0.trscn",
            "world/ik_scene/field/area/t1_last_battle/sub_scene/t1_lb_gimmick/t1_lb_item_ball_spawner_/t1_lb_item_ball_spawner_0.trscn",
            "world/ik_scene/field/area/t2/sub_scene/t2_gimmick/t2_item_ball_spawner_/t2_item_ball_spawner_0.trscn",
            "world/ik_scene/field/area/t3/sub_scene/t3_gimmick/t3_item_ball_spawnwer_/t3_item_ball_spawnwer_0.trscn",
            "world/ik_scene/field/area/t3_2/sub_scene/t3_2_gimmick/t3_2_item_ball_spawnwer_/t3_2_item_ball_spawnwer_0.trscn",
        };

        var fileSpawners = rom.GetPackedFile("world/ik_data/field/item_ball/item_ball_spawner_data/item_ball_spawner_data/item_ball_spawner_data_array.bin");
        var objSpawners = FlatBufferConverter.DeserializeFrom<SpawnerDataDBArray>(fileSpawners);
        List<(string FieldItemID, string ItemID)> spawners = [];

        const string path = "world/exl/item_data/item_data/item_data.bin";
        var itemTable = Get<ItemDataArray>(path).Table;

        bool IsTM(int id)
        {
            var item = itemTable.FirstOrDefault(z => z.Id == id);
            if (item is null)
                return false;
            return item.MachineIndex > -1;
        }

        int GetTMIndex(int id)
        {
            var item = itemTable.FirstOrDefault(z => z.Id == id);
            if (item is null)
                return -1;
            return item.MachineIndex + 1;
        }

        const string lang = "English";
        var cfg = new TextConfig(GameVersion.ZA);
        var names = GetCommonText("itemname", lang, cfg);

        foreach (var spawner in objSpawners.Table)
        {
            var data = spawner.Table;
            foreach (var item in data)
            {
                var fieldItemID = item.Id.StartsWith("itd_") ? item.Id : item.Id[(item.Id.IndexOf('_') + 1)..];
                foreach (var info in item.TableInfoList)
                    spawners.Add((fieldItemID, info.TableId!));
            }
        }

        foreach (var file in files)
        {
            var area = file.Split('/')[4];
            var fileScene = rom.GetPackedFile(file);
            var objScene = FlatBufferConverter.DeserializeFrom<TrinitySceneObjectTemplate>(fileScene);
            List<string> result =
            [
                "Spawner\tItem ID\tItem Name\tX\tY\tZ"
            ];

            foreach (var t in objScene.Objects.Where(z => z.Type is "trinity_ObjectTemplate"))
            {
                var obj = FlatBufferConverter.DeserializeFrom<TrinitySceneObjectTemplateData>(t.Data);
                var pos = FlatBufferConverter.DeserializeFrom<TrinitySceneObject>(obj.Data).Position.Translation;
                var name = obj.ObjectTemplateName;
                var item = spawners.FirstOrDefault(z => z.FieldItemID == name);

                // dummy items, not associated to any spawners
                if (item.ItemID is null)
                    continue;

                var itemID = int.Parse(item.ItemID[16..20]);
                var itemName = IsTM(itemID) ? $"TM{GetTMIndex(itemID):000}" : names[itemID];
                result.Add($"{name}\t{itemID:0000}\t{itemName}\t{pos.X}\t{pos.Y}\t{pos.Z}");
            }

            if (result.Count > 1)
                File.WriteAllLines(GetPath("field_items", $"field_items_{area}.txt"), result);
        }
    }

    public void DumpHiddenItems()
    {
        var files = new[]
        {
            "world/ik_scene/field/area/t1/sub_scene/t1_gimmick/t1_random_pop_item_spawner_/t1_random_pop_item_spawner_0.trscn",
            "world/ik_scene/field/area/t1/sub_scene/t1_gimmick/t1_random_pop_item_spawner/t1_center_random_pop_item_sapwner_/t1_center_random_pop_item_sapwner_0.trscn",
            "world/ik_scene/field/area/t1/sub_scene/t1_gimmick/t1_random_pop_item_spawner/t1_rouge_random_pop_item_spawner_/t1_rouge_random_pop_item_spawner_0.trscn",
            "world/ik_scene/field/area/t1/sub_scene/t1_gimmick/t1_random_pop_item_spawner/t1_rose_random_pop_item_spawner_/t1_rose_random_pop_item_spawner_0.trscn",
            "world/ik_scene/field/area/t1/sub_scene/t1_gimmick/t1_random_pop_item_spawner/t1_jaune_random_pop_item_spawner_/t1_jaune_random_pop_item_spawner_0.trscn",
            "world/ik_scene/field/area/t1/sub_scene/t1_gimmick/t1_random_pop_item_spawner/t1_blue_random_pop_item_spawner_/t1_blue_random_pop_item_spawner_0.trscn",
            "world/ik_scene/field/area/t1/sub_scene/t1_gimmick/t1_random_pop_item_spawner/t1_veil_random_pop_item_spawner_/t1_veil_random_pop_item_spawner_0.trscn",
            "world/ik_scene/field/area/t2/sub_scene/t2_gimmick/t2_random_pop_item_spawner_/t2_random_pop_item_spawner_0.trscn",
            "world/ik_scene/field/area/t3/sub_scene/t3_gimmick/t3_random_pop_item_spawner_/t3_random_pop_item_spawner_0.trscn",
            "world/ik_scene/field/area/t3_2/sub_scene/t3_2_gimmick/t3_2_random_pop_item_spawner_/t3_2_random_pop_item_spawner_0.trscn",
        };

        // get all hidden item tables
        var itemTable = rom.GetPackedFile("world/ik_data/field/random_pop_item/random_pop_item_table/random_pop_item_table_data/random_pop_item_table_data_array.bin");
        var objItem = FlatBufferConverter.DeserializeFrom<ItemTableDataDBArray>(itemTable);
        List<string> tables = [];
        List<List<(string ItemID, int ItemWeight)>> items = [];

        foreach (var t in objItem.Table)
        {
            var data = t.Data;
            foreach (var i in data)
                tables.Add(i.Id);
        }

        foreach (var t in objItem.Table)
        {
            foreach (var d in t.Data)
            {
                List<(string ItemID, int ItemWeight)> result = [];
                foreach (var i in d.ItemLotteryDataList)
                    result.Add((i.ItemId, i.Weight));
                items.Add(result);
            }
        }

        // get all hidden item spawner objects
        var spawnerTable = rom.GetPackedFile("world/ik_data/field/random_pop_item_spawner/random_pop_item_spawner_data/random_pop_item_spawner_data_array.bin");
        var objSpawner = FlatBufferConverter.DeserializeFrom<RandomPopItemSpawnerDataDBArray>(spawnerTable);
        List<(string ObjectName, string TableID)> spawners = [];
        foreach (var t in objSpawner.Table)
        {
            var data = t.Table;
            foreach (var i in data)
            {
                var objectName = i.AppearanceSpawnerObjectInfoList[0].ObjectName;
                var tableID = i.TableInfoList[0].TableId;
                spawners.Add((objectName, tableID));
            }
        }

        // get all hidden item spawner coordinates
        List<(string TableID, PackedVec3f Coordinates)> coordinates = [];
        foreach (var file in files)
        {
            var fileScene = rom.GetPackedFile(file);
            var objScene = FlatBufferConverter.DeserializeFrom<TrinitySceneObjectTemplate>(fileScene);
            foreach (var t in objScene.Objects.Where(z => z.Type is "trinity_ObjectTemplate"))
            {
                var obj = FlatBufferConverter.DeserializeFrom<TrinitySceneObjectTemplateData>(t.Data);
                var pos = FlatBufferConverter.DeserializeFrom<TrinitySceneObject>(obj.Data).Position.Translation;
                var name = obj.ObjectTemplateName;
                coordinates.Add((name, pos));
            }
        }

        const string lang = "English";
        var cfg = new TextConfig(GameVersion.ZA);
        var names = GetCommonText("itemname", lang, cfg);

        // output to a table!
        List<string> output = [];
        foreach (var (objectName, tableID) in spawners)
        {
            var (tableId, pos) = coordinates.FirstOrDefault(z => z.TableID == objectName);
            var (_, s) = spawners.FirstOrDefault(z => z.TableID == tableID);

            if (s.Contains("_test_"))
                continue;

            output.Add($"Table ID: {s}");
            output.Add($"Hidden Item Table: {tableId}");
            output.Add($"Coordinates: V3f({pos.X}, {pos.Y}, {pos.Z})");

            var itemTableIndex = tables.IndexOf(s);
            var itemList = items[itemTableIndex].OrderByDescending(z => z.ItemWeight).ToList();
            float totalWeight = 0;

            foreach (var item in itemList)
                totalWeight += item.ItemWeight;

            foreach (var item in itemList)
            {
                float rate = (float)(Math.Round((item.ItemWeight / totalWeight) * 100f, 2));
                var itemID = int.Parse(item.ItemID);
                output.Add($"- {rate:00.00}% {names[itemID]}");
            }

            output.Add("");
        }

        File.WriteAllLines(GetPath("field_items", "hidden_items.txt"), output);
    }

    public void DumpMegaCrystals()
    {
        var files = new[]
        {
            "world/ik_scene/field/area/t1/sub_scene/t1_gimmick/t1_waza_gimmick/t1_waza_gimmick_blue_/t1_waza_gimmick_blue_0.trscn",
            "world/ik_scene/field/area/t1/sub_scene/t1_gimmick/t1_waza_gimmick/t1_waza_gimmick_center_/t1_waza_gimmick_center_0.trscn",
            "world/ik_scene/field/area/t1/sub_scene/t1_gimmick/t1_waza_gimmick/t1_waza_gimmick_jaune_/t1_waza_gimmick_jaune_0.trscn",
            "world/ik_scene/field/area/t1/sub_scene/t1_gimmick/t1_waza_gimmick/t1_waza_gimmick_rose_/t1_waza_gimmick_rose_0.trscn",
            "world/ik_scene/field/area/t1/sub_scene/t1_gimmick/t1_waza_gimmick/t1_waza_gimmick_rouge_/t1_waza_gimmick_rouge_0.trscn",
            "world/ik_scene/field/area/t1/sub_scene/t1_gimmick/t1_waza_gimmick/t1_waza_gimmick_veil_/t1_waza_gimmick_veil_0.trscn",
            "world/ik_scene/field/area/t1/sub_scene/t1_gimmick/t1_waza_gimmick_/t1_waza_gimmick_0.trscn",
            "world/ik_scene/field/area/t1/sub_scene/t1_zone/t1_wild_a0101_w01/t1_wild_a0101_w01_waza_gimmick_/t1_wild_a0101_w01_waza_gimmick_0.trscn",
            "world/ik_scene/field/area/t1/sub_scene/t1_zone/t1_wild_a0102_w01/t1_wild_a0102_w01_waza_gimmick_/t1_wild_a0102_w01_waza_gimmick_0.trscn",
            "world/ik_scene/field/area/t1/sub_scene/t1_zone/t1_wild_a0103_w01/t1_wild_a0103_w01_waza_gimmick_/t1_wild_a0103_w01_waza_gimmick_0.trscn",
            "world/ik_scene/field/area/t1/sub_scene/t1_zone/t1_wild_a0201_w01/t1_wild_a0201_w01_waza_gimmick_/t1_wild_a0201_w01_waza_gimmick_0.trscn",
            "world/ik_scene/field/area/t1/sub_scene/t1_zone/t1_wild_a0202_w01/t1_wild_a0202_w01_waza_gimmick_/t1_wild_a0202_w01_waza_gimmick_0.trscn",
            "world/ik_scene/field/area/t1/sub_scene/t1_zone/t1_wild_a0202_w02/t1_wild_a0202_w02_waza_gimmick_/t1_wild_a0202_w02_waza_gimmick_0.trscn",
            "world/ik_scene/field/area/t1/sub_scene/t1_zone/t1_wild_a0203_w01/t1_wild_a0203_w01_waza_gimmick_/t1_wild_a0203_w01_waza_gimmick_0.trscn",
            "world/ik_scene/field/area/t1/sub_scene/t1_zone/t1_wild_a0301_w01/t1_wild_a0301_w01_waza_gimmick_/t1_wild_a0301_w01_waza_gimmick_0.trscn",
            "world/ik_scene/field/area/t1/sub_scene/t1_zone/t1_wild_a0301_w02/t1_wild_a0301_w02_waza_gimmick_/t1_wild_a0301_w02_waza_gimmick_0.trscn",
            "world/ik_scene/field/area/t1/sub_scene/t1_zone/t1_wild_a0302_w01/t1_wild_a0302_w01_waza_gimmick_/t1_wild_a0302_w01_waza_gimmick_0.trscn",
            "world/ik_scene/field/area/t1/sub_scene/t1_zone/t1_wild_a0303_w01/t1_wild_a0303_w01_waza_gimmick_/t1_wild_a0303_w01_waza_gimmick_0.trscn",
            "world/ik_scene/field/area/t1/sub_scene/t1_zone/t1_wild_a0401_w01/t1_wild_a0401_w01_waza_gimmick_/t1_wild_a0401_w01_waza_gimmick_0.trscn",
            "world/ik_scene/field/area/t1/sub_scene/t1_zone/t1_wild_a0402_w01/t1_wild_a0402_w01_waza_gimmick_/t1_wild_a0402_w01_waza_gimmick_0.trscn",
            "world/ik_scene/field/area/t1/sub_scene/t1_zone/t1_wild_a0403_w01/t1_wild_a0403_w01_waza_gimmick_/t1_wild_a0403_w01_waza_gimmick_0.trscn",
            "world/ik_scene/field/area/t1/sub_scene/t1_zone/t1_wild_a0501_w01/t1_wild_a0501_w01_waza_gimmick_/t1_wild_a0501_w01_waza_gimmick_0.trscn",
            "world/ik_scene/field/area/t1/sub_scene/t1_zone/t1_wild_a0501_w02/t1_wild_a0501_w02_waza_gimmick_/t1_wild_a0501_w02_waza_gimmick_0.trscn",
            "world/ik_scene/field/area/t1/sub_scene/t1_zone/t1_wild_a0502_w01/t1_wild_a0502_w01_waza_gimmick_/t1_wild_a0502_w01_waza_gimmick_0.trscn",
            "world/ik_scene/field/area/t1/sub_scene/t1_zone/t1_wild_a0502_w02/t1_wild_a0502_w02_waza_gimmick_/t1_wild_a0502_w02_waza_gimmick_0.trscn",
            "world/ik_scene/field/area/t1/sub_scene/t1_zone/t1_wild_a0503_w01/t1_wild_a0503_w01_waza_gimmick_/t1_wild_a0503_w01_waza_gimmick_0.trscn",
            "world/ik_scene/field/area/t1/sub_scene/t1_zone/t1_wild_a0601_w01/t1_wild_a0601_w01_waza_gimmick_/t1_wild_a0601_w01_waza_gimmick_0.trscn",
            "world/ik_scene/field/area/t1/sub_scene/t1_zone/t1_wild_a0601_w01_v01/t1_wild_a0601_w01_v01_waza_gimmick_/t1_wild_a0601_w01_v01_waza_gimmick_0.trscn",
            "world/ik_scene/field/area/t1/sub_scene/t1_zone/t1_wild_a0601_w01_v02/t1_wild_a0601_w01_v02_waza_gimmick_/t1_wild_a0601_w01_v02_waza_gimmick_0.trscn",
            "world/ik_scene/field/area/t1/sub_scene/t1_zone/t1_wild_a0601_w01_v03/t1_wild_a0601_w01_v03_waza_gimmick_/t1_wild_a0601_w01_v03_waza_gimmick_0.trscn",
            "world/ik_scene/field/area/t2/sub_scene/t2_gimmick/t2_waza_gimmick_/t2_waza_gimmick_0.trscn",
            "world/ik_scene/field/area/t3/sub_scene/t3_gimmick/t3_waza_gimmick_/t3_waza_gimmick_0.trscn",
            "world/ik_scene/field/area/t3_2/sub_scene/t3_2_gimmick/t3_2_waza_gimmick_/t3_2_waza_gimmick_0.trscn",
        };

        const string wgp = "world/ik_data/field/field_wazagimmick_private/field_wazagimmick_private/field_wazagimmick_private_array.bin";
        const string wsd = "world/ik_data/field/field_wazagimmick_spawner/wazagimmick_spawner_data/wazagimmick_spawner_data_array.bin";
        var wgd = rom.GetPackedFile(wgp);
        var wgt = Get<FieldWazaGimmickPrivateDBArray>(wgd).Table;
        Dump<FieldWazaGimmickPrivateDBArray, FieldWazaGimmickPrivateDB>(wgp, z => z.Table);
        Dump<WazaGimmickSpawnerDataDBArray, WazaGimmickSpawnerDataDB>(wsd, z => z.Table);
        List<(string Identifier, PackedVec3f Coordinates)> gimmicks = [];

        foreach (var file in files)
        {
            var scene = rom.GetPackedFile(file);
            var objScene = FlatBufferConverter.DeserializeFrom<TrinitySceneObjectTemplate>(scene);

            foreach (var t in objScene.Objects.Where(z => z.Type is "trinity_ObjectTemplate"))
            {
                var obj = FlatBufferConverter.DeserializeFrom<TrinitySceneObjectTemplateData>(t.Data);
                var pos = FlatBufferConverter.DeserializeFrom<TrinitySceneObject>(obj.Data).Position.Translation;
                var name = obj.ObjectTemplateName;
                var coords = new PackedVec3f { X = pos.X, Y = pos.Y, Z = pos.Z };

                if (name.StartsWith("spn_"))
                    name = name[4..];

                gimmicks.Add((name, coords));
            }
        }

        List<string> Result =
        [
            "Object Name\tCluster Type\tX\tY\tZ"
        ];
        foreach (var (identifier, pos) in gimmicks)
        {
            foreach (var t in wgt)
            {
                var tbl = t.Table;
                var cluster = tbl.FirstOrDefault(z => z.ObjectName.Contains(identifier));
                if (cluster is null)
                    continue;
                var id = cluster.Extension.ItemPopId;
                if (!IsMegaCrystalCluster(id))
                    continue;

                foreach (var c in cluster.ObjectName)
                    Result.Add($"{c}\t{id}\t{pos.X}\t{pos.Y}\t{pos.Z}");
            }
        }

        File.WriteAllLines(GetPath("field_items", "mega_crystals.txt"), Result);
        return;
        static bool IsMegaCrystalCluster(string? id) => id is "id_wazagim_item_spn_mega_01" or "id_wazagim_item_spn_mega_11";
    }

    public void DumpMoves()
    {
        const string language = "English";

        const string wazaPath = "avalon/data/waza_array.bin";
        Dump<WazaTable, Waza>(wazaPath, z => z.Table);
        var table = Get<WazaTable>(wazaPath);
        var outPath = GetPath("pkhex");

        var pp = table.Table.Select(z => z.PP).Select(z => $"{z:00}");
        File.WriteAllText(Path.Combine(outPath, "move_pp.txt"), string.Join(',', pp));

        var types = table.Table.Select(z => z.Type).Select(z => $"{z:00}");
        File.WriteAllText(Path.Combine(outPath, "move_type.txt"), string.Join(',', types));

        // get dummied moves
        var cfg = new TextConfig(rom.Game);
        string[] GetText(string name) => GetCommonText(name, language, cfg);

        var moveNames = GetText("wazaname");
        var moveDesc = GetText("wazainfo");

        var moves = table.Table;
        var snapped = new List<ushort>(moves.Count);
        for (int i = 1; i < moveNames.Length; i++)
        {
            var move = moves.FirstOrDefault(m => m.MoveID == i);
            if (move?.CanUseMove != true)
                snapped.Add((ushort)i);
        }

        var path = GetPath("snappedMoves.txt");
        var tuple = snapped.Select(z => $"{z:000}\t{moveNames[z]}");
        File.WriteAllLines(path, tuple); // rest in peace

        var snap2 = GetPath("snappedID.txt");
        var snapMove = string.Join(", ", snapped.Select(z => $"{z:000}"));
        File.WriteAllText(snap2, snapMove);

        var dummied = new byte[(snapped[^1] >> 3) + 1];
        foreach (var value in snapped)
            dummied[value >> 3] |= (byte)(1 << (value & 7));
        var dumb = GetPath("dummied.txt");
        var dummiedList = string.Join(',', dummied.Select(z => $"0x{z:X2}"));
        File.WriteAllText(dumb, dummiedList);

        // actual move info
        const string pwpa = "param_ai/data/battle/personal_waza_param/personal_waza_param_array.bfbs";
        Dump<PersonalWazaParamDBArray, PersonalWazaParamDB>(pwpa, z => z.Table);
        DumpSel<PersonalWazaParamDBArray, PersonalWazaParam>(pwpa, z => z.Table.SelectMany(x => x.Table), "x");

        const string wpa = "param_ai/data/ai/waza/waza_param/waza_param_array.bin";
        Dump<WazaParamDBArray, WazaParamDB>(wpa, z => z.Table);
        DumpSel<WazaParamDBArray, WazaParam>(wpa, z => z.Table.SelectMany(x => x.Table), "x");
    }

    public void DumpPersonal()
    {
        const string suffix = "za";
        var ident = "za"u8;
        const string personalPath = "avalon/data/personal_array.bin";
        Dump<PersonalTable, PersonalInfo>(personalPath, z => z.Table);

        var perbin = rom.GetPackedFile(personalPath);
        var pt = new PersonalTable9ZA(new FakeContainer([perbin]));

        const string alphaMovesPath = "world/ik_data/field/oyabun/oyabun_waza/oyabun_waza.bin";
        var alphaMoves = Dump<OyabunWazaDB, OyabunWaza>(alphaMovesPath, z => z.Table)
            .Table;

        // Only acknowledge the first set of entries.
        foreach (var alpha in alphaMoves)
        {
            foreach (var form in alpha.FormTableList)
            {
                var pi = pt[SpeciesConverterZA.GetNational9(alpha.DevNo), (byte)form.FormNo];
                if (pi.AlphaMove == 0)
                    pi.AlphaMove = form.WazaNo;
                // don't throw an exception if already non-zero -- Raichu's mega forms were incorrectly configured as 1/2 rather than 2/3.
                // The Alpha Move search returns on first match.
                // the only clash is Raichu-1, which needs to stay as Iron Tail (not Play Rough).
            }
        }

        var bin = pt.Write();
        var path2 = GetPath("pkhex", "personal_za");
        File.WriteAllBytes(path2, bin);

        var learnsets = SerializeLearnsetPickle(pt);
        File.WriteAllBytes(GetPath("pkhex", $"lvlmove_{suffix}.pkl"), BinLinkerWriter.Write16(learnsets, ident));
        var plus = SerializePlusPickle(pt);
        File.WriteAllBytes(GetPath("pkhex", $"plus_{suffix}.pkl"), BinLinkerWriter.Write16(plus, ident));
      //var egg = SerializeU16Pickle(pt, z => z.EggMoves);
      //File.WriteAllBytes(GetPath("pkhex", $"eggmove_{suffix}.pkl"), BinLinkerWriter.Write16(egg, ident));
      //var remind = SerializeU16Pickle(pt, z => z.ReminderMoves);
      //File.WriteAllBytes(GetPath("pkhex", $"reminder_{suffix}.pkl"), BinLinkerWriter.Write16(remind, ident));
        var evos = SerializeEvolutionPickle(pt);
        File.WriteAllBytes(GetPath("pkhex", $"evos_{suffix}.pkl"), BinLinkerWriter.Write16(evos, ident));

        // No need to dump the species remap table; unchanged from S/V.
        // List<(ushort Internal, ushort National)> map = [];
        // for (ushort i = 0; i <= (ushort)DevID.DEV_MATCHA2; i++)
        // {
        //     var pi = pt[i];
        //     var info = pi.FB.Info;
        //     map.Add((info.SpeciesInternal, info.SpeciesNational));
        // }
        // File.WriteAllText(GetPath("pkhex", "national dex.txt"), string.Join(Environment.NewLine, map.Select(z => $"{z.Internal},{z.National}")));

        foreach (var lang in LanguageEnglishNames)
            RipPersonal(pt, lang);
    }

    public void DumpDimension()
    {
        const string dimWild = "world/ik_data/field/dimension/dimension_wild_pokemon/dimension_wild_pokemon/dimension_wild_pokemon_array.bfbs";
        Dump<DimensionWildPokemonDBArray, DimensionWildPokemonDB>(dimWild, z => z.Table);
        DumpSel<DimensionWildPokemonDBArray, DimensionWildPokemon>(dimWild, z => z.Table.SelectMany(x => x.Table), "x");

        const string dimSpecialLottery = "world/ik_data/field/dimension/dimension_special_lottery/dimension_special_lottery/dimension_special_lottery_array.bfbs";
        Dump<DimensionSpecialLotteryDBArray, DimensionSpecialLotteryDB>(dimSpecialLottery, z => z.Table);

        const string dimItem = "world/ik_data/field/dimension/dimension_gimmick_item/dimension_gimmick_item/dimension_gimmick_item_array.bfbs";
        Dump<DimensionGimmickItemDBArray, DimensionGimmickItemDB>(dimItem, z => z.Table);

        const string dimUniqueLottery = "world/ik_data/field/dimension/dimension_unique_lottery/dimension_unique_lottery/dimension_unique_lottery_array.bfbs";
        Dump<DimensionUniqueLotteryDBArray, DimensionUniqueLotteryDB>(dimUniqueLottery, z => z.Table);

        const string dimZoneLottery = "world/ik_data/field/dimension/dimension_zone_lottery/dimension_zone_lottery/dimension_zone_lottery_array.bfbs";
        Dump<DimensionZoneLotteryDBArray, DimensionZoneLotteryDB>(dimZoneLottery, z => z.Table);

        const string dimBossLottery = "world/ik_data/field/dimension/dimension_boss_lottery/dimension_boss_lottery/dimension_boss_lottery_array.bfbs";
        Dump<DimensionBossLotteryDBArray, DimensionBossLotteryDB>(dimBossLottery, z => z.Table);

        const string dimGateSpawner = "world/ik_data/field/dimension/dimension_gate_spawner/dimension_gate_spawner/dimension_gate_spawner_array.bfbs";
        Dump<DimensionGateSpawnerDBArray, DimensionGateSpawnerDB>(dimGateSpawner, z => z.Table);

        const string dimGateArray = "world/ik_data/field/dimension/dimension_gate_object/dimension_gate_object/dimension_gate_object_array.bfbs";
        Dump<DimensionGateObjectDBArray, DimensionGateObjectDB>(dimGateArray, z => z.Table);

        const string dimProgress = "world/ik_data/field/dimension/dimension_progress/dimension_progress/dimension_progress_array.bfbs";
        Dump<DimensionProgressDBArray, DimensionProgressDB>(dimProgress, z => z.Table);

        const string dimMapArray = "world/ik_data/field/dimension/dimension_map/dimension_map/dimension_map_array.bfbs";
        Dump<DimensionMapDBArray, DimensionMapDB>(dimMapArray, z => z.Table);

        const string dimRankArray = "world/ik_data/field/dimension/dimension_rank/dimension_rank/dimension_rank_array.bfbs";
        Dump<DimensionRankDBArray, DimensionRankDB>(dimRankArray, z => z.Table);

        const string dimConfig = "world/ik_data/field/dimension/dimension_config/dimension_config/dimension_config.bfbs";
        DumpJson<DimensionConfigDB>(dimConfig);

        const string wazaConfig = "world/ik_data/field/wazagimmick_item_spawner/wazagimmick_item_spawner/wazagimmick_item_spawner.bfbs";
        Dump<WazagimmickItemSpawnerDB, WazagimmickItemSpawner>(wazaConfig, z => z.Table);

        const string itemTable = "world/ik_data/field/wazagimmick_item_table/wazagimmick_item_table/wazagimmick_item_table_array.bfbs";
        Dump<WazagimmickItemTableDBArray, WazagimmickItemTableDB>(itemTable, z => z.Table);

        DumpDimensionGates();
    }

    private void RipPersonal(PersonalTable9ZA pt, string lang)
    {
        var cfg = new TextConfig(rom.Game);
        string[] GetText(string name) => GetCommonText(name, lang, cfg);
        var specNames = GetText("monsname");
        var moveNames = GetText("wazaname");
        var abilNames = GetText("tokusei");
        var itemNames = GetText("itemname");
        var zukanA = GetText("zukan_comment_A");
        var zukanB = GetText("zukan_comment_B");
        var zukanAHTB = rom.GetPackedFile($"{message}/dat/{lang}/common/zukan_comment_A.tbl");
        var pd = new PersonalDumper9a
        {
            Species = specNames,
            Moves = moveNames,
            Abilities = abilNames,
            Items = itemNames,
            ZukanA = zukanA,
            ZukanB = zukanB,
            ZukanAHTB = new(zukanAHTB),

            Types = GetText("typename"),
            Colors = Enum.GetNames<PokeColor>(),
            EggGroups = Enum.GetNames<EggGroup>(),
            ExpGroups = Enum.GetNames<EXPGroup>(),
        };

        var lines = pd.Dump(pt);
        File.WriteAllLines(GetPath(lang, "personal.txt"), lines);

        var moveLines = pd.MoveSpeciesLearn
            .Select((z, i) => $"{i:000}\t{moveNames[i]}\t{string.Join(", ", z.Distinct())}");
        File.WriteAllLines(GetPath(lang, "MovePerPokemon.txt"), moveLines);

        var dexOrder = pt.Table
            .Where(z => z.FB.Dex != null && z.FB.IsPresentInGame)
            .OrderBy(z => z.FB.Dex!.Index)
            .GroupBy(z => z.FB.Info.SpeciesNational)
            .Select(z => z.First());

        var pc = dexOrder.Select(z => $"{z.FB.Dex!.Index:000}\t{specNames[z.FB.Info.SpeciesInternal]}");
        File.WriteAllText(GetPath(lang, "dex.txt"), string.Join(Environment.NewLine, pc));

        var foreign = pt.Table
            .Where(z => z.FB.Dex == null && z.IsPresentInGame)
            .GroupBy(z => z.FB.Info.SpeciesNational)
            .Select(z => z.First());

        var fl = foreign
            .Select(z => $"{z.FB.Info.SpeciesNational:000}\t{specNames[z.FB.Info.SpeciesInternal]}");
        File.WriteAllText(GetPath(lang, "foreign.txt"), string.Join(Environment.NewLine, fl));

        File.WriteAllLines(GetPath(lang, "species.txt"), SpeciesConverterZA.GetRearrangedAsNational(specNames));
        File.WriteAllLines(GetPath(lang, "abilities.txt"), abilNames);
        File.WriteAllLines(GetPath(lang, "items.txt"), itemNames);
        File.WriteAllLines(GetPath(lang, "moves.txt"), moveNames);
        File.WriteAllLines(GetPath(lang, "ZukanA.txt"), zukanA);
        File.WriteAllLines(GetPath(lang, "ZukanB.txt"), zukanB);

        var tms = PersonalDumper9a.TMIndexes;
        var tmNames = new string[tms.Length];
        for (int i = 0; i < tms.Length; i++)
            tmNames[i] = $"TM{i + 1:000}\t{moveNames[tms[i]]}";
        File.WriteAllText(GetPath(lang, "tm.txt"), string.Join(Environment.NewLine, tmNames));
    }

    private static byte[][] SerializeU16Pickle(PersonalTable9ZA pt, Func<PersonalInfo, IList<ushort>> sel)
    {
        var t = pt.Table;
        var result = new byte[t.Length][];
        for (int i = 0; i < t.Length; i++)
        {
            var p = t[i].FB;
            if (!p.IsPresentInGame)
                result[i] = [];
            else
                result[i] = Write(sel(p));
        }
        return result;

        static byte[] Write(IList<ushort> moves)
        {
            using var ms = new MemoryStream();
            using var bw = new BinaryWriter(ms);
            foreach (var m in moves.Order()) // just in case
                bw.Write(m);
            return ms.ToArray();
        }
    }

    private static byte[][] SerializeEvolutionPickle(PersonalTable9ZA pt)
    {
        var t = pt.Table;
        var result = new byte[t.Length][];
        for (int i = 0; i < t.Length; i++)
            result[i] = GetPickle(t[i]);
        return result;

        static byte[] GetPickle(PersonalInfo9ZA e)
        {
            if (!e.IsPresentInGame)
                return [];
            return Write(e.FB.Info.SpeciesNational, e.FB.Evolutions);
        }

        static byte[] Write(int species, IList<PersonalInfoEvolution> evos)
        {
            using var ms = new MemoryStream();
            using var bw = new BinaryWriter(ms);

            var list = evos;
            if (species is (int)Species.Crabrawler or (int)Species.Feebas)
            {
                var tmp = evos.ToArray();
                Array.Reverse(tmp); // put the levelup evo last.
                list = tmp;
            }

            foreach (var m in list) // just in case
            {
                var method = ((EvolutionType)m.Method);
                if (method == EvolutionType.Hisui)
                    continue; // skip: no evolution

                // ReSharper disable RedundantCast
                bw.Write((byte)method);
                bool levelRequired = method.IsLevelUpRequired();
                bw.Write(levelRequired);
                bw.Write(GetArg(method, m.Argument));
                bw.Write(SpeciesConverterZA.GetNational9(m.SpeciesInternal));
                bw.Write((byte)m.Form);
                bw.Write((byte)m.Level);
                // ReSharper restore RedundantCast

                static ushort GetArg(EvolutionType type, ushort arg)
                {
                    if (type.IsPlibUseItemType())
                        return Plib9.PlibToItem[arg];
                    return arg;
                }
            }
            return ms.ToArray();
        }
    }

    private static byte[][] SerializeLearnsetPickle(PersonalTable9ZA pt)
    {
        var t = pt.Table;
        var result = new byte[t.Length][];
        for (int i = 0; i < t.Length; i++)
        {
            var p = t[i];
            if (!p.IsPresentInGame)
            {
                result[i] = [];
                continue;
            }
            var learn = p.FB.Learnset;
            result[i] = Write(learn);
        }

        return result;

        static byte[] Write(IList<PersonalInfoMove> fbLearnset)
        {
            using var ms = new MemoryStream(4 * fbLearnset.Count);
            using var bw = new BinaryWriter(ms);
            foreach (var m in fbLearnset)
                bw.Write(m.Move);
            foreach (var m in fbLearnset)
            {
                var lvl = m.Level is (-3 or -2) ? 0 : m.Level; // Evo or Relearn
                bw.Write((byte)lvl);
            }

            return ms.ToArray();
        }
    }

    private static byte[][] SerializePlusPickle(PersonalTable9ZA pt)
    {
        var t = pt.Table;
        var result = new byte[t.Length][];
        for (int i = 0; i < t.Length; i++)
        {
            var p = t[i];
            if (!p.IsPresentInGame)
            {
                result[i] = [];
                continue;
            }
            var learn = p.FB.Learnset;
            result[i] = Write(learn);
        }
        return result;

        static byte[] Write(IList<PersonalInfoMove> fbLearnset)
        {
            using var ms = new MemoryStream(4 + (4 * fbLearnset.Count));
            using var bw = new BinaryWriter(ms);
            foreach (var m in fbLearnset)
                bw.Write(m.Move);
            foreach (var m in fbLearnset)
            {
                var lvl = m.LevelPlus;
                bw.Write((byte)lvl);
            }
            return ms.ToArray();
        }
    }

    public void DumpTrainers()
    {
        const string trd = "world/ik_data/trainer/trdata/trdata_array.bfbs";
        const string trt = "world/ik_data/trainer/trtype/trtype_array.bfbs";
        const string tre = "world/ik_data/trainer/trenv/trenv_array.bfbs";
        const string npc = "world/ik_data/field/npc/npc_asset_data/npc_asset_data/npc_asset_data.bfbs";
        const string btl = "ik_event/bin/event_trainer_battle_data/trainer_btl_data_global.bin";
        RipBFBS(trd);
        RipBFBS(trt);
        RipBFBS(tre);
        Dump<TrDataMainArray, TrDataMain>(trd, z => z.Table);
        Dump<TrainerEnvArray, TrainerEnv>(tre, z => z.Table);
        Dump<TrainerTypeArray, TrainerType>(trt, z => z.Table);
        DumpX<TrDataMainArray, TrDataMain, PokeDataBattle>(trd, z => z.Table, z => [z.Poke1, z.Poke2, z.Poke3, z.Poke4, z.Poke5, z.Poke6]);

        var trainers = Get<TrDataMainArray>(trd);
        var classes = Get<TrainerTypeArray>(trt);
        var npcs = Get<NpcAssetDataDBArray>(npc).Table;
        var trBtlData = Get<TrainerBattleGlobalDBArray>(btl).Table;
        List<(string TrainerBattleID1, string NPCAssetID1)> trBtls1 = trBtlData.Select(z => (z.TrainerBattleID1, z.NPCAssetID1)).ToList()!;
        List<(string TrainerBattleID2, string NPCAssetID2)> trBtls2 = trBtlData.Select(z => (z.TrainerBattleID2, z.NPCAssetID2)).ToList()!;

        // pretty
        List<string> result = [];
        var cfg = new TextConfig(GameVersion.ZA);
        const string lang = "English";
        var names = GetCommonText("monsname", lang, cfg);
        var items = GetCommonText("itemname", lang, cfg);
        var abilities = GetCommonText("tokusei", lang, cfg);
        var natures = GetCommonText("seikaku", lang, cfg);
        var moves = GetCommonText("wazaname", lang, cfg);
        var trNames = GetCommonText("trname", lang, cfg);
        var trTypes = GetCommonText("trtype", lang, cfg);
        var trTypesFlags = GetCommonTextFlags("trtype", lang, cfg);

        var personal = rom.GetPackedFile("avalon/data/personal_array.bin");
        var personalTable = Get<PersonalTable>(personal);

        var ahtbNames = new AHTB(rom.GetPackedFile($"ik_message/dat/{lang}/common/trname.tbl"));
        var ahtbTypes = new AHTB(rom.GetPackedFile($"ik_message/dat/{lang}/common/trtype.tbl"));

        foreach (var t in trainers.Table)
        {
            var team = new[] { t.Poke1, t.Poke2, t.Poke3, t.Poke4, t.Poke5, t.Poke6 };
            var trName = GetTrainerName(t.TrId);
            var trClass = ahtbTypes.GetString(t.TrType, trTypes);
            var trClassIndex = Array.IndexOf(trTypes, trClass);
            var trClassFlags = trTypesFlags[trClassIndex];
            var trNameFormatted = GetTrainerNameFormatted(trClass, trName, trClassFlags);
            result.Add("===");
            result.Add($"{trNameFormatted} ({t.TrId})");

            if (t.MoneyRate is not 0)
                result.Add($"Prize Money: ${GetPrizePayout(team, t.MoneyRate):N0}");

            if (t.ZARank is not (0 or ZARank.Infinite))
                result.Add($"Z-A Royale Rank: {(char)('Z' - ((int)t.ZARank - 1))}");
            else if (t.ZARank is ZARank.Infinite)
                result.Add("Z-A Royale Rank: Infinite");

            result.Add("===");

            foreach (var pk in team.Where(z => z.DevId is not DevID.DEV_NULL))
            {
                var name = names[(int)pk.DevId];
                var form = pk.FormId is 0 ? string.Empty : $"-{pk.FormId}";
                var gender = pk.Sex is 0 ? string.Empty : pk.Sex is SexType.MALE ? " (♂)" : " (♀)";

                var level = $" (Lv. {pk.Level})";
                var alpha = pk.IsOybn ? " (Alpha)" : string.Empty;
                var item = pk.Item is 0 ? string.Empty : $" (Held Item: {items[(int)pk.Item]})";
                var rare = pk.RareType is RareType.RARE ? " ★" : string.Empty;
                var move1 = PrettyDumpUtil.GetMoveText(pk.Waza1, moves);
                var move2 = PrettyDumpUtil.GetMoveText(pk.Waza2, moves);
                var move3 = PrettyDumpUtil.GetMoveText(pk.Waza3, moves);
                var move4 = PrettyDumpUtil.GetMoveText(pk.Waza4, moves);

                // standard level up
                if (pk.Waza1.WazaId is 0)
                {
                    var empty = PrettyDumpUtil.GetCurrentMoves(pk.DevId, pk.FormId, (byte)pk.Level, personalTable);
                    move1 = moves[empty[0]];
                    move2 = moves[empty[1]];
                    move3 = moves[empty[2]];
                    move4 = moves[empty[3]];
                }

                var moveset = new[] { move1, move2, move3, move4 };
                var moveOutput = $" (Moves: {string.Join(" / ", moveset.Where(z => z != moves[0] && !string.IsNullOrWhiteSpace(z)))})";
                var abilName = PrettyDumpUtil.GetAbilityName(pk.DevId, pk.FormId, abilities, personalTable, pk.Tokusei);
                var ability = pk.Tokusei is TokuseiType.RANDOM_12 or TokuseiType.RANDOM_123 ? string.Empty : $" (Ability: {abilName[..(abilName.IndexOf('(') - 1)]})";
                var nature = pk.Seikaku is 0 ? string.Empty : $" (Nature: {natures[(int)pk.Seikaku - 1]})";
                var ball = $" (Ball: {(Ball)pk.BallId})";

                var ivs = pk.TalentValue;
                var textIVs = $" (IVs: {ivs.HP}/{ivs.ATK}/{ivs.DEF}/{ivs.SPA}/{ivs.SPD}/{ivs.SPE})";

                var eff = pk.EffortValue;
                ReadOnlySpan<int> evs = [eff.HP, eff.ATK, eff.DEF, eff.SPA, eff.SPD, eff.SPE];
                var textEVs = evs.ContainsAnyExcept(0) ? $" (EVs: {PrettyDumpUtil.GetEVSpread(evs)})" : string.Empty;
                var line = $"- {name}{form}{gender}{rare}{level}{alpha}{item}{ability}{nature}{ball}{moveOutput}{textIVs}{textEVs}";
                result.Add(line);
            }

            result.Add("");
        }

        File.WriteAllLines(GetPath("pretty", "trainers_pretty.txt"), result);

        static int GetPrizePayout(PokeDataBattle[] team, int pay)
        {
            var target = team.MaxBy(z => z.Level)!.Level;
            return target * pay * 4;
        }

        static string GetTrainerNameFormatted(string type, string name, ushort flags)
        {
            bool article = ((flags >> 9) & 1) == 0;
            var format = ((flags >> 7) & 0b11);
            var display = format switch
            {
                0 when type is "Rust Syndicate" && name is "Corbeau" or "Philippe" => "{1} of the {0}", // no clue how they differentiate these two from Grunts, hardcode it
                0 => "{0} {1}",
                1 when article => "{1} of the {0}",
                1 => "{1} of {0}",
                2 => "{1} the {0}",
                _ => throw new ArgumentOutOfRangeException(nameof(format), format, null),
            };

            // edge case for the longest trainer name in history
            if (name is "representative")
                display = display.Insert(0, "The ");

            // no unique name
            if (type is "Alpha Trainer" or "Hyperspace Trainer")
                return type;

            return string.Format(display, type, name);
        }

        string GetTrainerName(string id)
        {
            // Z-A Royale Trainers
            if (id.StartsWith("za_rank"))
            {
                id = id.Insert(2, "_trainer_asset");
                var check = npcs.FirstOrDefault(z => z.AssetId == id);
                if (check is null)
                    return "NO TEXT FOUND";
                return ahtbNames.GetString(FnvHash.HashFnv1a_64(check.NpcInfoList[0].Name), trNames);
            }

            if (id.EndsWith("kusa") || id.EndsWith("hono") || id.EndsWith("mizu"))
            {
                var rivalF = ahtbNames.GetString(FnvHash.HashFnv1a_64("rival_01"), trNames); // Taunie
                var rivalM = ahtbNames.GetString(FnvHash.HashFnv1a_64("rival_02"), trNames); // Urbain
                return $"{rivalF}/{rivalM}";
            }

            // no unique name
            if (id.StartsWith("dim_rank_"))
                return string.Empty;

            var (trainerBattleId1, npcAssetId1) = trBtls1.FirstOrDefault(z => z.TrainerBattleID1 == id);
            var (trainerBattleId2, npcAssetId2) = trBtls2.FirstOrDefault(z => z.TrainerBattleID2 == id);
            var btlToCheck = trainerBattleId2 is null ? trainerBattleId1 : trainerBattleId2;
            var npcToCheck = npcAssetId2 is null ? npcAssetId1 : npcAssetId2;

            if (btlToCheck is null || id is "Ev_sub_123_010" || id.Contains("Ev_sub_124"))
            {
                // not in trname
                if (id is "Ev_sub_145_010_multi")
                    return "Marcia";

                var fallback = GetFallbackTrainerName(id);
                return ahtbNames.GetString(FnvHash.HashFnv1a_64(fallback), trNames);
            }

            // special name format
            if (npcToCheck.Contains("rest_trainer"))
            {
                var restName = ahtbNames.GetString(FnvHash.HashFnv1a_64(npcToCheck.Replace("_trainer", "")), trNames);
                return restName;
            }

            if (npcToCheck.StartsWith("2on2_"))
                npcToCheck = npcToCheck[5..];

            var target = npcs.FirstOrDefault(z => z.AssetId == npcToCheck)!;

            if (target is null)
            {
                var startQuest = npcToCheck.Length is 15 ? 8 : 7;
                var startSub = npcToCheck.Length is 15 ? 12 : 11;
                var questID = int.Parse(npcToCheck[startQuest..(startQuest + 3)]);
                var subID = int.Parse(npcToCheck[startSub..(startSub + 3)]);

                // this is off-by-two for some reason?
                if (questID == 180)
                    subID += 2;

                target = npcs.FirstOrDefault(z => z.AssetId == $"npc_sub{questID}_{subID:0000}")!;
            }

            var name = target.NpcInfoList.Where(z => !string.IsNullOrWhiteSpace(z.Name)).ToArray()[0].Name;
            return ahtbNames.GetString(FnvHash.HashFnv1a_64(name), trNames);
        }

        string GetFallbackTrainerName(string id) => id switch
        {
            "00_test_data" => "TR_NONE",
            "Ev_m04_boss_0071_multi" or "Ev_m05_boss_0354_multi" or "Ev_m06_boss_0181_multi" or "Ev_m07_boss_0478_multi" or "Ev_m08_boss_0248_multi" or "Ev_m09_1135_multi_02" => "friend_01", // Naveen
            "Ev_m04_boss_0323_multi" or "Ev_m05_boss_0701_multi" or "Ev_m06_boss_0689_multi" or "Ev_m07_boss_0334_multi" or "Ev_m08_boss_0121_multi" or "Ev_m09_1135_multi_01" => "friend_02", // Lida
            "Ev_m06_multi" or "Ev_m09_2300_multi_01" => "boss03", // Corbeau
            "Ev_m09_2000_multi_01" => "boss02", // Ivor
            "Ev_m09_2000_multi_02" => "executive02", // Gwynn
            "Ev_m09_2100_multi_01" => "boss01", // Canari
            "Ev_m09_2200_multi_01" => "executive04", // Lebanne
            "Ev_m09_2400_multi_01" => "boss05", // Grisham
            "Ev_m09_2400_multi_02" => "executive05", // Griselle
            "Ev_sub_103_multi" => "alias01", // Andi
            "Ev_sub_114_multi" => "battleg", // Josée

            // DLC
            "Ev_d02_0010" or "Ev_sub_161_010_multi" => "friend_01", // Naveen
            "Ev_d00_2100_multi" or "Ev_d04_1100" or "Ev_sub_164_010_multi" => "friend_02", // Lida
            "Ev_d01_3100" or "Ev_d02_2200" or "Ev_d03_1300" or "Ev_d04_2100" or "Ev_d05_1200" or "Ev_d06_1020" or "Ev_d07_1000" or "Ev_d07_1100" or "Ev_d07_2000" or "Ev_sub_162_010_multi" or "Ev_sub_203_010_multi_02" => "xyleader3_01", // Korrina
            "Ev_d05_0100" => "boss03", // Corbeau
          //"Ev_sub_145_010_multi" => "sub_145", // Marcia
            "Ev_sub_148_020_client" => "sub_148", // Dixan
            "Ev_sub_165_010_multi" or "Ev_sub_203_010_multi_01" => "detective", // Emma
            "Ev_sub_166_010_multi" => "executive02", // Gwynn
            "Ev_sub_167_010_multi" => "executive04", // Lebanne
            "Ev_sub_168_010_multi" => "secretary", // Vinnie
            "Ev_sub_169_010_multi" => "boss04", // Jacinthe
            "Ev_sub_170_010_multi" => "executive03", // Philippe

            // Some DLC Side Missions with NPCs from past Side Missions are, annoyingly, not mapped as such in the NPC db array
            "Ev_sub_123_010" => "sub_002", // Trevelle
            _ when id.Contains("megax") => "sub_001", // Tracie
            _ when id.Contains("megay") => "sub_072", // Griddella

            _ => throw new ArgumentOutOfRangeException(nameof(id), id, null),
        };
    }

    public void DumpAbilities()
    {
        const string tokuseiPath = "avalon/data/tokusei_array.bin";
        Dump<TokuseiTable, Tokusei>(tokuseiPath, z => z.Table);
    }

    public void DumpEncounters()
    {
        const string language = "English";

        var cfg = new TextConfig(GameVersion.ZA);
        var ahtb = GetCommonAHTB("place_name", language);
        var place_names = GetCommonText("place_name", language, cfg);
        var nameDict = ahtb.ToIndexedDictionary(place_names);

        var config = new EncounterDumpConfig9a
        {
            PlaceNameMap = nameDict,
            SpecNamesInternal = GetCommonText("monsname", language, cfg),
            MoveNames = GetCommonText("wazaname", language, cfg),
            Path = GetPath("encounters"),
        };
        EncounterDumper9a.Dump(rom, config);
    }

    public void DumpMegaInfo()
    {
        const string mega = "world/exl/pokemon/megaevo_data/megaevo_data.bin";
        var table = Dump<MegaEvoArray, MegaEvo>(mega, z => z.Table);

        // pretty
        var cfg = new TextConfig(GameVersion.ZA);
        const string lang = "English";
        var names = GetCommonText("monsname", lang, cfg);
        var items = GetCommonText("itemname", lang, cfg);

        // pretty

        var result = table.Table.Select(z =>
            $"{names[(int)z.Species]}-{z.FromForm} => {z.ToForm} @ {items[(int)z.Item]} ({z.Type} & {z.Short})");
        var path = GetPath("pkhex", "mega.txt");
        File.WriteAllLines(path, result);
    }

    public void DumpLocationNames()
    {
        foreach (var lang in LanguageEnglishNames)
            RipLocationNames(lang);
    }

    // Language Set => Language codes for PKHeX
    private static string GetLanguageCode(string lang) => lang switch
    {
        "JPN" => "ja",
        "JPN_KANJI" => "ja",
        "English" => "en",
        "French" => "fr",
        "Italian" => "it",
        "German" => "de",
        "Spanish" => "es",
        "Korean" => "ko",
        "Simp_Chinese" => "zh-Hans",
        "Trad_Chinese" => "zh-Hant",
        "LATAM" => "es-419",
        _ => throw new ArgumentOutOfRangeException(nameof(lang), lang, null),
    };

    private void RipLocationNames(string lang)
    {
        const string g = "za";
        var cfg = new TextConfig(rom.Game);
        string GetText(string name) => string.Join('\n', GetCommonText(name, lang, cfg));
        var code = GetLanguageCode(lang);
        File.WriteAllText(GetPath("pkhex", "locations", $"text_{g}_00000_{code}.txt"), GetText("place_name"));
        File.WriteAllText(GetPath("pkhex", "locations", $"text_{g}_30000_{code}.txt"), "\n" + GetText("place_name_spe"));
        File.WriteAllText(GetPath("pkhex", "locations", $"text_{g}_40000_{code}.txt"), "\n" + GetText("place_name_out"));
        File.WriteAllText(GetPath("pkhex", "locations", $"text_{g}_60000_{code}.txt"), "\n" + GetText("place_name_per"));
    }

    public void ScanPatterns()
    {
        var root = GetPath("archive", "extracted");
        var files = Directory.EnumerateFiles(root, "*.*", SearchOption.AllDirectories);

        (string Pattern, bool Include)[] patterns =
        [
            ("SYS_", true), // system work
            ("flag_", true), // system flag, event flag
            ("DBG_", true), // debug flag, debug work
            ("TEMP_", true),
            ("momiji", true),
            ("MOMIJI", true),
            ("Momiji", true),
            ("FLAG", true),
            ("Flag", true),
            ("QUEST_", true),
            ("debug_", true),
            ("DEBUG", true),
            ("sys_", true),
            ("dbg_", true),
            ("spawner_", true),
            ("random_", true),
            ("sub_", true),
            ("main_", true),
            ("event_", true),
            ("play", true),
            ("report", true),
            ("itb_", true), // item ball
            ("spn_", true), // spawn
            ("traffic", true),
            ("rnd_", true),
            ("TITLE_", true),
            ("itd_", true),
            ("donut", true),
            ("dim_", true),

            ("rso_", false), // royale card spawn
            ("t1_", false),
            (".bfbs", false),
            ("_condition", false),
        ];

        const string outRoot = "pattern";

        var results = StringSearch.SearchFiles(files, patterns.Select(z => z.Pattern).ToArray());
        var outPath = GetPath(outRoot);
        if (!Directory.Exists(outPath))
            Directory.CreateDirectory(outPath);

        var saveHashHits = new HashSet<string>();
        for (var i = 0; i < results.Length; i++)
        {
            var result = results[i];
            var (pattern, include) = patterns[i];
            var hits = result.List;

            var path = Path.Combine(outPath, $"{pattern}.txt");
            using var fs = new FileStream(path, FileMode.Create, FileAccess.Write);
            using var sw = new StreamWriter(fs);
            foreach (var hit in hits)
            {
                sw.WriteLine(hit);
                if (include)
                    saveHashHits.Add(hit.Result);
            }
        }
        CreateSaveHashDump(saveHashHits, outPath);

        System.Media.SystemSounds.Asterisk.Play();
    }

    private void CreateSaveHashDump(IEnumerable<string> saveHashHits, string outPath)
    {
        string[] extra =
        [
            ..FromFile<EventLabelArray>("ik_event/bin/flag/system_flag.bin").Table.Select(z => z.Name),
            ..FromFile<EventLabelArray>("ik_event/bin/flag/event_flag.bin").Table.Select(z => z.Name),
            ..FromFile<EventLabelArray>("ik_event/bin/flag/system_work.bin").Table.Select(z => z.Name),
            ..FromFile<EventLabelArray>("ik_event/bin/flag/quest_work.bin").Table.Select(z => z.Name),
            ..FromFile<EventLabelArray>("ik_event/bin/flag/momiji_work.bin").Table.Select(z => z.Name),
            ..FromFile<EventLabelArray>("ik_event/bin/flag/temp_flag.bin").Table.Select(z => z.Name),
            ..FromFile<EventLabelArray>("ik_event/bin/flag/temp_work.bin").Table.Select(z => z.Name),

            ..FromFile<TitleArray>("ik_event/bin/title/title.bin").Table.Select(z => z.Name),

            // works to grab first field of object as string, even though this isn't the right schema to parse as
            ..FromFile<EventLabelArray>("ik_event/bin/momiji_count/momiji_count.bin").Table.Select(z => z.Name),
        ];

        // Gotta hash em all!
        var all = saveHashHits.Concat(extra)
            .Where(z => IsSaveBlockLabel(z))
            .Distinct()
            .Order();

        var hashed = Path.Combine(outPath, "SAV9ZA_flagwork.txt");
        var text = all.Select(z => $"{FnvHash.HashFnv1a_64(z):X16}\t{z}");
        File.WriteAllLines(hashed, text);
        return;

        T FromFile<T>(string path) where T : class, IFlatBufferSerializable<T> => DumpJson<T>(path);
    }

    private static bool IsSaveBlockLabel(ReadOnlySpan<char> input)
    {
        foreach (char c in input)
        {
            // Check if character is A-Z, a-z, 0-9, or _
            if (c is ((>= 'A' and <= 'Z') or (>= 'a' and <= 'z') or (>= '0' and <= '9') or '_' or '-'))
                continue;
            return false;
        }
        return true; // All characters are valid
    }

    public void DumpItems()
    {
        const string language = "English";

        const string path = "world/exl/item_data/item_data/item_data.bin";
        Dump<ItemDataArray, ItemData>(path, z => z.Table);
        var table = Get<ItemDataArray>(path);
        var outPath = GetPath("pkhex");
        var cfg = new TextConfig(rom.Game);
        var itemNames = GetCommonText("itemname", language, cfg);
        var moveNames = GetCommonText("wazaname", language, cfg);
        var labeled = TableUtil.GetNamedTable(table.Table, itemNames, z => z.Id, "Item");
        File.WriteAllText(Path.Combine(outPath, "item_table.txt"), labeled);

        var groups = table.Table.GroupBy(z => z.Pocket);
        var sb = new StringBuilder();
        foreach (var group in groups)
        {
            // Dump in sets of 10.
            int i = 0;
            foreach (var item in group)
            {
                sb.Append($"{item.Id:0000}, ");
                if (++i % 10 != 0)
                    continue;
                sb.AppendLine();
                i = 0;
            }
            File.WriteAllText(Path.Combine(outPath, $"pouch_{group.Key}.txt"), sb.ToString());
            sb.Clear();
        }

        // TM-BitFlag Indexes
        var tms = table.Table.Where(z => z.MachineIndex >= 0)
            .OrderBy(z => z.MachineIndex).ToArray();
        const string prefix = "000, "; // TM001 is the first TM, and indexes start at 0.
        var tmString = prefix + string.Join(", ", tms.Select(z => $"{(int)z.MachineWaza:000}"));
        File.WriteAllText(Path.Combine(outPath, "tm.txt"), tmString);

        // TM-Actual Move IDs
        var tmByName = tms.Select(z => (Item: z, Name: itemNames[z.Id]))
                .OrderBy(z => z.Name)
                .Select(z => $"{z.Name}\t{z.Item.MachineIndex+1:000}\t{(int)z.Item.MachineWaza:000}\t{moveNames[(int)z.Item.MachineWaza]}")
            ;
        File.WriteAllLines(Path.Combine(outPath, "tm_by_name.txt"), tmByName);

        var tmDisplay = tms
            .Select(z => (int)z.MachineWaza);
        var asDisplay = "000, " + string.Concat(tmDisplay.Select((z, i) => $"{z:000}, {(i != 0 && i % 10 == 9 ? Environment.NewLine : "")}"));
        File.WriteAllText(Path.Combine(outPath, "tm_display.txt"), asDisplay);

        var remap = GetRemap(tms);
        var asRemap = string.Concat(remap.Select((z, i) => $"{z}, {(i != 0 && i % 10 == 9 ? Environment.NewLine : "")}"));
        File.WriteAllText(Path.Combine(outPath, "tm_remap.txt"), asRemap);
        static string[] GetRemap(IReadOnlyList<ItemData> items)
        {
            // Gets a bias value to convert an old-gen TM index text to the new-gen TM index.
            // Relies on the TM's internal name (consistent with old games).

            static int GetOldIndex(ItemData item) => int.Parse(item.InternalName.AsSpan("WAZAMASIN".Length));
            var maxIndex = items.Max(GetOldIndex) + 1;
            var result = new string[maxIndex];
            result.AsSpan().Fill("  0");
            foreach (var item in items)
            {
                var shift = GetRemap(item, out var oldIndex);
                result[oldIndex] = shift;
            }
            return result;

            static string GetRemap(ItemData item, out int oldIndex)
            {
                oldIndex = GetOldIndex(item);
                var newIndex = item.MachineIndex + 1;

                var delta = newIndex - oldIndex;
                // return a 3-char string
                return GetFormatted(delta);
            }

            static string GetFormatted(int delta)
            {
                if (delta is 0)
                    return "  0";
                return delta < 0 ? $"{delta:00}" : $"+{delta:00}";
            }
        }
    }

    public void DumpConfig()
    {
        const string playerDamage = "world/ik_data/battle/battle_player_damage/battle_player_damage.bfbs";
        DumpJson<PlayerDamage>(playerDamage);
        DumpSel<PlayerDamage, WazaDamage>(playerDamage, z => z.Move, "move");
        DumpSel<PlayerDamage, PokemonDamageRatio>(playerDamage, z => z.PokemonRatio, "poke");
        DumpSel<PlayerDamage, MegaPokemonRatio>(playerDamage, z => z.MegaPokemon, "mega");

        DumpJson<FieldPlayer>("world/ik_data/field/playables/field_player/data.bfbs");

        DumpJson<LockonDesc>("world/ik_data/battle/lockon_desc/player.bfbs");
        DumpJson<BattleSetting>("world/ik_data/battle/battle_setting/battle_setting.bfbs");
        DumpJson<OyabunSetting>("world/ik_data/field/oyabun/oyabun_setting/oyabun_setting.bfbs");
        DumpJson<FieldConfig>("world/ik_data/field/config/field_config/field_config_data.bfbs");
        DumpJson<AreaConfig>("world/ik_data/field/area/area_config/area_config_data.bfbs");

        Dump<FieldDataConfigArray, FieldDataConfig>("world/ik_data/field/data/field_data_config/field_data_config_array.bfbs", z => z.Table);
        DumpSel<FieldDataConfigArray, FieldDataInfo>("world/ik_data/field/data/field_data_config/field_data_config_array.bfbs", z => z.Table.Select(x => x.Info));
    }

    public void DumpPokemonData()
    {
        const string pmd = "world/ik_data/field/pokemon/pokemon_data/pokemon_data/pokemon_data_array.bfbs";
        var pokemon = Dump<PokemonDataDBArray, PokemonDataDB>(pmd, z => z.Table);

        DumpPokemonPretty(pokemon);
        DumpPokemonPKHeX(pokemon);
    }

    private void DumpPokemonPretty(PokemonDataDBArray pokemon)
    {
        var cfg = new TextConfig(GameVersion.ZA);
        const string lang = "English";
        List<string> result = [];

        var names = GetCommonText("monsname", lang, cfg);
        var forms = GetCommonText("zkn_form", lang, cfg);
        var abilities = GetCommonText("tokusei", lang, cfg);
        var natures = GetCommonText("seikaku", lang, cfg);
        var moves = GetCommonText("wazaname", lang, cfg);
        var items = GetCommonText("itemname", lang, cfg);

        var ahtbForms = rom.GetPackedFile($"ik_message/dat/{lang}/common/zkn_form.tbl");
        var personal = rom.GetPackedFile("avalon/data/personal_array.bin");
        var personalTable = Get<PersonalTable>(personal);

        var helper = new DumpHelper9a
        {
            SpeciesNames = names,
            AbilityNames = abilities,
            MoveNames = moves,
            NatureNames = natures,
            FormNames = forms,
            Personal = personalTable,
            Forms = new(ahtbForms),
        };

        int index = 0;
        foreach (var enc in pokemon.Table)
        {
            result.Add($"Table: {index++}");
            foreach (var t in enc.Table)
            {
                result.Add("===");
                result.Add($"{t.Id}");
                result.Add("===");

                var name = names[(int)t.DevNo];
                var form = helper.GetFormName((ushort)t.DevNo, (short)t.FormNo);
                var formDisplay = PrettyDumpUtil.GetFormDisplay((short)t.FormNo, form);
                result.Add($"{name}{formDisplay}");

                var fixedLevel = t.MinLevel == t.MaxLevel;
                if (fixedLevel)
                {
                    result.Add($"Level: {t.MinLevel}");
                }
                else
                {
                    result.Add($"Min Level: {t.MinLevel}");
                    result.Add($"Max Level: {t.MaxLevel}");
                }

                if (t.Sex is not (-1 or 2))
                    result.Add($"Gender: {PrettyDumpUtil.GetGender(t.Sex)}");

                result.Add($"Shiny: {PrettyDumpUtil.GetShiny(t.Rare)}");

                if ((sbyte)t.Tokusei is not -1)
                    result.Add($"Ability: {PrettyDumpUtil.GetAbilityName(t.DevNo, (short)t.FormNo, abilities, personalTable, (TokuseiType)t.Tokusei)}");

                result.Add($"Nature: {PrettyDumpUtil.GetNature(natures, t.Seikaku)}");

                if (t.TalentScale is not -1)
                    result.Add($"Scale: {t.TalentScale}");

                var ivs = t.TalentValue;
                if (ivs is { HP: -1, ATK: -1, DEF: -1, SPA: -1, SPD: -1, SPE: -1 })
                    result.Add(t.TalentVNum < 1 ? "IVs: Random" : $"IVs: {t.TalentVNum} Flawless");
                else
                    result.Add($"IVs: {ivs.HP}/{ivs.ATK}/{ivs.DEF}/{ivs.SPA}/{ivs.SPD}/{ivs.SPE}");

                if (t.OyabunProbability is not 0)
                    result.Add($"Alpha Probability: {t.OyabunProbability}%");

                if (t.OyabunAdditionalLevel is not 0)
                    result.Add($"Alpha Additional Level: +{t.OyabunAdditionalLevel}");

                if (t.Item is { } item)
                    result.Add($"Held Item: {items[item.ItemId]}");

                // not a generic wild zone encounter
                if (fixedLevel && t.OyabunAdditionalLevel is 0 && t.Moves is { } wl && (int)wl.Waza1 is not (0 or 65535))
                {
                    result.Add("Moves:");
                    result.Add($"- {moves[(int)wl.Waza1]}");
                    if ((int)wl.Waza2 is not (0 or 65535)) result.Add($"- {moves[(int)wl.Waza2]}");
                    if ((int)wl.Waza3 is not (0 or 65535)) result.Add($"- {moves[(int)wl.Waza3]}");
                    if ((int)wl.Waza4 is not (0 or 65535)) result.Add($"- {moves[(int)wl.Waza4]}");
                }

                if (t.ActivationConditionArray is { } a)
                {
                    foreach (var cond in a)
                    {
                        if (cond.Element is not { Count: not 0 } elemList)
                            continue; // never true

                        foreach (var elem in elemList)
                        {
                            if (elem.Param is not { Count: not 0 } pl)
                                continue; // never true

                            for (var i = 0; i < pl.Count; i++)
                            {
                                var param = pl[i];
                                var set = param.Param is null ? "NO PARAM" : string.Join(", ", param.Param);
                                result.Add($"Param[{i}] {param.Condition} - {param.Op} - {set}");
                            }
                        }
                    }
                }

                result.Add("");
            }
        }

        File.WriteAllLines(GetPath("pretty", "pokemon_data_pretty.txt"), result);
    }

    private void DumpPokemonPKHeX(PokemonDataDBArray pokemon)
    {
        var cfg = new TextConfig(GameVersion.ZA);
        const string lang = "English";
        List<string> result = [];

        var names = GetCommonText("monsname", lang, cfg);
        var natures = GetCommonText("seikaku", lang, cfg);
        ReadOnlySpan<string> Trades = ["sub_addpoke_araichu", "sub_tradepoke_riolu", "sub_addpoke_gyadon", "sub_tradepoke_heracros", "sub_tradepoke_poligon2"];

        foreach (var enc in pokemon.Table)
        {
            foreach (var t in enc.Table)
            {
                var name = names[(int)t.DevNo];
                var national = SpeciesConverterZA.GetNational9((ushort)t.DevNo);
                var formDisplay = t.FormNo is 0 ? string.Empty : $"-{t.FormNo}";
                var shiny = t.Rare switch
                {
                    0x3FFFFFFF => ", Shiny = Random",
                    0x2FFFFFFF => ", Shiny = Always",
                    _ => string.Empty,
                };
                var gender = t.Sex is -1 or 2 ? string.Empty : $", Gender = {t.Sex}";
                var nature = t.Seikaku is -1 ? string.Empty : $", Nature = Nature.{natures[t.Seikaku]}";

                var ivs = t.TalentValue;
                string iv = ivs switch
                {
                    { HP: 31, ATK: 31, DEF: 31, SPA: 31, SPD: 31, SPE: 31 } => ", FlawlessIVCount = 6",
                    { HP: -1, ATK: -1, DEF: -1, SPA: -1, SPD: -1, SPE: -1 }
                        => t.TalentVNum is 0 ? string.Empty : $", FlawlessIVCount = {t.TalentVNum}",
                    _ => $", IVs = new({ivs.HP:00},{ivs.ATK:00},{ivs.DEF:00},{ivs.SPE:00},{ivs.SPA:00},{ivs.SPD:00})",
                };

                var moves = "";
                if (t.OyabunAdditionalLevel is 0 && t.Moves is { } m && (int)m.Waza1 is not (0 or 65535))
                {
                    Span<int> moveset = [(int)m.Waza1, (int)m.Waza2, (int)m.Waza3, (int)m.Waza4];
                    moveset.Replace(65535, 0);
                    moves = $", Moves = new({moveset[0]:000},{moveset[1]:000},{moveset[2]:000},{moveset[3]:000})";
                }

                var alpha = t.OyabunProbability is 0 ? string.Empty : ", IsAlpha = true";
                var level = t.OyabunProbability is 0 ? t.MinLevel : (t.MinLevel + t.OyabunAdditionalLevel);
                var scale = t.TalentScale is -1 ? 0 : t.TalentScale;
                var line = $"        new({national:0000},{t.FormNo},{level:00},{scale:000}) {{ Location = 000{shiny}{gender}{nature}{iv}{moves}{alpha} }}, // {name}{formDisplay} ({t.Id})";

                // annoying that these are lumped together with gift encounters
                if (Trades.Contains(t.Id))
                {
                    var index = Trades.IndexOf(t.Id);
                    line = $"        new(TradeNames,{index},{national:0000},{t.FormNo},{level:00}) {{ ID32 = 000000, OTGender = 0{gender}{nature}{iv}{moves}{alpha} }}, // {name}{formDisplay} ({t.Id})";
                }

                result.Add(line);

                // is not fixed alpha, but can still be alpha OR non-alpha
                if (t.OyabunProbability is >= 1 and <= 99)
                {
                    line = line.Remove(19, 2).Insert(19, $"{t.MinLevel:00}").Replace(", IsAlpha = true", "");
                    result.Add(line);
                }
            }
        }

        File.WriteAllLines(GetPath("pkhex", "pokemon_data_pkhex.txt"), result);
        DumpTradeNames();
    }

    public void DumpEncountData()
    {
        const string emd = "world/ik_data/field/pokemon/encount_data/encount_data/encount_data_array.bfbs";
        const string dro = "world/ik_data/field/pokemon/pokemon_drop_item_table/pokemon_drop_item_table_data/pokemon_drop_item_table_data_array.bfbs";
        var pokemon = Dump<EncountDataDBArray, EncountDataDB>(emd, z => z.Table);
        DumpSel<EncountDataDBArray, EncountData>(emd, z => z.Table.SelectMany(x => x.Table));
        var drops = Dump<PokemonDropItemTableDataDBArray, PokemonDropItemTableDataDB>(dro, z => z.Table);

        for (var i = 0; i < pokemon.Table.Count; i++)
        {
            var table = pokemon.Table[i];
            var lines = TableUtil.GetTable(table.Table);
            var path = GetRawPath(emd);
            var fileName = Path.ChangeExtension(path, $".{i:00}.txt");
            File.WriteAllText(fileName, lines);
        }

        const string psp = "world/ik_data/field/pokemon_spawner/pokemon_spawner_data/pokemon_spawner_data_array.bfbs";
        var spawners = Dump<PokemonSpawnerDataDBArray, PokemonSpawnerDataDB>(psp, z => z.Table);
        for (var i = 0; i < spawners.Table.Count; i++)
        {
            var table = spawners.Table[i];
            var lines = TableUtil.GetTable(table.Table);
            var path = GetRawPath(psp);
            var fileName = Path.ChangeExtension(path, $".{i:00}.txt");
            File.WriteAllText(fileName, lines);
        }

        const string pst = "world/ik_data/field/spawner_transform_data/pokemon_spawner_transform/pokemon_spawner_transform/pokemon_spawner_transform_array.bfbs";
        var spawnerT = Dump<SpawnerTransformDataDBArray, SpawnerTransformDataDB>(pst, z => z.Table);
        for (var i = 0; i < spawnerT.Table.Count; i++)
        {
            var table = spawnerT.Table[i];
            var lines = TableUtil.GetTable(table.Table);
            var path = GetRawPath(pst);
            var fileName = Path.ChangeExtension(path, $".{i:00}.txt");
            File.WriteAllText(fileName, lines);
        }

        DumpEncountPretty(pokemon, drops);
        DumpEncountPKHeX(pokemon);

        Dump<FieldMainAreaArray, FieldMainArea>("world/ik_data/field/area/field_main_area/field_main_area_array.bin", z => z.Table);
        DumpSel<FieldMainAreaArray, FieldAreaInfo>("world/ik_data/field/area/field_main_area/field_main_area_array.bin", z => z.Table.Select(x => x.AreaInfo));

        Dump<FieldSubAreaArray, FieldSubArea>("world/ik_data/field/area/field_sub_area/field_sub_area_array.bin", z => z.Table);
        DumpSel<FieldSubAreaArray, FieldAreaInfo>("world/ik_data/field/area/field_sub_area/field_sub_area_array.bin", z => z.Table.Select(x => x.AreaInfo));

        Dump<FieldBattleZoneArray, FieldBattleZone>("world/ik_data/field/area/field_battle_zone/field_battle_zone_array.bin", z => z.Table);
        DumpSel<FieldBattleZoneArray, FieldAreaInfo>("world/ik_data/field/area/field_battle_zone/field_battle_zone_array.bin", z => z.Table.Select(x => x.AreaInfo));

        Dump<FieldWildZoneArray, FieldWildZone>("world/ik_data/field/area/field_wild_zone/field_wild_zone_array.bin", z => z.Table);
        DumpSel<FieldWildZoneArray, FieldAreaInfo>("world/ik_data/field/area/field_wild_zone/field_wild_zone_array.bin", z => z.Table.Select(x => x.AreaInfo));

        Dump<FieldSceneAreaArray, FieldSceneArea>("world/ik_data/field/area/field_scene_area/field_scene_area_array.bin", z => z.Table);
        DumpSel<FieldSceneAreaArray, FieldAreaInfo>("world/ik_data/field/area/field_scene_area/field_scene_area_array.bin", z => z.Table.Select(x => x.AreaInfo));

        Dump<FieldLocationArray, FieldLocation>("world/ik_data/field/area/field_location/field_location_array.bin", z => z.Table);
        DumpSel<FieldLocationArray, FieldAreaInfo>("world/ik_data/field/area/field_location/field_location_array.bin", z => z.Table.Select(x => x.AreaInfo));

        var outPath = GetPath("spawner");
        var dumper = new SpawnRipper9a(rom, "English", outPath);
        dumper.ExportRippedMaps();
        dumper.ExportHyperspacePickle();
    }

    private void DumpEncountPretty(EncountDataDBArray pokemon, PokemonDropItemTableDataDBArray drops)
    {
        var cfg = new TextConfig(GameVersion.ZA);
        const string lang = "English";
        List<string> result = [];

        var names = GetCommonText("monsname", lang, cfg);
        var forms = GetCommonText("zkn_form", lang, cfg);
        var abilities = GetCommonText("tokusei", lang, cfg);
        var natures = GetCommonText("seikaku", lang, cfg);
        var moves = GetCommonText("wazaname", lang, cfg);
        var items = GetCommonText("itemname", lang, cfg);

        var ahtbForms = rom.GetPackedFile($"ik_message/dat/{lang}/common/zkn_form.tbl");
        var personal = rom.GetPackedFile("avalon/data/personal_array.bin");
        var personalTable = Get<PersonalTable>(personal);

        var helper = new DumpHelper9a
        {
            SpeciesNames = names,
            AbilityNames = abilities,
            MoveNames = moves,
            NatureNames = natures,
            FormNames = forms,
            Personal = personalTable,
            Forms = new(ahtbForms),
        };

        int index = 0;
        foreach (var enc in pokemon.Table)
        {
            result.Add($"Table: {index++}");
            foreach (var t in enc.Table)
            {
                result.Add("===");
                result.Add($"{t.Id}");
                result.Add("===");

                var name = names[(int)t.DevNo];
                var form = helper.GetFormName((ushort)t.DevNo, (short)t.FormNo);
                var formDisplay = PrettyDumpUtil.GetFormDisplay((short)t.FormNo, form);
                result.Add($"{name}{formDisplay}");

                var fixedLevel = t.MinLevel == t.MaxLevel;
                if (fixedLevel)
                {
                    result.Add($"Level: {t.MinLevel}");
                }
                else
                {
                    result.Add($"Min Level: {t.MinLevel}");
                    result.Add($"Max Level: {t.MaxLevel}");
                }

                if (t.Sex is not (-1 or 2))
                    result.Add($"Gender: {PrettyDumpUtil.GetGender(t.Sex)}");

                result.Add($"Shiny: {PrettyDumpUtil.GetShiny(t.Rare)}");

                if ((sbyte)t.Tokusei is not -1)
                    result.Add($"Ability: {PrettyDumpUtil.GetAbilityName(t.DevNo, (short)t.FormNo, abilities, personalTable, (TokuseiType)t.Tokusei)}");

                result.Add($"Nature: {PrettyDumpUtil.GetNature(natures, t.Seikaku)}");

                if (t.TalentScale is not -1)
                    result.Add($"Scale: {t.TalentScale}");

                var ivs = t.TalentValue;
                if (ivs is { HP: -1, ATK: -1, DEF: -1, SPA: -1, SPD: -1, SPE: -1 })
                    result.Add(t.TalentVNum < 1 ? "IVs: Random" : $"IVs: {t.TalentVNum} Flawless");
                else
                    result.Add($"IVs: {ivs.HP}/{ivs.ATK}/{ivs.DEF}/{ivs.SPA}/{ivs.SPD}/{ivs.SPE}");

                if (t.StrengthenValue is { } s)
                {
                    ReadOnlySpan<int> evs = [s.HP, s.ATK, s.DEF, s.SPA, s.SPD, s.SPE];
                    if (evs.ContainsAnyInRange(1, 252))
                        result.Add($"EVs: {PrettyDumpUtil.GetEVSpread(evs)}");
                }

                if (t.OyabunProbability is not 0)
                    result.Add($"Alpha Probability: {t.OyabunProbability}%");

                if (t.OyabunAdditionalLevel is not 0)
                    result.Add($"Alpha Additional Level: +{t.OyabunAdditionalLevel}");

                if (t.Item is { } item)
                    result.Add($"Held Item: {items[item.ItemId]}");

                // not a generic wild zone encounter
                if (fixedLevel && t.OyabunAdditionalLevel is 0 && t.Moves is { } wl && (int)wl.Waza1 is not (0 or 65535))
                {
                    result.Add("Moves:");
                    result.Add($"- {moves[(int)wl.Waza1]}");
                    if ((int)wl.Waza2 is not (0 or 65535)) result.Add($"- {moves[(int)wl.Waza2]}");
                    if ((int)wl.Waza3 is not (0 or 65535)) result.Add($"- {moves[(int)wl.Waza3]}");
                    if ((int)wl.Waza4 is not (0 or 65535)) result.Add($"- {moves[(int)wl.Waza4]}");
                }

                if (t.ActivationConditionArray is { } a)
                {
                    foreach (var cond in a)
                    {
                        if (cond.Element is not { Count: not 0 } elemList)
                            continue; // never true

                        foreach (var elem in elemList)
                        {
                            if (elem.Param is not { Count: not 0 } pl)
                                continue; // never true

                            for (var i = 0; i < pl.Count; i++)
                            {
                                var param = pl[i];
                                var set = param.Param is null ? "NO PARAM" : string.Join(", ", param.Param);
                                result.Add($"Param[{i}] {param.Condition} - {param.Op} - {set}");
                            }
                        }
                    }
                }

                if (t.ItemDropInfoList is { } encDropData && t.OyabunProbability > 0)
                {
                    result.Add("Alpha Item Drops:");

                    var dropData = drops.Table;
                    foreach (var entry in dropData)
                    {
                        var dropItems = entry.Data;
                        foreach (var i in dropItems)
                        {
                            foreach (var slot in encDropData.Where(z => z.ItemTableId == i.Id))
                            {
                                float totalWeight = 0;
                                foreach (var dropped in i.ItemLotteryDataList!)
                                    totalWeight += dropped.Weight;

                                foreach (var dropped in i.ItemLotteryDataList!)
                                {
                                    var itemID = int.TryParse(dropped.ItemId, out var id);
                                    var weight = dropped.Weight;
                                    var maxCount = dropped.MaxCount;
                                    var type = dropped.Type;
                                    float rate = (float)(Math.Round((weight / totalWeight) * 100f, 2));
                                    var countDisplay = maxCount == 1 ? $"×{maxCount}" : $"×1-{maxCount}";
                                    result.Add($"- {rate:00.00}% {items[id]} ({countDisplay})");
                                }
                            }
                        }
                    }
                }

                result.Add("");
            }
        }

        File.WriteAllLines(GetPath("pretty", "encount_data_pretty.txt"), result);
    }

    private void DumpEncountPKHeX(EncountDataDBArray pokemon)
    {
        var cfg = new TextConfig(GameVersion.ZA);
        const string lang = "English";
        List<string> result = [];

        var names = GetCommonText("monsname", lang, cfg);
        var natures = GetCommonText("seikaku", lang, cfg);
        ReadOnlySpan<string> Trades = ["sub_addpoke_araichu", "sub_tradepoke_riolu", "sub_addpoke_gyadon", "sub_tradepoke_heracros"];

        foreach (var enc in pokemon.Table)
        {
            foreach (var t in enc.Table)
            {
                var name = names[(int)t.DevNo];
                var national = SpeciesConverterZA.GetNational9((ushort)t.DevNo);
                var formDisplay = t.FormNo is 0 ? string.Empty : $"-{t.FormNo}";
                var shiny = t.Rare switch
                {
                    0x3FFFFFFF => ", Shiny = Random",
                    0x2FFFFFFF => ", Shiny = Always",
                    _ => string.Empty,
                };
                var gender = t.Sex is -1 or 2 ? string.Empty : $", Gender = {t.Sex}";
                var nature = t.Seikaku is -1 ? string.Empty : $", Nature = Nature.{natures[t.Seikaku]}";

                var ivs = t.TalentValue;
                string iv = ivs switch
                {
                    { HP: 31, ATK: 31, DEF: 31, SPA: 31, SPD: 31, SPE: 31 } => ", FlawlessIVCount = 6",
                    { HP: -1, ATK: -1, DEF: -1, SPA: -1, SPD: -1, SPE: -1 }
                        => t.TalentVNum is 0 ? string.Empty : $", FlawlessIVCount = {t.TalentVNum}",
                    _ => $", IVs = new({ivs.HP:00},{ivs.ATK:00},{ivs.DEF:00},{ivs.SPE:00},{ivs.SPA:00},{ivs.SPD:00})",
                };

                var moves = "";
                if (t.OyabunAdditionalLevel is 0 && t.Moves is { } m && (int)m.Waza1 is not (0 or 65535))
                {
                    Span<int> moveset = [(int)m.Waza1, (int)m.Waza2, (int)m.Waza3, (int)m.Waza4];
                    moveset.Replace(65535, 0);
                    moves = $", Moves = new({moveset[0]:000},{moveset[1]:000},{moveset[2]:000},{moveset[3]:000})";
                }

                var alpha = t.OyabunProbability is 0 ? string.Empty : ", IsAlpha = true";
                var level = t.OyabunProbability is 0 ? t.MinLevel : (t.MinLevel + t.OyabunAdditionalLevel);
                var scale = t.TalentScale is -1 ? 0 : t.TalentScale;
                var line = $"        new({national:0000},{t.FormNo},{level:00},{scale:000}) {{ Location = 000{shiny}{gender}{nature}{iv}{moves}{alpha} }}, // {name}{formDisplay} ({t.Id})";

                // annoying that these are lumped together with gift encounters
                if (Trades.Contains(t.Id))
                {
                    var index = Trades.IndexOf(t.Id);
                    line = $"        new(TradeNames,{index},{national:0000},{t.FormNo},{level:00}) {{ ID32 = 000000, OTGender = 0{gender}{nature}{iv}{moves}{alpha} }}, // {name}{formDisplay} ({t.Id})";
                }

                result.Add(line);

                // is not fixed alpha, but can still be alpha OR non-alpha
                if (t.OyabunProbability is >= 1 and <= 99)
                {
                    line = line.Remove(19, 2).Insert(19, $"{t.MinLevel:00}").Replace(", IsAlpha = true", "");
                    result.Add(line);
                }
            }
        }

        File.WriteAllLines(GetPath("pkhex", "encount_data_pkhex.txt"), result);
    }

    private void DumpTradeNames()
    {
        var cfg = new TextConfig(GameVersion.ZA);
        foreach (var lang in LanguageEnglishNames)
        {
            var names = GetCommonText("namelist", lang, cfg);
            var l = GetLanguageCode(lang);
            List<string> result = [];

            result.Add(names[414]); // Bois (Heracross)
            result.Add(names[415]); // Riolouie (Riolu)
            result.Add(names[416]); // Spicy (Slowpoke)
            result.Add(names[417]); // Floffy (Raichu)
            result.Add(names[496]); // Alias (Porygon)

            result.Add(names[293]); // Tracie
            result.Add(names[306]); // Bond
            result.Add(names[310]); // Quille
            result.Add(names[362]); // Griddella
            result.Add(names[431]); // Trian

            File.WriteAllLines(GetPath("pkhex", "trades", $"text_tradeza_{l}.txt"), result);
        }
    }

    public void DumpScrubbedPokedexEntries()
    {
        var cfg = new TextConfig(GameVersion.ZA);
        const string lang = "English";
        List<string> result = [];

        var comments = GetCommonText("zukan_comment_A", lang, cfg);
        var names = GetCommonText("monsname", lang, cfg);
        var ahtb = rom.GetPackedFile("ik_message/dat/English/common/zukan_comment_A.tbl");
        var table = new AHTB(ahtb).Entries;

        for (int i = 0; i < table.Count - 1; i++)
        {
            var entry = table[i];
            var line = comments[i];
            var name = entry.Name;

            if (!name.StartsWith("msg_") || line.StartsWith("[~ "))
                continue;

            for (int s = 1; s <= 1025; s++)
            {
                var national = SpeciesConverterZA.GetNational9((ushort)s);
                for (int f = 0; f <= 28; f++)
                {
                    var target = $"ZKN_COMMENT_A_{s:000}_{f:000}";
                    var input = Encoding.ASCII.GetBytes(target);
                    var bytes = MD5.HashData(input);
                    var hash = Convert.ToHexString(bytes).ToLower();
                    if (name == $"msg_{hash}")
                        result.Add($"{national:0000}\t{target}\t{names[s]}-{f}");
                }
            }
        }

        result.Sort();
        File.WriteAllLines(GetPath("scrubbed", "pokedex_entries.txt"), result);
    }

    public void DumpScrubbedItemInfo()
    {
        var cfg = new TextConfig(GameVersion.ZA);
        const string lang = "English";
        List<string> result = [];

        var infos = GetCommonText("iteminfo", lang, cfg);
        var names = GetCommonText("itemname", lang, cfg);
        var ahtb = rom.GetPackedFile("ik_message/dat/English/common/iteminfo.tbl");
        var table = new AHTB(ahtb).Entries;

        for (int i = 0; i < table.Count - 1; i++)
        {
            var entry = table[i];
            var line = infos[i];
            var name = entry.Name;

            if (!name.StartsWith("msg_"))
                continue;

            var target = $"ITEMINFO_{i:000}";
            var input = Encoding.ASCII.GetBytes(target);
            var bytes = MD5.HashData(input);
            var hash = Convert.ToHexString(bytes).ToLower();
            if (name == $"msg_{hash}")
                result.Add($"{i:0000}\tITEMINFO_{i:000}\t{names[i]}");
        }

        File.WriteAllLines(GetPath("scrubbed", "item_infos.txt"), result);
    }

    public void DumpScrubbedMoveInfo()
    {
        var cfg = new TextConfig(GameVersion.ZA);
        const string lang = "English";
        List<string> result = [];

        var infos = GetCommonText("wazainfo", lang, cfg);
        var names = GetCommonText("wazaname", lang, cfg);
        var ahtb = rom.GetPackedFile("ik_message/dat/English/common/wazainfo.tbl");
        var table = new AHTB(ahtb).Entries;

        for (int i = 0; i < table.Count - 1; i++)
        {
            var entry = table[i];
            var line = infos[i];
            var name = entry.Name;

            if (!name.StartsWith("msg_") || line.StartsWith("[~ "))
                continue;

            var target = $"WAZAINFO_{i:000}";
            var input = Encoding.ASCII.GetBytes(target);
            var bytes = MD5.HashData(input);
            var hash = Convert.ToHexString(bytes).ToLower();
            if (name == $"msg_{hash}")
                result.Add($"{i:0000}\tWAZAINFO_{i:000}\t{names[i]}");
        }

        File.WriteAllLines(GetPath("scrubbed", "waza_infos.txt"), result);
    }

    public void DumpScrubbedPokedexCategories()
    {
        var cfg = new TextConfig(GameVersion.ZA);
        const string lang = "English";
        List<string> result = [];

        var comments = GetCommonText("zkn_type", lang, cfg);
        var names = GetCommonText("monsname", lang, cfg);
        var ahtb = rom.GetPackedFile("ik_message/dat/English/common/zkn_type.tbl");
        var table = new AHTB(ahtb).Entries;

        for (int i = 0; i < table.Count - 1; i++)
        {
            var entry = table[i];
            var line = comments[i];
            var name = entry.Name;

            if (!name.StartsWith("msg_") || line.StartsWith("[~ "))
                continue;

            for (int s = 1; s <= 1025; s++)
            {
                var national = SpeciesConverterZA.GetNational9((ushort)s);
                for (int f = 0; f <= 28; f++)
                {
                    var target = $"ZKN_TYPE_{s:000}_{f:000}";
                    var input = Encoding.ASCII.GetBytes(target);
                    var bytes = MD5.HashData(input);
                    var hash = Convert.ToHexString(bytes).ToLower();
                    if (name == $"msg_{hash}")
                        result.Add($"{national:0000}\t{target}\t{names[s]}-{f}");
                }
            }
        }

        result.Sort();
        File.WriteAllLines(GetPath("scrubbed", "pokedex_categories.txt"), result);
    }

    public void DumpScrubbedDressupItemNames()
    {
        var names = new[]
        {
            "dressup_item_bangs_{0:00}",
            "dressup_item_color_{0:000}",
            "dressup_item_darkcircles_{0:00}",
            "dressup_item_eye_{0:00}",
            "dressup_item_eyebrow_{0:00}",
            "dressup_item_eyecolor_tex_{0:00}",
            "dressup_item_eyecolor_{0:00}",
            "dressup_item_eyelash_{0:00}",
            "dressup_item_freckles_{0:00}",
            "dressup_item_group_{0:000}",
            "dressup_item_hair_{0:00}",
            "dressup_item_haircolor_{0:00}",
            "dressup_item_lip_{0:00}",
            "dressup_item_mole_{0:00}",
        };

        var cfg = new TextConfig(GameVersion.ZA);
        const string lang = "English";
        List<string> result = [];

        var dressup = GetCommonText("dressup_item_name", lang, cfg);
        var ahtb = rom.GetPackedFile("ik_message/dat/English/common/dressup_item_name.tbl");
        var table = new AHTB(ahtb).Entries;

        foreach (var t in names)
        {
            for (int i = 0; i < dressup.Length; i++)
            {
                var entry = table[i];
                var line = dressup[i];
                var name = entry.Name;

                if (!name.StartsWith("msg_"))
                    continue;

                for (int d = 0; d <= 999; d++)
                {
                    var target = string.Format(t, d);
                    var input = Encoding.ASCII.GetBytes(target);
                    var bytes = MD5.HashData(input);
                    var hash = Convert.ToHexString(bytes).ToLower();
                    if (name == $"msg_{hash}")
                        result.Add(target);
                }
            }
        }

        File.WriteAllLines(GetPath("scrubbed", "dressup_item_names.txt"), result);
    }

    public void DumpEventTriggers()
    {
        var eventTriggerFiles = GetTriggerFileList();

        const string outFolder = "event_trigger";
        Directory.CreateDirectory(GetPath(outFolder));

        //var unknownTriggers = new List<ulong>();
        //var unknownConditions = new List<ulong>();
        //var unknownCommands = new List<ulong>();

        var allLines = new List<string>();

        foreach (var f in eventTriggerFiles)
        {
            var data = rom.GetPackedFile(f);
            var table = FlatBufferConverter.DeserializeFrom<TriggerTable>(data);

            var curLines = new List<string> { $"File: {Path.GetFileName(f)}" };

            foreach (var line in TriggerUtil.GetTriggerTableSummary(table))
                curLines.Add($"\t{line}");

            var outFile = GetPath(outFolder, $"trigger_{Path.GetFileNameWithoutExtension(f).Replace("trigger_", string.Empty)}");

            DumpJson(table, $"{outFile}.json");
            File.WriteAllLines($"{outFile}.txt", curLines);

            allLines.AddRange(curLines);
            allLines.Add(string.Empty);
        }

        File.WriteAllLines(GetPath(outFolder, "triggerAll.txt"), allLines);

        //File.WriteAllLines(GetPath(outFolder, "triggerUnknownTypes.txt"), unknownTriggers.Order().Select(x => $"0x{x:X16},"));
        //File.WriteAllLines(GetPath(outFolder, "triggerUnknownConditions.txt"), unknownConditions.Order().Select(x => $"0x{x:X16},"));
        //File.WriteAllLines(GetPath(outFolder, "triggerUnknownCommands.txt"), unknownCommands.Order().Select(x => $"0x{x:X16},"));
    }

    private IEnumerable<string> GetTriggerFileList()
    {
        for (int i = 0; i <= 10; i++)
        {
            const string main = "ik_event/bin/trigger/main/trigger_main_{0:00}.bin";
            var file = string.Format(main, i);
            if (rom.HasFile(file))
                yield return file;
        }
        for (int i = 0; i <= 300; i++)
        {
            const string sub = "ik_event/bin/trigger/sub/trigger_sub_{0:0000}.bin";
            var file = string.Format(sub, i);
            if (rom.HasFile(file))
                yield return file;
        }

        yield return "ik_event/bin/trigger/map/trigger_map_t2.bin";
        yield return "ik_event/bin/trigger/global/trigger_global.bin";
        yield return "ik_event/bin/trigger/trainer_battle/trigger_trainer_battle_global.bin";
        yield return "ik_event/bin/trigger/wild_battle/trigger_wild_battle_global.bin";
        yield return "ik_event/bin/trigger/last_battle/trigger_last_battle_global.bin";
        yield return "ik_event/bin/trigger/boss_battle/trigger_boss_battle_global.bin";
        yield return "ik_event/bin/trigger/athletic_time_attack/trigger_athletic_time_attack.bin";
    }

    public void DumpDonutData()
    {
        var textConfig = new TextConfig(GameVersion.ZA);

        foreach (var language in LanguageEnglishNames)
        {
            var code = GetLanguageCode(language);

            // Get Donut Names
            // Skip index 0, only keep 203 entries
            var donuts = GetText("donut_name", language).AsSpan(1, 203);
            foreach (ref var name in donuts)
            {
                // remove string format. don't care for now.
                name = name.Replace("[VAR 01D9(0001)][VAR 0133(0000)] ", ""); // english
                name = name.Replace("[VAR 0133(0000)]", ""); // otherwise, before & after
                name = name.Replace("[VAR 01D9(0001)]", "");

            }
            WriteText("donutName", code, donuts.ToArray());

            // Get Flavor Names
            var flavors = GetText("donut_flavor", language).AsSpan(205, 290);
            // Need to skip over the unused entries.
            // Skip 3,4,5 and 147-152
            var flavorNames = new List<string>();
            for (int i = 0; i < flavors.Length; i++)
            {
                if (i is 3 or 4 or 5 or >= 147 and <= 152)
                    continue;
                flavorNames.Add(flavors[i]);
            }
            WriteText("donutFlavor", code, flavorNames);
        }

        return;

        void WriteText(string name, string code, IEnumerable<string> lines)
        {
            var path = GetPath("pkhex", "donut");
            Directory.CreateDirectory(path);
            var fileName = $"text_{name}_{code}.txt";
            File.WriteAllText(Path.Combine(path, fileName), string.Join('\n', lines));
        }

        string[] GetText(string name, string lang)
        {
            var data = rom.GetPackedFile($"{message}/dat/{lang}/sk/{name}.dat");
            return new TextFile(data, textConfig).Lines;
        }
    }

    public void DumpDimensionGates()
    {
        var files = new[]
        {
            "world/ik_scene/field/area/t1/sub_scene/t1_gimmick/t1_dim_gate_spawner/t1_dim_gate_spawner_blue_/t1_dim_gate_spawner_blue_0.trscn",
            "world/ik_scene/field/area/t1/sub_scene/t1_gimmick/t1_dim_gate_spawner/t1_dim_gate_spawner_jaune_/t1_dim_gate_spawner_jaune_0.trscn",
            "world/ik_scene/field/area/t1/sub_scene/t1_gimmick/t1_dim_gate_spawner/t1_dim_gate_spawner_rose_/t1_dim_gate_spawner_rose_0.trscn",
            "world/ik_scene/field/area/t1/sub_scene/t1_gimmick/t1_dim_gate_spawner/t1_dim_gate_spawner_rouge_/t1_dim_gate_spawner_rouge_0.trscn",
            "world/ik_scene/field/area/t1/sub_scene/t1_gimmick/t1_dim_gate_spawner/t1_dim_gate_spawner_veil_/t1_dim_gate_spawner_veil_0.trscn",
            "world/ik_scene/field/area/t1_i011/sub_scene/t1_i011_gimmick/t1_i011_dim_gate_spawner_/t1_i011_dim_gate_spawner_0.trscn",
            "world/ik_scene/field/area/t1_i012_1/sub_scene/t1_i012_1_gimmick/t1_i012_1_dim_gate_spawner_/t1_i012_1_dim_gate_spawner_0.trscn",
            "world/ik_scene/field/area/t1_i012_2/sub_scene/t1_i012_2_gimmick/t1_i012_2_dim_gate_spawner_/t1_i012_2_dim_gate_spawner_0.trscn",
            "world/ik_scene/field/area/t1_i014_1/sub_scene/t1_i014_1_gimmick/t1_i014_1_dim_gate_spawner_/t1_i014_1_dim_gate_spawner_0.trscn",
            "world/ik_scene/field/area/t2/sub_scene/t2_gimmick/t2_dim_gate_spawner_/t2_dim_gate_spawner_0.trscn",
            "world/ik_scene/field/area/t3/sub_scene/t3_gimmick/t3_dim_gate_spawner_/t3_dim_gate_spawner_0.trscn",
        };

        List<string> Result = [];
        foreach (var t in files)
        {
            var data = rom.GetPackedFile(t);
            var obj = FlatBufferConverter.DeserializeFrom<TrinitySceneObjectTemplate>(data);
            Result.Add(obj.ObjectTemplateName);

            foreach (var o in obj.Objects.Where(z => z.Type is "trinity_ObjectTemplate"))
            {
                var sceneObject = FlatBufferConverter.DeserializeFrom<TrinitySceneObjectTemplateData>(o.Data);
                var scenePoint = FlatBufferConverter.DeserializeFrom<TrinityScenePoint>(sceneObject.Data);
                var dim = sceneObject.ObjectTemplateName;
                var pos = scenePoint.Position;

                Result.Add($"{dim}\t{pos.X}\t{pos.Y}\t{pos.Z}");
            }

            Result.Add("");
        }

        File.WriteAllLines(GetPath("dim_gate", "dim_gate.txt"), Result);
    }
}

public static class PrettyDumpUtil
{
    private static readonly string[] StatNames = ["HP", "Atk", "Def", "SpA", "SpD", "Spe"];

    public static string GetAbilityName(DevID species, short form, ReadOnlySpan<string> abilities, PersonalTable personal, TokuseiType type)
    {
        if (type is TokuseiType.RANDOM_123)
            return "Any (1/2/H)";
        if (type is TokuseiType.RANDOM_12)
            return "Any (1/2)";

        var target = personal.Table.FirstOrDefault(z => z.Info.SpeciesInternal == (ushort)species && z.Info.Form == form)!;
        var value = GetAbility(target, type);
        var text = GetAbilitySuffix(type);
        return $"{abilities[value]} ({text})";
    }

    private static ushort GetAbility(PersonalInfo target, TokuseiType type) => type switch
    {
        TokuseiType.SET_1 => target.Ability1,
        TokuseiType.SET_2 => target.Ability2,
        TokuseiType.SET_3 => target.AbilityH,
        (TokuseiType)0xFF => 0,
        _ => throw new ArgumentOutOfRangeException(nameof(type), type, null),
    };

    private static string GetAbilitySuffix(TokuseiType type) => type switch
    {
        TokuseiType.SET_1 => "1",
        TokuseiType.SET_2 => "2",
        TokuseiType.SET_3 => "H",
        (TokuseiType)0xFF => "-",
        _ => throw new ArgumentOutOfRangeException(nameof(type), type, null),
    };

    public static string GetMoveText(WazaSetBattle waza, ReadOnlySpan<string> moves) => GetMoveText(waza.WazaId, moves, waza.IsPlusWaza);
    public static string GetMoveText(WazaSet waza, ReadOnlySpan<string> moves) => GetMoveText(waza.WazaId, moves, waza.IsPlusWaza);

    public static string GetMoveText(WazaID id, ReadOnlySpan<string> moves, bool plus)
    {
        if (id == 0)
            return string.Empty;
        var name = $"{moves[(int)id]}";
        if (plus)
            name += " (+)";
        return name;
    }

    public static List<ushort> GetCurrentMoves(DevID species, short form, byte level, PersonalTable personal)
    {
        var target = personal.Table
            .First(z => z.Info.SpeciesInternal == (ushort)species && z.Info.Form == form);
        List<ushort> Moves = target.Learnset.Where(z => z.Level <= level)
            .Distinct().TakeLast(4).Select(z => z.Move).ToList();

        // always return 4 moves
        while (Moves.Count < 4)
            Moves.Add(0);

        return Moves;
    }

    public static string GetEVSpread(ReadOnlySpan<int> evs)
    {
        StringBuilder sb = new();
        for (int i = 0; i < evs.Length; i++)
        {
            if (evs[i] <= 0)
                continue;
            if (sb.Length != 0)
                sb.Append(" / ");
            sb.Append($"{evs[i]} {StatNames[i]}");
        }
        return sb.ToString();
    }

    public static string GetFormString(ushort species, short form, ReadOnlySpan<string> forms, AHTB formNames)
    {
        var hash = FnvHash.HashFnv1a_64($"ZKN_FORM_{species:000}_{form:000}");
        var index = formNames.GetIndex(hash);
        return forms[index];
    }

    public static string GetFormDisplay(short form, string name) => name switch
    {
        not "" when form is 0 => $" ({name})",
        not "" => $"-{form} ({name})",
        _ => string.Empty,
    };

    public static string GetGender(int gender) => gender switch
    {
        0 => "Male",
        1 => "Female",
        _ => "Random",
    };

    public static string GetNature(ReadOnlySpan<string> natures, int nature) => nature is -1 ? "Random" : $"{natures[nature]}";

    public static string GetShiny(int rare) => rare switch
    {
        0x1FFFFFFF => "Never",
        0x2FFFFFFF => "Always",
        _ => "Random",
    };
}

public static class TriggerUtil
{
    public static IEnumerable<string> GetTriggerTableSummary(TriggerTable tab)
    {
        foreach (var trg in tab.Table!)
        {
            yield return "Trigger:";
            foreach (var line in GetTriggerSummary(trg))
                yield return $"\t{line}";
        }
    }

    public static IEnumerable<string> GetTriggerSummary(Trigger trigger)
    {
        if (trigger.Start is not { } start)
        {
            yield return "No Start Condition!";
            yield break;
        }
        yield return "Start:";
        yield return $"Condition: {start.Condition}";
        if (start.Param is { Count: not 0 } p)
        {
            yield return "Params:";
            foreach (var param in p)
                yield return $"\t{string.Join(", ", param)}";
        }

        if (start.Activators is { Count: not 0 } act)
        {
            yield return "Activators:";
            foreach (var a in act)
            {
                if (a.Triggers is { Count: not 0 } t)
                {
                    yield return $"(Triggers: {t.Count})";
                    foreach (var trg in t)
                        yield return $"\t- {string.Join(Environment.NewLine, trg.Summarize())}";
                }
                else
                {
                    yield return "(No Triggers)";
                }

                if (a.Element is { Count: not 0 } elem)
                {
                    yield return $"Elements: {elem.Count}";
                    foreach (var e in elem)
                        yield return $"\t- {string.Join(Environment.NewLine, e.Summarize())}";
                }
                else
                {
                    yield return "No Elements";
                }
            }
        }
    }

    public static IEnumerable<string> Summarize(this TriggerCommand cmd)
    {
        if (cmd.Element is not { Count: not 0 } x)
        {
            yield return "Command: (No Elements)";
            yield break;
        }

        foreach (var elem in x)
        {
            var args = elem.Param is { Count: not 0 } p ? GetTriggerArgsSummary(p) : "No Args";
            yield return $"Command: {elem.Command} - {args}";
        }
    }

    public static IEnumerable<string> Summarize(this ActivationConditionElement act)
    {
        if (act.Param is not { Count: not 0 } p)
        {
            yield return "Condition: (No Params)";
            yield break;
        }

        foreach (var elem in p)
        {
            var args = elem.Param is { Count: not 0 } pp ? GetTriggerArgsSummary(pp) : "No Args";
            yield return $"Condition: {elem.Condition} - {elem.Op} - {args}";
        }
    }

    public static string GetTriggerArgsSummary(IList<string> args)
    {
        var firstEmpty = -1;
        for (var i = 0; i < args.Count; i++)
        {
            if (firstEmpty >= 0 && !string.IsNullOrEmpty(args[i]))
                throw new ArgumentException($"Invalid TriggerArg at index {i}!");
            if (firstEmpty < 0 && string.IsNullOrEmpty(args[i]))
                firstEmpty = i;
        }

        return string.Join(", ", args.Select(s => $"\"{s}\"").Take(firstEmpty >= 0 ? firstEmpty : args.Count));
    }
}

