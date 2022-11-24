using FlatSharp.Attributes;
// ReSharper disable UnusedMember.Global

namespace pkNX.Structures.FlatBuffers;

[FlatBufferEnum(typeof(int))]
public enum ClerkType
{
    CLERK = 0,
    NO_CLERK = 1,
}
