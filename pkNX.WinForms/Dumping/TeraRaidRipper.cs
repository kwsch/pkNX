using System;
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
                enc.Enemy.RaidEnemyInfo.SerializePKHeX(bw, (byte)enc.Stars, enc.Rate, rmS, rmV);
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

    public static void DumpDistributionRaids(IFileInternal ROM, string path, string outPath)
    {
        // todo - file names for the FlatBuffer files
        var dataEncounters = GetDistributionContents(Path.Combine(path, "dai_encount"), out int indexEncounters);
        var dataDrop = GetDistributionContents(Path.Combine(path, "drop_rewards"), out int indexDrop);
        var dataBonus = GetDistributionContents(Path.Combine(path, "bonus_rewards"), out int indexBonus);

        // BCAT Indexes can be reused by mixing and matching old files when reverting temporary distributions back to prior long-running distributions.
        // They don't have to match, but just note if they do.
        Debug.WriteLineIf(indexEncounters == indexDrop && indexDrop == indexBonus,
            $"Info: BCAT indexes are inconsistent! enc:{indexEncounters} drop:{indexDrop} bonus:{indexBonus}");

        var tableEncounters = FlatBufferConverter.DeserializeFrom<DeliveryRaidEnemyTableArray>(dataEncounters);
        var tableDrops = FlatBufferConverter.DeserializeFrom<DeliveryRaidFixedRewardItemArray>(dataDrop);
        var tableBonus = FlatBufferConverter.DeserializeFrom<DeliveryRaidLotteryRewardItemArray>(dataBonus);

        const string lang = "English";
        var cfg = new TextConfig(GameVersion.SV);
        string[] GetText(string name) => GetCommonText(ROM, name, lang, cfg);
        var speciesNames = GetText("monsname");
        var moveNames = GetText("wazaname");
        var itemNames = GetText("itemname");
    }

    private static string[] GetCommonText(IFileInternal ROM, string name, string lang, TextConfig cfg)
    {
        var monsnameDat = ROM.GetPackedFile($"message/dat/{lang}/common/{name}.dat");
        return new TextFile(monsnameDat, cfg).Lines;
    }

    private static byte[] GetDistributionContents(string path, out int index)
    {
        // todo once we get distribution raids
        index = 0;
        return Array.Empty<byte>();
    }
}
