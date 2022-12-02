using System.ComponentModel;
using FlatSharp.Attributes;
// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedType.Global
#nullable disable

namespace pkNX.Structures.FlatBuffers;
[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class TreeShakePokemonArray : IFlatBufferArchive<TreeShakePokemon>
{
    [FlatBufferItem(0)] public TreeShakePokemon[] Table { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class TreeShakePokemon
{
    [FlatBufferItem(00)] public string Id { get; set; }
    [FlatBufferItem(01)] public TreeShakePokemonSelect Data1 { get; set; }
    [FlatBufferItem(02)] public TreeShakePokemonSelect Data2 { get; set; }
    [FlatBufferItem(03)] public TreeShakePokemonSelect Data3 { get; set; }
    [FlatBufferItem(04)] public TreeShakePokemonSelect Data4 { get; set; }
    [FlatBufferItem(05)] public TreeShakePokemonSelect Data5 { get; set; }
    [FlatBufferItem(06)] public TreeShakePokemonSelect Data6 { get; set; }
    [FlatBufferItem(07)] public TreeShakePokemonSelect Data7 { get; set; }
    [FlatBufferItem(08)] public TreeShakePokemonSelect Data8 { get; set; }
    [FlatBufferItem(09)] public TreeShakePokemonSelect Data9 { get; set; }
    [FlatBufferItem(10)] public TreeShakePokemonSelect Data10 { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class TreeShakePokemonSelect
{
    [FlatBufferItem(0)] public string Name { get; set; }
    [FlatBufferItem(1)] public uint Probability { get; set; }
}
