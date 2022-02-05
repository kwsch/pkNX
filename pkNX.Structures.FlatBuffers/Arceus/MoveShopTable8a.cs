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
public class MoveShopTable8a : IFlatBufferArchive<MoveShopIndex>
{
    public byte[] Write() => FlatBufferConverter.SerializeFrom(this);

    [FlatBufferItem(0)] public MoveShopIndex[] Table { get; set; } = Array.Empty<MoveShopIndex>();
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class MoveShopIndex
{
    [FlatBufferItem(0)] public int Index { get; set; }
    [FlatBufferItem(1)] public int Move { get; set; }
    [FlatBufferItem(2)] public int Price { get; set; }
    [FlatBufferItem(3)] public ulong Hash { get; set; }
}
