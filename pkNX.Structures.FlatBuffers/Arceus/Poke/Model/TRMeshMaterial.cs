using FlatSharp.Attributes;
using System;
using System.ComponentModel;

namespace pkNX.Structures.FlatBuffers;

// *.trmmt

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class MaterialSwitches
{
    [FlatBufferItem(00)] public string Name { get; set; } = string.Empty;
    [FlatBufferItem(01)] public byte Flags { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class MaterialMapper
{
    [FlatBufferItem(00)] public string MeshName { get; set; } = string.Empty;
    [FlatBufferItem(01)] public string MaterialName { get; set; } = string.Empty;
    [FlatBufferItem(02)] public string LayerName { get; set; } = string.Empty;
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class EmbeddedTracm
{
    [FlatBufferItem(00)] public byte[] ByteBuffer { get; set; } = Array.Empty<byte>();
}

//Appears only on Arceus?
[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class MaterialProperties
{
    [FlatBufferItem(00)] public string Name { get; set; } = string.Empty;
    [FlatBufferItem(01)] public MaterialMapper[] Mappers { get; set; } = Array.Empty<MaterialMapper>();
    [FlatBufferItem(02)] public uint Field_02 { get; set; }
    [FlatBufferItem(03)] public uint Field_03 { get; set; }
    [FlatBufferItem(04)] public EmbeddedTracm Tracm { get; set; } = new();
    [FlatBufferItem(05)] public uint[] Field_05 { get; set; } = Array.Empty<uint>();
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class Mmt
{
    [FlatBufferItem(00)] public string Name { get; set; } = string.Empty;
    [FlatBufferItem(01)] public string[] FileNames { get; set; } = Array.Empty<string>();
    [FlatBufferItem(02)] public MaterialSwitches[] MaterialSwitches { get; set; } = Array.Empty<MaterialSwitches>();
    [FlatBufferItem(03)] public MaterialProperties[] MaterialProperties { get; set; } = Array.Empty<MaterialProperties>();
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class TRMeshMaterial
{
    [FlatBufferItem(00)] public uint Field_00 { get; set; }
    [FlatBufferItem(01)] public uint Field_01 { get; set; }
    [FlatBufferItem(02)] public Mmt[] Material { get; set; } = Array.Empty<Mmt>();
}
