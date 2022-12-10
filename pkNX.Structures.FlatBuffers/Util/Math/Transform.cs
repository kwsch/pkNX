using System.ComponentModel;
using FlatSharp.Attributes;

namespace pkNX.Structures.FlatBuffers;

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class Transform
{
    [FlatBufferItem(0)] public PackedVec3f Scale { get; set; } = new();
    [FlatBufferItem(1)] public PackedVec3f Rotate { get; set; } = new();
    [FlatBufferItem(2)] public PackedVec3f Translate { get; set; } = new();

    public override string ToString() => $"{{ S: {Scale}, R: {Rotate}, T: {Translate} }}";
}
