using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using pkNX.Containers;
using pkNX.Structures;
using pkNX.Structures.FlatBuffers;

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
                enc.Enemy.RaidEnemyInfo.SerializePKHeX(bw, (byte)enc.Stars, enc.Rate);
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
        new [] { 3, 4, 5 },
    };

    public static void DumpDistributionRaids(string path)
    {
        var dirs = Directory.GetDirectories(path).OrderBy(z => z);
        var list = new List<byte[]>();

        foreach (var dir in dirs)
            DumpDistributionRaids(dir, list);

        var pathPickle = Path.Combine(path, "encounter_dist_paldea.pkl");
        var ordered = list
                .OrderBy(z => BinaryPrimitives.ReadUInt16LittleEndian(z)) // Species
                .ThenBy(z => z[2]) // Form
                .ThenBy(z => z[3]) // Level
                .ThenBy(z => z[0x11]) // Distribution Index
            ;
        File.WriteAllBytes(pathPickle, ordered.SelectMany(z => z).ToArray());
    }

    private static void DumpDistributionRaids(string path, List<byte[]> list)
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

        AddToList(tableEncounters.Table, list);

        var dirDistText = Path.Combine(path, "parse");
        ExportParse(dirDistText, tableEncounters, tableDrops, tableBonus, tablePriority);
    }

    private static void AddToList(IReadOnlyCollection<DeliveryRaidEnemyTable> table, List<byte[]> list)
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
            TryAddToPickle(info, list, weightTotalS, weightTotalV, weightMinS, weightMinV);
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

    private static void TryAddToPickle(RaidEnemyInfo enc, ICollection<byte[]> list, ReadOnlySpan<ushort> totalS, ReadOnlySpan<ushort> totalV, ReadOnlySpan<ushort> minS, ReadOnlySpan<ushort> minV)
    {
        using var ms = new MemoryStream();
        using var bw = new BinaryWriter(ms);

        enc.SerializePKHeX(bw, (byte)enc.Difficulty, enc.Rate);
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

        var bin = ms.ToArray();
        if (!list.Any(z => z.SequenceEqual(bin)))
            list.Add(bin);
    }

    private static void ExportParse(string dir,
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
    }

    private static void DumpJson(object flat, string dir, string name)
    {
        var opt = new System.Text.Json.JsonSerializerOptions { WriteIndented = true };
        var json = System.Text.Json.JsonSerializer.Serialize(flat, opt);

        var fileName = Path.ChangeExtension(name, ".json");
        File.WriteAllText(Path.Combine(dir, fileName), json);
    }

    private static string[] GetCommonText(IFileInternal ROM, string name, string lang, TextConfig cfg)
    {
        var monsnameDat = ROM.GetPackedFile($"message/dat/{lang}/common/{name}.dat");
        return new TextFile(monsnameDat, cfg).Lines;
    }

    private static byte[] GetDistributionContents(string path, out int index)
    {
        index = 0; //  todo
        return File.ReadAllBytes(path);
    }
}
