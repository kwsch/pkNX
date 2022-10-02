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
public class PokeDropItemArchive8a : IFlatBufferArchive<PokeDropItem8a>
{
    public byte[] Write() => FlatBufferConverter.SerializeFrom(this);

    [FlatBufferItem(0)] public PokeDropItem8a[] Table { get; set; } = Array.Empty<PokeDropItem8a>();
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class PokeDropItem8a
{
    [FlatBufferItem(00)] public ulong Hash { get; set; }
    [FlatBufferItem(01)][TypeConverter(typeof(ItemConverter))] public int RegularItem { get; set; }
    [FlatBufferItem(02)] public int RegularItemProbability { get; set; }
    [FlatBufferItem(03)][TypeConverter(typeof(ItemConverter))] public int RareItem { get; set; }
    [FlatBufferItem(04)] public int RareItemProbability { get; set; }

    public string Dump(string[] itemNames) => $"{Hash:X16}\t{itemNames[RegularItem]}\t{RegularItemProbability}\t{itemNames[RareItem]}\t{RareItemProbability}";
}
