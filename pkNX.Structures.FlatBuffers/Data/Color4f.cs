using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace pkNX.Structures.FlatBuffers;

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial struct Color4f
{
    /// <summary>
    /// Multiply a [0.0f..1.0f] float color by FloatToRGB to get the [0..255] byte color
    /// </summary>
    private static readonly float FloatToRGB = 255.0f;

    /// <summary>
    /// Multiply a [0..255] byte color by RGBToFloat to get the [0.0f..1.0f] float color
    /// </summary>
    private static readonly float RGBToFloat = 0.0039215686274509803921568627451f;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Color4f(float r = 1.0f, float g = 1.0f, float b = 1.0f, float a = 1.0f)
    {
        R = r;
        G = g;
        B = b;
        A = a;
    }

    public Color4f(Vec4f v)
    {
        R = v.X;
        G = v.Y;
        B = v.Z;
        A = v.W;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Color4f FromByteColor(Vec4i c) => new(c.X * RGBToFloat, c.Y * RGBToFloat, c.Z * RGBToFloat, c.W * RGBToFloat);


    public override string ToString() => $"{{ R: {R}, G: {G}, B: {B}, A: {A} }}";
}
