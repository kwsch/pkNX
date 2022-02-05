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
public class PlacementParameters8a : IHasCondition8a
{
    [FlatBufferItem(0)] public ConditionType8a ConditionTypeID { get; set; }
    [FlatBufferItem(1)] public Condition8a ConditionID { get; set; }
    [FlatBufferItem(2)] public string ConditionArg1 { get; set; } = string.Empty;
    [FlatBufferItem(3)] public string ConditionArg2 { get; set; } = string.Empty;
    [FlatBufferItem(4)] public string ConditionArg3 { get; set; } = string.Empty;
    [FlatBufferItem(5)] public string ConditionArg4 { get; set; } = string.Empty;
    [FlatBufferItem(6)] public string ConditionArg5 { get; set; } = string.Empty;
    [FlatBufferItem(7)] public PlacementV3f8a Coordinates { get; set; } = new();
    [FlatBufferItem(8)] public PlacementV3f8a Rotation { get; set; } = new();
    [FlatBufferItem(9)] public PlacementV3f8a Scale { get; set; } = new();

    public override string ToString() => $"PlacementParameters(/* ConditionTypeID = */ {this.GetConditionTypeSummary()}, /* Condition = */ {this.GetConditionSummary()}, {Coordinates}, {Rotation}, {Scale})";
}
