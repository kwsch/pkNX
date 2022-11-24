using FlatSharp.Attributes;
// ReSharper disable UnusedMember.Global

namespace pkNX.Structures.FlatBuffers;

[FlatBufferEnum(typeof(int))]
public enum TalentType
{
    RANDOM = 0,
    V_NUM = 1,
    VALUE = 2,
}
