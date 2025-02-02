namespace pkNX.Structures.FlatBuffers.SWSH;

public partial class GFBPokeConfig
{
    public AABB Bounds => new() { Min = new() { X = MinBX / 100, Y = MinBY / 100, Z = MinBZ / 100 }, Max = new() { X = MaxBX / 100, Y = MaxBY / 100, Z = MaxBZ / 100 } };
}
