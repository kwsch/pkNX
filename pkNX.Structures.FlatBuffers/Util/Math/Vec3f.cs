using System;
using System.ComponentModel;
using FlatSharp.Attributes;

namespace pkNX.Structures.FlatBuffers;

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class Vec3f : IEquatable<Vec3f>
{
    [FlatBufferItem(0)] public float X { get; set; }
    [FlatBufferItem(1)] public float Y { get; set; }
    [FlatBufferItem(2)] public float Z { get; set; }

    public static readonly Vec3f Zero = new();
    public static readonly Vec3f One = new(1, 1, 1);

    public Vec3f() { }

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
    public Vec3f Normalized => this * (1 / Magnitude);

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
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((Vec3f)obj);
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
