using FlatSharp.Attributes;
// ReSharper disable UnusedMember.Global

namespace pkNX.Structures.FlatBuffers;

[FlatBufferEnum(typeof(int))]
public enum AjitoTypeEnum
{
    Fire = 0,
    Dark = 1,
    Fairy = 2,
    Fighting = 3,
    Poison = 4,
}
