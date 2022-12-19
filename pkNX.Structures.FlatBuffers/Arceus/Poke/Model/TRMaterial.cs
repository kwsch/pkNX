using FlatSharp.Attributes;
using System;
using System.ComponentModel;

namespace pkNX.Structures.FlatBuffers;

// *.trmtr

[FlatBufferEnum(typeof(uint))]
public enum UvWrapMode : uint
{
    //Time to test these as bit fields?
    Wrap = 0, //0000
    Clamp = 1, //0001
    Mirror = 6, //0110
    MirrorOnce = 7, //0111
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class SamplerState
{
    [FlatBufferItem(00)] public uint SamplerState0 { get; set; } = 0x0; //default
    [FlatBufferItem(01)] public uint SamplerState1 { get; set; } = 0x0; //default
    [FlatBufferItem(02)] public uint SamplerState2 { get; set; } = 0x0; //default
    [FlatBufferItem(03)] public uint SamplerState3 { get; set; } = 0x0; //default
    [FlatBufferItem(04)] public uint SamplerState4 { get; set; } = 0x0; //default
    [FlatBufferItem(05)] public uint SamplerState5 { get; set; } = 0x0; //default
    [FlatBufferItem(06)] public uint SamplerState6 { get; set; } = 0x0; //default
    [FlatBufferItem(07)] public uint SamplerState7 { get; set; } = 0x0; //default
    [FlatBufferItem(08)] public uint SamplerState8 { get; set; } = 0x0; //default
    [FlatBufferItem(09)] public UvWrapMode RepeatU { get; set; } = UvWrapMode.Clamp; //0x1, 0x6 or 0x7
    [FlatBufferItem(10)] public UvWrapMode RepeatV { get; set; } = UvWrapMode.Clamp; //0x1, 0x6 or 0x7
    [FlatBufferItem(11)] public UvWrapMode RepeatW { get; set; } = UvWrapMode.Wrap; //Never used
    [FlatBufferItem(12)] public PackedColor4f BorderColor { get; set; } = new();
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class Float4Parameter
{
    [FlatBufferItem(00)] public string PropertyBinding { get; set; } = string.Empty;
    [FlatBufferItem(01)] public PackedColor4f ColorValue { get; set; } = new();

    public Float4Parameter() { }
    public Float4Parameter(string shaderPropertyBinding, PackedColor4f value)
    {
        PropertyBinding = shaderPropertyBinding;
        ColorValue = value;
    }

    public override string ToString() => $"{{ {PropertyBinding}: {ColorValue} }}";
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class FloatParameter
{
    [FlatBufferItem(00)] public string PropertyBinding { get; set; } = string.Empty;
    [FlatBufferItem(01)] public float FloatValue { get; set; }

    public FloatParameter() { }
    public FloatParameter(string shaderPropertyBinding, float value)
    {
        PropertyBinding = shaderPropertyBinding;
        FloatValue = value;
    }

    public override string ToString() => $"{{ {PropertyBinding}: {FloatValue} }}";
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class IntParameter
{
    [FlatBufferItem(00)] public string PropertyBinding { get; set; } = string.Empty;
    [FlatBufferItem(01)] public int IntValue { get; set; }

    public IntParameter() { }
    public IntParameter(string shaderPropertyBinding, int value)
    {
        PropertyBinding = shaderPropertyBinding;
        IntValue = value;
    }

    public override string ToString() => $"{{ {PropertyBinding}: {IntValue} }}";
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class StringParameter
{
    [FlatBufferItem(00)] public string PropertyBinding { get; set; } = string.Empty;
    [FlatBufferItem(01)] public string StringValue { get; set; } = string.Empty;

    public StringParameter() { }
    public StringParameter(string shaderPropertyBinding, string value)
    {
        PropertyBinding = shaderPropertyBinding;
        StringValue = value;
    }

    public override string ToString() => $"{{ {PropertyBinding}: {StringValue} }}";
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class Shader
{
    [FlatBufferItem(00)] public string ShaderName { get; set; } = string.Empty;
    [FlatBufferItem(01)] public StringParameter[] ShaderValues { get; set; } = Array.Empty<StringParameter>();

    public override string ToString() => $"{{ {ShaderName} }}";
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class TextureParameter
{
    [FlatBufferItem(00)] public string PropertyBinding { get; set; } = string.Empty;
    [FlatBufferItem(01)] public string TextureFile { get; set; } = string.Empty;
    [FlatBufferItem(02)] public uint TextureSlot { get; set; }

    public TextureParameter() { }
    public TextureParameter(string shaderPropertyBinding, uint slot, string textureFile)
    {
        PropertyBinding = shaderPropertyBinding;
        TextureSlot = slot;
        TextureFile = textureFile;
    }

    public override string ToString() => $"{{ {TextureFile}, Bound to: {PropertyBinding}, t{TextureSlot} }}";
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class WriteMask
{
    //Only pm0448 has it as 0x0 in Transparent Shader
    [FlatBufferItem(00, DefaultValue = (byte)0xFF)] public byte Mask { get; set; } = 0xFF;
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class IntExtra
{
    [FlatBufferItem(00)] public uint Field_00 { get; set; }
    //0 when Transparent
    //1 when NonDirectional
    [FlatBufferItem(01)] public int Value { get; set; } = -1;
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class MaterialPass
{
    [FlatBufferItem(00)] public string Name { get; set; } = string.Empty;
    [FlatBufferItem(01)] public Shader[] Shaders { get; set; } = Array.Empty<Shader>();
    [FlatBufferItem(02)] public TextureParameter[] TextureParameters { get; set; } = Array.Empty<TextureParameter>();
    [FlatBufferItem(03)] public SamplerState[] Samplers { get; set; } = Array.Empty<SamplerState>();
    [FlatBufferItem(04)] public FloatParameter[] FloatParameters { get; set; } = Array.Empty<FloatParameter>();
    [FlatBufferItem(05)] public string[] Field_05 { get; set; } = Array.Empty<string>();
    [FlatBufferItem(06)] public Float4Parameter[] Float4LightParameter { get; set; } = Array.Empty<Float4Parameter>();
    [FlatBufferItem(07)] public Float4Parameter[] Float4Parameters { get; set; } = Array.Empty<Float4Parameter>();
    [FlatBufferItem(08)] public string[] Field_08 { get; set; } // TODO
    [FlatBufferItem(09)] public IntParameter[] IntParameters { get; set; } = Array.Empty<IntParameter>();
    [FlatBufferItem(10)] public string[] Field_10 { get; set; } // TODO
    [FlatBufferItem(11)] public string[] Field_11 { get; set; } // TODO
    [FlatBufferItem(12)] public string[] Field_12 { get; set; } // TODO
    [FlatBufferItem(13)] public WriteMask ByteExtra { get; set; } = new();
    [FlatBufferItem(14)] public IntExtra IntExtra { get; set; } = new();
    [FlatBufferItem(15)] public string AlphaType { get; set; } = string.Empty;
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class TRMaterial
{
    [FlatBufferItem(00)] public uint Field_00 { get; set; } // Default
    [FlatBufferItem(01)] public MaterialPass[] MaterialPasses { get; set; } = Array.Empty<MaterialPass>();
}
