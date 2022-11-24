using System.ComponentModel;
using FlatSharp.Attributes;

// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedType.Global
#nullable disable

namespace pkNX.Structures.FlatBuffers;

[FlatBufferStruct, TypeConverter(typeof(ExpandableObjectConverter))]
public class AjitoCommonLevelStruct
{
    [FlatBufferItem(0)] public AjitoTypeEnum AjitoTypeEnum { get; set; }
    [FlatBufferItem(1)] public AjitoDifficultEnum AjitoDifficultEnum { get; set; }
    [FlatBufferItem(2)] public int ClearPokemonNum { get; set; }
    [FlatBufferItem(3)] public int MinPokemonLv { get; set; }
    [FlatBufferItem(4)] public int MaxPokemonLv { get; set; }
    [FlatBufferItem(5)] public int LpCoefficient { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class AjitoCommonLevelArray : IFlatBufferArchive<AjitoCommonLevel>
{
    [FlatBufferItem(0)] public AjitoCommonLevel[] Table { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class AjitoCommonLevel
{
    [FlatBufferItem(0)] public AjitoCommonLevelStruct AjitoCommonLevelStruct { get; set; }
}
