using System.ComponentModel;
using FlatSharp.Attributes;
// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedType.Global
#nullable disable

namespace pkNX.Structures.FlatBuffers;

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class ConditionSimpleAutoBattleHecklerAreaArray : IFlatBufferArchive<ConditionSimpleAutoBattleHecklerArea>
{
    [FlatBufferItem(0)] public ConditionSimpleAutoBattleHecklerArea[] Table { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class ConditionSimpleAutoBattleHecklerArea
{
    [FlatBufferItem(0)] public PokemonTriggerID TriggerID { get; set; }
    [FlatBufferItem(1)] public ComparisonOperatorType ComparisonOperatorType { get; set; }
}
