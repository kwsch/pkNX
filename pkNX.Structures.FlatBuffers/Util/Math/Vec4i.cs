using System.ComponentModel;
using FlatSharp.Attributes;

namespace pkNX.Structures.FlatBuffers;

[FlatBufferStruct, TypeConverter(typeof(ExpandableObjectConverter))]
public class Vec4i
{
    [FlatBufferItem(0)] public int X { get; set; }
    [FlatBufferItem(1)] public int Y { get; set; }
    [FlatBufferItem(2)] public int Z { get; set; }
    [FlatBufferItem(3)] public int W { get; set; }

    public static readonly Vec4i Zero = new();
    public static readonly Vec4i One = new(1, 1, 1, 1);

    public Vec4i() { }

    public Vec4i(int x = 0, int y = 0, int z = 0, int w = 0)
    {
        X = x;
        Y = y;
        Z = z;
        W = w;
    }

    public override string ToString() => $"{{ X: {X}, Y: {Y}, Z: {Z}, W: {W} }}";
}
