using FlatSharp.Attributes;
// ReSharper disable UnusedMember.Global

namespace pkNX.Structures.FlatBuffers;

[FlatBufferEnum(typeof(int))]
public enum SexType
{
    DEFAULT = 0,
    MALE = 1,
    FEMALE = 2,
}
