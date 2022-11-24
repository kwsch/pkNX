using System.ComponentModel;
using FlatSharp.Attributes;
// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedType.Global
#nullable disable

namespace pkNX.Structures.FlatBuffers;

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class FieldAutomaticReturnTableArray : IFlatBufferArchive<FieldAutomaticReturnTable>
{
    [FlatBufferItem(0)] public FieldAutomaticReturnTable[] Table { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class FieldAutomaticReturnTable
{
    [FlatBufferItem(0)] public float AutoReturnFallTime { get; set; }
    [FlatBufferItem(1)] public float AutoReturnSlideTime { get; set; }
    [FlatBufferItem(2)] public float AutoReturnMoveAmount { get; set; }
    [FlatBufferItem(3)] public float AutoReturnTiltAngle { get; set; }
    [FlatBufferItem(4)] public float CliffReturnHeavyLandingHeight { get; set; }
    [FlatBufferItem(5)] public float CliffReturnInvalidDistance { get; set; }
}
