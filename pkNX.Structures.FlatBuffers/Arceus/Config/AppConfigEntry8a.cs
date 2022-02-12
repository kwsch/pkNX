using System;
using System.ComponentModel;
using FlatSharp.Attributes;

// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global

namespace pkNX.Structures.FlatBuffers;

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class AppconfigEntry8a
{
    [FlatBufferItem(00)] public uint Hash { get; set; }
    [FlatBufferItem(01)] public string OriginalPath { get; set; } = string.Empty;
    [FlatBufferItem(02)] public string Folder { get; set; } = string.Empty;
    [FlatBufferItem(03)] public string FileNameWithoutExtension { get; set; } = string.Empty;
    [FlatBufferItem(04)] public byte Unused { get => 0; set { } }
    [FlatBufferItem(05)] public FlatDummyEntry[] UnusedEntries { get; set; } = Array.Empty<FlatDummyEntry>();
}
