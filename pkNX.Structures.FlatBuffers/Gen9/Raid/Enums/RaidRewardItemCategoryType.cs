using FlatSharp.Attributes;
// ReSharper disable UnusedMember.Global

namespace pkNX.Structures.FlatBuffers;

[FlatBufferEnum(typeof(int))]
public enum RaidRewardItemCategoryType
{
    ITEM = 0,
    POKE = 1,
    GEM = 2,
}
