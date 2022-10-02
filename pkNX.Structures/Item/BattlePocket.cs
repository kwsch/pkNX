using System;

namespace pkNX.Structures;

[Flags]
public enum BattlePocket : byte
{
    None,
    Ball = 1 << 0,
    Boosts = 1 << 1,
    Restore = 1 << 2,
    Misc = 1 << 3,
}
