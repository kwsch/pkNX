using System.ComponentModel;
using FlatSharp.Attributes;
// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedType.Global
#nullable disable

namespace pkNX.Structures.FlatBuffers;

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class SRT
{
    [FlatBufferItem(0)] public Vector3f Scale { get; set; }
    [FlatBufferItem(1)] public Vector3f Rotate { get; set; }
    [FlatBufferItem(2)] public Vector3f Translate { get; set; }
}
