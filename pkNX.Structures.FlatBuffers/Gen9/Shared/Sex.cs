using FlatSharp.Attributes;
// ReSharper disable UnusedMember.Global

namespace pkNX.Structures.FlatBuffers;

[FlatBufferEnum(typeof(int))]
public enum Sex
{
    MALE = 0,
    FEMALE = 1,
    UNKNOWN = 2,
}
