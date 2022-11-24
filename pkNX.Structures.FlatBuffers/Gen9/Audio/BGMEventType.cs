using FlatSharp.Attributes;
// ReSharper disable UnusedMember.Global

namespace pkNX.Structures.FlatBuffers;

[FlatBufferEnum(typeof(int))]
public enum BGMEventType
{
    Set = 0,
    Play = 1,
    Stop = 2,
    PlayWait = 3,
    StopWait = 4,
    Lock = 5,
    Unlock = 6,
    SetPlay = 7,
    PostEvent = 8,
    SetState = 9,
    SetPending = 10,
    UnsetPending = 11,
    ResetHoldTime = 12,
    UnlockForce = 13,
}
