using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace pkNX.Structures
{
    public sealed class SeedPokeTable
    {
        private readonly ushort[] Table;

        public SeedPokeTable(byte[] data)
        {
            Table = new ushort[data.Length/2];
            for (int i = 0; i < Table.Length; i++)
                Table[i] = BitConverter.ToUInt16(data, i * 2);
        }

        public ushort this[int index] => Table[index];

        public byte[] Write()
        {
            using var ms = new MemoryStream();
            using var bw = new BinaryWriter(ms);
            foreach (var seed in Table)
                bw.Write(seed);
            return ms.ToArray();
        }

        public IEnumerable<string> Dump(string[] specNames)
        {
            return Table.Select((t, i) => $"{i:000}\t{specNames[i]}\t{specNames[t]}");
        }
    }
}
