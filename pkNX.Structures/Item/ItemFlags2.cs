using System;

namespace pkNX.Structures;

[Flags]
public enum ItemFlags2 : byte
{
    None,
    AddEVSpD = 1 << 0,
    AddEVAbove100 = 1 << 1,
    AddFriendship1 = 1 << 2,
    AddFriendship2 = 1 << 3,
    AddFriendship3 = 1 << 4,
    Unused1 = 1 << 5,
    Unused2 = 1 << 6,
    Unused3 = 1 << 7,
}
