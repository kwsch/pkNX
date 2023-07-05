using System.ComponentModel;

namespace pkNX.Structures.FlatBuffers.Arceus;

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial struct PackedVec2f
{
    public static implicit operator PackedVec2f(Vec2f v) => new() { X = v.X, Y = v.Y };
}
