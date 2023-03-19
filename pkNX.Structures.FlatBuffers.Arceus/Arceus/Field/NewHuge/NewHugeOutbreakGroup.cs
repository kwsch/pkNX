using System.ComponentModel;

// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global

namespace pkNX.Structures.FlatBuffers.Arceus;

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class NewHugeOutbreakGroupArchive { }

[TypeConverter(typeof(ExpandableObjectConverter))]
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

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class NewHugeOutbreakDetail : IHasCondition
{
    public override string ToString() => $"PlacementParameters(/* ConditionTypeID = */ {this.GetConditionTypeSummary()}, /* Condition = */ {this.GetConditionSummary()}, {EncounterTableID:X16}, {Rate})";
}
