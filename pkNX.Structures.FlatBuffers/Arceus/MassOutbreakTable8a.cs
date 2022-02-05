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
public class MassOutbreakTable8a : IFlatBufferArchive<MassOutbreak8a>
{
    public byte[] Write() => FlatBufferConverter.SerializeFrom(this);

    [FlatBufferItem(0)] public MassOutbreak8a[] Table { get; set; } = Array.Empty<MassOutbreak8a>();
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class MassOutbreak8a : IFlatBufferArchive<MassOutbreakInfo8a>
{
    [FlatBufferItem(0)] public ulong Hash { get; set; }
    [FlatBufferItem(1)] public int Field_01 { get; set; }
    [FlatBufferItem(2)] public MassOutbreakInfo8a[] Table { get; set; } = Array.Empty<MassOutbreakInfo8a>();
    [FlatBufferItem(3)] public string Field_03 { get; set; } = string.Empty;
    [FlatBufferItem(4)] public int Field_04 { get; set; }
    [FlatBufferItem(5)] public int Field_05 { get; set; }
    [FlatBufferItem(6)] public int Field_06 { get; set; }
    [FlatBufferItem(7)] public int Field_07 { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class MassOutbreakInfo8a
{
    [FlatBufferItem(0)] public ulong Hash_00 { get; set; }
    [FlatBufferItem(1)] public ulong Hash_01 { get; set; }
    [FlatBufferItem(2)] public string Field_02 { get; set; } = string.Empty;
    [FlatBufferItem(3)] public string Field_03 { get; set; } = string.Empty;
    [FlatBufferItem(4)] public string Field_04 { get; set; } = string.Empty;
    [FlatBufferItem(5)] public string Field_05 { get; set; } = string.Empty;
    [FlatBufferItem(6)] public string Field_06 { get; set; } = string.Empty;
}
