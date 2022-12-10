using FlatSharp.Attributes;
using System;
using System.ComponentModel;

namespace pkNX.Structures.FlatBuffers;

// *.trmdl


[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class FileReference
{
    [FlatBufferItem(00)] public string Filename { get; set; } = string.Empty;
    public override string ToString() => $"{{ {Filename} }}";
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class LODIndex
{
    [FlatBufferItem(00)] public uint Index { get; set; } = 0;
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class LOD
{
    [FlatBufferItem(00)] public LODIndex[] Entries { get; set; } = Array.Empty<LODIndex>();
    [FlatBufferItem(01)] public string Type { get; set; } = string.Empty;
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class TRModel
{
    [FlatBufferItem(00)] public uint Field_00 { get; set; }
    [FlatBufferItem(01)] public FileReference[] Meshes { get; set; } = Array.Empty<FileReference>();
    [FlatBufferItem(02)] public FileReference Skeleton { get; set; } = new();
    [FlatBufferItem(03)] public string[] Materials { get; set; } = Array.Empty<string>();
    [FlatBufferItem(04)] public LOD[] LODs { get; set; } = Array.Empty<LOD>();
    [FlatBufferItem(05)] public AABB Bounds { get; set; } = new();
    [FlatBufferItem(06)] public PackedVec4f Field_06 { get; set; } = new();
}
