using FlatSharp.Attributes;
// ReSharper disable UnusedMember.Global

namespace pkNX.Structures.FlatBuffers;

[FlatBufferEnum(typeof(int))]
public enum GemType
{
    DEFAULT = 0,
    RANDOM = 1,
    NORMAL = 2,
    KAKUTOU = 3,
    HIKOU = 4,
    DOKU = 5,
    JIMEN = 6,
    IWA = 7,
    MUSHI = 8,
    GHOST = 9,
    HAGANE = 10,
    HONOO = 11,
    MIZU = 12,
    KUSA = 13,
    DENKI = 14,
    ESPER = 15,
    KOORI = 16,
    DRAGON = 17,
    AKU = 18,
    FAIRY = 19,
}
