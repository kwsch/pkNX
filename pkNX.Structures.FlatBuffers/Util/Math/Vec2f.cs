using System.ComponentModel;
using FlatSharp.Attributes;

namespace pkNX.Structures.FlatBuffers.Util.Math;

[FlatBufferStruct, TypeConverter(typeof(ExpandableObjectConverter))]
public class Vec2f
{
    [FlatBufferItem(0)] public float X { get; set; }
    [FlatBufferItem(1)] public float Y { get; set; }
}
