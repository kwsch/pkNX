using FlatSharp.Attributes;
using System;
using System.ComponentModel;

namespace pkNX.Structures.FlatBuffers.SWSH;

// *.gfbmdl


[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class LayerData
{
}

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class RenderLayer
{
}

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class Info
{
}

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class GFBModel
{
}

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class VertexAttribute8
{
}

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class Polygon
{
}

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class Shape
{
}

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class Mesh8
{
}

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class TransparencyGroupNode
{
}

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class Bone8
{
}

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class UberShaderSettings
{
}

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class SamplerState8
{
}

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class Texture8
{
    public override string ToString() => $"{{ texture[{TextureIndex}], Bound to: {SamplerName}, {Settings} }}";
}

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class Flag
{
    public override string ToString() => $"{{ {FlagName}: {FlagEnable} }}";
}

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class FloatParam
{
    public override string ToString() => $"{{ {ValueName}: {Value} }}";
}

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class Color3Param
{
    public override string ToString() => $"{{ {ColorName}: {Color} }}";
}

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class Material8
{
}
