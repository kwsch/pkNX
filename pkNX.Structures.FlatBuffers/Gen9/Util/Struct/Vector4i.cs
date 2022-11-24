using System.ComponentModel;
using FlatSharp.Attributes;
// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedType.Global

namespace pkNX.Structures.FlatBuffers;

[FlatBufferStruct, TypeConverter(typeof(ExpandableObjectConverter))]
public class Vector4i
{
    [FlatBufferItem(0)] public int X { get; set; }
    [FlatBufferItem(1)] public int Y { get; set; }
    [FlatBufferItem(2)] public int Z { get; set; }
    [FlatBufferItem(3)] public int W { get; set; }
}
