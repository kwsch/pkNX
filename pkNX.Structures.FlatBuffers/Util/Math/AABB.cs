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
    [FlatBufferItem(0)] public PackedVec3f Min { get; set; }
    [FlatBufferItem(1)] public PackedVec3f Max { get; set; }

    public override string ToString() => $"{{ Min: {Min}, Max: {Max} }}";
}
