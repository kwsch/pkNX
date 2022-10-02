using System;

namespace pkNX.Structures;

[Flags]
public enum ItemFlags1 : byte
{
    None,
    RestorePP = 1 << 0,
    RestorePPAll = 1 << 1,
    RestoreHP = 1 << 2,
    AddEVHP = 1 << 3,
    AddEVAtk = 1 << 4,
    AddEVDef = 1 << 5,
    AddEVSpe = 1 << 6,
    AddEVSpA = 1 << 7,
}
