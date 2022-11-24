using FlatSharp.Attributes;
// ReSharper disable UnusedMember.Global

namespace pkNX.Structures.FlatBuffers;

[FlatBufferEnum(typeof(int))]
public enum CollisionShape
{
    NONE = 0,
    SPHERE = 1,
    BOX = 2,
    CAPSULE = 3,
}
