using System;
using System.IO;

namespace pkNX.Containers;

/// <summary>
/// <see cref="SARC"/> File Name Table
/// </summary>
public class SFNT
{
    public const string Identifier = nameof(SFNT);

    /// <summary>
    /// The required <see cref="Magic"/> matches the first 4 bytes of the file data.
    /// </summary>
    public bool SigMatches => Magic == Identifier;

    public string Magic = Identifier;
    public ushort HeaderSize;
    public ushort Reserved;
    public uint StringOffset;

    public SFNT() { }

    public SFNT(BinaryReader br)
    {
        Magic = new string(br.ReadChars(4));
        if (!SigMatches)
            throw new FormatException(nameof(SFNT));

        HeaderSize = br.ReadUInt16();
        Reserved = br.ReadUInt16();
        StringOffset = (uint)br.BaseStream.Position;
    }

    public void Write(BinaryWriter bw)
    {
        foreach (var c in Magic)
            bw.Write((byte)c);
        bw.Write(HeaderSize);
        bw.Write(Reserved);
        StringOffset = (uint)bw.BaseStream.Position;
    }
}
