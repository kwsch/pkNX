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

    public PackedVec3f Center => Min + Extents;
    public PackedVec3f Extents => Size / 2;
    public PackedVec3f Size => Max - Min;

    public static AABB operator /(AABB l, float r) => new(){ Min = l.Min / r, Max = l.Max / r };

    public static implicit operator AABB(PackedAABB p) => new() { Min = p.Min, Max = p.Max };
    public override string ToString() => $"{{ Min: {Min}, Max: {Max} }}";
}
