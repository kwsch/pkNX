using FlatSharp.Attributes;
// ReSharper disable UnusedMember.Global

namespace pkNX.Structures.FlatBuffers;

[FlatBufferEnum(typeof(int))]
public enum LangType
{
    ROM_LANG = 0,
    JAPAN = 1,
    ENGLISH = 2,
    FRANCE = 3,
    ITALY = 4,
    GERMANY = 5,
    SPAIN = 6,
    KOREA = 7,
    SIMPLIFIED_CHINESE = 8,
    TRADITIONAL_CHINESE = 9,
}
