using FlatSharp.Attributes;
// ReSharper disable UnusedMember.Global

namespace pkNX.Structures.FlatBuffers;

[FlatBufferEnum(typeof(int))]
public enum AreaType
{
    Default = 0,
    Field = 1,
    Town = 2,
    Cave = 3,
    Room = 4,
}
