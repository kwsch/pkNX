using FlatSharp.Attributes;
// ReSharper disable UnusedMember.Global

namespace pkNX.Structures.FlatBuffers;

[FlatBufferEnum(typeof(int))]
public enum BattleFunctionType
{
    BTLFUNC_NONE = 0,
    BTLFUNC_BALL = 1,
    BTLFUNC_RECOVER = 2,
    BTLFUNC_ESCAPE = 3,
}
