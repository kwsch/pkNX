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
public class ThrowableResourceSetDictionary8a : IFlatBufferArchive<ThrowableResourceSetEntry8a>
{
    public byte[] Write() => FlatBufferConverter.SerializeFrom(this);

    [FlatBufferItem(0)] public ThrowableResourceSetEntry8a[] Table { get; set; } = Array.Empty<ThrowableResourceSetEntry8a>();
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class ThrowableResourceSetEntry8a
{
    [FlatBufferItem(00)] public ulong Hash_00 { get; set; }
    [FlatBufferItem(01)] public bool  Flag    { get; set; }
    [FlatBufferItem(02)] public ulong Hash_02 { get; set; }
    [FlatBufferItem(03)] public ulong Hash_03 { get; set; }
    [FlatBufferItem(04)] public ulong Hash_04 { get; set; }
    [FlatBufferItem(05)] public ulong Hash_05 { get; set; }
    [FlatBufferItem(06)] public ulong Hash_06 { get; set; }
    [FlatBufferItem(07)] public string SoundDrop { get; set; } = string.Empty;
    [FlatBufferItem(08)] public string SoundFly  { get; set; } = string.Empty;
}
