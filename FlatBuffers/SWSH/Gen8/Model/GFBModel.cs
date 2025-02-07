namespace pkNX.Structures.FlatBuffers.SWSH;

// *.gfbmdl

public partial class Texture
{
    public override string ToString() => $"{{ texture[{TextureIndex}], Bound to: {SamplerName}, {Settings} }}";
}

public partial class Flag
{
    public override string ToString() => $"{{ {FlagName}: {FlagEnable} }}";
}

public partial class FloatParam
{
    public override string ToString() => $"{{ {ValueName}: {Value} }}";
}

public partial class Color3Param
{
    public override string ToString() => $"{{ {ColorName}: {Color} }}";
}
