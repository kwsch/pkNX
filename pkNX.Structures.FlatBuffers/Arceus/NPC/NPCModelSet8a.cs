using FlatSharp.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace pkNX.Structures.FlatBuffers;

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class NPCModelSet8a : IFlatBufferArchive<NPCModelSetEntry8a>
{
    public byte[] Write() => FlatBufferConverter.SerializeFrom(this);

    [FlatBufferItem(00)] public NPCModelSetEntry8a[] Table { get; set; } = Array.Empty<NPCModelSetEntry8a>();
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class NPCModelSetEntry8a
{
    [FlatBufferItem(00)][TypeConverter(typeof(NPCHashConverter))] public ulong NPCHash { get; set; }
    [FlatBufferItem(01)] public string NPCIndex { get; set; } = string.Empty;
    [FlatBufferItem(02)] public ulong Field_02 { get; set; }
    [FlatBufferItem(03)] public NPCModelAnimations Animations { get; set; } = new();
    [FlatBufferItem(04)] public NPCModelMeshes Meshes { get; set; } = new();
    [FlatBufferItem(05)] public NPCModelRigs Rigs { get; set; } = new();
    [FlatBufferItem(06)] public string Attachment0 { get; set; } = string.Empty;
    [FlatBufferItem(07)] public string Attachment1 { get; set; } = string.Empty;
    [FlatBufferItem(08)] public string Attachment2 { get; set; } = string.Empty;
    [FlatBufferItem(09)] public NPCModelColorConfig Colors { get; set; } = new();
    [FlatBufferItem(10)] public string ValueListID { get; set; } = string.Empty;
    [FlatBufferItem(11)] public uint[] Field_11 { get; set; } = Array.Empty<uint>();
    [FlatBufferItem(12)] public string ArchivePath { get; set; } = string.Empty;
    [FlatBufferItem(13)] public float Field_13 { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class NPCModelAnimations
{
    [FlatBufferItem(00)] public string[] FolderPath { get; set; } = Array.Empty<string>();//First value is base, usually empty string
    [FlatBufferItem(01)] public string[] AnimationPath { get; set; } = Array.Empty<string>();
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class NPCModelMeshes
{
    [FlatBufferItem(00)] public string[] FolderPath { get; set; } = Array.Empty<string>(); //First value is base, usually empty string
    [FlatBufferItem(01)] public string[] MeshPath { get; set; } = Array.Empty<string>();
    [FlatBufferItem(02)] public string[] MeshFlags { get; set; } = Array.Empty<string>(); //Can hide certain meshes like "hide_a_mesh"
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class NPCModelRigs
{
    [FlatBufferItem(00)] public string[] Rigs { get; set; } = Array.Empty<string>();
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class NPCModelColorConfig
{
    [FlatBufferItem(00)] public string[] Slots { get; set; } = Array.Empty<string>(); // First value is base, usually empty string
    [FlatBufferItem(01)] public uint[] R1 { get; set; } = Array.Empty<uint>();
    [FlatBufferItem(02)] public uint[] G1 { get; set; } = Array.Empty<uint>();
    [FlatBufferItem(03)] public uint[] B1 { get; set; } = Array.Empty<uint>();
    [FlatBufferItem(04)] public uint[] R2 { get; set; } = Array.Empty<uint>();
    [FlatBufferItem(05)] public uint[] G2 { get; set; } = Array.Empty<uint>();
    [FlatBufferItem(06)] public uint[] B2 { get; set; } = Array.Empty<uint>();
    [FlatBufferItem(07)] public uint[] R3 { get; set; } = Array.Empty<uint>();
    [FlatBufferItem(08)] public uint[] G3 { get; set; } = Array.Empty<uint>();
    [FlatBufferItem(09)] public uint[] B3 { get; set; } = Array.Empty<uint>();
}