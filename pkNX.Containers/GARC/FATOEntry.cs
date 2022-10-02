using System.IO;

namespace pkNX.Containers;

internal class FATOEntry
{
    public uint Offset { get; set; }

    internal FATOEntry(uint offset = 0) => Offset = offset;
    internal FATOEntry(BinaryReader br) => Offset = br.ReadUInt32();

    internal void Write(BinaryWriter bw) => bw.Write(Offset);
}
