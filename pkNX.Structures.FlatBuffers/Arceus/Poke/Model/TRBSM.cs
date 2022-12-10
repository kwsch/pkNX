using FlatSharp.Attributes;
using System;
using System.ComponentModel;

namespace pkNX.Structures.FlatBuffers;

// *.trbsm

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class UnkEntry
{
    [FlatBufferItem(00)] public string Name { get; set; } = string.Empty;
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class BoneEntry
{
    [FlatBufferItem(00)] public string Name { get; set; } = string.Empty;
    [FlatBufferItem(01)] public string[] Slots { get; set; } = Array.Empty<string>();
    [FlatBufferItem(02)] public float Unk3 { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class ScalerEntry
{
    [FlatBufferItem(00)] public string Name { get; set; } = string.Empty;
    [FlatBufferItem(01)] public string[] SlotName { get; set; } = Array.Empty<string>();
    [FlatBufferItem(02)] public byte Unk3 { get; set; }
    [FlatBufferItem(03)] public float[] Unk4 { get; set; } = Array.Empty<float>();
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class MeshEntry
{
    [FlatBufferItem(00)] public string Name { get; set; } = string.Empty;
    [FlatBufferItem(01)] public string[] SlotName { get; set; } = Array.Empty<string>();
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class Trbsm
{
    [FlatBufferItem(00)] public BoneEntry[] Bones { get; set; } = Array.Empty<BoneEntry>();
    [FlatBufferItem(01)] public ScalerEntry[] Scalers { get; set; } = Array.Empty<ScalerEntry>();
    [FlatBufferItem(02)] public MeshEntry[] Mesh { get; set; } = Array.Empty<MeshEntry>();
    [FlatBufferItem(03)] public UnkEntry Unk { get; set; } = new();
}
