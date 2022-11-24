using FlatSharp.Attributes;
// ReSharper disable UnusedMember.Global

namespace pkNX.Structures.FlatBuffers;

[FlatBufferEnum(typeof(int))]
public enum TokuseiType
{
    RANDOM_12 = 0,
    RANDOM_123 = 1,
    SET_1 = 2,
    SET_2 = 3,
    SET_3 = 4,
}
