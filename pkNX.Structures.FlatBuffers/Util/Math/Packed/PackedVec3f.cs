using System;
using System.ComponentModel;
using FlatSharp.Attributes;

namespace pkNX.Structures.FlatBuffers;

[FlatBufferStruct, TypeConverter(typeof(ExpandableObjectConverter))]
public class PackedVec3f : IEquatable<PackedVec3f>
{
    [FlatBufferItem(00)] public float X { get; set; }
    [FlatBufferItem(01)] public float Y { get; set; }
    [FlatBufferItem(02)] public float Z { get; set; }

    public override string ToString() => $"{{ X: {X}, Y: {Y}, Z: {Z} }}";

    public static readonly PackedVec3f Zero = new();
    public static readonly PackedVec3f One = new(1, 1, 1);

    public PackedVec3f() { }

    public PackedVec3f(float x = 0, float y = 0, float z = 0)
    {
        X = x;
        Y = y;
        Z = z;
    }
    public static implicit operator Vec3f(PackedVec3f v) => new(v.X, v.Y, v.Z);

    public bool IsOne => X is 1 && Y is 1 && Z is 1;
    public bool IsZero => X is 0 && Y is 0 && Z is 0;

    public float Magnitude => MathF.Sqrt(MagnitudeSqr);
    public float MagnitudeSqr => Dot(this);
    public PackedVec3f Normalized() => this * (1 / Magnitude);

    public float Dot(PackedVec3f other) => X * other.X + Y * other.Y + Z * other.Z;
    public PackedVec3f Cross(PackedVec3f other) => new(Y * other.Z - Z * other.Y, Z * other.X - X * other.Z, X * other.Y - Y * other.X);
    public float DistanceTo(PackedVec3f other) => (this - other).Magnitude;
    public float DistanceToSqr(PackedVec3f other) => (this - other).MagnitudeSqr;
    public PackedVec3f Lerp(PackedVec3f other, float t) => this + (other - this) * t;

    public PackedVec3f Clone() => new()
    {
        X = X,
        Y = Y,
        Z = Z,
    };

    public bool Equals(PackedVec3f? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return X.Equals(other.X) && Y.Equals(other.Y) && Z.Equals(other.Z);
    }

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((PackedVec3f)obj);
    }

    public override int GetHashCode() => HashCode.Combine(X, Y, Z);

    public static PackedVec3f operator -(PackedVec3f v) => new(-v.X, -v.Y, -v.Z);

    public static PackedVec3f operator +(PackedVec3f l, PackedVec3f r) => new(l.X + r.X, l.Y + r.Y, l.Z + r.Z);
    public static PackedVec3f operator -(PackedVec3f l, PackedVec3f r) => new(l.X - r.X, l.Y - r.Y, l.Z - r.Z);
    public static PackedVec3f operator *(PackedVec3f l, PackedVec3f r) => new(l.X * r.X, l.Y * r.Y, l.Z * r.Z);
    public static PackedVec3f operator /(PackedVec3f l, PackedVec3f r) => new(l.X / r.X, l.Y / r.Y, l.Z / r.Z);

    public static PackedVec3f operator +(PackedVec3f l, float r) => new(l.X + r, l.Y + r, l.Z + r);
    public static PackedVec3f operator -(PackedVec3f l, float r) => new(l.X - r, l.Y - r, l.Z - r);
    public static PackedVec3f operator *(PackedVec3f l, float r) => new(l.X * r, l.Y * r, l.Z * r);
    public static PackedVec3f operator /(PackedVec3f l, float r) => new(l.X / r, l.Y / r, l.Z / r);

    public static bool operator ==(PackedVec3f? left, PackedVec3f? right) => Equals(left, right);
    public static bool operator !=(PackedVec3f? left, PackedVec3f? right) => !Equals(left, right);
}
