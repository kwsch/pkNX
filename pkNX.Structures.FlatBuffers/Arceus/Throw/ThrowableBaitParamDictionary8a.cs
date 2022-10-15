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
public class ThrowableBaitParamDictionary8a : IFlatBufferArchive<ThrowableBaitParamEntry8a>
{
    public byte[] Write() => FlatBufferConverter.SerializeFrom(this);

    [FlatBufferItem(0)] public ThrowableBaitParamEntry8a[] Table { get; set; } = Array.Empty<ThrowableBaitParamEntry8a>();
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class ThrowableBaitParamEntry8a
{
    [FlatBufferItem(00)] public ulong Hash_00 { get; set; }
    [FlatBufferItem(01)] public string Field_01 { get; set; } = string.Empty;
    [FlatBufferItem(02)] public string Field_02 { get; set; } = string.Empty;
    [FlatBufferItem(03)] public string Field_03 { get; set; } = string.Empty;
    [FlatBufferItem(04)] public int Unused { get; set; }
    [FlatBufferItem(05)] public int Field_05 { get; set; }
    [FlatBufferItem(06)] public int Field_06 { get; set; }
    [FlatBufferItem(07)] public string Field_07 { get; set; } = string.Empty;
}
