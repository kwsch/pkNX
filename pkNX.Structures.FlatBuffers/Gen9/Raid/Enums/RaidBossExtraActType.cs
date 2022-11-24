using FlatSharp.Attributes;
// ReSharper disable UnusedMember.Global

namespace pkNX.Structures.FlatBuffers;

[FlatBufferEnum(typeof(short))]
public enum RaidBossExtraActType : short
{
    NONE = 0,
    BOSS_STATUS_RESET = 1,
    PLAYER_STATUS_RESET = 2,
    WAZA = 3,
    GEM_COUNT = 4,
}
