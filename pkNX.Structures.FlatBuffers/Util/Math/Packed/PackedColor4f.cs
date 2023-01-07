using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using FlatSharp.Attributes;

namespace pkNX.Structures.FlatBuffers;

[FlatBufferStruct, TypeConverter(typeof(ExpandableObjectConverter))]
public class PackedColor4f : IEquatable<PackedColor4f>
{
    [FlatBufferItem(00)] public float R { get; set; } = 1.0f;
    [FlatBufferItem(01)] public float G { get; set; } = 1.0f;
    [FlatBufferItem(02)] public float B { get; set; } = 1.0f;
    [FlatBufferItem(03)] public float A { get; set; } = 1.0f;

    /// <summary>
    /// Multiply a [0.0f..1.0f] float color by FloatToRGB to get the [0..255] byte color
    /// </summary>
    private static float FloatToRGB = 255.0f;

    /// <summary>
    /// Multiply a [0..255] byte color by RGBToFloat to get the [0.0f..1.0f] float color
    /// </summary>
    private static float RGBToFloat = 0.0039215686274509803921568627451f;

    public PackedColor4f() { }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public PackedColor4f(float r = 1.0f, float g = 1.0f, float b = 1.0f, float a = 1.0f)
    {
        R = r;
        G = g;
        B = b;
        A = a;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator PackedVec4f(PackedColor4f c) => new(c.R, c.G, c.B, c.A);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Vec4f(PackedColor4f c) => new(c.R, c.G, c.B, c.A);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator PackedColor4f(PackedVec4f v) => new(v.X, v.Y, v.Z, v.W);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator PackedColor4f(Vec4f v) => new(v.X, v.Y, v.Z, v.W);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static PackedColor4f FromByteColor(Vec4i c) => new(c.X * RGBToFloat, c.Y * RGBToFloat, c.Z * RGBToFloat, c.W * RGBToFloat);

    public bool Equals(PackedColor4f? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return R.Equals(other.R) && G.Equals(other.G) && B.Equals(other.B) && A.Equals(other.A);
    }

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((PackedColor4f)obj);
    }

    public override int GetHashCode() => HashCode.Combine(R, G, B, A);

    public override string ToString() => $"{{ R: {R}, G: {G}, B: {B}, A: {A} }}";
}
