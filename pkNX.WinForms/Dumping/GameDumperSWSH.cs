using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using pkNX.Containers;
using pkNX.Game;
using pkNX.Structures;
using pkNX.Structures.FlatBuffers;

// ReSharper disable StringLiteralTypo

namespace pkNX.WinForms
{
    public class GameDumperSWSH
    {
        private readonly GameManagerSWSH ROM;
        public GameDumperSWSH(GameManagerSWSH rom) => ROM = rom;
        public string DumpFolder => Path.Combine(Directory.GetParent(ROM.PathRomFS).FullName, "Dump");

        private string GetPath(string path)
        {
            Directory.CreateDirectory(DumpFolder);
            var result = Path.Combine(DumpFolder, path);
            Directory.CreateDirectory(Directory.GetParent(result).FullName); // double check :(
            return result;
        }

        private string GetPath(string parent, string path)
        {
            Directory.CreateDirectory(DumpFolder);
            var result = Path.Combine(DumpFolder, parent, path);
            Directory.CreateDirectory(Directory.GetParent(result).FullName); // double check :(
            return result;
        }

        public void DumpDummiedMoves()
        {
            // get dummied moves
            var moveNames = ROM.GetStrings(TextName.MoveNames);
            var moveDesc = ROM.GetStrings(TextName.MoveFlavor);

            var dummy = moveDesc[237]; // hidden power is kill
            var tuple = moveNames
                .Select((z, i) => new { Name = z, Desc = moveDesc[i], Index = i })
                .Where(z => z.Desc == dummy)
                .Select(z => $"{z.Index:000}\t{z.Name}");

            var path = GetPath("snappedMoves.txt");
            File.WriteAllLines(path, tuple); // rest in peace
        }

        public void DumpPokeInfo()
        {
            var s = ROM.GetStrings(TextName.SpeciesNames);

            var eggdata = ROM.GetFilteredFolder(GameFile.EggMoves);
            var egg = EggMoves7.GetArray(eggdata.GetFiles().Result);

            var pt = ROM.Data.PersonalData;
            var altForms = pt.GetFormList(s, pt.MaxSpeciesID);
            var entryNames = pt.GetPersonalEntryList(altForms, s, pt.MaxSpeciesID, out _, out _);
            var moveNames = ROM.GetStrings(TextName.MoveNames);

            var pd = new PersonalDumperSWSH
            {
                Colors = Enum.GetNames(typeof(PokeColor)),
                EggGroups = Enum.GetNames(typeof(EggGroup)),
                EntryEggMoves = egg,
                EntryLearnsets = ROM.Data.LevelUpData.LoadAll(),
                EntryNames = entryNames,
                ExpGroups = Enum.GetNames(typeof(EXPGroup)),
                Evos = ROM.Data.EvolutionData.LoadAll(),

                Abilities = ROM.GetStrings(TextName.AbilityNames),
                Items = ROM.GetStrings(TextName.ItemNames),
                Moves = moveNames,
                Types = ROM.GetStrings(TextName.Types),
                Species = ROM.GetStrings(TextName.SpeciesNames),
                ZukanA = ROM.GetStrings(TextName.PokedexEntry1),
                ZukanB = ROM.GetStrings(TextName.PokedexEntry2),
                TMIndexes = Legal.TMHM_SWSH,
            };

            var result = pd.Dump(pt);

            var outname = GetPath("Pokemon.txt");
            File.WriteAllLines(outname, result);

            var learnTable = pd.MoveSpeciesLearn;
            var outLearn = GetPath("MovePerPokemon.txt");
            var moveLines = learnTable.Select((z, i) => $"{i:000}\t{moveNames[i]}\t{string.Join(", ", z.Distinct())}");
            File.WriteAllLines(outLearn, moveLines);
        }

        private static readonly string[] ahtbext = { ".tbl", ".hsh" };

        public void DumpAHTB()
        {
            var files = Directory.EnumerateFiles(ROM.PathRomFS, "*", SearchOption.AllDirectories)
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

            var paks = Directory.EnumerateFiles(ROM.PathRomFS, "*", SearchOption.AllDirectories)
                .Where(z => Path.GetExtension(z) == ".gfpak");
            foreach (var f in paks)
            {
                var pak = new GFPack(f);
                foreach (var bytes in pak.DecompressedFiles)
                {
                    if (AHTB.IsAHTB(bytes))
                    {
                        var tbl = new AHTB(bytes);
                        var summaries = tbl.Summary;
                        foreach (var t in tbl.ShortSummary)
                            result.Add(t);
                        list.Add(Path.GetFileName(f));
                        list.AddRange(summaries);
                    }
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

        public static Dictionary<ulong, string> ReadAHTB(byte[] bytes)
        {
            if (!AHTB.IsAHTB(bytes))
                throw new ArgumentException();

            var tbl = new AHTB(bytes);
            return tbl.ToDictionary();
        }

        private TrainerEditor GetTrainerEditor()
        {
            var editor = new TrainerEditor
            {
                ReadClass = data => new TrainerClass8(data),
                ReadPoke = data => new TrainerPoke8(data),
                ReadTrainer = data => new TrainerData8(data),
                ReadTeam = TrainerPoke8.ReadTeam,
                WriteTeam = TrainerPoke8.WriteTeam,
                TrainerData = ROM.GetFilteredFolder(GameFile.TrainerData),
                TrainerPoke = ROM.GetFilteredFolder(GameFile.TrainerPoke),
                TrainerClass = ROM.GetFilteredFolder(GameFile.TrainerClass),
            };
            editor.Initialize();
            return editor;
        }

        public void DumpTrainers()
        {
            var trc = ROM.GetStrings(TextName.TrainerClasses);
            var trn = ROM.GetStrings(TextName.TrainerNames);
            var m = ROM.GetStrings(TextName.MoveNames);
            var s = ROM.GetStrings(TextName.SpeciesNames);

            var tr = GetTrainerEditor();
            var result = new List<string>();
            for (int i = 0; i < tr.Length; i++)
            {
                var t = tr[i];

                // some battles with Avery and Klara have out of bounds trclasses -- set back to "Pokémon Trainer"
                if (t.Self.Class > trc.Length)
                    t.Self.Class = 1;

                result.Add($"{i:000} - {trc[t.Self.Class]}: {trn[i]}");
                const int MoneyScalar = 80;
                result.Add($"AI: {t.Self.AI} | Mode: {t.Self.Mode} | Money: {t.Self.Money * MoneyScalar}");
                result.Add($"Pokémon Count: {t.Self.NumPokemon}");

                result.Add("---");
                for (int j = 0; j < t.Team.Count; j++)
                {
                    IEnumerable<string> moves = t.Team[j].Moves.Where(z => z != 0).Select(z => m[z]).ToArray();
                    if (!moves.Any()) moves = new[] { "Default Level Up" };
                    int form = t.Team[j].Form;
                    var formstr = form != 0 ? $"-{form}" : "";
                    var str = $"{j + 1}: Lv{t.Team[j].Level:00} {s[t.Team[j].Species]}{formstr} : {string.Join(" / ", moves)}";
                    result.Add(str);
                }
                result.Add("---");

                result.Add("=========");
                result.Add("");
            }

            var outname = GetPath("trparse.txt");
            File.WriteAllLines(outname, result);
        }

        public void DumpBattleTower()
        {
            var pk_table_path = ROM.GetFile(GameFile.FacilityPokeNormal)[0];
            var tr_table_path = ROM.GetFile(GameFile.FacilityTrainerNormal)[0];
            var pokes = FlatBufferConverter.DeserializeFrom<BattleTowerPoke8Archive>(pk_table_path);
            var trainers = FlatBufferConverter.DeserializeFrom<BattleTowerTrainer8Archive>(tr_table_path);

            var pk = TableUtil.GetTable(pokes.Table);
            var tr = TableUtil.GetTable(trainers.Entries);

            File.WriteAllText(GetPath("towerPoke.txt"), pk);
            File.WriteAllText(GetPath("towerTrainer.txt"), tr);
        }

        public void DumpItems()
        {
            var items = ROM.GetFile(GameFile.ItemStats);
            var file = items[0];
            var array = Item8.GetArray(file);
            var groups = array.GroupBy(z => z.Pouch);
            foreach (var g in groups)
            {
                var key = g.Key;
                var IDs = g.Where(z => z.ItemSprite >= 0).Select(z => z.ItemID);
                var path = GetPath($"{key}.txt");
                File.WriteAllText(path, string.Join(", ", IDs.Select(z => $"{z:000}")));
            }

            var names = ROM.GetStrings(TextName.ItemNames);
            var lines = TableUtil.GetNamedTypeTable(array, names, "Items");
            var table = GetPath("ItemData.txt");
            var bin = GetPath("ItemData.bin");
            File.WriteAllText(table, lines);
            File.WriteAllBytes(bin, array.SelectMany(z => z.Data).ToArray());
        }

        public void DumpMoves()
        {
            var dir = Path.Combine(ROM.PathRomFS, "bin", "pml", "waza");
            var files = Directory.GetFiles(dir);
            var moves = FlatBufferConverter.DeserializeFrom<Waza8>(files);
            var names = ROM.GetStrings(TextName.MoveNames);
            var lines = TableUtil.GetNamedTypeTable(moves, names, "Moves");
            var table = GetPath("MoveData.txt");
            File.WriteAllText(table, lines);
        }

        public void DumpLearnsetBinary()
        {
            var data = ROM.Data.LevelUpData.LoadAll()
                .Cast<Learnset8>().Select(z => z.WriteAsLearn6()).ToArray();
            var mini = MiniUtil.PackMini(data, "ss");
            var bin = GetPath(Path.Combine("bin", "lvlmove_swsh.pkl"));
            File.WriteAllBytes(bin, mini);
        }

        public void DumpEggBinary()
        {
            // format matches past gen and PKHeX's expected format
            var data = ROM.GetFilteredFolder(GameFile.EggMoves).GetFiles().Result;
            var mini = MiniUtil.PackMini(data, "ss");
            var bin = GetPath(Path.Combine("bin", "eggmove_swsh.pkl"));
            File.WriteAllBytes(bin, mini);
        }

        public void DumpEvolutionBinary()
        {
            // format matches past gen and PKHeX's expected format
            var data = ROM.GetFilteredFolder(GameFile.Evolutions).GetFiles().Result;
            var mini = MiniUtil.PackMini(data, "ss");
            var bin = GetPath(Path.Combine("bin", "evos_ss.pkl"));
            File.WriteAllBytes(bin, mini);
        }

        public void DumpGifts()
        {
            var speciesNames = ROM.GetStrings(TextName.SpeciesNames);
            var data = ROM.GetFile(GameFile.EncounterGift)[0];
            var gifts = FlatBufferConverter.DeserializeFrom<EncounterGift8Archive>(data);
            var table = TableUtil.GetTable(gifts.Table);
            var fn = GetPath("Gifts.txt");
            File.WriteAllText(fn, table);

            var f2 = GetPath("GiftsPKHeX.txt");
            File.WriteAllLines(f2, gifts.Table.Select(z => z.GetSummary(speciesNames)));
        }

        public void DumpStatic()
        {
            var speciesNames = ROM.GetStrings(TextName.SpeciesNames);
            var data = ROM.GetFile(GameFile.EncounterStatic)[0];
            var statics = FlatBufferConverter.DeserializeFrom<EncounterStatic8Archive>(data);
            var table = TableUtil.GetTable(statics.Table);
            var fn = GetPath("StaticEncounters.txt");
            File.WriteAllText(fn, table);

            var f2 = GetPath("StaticEncountersPKHeX.txt");
            File.WriteAllLines(f2, statics.Table.Select(z => z.GetSummary(speciesNames)));
        }

        public void DumpWilds()
        {
            var wildpak = ROM.GetFile(GameFile.NestData)[0];
            var data_table = new GFPack(wildpak);
            var encount_sw = FlatBufferConverter.DeserializeFrom<EncounterArchive8>(data_table.GetDataFileName("encount_k.bin"));
            var encount_symbol_sw = FlatBufferConverter.DeserializeFrom<EncounterArchive8>(data_table.GetDataFileName("encount_symbol_k.bin"));
            var encount_sh = FlatBufferConverter.DeserializeFrom<EncounterArchive8>(data_table.GetDataFileName("encount_t.bin"));
            var encount_symbol_sh = FlatBufferConverter.DeserializeFrom<EncounterArchive8>(data_table.GetDataFileName("encount_symbol_t.bin"));

            var species = ROM.GetStrings(TextName.SpeciesNames);
            var zones = SWSHInfo.Zones;
            var subtables = Enum.GetNames(typeof(SWSHEncounterType)).Select(z => z.Replace("_", " ")).ToArray();

            File.WriteAllLines(GetPath("Encounters_Sword.txt"), EncounterTable8Util.GetLines(encount_sw, zones, subtables, species));
            File.WriteAllLines(GetPath("Encounters_Symbol_Sword.txt"), EncounterTable8Util.GetLines(encount_symbol_sw, zones, subtables, species));
            File.WriteAllLines(GetPath("Encounters_Shield.txt"), EncounterTable8Util.GetLines(encount_sh, zones, subtables, species));
            File.WriteAllLines(GetPath("Encounters_Symbol_Shield.txt"), EncounterTable8Util.GetLines(encount_symbol_sh, zones, subtables, species));

            File.WriteAllBytes(GetPath("encounter_sw_hidden.pkl"), MiniUtil.PackMini(EncounterTable8Util.GetBytes(SWSHInfo.ZoneLocations, SWSHInfo.ZoneType, encount_sw, true), "sw"));
            File.WriteAllBytes(GetPath("encounter_sh_hidden.pkl"), MiniUtil.PackMini(EncounterTable8Util.GetBytes(SWSHInfo.ZoneLocations, SWSHInfo.ZoneType, encount_sh, true), "sh"));
            File.WriteAllBytes(GetPath("encounter_sw_symbol.pkl"), MiniUtil.PackMini(EncounterTable8Util.GetBytes(SWSHInfo.ZoneLocations, SWSHInfo.ZoneType, encount_symbol_sw), "sw"));
            File.WriteAllBytes(GetPath("encounter_sh_symbol.pkl"), MiniUtil.PackMini(EncounterTable8Util.GetBytes(SWSHInfo.ZoneLocations, SWSHInfo.ZoneType, encount_symbol_sh), "sh"));
        }

        public void DumpPlacement()
        {
            var statics = FlatBufferConverter.DeserializeFrom<EncounterStatic8Archive>(ROM.GetFile(GameFile.EncounterStatic)[0]);
            var placement = new GFPack(ROM.GetFile(GameFile.Placement)[0]);
            var species_names = ROM.GetStrings(TextName.SpeciesNames);
            var weathers = Enum.GetNames(typeof(SWSHEncounterType)).Select(z => z.Replace("_", " ")).ToArray();
            var area_names = new AHTB(placement.GetDataFileName("AreaNameHashTable.tbl")).ToDictionary();
            var zone_names = new AHTB(placement.GetDataFileName("ZoneNameHashTable.tbl")).ToDictionary();
            var zone_descs = SWSHInfo.Zones;
            var obj_names = new AHTB(placement.GetDataFileName("ObjectNameHashTable.tbl")).ToDictionary();
          //var vanish_flags = new AHTB(placement.GetDataFileName("VanishFlagAutoTable.tbl")).ToDictionary();
          //var x = placement.GetDataFileName("template_data.bin"); // flatbuffer skybox
            var wild_area = FlatBufferConverter.DeserializeFrom<PlacementArea8Archive>(placement.GetDataFileName("a_wr0101.bin"));
            var isle_of_armor = FlatBufferConverter.DeserializeFrom<PlacementArea8Archive>(placement.GetDataFileName("a_wr0201.bin"));
            var crown_tundra = FlatBufferConverter.DeserializeFrom<PlacementArea8Archive>(placement.GetDataFileName("a_wr0301.bin"));

            File.WriteAllLines(GetPath("Placement_WildArea.txt"), wild_area.Table.SelectMany(z => z.GetSummary(statics.Table, species_names, zone_names, zone_descs, obj_names, weathers)));
            File.WriteAllLines(GetPath("Placement_IsleOfArmor.txt"), isle_of_armor.Table.SelectMany(z => z.GetSummary(statics.Table, species_names, zone_names, zone_descs, obj_names, weathers)));
            File.WriteAllLines(GetPath("Placement_CrownTundra.txt"), crown_tundra.Table.SelectMany(z => z.GetSummary(statics.Table, species_names, zone_names, zone_descs, obj_names, weathers)));

            var placement_all = new List<string>();
            foreach (var area in area_names)
            {
                var areaName = area.Value;
                var fileName = $"{areaName}.bin";
                if (placement.GetIndexFileName(fileName) < 0)
                    continue;

                placement_all.Add("==================================");
                placement_all.Add(areaName);
                placement_all.Add("==================================");
                placement_all.Add(string.Empty);

                var bin = placement.GetDataFileName(fileName);
                var data = FlatBufferConverter.DeserializeFrom<PlacementArea8Archive>(bin);
                placement_all.AddRange(data.Table.SelectMany(z => z.GetSummary(statics.Table, species_names, zone_names, zone_descs, obj_names, weathers)));
                placement_all.Add(string.Empty);

                File.WriteAllBytes(GetPath("areas", fileName), bin);
            }

            File.WriteAllLines(GetPath("Placement_all.txt"), placement_all);
        }

        public void DumpNestEntries()
        {
            var speciesNames = ROM.GetStrings(TextName.SpeciesNames);
            var itemNames = ROM.GetStrings(TextName.ItemNames);
            var moveNames = ROM.GetStrings(TextName.MoveNames);
            var wildpak = ROM.GetFile(GameFile.NestData)[0];
            var data_table = new GFPack(wildpak);
            var nest_encounts = FlatBufferConverter.DeserializeFrom<EncounterNest8Archive>(data_table.GetDataFileName("nest_hole_encount.bin"));
            //  var nest_levels = FlatBufferConverter.DeserializeFrom<NestHoleLevel8Archive>(data_table.GetDataFileName("nest_hole_level.bin"));
            var nest_drops = FlatBufferConverter.DeserializeFrom<NestHoleReward8Archive>(data_table.GetDataFileName("nest_hole_drop_rewards.bin"));
            var nest_bonus = FlatBufferConverter.DeserializeFrom<NestHoleReward8Archive>(data_table.GetDataFileName("nest_hole_bonus_rewards.bin"));

            string[][] nestHex = new string[2][];
            foreach (var game in new[] { 1, 2 })
            {
                var tables = nest_encounts.Table.Where(z => z.GameVersion == game).ToList();
                var entries = tables.Select((_, x) => $"private const int Nest{x:000} = {x + 100_000};");
                var encounters = tables.SelectMany((z, x) => z.GetSummary(speciesNames, x)).ToArray();

                var path1 = GetPath($"nestHex{game}.txt");
                File.WriteAllLines(path1, entries.Concat(encounters));
                nestHex[game - 1] = encounters;

                var result = tables.Select(z => z.GetSummarySimple());
                var path2 = GetPath($"nest{game}.txt");
                File.WriteAllLines(path2, result);
            }

            var common = nestHex[0].Intersect(nestHex[1]).Where(z => z.StartsWith(" ")).Distinct();
            var sword = nestHex[0].Where(z => !nestHex[1].Contains(z)).Distinct();
            var shield = nestHex[1].Where(z => !nestHex[0].Contains(z)).Distinct();

            File.WriteAllLines(GetPath("nestCommon.txt"), common);
            File.WriteAllLines(GetPath("nestSword.txt"), sword);
            File.WriteAllLines(GetPath("nestShield.txt"), shield);

            var nest_pretty_sw = nest_encounts.Table.Where(z => z.GameVersion == 1).SelectMany((z, x) =>
                z.GetPrettySummary(speciesNames, itemNames, moveNames, Legal.TMHM_SWSH, nest_drops.Table, nest_bonus.Table, x));
            var nest_pretty_sh = nest_encounts.Table.Where(z => z.GameVersion == 2).SelectMany((z, x) =>
                z.GetPrettySummary(speciesNames, itemNames, moveNames, Legal.TMHM_SWSH, nest_drops.Table, nest_bonus.Table, x));
            File.WriteAllLines(GetPath("nestPrettySword.txt"), nest_pretty_sw);
            File.WriteAllLines(GetPath("nestPrettyShield.txt"), nest_pretty_sh);
        }

        public void DumpGalarDex()
        {
            var pt = ROM.Data.PersonalData;
            var s = ROM.GetStrings(TextName.SpeciesNames);

            var galar = new List<string>();
            var armor = new List<string>();
            var crown = new List<string>();
            var dexit = new List<string>();
            var foreign = new List<string>();
            for (int i = 1; i <= ROM.Info.MaxSpeciesID; i++)
            {
                var p = (PersonalInfoSWSH)pt[i];
                bool any = false;
                if (p.PokeDexIndex != 0)
                {
                    galar.Add($"{p.PokeDexIndex:000} - [{i:000]} - {s[i]}");
                    any = true;
                }
                if (p.ArmorDexIndex != 0)
                {
                    armor.Add($"{p.ArmorDexIndex:000} - [{i:000]} - {s[i]}");
                    any = true;
                }
                if (p.CrownDexIndex != 0)
                {
                    crown.Add($"{p.CrownDexIndex:000} - [{i:000]} - {s[i]}");
                    any = true;
                }

                if (!p.IsPresentInGame)
                    dexit.Add($"[{i:000]} - {s[i]}");
                else if (!any)
                    foreign.Add($"[{i:000]} - {s[i]}");
            }

            var path = GetPath("GalarDex.txt");
            File.WriteAllLines(path, galar.OrderBy(z => z));

            var path2 = GetPath("ArmorDex.txt");
            File.WriteAllLines(path2, armor.OrderBy(z => z));

            var path3 = GetPath("CrownDex.txt");
            File.WriteAllLines(path3, crown.OrderBy(z => z));

            var path4 = GetPath("Dexit.txt");
            File.WriteAllLines(path4, dexit);

            var path5 = GetPath("Foreign.txt");
            File.WriteAllLines(path5, foreign);
        }

        public void DumpFlavorText()
        {
            IEnumerable<string> Zip(TextName name, TextName flavor)
            {
                var n = ROM.GetStrings(name);
                var f = ROM.GetStrings(flavor);
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

        public void DumpEggEntries()
        {
            var eggdata = ROM.GetFilteredFolder(GameFile.EggMoves).GetFiles().Result;
            var egg = EggMoves7.GetArray(eggdata);
            var moves = ROM.GetStrings(TextName.MoveNames);
            var lines = egg.Select((z, i) => $"{i:000}\t{z.FormTableIndex:0000}\t{string.Join(", ", z.Moves.Select(m => moves[m]))}");
            var bin = GetPath(Path.Combine("eggEntries.txt"));
            File.WriteAllLines(bin, lines);
        }

        public void DumpMaxDens()
        {
            var file = ROM.GetFile(GameFile.DynamaxDens)[0];
            var encounters = FlatBufferConverter.DeserializeFrom<EncounterUnderground8Archive>(file);
            var lines = TableUtil.GetTable(encounters.Table);
            var table = GetPath("MaxDens.txt");
            File.WriteAllText(table, lines);

            var names = ROM.GetStrings(TextName.SpeciesNames);
            var pkhex = encounters.Table.Select(z => z.GetSummary(names));
            File.WriteAllText(GetPath("MaxDens_hex.txt"), string.Join(Environment.NewLine, pkhex));
        }

        public static byte[] GetDistributionContents(string path, out int index)
        {
            var archive = File.ReadAllBytes(path);

            // Validate Header
            if (archive.Length < 0x20 || archive.Length != 4 + BitConverter.ToInt32(archive, 0x10) || BitConverter.ToInt32(archive, 0x8) != 0x20)
                throw new ArgumentException();

            index = archive[0];

            // TODO: Eventually validate CRC16 over Data[:-4], CRC stored at Data[-4:]

            var data = new byte[archive.Length - 0x24];
            Array.Copy(archive, 0x20, data, 0, data.Length);
            return data;
        }

        public void DumpDistributionNestEntries(string path)
        {
            var speciesNames = ROM.GetStrings(TextName.SpeciesNames);
            var itemNames = ROM.GetStrings(TextName.ItemNames);
            var moveNames = ROM.GetStrings(TextName.MoveNames);
            var wildpak = ROM.GetFile(GameFile.NestData)[0];
            var data_table = new GFPack(wildpak);
            var nest_drops = FlatBufferConverter.DeserializeFrom<NestHoleReward8Archive>(data_table.GetDataFileName("nest_hole_drop_rewards.bin"));
            var nest_bonus = FlatBufferConverter.DeserializeFrom<NestHoleReward8Archive>(data_table.GetDataFileName("nest_hole_bonus_rewards.bin"));

            var dai_data = GetDistributionContents(Path.Combine(path, "dai_encount"), out int dai_index);
            var drop_data = GetDistributionContents(Path.Combine(path, "drop_rewards"), out int drop_index);
            var bonus_data = GetDistributionContents(Path.Combine(path, "bonus_rewards"), out int bonus_index);
            var dist_drops = FlatBufferConverter.DeserializeFrom<NestHoleDistributionReward8Archive>(drop_data);
            var dist_bonus = FlatBufferConverter.DeserializeFrom<NestHoleDistributionReward8Archive>(bonus_data);
            var dai_encounts = FlatBufferConverter.DeserializeFrom<NestHoleCrystalEncounter8Archive>(dai_data);

            System.Diagnostics.Debug.Assert(dai_index == drop_index && drop_index == bonus_index, "BCAT Index should be the same for all files!");

            DumpDistIfExists("normal_encount", "");
            DumpDistIfExists("normal_encount_rigel1", "Armor");
            DumpDistIfExists("normal_encount_rigel2", "Crown");

            void DumpDistIfExists(string filePath, string dest)
            {
                var p = Path.Combine(path, filePath);
                if (!File.Exists(p))
                    return;

                var data = GetDistributionContents(p, out int index);
                var encounts = FlatBufferConverter.DeserializeFrom<NestHoleDistributionEncounter8Archive>(data);
                var pretty_sw = encounts.Table.Where(z => z.GameVersion == 1).SelectMany((z, x) =>
                    z.GetPrettySummary(speciesNames, itemNames, moveNames, Legal.TMHM_SWSH, nest_drops.Table, nest_bonus.Table, dist_drops.Table, dist_bonus.Table, x));
                var pretty_sh = encounts.Table.Where(z => z.GameVersion == 2).SelectMany((z, x) =>
                    z.GetPrettySummary(speciesNames, itemNames, moveNames, Legal.TMHM_SWSH, nest_drops.Table, nest_bonus.Table, dist_drops.Table, dist_bonus.Table, x));

                File.WriteAllLines(GetPath($"nestDist{dest}PrettySword.txt"), pretty_sw);
                File.WriteAllLines(GetPath($"nestDist{dest}PrettyShield.txt"), pretty_sh);

                var dist_sw = TableUtil.GetTable(encounts.Table.Where(z => z.GameVersion == 1).SelectMany(z => z.Entries));
                var dist_sh = TableUtil.GetTable(encounts.Table.Where(z => z.GameVersion == 2).SelectMany(z => z.Entries));

                File.WriteAllText(GetPath($"nestDist{dest}_sw.txt"), dist_sw);
                File.WriteAllText(GetPath($"nestDist{dest}_sh.txt"), dist_sh);

                string[][] nestHex = new string[2][];
                foreach (var game in new[] { 1, 2 })
                {
                    var tables = encounts.Table.Where(z => z.GameVersion == game).ToList();
                    var encounters = tables.SelectMany(z => z.GetSummary(speciesNames, index)).ToArray();

                    var path1 = GetPath($"nestDistHex{game}.txt");
                    File.WriteAllLines(path1, encounters);
                    nestHex[game - 1] = encounters;

                    var result = tables.Select(z => z.GetSummarySimple());
                    var path2 = GetPath($"nestDistFormat_{game}.txt");
                    File.WriteAllLines(path2, result);
                }

                var common = nestHex[0].Intersect(nestHex[1]).Where(z => z.StartsWith(" ")).Distinct();
                var sword = nestHex[0].Where(z => !nestHex[1].Contains(z)).Distinct();
                var shield = nestHex[1].Where(z => !nestHex[0].Contains(z)).Distinct();

                File.WriteAllLines(GetPath($"hex_nestDist{dest}Common.txt"), common);
                File.WriteAllLines(GetPath($"hex_nestDist{dest}Sword.txt"), sword);
                File.WriteAllLines(GetPath($"hex_nestDist{dest}Shield.txt"), shield);
            }

            var dai_pretty_sw = dai_encounts.Table.Where(z => z.GameVersion == 1).SelectMany((z, x) =>
                z.GetPrettySummary(speciesNames, itemNames, moveNames, Legal.TMHM_SWSH, nest_drops.Table, nest_bonus.Table, dist_drops.Table, dist_bonus.Table, x));
            var dai_pretty_sh = dai_encounts.Table.Where(z => z.GameVersion == 2).SelectMany((z, x) =>
                z.GetPrettySummary(speciesNames, itemNames, moveNames, Legal.TMHM_SWSH, nest_drops.Table, nest_bonus.Table, dist_drops.Table, dist_bonus.Table, x));
            File.WriteAllLines(GetPath("nestCrystalPrettySword.txt"), dai_pretty_sw);
            File.WriteAllLines(GetPath("nestCrystalPrettyShield.txt"), dai_pretty_sh);

            var dai_sw = TableUtil.GetTable(dai_encounts.Table.Where(z => z.GameVersion == 1).SelectMany(z => z.Entries));
            var dai_sh = TableUtil.GetTable(dai_encounts.Table.Where(z => z.GameVersion == 2).SelectMany(z => z.Entries));
            File.WriteAllText(GetPath("nestCrystal_sw.txt"), dai_sw);
            File.WriteAllText(GetPath("nestCrystal_sh.txt"), dai_sh);

            DumpHexCrystal(dai_encounts, speciesNames, itemNames);
        }

        private void DumpHexCrystal(NestHoleCrystalEncounter8Archive dai_encounts, string[] speciesNames, string[] itemNames)
        {
            string[][] nestHex = new string[2][];
            foreach (var game in new[] { 1, 2 })
            {
                var tables = dai_encounts.Table.Where(z => z.GameVersion == game).ToList();
                var encounters = tables.SelectMany(z => z.GetSummary(speciesNames, itemNames)).ToArray();

                var path1 = GetPath($"nestCrystalHex{game}.txt");
                File.WriteAllLines(path1, encounters);
                nestHex[game - 1] = encounters;

                var result = tables.Select(z => z.GetSummarySimple());
                var path2 = GetPath($"nestCrystalFormat_{game}.txt");
                File.WriteAllLines(path2, result);
            }

            var common = nestHex[0].Intersect(nestHex[1]).Where(z => z.StartsWith(" ")).Distinct();
            var sword = nestHex[0].Where(z => !nestHex[1].Contains(z)).Distinct();
            var shield = nestHex[1].Where(z => !nestHex[0].Contains(z)).Distinct();

            File.WriteAllLines(GetPath("hex_nestCrystalCommon.txt"), common);
            File.WriteAllLines(GetPath("hex_nestCrystalSword.txt"), sword);
            File.WriteAllLines(GetPath("hex_nestCrystalShield.txt"), shield);
        }

        private static readonly int[] LanguageIndexes = { 0, 2, 3, 4, 5, 6, 7, 8, 9 };
        private static readonly string[] LanguageCodes = { "ja", "en", "fr", "it", "de", "es", "ko", "zh", "zh2" };

        private static readonly string[] LanguageNames =
        {
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
        };

        private string[] GetStoryLines(string str)
        {
            var strpath = ((FolderContainer)ROM[GameFile.StoryText]).FilePath;
            var file = Path.Combine(strpath, str);
            var txt = new TextFile(File.ReadAllBytes(file));
            return txt.Lines;
        }

        private void ChangeLanguage(int index)
        {
            ROM.Language = index;
            ROM.ResetText();
        }

        public void DumpStrings()
        {
            int lang = ROM.Language;
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
            DumpStringSet(TextName.metlist_00000, "swsh_00000");
            DumpStringSet(TextName.metlist_30000, "swsh_30000");
            DumpStringSet(TextName.metlist_40000, "swsh_40000");
            DumpStringSet(TextName.metlist_60000, "swsh_60000");
            DumpStringSet(TextName.Forms, "forms");

            void DumpStringSet(TextName t, string file)
            {
                var strings = ROM.GetStrings(t);
                var fn = $"text_{file}_{code}.txt";

                var folder = Path.Combine(code, fn);

                var path = GetPath(folder);
                File.WriteAllLines(path, strings);
            }
        }

        public void DumpFormNames()
        {
            int lang = ROM.Language;
            for (int i = 0; i < LanguageIndexes.Length; i++)
            {
                var code = LanguageCodes[i];
                var name = LanguageNames[i];
                var index = LanguageIndexes[i];

                DumpForms(code, name, index);
            }
            ChangeLanguage(lang);
        }

        private void DumpForms(string code, string name, int index)
        {
            Console.WriteLine($"Dumping strings for {name}.");
            ChangeLanguage(index);

            var strings = ROM.GetStrings(TextName.Forms);

            var indexes = new[]
            {
                0930, // Galarian
                0931, // Gigantamax
                1258, // Gulping
                1259, // Gorging
                1260, // Low Key

                1267, // Ruby Cream
                1268, // Matcha Cream
                1269, // Mint Cream
                1270, // Lemon Cream
                1271, // Salted Cream
                1272, // Ruby Swirl
                1273, // Caramel Swirl
                1274, // Rainbow Swirl

                1276, // Noice Face (iceman)
                1278, // Hangry Mode

                1281, // Crowned (skip sword/shield)
                1284, // Eternamax

                // DLC
                0892, // Single Strike Style
                0920, // World Cap
                1285, // Rapid Strike Style
                1288, // Dada
                1289, // Ice Rider
                1290, // Shadow Rider
            };

            var fn = $"NewFormNames_{code}.txt";

            var folder = Path.Combine(code, fn);

            var path = GetPath(folder);
            var newNames = strings.Select((z, i) => new { Index = i, Name = z })
                .Where(z => indexes.Contains(z.Index))
                .Select(z => z.Name);
            File.WriteAllLines(path, newNames);
        }

        public void DumpRibbonNames()
        {
            int lang = ROM.Language;
            var indexes = new[] { 0, 2, 3, 4, 5, 6, 7, 8, 9 };
            for (int i = 0; i < indexes.Length; i++)
            {
                var code = LanguageCodes[i];
                var name = LanguageNames[i];
                var index = indexes[i];

                DumpRibbonNames(code, name, index);
            }
            ChangeLanguage(lang);
        }

        private void DumpRibbonNames(string code, string name, int index)
        {
            Console.WriteLine($"Dumping strings for {name}.");
            ChangeLanguage(index);

            var strings = ROM.GetStrings(TextName.RibbonMark);
            var lines = strings.Skip(148).Take(48).ToArray();

            var fn = $"NewRibbonMarks_{code}.txt";
            var folder = Path.Combine(code, fn);
            var path = GetPath(folder);

            File.WriteAllLines(path, lines);
        }

        public void DumpTrades()
        {
            var speciesNames = ROM.GetStrings(TextName.SpeciesNames);
            var data = ROM.GetFile(GameFile.EncounterTrade)[0];
            var trades = FlatBufferConverter.DeserializeFrom<EncounterTrade8Archive>(data);
            var table = TableUtil.GetTable(trades.Table);
            var fn = GetPath("Trades.txt");
            File.WriteAllText(fn, table);

            var f2 = GetPath("TradesPKHeX.txt");
            File.WriteAllLines(f2, trades.Table.Select(z => z.GetSummary(speciesNames)));

            int lang = ROM.Language;
            var indexes = new[] { 0, 2, 3, 4, 5, 6, 7, 8, 9 };
            for (int i = 0; i < indexes.Length; i++)
            {
                var code = LanguageCodes[i];
                var name = LanguageNames[i];
                var index = indexes[i];

                DumpTradeNames(code, name, index);
            }
            ChangeLanguage(lang);
        }

        private void DumpTradeNames(string code, string name, int index)
        {
            Console.WriteLine($"Dumping Trade strings for {name}.");
            ChangeLanguage(index);

            var strings = GetStoryLines("field_trade.dat");

            const int count = 11;
            string[] result = new string[count * 2];
            for (int i = 0; i < count; i++)
            {
                var nickIndex = 6 + (8 * i);
                var otIndex = 7 + (8 * i);

                result[i] = strings[nickIndex];
                result[i + count] = strings[otIndex];
            }

            var fn = $"text_tradeswsh_{code}.txt";
            var folder = Path.Combine(code, fn);
            var path = GetPath(folder);

            File.WriteAllLines(path, result);
        }

        public void DumpMemoryStrings()
        {
            int lang = ROM.Language;
            var indexes = new[] { 0, 2, 3, 4, 5, 6, 7, 8, 9 };
            for (int i = 0; i < indexes.Length; i++)
            {
                var code = LanguageCodes[i];
                var name = LanguageNames[i];
                var index = indexes[i];

                DumpMemories(code, name, index);
            }
            ChangeLanguage(lang);
        }

        private void DumpMemories(string code, string name, int index)
        {
            Console.WriteLine($"Dumping Trade strings for {name}.");
            ChangeLanguage(index);

            var places = GetStoryLines("poke_memory_place.dat");
            File.WriteAllLines(GetPath(Path.Combine(code, $"text_GenLoc_{code}.txt")), places);

            var strings = GetStoryLines("sub_event_129.dat").Skip(90).ToArray();
            for (int i = 0; i < strings.Length; i++)
            {
                strings[i] = strings[i].Replace("[VAR 0100(0001)]", "{1}");
                strings[i] = strings[i].Replace("[VAR 0101(0004)]", "{2}");
                strings[i] = strings[i].Replace("[VAR 0102(0000)]", "{0}");
                strings[i] = strings[i].Replace("[VAR 0107(0005)]", "{2}");
                strings[i] = strings[i].Replace("[VAR 0108(0005)]", "{2}");
                strings[i] = strings[i].Replace("[VAR 0109(0003)]", "{2}");
                strings[i] = strings[i].Replace("[VAR 010A(0003)]", "{2}");
                strings[i] = strings[i].Replace("[VAR 01D6(0002)]", "{2}");
                strings[i] = strings[i].Replace("[VAR 01D7(0006)]", "{3}");
                strings[i] = strings[i].Replace("[VAR 01D8(0007)]", "{4}");
                strings[i] = strings[i].Replace("[VAR 1001]", "");
                strings[i] = strings[i].Replace("[VAR 1002]", "");
                strings[i] = strings[i].Replace("[VAR 1003]", "");
                strings[i] = strings[i].Replace("[VAR 1100(0001,0101)]", "");
                strings[i] = strings[i].Replace("[VAR 1100(00FF,0101)]", "");
                strings[i] = strings[i].Replace("[VAR 1101(0003,0100)]", "");
                strings[i] = strings[i].Replace("[VAR 1101(0003,0200)]", "");
                strings[i] = strings[i].Replace("[VAR 1302(0003,0000)]", "");
                strings[i] = strings[i].Replace("[VAR 1302(0004,0000)]", "");
                strings[i] = strings[i].Replace("[VAR 1400(0001,0001)]", "");
                strings[i] = strings[i].Replace("[VAR 1400(0003,0000)]", "");
                strings[i] = strings[i].Replace("[VAR 1402(0003,0000)]", "");
                strings[i] = strings[i].Replace("[VAR 1408(0001,0000)]", "");
                strings[i] = strings[i].Replace("[VAR 1408(0001,0001)]", "");
                strings[i] = strings[i].Replace("[VAR 140A(0001,0001)]", "");
                strings[i] = strings[i].Replace("[VAR 1500(0003,0000)]", "");
                strings[i] = strings[i].Replace("[VAR 1502(0003,0000)]", "");
                strings[i] = strings[i].Replace("[VAR 1502(0003,0001)]", "");
                strings[i] = strings[i].Replace("[VAR 1502(0004,0000)]", "");
                strings[i] = strings[i].Replace("[VAR 1502(0004,0001)]", "");
                strings[i] = strings[i].Replace("[VAR 1602(0003,0000)]", "");
                strings[i] = strings[i].Replace("[VAR 1606(0003,0000)]", "");
                strings[i] = strings[i].Replace("[VAR 1700(0003,0000)]", "");
                strings[i] = strings[i].Replace("[VAR 1702(0002,0000)]", "");
                strings[i] = strings[i].Replace("[VAR 1702(0003,0000)]", "");
                strings[i] = strings[i].Replace("[VAR 1900(0001)]", "");
                strings[i] = strings[i].Replace("[VAR 1900(0002)]", "");
                strings[i] = strings[i].Replace("[VAR 1900(0003)]", "");
                strings[i] = strings[i].Replace("[VAR 1900(0004)]", "");
                strings[i] = strings[i].Replace("[VAR 1900(0005)]", "");
                strings[i] = strings[i].Replace("\\c", "");
                strings[i] = strings[i].Replace("\\n", " ");
                strings[i] = strings[i].Replace("\\r", "");
            }
            File.WriteAllLines(GetPath(Path.Combine(code, $"text_NewMemories_{code}.txt")), strings);
        }
    }
}