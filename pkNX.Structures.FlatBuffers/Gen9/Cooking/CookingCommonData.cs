using System.ComponentModel;
using FlatSharp.Attributes;
// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedType.Global
#nullable disable

namespace pkNX.Structures.FlatBuffers;

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class CookingCommonData
{
    [FlatBufferItem(0)] public PopWildPokemonParam PopParam { get; set; }
}

[FlatBufferStruct, TypeConverter(typeof(ExpandableObjectConverter))]
public class PopWildPokemonParam
{
    // float[4]
    [FlatBufferItem(0)] public float OccurrenceProb0 { get; set; }
    [FlatBufferItem(1)] public float OccurrenceProb1 { get; set; }
    [FlatBufferItem(2)] public float OccurrenceProb2 { get; set; }
    [FlatBufferItem(3)] public float OccurrenceProb3 { get; set; }
}
