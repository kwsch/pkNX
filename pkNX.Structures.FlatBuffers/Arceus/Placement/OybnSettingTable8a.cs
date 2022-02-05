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
public class OybnSettingTable8a : IFlatBufferArchive<OybnSetting8a>
{
    public byte[] Write() => FlatBufferConverter.SerializeFrom(this);

    [FlatBufferItem(0)] public OybnSetting8a[] Table { get; set; } = Array.Empty<OybnSetting8a>();
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class OybnSetting8a
{
    [FlatBufferItem(00)] public ulong Hash { get; set; }
    [FlatBufferItem(01)] public int Field_01 { get; set; }
    [FlatBufferItem(02)] public int Field_02 { get; set; }
    [FlatBufferItem(03)] public int Field_03 { get; set; }
    [FlatBufferItem(04)] public int Field_04 { get; set; }
    [FlatBufferItem(05)] public int Field_05 { get; set; }
}
