using System;

namespace pkNX.Containers;

[Flags]
public enum NSOFlag : uint
{
    None = 0,
    CompressedText = 1 << 0,
    CompressedRO = 1 << 1,
    CompressedData = 1 << 2,
    CheckHashText = 1 << 3,
    CheckHashRO = 1 << 4,
    CheckHashData = 1 << 5,
    Unused6 = 1 << 6,
    Unused7 = 1 << 7,
}

public static class NsoFlagExtensions
{
    public static bool HasFlagFast(this NSOFlag value, NSOFlag flag) => (value & flag) != 0;
}
