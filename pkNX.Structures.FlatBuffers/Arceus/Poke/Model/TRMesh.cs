using FlatSharp.Attributes;
using pkNX.Containers;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;

namespace pkNX.Structures.FlatBuffers;

// *.trmsh

[FlatBufferEnum(typeof(uint))]
public enum InputLayoutSemanticName : uint
{
    NONE = 0,
    POSITION,
    NORMAL,
    TANGENT,
    BINORMAL,
    COLOR,
    TEXCOORD,
    BLEND_INDICES,
    BLEND_WEIGHTS,
}

[FlatBufferEnum(typeof(uint))]
public enum InputLayoutFormat : uint
{
    NONE = 0,
    RGBA_8_UNORM = 20,
    RGBA_8_UNSIGNED = 22,
    RGBA_16_UNORM = 39,
    RGBA_16_FLOAT = 43,
    RG_32_FLOAT = 48,
    RGB_32_FLOAT = 51,
    RGBA_32_FLOAT = 54,
}

[FlatBufferEnum(typeof(uint))]
public enum IndexLayoutFormat : uint // Possibly part of InputLayoutFormat and should then be named something like DataFormat
{
    UINT8 = 0, // TODO: Unconfirmed
    UINT16 = 1,
    UINT32,  // TODO: Unconfirmed
    UINT64,  // TODO: Unconfirmed
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class BoneWeights
{
    [FlatBufferItem(00)] public uint RigIndex { get; set; }
    [FlatBufferItem(01)] public float Weight { get; set; }

    public override string ToString() => $"{{ [{RigIndex}]: {Weight} }}";
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class InputLayoutElement
{
    [FlatBufferItem(00, DefaultValue = -1)] public int Slot { get; set; } // Possibly input slot? Always zero on PLA all models, but zero is not the default!
    [FlatBufferItem(01)] public InputLayoutSemanticName SemanticName { get; set; } = InputLayoutSemanticName.NONE;
    [FlatBufferItem(02)] public uint SemanticIndex { get; set; }
    [FlatBufferItem(03)] public InputLayoutFormat Format { get; set; }
    [FlatBufferItem(04)] public uint Offset { get; set; }


    public override string ToString() => $"{{ {SemanticName}{SemanticIndex:#} ({Format}) @ {Offset:00} }}";
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class VertexSize
{
    [FlatBufferItem(00)] public uint Size { get; set; }

    public override string ToString() => $"{{ {Size} }}";
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class VertexAttributeLayout
{
    [FlatBufferItem(00)] public InputLayoutElement[] Elements { get; set; } = Array.Empty<InputLayoutElement>();
    [FlatBufferItem(01)] public VertexSize[] Size { get; set; } = Array.Empty<VertexSize>();
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class SubMesh
{
    [FlatBufferItem(00)] public uint IndexCount { get; set; }
    [FlatBufferItem(01)] public uint IndexOffset { get; set; }
    [FlatBufferItem(02, DefaultValue = (uint)1)] public uint Field_02 { get; set; } // Always zero on PLA all models
    [FlatBufferItem(03)] public string MaterialName { get; set; } = string.Empty;
    [FlatBufferItem(04, DefaultValue = -1)] public int Field_04 { get; set; } // 0 or default value
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class MeshShape
{
    [FlatBufferItem(00)] public string MeshShapeName { get; set; } = string.Empty;
    [FlatBufferItem(01)] public AABB Bounds { get; set; } = new();
    [FlatBufferItem(02)] public IndexLayoutFormat IndexLayoutFormat { get; set; }
    [FlatBufferItem(03)] public VertexAttributeLayout[] VertexLayout { get; set; } = Array.Empty<VertexAttributeLayout>();
    [FlatBufferItem(04)] public SubMesh[] SubMeshes { get; set; } = Array.Empty<SubMesh>();
    [FlatBufferItem(05)] public uint Field_05 { get; set; } // Always default on PLA all models
    [FlatBufferItem(06)] public uint Field_06 { get; set; } // Always default on PLA all models
    [FlatBufferItem(07)] public uint Field_07 { get; set; } // Always default on PLA all models
    [FlatBufferItem(08)] public uint Field_08 { get; set; } // Always default on PLA all models
    [FlatBufferItem(09)] public PackedSphere BoundingSphere { get; set; } = new();
    [FlatBufferItem(10)] public BoneWeights[] Weights { get; set; } = Array.Empty<BoneWeights>();
    [FlatBufferItem(11)] public string Field_11 { get; set; } = string.Empty; // Always empty on PLA all models
    [FlatBufferItem(12)] public string MeshName { get; set; } = string.Empty;
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class TRMesh
{
    [FlatBufferItem(00, DefaultValue = (uint)1)] public uint Field_00 { get; set; } // Always zero on PLA all models
    [FlatBufferItem(01)] public MeshShape[] Shapes { get; set; } = Array.Empty<MeshShape>();
    [FlatBufferItem(02)] public string BufferFileName { get; set; } = string.Empty;
}
