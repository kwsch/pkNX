using System.Collections.Generic;
using System.IO;

namespace pkNX.Containers
{
    internal class FATB
    {
        private const uint MAGIC = 0x46415442;

        private readonly uint Magic = MAGIC;
        private readonly int HeaderSize = 0xC;
        private readonly ushort EntryCount;
        private readonly short Padding = -1;

        private FATBEntry[] Entries { get; }
        public FATBEntry this[int index] => Entries[index];

        public FATB(BinaryReader br, int DataOffset)
        {
            Magic = br.ReadUInt32();
            HeaderSize = br.ReadInt32();
            EntryCount = br.ReadUInt16();
            Padding = br.ReadInt16();

            Entries = new FATBEntry[EntryCount];
            for (int i = 0; i < EntryCount; i++)
                Entries[i] = new FATBEntry(br, DataOffset);
        }

        public FATB(IReadOnlyList<string> files)
        {
            EntryCount = (ushort)files.Count;
            Entries = new FATBEntry[EntryCount];
            for (int i = 0; i < EntryCount; i++)
            {
                if (!Directory.Exists(files[i]))
                    Entries[i] = new FATBEntry(files[i]);
                else
                    Entries[i] = new FATBEntry(Directory.GetFiles(files[i]));
            }
        }

        public void Write(BinaryWriter bw)
        {
            bw.Write(Magic);
            bw.Write(HeaderSize);
            bw.Write(EntryCount);
            bw.Write(Padding);
            foreach (var entry in Entries)
                entry.Write(bw);
            // sub entry writing handled separately
        }
    }
}