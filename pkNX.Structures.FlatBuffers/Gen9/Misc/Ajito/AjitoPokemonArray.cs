using System.ComponentModel;
using FlatSharp.Attributes;
// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedType.Global
#nullable disable

namespace pkNX.Structures.FlatBuffers;

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class AjitoPokemonArray : IFlatBufferArchive<AjitoPokemon>
{
    [FlatBufferItem(0)] public AjitoPokemon[] Table { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class AjitoPokemonTable
{
    [FlatBufferItem(0)] public int PokemonId { get; set; }
    [FlatBufferItem(1)] public DevID DevId { get; set; }
    [FlatBufferItem(2)] public int FormNo { get; set; }
}
[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class AjitoPokemon
{
    [FlatBufferItem(0)] public AjitoPokemonTable AjitoPokemonTable { get; set; }
}
