using FlatSharp.Attributes;
// ReSharper disable UnusedMember.Global
#nullable disable

namespace pkNX.Structures.FlatBuffers;

[FlatBufferEnum(typeof(int))]
public enum TrainerBodySize
{
    S = 0,
    M = 1,
    L = 2,
    LL = 3,
}
