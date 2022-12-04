using System.ComponentModel;
using FlatSharp;
using FlatSharp.Attributes;

// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global

namespace pkNX.Structures.FlatBuffers;

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class TrinityCollisionComponentSV
{
    [FlatBufferItem(00, Required = true)] public FlatBufferUnion<TrinityCollisionComponent1SV> Component { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class TrinityCollisionComponent1SV
{
    [FlatBufferItem(00, Required = true)] public FlatBufferUnion<TrinityCollisionShapeSphereSV, TrinityCollisionShapeBoxSV, TrinityCollisionShapeCapsuleSV, TrinityCollisionShapeHavokSV> CollisionShape { get; set; }
    [FlatBufferItem(02)] public uint Field_02 { get; set; }
    [FlatBufferItem(03)] public byte Field_03 { get; set; }
    [FlatBufferItem(04)] public uint Field_04 { get; set; }
    [FlatBufferItem(05)] public uint Field_05 { get; set; }
    [FlatBufferItem(06)] public uint Field_06 { get; set; }
    [FlatBufferItem(07)] public uint Field_07 { get; set; }
    [FlatBufferItem(08)] public Vec3f Field_08 { get; set; } = new();
    [FlatBufferItem(09)] public string Field_09 { get; set; } = string.Empty;
    [FlatBufferItem(10)] public uint Field_0A { get; set; }
    [FlatBufferItem(11)] public byte Field_0B { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class TrinityCollisionShapeSphereSV
{
    [FlatBufferItem(00)] public string TrcolFilePath { get; set; } = string.Empty;
    [FlatBufferItem(01)] public PackedVec3f Field_01 { get; set; } = new();
    [FlatBufferItem(02)] public PackedVec3f Field_02 { get; set; } = new();
    [FlatBufferItem(03)] public PackedVec3f Field_03 { get; set; } = new();
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class TrinityCollisionShapeBoxSV
{
    [FlatBufferItem(01)] public PackedVec3f Field_01 { get; set; } = new();
    [FlatBufferItem(02)] public PackedVec3f Field_02 { get; set; } = new();
    [FlatBufferItem(03)] public PackedVec3f Field_03 { get; set; } = new();
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class TrinityCollisionShapeCapsuleSV
{
    [FlatBufferItem(00)] public string TrcolFilePath { get; set; } = string.Empty;
    [FlatBufferItem(01)] public PackedVec3f Field_01 { get; set; } = new();
    [FlatBufferItem(02)] public PackedVec3f Field_02 { get; set; } = new();
    [FlatBufferItem(03)] public PackedVec3f Field_03 { get; set; } = new();
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class TrinityCollisionShapeHavokSV
{
    [FlatBufferItem(00)] public string TrcolFilePath { get; set; } = string.Empty;
    [FlatBufferItem(01)] public PackedVec3f Field_01 { get; set; } = new();
    [FlatBufferItem(02)] public PackedVec3f Field_02 { get; set; } = new();
    [FlatBufferItem(03)] public PackedVec3f Field_03 { get; set; } = new();
}
