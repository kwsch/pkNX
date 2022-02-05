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
public class ThrowParamTable8a : IFlatBufferArchive<ThrowParam8a>
{
    public byte[] Write() => FlatBufferConverter.SerializeFrom(this);

    [FlatBufferItem(0)] public ThrowParam8a[] Table { get; set; } = Array.Empty<ThrowParam8a>();
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class ThrowParam8a
{
    [FlatBufferItem(00)] public ulong Hash { get; set; }
    [FlatBufferItem(01)] public float Field_01 { get; set; }
    [FlatBufferItem(02)] public float Field_02 { get; set; }
    [FlatBufferItem(03)] public float Field_03 { get; set; }
    [FlatBufferItem(04)] public float Field_04 { get; set; }

    public string Dump() => $"{Hash:X16}\t{Field_01}\t{Field_02}\t{Field_03}\t{Field_04}";
}