using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using pkNX.Containers.VFS;
using static System.Buffers.Binary.BinaryPrimitives;

namespace pkNX.Containers;

/// <summary>
/// Asynchronous Hash TaBle containing file names and their corresponding hashes.
/// </summary>
/// <remarks>gfl::container::HashTable</remarks>
[Serializable]
public class AHTB : IBinarySerializable
{
    /// <summary>
    /// Indexed list of entries in this AHTB.
    /// </summary>
    public List<AHTBEntry> Entries { get; } = [];

    public const uint Magic = 0x42544841; // AHTB

    public int Count => Entries.Count;

    /// <summary>
    /// Determines whether the specified data begins with the predefined magic number.
    /// </summary>
    /// <param name="data">A read-only span of bytes to check.</param>
    /// <returns><see langword="true"/> if the first four bytes of <paramref name="data"/> match the magic number; otherwise, <see langword="false"/>.</returns>
    public static bool IsAHTB(ReadOnlySpan<byte> data) => ReadUInt32LittleEndian(data) == Magic;

    public AHTB(ReadOnlySpan<byte> table)
    {
        var magic = ReadUInt32LittleEndian(table);
        Debug.Assert(magic == Magic);

        var count = ReadInt32LittleEndian(table.Slice(4, 4));
        Entries.EnsureCapacity(count);

        int offset = 8;
        for (int i = 0; i < count; i++)
        {
            var span = table[offset..];
            Entries.Add(AHTBEntry.Read(span, out var used));
            offset += used;
        }
    }

    public AHTB(Stream table)
    {
        using var br = new BinaryReader(table);
        Read(br);
    }

    public AHTB(Dictionary<ulong, string> source)
    {
        Entries.EnsureCapacity(source.Count);
        foreach (var entry in source)
            Entries.Add(new AHTBEntry(entry.Key, entry.Value));
    }

    public void Read(BinaryReader br)
    {
        var magic = br.ReadUInt32();
        Debug.Assert(magic == Magic);

        var count = br.ReadInt32();
        Entries.EnsureCapacity(count);

        for (int i = 0; i < count; i++)
            Entries.Add(new AHTBEntry(br));
    }

    public byte[] Write()
    {
        using var ms = new MemoryStream();
        using var bw = new BinaryWriter(ms);
        Write(bw);
        return ms.ToArray();
    }

    public void Write(BinaryWriter bw)
    {
        bw.Write(Magic);
        bw.Write(Entries.Count);

        foreach (var entry in Entries)
            entry.Write(bw);
    }

    public int GetIndex(ulong hash) => Entries.FindIndex(z => z.Hash == hash);
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

    public Dictionary<ulong, string> ToDictionary(ReadOnlySpan<string> resource)
    {
        var count = Math.Min(resource.Length, Entries.Count);
        var map = new Dictionary<ulong, string>(count);
        for (int i = 0; i < count; i++)
        {
            var value = resource[i];
            var hash = Entries[i].Hash;
            map[hash] = value;
        }
        return map;
    }

    public Dictionary<string, (string Name, int Index)> ToIndexedDictionary(ReadOnlySpan<string> value)
    {
        var map = new Dictionary<string, (string Name, int Index)>();
        for (var i = 0; i < value.Length; i++)
            map[Entries[i].Name] = (value[i], i);
        return map;
    }

    public string[] MergeFlat(ReadOnlySpan<string> lines)
    {
        var detailed = new string[Count];
        for (int i = 0; i < lines.Length; i++)
        {
            var entry = Entries[i];
            var hash = entry.Hash;
            var name = entry.Name;
            var line = lines[i];
            detailed[i] = $"{i:000}\t{hash:X16}\t{name}\t{line}";
        }
        return detailed;
    }

    public string GetString(ulong hash, IReadOnlyList<string> text, string fallback = "NO TEXT FOUND")
    {
        var entries = Entries;
        var index = entries.FindIndex(z => z.Hash == hash);
        if (index == -1)
            return fallback;
        return text[index];
    }
}
