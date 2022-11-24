using System.ComponentModel;
using FlatSharp.Attributes;
// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedType.Global
#nullable disable

namespace pkNX.Structures.FlatBuffers;

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class MapChangeParametersArray : IFlatBufferArchive<MapChangeParameters>
{
    [FlatBufferItem(0)] public MapChangeParameters[] Table { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class MapChangeParameters
{
    [FlatBufferItem(0)] public float LhLadderWait { get; set; }
    [FlatBufferItem(1)] public float LhLadderFadeDuration { get; set; }
    [FlatBufferItem(2)] public float MapChangeSingleDoorAngle { get; set; }
    [FlatBufferItem(3)] public float MapChangeDoubleDoorAngle { get; set; }
    [FlatBufferItem(4)] public float MapChangeAutoDoorAngle { get; set; }
    [FlatBufferItem(5)] public float MapChangePosAngle { get; set; }
    [FlatBufferItem(6)] public float MapChangeCampusDoorAngle { get; set; }
    [FlatBufferItem(7)] public float MapChangeAtlantisDoorAngle { get; set; }
    [FlatBufferItem(8)] public float MapChangeStickAngle { get; set; }
}
