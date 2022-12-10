using System.ComponentModel;
using FlatSharp.Attributes;

namespace pkNX.Structures.FlatBuffers;

[FlatBufferStruct, TypeConverter(typeof(ExpandableObjectConverter))]
public class PackedQuaternion
{
    [FlatBufferItem(0)] public float W { get; set; } = 1.0f;
    [FlatBufferItem(1)] public float X { get; set; } = 0.0f;
    [FlatBufferItem(2)] public float Y { get; set; } = 0.0f;
    [FlatBufferItem(3)] public float Z { get; set; } = 0.0f;

    public override string ToString() => $"{{ W: {W}, X: {X}, Y: {Y}, Z: {Z} }}";
}
