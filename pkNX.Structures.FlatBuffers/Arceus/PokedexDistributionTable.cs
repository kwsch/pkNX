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
public class PokedexDistributionTable : IFlatBufferArchive<PokedexDistributionEntry>
{
    public byte[] Write() => FlatBufferConverter.SerializeFrom(this);

    [FlatBufferItem(0)] public PokedexDistributionEntry[] Table { get; set; } = Array.Empty<PokedexDistributionEntry>();
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class PokedexDistributionEntry
{
    [FlatBufferItem(00)] public int Species { get; set; }
    [FlatBufferItem(01)] public int Field_01 { get; set; }
    [FlatBufferItem(02)] public int Field_02 { get; set; }
    [FlatBufferItem(03)] public int Field_03 { get; set; }
    [FlatBufferItem(04)] public int Field_04 { get; set; }
    [FlatBufferItem(05)] public int Field_05 { get; set; }
    [FlatBufferItem(06)] public int Field_06 { get; set; }
    [FlatBufferItem(07)] public int Field_07 { get; set; }
    [FlatBufferItem(08)] public int Field_08 { get; set; }
    [FlatBufferItem(09)] public int Field_09 { get; set; }
    [FlatBufferItem(10)] public int Field_10 { get; set; }
    [FlatBufferItem(11)] public int Field_11 { get; set; }
    [FlatBufferItem(12)] public int Field_12 { get; set; }
    [FlatBufferItem(13)] public ulong Field_13 { get; set; }
    [FlatBufferItem(14)] public ulong Field_14 { get; set; }
    [FlatBufferItem(15)] public ulong Field_15 { get; set; }
    [FlatBufferItem(16)] public ulong Field_16 { get; set; }
    [FlatBufferItem(17)] public ulong Field_17 { get; set; }
    [FlatBufferItem(18)] public ulong Field_18 { get; set; }
    [FlatBufferItem(19)] public ulong Field_19 { get; set; }
    [FlatBufferItem(20)] public ulong Field_20 { get; set; }
    [FlatBufferItem(21)] public ulong Field_21 { get; set; }
    [FlatBufferItem(22)] public ulong Field_22 { get; set; }
    [FlatBufferItem(23)] public ulong Field_23 { get; set; }
    [FlatBufferItem(24)] public ulong Field_24 { get; set; }
    [FlatBufferItem(25)] public ulong Field_25 { get; set; }
    [FlatBufferItem(26)] public ulong Field_26 { get; set; }
    [FlatBufferItem(27)] public ulong Field_27 { get; set; }
    [FlatBufferItem(28)] public ulong Field_28 { get; set; }
    [FlatBufferItem(29)] public ulong Field_29 { get; set; }
    [FlatBufferItem(30)] public ulong Field_30 { get; set; }
    [FlatBufferItem(31)] public ulong Field_31 { get; set; }
    [FlatBufferItem(32)] public ulong Field_32 { get; set; }
    [FlatBufferItem(33)] public ulong Field_33 { get; set; }
}
