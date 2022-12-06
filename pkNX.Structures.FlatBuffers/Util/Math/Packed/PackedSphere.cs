using System.ComponentModel;
using FlatSharp.Attributes;

namespace pkNX.Structures.FlatBuffers;

[FlatBufferStruct, TypeConverter(typeof(ExpandableObjectConverter))]
public class PackedSphere
{
    [FlatBufferItem(00)] public PackedVec3f Center { get; set; } = new();
    [FlatBufferItem(01)] public float Radius { get; set; }

    public override string ToString() => $"{{ Radius: {Radius}, Center: {Center} }}";
}
