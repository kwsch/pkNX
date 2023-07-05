using System.ComponentModel;

namespace pkNX.Structures.FlatBuffers.Arceus;

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial struct PackedVec4f
{
    public static implicit operator PackedVec4f(Vec4f v) => new() { X = v.X, Y = v.Y, Z = v.Z, W = v.W };
}
