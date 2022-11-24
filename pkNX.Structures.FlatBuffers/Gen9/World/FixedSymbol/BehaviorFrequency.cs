using FlatSharp.Attributes;
// ReSharper disable UnusedMember.Global

namespace pkNX.Structures.FlatBuffers;

[FlatBufferEnum(typeof(int))]
public enum BehaviorFrequency
{
    NONE = -1,
    kEvery1Frame = 0,
    KEvery2Frames = 1,
    KEvery3Frames = 2,
    KEvery4Frames = 3,
    KEvery5Frames = 4,
    KEvery6Frames = 5,
    KEvery10Frames = 6,
    KEvery12Frames = 7,
    KEvery15Frames = 8,
    KEvery30Frames = 9,
    KEvery60Frames = 10,
}
