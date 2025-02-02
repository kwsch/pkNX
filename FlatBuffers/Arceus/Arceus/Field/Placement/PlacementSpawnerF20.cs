namespace pkNX.Structures.FlatBuffers.Arceus;

public partial class PlacementSpawnerF20 : IHasCondition
{
    public override string ToString() => $"Spawn_20(/* EncounterTableId = */ {EncounterTable.GetTableName(EncounterTableID)}, /* ConditionTypeID = */ {this.GetConditionTypeSummary()}, /* Condition = */ {this.GetConditionSummary()}, {BonusLevelMin}, {BonusLevelMax})";
}
