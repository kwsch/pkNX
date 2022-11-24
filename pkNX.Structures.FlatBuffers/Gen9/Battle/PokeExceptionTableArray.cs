using System.ComponentModel;
using FlatSharp.Attributes;
// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedType.Global
#nullable disable

namespace pkNX.Structures.FlatBuffers;

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class PokeExceptionTableArray : IFlatBufferArchive<PokeExceptionTable>
{
    [FlatBufferItem(0)] public PokeExceptionTable[] Table { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class PokeExceptionTable
{
    [FlatBufferItem(0)] public DevID DevId { get; set; }
    [FlatBufferItem(1)] public sbyte Formno { get; set; }
    [FlatBufferItem(2)] public GroundState DefaultBattleState { get; set; }
}

[FlatBufferEnum(typeof(sbyte))]
public enum GroundState : sbyte
{
    @default = 0,
    ground = 1,
    swim = 2,
    @float = 3,
}
