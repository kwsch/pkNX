using FlatSharp.Attributes;
// ReSharper disable UnusedMember.Global

namespace pkNX.Structures.FlatBuffers;

[FlatBufferEnum(typeof(byte))]
public enum MoveType : byte
{
    Normal = 0,
    Kakutou = 1,
    Hikou = 2,
    Doku = 3,
    Jimen = 4,
    Iwa = 5,
    Mushi = 6,
    Ghost = 7,
    Hagane = 8,
    Honoo = 9,
    Mizu = 10,
    Kusa = 11,
    Denki = 12,
    Esper = 13,
    Koori = 14,
    Dragon = 15,
    Aku = 16,
    Fairy = 17,
    Null = 18,
}
