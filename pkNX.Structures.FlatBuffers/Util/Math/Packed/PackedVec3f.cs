using System.ComponentModel;
using FlatSharp.Attributes;

namespace pkNX.Structures.FlatBuffers;

[FlatBufferStruct, TypeConverter(typeof(ExpandableObjectConverter))]
public class PackedVec3f
{
    [FlatBufferItem(00)] public float X { get; set; } = 0;
    [FlatBufferItem(01)] public float Y { get; set; } = 0;
    [FlatBufferItem(02)] public float Z { get; set; } = 0;
}
