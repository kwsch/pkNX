using System;
using System.ComponentModel;

namespace pkNX.Structures.FlatBuffers.Arceus;

// *.trmtr

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class SamplerState { }

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class Float4Parameter
{
    public Float4Parameter(string shaderPropertyBinding, Color4f value)
    {
        PropertyBinding = shaderPropertyBinding;
        ColorValue = value;
    }

    public override string ToString() => $"{{ {PropertyBinding}: {ColorValue} }}";
}

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class FloatParameter
{
    public FloatParameter(string shaderPropertyBinding, float value)
    {
        PropertyBinding = shaderPropertyBinding;
        FloatValue = value;
    }

    public override string ToString() => $"{{ {PropertyBinding}: {FloatValue} }}";
}

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class IntParameter
{
    public IntParameter(string shaderPropertyBinding, int value)
    {
        PropertyBinding = shaderPropertyBinding;
        IntValue = value;
    }

    public override string ToString() => $"{{ {PropertyBinding}: {IntValue} }}";
}

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class StringParameter
{
    public StringParameter(string shaderPropertyBinding, string value)
    {
        PropertyBinding = shaderPropertyBinding;
        StringValue = value;
    }

    public override string ToString() => $"{{ {PropertyBinding}: {StringValue} }}";
}

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class ShaderBinding
{
    public override string ToString() => $"{{ {ShaderName} }}";
}

[TypeConverter(typeof(ExpandableObjectConverter))]
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

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class WriteMaskData { }

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class IntExtraData { }

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class MaterialPass { }

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class Material { }
