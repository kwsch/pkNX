using System;
using System.ComponentModel;
using FlatSharp.Attributes;

// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global

namespace pkNX.Structures.FlatBuffers;

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class TrinityFileSystemMetadata
{
    [FlatBufferItem(00)] public ulong[] FileHashes { get; set; } = Array.Empty<ulong>();
    [FlatBufferItem(01)] public ulong[] FileOffsets { get; set; } = Array.Empty<ulong>();

    public ulong GetFileOffset(ulong hashFnv)
    {
        var index = Array.BinarySearch(FileHashes, hashFnv);
        if (index < 0)
            throw new ArgumentException(null, nameof(hashFnv));
        return FileOffsets[index];
    }

    public bool HasFile(ulong hashFnv) => Array.BinarySearch(FileHashes, hashFnv) >= 0;
}
