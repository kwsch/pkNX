using FlatSharp.Attributes;
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

namespace pkNX.Structures.FlatBuffers;

[FlatBufferEnum(typeof(uint))]
public enum FieldNpcMotion : uint
{
    Wait1 = 0,
    Wait2 = 1,
    Wait3 = 2,
    Yes = 3,
    No = 4,
    Get = 5,
    Pass = 6,
    Glad = 7,
    Think = 8,
    Byebye = 9,
    Cheer = 10,
    Search = 11,
    Angry = 12,
    Sad = 13,
    Surprise = 14,
    Grief = 15,
    Laugh = 16,
    Withstand = 17,
    Bow = 18,
    Typing = 19,
    Speak1 = 20,
    Speak2 = 21,
    LS14_chair01_idle = 22,
    LS14_speak01 = 23,
    LS14_listen01 = 24,
    LS18_chair03_idle = 25,
    LS18_speak03 = 26,
    LS18_listen03 = 27,
    LS19_chair02_idle = 28,
    LS19_speak02 = 29,
    LS19_listen02 = 30,
    LS19_chairsleep02 = 31,
    LS23_sleep01 = 32,
    LS27_halfsit01 = 33,
    LS36_chair04_idle = 34,
    LS36_speak04 = 35,
    LS36_listen04 = 36,
    Chair01 = 37,
    Chair02 = 38,
    Chair03 = 39,
}
