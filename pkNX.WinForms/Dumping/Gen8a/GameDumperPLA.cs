using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using pkNX.Containers;
using pkNX.Game;
using pkNX.Structures;
using pkNX.Structures.FlatBuffers;
using pkNX.Structures.FlatBuffers.Arceus;
using FnvHash = pkNX.Containers.FnvHash;

using Legal = pkNX.Structures.Legal;
using IPersonalTable = pkNX.Structures.IPersonalTable;
using Learnset = pkNX.Structures.FlatBuffers.Arceus.Learnset;
using PersonalInfo = pkNX.Structures.FlatBuffers.Arceus.PersonalInfo;
using PersonalTable = pkNX.Structures.FlatBuffers.Arceus.PersonalTable;
using Species = pkNX.Structures.Species;

namespace pkNX.WinForms;

public class GameDumperPLA(GameManagerPLA rom)
{
    public string DumpFolder
    {
        get
        {
            var parent = Directory.GetParent(rom.PathRomFS);
            ArgumentNullException.ThrowIfNull(parent, nameof(parent));
            return Path.Combine(parent.FullName, "Dump");
        }
    }

    private string GetPath(string path)
    {
        Directory.CreateDirectory(DumpFolder);
        var result = Path.Combine(DumpFolder, path);
        var parent = Directory.GetParent(result);
        ArgumentNullException.ThrowIfNull(parent);
        Directory.CreateDirectory(parent.FullName); // double check :(
        return result;
    }

    private string GetPath(string parent, string path)
    {
        Directory.CreateDirectory(DumpFolder);
        var result = Path.Combine(DumpFolder, parent, path);
        var parent2 = Directory.GetParent(result);
        ArgumentNullException.ThrowIfNull(parent2);
        Directory.CreateDirectory(parent2.FullName); // double check :(
        return result;
    }

    public void DumpPersonal()
    {
        var data = rom.GetFile(GameFile.PersonalStats)[0];
        /*var obj = FlatBufferConverter.DeserializeFrom<PersonalTableLA>(data);
        var test = PersonalConverter.GetBin(obj);
        var path = GetPath("personal_la");
        File.WriteAllBytes(path, test);*/

        var csv = GetPath("personal.csv");
        var dump = FlatDumper.GetTable<PersonalTable, PersonalInfo>(data, z => z.Table);
        File.WriteAllText(csv, dump);
    }

    public void DumpPokeInfo()
    {
        var s = rom.GetStrings(TextName.SpeciesNames);

        var lrd = rom.GetFile(GameFile.Learnsets)[0];
        var lr = FlatBufferConverter.DeserializeFrom<Learnset>(lrd);
        var evd = rom.GetFile(GameFile.Evolutions)[0];
        var ev = FlatBufferConverter.DeserializeFrom<EvolutionTable>(evd);
        var pt = new PersonalTable8LA(rom.GetFile(GameFile.PersonalStats));
        var altForms = pt.GetFormList(s);
        var entryNames = pt.GetPersonalEntryList(altForms, s, out _, out _);
        var moveNames = rom.GetStrings(TextName.MoveNames);

        var pd = new PersonalDumperPLA
        {
            Colors = Enum.GetNames<PokeColor>(),
            EggGroups = Enum.GetNames<EggGroup>(),
            EntryLearnsets = lr.Table,
            EntryNames = entryNames,
            ExpGroups = Enum.GetNames<EXPGroup>(),
            Evos = ev.Table,

            Abilities = rom.GetStrings(TextName.AbilityNames),
            Items = rom.GetStrings(TextName.ItemNames),
            Moves = moveNames,
            Types = rom.GetStrings(TextName.TypeNames),
            Species = rom.GetStrings(TextName.SpeciesNames),
            ZukanA = rom.GetStrings(TextName.PokedexEntry1),
            ZukanB = rom.GetStrings(TextName.PokedexEntry2),
            TMIndexes = Legal.TMHM_SWSH,
        };

        var result = pd.Dump(pt);

        var outname = GetPath("Pokemon.txt");
        File.WriteAllLines(outname, result);

        var learnTable = pd.MoveSpeciesLearn;
        var outLearn = GetPath("MovePerPokemon.txt");
        var moveLines = learnTable.Select((z, i) => $"{i:000}\t{moveNames[i]}\t{string.Join(", ", z.Distinct())}");
        File.WriteAllLines(outLearn, moveLines);

        DumpMoveUsers(pt, lr);
    }

    private void DumpMoveUsers(IPersonalTable pt, Learnset lr)
    {
        List<string> Users = [];
        var moves = rom.GetStrings(TextName.MoveNames);
        var spec = rom.GetStrings(TextName.SpeciesNames);
        var shop = Legal.MoveShop8_LA;
        for (int i = 0; i < moves.Length; i++)
        {
            var move = i;
            var shopIndex = Array.IndexOf(shop, move);
            bool isShop = shopIndex != -1;
            var learn = lr.Table.Where(z => z.Arceus.Any(x => x.Move == move));
            var filtered = learn.Where(z => ((IPersonalInfoPLA)pt.GetFormEntry(z.Species, (byte)z.Form)).IsPresentInGame);
            var result = filtered.Select(x => GetSpeciesMove(spec, x, move)).ToArray();

            List<string> r = [$"{moves[move]}:"];
            if (isShop)
            {
                var species = pt.Table.OfType<IPersonalInfoPLA>().Where(z => z.SpecialTutors[shopIndex] && z.IsPresentInGame);
                var names = species.Select(z => $"{spec[z.DexIndexNational]}{(z.Form == 0 ? "" : $"-{z.Form}")}");
                r.Add($"\tTutors: {string.Join(", ", names)}");
            }

            var lv = result.Length == 0 ? "None." : string.Join(", ", result);
            r.Add($"\tLevel Up: {lv}");
            Users.AddRange(r);
        }

        var outname = GetPath("MoveUsers.txt");
        File.WriteAllLines(outname, Users);
    }

    private static string GetSpeciesMove(ReadOnlySpan<string> spec, LearnsetMeta x, int move)
    {
        var learnset = x.Arceus.FirstOrDefault(w => w.Move == move);
        var level = learnset is null ? "INVALID" : learnset.Level.ToString();
        return $"{spec[x.Species]}{(x.Form == 0 ? "" : $"-{x.Form}")} @ {level}";
    }

    private static readonly string[] ahtbext = [".tbl", ".hsh"];

    public void DumpAHTB()
    {
        var files = Directory.EnumerateFiles(rom.PathRomFS, "*", SearchOption.AllDirectories)
            .Where(z => ahtbext.Contains(Path.GetExtension(z)));

        var result = new HashSet<string>();
        var list = new List<string>();
        var gf = new List<string>();
        foreach (var f in files)
        {
            var bytes = File.ReadAllBytes(f);
            if (AHTB.IsAHTB(bytes))
            {
                var tbl = new AHTB(bytes);
                var summaries = tbl.Summary;
                foreach (var t in tbl.ShortSummary)
                    result.Add(t);
                list.Add(Path.GetFileName(f));
                list.AddRange(summaries);
            }
            else if (DatTable.IsDatTable(bytes))
            {
                var tbl = new DatTable(bytes);
                var summaries = tbl.Summary;
                foreach (var t in tbl.ShortSummary)
                    result.Add(t);
                list.Add(Path.GetFileName(f));
                list.AddRange(summaries);
            }
        }

        var paks = Directory.EnumerateFiles(rom.PathRomFS, "*", SearchOption.AllDirectories)
            .Where(z => Path.GetExtension(z) == ".gfpak");
        foreach (var f in paks)
        {
            var pak = new GFPack(f);
            foreach (var bytes in pak.DecompressedFiles)
            {
                if (!AHTB.IsAHTB(bytes))
                    continue;

                var tbl = new AHTB(bytes);
                var summaries = tbl.Summary;
                foreach (var t in tbl.ShortSummary)
                    result.Add(t);
                list.Add(Path.GetFileName(f));
                list.AddRange(summaries);
            }

            for (var i = 0; i < pak.HashAbsolute.Length; i++)
            {
                var x = pak.HashAbsolute[i];
                gf.Add($"{x.HashFnv1aPathFull:X16}\t{f}.Absolute[{i}]");
            }

            for (var i = 0; i < pak.HashInFolder.Length; i++)
            {
                var x = pak.HashInFolder[i];
                var folder = x.Folder;
                gf.Add($"{folder.HashFnv1aPathFolderName:X16}\t{f}.Folder[{i}] ({folder.FileCount})");
                for (int j = 0; j < x.Files.Length; j++)
                {
                    var y = x.Files[j];
                    gf.Add($"{y.HashFnv1aPathFileName:X16}\t{f}.Folder[{i}][{j}] ({y.Index})");
                }
            }
        }

        var outname = GetPath("ahtb.txt");
        var outname2 = GetPath("ahtblist.txt");
        var outname3 = GetPath("gfpakhash.txt");
        File.WriteAllLines(outname, result);
        File.WriteAllLines(outname2, list);
        File.WriteAllLines(outname3, gf);
    }

    public void DumpDrops()
    {
        var names = rom.GetStrings(TextName.ItemNames);
        var field = Path.Combine(rom.PathRomFS, "bin", "pokemon", "data", "poke_drop_item.bin");
        var fieldItems = FlatBufferConverter.DeserializeFrom<PokeDropItemArchive>(field).Table.Select(z => z.Dump(names));
        File.WriteAllLines(GetPath("DropItems.txt"), fieldItems);

        var battle = Path.Combine(rom.PathRomFS, "bin", "pokemon", "data", "poke_drop_item_battle.bin");
        var battleItems = FlatBufferConverter.DeserializeFrom<PokeDropItemBattleArchive>(battle).Table.Select(z => z.Dump(names));
        File.WriteAllLines(GetPath("BattleDropItems.txt"), battleItems);
    }

    public void DumpItems()
    {
        var items = rom.GetFile(GameFile.ItemStats);
        var file = items[0];
        var array = Item8a.GetArray(file);
        var groups = array.GroupBy(z => z.Pouch);
        foreach (var g in groups)
        {
            var key = g.Key;
            var IDs = g.Where(z => z.ItemSprite >= 0).Select(z => z.ItemID);
            var path = GetPath($"{key}.txt");
            File.WriteAllText(path, string.Join(", ", IDs.Select(z => $"{z:000}")));
        }

        var names = rom.GetStrings(TextName.ItemNames);
        var lines = TableUtil.GetNamedTypeTable(array, names, "Items");
        var table = GetPath("ItemData.txt");
        var bin = GetPath("ItemData.bin");
        File.WriteAllText(table, lines);
        File.WriteAllBytes(bin, array.SelectMany(z => z.Data.ToArray()).ToArray());
    }

    public void DumpMoves()
    {
        var dir = Path.Combine(rom.PathRomFS, "bin", "pml", "waza");
        var files = Directory.GetFiles(dir);
        var moves = FlatBufferConverter.DeserializeFrom<Waza>(files);
        var names = rom.GetStrings(TextName.MoveNames);
        var lines = TableUtil.GetNamedTypeTable(moves, names, "Moves");
        var table = GetPath("MoveData.txt");
        File.WriteAllText(table, lines);

        var pp = moves.Select(z => z.PP);
        var str = string.Join(", ", pp.Select(z => $"{z:00}"));
        var pppath = GetPath("MovePP.txt");
        File.WriteAllText(pppath, str);

        // get dummied moves
        var moveNames = rom.GetStrings(TextName.MoveNames);
        var moveDesc = rom.GetStrings(TextName.MoveFlavor);

        var tuple = moveNames
            .Select((z, i) => new { Name = z, Desc = moveDesc[i], Index = i })
            .Where(z => !moves[z.Index].CanUseMove)
            .Select(z => $"{z.Index:000}\t{z.Name}");

        var path = GetPath("snappedMoves.txt");
        File.WriteAllLines(path, tuple); // rest in peace

        var snap2 = GetPath("snappedID.txt");
        var snapMove = string.Join(", ", moves.Where(z => !z.CanUseMove).Select(z => $"{z.MoveID:000}"));
        File.WriteAllText(snap2, snapMove);
    }

    public void DumpLearnsetBinary()
    {
        var data = rom.GetFile(GameFile.Learnsets)[0];
        var obj = FlatBufferConverter.DeserializeFrom<Learnset>(data);
        var pt = new PersonalTable8LA(rom.GetFile(GameFile.PersonalStats));
        var result = new byte[pt.Table.Length][];
        var mastery = new byte[pt.Table.Length][];
        for (int i = 0; i < result.Length; i++)
            result[i] = mastery[i] = [];

        var Dupes = new List<(int Species, int Form)>();
        foreach (var e in obj.Table)
        {
            if (e.Arceus.Count == 0)
                continue;
            var index = pt.GetFormIndex(e.Species, (byte)e.Form);
            var entry = (IPersonalInfoPLA)pt[index];
            if (!entry.IsPresentInGame)
                continue;
            result[index] = e.WriteLearnsetAsLearn6();
            mastery[index] = e.WriteMasteryAsLearn6();

            if (e.Arceus.Select(z => z.Level).Distinct().Count() != e.Arceus.Count)
                Dupes.Add(new(e.Species, e.Form));
        }

        // Learnset
        {
            var mini = MiniUtil.PackMini(result, "la");
            var bin = GetPath(Path.Combine("bin", "lvlmove_la.pkl"));
            File.WriteAllBytes(bin, mini);
        }
        // Mastery
        {
            var mini = MiniUtil.PackMini(mastery, "la");
            var bin = GetPath(Path.Combine("bin", "mastery_la.pkl"));
            File.WriteAllBytes(bin, mini);
        }
        // Dupes
        {
            var txt = GetPath(Path.Combine("bin", "lvlmove_dupes.txt"));
            File.WriteAllLines(txt, Dupes.Select(z => $"{(Species)z.Species}{(z.Form == 0 ? "" : $"{z.Form}")}"));
        }
    }

    public void DumpEvolutionBinary()
    {
        // format matches past gen and PKHeX's expected format
        var data = rom.GetFilteredFolder(GameFile.Evolutions)[0];
        var obj = FlatBufferConverter.DeserializeFrom<EvolutionTable>(data);
        var pt = new PersonalTable8LA(rom.GetFile(GameFile.PersonalStats));
        var result = new byte[pt.Table.Length][];
        for (int i = 0; i < result.Length; i++)
            result[i] = [];

        foreach (var e in obj.Table)
        {
            if (e.Table?.Count is not > 0)
                continue;
            var index = pt.GetFormIndex(e.Species, (byte)e.Form);
            var entry = (IPersonalInfoPLA)pt[index];
            if (!entry.IsPresentInGame)
                continue;
            result[index] = e.Write();
        }

        var mini = MiniUtil.PackMini(result, "la");
        var bin = GetPath(Path.Combine("bin", "evos_la.pkl"));
        File.WriteAllBytes(bin, mini);
    }

    public void DumpGifts()
    {
        var speciesNames = rom.GetStrings(TextName.SpeciesNames);
        var data = rom.GetFile(GameFile.EncounterTableGift)[0];
        var gifts = FlatBufferConverter.DeserializeFrom<PokeAddArchive>(data);
        var table = TableUtil.GetTable(gifts.Table);
        var fn = GetPath("Gifts.txt");
        File.WriteAllText(fn, table);

        var f2 = GetPath("GiftsPKHeX.txt");
        File.WriteAllLines(f2, gifts.Table.Select(z => z.Dump(speciesNames)));
    }

    public void DumpStatic()
    {
        var speciesNames = rom.GetStrings(TextName.SpeciesNames);
        var data = rom.GetFile(GameFile.EncounterTableStatic)[0];
        var statics = FlatBufferConverter.DeserializeFrom<EventEncountArchive>(data);

        var lines = new List<string>();

        foreach (var set in statics.Table)
        {
            lines.Add($"{set.EncounterName}:");

            var table = TableUtil.GetTable(set.Table);

            foreach (var line in table.Split(Environment.NewLine))
                lines.Add($"\t{line}");
        }

        var fn = GetPath("StaticEncounters.txt");
        File.WriteAllLines(fn, lines);

        var f2 = GetPath("StaticEncountersPKHeX.txt");
        var result = statics.Table.SelectMany(z => z.Table.Select(x => x.Dump(speciesNames, z.EncounterName)))
            .Order();
        File.WriteAllLines(f2, result);
    }

    public void DumpWilds()
    {
        var speciesName = rom.GetStrings(TextName.SpeciesNames);

        Dictionary<string, (string Name, int Index)> map = GetPlaceNameMap();
        var multdata = rom.GetFile(GameFile.EncounterRateTable)[0];
        var multipliers = FlatBufferConverter.DeserializeFrom<EncounterMultiplierArchive>(multdata);
        var miscdata = rom.GetFile(GameFile.PokeMisc)[0];
        var misc = FlatBufferConverter.DeserializeFrom<PokeMiscTable>(miscdata);

        var nhoGroup_b = rom.GetFile(GameFile.NewHugeGroup)[0];
        var nhoGroup = FlatBufferConverter.DeserializeFrom<NewHugeOutbreakGroupArchive>(nhoGroup_b);
        var nhoGroupL_b = rom.GetFile(GameFile.NewHugeGroupLottery)[0];
        var nhoGroupL = FlatBufferConverter.DeserializeFrom<NewHugeOutbreakGroupLotteryArchive>(nhoGroupL_b);

        var resident = (GFPack)rom.GetFile(GameFile.Resident);
        var bin_settings = resident.GetDataFullPath("bin/field/resident/AreaSettings.bin");
        var settings = FlatBufferConverter.DeserializeFrom<AreaSettingsTable>(bin_settings);

        const string wild = "wild";
        Directory.CreateDirectory(GetPath(wild));

        var all = new List<string>();
        var allUnownLines = new List<string>();
        var allUnownLinesBias = new List<string>();
        const float bias = 20;

        var hexBin = new List<byte[]>();
        var allSlots = new List<EncounterSlot>();
        var allSpawners = new List<PlacementSpawner>();
        var allWormholes = new List<PlacementSpawner>();
        var allLocations = new List<PlacementLocation>();
        var allLandItems = new List<LandmarkItemSpawn>();
        var allLandMarks = new List<LandmarkItem>();
        var allUnown = new List<PlacementUnnnEntry>();
        var allMkrg = new List<PlacementMkrgEntry>();
        var allSearchItem = new List<PlacementSearchItem>();
        var allEncounters = new List<EncounterTable>();
        foreach (var areaNameList in ResidentAreaSet.AreaNames)
        {
            var instance = AreaInstance.Create(resident, areaNameList, settings);
            var lines = EncounterTableUtil.GetLines(multipliers, misc, speciesName, instance, nhoGroup, nhoGroupL, map).ToList();
            File.WriteAllLines(GetPath(wild, $"Encounters_{instance.AreaName}.txt"), lines);

            var unown = EncounterTableUtil.GetUnownLines(instance, map).Distinct().ToList();
            File.WriteAllLines(GetPath(wild, $"unown_0_{instance.AreaName}.txt"), unown);
            var unownBias = EncounterTableUtil.GetUnownLinesBias(instance, map, bias).Distinct().ToList();
            File.WriteAllLines(GetPath(wild, $"unown_{bias:0}_{instance.AreaName}.txt"), unownBias);
            allUnownLines.AddRange(unown);
            allUnownLinesBias.AddRange(unownBias);

            var slices = EncounterTableUtil.GetEncounterDump(instance, map, misc, nhoGroup, nhoGroupL);
            foreach (var s in slices)
            {
                if (!hexBin.Any(z => z.SequenceEqual(s)))
                    hexBin.Add(s);
            }

            all.AddRange(lines);
            all.Add(string.Empty);

            allSlots.AddRange(instance.Encounters.SelectMany(z => z.Table));
            allSpawners.AddRange(instance.Spawners);
            allWormholes.AddRange(instance.Wormholes);
            allLocations.AddRange(instance.Locations);
            allLandItems.AddRange(instance.LandItems);
            allLandMarks.AddRange(instance.LandMarks);
            allUnown.AddRange(instance.Unown);
            allMkrg.AddRange(instance.Mikaruge);
            allSearchItem.AddRange(instance.SearchItem);
            allEncounters.AddRange(instance.Encounters);

            foreach (var subArea in instance.SubAreas)
            {
                allSlots.AddRange(subArea.Encounters.SelectMany(z => z.Table));
                allSpawners.AddRange(subArea.Spawners);
                allWormholes.AddRange(subArea.Wormholes);
                allLocations.AddRange(subArea.Locations);
                allLandItems.AddRange(subArea.LandItems);
                allLandMarks.AddRange(subArea.LandMarks);
                allUnown.AddRange(subArea.Unown);
                allMkrg.AddRange(subArea.Mikaruge);
                allSearchItem.AddRange(subArea.SearchItem);
                allEncounters.AddRange(subArea.Encounters);
            }
        }

        var mini = MiniUtil.PackMini(hexBin.ToArray(), "la");
        File.WriteAllBytes(GetPath(wild, "encounter_la.pkl"), mini);
        File.WriteAllLines(GetPath(wild, "Encounters_All.txt"), all);
        File.WriteAllLines(GetPath(wild, "Unown_All.txt"), allUnownLines);
        File.WriteAllLines(GetPath(wild, $"Unown_All_Bias_{bias}.txt"), allUnownLinesBias);

        File.WriteAllText(GetPath(wild, "allEncounters.csv"), TableUtil.GetTable(allEncounters));
        File.WriteAllText(GetPath(wild, "allMultipliers.csv"), TableUtil.GetTable(multipliers.Table));
        File.WriteAllText(GetPath(wild, "PokeMisc.csv"), TableUtil.GetTable(misc.Table));
        File.WriteAllText(GetPath(wild, "allSlotTable.csv"), TableUtil.GetTable(allSlots));
        File.WriteAllText(GetPath(wild, "allSpawnerTable.csv"), TableUtil.GetTable(allSpawners));
        File.WriteAllText(GetPath(wild, "allWormholeTable.csv"), TableUtil.GetTable(allWormholes));
        File.WriteAllText(GetPath(wild, "allLocationTable.csv"), TableUtil.GetTable(allLocations));
        File.WriteAllText(GetPath(wild, "allLandMarks.csv"), TableUtil.GetTable(allLandMarks));
        File.WriteAllText(GetPath(wild, "allLandMarkSpawns.csv"), TableUtil.GetTable(allLandItems));
        File.WriteAllText(GetPath(wild, "allUnown.csv"), TableUtil.GetTable(allUnown));
        File.WriteAllText(GetPath(wild, "allMkrg.csv"), TableUtil.GetTable(allMkrg));
        File.WriteAllText(GetPath(wild, "allSearchItem.csv"), TableUtil.GetTable(allSearchItem));
    }

    public void DumpResident()
    {
        var resident = (GFPack)rom.GetFile(GameFile.Resident);
        var bin_settings = resident.GetDataFullPath("bin/field/resident/AreaSettings.bin");
        var settings = FlatBufferConverter.DeserializeFrom<AreaSettingsTable>(bin_settings);
        var dir = GetPath("Resident");
        var props = typeof(AreaSettings).GetProperties();
        foreach (var x in settings.Table)
        {
            foreach (var p in props)
            {
                var value = p.GetValue(x);
                if (value is not string { Length: not 0 } s)
                    continue;
                if (!s.Contains('/'))
                    continue;
                if (File.Exists(Path.Combine(rom.PathRomFS, s)))
                    continue;

                try
                {
                    int index = resident.GetIndexFull(FnvHash.HashFnv1a_64(s));
                    if (index == -1)
                        continue;
                    var data = resident.GetDataFullPath(s);
                    var file = s.Replace('/', '\\');
                    var dest = Path.Combine(dir, file);
                    var folder = Path.GetDirectoryName(dest);
                    ArgumentNullException.ThrowIfNull(folder);

                    Directory.CreateDirectory(folder);
                    File.WriteAllBytes(dest, data);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Unable to extract {s}: {ex.Message}");
                }
            }
        }
    }

    public void DumpPlacement()
    {
        var resident = (GFPack)rom.GetFile(GameFile.Resident);
        var bin_settings = resident.GetDataFullPath("bin/field/resident/AreaSettings.bin");
        var settings = FlatBufferConverter.DeserializeFrom<AreaSettingsTable>(bin_settings);

        Dictionary<string, (string Name, int Index)> map = GetPlaceNameMap();

        var location_all = new List<string>();
        var spawner_all = new List<string>();
        var wh_spawner_all = new List<string>();
        var mkrg_all = new List<string>();
        const string placement = "placement";
        Directory.CreateDirectory(GetPath(placement));

        foreach (var areaNameList in ResidentAreaSet.AreaNames)
        {
            var area = AreaInstance.Create(resident, areaNameList, settings);
            foreach (var subArea in new[] { area }.Concat(area.SubAreas))
            {
                var areaName = subArea.AreaName;
                if (subArea.Locations.Count != 0)
                {
                    var loc_lines = GetLocationBoundLines(areaName, map, subArea.Locations);

                    location_all.AddRange(loc_lines);
                    location_all.Add(string.Empty);
                    File.WriteAllLines(GetPath(placement, $"Location_{areaName}.txt"), loc_lines);
                }
                if (subArea.Spawners.Count != 0)
                {
                    var spwn_lines = GetSpawnLines(areaName, map, subArea.Spawners, subArea.Locations);

                    spawner_all.AddRange(spwn_lines);
                    spawner_all.Add(string.Empty);
                    File.WriteAllLines(GetPath(placement, $"Spawner_{areaName}.txt"), spwn_lines);
                }
                if (subArea.Wormholes.Count != 0)
                {
                    var spwn_lines = GetSpawnLines(areaName, map, subArea.Wormholes, subArea.Locations);

                    wh_spawner_all.AddRange(spwn_lines);
                    wh_spawner_all.Add(string.Empty);
                    File.WriteAllLines(GetPath(placement, $"WhSpawner_{areaName}.txt"), spwn_lines);
                }
                if (subArea.Mikaruge.Count != 0)
                {
                    var mkrg_lines = GetMikarugeLines(areaName, map, subArea.Mikaruge, subArea.Locations);

                    mkrg_all.AddRange(mkrg_lines);
                    mkrg_all.Add(string.Empty);
                    File.WriteAllLines(GetPath(placement, $"Mikaruge_{areaName}.txt"), mkrg_lines);
                }
                if (subArea.SearchItem.Count != 0)
                {
                    var mkrg_lines = GetSearchItemLines(areaName, map, subArea.SearchItem, subArea.Locations);

                    mkrg_all.AddRange(mkrg_lines);
                    mkrg_all.Add(string.Empty);
                    File.WriteAllLines(GetPath(placement, $"SearchItem_{areaName}.txt"), mkrg_lines);
                }

                // Debug for Visualization
                if (areaName is "ha_area01" or "ha_area02" or "ha_area03" or "ha_area04" or "ha_area05")
                    DumpVisualizationData(area);
            }
        }

        File.WriteAllLines(GetPath(placement, "Location_all.txt"), location_all);
        File.WriteAllLines(GetPath(placement, "Spawner_all.txt"), spawner_all);
        File.WriteAllLines(GetPath(placement, "WhSpawner_all.txt"), wh_spawner_all);
        File.WriteAllLines(GetPath(placement, "Mikaruge_all.txt"), mkrg_all);
    }

    private Dictionary<string, (string Name, int Index)> GetPlaceNameMap()
    {
        var place_names = rom.GetStrings(TextName.metlist_00000);

        var scriptFolder = new FolderContainer(((FolderContainer)rom.GetFile(GameFile.StoryText)).FilePath!);
        scriptFolder.Initialize();

        var locationNames = scriptFolder.GetFileData("place_name.tbl");
        var ahtb = new AHTB(locationNames);
        return ahtb.ToIndexedDictionary(place_names);
    }

    private static IReadOnlyList<string> GetLocationBoundLines(string areaName,
        IReadOnlyDictionary<string, (string Name, int Index)> map,
        IEnumerable<PlacementLocation> locations)
    {
        var result = new List<string> { $"Area: {areaName}" };
        foreach (var location in locations)
        {
            string line = $"\t{location}";
            if (location.IsNamedPlace)
            {
                var (name, index) = map[location.PlaceName];
                line += $" // {index}, \"{name}\"";
            }
            result.Add(line);
        }
        return result;
    }

    private static IReadOnlyList<string> GetSpawnLines(string areaName,
        IReadOnlyDictionary<string, (string Name, int Index)> map,
        IEnumerable<PlacementSpawner> spawners,
        IList<PlacementLocation> locations)
    {
        var result = new List<string> { $"Area: {areaName}" };
        foreach (var spawner in spawners)
        {
            var contained = GetNearbyLocationNames(spawner, locations, map);
            result.Add($"\t{spawner} // Containing Locations: {contained}");
        }
        return result;
    }

    private static IReadOnlyList<string> GetMikarugeLines(string areaName,
        IReadOnlyDictionary<string, (string Name, int Index)> map,
        IEnumerable<PlacementMkrgEntry> mkrgs,
        IList<PlacementLocation> locations)
    {
        var result = new List<string> { $"Area: {areaName}" };
        foreach (var mkrg in mkrgs)
        {
            var contained = GetNearbyLocationNames(mkrg, locations, map);
            result.Add($"\t{mkrg} // Containing Locations: {contained}");
        }
        return result;
    }

    private static IReadOnlyList<string> GetSearchItemLines(string areaName,
        IReadOnlyDictionary<string, (string Name, int Index)> map,
        IEnumerable<PlacementSearchItem> mkrgs,
        IList<PlacementLocation> locations)
    {
        var result = new List<string> { $"Area: {areaName}" };
        foreach (var psi in mkrgs)
        {
            var contained = GetNearbyLocationNames(psi, locations, map);
            result.Add($"\t{psi} // Containing Locations: {contained}");
        }
        return result;
    }

    private static string GetNearbyLocationNames(PlacementSpawner spawner,
        IList<PlacementLocation> locations,
        IReadOnlyDictionary<string, (string Name, int Index)> map)
    {
        var containedBy = spawner.GetContainingLocations(locations);
        var placeNames = containedBy.Select(z => z.PlaceName).Distinct();
        var localized = placeNames.Select(pn => map[pn].Name);
        return string.Join(", ", localized);
    }

    private static string GetNearbyLocationNames(PlacementSearchItem mkrg,
        IList<PlacementLocation> locations,
        IReadOnlyDictionary<string, (string Name, int Index)> map)
    {
        var containedBy = mkrg.GetContainingLocations(locations);
        var placeNames = containedBy.Select(z => z.PlaceName).Distinct();
        var localized = placeNames.Select(pn => map[pn].Name);
        return string.Join(", ", localized);
    }

    private static string GetNearbyLocationNames(PlacementMkrgEntry mkrg,
        IList<PlacementLocation> locations,
        IReadOnlyDictionary<string, (string Name, int Index)> map)
    {
        var containedBy = mkrg.GetContainingLocations(locations);
        var placeNames = containedBy.Select(z => z.PlaceName).Distinct();
        var localized = placeNames.Select(pn => map[pn].Name);
        return string.Join(", ", localized);
    }

    private void DumpVisualizationData(AreaInstance area)
    {
        var vis_lines = new List<string> { "LOCS = [" };

        const string folder = "placementVis";
        Directory.CreateDirectory(GetPath(folder));

        foreach (var loc in area.Locations)
        {
            if (!loc.IsNamedPlace)
                continue;
            vis_lines.Add($"({loc.Parameters.Coordinates.ToTriple()}, {loc.Parameters.Rotation.ToTriple()}, {loc.ShapeSummary.Replace("/* Shape = */ ", "")}, {loc.SizeX}, {loc.SizeY}, {loc.SizeZ}, \"{loc.PlaceName}\"), ");
        }

        vis_lines.Add("]");
        vis_lines.Add(string.Empty);

        vis_lines.Add("SPAWNERS = [");
        foreach (var spwn in area.Spawners)
            vis_lines.Add($"({spwn.Parameters.Coordinates.ToTriple()}, {spwn.Scalar}),");
        vis_lines.Add("]");
        vis_lines.Add(string.Empty);

        vis_lines.Add("WH_SPAWNERS = [");
        foreach (var spwn in area.Wormholes)
            vis_lines.Add($"({spwn.Parameters.Coordinates.ToTriple()}, {spwn.Scalar}),");

        vis_lines.Add("LANDMARKS = [");
        foreach (var spwn in area.LandMarks)
            vis_lines.Add($"({spwn.Parameters.Coordinates.ToTriple()}, {spwn.Scalar}),");

        vis_lines.Add("]");
        vis_lines.Add(string.Empty);

        File.WriteAllLines(GetPath(folder, $"Vis_{area.AreaName}.txt"), vis_lines);
    }

    public void DumpOutbreak()
    {
        var file = rom.GetFile(GameFile.Outbreak).FilePath;
        var result = FlatDumper.GetTable<MassOutbreakTable, MassOutbreak>(file!, z => z.Table);
        File.WriteAllText(GetPath("massOutbreak.txt"), result);

        var arr = FlatBufferConverter.DeserializeFrom<MassOutbreakTable>(file!).Table;
        var cache = new DataCache<MassOutbreak>(arr!);
        var names = Enumerable.Range(0, cache.Length).Select(z => $"{z}").ToArray();
        var form = new GenericEditor<MassOutbreak>(cache, names, "Outbreak");
        form.ShowDialog();
    }

    public void DumpDex()
    {
        DumpDexSummarySpecies();

        var dexResearchPath = Path.Combine(rom.PathRomFS, "bin", "appli", "pokedex", "res_table", "pokedex_research_task_table.bin");
        var dexResearchBin = File.ReadAllBytes(dexResearchPath);
        var dexResearch = FlatBufferConverter.DeserializeFrom<PokedexResearchTable>(dexResearchBin);

        var csv = GetPath("pokedex_research.csv");
        File.WriteAllText(csv, TableUtil.GetTable(dexResearch.Table));

        // Pokedex Research for PKHeX
        DumpResearchTasks(dexResearch);
    }

    private void DumpResearchTasks(PokedexResearchTable dexResearch)
    {
        var pt = new PersonalTable8LA(rom.GetFile(GameFile.PersonalStats));
        var result = new byte[pt.Table.Max(p => p.DexIndexRegional)][];
        for (int i = 0; i < result.Length; i++)
            result[i] = [];

        ushort GetDexIndex(ushort species)
        {
            var formCount = pt.GetFormEntry(species, 0).FormCount;
            for (byte form = 0; form < formCount; form++)
            {
                var p = (IPersonalInfoPLA)pt.GetFormEntry(species, form);

                if (p.DexIndexRegional != 0)
                    return p.DexIndexRegional;
            }

            return 0;
        }

        for (ushort species = 0; species <= 980; species++)
        {
            var entries = dexResearch.Table.Where(z => z.Species == species).ToList();
            if (entries.Count == 0)
                continue;

            var dexInd = GetDexIndex(species);
            if (dexInd == 0)
                throw new ArgumentException($"Research tasks exist for species {species} not in dex?");

            if (result[dexInd - 1].Length != 0)
                throw new ArgumentException($"Two species share dex index {dexInd}?");

            using var ms = new MemoryStream();
            using var br = new BinaryWriter(ms);

            var moveTaskIndex = 0;
            var defeatTypeTaskIndex = 0;

            foreach (var task in entries)
            {
                var type = task.MoveType;
                var timeOfDay = task.TimeOfDay;

                var curMultiIndex = 0xFF;

                if (task.TaskType == ResearchTaskType.MoveTask)
                {
                    curMultiIndex = moveTaskIndex;
                    moveTaskIndex++;
                }

                if (task.TaskType == ResearchTaskType.DefeatTask)
                {
                    curMultiIndex = defeatTypeTaskIndex;
                    defeatTypeTaskIndex++;
                }
                else
                {
                    type = 18;
                }

                if (task.TaskType != ResearchTaskType.Unknown_10)
                {
                    timeOfDay = 5;
                }

                var thresholds =
                    new[] { task.Threshold1, task.Threshold2, task.Threshold3, task.Threshold4, task.Threshold5 }
                        .Where(e => e != 0).ToArray();

                // 00: u8 task
                // 01: u8 points_single
                // 02: u8 points_bonus
                // 03: u8 threshold
                // 04: u16 move
                // 06: u8 type
                // 07: u8 time of day
                // 08: u64 hash_06
                // 10: u64 hash_07
                // 18: u64 hash_08
                // 20: u8 num_thresholds
                // 21: u8 thresholds[5]
                // 26: u8 required
                // 27: u8 multi_index

                br.Write((byte)task.TaskType);
                br.Write((byte)task.PointsSingle);
                br.Write((byte)task.PointsBonus);
                br.Write((byte)task.Threshold);
                br.Write((ushort)task.Move);
                br.Write((byte)type);
                br.Write((byte)timeOfDay);
                br.Write(task.Hash06);
                br.Write(task.Hash07);
                br.Write(task.Hash08);

                br.Write((byte)thresholds.Length);
                for (var i = 0; i < 5; i++)
                {
                    if (i < thresholds.Length)
                        br.Write((byte)thresholds[i]);
                    else
                        br.Write((byte)0);
                }

                br.Write((byte)(task.RequiredForCompletion ? 1 : 0));
                br.Write((byte)curMultiIndex);
            }

            result[dexInd - 1] = ms.ToArray();
        }

        var mini = MiniUtil.PackMini(result, "la");
        var bin = GetPath(Path.Combine("bin", "researchtask_la.pkl"));
        File.WriteAllBytes(bin, mini);
    }

    private void DumpDexSummarySpecies()
    {
        var pt = new PersonalTable8LA(rom.GetFile(GameFile.PersonalStats));
        var s = rom.GetStrings(TextName.SpeciesNames);

        var dex = new List<string>();
        var dexit = new List<string>();
        var foreign = new List<string>();
        for (int i = 1; i < pt.Table.Length; i++)
        {
            var p = (IPersonalInfoPLA)pt[i];
            bool any = false;
            var specForm = $"{p.DexIndexNational:000}\t{p.Form}\t{s[p.DexIndexNational]}{(p.Form == 0 ? "" : $"-{p.Form:00}")}";
            if (p.DexIndexRegional != 0)
            {
                dex.Add($"{p.DexIndexRegional:000}\t{specForm}");
                any = true;
            }

            if (!p.IsPresentInGame)
                dexit.Add(specForm);
            else if (!any)
                foreign.Add(specForm);
        }

        var path = GetPath("Dex.txt");
        File.WriteAllLines(path, dex.Order());

        var path4 = GetPath("Dexit.txt");
        File.WriteAllLines(path4, dexit);

        var path5 = GetPath("Foreign.txt");
        File.WriteAllLines(path5, foreign);
    }

    public void DumpFlavorText()
    {
        IEnumerable<string> Zip(TextName name, TextName flavor)
        {
            var n = rom.GetStrings(name);
            var f = rom.GetStrings(flavor);
            return n.Select((z, i) => $"{z}\t{f[i].Replace("\\n", " ")}");
        }

        var p1 = GetPath("ItemFlavor.txt");
        var p2 = GetPath("MoveFlavor.txt");
        var p3 = GetPath("AbilityFlavor.txt");

        var l1 = Zip(TextName.ItemNames, TextName.ItemFlavor);
        var l2 = Zip(TextName.MoveNames, TextName.MoveFlavor);
        var l3 = Zip(TextName.AbilityNames, TextName.AbilityFlavor);

        File.WriteAllLines(p1, l1);
        File.WriteAllLines(p2, l2);
        File.WriteAllLines(p3, l3);
    }

    private static readonly string[] LanguageCodes = ["ja", "en", "fr", "it", "de", "es", "ko", "zh", "zh2"];

    private static readonly string[] LanguageNames =
    [
        "カタカナ",
        "漢字",
        "English",
        "Français",
        "Italiano",
        "Deutsch",
        "Español",
        "한국",
        "汉字简化方案",
        "漢字簡化方案",
    ];

    private void ChangeLanguage(int index)
    {
        rom.Language = index;
        GamePath.Initialize(rom.Game, rom.Language);
    }

    public void DumpStrings()
    {
        int lang = rom.Language;
        var indexes = new[] { 0, 2, 3, 4, 5, 6, 7, 8, 9 };
        for (int i = 0; i < indexes.Length; i++)
        {
            var code = LanguageCodes[i];
            var name = LanguageNames[i];
            var index = indexes[i];

            DumpStrings(code, name, index);
        }
        ChangeLanguage(lang);
    }

    private void DumpStrings(string code, string name, int index)
    {
        Console.WriteLine($"Dumping strings for {name}.");
        ChangeLanguage(index);

        DumpStringSet(TextName.MoveNames, "Moves");
        DumpStringSet(TextName.ItemNames, "Items");
        DumpStringSet(TextName.SpeciesNames, "Species");
        DumpStringSet(TextName.AbilityNames, "Abilities");
        DumpStringSet(TextName.metlist_00000, "la_00000");
        DumpStringSet(TextName.metlist_30000, "la_30000");
        DumpStringSet(TextName.metlist_40000, "la_40000");
        DumpStringSet(TextName.metlist_60000, "la_60000");
        DumpStringSet(TextName.Forms, "forms");

        void DumpStringSet(TextName t, string file)
        {
            var strings = rom.GetStrings(t);
            var fn = $"text_{file}_{code}.txt";

            var folder = Path.Combine(code, fn);

            var path = GetPath(folder);
            File.WriteAllLines(path, strings);
        }
    }

    public void DumpScriptID()
    {
        var file = Path.Combine(rom.PathRomFS, "bin", "event", "script_id_record_release.bin");
        var text = FlatDumper.GetTable<ScriptIDRecordRelease, ScriptIDRecord>(file, z => z.Table);
        var path = GetPath("scriptCommands.txt");
        File.WriteAllText(path, text);
    }

    public void DumpEventTriggers()
    {
        var eventTriggerDir = Path.Combine(rom.PathRomFS, "bin", "event", "event_progress", "trigger");
        var eventTriggerFiles = Directory.EnumerateFiles(eventTriggerDir, "*", SearchOption.AllDirectories).Where(p => Path.GetExtension(p) == ".bin");

        const string outFolder = "event_trigger";
        Directory.CreateDirectory(GetPath(outFolder));

        var unknownTriggers = new List<ulong>();
        var unknownConditions = new List<ulong>();
        var unknownCommands = new List<ulong>();

        var allLines = new List<string>();

        foreach (var f in eventTriggerFiles)
        {
            if (Path.GetFileName(f) == "trigger_preset.bin")
                continue;

            var table = FlatBufferConverter.DeserializeFrom<TriggerTable>(f);

            var curLines = new List<string> { $"File: {Path.GetFileName(f)}" };

            foreach (var line in pkNX.Structures.FlatBuffers.Arceus.TriggerUtil.GetTriggerTableSummary(table))
                curLines.Add($"\t{line}");

            File.WriteAllLines(GetPath(outFolder, $"trigger_{Path.GetFileNameWithoutExtension(f).Replace("trigger_", string.Empty)}.txt"), curLines);

            allLines.AddRange(curLines);
            allLines.Add(string.Empty);
        }

        File.WriteAllLines(GetPath(outFolder, "triggerAll.txt"), allLines);

        File.WriteAllLines(GetPath(outFolder, "triggerUnknownTypes.txt"), unknownTriggers.Order().Select(x => $"0x{x:X16},"));
        File.WriteAllLines(GetPath(outFolder, "triggerUnknownConditions.txt"), unknownConditions.Order().Select(x => $"0x{x:X16},"));
        File.WriteAllLines(GetPath(outFolder, "triggerUnknownCommands.txt"), unknownCommands.Order().Select(x => $"0x{x:X16},"));
    }

    public void DumpMoveShop()
    {
        var file = rom.GetFile(GameFile.MoveShop).FilePath;
        var result = FlatDumper.GetTable<MoveShopTable, MoveShopIndex>(file!, z => z.Table);
        File.WriteAllText(GetPath("MoveShop.csv"), result);
    }

    public void DumpGinkgoRareShop()
    {
        DumpGinkgoRareItems();
        DumpGinkgoRareItemTables();
    }

    public void DumpGinkgoRareItems()
    {
        const string outFolder = "ginkgo_item";
        var file = Path.Combine(rom.PathRomFS, "bin", "event", "event_progress", "ginkgo", "ginkgo_item.bin");
        var itemBin = File.ReadAllBytes(file);
        var itemData = FlatBufferConverter.DeserializeFrom<GinkgoRareItemList>(itemBin);

        var curLines = new List<string> { "ItemID\tQuantity\tCost\tFlag\tInventoryHash\tField1" };
        foreach (var item in itemData.Table)
            curLines.Add($"{item.Details[0].ItemID}\t{item.Details[0].Quantity}\t{item.Cost}\t{item.Flag}\t{item.InventoryHash:x16}\t{item.Field1:x16}");
        File.WriteAllLines(GetPath(outFolder, "ginkgo_item.txt"), curLines);
    }

    public void DumpGinkgoRareItemTables()
    {
        const string outFolder = "ginkgo_item";
        var file = Path.Combine(rom.PathRomFS, "bin", "event", "event_progress", "ginkgo", "ginkgo_item_table.bin");
        var itemBin = File.ReadAllBytes(file);
        var itemData = FlatBufferConverter.DeserializeFrom<GinkgoTableList>(itemBin);

        var curLines = new List<string>();
        var count = 0;

        // The game has 8 tables for different story progress points.
        foreach (var table in itemData.Table)
        {
            curLines.Add($"Table {++count}: {table.Field0}");
            curLines.Add("Progress Requirement:");
            var prog = table.ProgressTable[0]; // Only the first one is used for anything.
            curLines.Add($"\t{prog.InventoryHash:x16}\t{prog.Field1:x16}\t{prog.BlockName}\t{prog.MinReq}\t{prog.MaxReq}");

            curLines.Add("Weight Table:");
            curLines.Add("InventoryHash\tWeight\tField2\tField3\tFlag\tField5\tField6");
            foreach (var weight in table.WeightTable)
                curLines.Add($"\t{weight.InventoryHash:x16}\t{weight.Weight}\t{weight.Field2:x16}\t{weight.Field3:x16}\t{weight.Flag}\t{weight.Field5}\t{weight.Field6}");
            curLines.Add("");
        }

        File.WriteAllLines(GetPath(outFolder, "ginkgo_item_table.txt"), curLines);
    }
}
