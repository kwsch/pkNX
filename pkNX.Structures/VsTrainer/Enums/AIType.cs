using System;

namespace pkNX.Structures
{
    [Flags]
    public enum AIType : ushort
    {
        None = 0,
        Basic = 1 << 0,
        Strong = 1 << 1,
        Expert = 1 << 2,

        Double = 1 << 7,
        // Allowance,
        // UseItem,
        // Switch,
    }
}