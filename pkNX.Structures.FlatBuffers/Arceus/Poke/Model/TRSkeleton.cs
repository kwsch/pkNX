using FlatSharp.Attributes;
using System;
using System.ComponentModel;

namespace pkNX.Structures.FlatBuffers;

// *.trskl

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class TRSkeleton
{
    [FlatBufferItem(00)] public uint Field_00 { get; set; }
    [FlatBufferItem(01)] public TransformNode[] Bones { get; set; } = Array.Empty<TransformNode>();
    [FlatBufferItem(02)] public Bone[] BoneParams { get; set; } = Array.Empty<Bone>();
    [FlatBufferItem(03)] public IkControl[] Iks { get; set; } = Array.Empty<IkControl>();
    [FlatBufferItem(04)] public int RigOffset { get; set; }
}

[FlatBufferEnum(typeof(uint))]
public enum NodeType : uint
{
    Transform = 0,
    Joint = 1,
    Locator = 2,
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class TransformNode
{
    [FlatBufferItem(00)] public string Name { get; set; } = string.Empty;
    [FlatBufferItem(01)] public Transform Transform { get; set; } = new();
    [FlatBufferItem(02)] public PackedVec3f ScalePivot { get; set; } = new();
    [FlatBufferItem(03)] public PackedVec3f RotatePivot { get; set; } = new();

    [FlatBufferItem(04, DefaultValue = -1)] public int ParentIdx { get; set; } = -1;
    [FlatBufferItem(05, DefaultValue = -1)] public int RigIdx { get; set; } = -1;
    [FlatBufferItem(06)] public string LocatorBone { get; set; } = string.Empty;
    [FlatBufferItem(07)] public NodeType Type { get; set; } = NodeType.Transform;
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class IkControl
{
    [FlatBufferItem(00)] public string Name { get; set; } = string.Empty;
    [FlatBufferItem(01)] public string Target { get; set; } = string.Empty;
    [FlatBufferItem(02)] public string Pole { get; set; } = string.Empty;
    [FlatBufferItem(03)] public string Type { get; set; } = string.Empty;
    [FlatBufferItem(04)] public uint Field_04 { get; set; }
    [FlatBufferItem(05)] public PackedVec3f Position { get; set; } = new();
    [FlatBufferItem(06)] public PackedQuaternion Rotation { get; set; } = new();
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class Bone
{
    [FlatBufferItem(00)] public byte LockTranslation { get; set; }

    [FlatBufferItem(01)] public byte Field_01 { get; set; } = 1; //Always set to 1
    [FlatBufferItem(02)] public Matrix4x3f Matrix { get; set; } = new();
}
