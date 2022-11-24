using FlatSharp.Attributes;
// ReSharper disable UnusedMember.Global

namespace pkNX.Structures.FlatBuffers;

[FlatBufferEnum(typeof(int))]
public enum RareType
{
    DEFAULT = 0,
    NO_RARE = 1,
    RARE = 2,
}
