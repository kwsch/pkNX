namespace pkNX.Structures.FlatBuffers;

public partial struct PackedAABB
{
    public readonly Vec3f Center => Min + Extents;
    public readonly Vec3f Extents => Size / 2;
    public readonly Vec3f Size => Max - Min;

    public PackedAABB()
    {
        Min = PackedVec3f.Zero;
        Max = PackedVec3f.Zero;
    }

    public PackedAABB(Vec3f min, Vec3f max)
    {
        Min = (PackedVec3f)min;
        Max = (PackedVec3f)max;
    }

    public static explicit operator PackedAABB(AABB v) => new() { Min = v.Min, Max = v.Max };

    public static PackedAABB operator /(PackedAABB l, float r) => new() { Min = l.Min / r, Max = l.Max / r };

    public readonly override string ToString() => $"{{ Min: {Min}, Max: {Max} }}";
}
