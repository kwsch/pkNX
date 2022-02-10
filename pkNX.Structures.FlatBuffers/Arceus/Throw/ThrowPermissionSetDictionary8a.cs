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
public class ThrowPermissionSetDictionary8a : IFlatBufferArchive<ThrowPermissionSetEntry8a>
{
    public byte[] Write() => FlatBufferConverter.SerializeFrom(this);

    [FlatBufferItem(0)] public ThrowPermissionSetEntry8a[] Table { get; set; } = Array.Empty<ThrowPermissionSetEntry8a>();
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class ThrowPermissionSetEntry8a
{
    [FlatBufferItem(00)] public ulong Hash_00 { get; set; }
    [FlatBufferItem(01)] public int Field_01 { get; set; }
    [FlatBufferItem(02)] public int Field_02 { get; set; }
    [FlatBufferItem(03)] public int Field_03 { get; set; }
    [FlatBufferItem(04)] public int Field_04 { get; set; } // unused
    [FlatBufferItem(05)] public int Field_05 { get; set; }
    [FlatBufferItem(06)] public int Field_06 { get; set; }
    [FlatBufferItem(07)] public int Field_07 { get; set; }
    [FlatBufferItem(08)] public int Field_08 { get; set; }
}
