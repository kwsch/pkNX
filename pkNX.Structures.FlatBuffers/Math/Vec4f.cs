using System;
using System.ComponentModel;

namespace pkNX.Structures.FlatBuffers;

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class Vec4f : IEquatable<Vec4f>
{
    public static readonly Vec4f Zero = new();
    public static readonly Vec4f One = new(1, 1, 1, 1);

    public Vec4f(float x = 0, float y = 0, float z = 0, float w = 0)
    {
        X = x;
        Y = y;
        Z = z;
        W = w;
    }

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
