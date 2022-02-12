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
public class HaShopTable8a : IFlatBufferArchive<HaShopItem8a>
{
    public byte[] Write() => FlatBufferConverter.SerializeFrom(this);

    [FlatBufferItem(0)] public HaShopItem8a[] Table { get; set; } = Array.Empty<HaShopItem8a>();
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class HaShopItem8a
{
    [FlatBufferItem(00)] public ulong Field_00 { get; set; }
    [FlatBufferItem(01)] public ulong Field_01 { get; set; }
    [FlatBufferItem(02)] public int ItemID { get; set; }
}
