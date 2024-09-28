using System;

namespace pkNX.Structures;

[Flags]
public enum MoveStat : int
{
    None = 0,
    Attack = 1,
    Defense = 2,
    SpecialAttack = 3,
    SpecialDefense = 4,
    Speed = 5,
    Accuracy = 6,
    Evasiveness = 7,
    All = 8 // Used in Ancient Power and the like
}

