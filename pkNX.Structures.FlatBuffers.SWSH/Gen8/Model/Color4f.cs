using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace pkNX.Structures.FlatBuffers.SWSH;

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial struct Color4f
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Color4f(float r = 1.0f, float g = 1.0f, float b = 1.0f, float a = 1.0f)
    {
        R = r;
        G = g;
        B = b;
        A = a;
    }

    public override string ToString() => $"{{ R: {R}, G: {G}, B: {B}, A: {A} }}";
}
