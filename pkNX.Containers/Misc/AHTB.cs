using pkNX.Containers.VFS;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace pkNX.Containers;

/// <summary>
/// Asynchronous Hash TaBle containing file names and their corresponding hashes.
/// </summary>
/// <remarks>gfl::container::HashTable</remarks>
[Serializable]
public class AHTB : IBinarySerializable
{
    public AHTBEntry[] Entries { get; private set; } = null!;
    public const uint Magic = 0x42544841; // AHTB

    public int Count => Entries.Length;

    public static bool IsAHTB(byte[] data) => BitConverter.ToUInt32(data, 0) == Magic;

    public AHTB(byte[] table)
    {
        using var ms = new MemoryStream(table);
        using var br = new BinaryReader(ms);
        Read(br);
    }

    public AHTB(Stream table)
    {
        using var br = new BinaryReader(table);
        Read(br);
    }

    public AHTB(Dictionary<ulong, string> source)
    {
        Entries = new AHTBEntry[source.Count];
        int i = 0;
        foreach (var entry in source)
        {
            Entries[i] = new AHTBEntry(entry.Key, (ushort)entry.Value.Length, entry.Value);
            ++i;
        }
    }

    public void Read(BinaryReader br)
    {
        var magic = br.ReadUInt32();
        Debug.Assert(magic == Magic);
        var count = br.ReadUInt32();

        Entries = new AHTBEntry[count];
        for (int i = 0; i < count; i++)
        {
            var e = new AHTBEntry(br);
            Entries[i] = e;
        }
    }

    public void Write(BinaryWriter bw)
    {
        bw.Write((uint)Magic);
        bw.Write((uint)Entries.Length);

        foreach (var entry in Entries)
            entry.Write(bw);
    }

    public int GetIndex(ulong hash)
    {
        return Array.FindIndex(Entries, z => z.Hash == hash);
    }

    public int GetIndex(string value) => GetIndex(FnvHash.HashFnv1a_64(value));

    public IEnumerable<string> Summary => Entries.Select((z, i) => $"{i:0000}|{z}");
    public IEnumerable<string> ShortSummary => Entries.Select(z => z.ToString());

    public Dictionary<ulong, string> ToDictionary()
    {
        var map = new Dictionary<ulong, string>();
        foreach (var entry in Entries)
            map[entry.Hash] = entry.Name;
        return map;
    }
}
