using System;
using System.ComponentModel;
using FlatSharp.Attributes;

// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
#nullable disable

namespace pkNX.Structures.FlatBuffers;

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class TrinitySceneObjectTemplateSV
{
    [FlatBufferItem(00)] public string ObjectTemplateName { get; set; } = string.Empty;
    [FlatBufferItem(01)] public string ObjectTemplateExtra { get; set; } = string.Empty;
    [FlatBufferItem(02)] public uint Field_02 { get; set; }
    [FlatBufferItem(03)] public uint Field_03 { get; set; }
    [FlatBufferItem(04)] public TrinitySceneObjectTemplateEntrySV[] Objects { get; set; } = Array.Empty<TrinitySceneObjectTemplateEntrySV>();
    [FlatBufferItem(05)] public uint[] Field_05 { get; set; } = Array.Empty<uint>();
    [FlatBufferItem(06)] public byte Field_06 { get; set; } 

}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class TrinitySceneObjectTemplateEntrySV
{
    [FlatBufferItem(00)] public string Type { get; set; }
    [FlatBufferItem(01)] public byte[] Data { get; set; } = Array.Empty<byte>();
    [FlatBufferItem(02)] public TrinitySceneObjectTemplateEntrySV[] SubObjects { get; set; } = Array.Empty<TrinitySceneObjectTemplateEntrySV>();
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class TrinitySceneObjectTemplateDataSV
{
    [FlatBufferItem(00)] public string ObjectTemplateName { get; set; } = string.Empty;
    [FlatBufferItem(01)] public string ObjectTemplateExtra { get; set; } = string.Empty;
    [FlatBufferItem(02)] public string ObjectTemplatePath { get; set; } = string.Empty;
    [FlatBufferItem(03)] public byte Field_03 { get; set; }
    [FlatBufferItem(04)] public string Type { get; set; } = string.Empty;
    [FlatBufferItem(05)] public byte[] Data { get; set; } = Array.Empty<byte>();
}
