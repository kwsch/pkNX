using System;

namespace pkNX.Structures;

[Flags]
public enum TrainerAI : byte
{
    None = 0,

    Basic = 1 << 0,
    Strong = 1 << 1,
    Expert = 1 << 2,

    Doubles = 1 << 3,
    Allowance = 1 << 4,
    UseItem = 1 << 5,
    PokeChange = 1 << 6,
    Unused = 1 << 7,
}
