using System.ComponentModel;
using FlatSharp.Attributes;
// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedType.Global
#nullable disable

namespace pkNX.Structures.FlatBuffers;

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class TakaraSpicePowerTableArray : IFlatBufferArchive<TakaraSpicePowerTable>
{
    [FlatBufferItem(0)] public TakaraSpicePowerTable[] Table { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class TakaraSpicePowerTable
{
    [FlatBufferItem(0)] public TakaraSpicePowerParam Param { get; set; }
}

[FlatBufferStruct, TypeConverter(typeof(ExpandableObjectConverter))]
public class TakaraSpicePowerParam
{
    // SeasoningType[3]
    [FlatBufferItem(0)] public SeasoningType TakaraSeasoning0 { get; set; }
    [FlatBufferItem(1)] public SeasoningType TakaraSeasoning1 { get; set; }
    [FlatBufferItem(2)] public SeasoningType TakaraSeasoning2 { get; set; }

    [FlatBufferItem(3)] public FoodSkillType SkillType { get; set; }
}
