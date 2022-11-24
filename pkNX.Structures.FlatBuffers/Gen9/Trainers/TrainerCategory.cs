using FlatSharp.Attributes;
// ReSharper disable UnusedMember.Global
#nullable disable

namespace pkNX.Structures.FlatBuffers;

[FlatBufferEnum(typeof(byte))]
public enum TrainerCategory: byte
{
    NORMAL = 0,
    GYM_LEADER = 1,
}
