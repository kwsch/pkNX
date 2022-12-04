using System;
using System.ComponentModel;
using FlatSharp.Attributes;

// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global

namespace pkNX.Structures.FlatBuffers;

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class TrinitySceneObjectSV
{
    [FlatBufferItem(00)] public string ObjectName { get; set; } = string.Empty;
    [FlatBufferItem(01)] public TrinitySceneObjectPositionSV ObjectPosition { get; set; } = new();
    [FlatBufferItem(02)] public uint Field_02 { get; set; }
    [FlatBufferItem(03)] public uint Field_03 { get; set; }
    [FlatBufferItem(04)] public string Field_04 { get; set; } = string.Empty;
    [FlatBufferItem(05)] public byte Field_05 { get; set; }
    [FlatBufferItem(06)] public uint Field_06 { get; set; }
    [FlatBufferItem(07)] public string Field_07 { get; set; } = string.Empty;
    [FlatBufferItem(08)] public string[] Field_08 { get; set; } = Array.Empty<string>();

}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class TrinitySceneObjectPositionSV
{
    [FlatBufferItem(00)] public PackedVec3f Field_00 { get; set; } = new();
    [FlatBufferItem(01)] public PackedVec3f Field_01 { get; set; } = new();
    [FlatBufferItem(02)] public PackedVec3f Field_02 { get; set; } = new();
}
