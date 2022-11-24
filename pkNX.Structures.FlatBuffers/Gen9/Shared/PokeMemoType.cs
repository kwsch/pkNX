using FlatSharp.Attributes;
// ReSharper disable UnusedMember.Global

namespace pkNX.Structures.FlatBuffers;

[FlatBufferEnum(typeof(byte))]
public enum PokeMemoType : byte
{
    NONE = 0,
    Capture = 1,
    EventGet = 2,
    EventCapture = 3,
    InnerTrade = 4,
    NetTrade = 5,
    EggHatch = 6,
    Bank = 7,
    MysteryGift = 8,
    EggTakenFirst = 9,
    EggTakenTrade = 10,
}
