
// ReSharper disable UnusedMember.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global

using FlatSharp.Attributes;
using System.ComponentModel;

namespace pkNX.Structures.FlatBuffers;

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class TrinityScenePointSV
{
    [FlatBufferItem(00)] public string Name { get; set; } = string.Empty;
    [FlatBufferItem(01)] public Vector3f Position { get; set; } = new();
    [FlatBufferItem(02)] public byte Field_02 { get; set; }
}
