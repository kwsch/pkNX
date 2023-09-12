using System;
using System.ComponentModel;

namespace pkNX.Structures.FlatBuffers;

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class Vec3f : IEquatable<Vec3f>
{
    public static readonly Vec3f Zero = new();
    public static readonly Vec3f One = new() { X = 1, Y = 1, Z = 1 };

    public static implicit operator Vec3f(PackedVec3f v) => new() { X = v.X, Y = v.Y, Z = v.Z };

    public Vec3f(float x = 0, float y = 0, float z = 0)
    {
        X = x;
        Y = y;
        Z = z;
    }

    public bool IsOne => X is 1 && Y is 1 && Z is 1;
    public bool IsZero => X is 0 && Y is 0 && Z is 0;

    public float Magnitude => MathF.Sqrt(MagnitudeSqr);
    public float MagnitudeSqr => Dot(this);
    public Vec3f Normalized() => this * (1 / Magnitude);

    public float Dot(Vec3f other) => X * other.X + Y * other.Y + Z * other.Z;
    public Vec3f Cross(Vec3f other) => new(Y * other.Z - Z * other.Y, Z * other.X - X * other.Z, X * other.Y - Y * other.X);
    public float DistanceTo(Vec3f other) => (this - other).Magnitude;
    public float DistanceToSqr(Vec3f other) => (this - other).MagnitudeSqr;
    public Vec3f Lerp(Vec3f other, float t) => this + (other - this) * t;

    public Vec3f Clone() => new()
    {
        X = X,
        Y = Y,
        Z = Z,
    };

    public override string ToString() => $"V3f({X}, {Y}, {Z})";
    public string ToTriple() => $"({X}, {Y}, {Z})";

    public bool Equals(Vec3f? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return X.Equals(other.X) && Y.Equals(other.Y) && Z.Equals(other.Z);
    }

    public override bool Equals(object? obj)
    {
        return obj is Vec3f other && Equals(other);
    }

    public override int GetHashCode() => HashCode.Combine(X, Y, Z);

    public static Vec3f operator -(Vec3f v) => new(-v.X, -v.Y, -v.Z);

    public static Vec3f operator +(Vec3f l, Vec3f r) => new(l.X + r.X, l.Y + r.Y, l.Z + r.Z);
    public static Vec3f operator -(Vec3f l, Vec3f r) => new(l.X - r.X, l.Y - r.Y, l.Z - r.Z);
    public static Vec3f operator *(Vec3f l, Vec3f r) => new(l.X * r.X, l.Y * r.Y, l.Z * r.Z);
    public static Vec3f operator /(Vec3f l, Vec3f r) => new(l.X / r.X, l.Y / r.Y, l.Z / r.Z);

    public static Vec3f operator +(Vec3f l, float r) => new(l.X + r, l.Y + r, l.Z + r);
    public static Vec3f operator -(Vec3f l, float r) => new(l.X - r, l.Y - r, l.Z - r);
    public static Vec3f operator *(Vec3f l, float r) => new(l.X * r, l.Y * r, l.Z * r);
    public static Vec3f operator /(Vec3f l, float r) => new(l.X / r, l.Y / r, l.Z / r);

    public static bool operator ==(Vec3f? left, Vec3f? right) => Equals(left, right);
    public static bool operator !=(Vec3f? left, Vec3f? right) => !Equals(left, right);
}
