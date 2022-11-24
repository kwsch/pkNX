using System.ComponentModel;
using FlatSharp.Attributes;
// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedType.Global
#nullable disable

namespace pkNX.Structures.FlatBuffers;

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class EventTradePokemonArray : IFlatBufferArchive<EventTradePokemon>
{
    [FlatBufferItem(0)] public EventTradePokemon[] Table { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class EventTradePokemon
{
    [FlatBufferItem(0)] public string Label { get; set; }
    [FlatBufferItem(1)] public PokeDataTrade PokeData { get; set; }
}
