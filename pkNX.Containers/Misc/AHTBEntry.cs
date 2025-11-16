using System;
using System.IO;
using static System.Buffers.Binary.BinaryPrimitives;

namespace pkNX.Containers;

/// <summary>
/// An entry in an AHTB.
/// </summary>
public sealed record AHTBEntry
{
    public ulong Hash { get; set; } // fnv1a_64 hash of Name
    public string Name { get; set; } // u16 length + utf8 bytes + \0

    public static AHTBEntry Read(ReadOnlySpan<byte> data, out int used)
    {
        var Hash = ReadUInt64LittleEndian(data[..8]);
        var length = ReadUInt16LittleEndian(data.Slice(8, 2));
        var Name = System.Text.Encoding.UTF8.GetString(data.Slice(10, length - 1));
        used = 10 + length;
        return new AHTBEntry(Hash, Name);
    }

    public AHTBEntry(BinaryReader br)
    {
        Hash = br.ReadUInt64();
        var length = br.ReadUInt16();
        Span<byte> nameBytes = stackalloc byte[length];
        _ = br.Read(nameBytes);
        Name = System.Text.Encoding.UTF8.GetString(nameBytes[..^1]);
        //Debug.Assert(FnvHash.HashFnv1a_64(Name) == Hash);
        //Debug.Assert(Name.Length + 1 == NameLength); // Always null terminated
    }

    public AHTBEntry(ulong hash, string name)
    {
        Hash = hash;
        Name = name;
        //Debug.Assert(FnvHash.HashFnv1a_64(Name) == Hash);
    }

    public void Write(BinaryWriter bw)
    {
        bw.Write((ulong)Hash);
        var data = System.Text.Encoding.UTF8.GetBytes(Name);
        bw.Write((ushort)(data.Length + 1)); // +1 for null terminator
        bw.Write(data);
        bw.Write((byte)0); // \0 terminator
    }

    public static ulong GetHash(ReadOnlySpan<char> name) => FnvHash.HashFnv1a_64(name);
    public ulong Update(string name) => Hash = GetHash(Name = name);
    public ulong Update() => Hash = GetHash(Name);

    public override string ToString() => $"0x{Hash:X16}|{Name}";
}
