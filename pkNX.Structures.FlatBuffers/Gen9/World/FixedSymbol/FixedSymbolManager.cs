using System.ComponentModel;
using FlatSharp.Attributes;
// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedType.Global
#nullable disable

namespace pkNX.Structures.FlatBuffers;

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class FixedSymbolManager
{
    [FlatBufferItem(0)] public float PointFetchRange { get; set; }
    [FlatBufferItem(1)] public float PointReleaseRange { get; set; }
    [FlatBufferItem(2)] public float PointFetchRangeGem { get; set; }
    [FlatBufferItem(3)] public float PointReleaseRangeGem { get; set; }
}
