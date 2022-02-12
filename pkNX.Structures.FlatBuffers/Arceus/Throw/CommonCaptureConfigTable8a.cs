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
public class CommonCaptureConfigTable8a : IFlatBufferArchive<CommonCaptureConfig8a>
{
    public byte[] Write() => FlatBufferConverter.SerializeFrom(this);

    [FlatBufferItem(0)] public CommonCaptureConfig8a[] Table { get; set; } = Array.Empty<CommonCaptureConfig8a>();
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class CommonCaptureConfig8a
{
    [FlatBufferItem(00)] public string Name { get; set; } = string.Empty;
    [FlatBufferItem(01)] public ulong Hash { get; set; }
    [FlatBufferItem(02)] public int Type { get; set; }
    [FlatBufferItem(03)] public string String_03 { get; set; } = string.Empty;
    [FlatBufferItem(04)] public string String_04 { get; set; } = string.Empty;
    [FlatBufferItem(05)] public string[] String_05 { get; set; } = Array.Empty<string>();
    [FlatBufferItem(06)] public string[] String_06 { get; set; } = Array.Empty<string>();
}
