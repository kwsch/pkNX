using System.ComponentModel;
using FlatSharp.Attributes;
// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedType.Global
#nullable disable

namespace pkNX.Structures.FlatBuffers;

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class FieldInteriorCamera
{
    [FlatBufferItem(0)] public Range Yaw { get; set; }
    [FlatBufferItem(1)] public Range Pitch { get; set; }
    [FlatBufferItem(2)] public float Distance { get; set; }
    [FlatBufferItem(3)] public float DelayFactor { get; set; }
    [FlatBufferItem(4)] public Vec3f Offset { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class Range
{
    [FlatBufferItem(0)] public float Origin { get; set; }
    [FlatBufferItem(1)] public float Max { get; set; }
    [FlatBufferItem(2)] public float Min { get; set; }
}

