namespace pkNX.WinForms;

public sealed record MapTransform(TransformScale Texture, TransformScale Range, TransformScale Scale, TransformScale Dir, TransformScale Offset)
{
    public double ConvertWidth(double s) => (Range.X / Scale.X) * s;
    public double ConvertHeight(double s) => (Range.Z / Scale.Z) * s;
    public double ConvertX(double x) => (Texture.X / 2) + (Dir.X * ((Range.X / Scale.X) * (x + Offset.X)));
    public double ConvertZ(double z) => (Texture.Z / 2) + (Dir.Z * ((Range.Z / Scale.Z) * (z + Offset.Z)));

    public (float X, float Z) ScreenToWorld(double px, double pz, double controlWidth, double controlHeight)
    {
        var xTex = (px / controlWidth) * Texture.X;
        var zTex = (pz / controlHeight) * Texture.Z;
        var x = (((Texture.X / 2.0f) - xTex) * -Dir.X * (Scale.X / Range.X)) - Offset.X;
        var z = (((Texture.Z / 2.0f) - zTex) * -Dir.Z * (Scale.Z / Range.Z)) - Offset.Z;
        return ((float)x, (float)z);
    }
}

public readonly record struct TransformScale(double X, double Z)
{
    public static implicit operator TransformScale((double X, double Z) value) => new(value.X, value.Z);
}
