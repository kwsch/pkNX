using System.ComponentModel;

// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global

namespace pkNX.Structures.FlatBuffers.Arceus;

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class PlacementSpawnerF20 : IHasCondition
{
    public override string ToString() => $"Spawn_20(/* EncounterTableId = */ {EncounterTable.GetTableName(EncounterTableID)}, /* ConditionTypeID = */ {this.GetConditionTypeSummary()}, /* Condition = */ {this.GetConditionSummary()}, {BonusLevelMin}, {BonusLevelMax})";
}
