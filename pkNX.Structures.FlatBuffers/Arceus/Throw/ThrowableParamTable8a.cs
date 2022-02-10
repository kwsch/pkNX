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
public class ThrowableParamTable8a : IFlatBufferArchive<ThrowableParam8a>
{
    public byte[] Write() => FlatBufferConverter.SerializeFrom(this);

    [FlatBufferItem(0)] public ThrowableParam8a[] Table { get; set; } = Array.Empty<ThrowableParam8a>();
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class ThrowableParam8a
{
    [FlatBufferItem(00)] public int    ItemID { get; set; }
    [FlatBufferItem(01)] public ulong  Hash_01 { get; set; }
    [FlatBufferItem(02)] public ulong  ThrowPermissionSet  { get; set; }
    [FlatBufferItem(03)] public ulong  ThrowParam          { get; set; }
    [FlatBufferItem(04)] public ulong  Hash_04 { get; set; }
    [FlatBufferItem(05)] public int    Field_05 { get; set; }
    [FlatBufferItem(06)] public float  Field_06 { get; set; }
    [FlatBufferItem(07)] public string Label_01 { get; set; } = string.Empty;
    [FlatBufferItem(08)] public string Label_02 { get; set; } = string.Empty;
    [FlatBufferItem(09)] public float  Field_09 { get; set; }
}
