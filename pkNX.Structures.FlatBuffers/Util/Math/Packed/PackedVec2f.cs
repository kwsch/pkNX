using System.ComponentModel;
using FlatSharp.Attributes;

namespace pkNX.Structures.FlatBuffers;

[FlatBufferStruct, TypeConverter(typeof(ExpandableObjectConverter))]
public class PackedVec2f
{
    [FlatBufferItem(0)] public float X { get; set; }
    [FlatBufferItem(1)] public float Y { get; set; }

    public override string ToString() => $"{{ X: {X}, Y: {Y} }}";
}
