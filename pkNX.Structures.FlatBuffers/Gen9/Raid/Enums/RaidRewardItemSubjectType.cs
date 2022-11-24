using FlatSharp.Attributes;
// ReSharper disable UnusedMember.Global

namespace pkNX.Structures.FlatBuffers;

[FlatBufferEnum(typeof(int))]
public enum RaidRewardItemSubjectType
{
    ALL = 0,
    HOST = 1,
    CLIENT = 2,
    ONCE = 3,
}
