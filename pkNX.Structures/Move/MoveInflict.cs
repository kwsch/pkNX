using System;

namespace pkNX.Structures;

[Flags]
public enum MoveInflict : int
{
    None = 0,

    Paralyze     = 1, 
    Sleep           = 2, 
    Freeze         = 3, 
    Burn          = 4,
    Poison      = 5,
    Confusion           = 6,
    Infatuation           = 7,
    TransientDamage            = 8, // Used in Fire Spin, Wrap, etc.

    Nightmare            = 9,
    Unknown          = 10, // Not used by any move
    Unknown   = 11, // Not used by any move
    Torment             = 12,
    Disable = 13,
    Drowsiness    = 14,
    HealBlock      = 15,

    Identify            = 17, // Used by Odor Sleuth and whatnot
    Seed              = 18, // Used by Leech Seed
    Embargo              = 19,
    SingPerish              = 20, // Used by Perish Song
    Ingrain              = 22,
    ThroatAche           = 23, // Used by Throat Chop
    FireWeakness              = 42, // Used by Tar Shot
}

