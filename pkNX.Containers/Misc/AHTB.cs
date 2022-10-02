using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;

namespace pkNX.Containers;

/// <summary>
/// Asynchronous Hash TaBle containing file names and their corresponding hashes.
/// </summary>
/// <remarks>gfl::container::HashTable</remarks>
[Serializable]
public class AHTB : Dictionary<ulong, string>
{
    public readonly AHTBEntry[] Entries;
    public const uint Magic = 0x42544841; // AHTB
    public static bool IsAHTB(byte[] data) => BitConverter.ToUInt32(data, 0) == Magic;

    public AHTB(byte[] table)
    {
        using var ms = new MemoryStream(table);
        using var br = new BinaryReader(ms);
        var magic = br.ReadUInt32();
        Debug.Assert(magic == Magic);
        var count = br.ReadUInt32();

        Entries = new AHTBEntry[count];
        for (int i = 0; i < count; i++)
        {
            var e = new AHTBEntry(br);
            Entries[i] = e;
            if (!ContainsKey(e.Hash))
                Add(e.Hash, e.Name);
        }
    }

    public int GetIndex(ulong hash)
    {
        if (!TryGetValue(hash, out _))
            return -1;
        return Array.FindIndex(Entries, z => z.Hash == hash);
    }

    protected AHTB(SerializationInfo info, StreamingContext context)
    {
        Entries = this.Select(z => new AHTBEntry(z.Key, (ushort) z.Value.Length, z.Value)).ToArray();
    }

    public int GetIndex(string value) => GetIndex(FnvHash.HashFnv1a_64(value));

    public IEnumerable<string> Summary => Entries.Select((z, i) => $"{i:0000}\t{z.Summary}");
    public IEnumerable<string> ShortSummary => Entries.Select(z => z.Summary);

    public Dictionary<ulong, string> ToDictionary()
    {
        var map = new Dictionary<ulong, string>();
        foreach (var entry in Entries)
            map[entry.Hash] = entry.Name;
        return map;
    }
}
