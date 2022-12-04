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
public class PokeFieldObstructionWazaNsLegendArchive8a : IFlatBufferArchive<PokeFieldObstructionWazaNsLegend8a>
{
    public byte[] Write() => FlatBufferConverter.SerializeFrom(this);

    [FlatBufferItem(00)] public PokeFieldObstructionWazaNsLegend8a[] Table { get; set; } = Array.Empty<PokeFieldObstructionWazaNsLegend8a>();
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class PokeFieldObstructionWazaNsLegend8a
{

    [FlatBufferItem(00)] public uint Field_00 { get; set; }
    [FlatBufferItem(01)] public uint Field_01 { get; set; }
    [FlatBufferItem(02)] public uint Field_02 { get; set; }
    [FlatBufferItem(03)] public uint Field_03 { get; set; }
    [FlatBufferItem(04)] public uint Field_04 { get; set; }
    [FlatBufferItem(05)] public uint Field_05 { get; set; }
    [FlatBufferItem(06)] public uint Field_06 { get; set; }
    [FlatBufferItem(07)] public uint Field_07 { get; set; }
    [FlatBufferItem(08)] public uint Field_08 { get; set; }
    [FlatBufferItem(09)] public uint Field_09 { get; set; }
    [FlatBufferItem(10)] public uint Field_10 { get; set; }
    [FlatBufferItem(11)] public uint Field_11 { get; set; }
    [FlatBufferItem(12)] public uint Field_12 { get; set; }
    [FlatBufferItem(13)] public uint Field_13 { get; set; }
    [FlatBufferItem(14)] public uint Field_14 { get; set; }
    [FlatBufferItem(15)] public uint Field_15 { get; set; }
    [FlatBufferItem(16)] public uint Field_16 { get; set; }
    [FlatBufferItem(17)] public uint Field_17 { get; set; }
    [FlatBufferItem(18)] public uint Field_18 { get; set; }
    [FlatBufferItem(19)] public uint Field_19 { get; set; }
    [FlatBufferItem(20)] public uint Field_20 { get; set; }
    [FlatBufferItem(21)] public uint Field_21 { get; set; }
    [FlatBufferItem(22)] public uint Field_22 { get; set; }
    [FlatBufferItem(23)] public uint Field_23 { get; set; }
    [FlatBufferItem(24)] public uint Field_24 { get; set; }
    [FlatBufferItem(25)] public uint Field_25 { get; set; }
    [FlatBufferItem(26)] public uint Field_26 { get; set; }
    [FlatBufferItem(27)] public uint Field_27 { get; set; }
    [FlatBufferItem(28)] public uint Field_28 { get; set; }
    [FlatBufferItem(29)] public uint Field_29 { get; set; }
    [FlatBufferItem(30)] public uint Field_30 { get; set; }
    [FlatBufferItem(31)] public uint Field_31 { get; set; }
    [FlatBufferItem(32)] public uint Field_32 { get; set; }
    [FlatBufferItem(33)] public uint Field_33 { get; set; }
    [FlatBufferItem(34)] public uint Field_34 { get; set; }
    [FlatBufferItem(35)] public uint Field_35 { get; set; }
    [FlatBufferItem(36)] public uint Field_36 { get; set; }
    [FlatBufferItem(37)] public uint Field_37 { get; set; }
    [FlatBufferItem(38)] public uint Field_38 { get; set; }
    [FlatBufferItem(39)] public uint Field_39 { get; set; }
    [FlatBufferItem(40)] public uint Field_40 { get; set; }
    [FlatBufferItem(41)] public uint Field_41 { get; set; }
    [FlatBufferItem(42)] public uint Field_42 { get; set; }
    [FlatBufferItem(43)] public uint Field_43 { get; set; }
    [FlatBufferItem(44)] public uint Field_44 { get; set; }
    [FlatBufferItem(45)] public uint Field_45 { get; set; }
    [FlatBufferItem(46)] public uint Field_46 { get; set; }
    [FlatBufferItem(47)] public uint Field_47 { get; set; }
    [FlatBufferItem(48)] public uint Field_48 { get; set; }
}
