using FlatSharp.Attributes;
using pkNX.Containers;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;

namespace pkNX.Structures.FlatBuffers;

// *.trmsh

[FlatBufferEnum(typeof(uint))]
public enum VertexAttributeIndex : uint
{
    NONE = 0,
    POSITION,
    NORMAL,
    TANGENT,
    BINORMAL,
    COLOR,
    TEX_COORD,
    BLEND_INDEX,
    BLEND_WEIGHTS,
}

[FlatBufferEnum(typeof(uint))]
public enum VertexLayoutType : uint
{
    None = 0,
    R8_G8_B8_A8_Unsigned_Normalized = 20,
    W8_X8_Y8_Z8_Unsigned = 22,
    W16_X16_Y16_Z16_Signed_Normalized = 39,
    W16_X16_Y16_Z16_Float = 43,
    X32_Y32_Float = 48,
    X32_Y32_Z32_Float = 51,
    W32_X32_Y32_Z32_Float = 54,
}

[FlatBufferEnum(typeof(uint))]
public enum IndexLayoutType : uint
{
    IndexLayoutType_Uint8 = 0,
    IndexLayoutType_Uint16,
    IndexLayoutType_Uint32,
    IndexLayoutType_Uint64,
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class BoneWeights
{
    [FlatBufferItem(00)] public uint RigIndex { get; set; }
    [FlatBufferItem(01)] public float Weight { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class VertexAttribute
{
    [FlatBufferItem(00)] public int Field_00 { get; set; } // -1? Always zero on PLA all models
    [FlatBufferItem(01)] public VertexAttributeIndex Attribute { get; set; } = VertexAttributeIndex.NONE;
    [FlatBufferItem(02)] public uint AttributeLayer { get; set; } // Chooses which Color3Param or UV layer to use
    [FlatBufferItem(03)] public uint Type { get; set; }
    [FlatBufferItem(04)] public uint Offset { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class VertexSize
{
    [FlatBufferItem(00)] public uint Size { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class VertexAttributeLayout
{
    [FlatBufferItem(00)] public VertexAttribute[] Attrs { get; set; } = Array.Empty<VertexAttribute>();
    [FlatBufferItem(01)] public VertexSize[] Size { get; set; } = Array.Empty<VertexSize>();
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class SubMesh
{
    [FlatBufferItem(00)] public uint PolyCount { get; set; }
    [FlatBufferItem(01)] public uint PolyOffset { get; set; }
    [FlatBufferItem(02)] public uint Field_02 { get; set; } // Always zero on PLA all models
    [FlatBufferItem(03)] public string MaterialName { get; set; } = string.Empty;
    [FlatBufferItem(04)] public int Field_04 { get; set; } // -1?
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class MeshShape
{
    [FlatBufferItem(00)] public string MeshShapeName { get; set; } = string.Empty;
    [FlatBufferItem(01)] public AABB Bounds { get; set; } = new();
    [FlatBufferItem(02)] public IndexLayoutType IndexLayoutType { get; set; }
    [FlatBufferItem(03)] public VertexAttributeLayout[] Attributes { get; set; } = Array.Empty<VertexAttributeLayout>();
    [FlatBufferItem(04)] public SubMesh[] SubMeshes { get; set; } = Array.Empty<SubMesh>();
    [FlatBufferItem(05)] public uint Field_05 { get; set; } // Always zero on PLA all models
    [FlatBufferItem(06)] public uint Field_06 { get; set; } // Always zero on PLA all models
    [FlatBufferItem(07)] public uint Field_07 { get; set; } // Always zero on PLA all models
    [FlatBufferItem(08)] public uint Field_08 { get; set; } // Always zero on PLA all models
    [FlatBufferItem(09)] public PackedSphere BoundingSphere { get; set; } = new();
    [FlatBufferItem(10)] public BoneWeights[] Weights { get; set; } = Array.Empty<BoneWeights>();
    [FlatBufferItem(11)] public string Field_11 { get; set; } = string.Empty; // Always empty on PLA all models
    [FlatBufferItem(12)] public string MeshName { get; set; } = string.Empty;
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class TRMesh
{
    [FlatBufferItem(00)] public uint Field_00 { get; set; } // Always zero on PLA all models
    [FlatBufferItem(01)] public MeshShape[] Shapes { get; set; } = Array.Empty<MeshShape>();
    [FlatBufferItem(02)] public string BufferFileName { get; set; } = string.Empty;
}
