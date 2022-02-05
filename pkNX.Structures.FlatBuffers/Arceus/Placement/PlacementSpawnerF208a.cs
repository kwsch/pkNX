using System.ComponentModel;
using FlatSharp.Attributes;

// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global

namespace pkNX.Structures.FlatBuffers;

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class PlacementSpawnerF208a : IHasCondition8a
{
    [FlatBufferItem(0)] public ulong EncounterTableID { get; set; } // TableID in EncounterTable8a
    [FlatBufferItem(1)] public ConditionType8a ConditionTypeID { get; set; }
    [FlatBufferItem(2)] public Condition8a ConditionID { get; set; }
    [FlatBufferItem(3)] public string ConditionArg1 { get; set; } = string.Empty;
    [FlatBufferItem(4)] public string ConditionArg2 { get; set; } = string.Empty;
    [FlatBufferItem(5)] public string ConditionArg3 { get; set; } = string.Empty;
    [FlatBufferItem(6)] public string ConditionArg4 { get; set; } = string.Empty;
    [FlatBufferItem(7)] public string ConditionArg5 { get; set; } = string.Empty;
    [FlatBufferItem(8)] public int BonusLevelMin { get; set; }
    [FlatBufferItem(9)] public int BonusLevelMax { get; set; }

    public override string ToString() => $"Spawn_20(/* EncounterTableId = */ {EncounterTable8a.GetTableName(EncounterTableID)}, /* ConditionTypeID = */ {this.GetConditionTypeSummary()}, /* Condition = */ {this.GetConditionSummary()}, {BonusLevelMin}, {BonusLevelMax})";
}
