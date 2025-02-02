namespace pkNX.Structures.FlatBuffers.Arceus;

public partial class PlacementParameters : IHasCondition
{
    public override string ToString() => $"PlacementParameters(/* ConditionTypeID = */ {this.GetConditionTypeSummary()}, /* Condition = */ {this.GetConditionSummary()}, {Coordinates}, {Rotation}, {Scale})";
}
