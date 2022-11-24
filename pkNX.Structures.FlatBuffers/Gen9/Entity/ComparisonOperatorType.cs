using FlatSharp.Attributes;
// ReSharper disable UnusedMember.Global

namespace pkNX.Structures.FlatBuffers;

[FlatBufferEnum(typeof(int))]
public enum ComparisonOperatorType
{
    EQUAL = 0,
    NOT_EQUAL = 1,
    GREATER_THAN = 2,
    GREATER_THAN_EQUAL = 3,
    LESS_THAN = 4,
    LESS_THAN_EQUAL = 5,
}
