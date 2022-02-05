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
public class PokedexRankTable : IFlatBufferArchive<PokedexRankLevel>
{
    public byte[] Write() => FlatBufferConverter.SerializeFrom(this);

    [FlatBufferItem(0)] public PokedexRankLevel[] Table { get; set; } = Array.Empty<PokedexRankLevel>();
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class PokedexRankLevel
{
    [FlatBufferItem(0)] public int Rank { get; set; }
    [FlatBufferItem(1)] public int Required { get; set; }
}
