using FlatSharp.Attributes;
// ReSharper disable UnusedMember.Global

namespace pkNX.Structures.FlatBuffers;

[FlatBufferEnum(typeof(short))]
public enum RaidRomType : short
{
    BOTH = 0,
    TYPE_A = 1,
    TYPE_B = 2,
}
