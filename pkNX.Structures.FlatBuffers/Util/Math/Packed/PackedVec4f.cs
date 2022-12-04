using System.ComponentModel;
using FlatSharp.Attributes;

namespace pkNX.Structures.FlatBuffers;

[FlatBufferStruct, TypeConverter(typeof(ExpandableObjectConverter))]
public class PackedVec4f
{
    [FlatBufferItem(0)] public float X { get; set; }
    [FlatBufferItem(1)] public float Y { get; set; }
    [FlatBufferItem(2)] public float Z { get; set; }
    [FlatBufferItem(3)] public float W { get; set; }

    public PackedVec4f(float x = 0, float y = 0, float z = 0, float w = 0)
    {
        X = x;
        Y = y;
        Z = z;
        W = w;
    }
}
