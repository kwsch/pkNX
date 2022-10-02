using System.IO;

namespace pkNX.Containers;

internal class FATO
{
    private const uint MAGIC = 0x4641544F;

    private readonly uint Magic = MAGIC;
    private readonly int HeaderSize = 0xC;
    public readonly ushort EntryCount;
    private readonly short Padding = -1;

    private FATOEntry[] Entries { get; }
    public FATOEntry this[int index] => Entries[index];

    public FATO(BinaryReader br)
    {
        Magic = br.ReadUInt32();
        HeaderSize = br.ReadInt32();
        EntryCount = br.ReadUInt16();
        Padding = br.ReadInt16();

        Entries = new FATOEntry[EntryCount];
        for (int i = 0; i < EntryCount; i++)
            Entries[i] = new FATOEntry(br);
    }

    public FATO(int count)
    {
        EntryCount = (ushort)count;
        Entries = new FATOEntry[EntryCount];
        for (int i = 0; i < EntryCount; i++)
            Entries[i] = new FATOEntry();
    }

    public void Write(BinaryWriter bw)
    {
        bw.Write(Magic);
        bw.Write(HeaderSize);
        bw.Write(EntryCount);
        bw.Write(Padding);
        foreach (var entry in Entries)
            entry.Write(bw);
    }
}
