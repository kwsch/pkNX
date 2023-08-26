using System;
using System.ComponentModel;

namespace pkNX.Structures.FlatBuffers;

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class Vec2f : IEquatable<Vec2f>
{
    public static readonly Vec2f Zero = new();
    public static readonly Vec2f One = new() { X = 1, Y = 1 };

    public Vec2f(float x = 0, float y = 0)
    {
        X = x;
        Y = y;
    }

    public bool Equals(Vec2f? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return X.Equals(other.X) && Y.Equals(other.Y);
    }

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((Vec2f)obj);
    }

    public override int GetHashCode() => HashCode.Combine(X, Y);

    public override string ToString() => $"{{ X: {X:0.0######}, Y: {Y:0.0######} }}";
}
