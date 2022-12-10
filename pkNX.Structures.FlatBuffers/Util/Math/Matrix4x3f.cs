using System.ComponentModel;
using FlatSharp.Attributes;

namespace pkNX.Structures.FlatBuffers;

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class Matrix4x3f
{
    [FlatBufferItem(0)] public PackedVec3f AxisX { get; set; } = new();
    [FlatBufferItem(1)] public PackedVec3f AxisY { get; set; } = new();
    [FlatBufferItem(2)] public PackedVec3f AxisZ { get; set; } = new();
    [FlatBufferItem(3)] public PackedVec3f AxisW { get; set; } = new();

    public override string ToString() => $"{{ 0: {AxisX}, 1: {AxisY}, 2: {AxisZ}, 3: {AxisW} }}";
}
