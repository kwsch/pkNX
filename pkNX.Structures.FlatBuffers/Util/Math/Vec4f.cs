using System;
using System.ComponentModel;
using FlatSharp.Attributes;

namespace pkNX.Structures.FlatBuffers;

[FlatBufferStruct, TypeConverter(typeof(ExpandableObjectConverter))]
public class Vec4f : IEquatable<Vec4f>
{
    [FlatBufferItem(0)] public float X { get; set; }
    [FlatBufferItem(1)] public float Y { get; set; }
    [FlatBufferItem(2)] public float Z { get; set; }
    [FlatBufferItem(3)] public float W { get; set; }

    public static readonly Vec4f Zero = new();
    public static readonly Vec4f One = new(1, 1, 1, 1);

    public Vec4f() { }

    public Vec4f(float x = 0, float y = 0, float z = 0, float w = 0)
    {
        X = x;
        Y = y;
        Z = z;
        W = w;
    }
    public static implicit operator Vec4f(PackedVec4f v) => new(v.X, v.Y, v.Z, v.W);

    public bool Equals(Vec4f? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return X.Equals(other.X) && Y.Equals(other.Y) && Z.Equals(other.Z) && W.Equals(other.W);
    }

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((Vec4f)obj);
    }

    public override int GetHashCode() => HashCode.Combine(X, Y, Z, W);

    public override string ToString() => $"{{ X: {X:0.0######}, Y: {Y:0.0######}, Z: {Z:0.0######}, W: {W:0.0######} }}";
}
