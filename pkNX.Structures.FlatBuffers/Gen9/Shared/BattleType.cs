using FlatSharp.Attributes;
// ReSharper disable UnusedMember.Global

namespace pkNX.Structures.FlatBuffers;

[FlatBufferEnum(typeof(int))]
public enum BattleType
{
    SINGLE = 0,
    DOUBLE = 1,
    MULTI = 2,
}
