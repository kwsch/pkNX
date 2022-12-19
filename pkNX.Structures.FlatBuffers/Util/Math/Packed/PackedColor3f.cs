using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using FlatSharp.Attributes;

namespace pkNX.Structures.FlatBuffers;

[FlatBufferStruct, TypeConverter(typeof(ExpandableObjectConverter))]
public class PackedColor3f : IEquatable<PackedColor3f>
{
    [FlatBufferItem(00)] public float R { get; set; } = 0.0f;
    [FlatBufferItem(01)] public float G { get; set; } = 0.0f;
    [FlatBufferItem(02)] public float B { get; set; } = 0.0f;

    public PackedColor3f() { }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public PackedColor3f(float r = 1.0f, float g = 1.0f, float b = 1.0f)
    {
        R = r;
        G = g;
        B = b;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator PackedVec3f(PackedColor3f c) => new(c.R, c.G, c.B);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Vec3f(PackedColor3f c) => new(c.R, c.G, c.B);

    public bool Equals(PackedColor3f? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return R.Equals(other.R) && G.Equals(other.G) && B.Equals(other.B);
    }

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((PackedColor3f)obj);
    }

    public override int GetHashCode() => HashCode.Combine(R, G, B);

    public override string ToString() => $"{{ R: {R}, G: {G}, B: {B} }}";
}
