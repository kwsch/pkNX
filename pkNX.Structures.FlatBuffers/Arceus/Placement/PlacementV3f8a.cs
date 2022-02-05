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
    [FlatBufferItem(0)] public float X { get; set; }
    [FlatBufferItem(1)] public float Y { get; set; }
    [FlatBufferItem(2)] public float Z { get; set; }
    public bool IsDefault => X is 1.0f && Y is 1.0f && Z is 1.0f;

    public double DistanceTo(PlacementV3f8a other) => DistanceTo(other.X, other.Y, other.Z);
    public double DistanceTo(float x, float y, float z) => Math.Sqrt(Math.Pow(X - x, 2) + Math.Pow(Y - y, 2) + Math.Pow(Z - z, 2));

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

    public static bool operator ==(PlacementV3f8a? left, PlacementV3f8a? right) => Equals(left, right);
    public static bool operator !=(PlacementV3f8a? left, PlacementV3f8a? right) => !Equals(left, right);
}