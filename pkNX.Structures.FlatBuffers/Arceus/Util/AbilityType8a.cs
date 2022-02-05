using FlatSharp.Attributes;

// ReSharper disable UnusedMember.Global

namespace pkNX.Structures.FlatBuffers;

[FlatBufferEnum(typeof(int))]
public enum AbilityType8a
{
    Any12 = 0,
    Any12H = 1,
    Only1 = 2,
    Only2 = 3,
    OnlyH = 4,
}
