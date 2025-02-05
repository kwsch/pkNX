namespace pkNX.Structures.FlatBuffers;

public partial struct PackedVec4i
{
    public static implicit operator PackedVec4i(Vec4i v) => new() { X = v.X, Y = v.Y, Z = v.Z, W = v.W };
}
