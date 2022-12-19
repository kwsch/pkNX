using System;
using System.ComponentModel;
using FlatSharp.Attributes;

namespace pkNX.Structures.FlatBuffers;

[FlatBufferStruct, TypeConverter(typeof(ExpandableObjectConverter))]
public class PackedSphere
{
    public PackedSphere() { }

    public PackedSphere(PackedAABB bounds)
    {
        Center = bounds.Center;
        Radius = bounds.Extents.Magnitude;
    }

    [FlatBufferItem(00)] public PackedVec3f Center { get; set; } = new();
    [FlatBufferItem(01)] public float Radius { get; set; }

    public bool Equals(PackedSphere? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return Center.Equals(other.Center) && Radius.Equals(other.Radius);
    }

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((PackedSphere)obj);
    }

    public override int GetHashCode() => HashCode.Combine(Center, Radius);

    public override string ToString() => $"{{ Radius: {Radius}, Center: {Center} }}";
}
