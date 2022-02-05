using System.Collections.Generic;
using System.IO;

namespace pkNX.Structures
{
    /// <summary>
    /// Rewards given by <see cref="GameVersion.GG"/> after capturing a Pokémon.
    /// </summary>
    /// <remarks>They do this because the Pickup Ability no longer exists, and to ease the grind of marts.</remarks>
    public class CaptureRewardTable
    {
        public List<CaptureRewardGroup> Table = new();

        public CaptureRewardTable(byte[] data)
        {
            using var ms = new MemoryStream(data);
            using var br = new BinaryReader(ms);
            ReadHeader(br);
            ReadEntries(br);
        }

        private void ReadEntries(BinaryReader br)
        {
            foreach (var g in Table)
                ReadTable(br, g);
        }

        private static void ReadTable(BinaryReader br, CaptureRewardGroup g)
        {
            for (int i = 0; i < g.EntryCount; i++)
                g.Entries.Add(ReadEntry(br));
        }

        private static CaptureRewardEntry ReadEntry(BinaryReader br) => new()
        {
            Item = br.ReadInt32(),
            Count = br.ReadInt32(),
            Rate = br.ReadInt32(),
        };

        private void ReadHeader(BinaryReader br)
        {
            while (true)
            {
                var count = br.ReadInt32();
                if (Table.Count != 0 && Table[^1].CaptureCount > count)
                {
                    br.BaseStream.Position -= 4;
                    break;
                }
                var entries = br.ReadInt32();
                var group = new CaptureRewardGroup(count, entries);
                Table.Add(group);
            }
        }

        public byte[] Write()
        {
            using var ms = new MemoryStream();
            using var bw = new BinaryWriter(ms);
            foreach (var g in Table)
            {
                bw.Write(g.CaptureCount);
                bw.Write(g.Entries.Count); // instead of using the read value, use the list count (allow modification)
            }
            foreach (var g in Table)
            {
                foreach (var e in g.Entries)
                {
                    bw.Write(e.Item);
                    bw.Write(e.Count);
                    bw.Write(e.Rate);
                }
            }
            return ms.ToArray();
        }

        public IEnumerable<string> Dump(string[] itemNames)
        {
            foreach (var g in Table)
            {
                yield return "========";
                yield return $"Count: {g.CaptureCount}";
                yield return "========";
                foreach (var item in g.Entries)
                    yield return $"{item.Rate:00}%\tx{item.Count}\t{itemNames[item.Item]}";
                yield return "";
            }
        }
    }

    public class CaptureRewardGroup
    {
        public int CaptureCount;
        public int EntryCount;

        public List<CaptureRewardEntry> Entries;

        public CaptureRewardGroup(int count, int entries)
        {
            CaptureCount = count;
            EntryCount = entries;
            Entries = new List<CaptureRewardEntry>(entries);
        }
    }

    public class CaptureRewardEntry
    {
        public int Item;
        public int Count;
        public int Rate;
    }
}
