using FlatSharp.Attributes;
// ReSharper disable UnusedMember.Global

namespace pkNX.Structures.FlatBuffers;

[FlatBufferEnum(typeof(int))]
public enum DataType
{
    NORMAL = 0,
    ITEM = 1,
    WAZA = 2,
    MULTI = 3,
}
