using System.ComponentModel;
using FlatSharp.Attributes;

namespace pkNX.Structures.FlatBuffers;

[FlatBufferStruct, TypeConverter(typeof(ExpandableObjectConverter))]
public class PackedAABB
{
    [FlatBufferItem(0)] public PackedVec3f Min { get; set; } = new();
    [FlatBufferItem(1)] public PackedVec3f Max { get; set; } = new();

    public PackedVec3f Center => Min + Extents;
    public PackedVec3f Extents => Size / 2;
    public PackedVec3f Size => Max - Min;

    public static PackedAABB operator /(PackedAABB l, float r) => new(){ Min = l.Min / r, Max = l.Max / r };

    public static implicit operator PackedAABB(AABB p) => new() { Min = p.Min, Max = p.Max };

    public override string ToString() => $"{{ Min: {Min}, Max: {Max} }}";
}
