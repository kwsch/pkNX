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
public class PlayReportTable8a : IFlatBufferArchive<PlayReportItem8a>
{
    public byte[] Write() => FlatBufferConverter.SerializeFrom(this);

    [FlatBufferItem(0)] public PlayReportItem8a[] Table { get; set; } = Array.Empty<PlayReportItem8a>();
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class PlayReportItem8a
{
    [FlatBufferItem(00)] public string Field_00 { get; set; } = string.Empty;
    [FlatBufferItem(01)] public ulong Field_01 { get; set; }
    [FlatBufferItem(02)] public ulong Field_02 { get; set; }
    [FlatBufferItem(03)] public ulong Field_03 { get; set; }
    [FlatBufferItem(04)] public ulong Field_04 { get; set; }
    [FlatBufferItem(05)] public ulong Field_05 { get; set; }
    [FlatBufferItem(06)] public ulong Field_06 { get; set; }
    [FlatBufferItem(07)] public ulong Field_07 { get; set; }
    [FlatBufferItem(08)] public ulong Field_08 { get; set; }
    [FlatBufferItem(09)] public ulong Field_09 { get; set; }
    [FlatBufferItem(10)] public ulong Field_10 { get; set; }
    [FlatBufferItem(11)] public ulong Field_11 { get; set; }
    [FlatBufferItem(12)] public ulong Field_12 { get; set; }
    [FlatBufferItem(13)] public ulong Field_13 { get; set; }
    [FlatBufferItem(14)] public ulong Field_14 { get; set; }
    [FlatBufferItem(15)] public ulong Field_15 { get; set; }
    [FlatBufferItem(16)] public ulong Field_16 { get; set; }
    [FlatBufferItem(17)] public ulong Field_17 { get; set; }
    [FlatBufferItem(18)] public ulong Field_18 { get; set; }
    [FlatBufferItem(19)] public ulong Field_19 { get; set; }
    [FlatBufferItem(20)] public ulong Field_20 { get; set; }
    [FlatBufferItem(21)] public ulong Field_21 { get; set; }
}
