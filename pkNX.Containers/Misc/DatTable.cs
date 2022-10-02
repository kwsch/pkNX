using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;

namespace pkNX.Containers;

/// <summary>
/// Data Table for Hash->Value lookup
/// </summary>
/// <seealso cref="AHTB"/> for similar structure.
[Serializable]
public class DatTable : Dictionary<ulong, int>
{
    // u32 count
    // hash-val tuple[count]

    // hash-val tuple: size=0x10
    // - 8byte hash
    // - u32(?) index
    // - u32(?) unk

    public static bool IsDatTable(byte[] bytes)
    {
        // pretty weak, don't call this if you aren't sure!
        var count = BitConverter.ToUInt32(bytes, 0);
        return 4 + (count * 0x10) == bytes.Length;
    }

    private readonly DatEntry[] Entries;

    public DatTable(byte[] data)
    {
        using var ms = new MemoryStream(data);
        using var br = new BinaryReader(ms);
        var count = br.ReadInt32();

        Entries = new DatEntry[count];
        for (int i = 0; i < Entries.Length; i++)
        {
            var e = new DatEntry(br);
            Entries[i] = e;
            if (!ContainsKey(e.Hash))
                Add(e.Hash, e.Value);
        }
    }

    public int GetIndex(ulong hash)
    {
        if (!TryGetValue(hash, out _))
            return -1;
        return Array.FindIndex(Entries, z => z.Hash == hash);
    }

    protected DatTable(SerializationInfo info, StreamingContext context)
    {
        Entries = this.Select(z => new DatEntry(z.Key, z.Value)).ToArray();
    }

    public int GetIndex(string value) => GetIndex(FnvHash.HashFnv1a_64(value));

    public IEnumerable<string> Summary => Entries.Select((z, i) => $"{i:0000}\t{z.Summary}");
    public IEnumerable<string> ShortSummary => Entries.Select(z => z.Summary);
}
