using FlatSharp.Attributes;
// ReSharper disable UnusedMember.Global

namespace pkNX.Structures.FlatBuffers;

[FlatBufferEnum(typeof(int))]
public enum SellType
{
    SELL_BUY = 0,
    BUY_ONLY = 1,
}
