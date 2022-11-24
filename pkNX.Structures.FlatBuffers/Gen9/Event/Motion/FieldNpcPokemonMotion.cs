using FlatSharp.Attributes;
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

namespace pkNX.Structures.FlatBuffers;

[FlatBufferEnum(typeof(uint))]
public enum FieldNpcPokemonMotion : uint
{
    DefaultWait = 0,
    Glad = 1,
    Notice = 2,
    Hate = 3,
    Refresh = 4,
    Sleep = 5,
    Eat = 6,
    BattleWait = 7,
    Attack = 8,
    RangeAttack = 9,
    Roar = 10,
    Body = 11,
    Punch = 12,
    Kick = 13,
    Tail = 14,
    Bite = 15,
    WildBool02 = 16,
}
