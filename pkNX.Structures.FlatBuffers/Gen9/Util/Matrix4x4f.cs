using System.ComponentModel;
using FlatSharp.Attributes;
// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedType.Global
#nullable disable

namespace pkNX.Structures.FlatBuffers;

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class Matrix4x4f
{
    [FlatBufferItem(0)] public Vector4f AxisX { get; set; }
    [FlatBufferItem(1)] public Vector4f AxisY { get; set; }
    [FlatBufferItem(2)] public Vector4f AxisZ { get; set; }
    [FlatBufferItem(3)] public Vector4f AxisW { get; set; }
}
