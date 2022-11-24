using FlatSharp.Attributes;
// ReSharper disable UnusedMember.Global

namespace pkNX.Structures.FlatBuffers;

[FlatBufferEnum(typeof(int))]
public enum SeikakuType
{
    DEFAULT = 0,
    GANBARIYA = 1,
    SAMISIGARIYA = 2,
    YUUKAN = 3,
    IJIPPARI = 4,
    YANTYA = 5,
    ZUBUTOI = 6,
    SUNAO = 7,
    NONKI = 8,
    WANPAKU = 9,
    NOUTENKI = 10,
    OKUBYOU = 11,
    SEKKATI = 12,
    MAJIME = 13,
    YOUKI = 14,
    MUJYAKI = 15,
    HIKAEME = 16,
    OTTORI = 17,
    REISEI = 18,
    TEREYA = 19,
    UKKARIYA = 20,
    ODAYAKA = 21,
    OTONASII = 22,
    NAMAIKI = 23,
    SINNTYOU = 24,
    KIMAGURE = 25,
}
