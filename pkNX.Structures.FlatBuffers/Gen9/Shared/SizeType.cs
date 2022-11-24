using FlatSharp.Attributes;
// ReSharper disable UnusedMember.Global

namespace pkNX.Structures.FlatBuffers;

[FlatBufferEnum(typeof(int))]
public enum SizeType
{
    RANDOM = 0,
    XS = 1,
    S = 2,
    M = 3,
    L = 4,
    XL = 5,
    VALUE = 6,
}
