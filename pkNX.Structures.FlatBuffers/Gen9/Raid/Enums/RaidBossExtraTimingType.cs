using FlatSharp.Attributes;
// ReSharper disable UnusedMember.Global

namespace pkNX.Structures.FlatBuffers;

[FlatBufferEnum(typeof(short))]
public enum RaidBossExtraTimingType : short
{
    NONE = 0,
    TIME = 1,
    HP = 2,
}
