using System.Runtime.CompilerServices;

namespace pkNX.Structures.FlatBuffers;

[method: MethodImpl(MethodImplOptions.AggressiveInlining)]
public partial struct Color4f(float r = 1.0f, float g = 1.0f, float b = 1.0f, float a = 1.0f)
{
    /// <summary>
    /// Multiply a [0.0f,1.0f] float color by FloatToRGB to get the [0,255] byte color
    /// </summary>
    private const float FloatToRGB = 255.0f;

    /// <summary>
    /// Multiply a [0,255] byte color by RGBToFloat to get the [0.0f,1.0f] float color
    /// </summary>
    private const float RGBToFloat = 0.0039215686274509803921568627451f;

    public Color4f(Vec4f v) : this(v.X, v.Y, v.Z, v.W)
    {
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Color4f FromByteColor(Vec4i c) => new(c.X * RGBToFloat, c.Y * RGBToFloat, c.Z * RGBToFloat, c.W * RGBToFloat);

    public readonly override string ToString() => $"{{ R: {R}, G: {G}, B: {B}, A: {A} }}";
}
