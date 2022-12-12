using System.ComponentModel;
using FlatSharp.Attributes;

namespace pkNX.Structures.FlatBuffers;

[FlatBufferStruct, TypeConverter(typeof(ExpandableObjectConverter))]
public class Vec2f
{
    [FlatBufferItem(0)] public float X { get; set; }
    [FlatBufferItem(1)] public float Y { get; set; }

    public static readonly Vec2f Zero = new();
    public static readonly Vec2f One = new(1, 1);

    public Vec2f() { }

    public Vec2f(float x = 0, float y = 0)
    {
        X = x;
        Y = y;
    }
    public static implicit operator Vec2f(PackedVec2f v) => new(v.X, v.Y);
    public override string ToString() => $"{{ X: {X:0.0######}, Y: {Y:0.0######} }}";
}
