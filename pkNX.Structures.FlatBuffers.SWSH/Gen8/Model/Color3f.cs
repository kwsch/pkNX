using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace pkNX.Structures.FlatBuffers.SWSH;

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial struct Color3f
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Color3f(float r = 1.0f, float g = 1.0f, float b = 1.0f)
    {
        R = r;
        G = g;
        B = b;
    }

    public override string ToString() => $"{{ R: {R}, G: {G}, B: {B} }}";
}
