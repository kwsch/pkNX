using System.ComponentModel;
using FlatSharp.Attributes;
// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedType.Global
#nullable disable

namespace pkNX.Structures.FlatBuffers;

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class AjitoUnitArray : IFlatBufferArchive<AjitoUnit>
{
    [FlatBufferItem(0)] public AjitoUnit[] Table { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class AjitoUnit
{
    [FlatBufferItem(0)] public AjitoUnitTable AjitoUnitTable { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class AjitoUnitTable
{
    [FlatBufferItem(00)] public AjitoTypeEnum AjitoTypeEnum { get; set; }
    [FlatBufferItem(01)] public AjitoDifficultEnum AjitoDifficultEnum { get; set; }
    [FlatBufferItem(02)] public int PokemonId1 { get; set; }
    [FlatBufferItem(03)] public int PokemonId2 { get; set; }
    [FlatBufferItem(04)] public int PokemonId3 { get; set; }
    [FlatBufferItem(05)] public int PokemonId4 { get; set; }
    [FlatBufferItem(06)] public int PokemonId5 { get; set; }
    [FlatBufferItem(07)] public int PokemonId6 { get; set; }
    [FlatBufferItem(08)] public int PokemonId7 { get; set; }
    [FlatBufferItem(09)] public int PokemonId8 { get; set; }
    [FlatBufferItem(10)] public int PokemonId9 { get; set; }
    [FlatBufferItem(11)] public int PokemonId10 { get; set; }
}
