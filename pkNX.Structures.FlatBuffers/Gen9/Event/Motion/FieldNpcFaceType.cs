using FlatSharp.Attributes;
// ReSharper disable UnusedMember.Global

namespace pkNX.Structures.FlatBuffers;

[FlatBufferEnum(typeof(uint))]
public enum FieldNpcFaceType : uint
{
    Empty = 0,
    Default = 1,
    BattleWait = 2,
    Glad = 3,
    Angry = 4,
    Sad = 5,
    Fun = 6,
    Surprise = 7,
    CloseEyes = 8,
    BattleIntro = 9,
    Lose = 10,
    Unique01 = 20,
    Unique02 = 21,
    Unique03 = 22,
}
