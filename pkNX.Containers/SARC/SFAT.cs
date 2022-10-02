using System;
using System.Collections.Generic;
using System.IO;

namespace pkNX.Containers;

/// <summary>
/// <see cref="SARC"/> File Access Table
/// </summary>
public class SFAT
{
    public const string Identifier = nameof(SFAT);

    /// <summary>
    /// The required <see cref="Magic"/> matches the first 4 bytes of the file data.
    /// </summary>
    public bool SigMatches => Magic == Identifier;

    private readonly string Magic = Identifier;
    private readonly ushort HeaderSize = 0xC;
    private readonly ushort EntryCount;
    private readonly uint HashMult = 0x65;
    public readonly List<SFATEntry> Entries;

    public SFATEntry this[int index]
    {
        get => Entries[index];
        set => Entries[index] = value;
    }

    public SFAT(string baseFolder, IReadOnlyList<string> files)
    {
        EntryCount = (ushort)files.Count;
        Entries = new List<SFATEntry>(EntryCount);
        for (int i = 0; i < EntryCount; i++)
        {
            Entries[i] = new SFATEntry {File = files[i]};

            var fn = files[i].Remove(baseFolder.Length);
            Entries[i].SetFileName(fn, HashMult);
        }
    }

    public SFAT(BinaryReader br, int DataOffset)
    {
        Magic = new string(br.ReadChars(4));
        if (!SigMatches)
            throw new FormatException(nameof(SFAT));

        HeaderSize = br.ReadUInt16();
        EntryCount = br.ReadUInt16();
        HashMult = br.ReadUInt32();
        Entries = new List<SFATEntry>(EntryCount);

        for (int i = 0; i < EntryCount; i++)
            Entries.Add(new SFATEntry(br, DataOffset));
    }

    public string GetFileName(int index, Stream parent, int StringOffset) => this[index].GetFileName(parent, StringOffset);
    public void SetFileName(int index, string value) => this[index].SetFileName(value, HashMult);

    public void Write(BinaryWriter bw)
    {
        foreach (var c in Magic)
            bw.Write((byte)c);
        bw.Write(HeaderSize);
        bw.Write(EntryCount);
        bw.Write(HashMult);
        WriteEntries(bw);
    }

    public void WriteEntries(BinaryWriter bw)
    {
        foreach (var entry in Entries)
            entry.Write(bw);
    }
}
