using System.ComponentModel;
using FlatSharp.Attributes;

namespace pkNX.Structures.FlatBuffers;

[FlatBufferStruct, TypeConverter(typeof(ExpandableObjectConverter))]
public class PackedColor4f
{
    [FlatBufferItem(00)] public float R { get; set; } = 0.0f;
    [FlatBufferItem(01)] public float G { get; set; } = 0.0f;
    [FlatBufferItem(02)] public float B { get; set; } = 0.0f;
    [FlatBufferItem(03)] public float A { get; set; } = 1.0f;
}
