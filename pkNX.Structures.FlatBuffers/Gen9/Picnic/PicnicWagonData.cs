using System.ComponentModel;
using FlatSharp.Attributes;
// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedType.Global
#nullable disable

namespace pkNX.Structures.FlatBuffers;

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class PicnicWagonData
{
    [FlatBufferItem(0)] public int WagonSize { get; set; }
    [FlatBufferItem(1)] public PokemonEggCommonData EggCommonData { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class PokemonEggCommonData
{
    [FlatBufferItem(0)] public float CreateEggTimer { get; set; }
    [FlatBufferItem(1)] public float CreateEggTimerShort { get; set; }
    [FlatBufferItem(2)] public LoveLevelProb CreateEggProb { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class LoveLevelProb
{
    [FlatBufferItem(0)] public float Good { get; set; }
    [FlatBufferItem(1)] public float Normal { get; set; }
    [FlatBufferItem(2)] public float Bad { get; set; }
    [FlatBufferItem(3)] public float Worst { get; set; }
}
