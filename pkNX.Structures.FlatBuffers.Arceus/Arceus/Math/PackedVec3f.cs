using System;
using System.ComponentModel;

namespace pkNX.Structures.FlatBuffers.Arceus;

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial struct PackedVec3f
{
    public static implicit operator PackedVec3f(Vec3f v) => new() { X = v.X, Y = v.Y, Z = v.Z };
}
