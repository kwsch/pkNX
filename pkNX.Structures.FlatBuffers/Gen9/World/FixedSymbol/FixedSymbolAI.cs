using System.ComponentModel;
using FlatSharp.Attributes;
// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedType.Global
#nullable disable

namespace pkNX.Structures.FlatBuffers;

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class FixedSymbolAI
{
    [FlatBufferItem(0)] public PokemonActionID ActionId { get; set; }
    [FlatBufferItem(1)] public float Hunger { get; set; }
    [FlatBufferItem(2)] public float Fatigue { get; set; }
    [FlatBufferItem(3)] public float Sleepiness { get; set; }
    [FlatBufferItem(4)] public int Priority { get; set; }
    [FlatBufferItem(5)] public PokemonActionID TriggerActionId { get; set; }
    [FlatBufferItem(6)] public BehaviorFrequency OverrideFrequency { get; set; }
}
