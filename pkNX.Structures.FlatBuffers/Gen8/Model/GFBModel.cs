using FlatSharp.Attributes;
using System;
using System.ComponentModel;

namespace pkNX.Structures.FlatBuffers;

// *.gfbmdl


[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class LayerData
{
    [FlatBufferItem(00)] public uint LayerIndex { get; set; }
    [FlatBufferItem(01)] public float MixFactor { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class RenderLayers
{
    [FlatBufferItem(00)] public LayerData[] Layers { get; set; } = Array.Empty<LayerData>();
    [FlatBufferItem(01)] public float MixFactor { get; set; }
}

[FlatBufferStruct, TypeConverter(typeof(ExpandableObjectConverter))]
public class Info
{
    [FlatBufferItem(00)] public ushort V1 { get; set; }
    [FlatBufferItem(01)] public ushort V2 { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class GFBModel
{
    [FlatBufferItem(00)] public Info Version { get; set; } = new();
    [FlatBufferItem(01)] public PackedAABB BoundingBox { get; set; } = new();
    [FlatBufferItem(02)] public string[] TextureFiles { get; set; } = Array.Empty<string>();
    [FlatBufferItem(03)] public string[] VertexShaders { get; set; } = Array.Empty<string>();
    [FlatBufferItem(04)] public string[] GeometryShaders { get; set; } = Array.Empty<string>();
    [FlatBufferItem(05)] public string[] FragmentShaders { get; set; } = Array.Empty<string>();
    [FlatBufferItem(06)] public Material8[] Materials { get; set; } = Array.Empty<Material8>();
    [FlatBufferItem(07)] public Mesh8[] Mesh { get; set; } = Array.Empty<Mesh8>();
    [FlatBufferItem(08)] public Shape[] Shapes { get; set; } = Array.Empty<Shape>();
    [FlatBufferItem(09)] public Bone8[] Skeleton { get; set; } = Array.Empty<Bone8>();
    [FlatBufferItem(10)] public TransparencyGroupNode[] TransparencyGroup { get; set; } = Array.Empty<TransparencyGroupNode>();
    [FlatBufferItem(11)] public RenderLayers[] RenderLayers { get; set; } = Array.Empty<RenderLayers>();//
}

[FlatBufferEnum(typeof(byte))]
public enum Attribute8 : byte
{
    //Using GLTF friendly attribute names
    Position = 0,
    Normal,
    Tangent,
    Texcoord_0,
    Texcoord_1,
    Texcoord_2,
    Texcoord_3,
    Color_0,
    Color_1,
    Color_2,
    Color_3,
    Group_Idx = 11,
    Group_Weight,
}

[FlatBufferEnum(typeof(byte))]
public enum DataType8 : byte
{
    Float = 0,
    HalfFloat = 1,
    Byte = 2,
    UByte = 3,
    Short = 4,
    UShort = 5,
    Int = 6,
    UInt = 7,
    FixedPoint = 8,
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class VertexAttribute8
{
    [FlatBufferItem(00)] public Attribute8 Type { get; set; } = 0;
    [FlatBufferItem(01)] public DataType8 Format { get; set; } = 0;
    [FlatBufferItem(02)] public uint Count { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class Polygon
{
    [FlatBufferItem(00)] public uint MaterialId { get; set; }
    [FlatBufferItem(01)] public ushort[] Indices { get; set; } = Array.Empty<ushort>();
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class Shape
{
    [FlatBufferItem(00)] public Polygon[] Polygons { get; set; } = Array.Empty<Polygon>();
    [FlatBufferItem(01)] public VertexAttribute8[] Attributes { get; set; } = Array.Empty<VertexAttribute8>();
    [FlatBufferItem(02)] public byte[] Vertices { get; set; } = Array.Empty<byte>();
}

[FlatBufferEnum(typeof(uint))]
public enum SortPriority : uint
{
    S0 = 0,
    S1 = 1,
    S2 = 2,
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class Mesh8
{
    [FlatBufferItem(00)] public uint BoneId { get; set; }
    [FlatBufferItem(01)] public uint ShapeId { get; set; }
    [FlatBufferItem(02)] public PackedAABB PackedAabb { get; set; } = new();
    [FlatBufferItem(03)] public SortPriority SortPriority { get; set; } = SortPriority.S0; // Values have been 0 or 0x2 so far
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class TransparencyGroupNode
{
    [FlatBufferItem(00)] public uint BoneIndex { get; set; }
    //Possible values: -1, 0, 1, 2
    [FlatBufferItem(01)] public uint DrawLayer { get; set; }
    //Always a descending array
    [FlatBufferItem(02)] public uint[] BoneChildren { get; set; } = Array.Empty<uint>();
    [FlatBufferItem(03)] public PackedAABB CollisionVolume { get; set; }
}

[FlatBufferEnum(typeof(uint))]
public enum BoneType : uint
{
    Transform = 0, //Used for area and effect rigs
    Joint = 1, //Used for normal rigs
    Locator = 2, //Unused for the most part
    Transparency_Group = 3, //Used for "Alphas", "Transparency", "Okus" and "Adds"
}

[FlatBufferEnum(typeof(uint))]
public enum BillboardType : uint
{
    None = 0,
    AxisY = 1,
    AxisXy = 2,
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class Bone8
{
    [FlatBufferItem(00)] public string Name { get; set; }
    [FlatBufferItem(01)] public BoneType Type { get; set; } = 0; //0 (Default), 1, 2, 3
    [FlatBufferItem(02)] public int ParentIdx { get; set; } = 0; //0, 1, 2 or -1
    [FlatBufferItem(03)] public BillboardType Effect { get; set; } = 0; //0, 1, 2, mostly seen in effects (ee, em and ew)

    [FlatBufferItem(04)] public bool Visible { get; set; } = true;

    [FlatBufferItem(05)] public PackedVec3f Scale { get; set; }
    [FlatBufferItem(06)] public PackedVec3f Rotation { get; set; }
    [FlatBufferItem(07)] public PackedVec3f Translation { get; set; }

    [FlatBufferItem(08)] public PackedVec3f ScalePivot { get; set; }
    [FlatBufferItem(09)] public PackedVec3f RotatePivot { get; set; }

    [FlatBufferItem(10, DefaultValue = true)] public bool IsRigged { get; set; } = true;
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class UberShaderSettings
{
    [FlatBufferItem(00)] public Flag[] UberFlags { get; set; } = Array.Empty<Flag>();
    [FlatBufferItem(01)] public FloatParam[] UberValue { get; set; } = Array.Empty<FloatParam>();
    [FlatBufferItem(02)] public Color3Param[] UberColor { get; set; } = Array.Empty<Color3Param>();
}

[FlatBufferEnum(typeof(uint))]
public enum FilterMode : uint
{
    MinMagMipLinear,
    MinMagMipPoint,
    Anisotropic,
    MinPoint_MagPoint_MipLinear,
    MinPoint_MagLinear_MipPoint,
    MinPoint_MagLinear_MipLinear,
    MinLinear_MagPoint_MipPoint,
    MinLinear_MagPoint_MipLinear,
    MinLinear_MagLinear_MipPoint,
    Comparison_MinPoint_MagPoint_MipPoint,
    Comparison_MinPoint_MagPoint_MipLinear,
    Comparison_MinPoint_MagLinear_MipPoint,
    Comparison_MinPoint_MagLinear_MipLinear,
    Comparison_MinLinear_MagPoint_MipPoint,
    Comparison_MinLinear_MagPoint_MipLinear,
    Comparison_MinLinear_MagLinear_MipPoint,
    Comparison_MinLinear_MagLinear_MipLinear,
    Comparison_Anisotropic,
    Minimum_MinPoint_MagPoint_MipPoint,
    Minimum_MinPoint_MagPoint_MipLinear,
    Minimum_MinPoint_MagLinear_MipPoint,
    Minimum_MinPoint_MagLinear_MipLinear,
    Minimum_MinLinear_MagPoint_MipPoint,
    Minimum_MinLinear_MagPoint_MipLinear,
    Minimum_MinLinear_MagLinear_MipPoint,
    Minimum_MinLinear_MagLinear_MipLinear,
    Minimum_Anisotropic,
    Maximum_MinPoint_MagPoint_MipPoint,
    Maximum_MinPoint_MagPoint_MipLinear,
    Maximum_MinPoint_MagLinear_MipPoint,
    Maximum_MinPoint_MagLinear_MipLinear,
    Maximum_MinLinear_MagPoint_MipPoint,
    Maximum_MinLinear_MagPoint_MipLinear,
    Maximum_MinLinear_MagLinear_MipPoint,
    Maximum_MinLinear_MagLinear_MipLinear,
    Maximum_Anisotropic
}

[FlatBufferEnum(typeof(uint))]
public enum UVWrapMode8 : uint
{
    Wrap = 0,
    Clamp = 1,
    Mirror = 2,
    Border = 3,
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class SamplerState8
{
    [FlatBufferItem(00)] public FilterMode Filtermode { get; set; }
    [FlatBufferItem(01)] public UVWrapMode8 RepeatU { get; set; }
    [FlatBufferItem(02)] public UVWrapMode8 RepeatV { get; set; } //Clamp related
    [FlatBufferItem(03)] public UVWrapMode8 Repeat2 { get; set; } //Clamp related
    [FlatBufferItem(04)] public PackedColor4f BorderColor { get; set; }
    [FlatBufferItem(05)] public float MipMapBias { get; set; }
}

//TODO: Use GLSL friendly names, these match up to the shader
[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class Texture8
{
    [FlatBufferItem(00)] public string SamplerName { get; set; } = string.Empty;
    [FlatBufferItem(01)] public uint TextureIndex { get; set; }
    [FlatBufferItem(02)] public SamplerState8 Settings { get; set; } = new();

    public override string ToString() => $"{{ texture[{TextureIndex}], Bound to: {SamplerName}, {Settings} }}";
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class Flag
{
    [FlatBufferItem(00)] public string FlagName { get; set; } = string.Empty;
    [FlatBufferItem(01)] public bool FlagEnable { get; set; }

    public override string ToString() => $"{{ {FlagName}: {FlagEnable} }}";
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class FloatParam
{
    [FlatBufferItem(00)] public string ValueName { get; set; } = string.Empty;
    [FlatBufferItem(01)] public float Value { get; set; }

    public override string ToString() => $"{{ {ValueName}: {Value} }}";
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class Color3Param
{
    [FlatBufferItem(00)] public string ColorName { get; set; } = string.Empty;
    [FlatBufferItem(01)] public PackedColor3f Color { get; set; }

    public override string ToString() => $"{{ {ColorName}: {Color} }}";
}

[FlatBufferEnum(typeof(uint))]
public enum Pass : uint
{
    Default = 0,
    Particle = 1, //Used for bubbles and lights
    Addglass = 2, //add
    Alpglass = 3 //referred to as alp
}

[FlatBufferEnum(typeof(uint))]
public enum BlendMode : uint
{
    None = 0,
    Alpha,
    Add,
    Subtract,
    Multiply,
    Premultiply,
    Count,
}

[FlatBufferEnum(typeof(uint))]
public enum CullMode : uint
{
    None = 0,
    Back,
    Front,
}


[FlatBufferEnum(typeof(uint))]
public enum DepthComparison : uint
{
    Never = 0,
    Less,
    Lessequal,
    Greater,
    Notequal,
    Greaterequal,
    Always = 7,
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class Material8
{
    [FlatBufferItem(00)] public string Name { get; set; } = string.Empty;
    [FlatBufferItem(01)] public string Shader { get; set; } = string.Empty;

    [FlatBufferItem(02)] public Pass SortPriority { get; set; } = 0;

    //Byte Params
    [FlatBufferItem(03)] public byte DepthWrite { get; set; } = 0;
    [FlatBufferItem(04)] public byte DepthTest { get; set; } = 0;

    //Int Params
    [FlatBufferItem(05)] public uint LightSetNum { get; set; } = 0;
    [FlatBufferItem(06)] public BlendMode BlendMode { get; set; } = 0;
    [FlatBufferItem(07)] public CullMode CullMode { get; set; } = 0;
    [FlatBufferItem(08)] public int VertexShaderFileId { get; set; } = 0;
    [FlatBufferItem(09)] public int GeomShaderFileId { get; set; } = 0;
    [FlatBufferItem(10)] public int FragShaderFileId { get; set; } = 0;

    //Nodes
    [FlatBufferItem(11)] public Texture8[] Textures { get; set; } = Array.Empty<Texture8>();
    [FlatBufferItem(12)] public Flag[] Flags { get; set; } = Array.Empty<Flag>();
    [FlatBufferItem(13)] public FloatParam[] Values { get; set; } = Array.Empty<FloatParam>();
    [FlatBufferItem(14)] public Color3Param[] Colors { get; set; } = Array.Empty<Color3Param>();

    //Four bits
    [FlatBufferItem(15)] public byte ReceiveShadow { get; set; } = 0;
    [FlatBufferItem(16)] public byte CastShadow { get; set; } = 0;
    [FlatBufferItem(17)] public byte SelfShadow { get; set; } = 0;
    [FlatBufferItem(18)] public byte TextureAlphaTest { get; set; } = 0;
    [FlatBufferItem(19)] public DepthComparison DepthComparisonFunction { get; set; } = 0;

    [FlatBufferItem(20)] public UberShaderSettings StaticParam { get; set; } = new();
    [FlatBufferItem(21)] public int DepthBias { get; set; }
    [FlatBufferItem(22)] public float Field_18 { get; set; }
    [FlatBufferItem(23)] public float Field_19 { get; set; }
}
