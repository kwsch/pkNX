namespace pkNX.Structures.FlatBuffers.Arceus;

public partial class NewHugeOutbreakGroup
{
    public int SumTable => Table.Sum(z => z.Rate);

    public bool UsesTable(ulong tableID)
    {
        if (EncounterTableID == tableID)
            return true;
        var matches = Table.FirstOrDefault(z => z.EncounterTableID == tableID);
        return matches is not null;
    }
}

public partial class NewHugeOutbreakDetail : IHasCondition
{
    public override string ToString() => $"PlacementParameters(/* ConditionTypeID = */ {this.GetConditionTypeSummary()}, /* Condition = */ {this.GetConditionSummary()}, {EncounterTableID:X16}, {Rate})";
}
