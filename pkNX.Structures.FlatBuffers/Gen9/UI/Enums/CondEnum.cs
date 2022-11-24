using FlatSharp.Attributes;
// ReSharper disable UnusedMember.Global

namespace pkNX.Structures.FlatBuffers;

[FlatBufferEnum(typeof(int))]
public enum CondEnum
{
    NONE = 0,
    SYSTEM_FLAG = 1,
    SCENARIO = 2,
    GYMBADGENUM = 3,
}
