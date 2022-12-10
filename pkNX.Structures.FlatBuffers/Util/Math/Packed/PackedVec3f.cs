using System.ComponentModel;
using FlatSharp.Attributes;

namespace pkNX.Structures.FlatBuffers;

[FlatBufferStruct, TypeConverter(typeof(ExpandableObjectConverter))]
public class PackedVec3f
{
    [FlatBufferItem(00)] public float X { get; set; }
    [FlatBufferItem(01)] public float Y { get; set; }
    [FlatBufferItem(02)] public float Z { get; set; }

    public override string ToString() => $"{{ X: {X}, Y: {Y}, Z: {Z} }}";
}
