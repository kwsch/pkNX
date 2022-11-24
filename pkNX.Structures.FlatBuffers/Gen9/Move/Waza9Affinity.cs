using FlatSharp.Attributes;
// ReSharper disable UnusedMember.Global

namespace pkNX.Structures.FlatBuffers;

[FlatBufferEnum(typeof(byte))]
public enum Waza9Affinity : byte
{
    None = 0,
    Support = 3,
    Self = 4,
    Attack = 5,
    Strong = 6,
    Shuriken = 7,
}
