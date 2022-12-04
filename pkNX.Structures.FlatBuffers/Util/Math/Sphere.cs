using System.ComponentModel;
using FlatSharp.Attributes;

namespace pkNX.Structures.FlatBuffers;

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class Sphere
{
    [FlatBufferItem(0)] public PackedVec3f Center { get; set; } = new();
    [FlatBufferItem(1)] public float Radius { get; set; }
}
