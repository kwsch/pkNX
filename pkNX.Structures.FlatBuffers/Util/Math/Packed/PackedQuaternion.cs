using System;
using System.ComponentModel;
using FlatSharp.Attributes;

namespace pkNX.Structures.FlatBuffers;

[FlatBufferStruct, TypeConverter(typeof(ExpandableObjectConverter))]
public class PackedQuaternion : IEquatable<PackedQuaternion>
{
    [FlatBufferItem(0)] public float W { get; set; } = 1.0f;
    [FlatBufferItem(1)] public float X { get; set; } = 0.0f;
    [FlatBufferItem(2)] public float Y { get; set; } = 0.0f;
    [FlatBufferItem(3)] public float Z { get; set; } = 0.0f;

    public bool Equals(PackedQuaternion? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return W.Equals(other.W) && X.Equals(other.X) && Y.Equals(other.Y) && Z.Equals(other.Z);
    }

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((PackedQuaternion)obj);
    }

    public override int GetHashCode() => HashCode.Combine(W, X, Y, Z);

    public override string ToString() => $"{{ W: {W}, X: {X}, Y: {Y}, Z: {Z} }}";
}
