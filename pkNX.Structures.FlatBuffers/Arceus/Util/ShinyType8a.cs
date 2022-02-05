using FlatSharp.Attributes;

// ReSharper disable UnusedMember.Global

namespace pkNX.Structures.FlatBuffers;

[FlatBufferEnum(typeof(int))]
public enum ShinyType8a
{
    Random = 0,
    Always = 1,
    Never = 2,
}
