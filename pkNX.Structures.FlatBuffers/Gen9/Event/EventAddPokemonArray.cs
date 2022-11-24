using System.ComponentModel;
using FlatSharp.Attributes;
// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedType.Global
#nullable disable

namespace pkNX.Structures.FlatBuffers;

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class EventAddPokemonArray : IFlatBufferArchive<EventAddPokemon>
{
    [FlatBufferItem(0)] public EventAddPokemon[] Table { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class EventAddPokemon
{
    [FlatBufferItem(0)] public string Label { get; set; }
    [FlatBufferItem(1)] public PokeDataFull PokeData { get; set; }
    [FlatBufferItem(2)] public bool PokedexRegistration { get; set; }
}
