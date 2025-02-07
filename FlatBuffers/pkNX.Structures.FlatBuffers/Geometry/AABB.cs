namespace pkNX.Structures.FlatBuffers;

public partial class AABB
{
    public Vec3f Center => Min + Extents;
    public Vec3f Extents => Size / 2;
    public Vec3f Size => Max - Min;

    public static AABB operator /(AABB l, float r) => new() { Min = l.Min / r, Max = l.Max / r };
    public static implicit operator AABB(PackedAABB p) => new() { Min = p.Min, Max = p.Max };

    public override string ToString() => $"{{ Min: {Min}, Max: {Max} }}";
}
