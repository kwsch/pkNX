using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using pkNX.Containers;
using pkNX.Structures;
using pkNX.Structures.FlatBuffers;
using pkNX.Structures.FlatBuffers.SV;

namespace pkNX.WinForms;

public static class TeraRaidRipper
{
    public static void DumpRaids(IFileInternal ROM, IReadOnlyList<string> internalRaidFiles, string outPath)
    {
        var list = new List<RaidStorage>();
        var rateTotal = new (int Scarlet, int Violet)[8];
        for (int i = 0; i < internalRaidFiles.Count; i++)
        {
            var path = internalRaidFiles[i];
            path = Path.ChangeExtension(path, ".bin");
            var data = ROM.GetPackedFile(path);
            var fb = FlatBufferConverter.DeserializeFrom<RaidEnemyTableArray>(data);
            var table = fb.Table;

            int totalRateScarlet = 0;
            int totalRateViolet = 0;
            foreach (var enc in table)
            {
                var wrap = new RaidStorage(enc, i);
                if (enc.Info.RomVer != RaidRomType.TYPE_B)
                {
                    wrap.RandRateStartScarlet = totalRateScarlet;
                    totalRateScarlet += enc.Info.Rate;
                }

                if (enc.Info.RomVer != RaidRomType.TYPE_A)
                {
                    wrap.RandRateStartViolet = totalRateViolet;
                    totalRateViolet += enc.Info.Rate;
                }
                list.Add(wrap);
            }
            rateTotal[i+1] = (totalRateScarlet, totalRateViolet);
        }

        var all = list
            .OrderBy(z => z.Species)
            .ThenBy(z => z.Form)
            .ThenByDescending(z => z.Stars)
            .ThenByDescending(z => z.Delivery)
            .ToList();

        var rateString = string.Join(Environment.NewLine, rateTotal.Select((z, i) => $"{i}\t{z.Scarlet}\t{z.Violet}"));
        File.WriteAllText(Path.Combine(outPath, "rateTotal.txt"), rateString);
        WritePickle(outPath, all, "encounter_gem_paldea.pkl");
        // Raids can be shared, and show up with the same met location regardless of shared vs not.
        // No need to differentiate.
        // var scarlet = all.Where(z => z.Enemy.Info.RomVer != RaidRomType.TYPE_B);
        // var violet = all.Where(z => z.Enemy.Info.RomVer != RaidRomType.TYPE_A);
        // WritePickle(outPath, scarlet, "encounter_gem_sl.pkl");
        // WritePickle(outPath, violet, "encounter_gem_vl.pkl");
        // However, the RNG pattern is different for the version of the host. Need to account for those.

        static void WritePickle(string outPath, IEnumerable<RaidStorage> list, string fileName)
        {
            var path = Path.Combine(outPath, fileName);
            using var fs = File.Create(path);
            using var bw = new BinaryWriter(fs);
            foreach (var enc in list)
            {
                var rmS = enc.GetScarletRandMinScarlet();
                var rmV = enc.GetVioletRandMinViolet();
                enc.Enemy.Info.SerializePKHeX(bw, (byte)enc.Stars, enc.Rate, RaidSerializationFormat.BaseROM);
                bw.Write(rmS);
                bw.Write(rmV);
            }
        }
    }

    private record RaidStorage(RaidEnemyTable Enemy, int File)
    {
        private PokeDataBattle Poke => Enemy.Info.BossPokePara;

        public int Stars => Enemy.Info.Difficulty == 0 ? File + 1 : Enemy.Info.Difficulty;
        public DevID Species => Poke.DevId;
        public short Form => Poke.FormId;
        public int Delivery => Enemy.Info.DeliveryGroupID;
        public sbyte Rate => Enemy.Info.Rate;

        public int RandRateStartScarlet { get; set; }
        public int RandRateStartViolet { get; set; }

        public short GetScarletRandMinScarlet()
        {
            if (Enemy.Info.RomVer == RaidRomType.TYPE_B)
                return -1;
            return (short)RandRateStartScarlet;
        }

        public short GetVioletRandMinViolet()
        {
            if (Enemy.Info.RomVer == RaidRomType.TYPE_A)
                return -1;
            return (short)RandRateStartViolet;
        }
    }

    private static readonly int[][] StageStars =
    {
        new [] { 1, 2 },
        new [] { 1, 2, 3 },
        new [] { 1, 2, 3, 4 },
        new [] { 3, 4, 5, 6, 7 },
    };

    public static void DumpDistributionRaids(IFileInternal ROM, string path)
    {
        var dirs = Directory.GetDirectories(path, "*", SearchOption.AllDirectories).OrderBy(z => z);
        var type2 = new List<byte[]>();
        var type3 = new List<byte[]>();

        foreach (var dir in dirs)
            DumpDistributionRaids(ROM, dir, type2, type3);

        DumpPicklePath(type2, path, "encounter_dist_paldea.pkl");
        DumpPicklePath(type3, path, "encounter_might_paldea.pkl");
    }

    private static void DumpPicklePath(List<byte[]> list, string path, string pp)
    {
        var pathPickle = Path.Combine(path, pp);
        var ordered = list
                .OrderBy(z => BinaryPrimitives.ReadUInt16LittleEndian(z)) // Species
                .ThenBy(z => z[2]) // Form
                .ThenBy(z => z[3]) // Level
            ;
        File.WriteAllBytes(pathPickle, ordered.SelectMany(z => z).ToArray());
    }

    private static void DumpDistributionRaids(IFileInternal ROM, string path, List<byte[]> type2, List<byte[]> type3)
    {
        var enemy = Path.Combine(path, "raid_enemy_array");
        var reward = Path.Combine(path, "fixed_reward_item_array");
        var lottery = Path.Combine(path, "lottery_reward_item_array");
        var priority = Path.Combine(path, "raid_priority_array");
        const string v130 = "_1_3_0";

        if (!File.Exists(enemy))
            return;

        if (File.Exists(enemy + v130))
        {
            // Starting with Ver. 1.3.0, BCAT is distributed with 1.3.0 specific binaries, in addition to the original base game binaries (e.g. raid_enemy_array_1_3_0).
            // The original data is dummied out, while the 1.3.0 data contains the raids we want to parse.
            enemy += v130;
            reward += v130;
            lottery += v130;
            priority += v130;
        }

        var dataEncounters = GetDistributionContents(enemy, out int indexEncounters);
        var dataDrop = GetDistributionContents(Path.Combine(path, reward), out int indexDrop);
        var dataBonus = GetDistributionContents(Path.Combine(path, lottery), out int indexBonus);
        var dataPriority = GetDistributionContents(Path.Combine(path, priority), out int indexPriority);

        // BCAT Indexes can be reused by mixing and matching old files when reverting temporary distributions back to prior long-running distributions.
        // They don't have to match, but just note if they do.
        Debug.WriteLineIf(indexEncounters == indexDrop && indexDrop == indexBonus && indexBonus == indexPriority,
            $"Info: BCAT indexes are inconsistent! enc:{indexEncounters} drop:{indexDrop} bonus:{indexBonus} priority:{indexPriority}");

        var tableEncounters = FlatBufferConverter.DeserializeFrom<DeliveryRaidEnemyTableArray>(dataEncounters);
        var tableDrops = FlatBufferConverter.DeserializeFrom<DeliveryRaidFixedRewardItemArray>(dataDrop);
        var tableBonus = FlatBufferConverter.DeserializeFrom<DeliveryRaidLotteryRewardItemArray>(dataBonus);
        var tablePriority = FlatBufferConverter.DeserializeFrom<DeliveryRaidPriorityArray>(priority);

        var byGroupID = tableEncounters.Table
            .Where(z => z.Info.Rate != 0)
            .GroupBy(z => z.Info.DeliveryGroupID);

        var seven = DistroGroupSet.None;
        var other = DistroGroupSet.None;

        foreach (var group in byGroupID)
        {
            var items = group.ToArray();
            var groupSet = Evaluate(items);

            if (items.Any(z => z.Info.Difficulty > 7))
                throw new Exception($"Undocumented difficulty {items.First(z => z.Info.Difficulty > 7).Info.Difficulty}");

            if (items.All(z => z.Info.Difficulty == 7))
            {
                if (items.Any(z => z.Info.CaptureRate != 2))
                    throw new Exception($"Undocumented 7 star capture rate {items.First(z => z.Info.CaptureRate != 2).Info.CaptureRate}");

                if (!TryAdd(ref seven, groupSet))
                    Console.WriteLine("Already saw a 7-star group. How do we differentiate this slot determination from prior?");
                AddToList(items, type3, RaidSerializationFormat.Might);
                continue;
            }

            if (items.Any(z => z.Info.Difficulty == 7))
                throw new Exception($"Mixed difficulty {items.First(z => z.Info.Difficulty >= 7).Info.Difficulty}");

            if (!TryAdd(ref other, groupSet))
                Console.WriteLine("Already saw a not-7-star group. How do we differentiate this slot determination from prior?");
            AddToList(items, type2, RaidSerializationFormat.Distribution);
        }

        var dirDistText = Path.Combine(path, "parse");
        ExportParse(ROM, dirDistText, tableEncounters, tableDrops, tableBonus, tablePriority);
    }

    private static bool TryAdd(ref DistroGroupSet exist, DistroGroupSet add)
    {
        if ((exist & add) != 0)
            return false;
        exist |= add;
        return true;
    }

    [Flags]
    private enum DistroGroupSet
    {
        None = 0,
        SL = 1,
        VL = 2,
        Both = SL | VL,
    }

    private static DistroGroupSet Evaluate(DeliveryRaidEnemyTable[] items)
    {
        var versions = items.Select(z => z.Info.RomVer).Distinct().ToArray();
        if (versions.Length == 2 && versions.Contains(RaidRomType.TYPE_A) && versions.Contains(RaidRomType.TYPE_B))
            return DistroGroupSet.Both;
        if (versions.Length == 1)
        {
            return versions[0] switch
            {
                RaidRomType.BOTH => DistroGroupSet.Both,
                RaidRomType.TYPE_A => DistroGroupSet.SL,
                RaidRomType.TYPE_B => DistroGroupSet.VL,
                _ => throw new Exception("Unknown type."),
            };
        }
        throw new Exception("Unknown version");
    }

    private static void AddToList(IReadOnlyCollection<DeliveryRaidEnemyTable> table, List<byte[]> list, RaidSerializationFormat format)
    {
        // Get the total weight for each stage of star count
        Span<ushort> weightTotalS = stackalloc ushort[StageStars.Length];
        Span<ushort> weightTotalV = stackalloc ushort[StageStars.Length];
        foreach (var enc in table)
        {
            var info = enc.Info;
            if (info.Rate == 0)
                continue;
            var difficulty = info.Difficulty;
            for (int stage = 0; stage < StageStars.Length; stage++)
            {
                if (!StageStars[stage].Contains(difficulty))
                    continue;
                if (info.RomVer != RaidRomType.TYPE_B)
                    weightTotalS[stage] += (ushort)info.Rate;
                if (info.RomVer != RaidRomType.TYPE_A)
                    weightTotalV[stage] += (ushort)info.Rate;
            }
        }

        Span<ushort> weightMinS = stackalloc ushort[StageStars.Length];
        Span<ushort> weightMinV = stackalloc ushort[StageStars.Length];
        foreach (var enc in table)
        {
            var info = enc.Info;
            if (info.Rate == 0)
                continue;
            var difficulty = info.Difficulty;
            TryAddToPickle(info, list, format, weightTotalS, weightTotalV, weightMinS, weightMinV);
            for (int stage = 0; stage < StageStars.Length; stage++)
            {
                if (!StageStars[stage].Contains(difficulty))
                    continue;
                if (info.RomVer != RaidRomType.TYPE_B)
                    weightMinS[stage] += (ushort)info.Rate;
                if (info.RomVer != RaidRomType.TYPE_A)
                    weightMinV[stage] += (ushort)info.Rate;
            }
        }
    }

    private static void TryAddToPickle(RaidEnemyInfo enc, ICollection<byte[]> list, RaidSerializationFormat format,
        ReadOnlySpan<ushort> totalS, ReadOnlySpan<ushort> totalV, ReadOnlySpan<ushort> minS, ReadOnlySpan<ushort> minV)
    {
        using var ms = new MemoryStream();
        using var bw = new BinaryWriter(ms);

        enc.SerializePKHeX(bw, (byte)enc.Difficulty, enc.Rate, format);
        for (int stage = 0; stage < StageStars.Length; stage++)
        {
            bool noTotal = !StageStars[stage].Contains(enc.Difficulty);
            ushort mS = minS[stage];
            ushort mV = minV[stage];
            bw.Write(noTotal ? (ushort)0 : mS);
            bw.Write(noTotal ? (ushort)0 : mV);
            bw.Write(noTotal ? (ushort)0 : totalS[stage]);
            bw.Write(noTotal ? (ushort)0 : totalV[stage]);
        }
        if (format == RaidSerializationFormat.Distribution)
            enc.SerializeDistribution(bw);
        if (format == RaidSerializationFormat.Might)
            enc.SerializeMight(bw);

        var bin = ms.ToArray();
        if (!list.Any(z => z.SequenceEqual(bin)))
            list.Add(bin);
    }

    private static void ExportParse(IFileInternal ROM, string dir,
        DeliveryRaidEnemyTableArray tableEncounters,
        DeliveryRaidFixedRewardItemArray tableDrops,
        DeliveryRaidLotteryRewardItemArray tableBonus,
        DeliveryRaidPriorityArray tablePriority)
    {
        var dumpE = TableUtil.GetTable(tableEncounters.Table);
        var dumpEnc = TableUtil.GetTable(tableEncounters.Table.Select(z => z.Info.BossPokePara));
        var dumpRate = TableUtil.GetTable(tableEncounters.Table.Select(z => z.Info));
        var dumpSize = TableUtil.GetTable(tableEncounters.Table.Select(z => z.Info.BossPokeSize));
        var dumpD = TableUtil.GetTable(tableDrops.Table);
        var dumpB = TableUtil.GetTable(tableBonus.Table);
        var dumpP = TableUtil.GetTable(tablePriority.Table);
        var dumpP_2 = TableUtil.GetTable(tablePriority.Table.Select(z => z.GroupID));
        var dump = new[]
        {
            ("encounters", dumpE),
            ("encounters_poke", dumpEnc),
            ("encounters_rate", dumpRate),
            ("encounters_size", dumpSize),
            ("drops", dumpD),
            ("bonus", dumpB),
            ("priority", dumpP),
            ("priority_alt", dumpP_2),
        };

        Directory.CreateDirectory(dir);
        foreach (var (name, data) in dump)
        {
            var path2 = Path.Combine(dir, $"{name}.txt");
            File.WriteAllText(path2, data);
        }

        DumpJson(tableEncounters, dir, "enc");
        DumpJson(tableDrops, dir, "drop");
        DumpJson(tableBonus, dir, "bonus");
        DumpJson(tablePriority, dir, "priority");
        DumpPretty(ROM, tableEncounters, tableDrops, tableBonus, tablePriority, dir);
    }

    private static void DumpJson(object flat, string dir, string name)
    {
        var opt = new System.Text.Json.JsonSerializerOptions { WriteIndented = true };
        var json = System.Text.Json.JsonSerializer.Serialize(flat, opt);

        var fileName = Path.ChangeExtension(name, ".json");
        File.WriteAllText(Path.Combine(dir, fileName), json);
    }

    private static byte[] GetDistributionContents(string path, out int index)
    {
        index = 0; //  todo
        return File.ReadAllBytes(path);
    }

    private static string[] GetCommonText(IFileInternal ROM, string name, string lang, TextConfig cfg)
    {
        var data = ROM.GetPackedFile($"message/dat/{lang}/common/{name}.dat");
        return new TextFile(data, cfg).Lines;
    }

    private static void DumpPretty(IFileInternal ROM, DeliveryRaidEnemyTableArray tableEncounters, DeliveryRaidFixedRewardItemArray tableDrops, DeliveryRaidLotteryRewardItemArray tableBonus, DeliveryRaidPriorityArray tablePriority, string dir)
    {
        var cfg = new TextConfig(GameVersion.SV);
        var lines = new List<string>();
        var ident = tablePriority.Table[0].VersionNo;
        const string lang = "English";

        var species = GetCommonText(ROM, "monsname", lang, cfg);
        var items = GetCommonText(ROM, "itemname", lang, cfg);
        var moves = GetCommonText(ROM, "wazaname", lang, cfg);
        var types = GetCommonText(ROM, "typename", lang, cfg);
        var natures = GetCommonText(ROM, "seikaku", lang, cfg);

        lines.Add($"Event Raid Identifier: {ident}");

        foreach (var entry in tableEncounters.Table)
        {
            var boss = entry.Info.BossPokePara;
            var extra = entry.Info.BossDesc;
            var nameDrop = entry.Info.DropTableFix;
            var nameBonus = entry.Info.DropTableRandom;

            if (boss.DevId == DevID.DEV_NULL)
                continue;

            var version = entry.Info.RomVer switch
            {
                RaidRomType.TYPE_A => "Scarlet",
                RaidRomType.TYPE_B => "Violet",
                _ => string.Empty,
            };

            var gem = boss.GemType switch
            {
                GemType.DEFAULT => "Default",
                GemType.RANDOM => "Random",
                _ => $"{types[(int)boss.GemType - 2]}",
            };

            var ability = boss.Tokusei switch
            {
                TokuseiType.SET_1 => "1 Only",
                TokuseiType.SET_2 => "2 Only",
                TokuseiType.SET_3 => "Hidden Only",
                TokuseiType.RANDOM_12 => "1/2",
                _ => "1/2/H",
            };

            var shiny = boss.RareType switch
            {
                RareType.RARE => "Always",
                RareType.NO_RARE => "Never",
                _ => string.Empty,
            };

            var talent = boss.TalentValue;
            var iv = boss.TalentType switch
            {
                TalentType.VALUE when talent is { HP: 31, ATK: 31, DEF: 31, SPA: 31, SPD: 31, SPE: 31 } => "6 Flawless",
                TalentType.VALUE => talent.SlashSeparated(),
                _ => $"{boss.TalentVnum} Flawless",
            };

            var capture = entry.Info.CaptureRate switch
            {
                // 0 never?
                // 1 always
                2 => "Only Once",
                _ => $"{entry.Info.CaptureRate}",
            };

            var size = boss.ScaleType switch
            {
                SizeType.VALUE => $"{boss.ScaleValue}",
                SizeType.XS => "0-15",
                SizeType.S => "16-47",
                SizeType.M => "48-207",
                SizeType.L => "208-239",
                SizeType.XL => "240-255",
                _ => string.Empty,
            };

            var gender = boss.Sex switch
            {
                SexType.MALE => "Male",
                SexType.FEMALE => "Female",
                _ => string.Empty,
            };

            var form = boss.FormId == 0 ? string.Empty : $"-{(int)boss.FormId}";

            lines.Add($"{entry.Info.Difficulty}-Star {species[(int)boss.DevId]}{form}");
            if (entry.Info.RomVer != RaidRomType.BOTH)
                lines.Add($"\tVersion: {version}");

            lines.Add($"\tTera Type: {gem}");
            lines.Add($"\tCapture Level: {entry.Info.CaptureLv}");
            lines.Add($"\tAbility: {ability}");

            if (boss.Seikaku != SeikakuType.DEFAULT)
                lines.Add($"\tNature: {natures[(int)boss.Seikaku - 1]}");

            if (boss.Sex != SexType.DEFAULT)
                lines.Add($"\tGender: {gender}");

            lines.Add($"\tIVs: {iv}");

            var evs = boss.EffortValue.ToArray();
            if (evs.Any(z => z != 0))
            {
                string[] names = new[] { "HP", "Atk", "Def", "SpA", "SpD", "Spe" };
                var spread = new List<string>();

                for (int i = 0; i < evs.Length; i++)
                {
                    if (evs[i] == 0)
                        continue;
                    spread.Add($"{evs[i]} {names[i]}");
                }

                lines.Add($"\tEVs: {string.Join(" / ", spread)}");
            }

            if (boss.RareType != RareType.DEFAULT)
                lines.Add($"\tShiny: {shiny}");

            if (boss.ScaleType != SizeType.RANDOM)
                lines.Add($"\tScale: {size}");

            if (entry.Info.Difficulty == 7)
            {
                float hp = entry.Info.BossDesc.HpCoef / 100f;
                lines.Add($"\tHP Multiplier: {hp:0.0}x");
            }

            if (boss.Item != ItemID.ITEMID_NONE)
                lines.Add($"\tHeld Item: {items[(int)boss.Item]}");

            if (entry.Info.CaptureRate != 1)
                lines.Add($"\tCatchable: {capture}");

            lines.Add($"\t\tMoves:");
            lines.Add($"\t\t\t- {moves[(int)boss.Waza1.WazaId]}");
            if ((int)boss.Waza2.WazaId != 0) lines.Add($"\t\t\t- {moves[(int)boss.Waza2.WazaId]}");
            if ((int)boss.Waza3.WazaId != 0) lines.Add($"\t\t\t- {moves[(int)boss.Waza3.WazaId]}");
            if ((int)boss.Waza4.WazaId != 0) lines.Add($"\t\t\t- {moves[(int)boss.Waza4.WazaId]}");

            lines.Add($"\t\tExtra Moves:");

            if ((int)extra.ExtraAction1.Wazano == 0 && (int)extra.ExtraAction2.Wazano == 0 && (int)extra.ExtraAction3.Wazano == 0 && (int)extra.ExtraAction4.Wazano == 0 && (int)extra.ExtraAction5.Wazano == 0 && (int)extra.ExtraAction6.Wazano == 0)
            {
                lines.Add("\t\t\tNone!");
            }

            else
            {
                if ((int)extra.ExtraAction1.Wazano != 0) lines.Add($"\t\t\t- {moves[(int)extra.ExtraAction1.Wazano]}");
                if ((int)extra.ExtraAction2.Wazano != 0) lines.Add($"\t\t\t- {moves[(int)extra.ExtraAction2.Wazano]}");
                if ((int)extra.ExtraAction3.Wazano != 0) lines.Add($"\t\t\t- {moves[(int)extra.ExtraAction3.Wazano]}");
                if ((int)extra.ExtraAction4.Wazano != 0) lines.Add($"\t\t\t- {moves[(int)extra.ExtraAction4.Wazano]}");
                if ((int)extra.ExtraAction5.Wazano != 0) lines.Add($"\t\t\t- {moves[(int)extra.ExtraAction5.Wazano]}");
                if ((int)extra.ExtraAction6.Wazano != 0) lines.Add($"\t\t\t- {moves[(int)extra.ExtraAction6.Wazano]}");
            }

            lines.Add("\t\tItem Drops:");

            foreach (var item in tableDrops.Table.Where(z => z.TableName == nameDrop))
            {
                const int count = RaidFixedRewardItem.Count;
                for (int i = 0; i < count; i++)
                {
                    if (nameDrop != item.TableName)
                        continue;

                    var drop = item.GetReward(i);
                    var limitation = drop.SubjectType switch
                    {
                        RaidRewardItemSubjectType.HOST => " (Only Host)",
                        RaidRewardItemSubjectType.CLIENT => " (Only Guests)",
                        RaidRewardItemSubjectType.ONCE => " (Only Once)",
                        _ => string.Empty,
                    };

                    if (drop.Category == RaidRewardItemCategoryType.POKE) // Material
                        lines.Add($"\t\t\t{drop.Num,2} × TM Material{limitation}");

                    if (drop.Category == RaidRewardItemCategoryType.GEM) // Tera Shard
                        lines.Add($"\t\t\t{drop.Num,2} × Tera Shard{limitation}");

                    if (drop.ItemID != 0)
                        lines.Add($"\t\t\t{drop.Num,2} × {GetItemName((ushort)drop.ItemID, items, moves)}{limitation}");
                }
            }

            lines.Add("\t\tBonus Drops:");

            foreach (var item in tableBonus.Table.Where(z => z.TableName == nameBonus))
            {
                const int count = RaidLotteryRewardItem.RewardItemCount;
                float totalRate = 0;
                for (int i = 0; i < count; i++)
                    totalRate += item.GetRewardItem(i).Rate;

                for (int i = 0; i < count; i++)
                {
                    if (nameBonus != item.TableName)
                        continue;

                    var drop = item.GetRewardItem(i);
                    float rate = (float)(Math.Round((item.GetRewardItem(i).Rate / totalRate) * 100f, 2));

                    if (drop.Category == RaidRewardItemCategoryType.POKE) // Material
                        lines.Add($"\t\t\t{rate,5}% {drop.Num,2} × TM Material");

                    if (drop.Category == RaidRewardItemCategoryType.GEM) // Tera Shard
                        lines.Add($"\t\t\t{rate,5}% {drop.Num,2} × Tera Shard");

                    if (drop.ItemID != 0)
                        lines.Add($"\t\t\t{rate,5}% {drop.Num,2} × {GetItemName((ushort)drop.ItemID, items, moves)}");
                }
            }

            lines.Add("");
        }

        File.WriteAllLines(Path.Combine(dir, $"pretty_{ident}.txt"), lines);
    }

    private static string GetItemName(ushort item, ReadOnlySpan<string> items, ReadOnlySpan<string> moves)
    {
        bool isTM = IsTM(item);
        var tm = PersonalDumperSV.TMIndexes;

        if (isTM) // append move name to TM
            return GetNameTM(item, items, moves, tm);
        return $"{items[item]}";
    }

    private static bool IsTM(ushort item) => item switch
    {
        >= 328 and <= 419 => true, // TM001 to TM092, skip TM000 Mega Punch
        618 or 619 or 620 => true, // TM093 to TM095
        690 or 691 or 692 or 693 => true, // TM096 to TM099
        >= 2160 and <= 2231 => true, // TM100 to TM171
        _ => false,
    };

    private static string GetNameTM(ushort item, ReadOnlySpan<string> items, ReadOnlySpan<string> moves, ReadOnlySpan<ushort> tm) => item switch
    {
        >= 328 and <= 419 => $"{items[item]} {moves[tm[001 + item - 328]]}", // TM001 to TM092, skip TM000 Mega Punch
        618 or 619 or 620 => $"{items[item]} {moves[tm[093 + item - 618]]}", // TM093 to TM095
        690 or 691 or 692 or 693 => $"{items[item]} {moves[tm[096 + item - 690]]}", // TM096 to TM099
        _ => $"{items[item]} {moves[tm[100 + item - 2160]]}", // TM100 to TM171
    };
}
