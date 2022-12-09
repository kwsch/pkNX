using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using pkNX.Containers;
using pkNX.Game;
using pkNX.Structures;
using pkNX.Structures.FlatBuffers;

namespace pkNX.WinForms;

public class TeraRaidRipper
{
    private readonly GameManagerSV ROM;
    public TeraRaidRipper(GameManagerSV rom) => ROM = rom;
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
                if (enc.RaidEnemyInfo.RomVer != RaidRomType.TYPE_B)
                {
                    wrap.RandRateStartScarlet = totalRateScarlet;
                    totalRateScarlet += enc.RaidEnemyInfo.Rate;
                }

                if (enc.RaidEnemyInfo.RomVer != RaidRomType.TYPE_A)
                {
                    wrap.RandRateStartViolet = totalRateViolet;
                    totalRateViolet += enc.RaidEnemyInfo.Rate;
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
        // var scarlet = all.Where(z => z.Enemy.RaidEnemyInfo.RomVer != RaidRomType.TYPE_B);
        // var violet = all.Where(z => z.Enemy.RaidEnemyInfo.RomVer != RaidRomType.TYPE_A);
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
                enc.Enemy.RaidEnemyInfo.SerializePKHeX(bw, (byte)enc.Stars, enc.Rate, RaidSerializationFormat.BaseROM);
                bw.Write(rmS);
                bw.Write(rmV);
            }
        }
    }

    private record RaidStorage(RaidEnemyTable Enemy, int File)
    {
        private PokeDataBattle Poke => Enemy.RaidEnemyInfo.BossPokePara;

        public int Stars => Enemy.RaidEnemyInfo.Difficulty == 0 ? File + 1 : Enemy.RaidEnemyInfo.Difficulty;
        public DevID Species => Poke.DevId;
        public short Form => Poke.FormId;
        public int Delivery => Enemy.RaidEnemyInfo.DeliveryGroupID;
        public sbyte Rate => Enemy.RaidEnemyInfo.Rate;

        public int RandRateStartScarlet { get; set; }
        public int RandRateStartViolet { get; set; }

        public short GetScarletRandMinScarlet()
        {
            if (Enemy.RaidEnemyInfo.RomVer == RaidRomType.TYPE_B)
                return -1;
            return (short)RandRateStartScarlet;
        }

        public short GetVioletRandMinViolet()
        {
            if (Enemy.RaidEnemyInfo.RomVer == RaidRomType.TYPE_A)
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
        var dirs = Directory.GetDirectories(path).OrderBy(z => z);
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
        var dataEncounters = GetDistributionContents(Path.Combine(path, "raid_enemy_array"), out int indexEncounters);
        var dataDrop = GetDistributionContents(Path.Combine(path, "fixed_reward_item_array"), out int indexDrop);
        var dataBonus = GetDistributionContents(Path.Combine(path, "lottery_reward_item_array"), out int indexBonus);
        var priority = GetDistributionContents(Path.Combine(path, "raid_priority_array"), out int indexPriority);

        // BCAT Indexes can be reused by mixing and matching old files when reverting temporary distributions back to prior long-running distributions.
        // They don't have to match, but just note if they do.
        Debug.WriteLineIf(indexEncounters == indexDrop && indexDrop == indexBonus && indexBonus == indexPriority,
            $"Info: BCAT indexes are inconsistent! enc:{indexEncounters} drop:{indexDrop} bonus:{indexBonus} priority:{indexPriority}");

        var tableEncounters = FlatBufferConverter.DeserializeFrom<DeliveryRaidEnemyTableArray>(dataEncounters);
        var tableDrops = FlatBufferConverter.DeserializeFrom<DeliveryRaidFixedRewardItemArray>(dataDrop);
        var tableBonus = FlatBufferConverter.DeserializeFrom<DeliveryRaidLotteryRewardItemArray>(dataBonus);
        var tablePriority = FlatBufferConverter.DeserializeFrom<DeliveryRaidPriorityArray>(priority);

        var byGroupID = tableEncounters.Table
            .Where(z => z.RaidEnemyInfo.Rate != 0)
            .GroupBy(z => z.RaidEnemyInfo.DeliveryGroupID);

        bool isNot7Star = false;
        foreach (var group in byGroupID)
        {
            var items = group.ToArray();
            if (items.Any(z => z.RaidEnemyInfo.Difficulty > 7))
                throw new Exception($"Undocumented difficulty {items.First(z => z.RaidEnemyInfo.Difficulty > 7).RaidEnemyInfo.Difficulty}");

            if (items.All(z => z.RaidEnemyInfo.Difficulty == 7))
            {
                if (items.Any(z => z.RaidEnemyInfo.CaptureRate != 2))
                    throw new Exception($"Undocumented 7 star capture rate {items.First(z => z.RaidEnemyInfo.CaptureRate != 2).RaidEnemyInfo.CaptureRate}");
                AddToList(items, type3, RaidSerializationFormat.Type3);
                continue;
            }

            if (items.Any(z => z.RaidEnemyInfo.Difficulty == 7))
                throw new Exception($"Mixed difficulty {items.First(z => z.RaidEnemyInfo.Difficulty > 7).RaidEnemyInfo.Difficulty}");
            if (isNot7Star)
                throw new Exception("Already saw a not-7-star group. How do we differentiate this slot determination from prior?");
            isNot7Star = true;
            AddToList(items, type2, RaidSerializationFormat.Type2);
        }

        var dirDistText = Path.Combine(path, "parse");
        ExportParse(ROM, dirDistText, tableEncounters, tableDrops, tableBonus, tablePriority);
    }

    private static void AddToList(IReadOnlyCollection<DeliveryRaidEnemyTable> table, List<byte[]> list, RaidSerializationFormat format)
    {
        // Get the total weight for each stage of star count
        Span<ushort> weightTotalS = stackalloc ushort[StageStars.Length];
        Span<ushort> weightTotalV = stackalloc ushort[StageStars.Length];
        foreach (var enc in table)
        {
            var info = enc.RaidEnemyInfo;
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
            var info = enc.RaidEnemyInfo;
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
        if (format == RaidSerializationFormat.Type3)
            enc.SerializeType3(bw);

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
        var dumpEnc = TableUtil.GetTable(tableEncounters.Table.Select(z => z.RaidEnemyInfo.BossPokePara));
        var dumpRate = TableUtil.GetTable(tableEncounters.Table.Select(z => z.RaidEnemyInfo));
        var dumpSize = TableUtil.GetTable(tableEncounters.Table.Select(z => z.RaidEnemyInfo.BossPokeSize));
        var dumpD = TableUtil.GetTable(tableDrops.Table);
        var dumpB = TableUtil.GetTable(tableBonus.Table);
        var dumpP = TableUtil.GetTable(tablePriority.Table);
        var dumpP_2 = TableUtil.GetTable(tablePriority.Table.Select(z => z.DeliveryGroupID));
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
        DumpPretty(ROM, tableEncounters, tableDrops, tableBonus, dir);
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

    private static void DumpPretty(IFileInternal ROM, DeliveryRaidEnemyTableArray tableEncounters, DeliveryRaidFixedRewardItemArray tableDrops, DeliveryRaidLotteryRewardItemArray tableBonus, string dir)
    {
        var cfg = new TextConfig(GameVersion.SV);
        var lines = new List<string>();

        var bosses = tableEncounters.Table.Select(z => z.RaidEnemyInfo.BossPokePara);
        var dropTable = tableEncounters.Table.Select(z => z.RaidEnemyInfo.DropTableFix);
        var bonusTable = tableEncounters.Table.Select(z => z.RaidEnemyInfo.DropTableRandom);
        var ident = tableEncounters.Table[0].RaidEnemyInfo.No;

        var species = GetCommonText(ROM, "monsname", "English", cfg);
        var items = GetCommonText(ROM, "itemname", "English", cfg);
        var moves = GetCommonText(ROM, "wazaname", "English", cfg);

        lines.Add($"Event Raid Identifier: {ident}");

        foreach (var entry in tableEncounters.Table)
        {
            var boss = entry.RaidEnemyInfo.BossPokePara;
            var extra = entry.RaidEnemyInfo.BossDesc;
            var nameDrop = entry.RaidEnemyInfo.DropTableFix;
            var nameBonus = entry.RaidEnemyInfo.DropTableRandom;

            if (boss.DevId == DevID.DEV_NULL)
                continue;

            var version = entry.RaidEnemyInfo.RomVer switch
            {
                RaidRomType.TYPE_A => "Scarlet",
                RaidRomType.TYPE_B => "Violet",
                _ => string.Empty,
            };

            var gem = boss.GemType switch
            {
                GemType.DEFAULT => "Default",
                GemType.RANDOM => "Random",
                _ => $"{(PKHeX.Core.MoveType)boss.GemType - 2}",
            };

            var ability = boss.Tokusei switch
            {
                TokuseiType.SET_1 => "1 Only",
                TokuseiType.SET_2 => "2 Only",
                TokuseiType.SET_3 => "Hidden Only",
                TokuseiType.RANDOM_12 => "1/2",
                _ => "1/2/Hidden",
            };

            var shiny = boss.RareType switch
            {
                RareType.RARE => "Always",
                RareType.NO_RARE => "Never",
                _ => string.Empty,
            };

            var nature = boss.Seikaku == SeikakuType.DEFAULT ? "Random" : $"{(PKHeX.Core.Nature)(int)entry.RaidEnemyInfo.BossPokePara.Seikaku - 1}";

            var talent = boss.TalentValue;
            var iv = boss.TalentType switch
            {
                TalentType.VALUE when talent.HP == 31 && talent.ATK == 31 && talent.DEF == 31 && talent.SPA == 31 && talent.SPD == 31 && talent.SPE == 31 => "6 Flawless",
                TalentType.VALUE => $"{boss.TalentValue.HP}/{boss.TalentValue.ATK}/{boss.TalentValue.DEF}/{boss.TalentValue.SPA}/{boss.TalentValue.SPD}/{boss.TalentValue.SPE}",
                _ => $"{boss.TalentVnum} Flawless",
            };

            var form = boss.FormId == 0 ? string.Empty : $"-{entry.RaidEnemyInfo.BossPokePara.FormId}";

            lines.Add($"{entry.RaidEnemyInfo.Difficulty}-Star {species[(int)entry.RaidEnemyInfo.BossPokePara.DevId]}{form}");
            if (entry.RaidEnemyInfo.RomVer != RaidRomType.BOTH)
                lines.Add($"\tVersion: {version}");

            lines.Add($"\tTera Type: {gem}");
            lines.Add($"\tCapture Level: {entry.RaidEnemyInfo.CaptureLv}");
            lines.Add($"\tAbility: {ability}");
            lines.Add($"\tNature: {nature}");
            lines.Add($"\tIVs: {iv}");

            if (entry.RaidEnemyInfo.BossPokePara.RareType != RareType.DEFAULT)
                lines.Add($"\tShiny: {shiny}");

            lines.Add($"\t\tMoves:");
            lines.Add($"\t\t\t- {moves[(int)boss.Waza1.WazaId]}");
            if ((int)boss.Waza2.WazaId != 0) lines.Add($"\t\t\t- {moves[(int)boss.Waza2.WazaId]}");
            if ((int)boss.Waza2.WazaId != 0) lines.Add($"\t\t\t- {moves[(int)boss.Waza3.WazaId]}");
            if ((int)boss.Waza2.WazaId != 0) lines.Add($"\t\t\t- {moves[(int)boss.Waza4.WazaId]}");

            lines.Add($"\t\tExtra Moves:");
            if ((int)extra.ExtraAction1.Wazano == 0 && (int)extra.ExtraAction2.Wazano == 0 && (int)extra.ExtraAction3.Wazano == 0 && (int)extra.ExtraAction4.Wazano == 0 && (int)extra.ExtraAction5.Wazano == 0 && (int)extra.ExtraAction6.Wazano == 0)
                lines.Add("\t\t\tNone!");

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
                for (int i = 0; i <= 14; i++)
                {
                    if (entry.RaidEnemyInfo.DropTableFix != item.TableName)
                        continue;

                    var drop = item.GetReward(i);
                    var one = (int)item.GetReward(i).SubjectType == 3 ? " (Only Once)" : string.Empty;

                    string GetItemName(int item)
                    {
                        bool isTM = (int)drop.ItemID is (>= 328 and <= 419) or (>= 618 and <= 620) or (>= 690 and <= 693) or (>= 2160 and <= 2231);
                        var tm = PKHeX.Core.LearnSource9SV.TM_SV.ToArray();

                        if (isTM) // append move name to TM
                        {
                            if ((int)drop.ItemID is >= 328 and <= 419)
                                return $"{items[(int)drop.ItemID]} {moves[tm[(int)drop.ItemID]]}";
                            if ((int)drop.ItemID is 618 or 619 or 620)
                                return $"{items[(int)drop.ItemID]} {moves[tm[100 + (int)drop.ItemID - 618]]}";
                            if ((int)drop.ItemID is 690 or 691 or 692 or 693)
                                return $"{items[(int)drop.ItemID]} {moves[tm[100 + (int)drop.ItemID - 690]]}";

                            return $"{items[(int)drop.ItemID]} {moves[tm[100 + (int)drop.ItemID - 2160]]}";
                        }

                        return $"{items[(int)drop.ItemID]}";
                    }

                    if ((int)drop.Category == 1) // Material
                        lines.Add($"\t\t\t{drop.Num,2} × Crafting Material{one}");

                    if ((int)drop.Category == 2) // Tera Shard
                        lines.Add($"\t\t\t{drop.Num,2} × Tera Shard{one}");

                    if (drop.ItemID != 0)
                        lines.Add($"\t\t\t{drop.Num,2} × {GetItemName((int)drop.ItemID)}{one}");
                }
            }

            lines.Add("\t\tBonus Drops:");

            foreach (var item in tableBonus.Table.Where(z => z.TableName == nameBonus))
            {
                float totalRate = 0;
                for (int i = 0; i <= 29; i++)
                    totalRate += item.GetRewardItem(i).Rate;

                for (int i = 0; i <= 29; i++)
                {
                    if (entry.RaidEnemyInfo.DropTableRandom != item.TableName)
                        continue;

                    var drop = item.GetRewardItem(i);
                    if (drop.ItemID != 0)
                    {
                        float rate = (float)(Math.Round((float)((item.GetRewardItem(i).Rate / totalRate) * 100f), 2));
                        lines.Add($"\t\t\t{(float)rate,5}% {drop.Num,2} × {items[(int)drop.ItemID]}");
                    }
                }
            }

            lines.Add("");
        }

        File.WriteAllLines(Path.Combine(dir, $"pretty_{ident}.txt"), lines);
    }
}
