namespace pkNX.Structures.FlatBuffers.Arceus;

// *.trmtr

public partial class Float4Parameter
{
    public Float4Parameter(string shaderPropertyBinding, Color4f value)
    {
        PropertyBinding = shaderPropertyBinding;
        ColorValue = value;
    }

    public override string ToString() => $"{{ {PropertyBinding}: {ColorValue} }}";
}

public partial class FloatParameter
{
    public FloatParameter(string shaderPropertyBinding, float value)
    {
        PropertyBinding = shaderPropertyBinding;
        FloatValue = value;
    }

    public override string ToString() => $"{{ {PropertyBinding}: {FloatValue} }}";
}

public partial class IntParameter
{
    public IntParameter(string shaderPropertyBinding, int value)
    {
        PropertyBinding = shaderPropertyBinding;
        IntValue = value;
    }

    public override string ToString() => $"{{ {PropertyBinding}: {IntValue} }}";
}

public partial class StringParameter
{
    public StringParameter(string shaderPropertyBinding, string value)
    {
        PropertyBinding = shaderPropertyBinding;
        StringValue = value;
    }

    public override string ToString() => $"{{ {PropertyBinding}: {StringValue} }}";
}

public partial class ShaderBinding
{
    public override string ToString() => $"{{ {ShaderName} }}";
}

public partial class TextureParameter
{
    public TextureParameter(string shaderPropertyBinding, uint slot, string textureFile)
    {
        PropertyBinding = shaderPropertyBinding;
        TextureSlot = slot;
        TextureFile = textureFile;
    }

    public override string ToString() => $"{{ {TextureFile}, Bound to: {PropertyBinding}, t{TextureSlot} }}";
}

