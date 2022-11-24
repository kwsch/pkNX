using FlatSharp.Attributes;
// ReSharper disable UnusedMember.Global

namespace pkNX.Structures.FlatBuffers;

[FlatBufferEnum(typeof(int))]
public enum NpcTrafficFormation
{
    Right = 0,
    Left = 1,
    Back = 2,
}
