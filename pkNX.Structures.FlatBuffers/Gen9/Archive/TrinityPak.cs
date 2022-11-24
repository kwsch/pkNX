using System;
using System.ComponentModel;
using System.Diagnostics;
using FlatSharp.Attributes;
using pkNX.Containers;

// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global

namespace pkNX.Structures.FlatBuffers;

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class TrinityPak
{
    [FlatBufferItem(00)] public ulong[] Hashes { get; set; } = Array.Empty<ulong>();
    [FlatBufferItem(01)] public TrinityPakFileData[] Files { get; set; } = Array.Empty<TrinityPakFileData>();

    public TrinityPakFileData GetFileData(ulong hash)
    {
        var index = Array.BinarySearch(Hashes, hash);
        Debug.Assert((uint)index < Hashes.Length);
        return Files[index];
    }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class TrinityPakFileData
{
    [FlatBufferItem(00)] public uint Field_00 { get; set; }
    [FlatBufferItem(01)] public byte CompressionType { get; set; }
    [FlatBufferItem(02)] public byte Field_02 { get; set; }
    [FlatBufferItem(03)] public ulong UncompressedSize { get; set; }
    [FlatBufferItem(04)] public byte[] Data { get; set; } = Array.Empty<byte>();

    public byte[] GetUncompressedData() => CompressionType switch
    {  // TODO: What specific ID is zlib?
        3 => Oodle.Decompress(Data, (long)UncompressedSize)!,  // Oodle
        0xFF => (byte[])Data.Clone(), // Uncompressed
        _ => throw new ArgumentException($"Unknown compression type {CompressionType}"),
    };

    public ReadOnlySpan<byte> GetUncompressedDataReadOnly() => CompressionType switch
    {  // TODO: What specific ID is zlib?
        3 => Oodle.Decompress(Data, (long)UncompressedSize)!,  // Oodle
        0xFF => Data, // Uncompressed
        _ => throw new ArgumentException($"Unknown compression type {CompressionType}"),
    };
}
