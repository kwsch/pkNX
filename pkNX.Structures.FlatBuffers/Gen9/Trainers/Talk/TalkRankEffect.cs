using FlatSharp.Attributes;
// ReSharper disable UnusedMember.Global

namespace pkNX.Structures.FlatBuffers;

[FlatBufferEnum(typeof(uint))]
public enum TalkRankEffect: uint
{
    Non = 0,
    Attack = 1,
    Defence = 2,
    SpAttack = 3,
    SpDefence = 4,
    Agility = 5,
    Hit = 6,
    Avoid = 7,
    CriticalRaito = 8,
    Multi5 = 9,
    Attack_SpAttack = 11,
    Defence_SpDefence = 12,
    Attack2 = 13,
    SpAttack2 = 14,
}
