using System.ComponentModel;
using FlatSharp.Attributes;

namespace pkNX.Structures.FlatBuffers;

[FlatBufferStruct, TypeConverter(typeof(ExpandableObjectConverter))]
public class PackedAABB
{
    [FlatBufferItem(0)] public PackedVec3f Min { get; set; } = new();
    [FlatBufferItem(1)] public PackedVec3f Max { get; set; } = new();

    public override string ToString() => $"{{ Min: {Min}, Max: {Max} }}";
}
