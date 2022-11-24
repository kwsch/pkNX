using FlatSharp.Attributes;
using System.ComponentModel;
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global
#nullable disable

namespace pkNX.Structures.FlatBuffers;

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class EventBattlePokemonArray : IFlatBufferArchive<EventBattlePokemon>
{
    [FlatBufferItem(0)] public EventBattlePokemon[] Table { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class EventBattlePokemon
{
    [FlatBufferItem(0)] public string Label { get; set; }
    [FlatBufferItem(1)] public PokeDataEventBattle PokeData { get; set; }
    [FlatBufferItem(2)] public bool DisableBattleOut { get; set; }
    [FlatBufferItem(3)] public bool EventEncount { get; set; }
}
