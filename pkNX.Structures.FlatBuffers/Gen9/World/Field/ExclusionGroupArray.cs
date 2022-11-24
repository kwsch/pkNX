using System.ComponentModel;
using FlatSharp.Attributes;
// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedType.Global
#nullable disable

namespace pkNX.Structures.FlatBuffers;

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class ExclusionGroupArray : IFlatBufferArchive<ExclusionGroup>
{
    [FlatBufferItem(0)] public ExclusionGroup[] Table { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class ExclusionGroup
{
    [FlatBufferItem(0)] public string Id { get; set; }
    [FlatBufferItem(1)] public ExclusionContentSet Content { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class ExclusionContentSet
{
    [FlatBufferItem(0)] public bool RaidGem { get; set; }
    [FlatBufferItem(1)] public bool Item { get; set; }
    [FlatBufferItem(2)] public bool SymbolPokemon { get; set; }
    [FlatBufferItem(3)] public bool CommonNpc { get; set; }
    [FlatBufferItem(4)] public bool TrafficNpc { get; set; }
    [FlatBufferItem(5)] public bool CoinPokemon { get; set; }
    [FlatBufferItem(6)] public bool PartnerPokemon { get; set; }
    [FlatBufferItem(7)] public bool Picnic { get; set; }
    [FlatBufferItem(8)] public bool Rotom { get; set; }
    [FlatBufferItem(9)] public bool OtherPlayerThings { get; set; }
}
