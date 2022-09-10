using System;
using System.ComponentModel;
using FlatSharp.Attributes;

// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global

namespace pkNX.Structures.FlatBuffers;

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class PlacementV3f8a : IEquatable<PlacementV3f8a>
{
    [FlatBufferItem(0)] public float X { get; set; } = 0;
    [FlatBufferItem(1)] public float Y { get; set; } = 0;
    [FlatBufferItem(2)] public float Z { get; set; } = 0;

    public PlacementV3f8a()
    {
        X = 0;
        Y = 0;
        Z = 0;
    }

    public PlacementV3f8a(float x = 0, float y = 0, float z = 0)
    {
        X = x;
        Y = y;
        Z = z;
    }

    public bool IsOne => X is 1 && Y is 1 && Z is 1;
    public bool IsZero => X is 0 && Y is 0 && Z is 0;

    public float Magnitude => (float)Math.Sqrt(MagnitudeSqr);
    public float MagnitudeSqr => Dot(this);
    public PlacementV3f8a Normalized => this * (1 / Magnitude);

    public float Dot(PlacementV3f8a other) => X * other.X + Y * other.Y + Z * other.Z;
    public PlacementV3f8a Cross(PlacementV3f8a other) => new(Y * other.Z - Z * other.Y, Z * other.X - X * other.Z, X * other.Y - Y * other.X);
    public float DistanceTo(PlacementV3f8a other) => (this - other).Magnitude;
    public float DistanceToSqr(PlacementV3f8a other) => (this - other).MagnitudeSqr;
    public PlacementV3f8a Lerp(PlacementV3f8a other, float t) => this + (other - this) * t;

    public PlacementV3f8a Clone() => new()
    {
        X = X,
        Y = Y,
        Z = Z,
    };

    public override string ToString() => $"V3f({X}, {Y}, {Z})";
    public string ToTriple() => $"({X}, {Y}, {Z})";

    public bool Equals(PlacementV3f8a? other)
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
        return Equals((PlacementV3f8a)obj);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = X.GetHashCode();
            hashCode = (hashCode * 397) ^ Y.GetHashCode();
            hashCode = (hashCode * 397) ^ Z.GetHashCode();
            return hashCode;
        }
    }

    public static PlacementV3f8a operator -(PlacementV3f8a v) => new(-v.X, -v.Y, -v.Z);

    public static PlacementV3f8a operator +(PlacementV3f8a l, PlacementV3f8a r) => new(l.X + r.X, l.Y + r.Y, l.Z + r.Z);
    public static PlacementV3f8a operator -(PlacementV3f8a l, PlacementV3f8a r) => new(l.X - r.X, l.Y - r.Y, l.Z - r.Z);
    public static PlacementV3f8a operator *(PlacementV3f8a l, PlacementV3f8a r) => new(l.X * r.X, l.Y * r.Y, l.Z * r.Z);
    public static PlacementV3f8a operator /(PlacementV3f8a l, PlacementV3f8a r) => new(l.X / r.X, l.Y / r.Y, l.Z / r.Z);

    public static PlacementV3f8a operator +(PlacementV3f8a l, float r) => new(l.X + r, l.Y + r, l.Z + r);
    public static PlacementV3f8a operator -(PlacementV3f8a l, float r) => new(l.X - r, l.Y - r, l.Z - r);
    public static PlacementV3f8a operator *(PlacementV3f8a l, float r) => new(l.X * r, l.Y * r, l.Z * r);
    public static PlacementV3f8a operator /(PlacementV3f8a l, float r) => new(l.X / r, l.Y / r, l.Z / r);

    public static bool operator ==(PlacementV3f8a? left, PlacementV3f8a? right) => Equals(left, right);
    public static bool operator !=(PlacementV3f8a? left, PlacementV3f8a? right) => !Equals(left, right);
}