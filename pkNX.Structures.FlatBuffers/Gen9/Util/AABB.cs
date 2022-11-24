using System.ComponentModel;
using FlatSharp.Attributes;
// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedType.Global
#nullable disable

namespace pkNX.Structures.FlatBuffers;

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class AABB
{
    [FlatBufferItem(0)] public Vector3f Min { get; set; }
    [FlatBufferItem(1)] public Vector3f Max { get; set; }
}
