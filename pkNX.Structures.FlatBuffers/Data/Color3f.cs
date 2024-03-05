using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace pkNX.Structures.FlatBuffers;

[TypeConverter(typeof(ExpandableObjectConverter))]
[method: MethodImpl(MethodImplOptions.AggressiveInlining)]
public partial struct Color3f(float r = 1.0f, float g = 1.0f, float b = 1.0f)
{
    public override string ToString() => $"{{ R: {R}, G: {G}, B: {B} }}";
}
