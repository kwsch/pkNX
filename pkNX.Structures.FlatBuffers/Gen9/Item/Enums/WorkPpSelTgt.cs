using FlatSharp.Attributes;
// ReSharper disable UnusedMember.Global

namespace pkNX.Structures.FlatBuffers;

[FlatBufferEnum(typeof(int))]
public enum WorkPpSelTgt
{
    WORKPPSEL_NONE = 0,
    WORKPPSEL_SINGLE = 1,
    WORKPPSEL_OVERALL = 2,
}
