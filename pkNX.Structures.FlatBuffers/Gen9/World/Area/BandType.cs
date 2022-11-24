using FlatSharp.Attributes;
// ReSharper disable UnusedMember.Global

namespace pkNX.Structures.FlatBuffers;

[FlatBufferEnum(typeof(short))]
public enum BandType : short
{
    NONE = 0,
    SAME = 1,
    BOSS = 2,
}
