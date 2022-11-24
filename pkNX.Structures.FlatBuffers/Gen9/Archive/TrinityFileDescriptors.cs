using System;
using System.ComponentModel;
using System.Diagnostics;
using FlatSharp.Attributes;

// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global

namespace pkNX.Structures.FlatBuffers;

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class TrinityFileDescriptors
{
    [FlatBufferItem(00)] public ulong[] SubFileHashes { get; set; } = Array.Empty<ulong>();
    [FlatBufferItem(01)] public string[] FilePaths { get; set; } = Array.Empty<string>();
    [FlatBufferItem(02)] public TrinityFileDescriptorSubFileInfo[] SubFileInfos { get; set; } = Array.Empty<TrinityFileDescriptorSubFileInfo>();
    [FlatBufferItem(03)] public TrinityFileDescriptorInfo[] FileInfos { get; set; } = Array.Empty<TrinityFileDescriptorInfo>();

    public ulong GetSubFileIndex(ulong hash)
    {
        var index = Array.BinarySearch(SubFileHashes, hash);
        Debug.Assert((uint)index < SubFileHashes.Length);
        return SubFileInfos[index].Index;
    }

    public bool GetHasSubFile(ulong hash)
    {
        var index = Array.BinarySearch(SubFileHashes, hash);
        return index >= 0;
    }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class TrinityFileDescriptorSubFileInfo
{
    [FlatBufferItem(00)] public ulong Index { get; set; }
    [FlatBufferItem(01)] public TrinityFileDescriptorSubFileUnknown SubInfo { get; set; } = new();
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class TrinityFileDescriptorInfo
{
    [FlatBufferItem(00)] public ulong FileSize { get; set; }
    [FlatBufferItem(01)] public ulong FileCount { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class TrinityFileDescriptorSubFileUnknown
{
    [FlatBufferItem(00)] public uint Field_00 { get; set; } // TODO
}
