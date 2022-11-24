using FlatSharp.Attributes;
// ReSharper disable UnusedMember.Global

namespace pkNX.Structures.FlatBuffers;

[FlatBufferEnum(typeof(int))]
public enum FieldPocket
{
    FPOCKET_DRUG = 0,
    FPOCKET_BALL = 1,
    FPOCKET_BATTLE = 2,
    FPOCKET_NUTS = 3,
    FPOCKET_OTHER = 4,
    FPOCKET_WAZA = 5,
    FPOCKET_TREASURE = 6,
    FPOCKET_PICNIC = 7,
    FPOCKET_EVENT = 8,
    FPOCKET_MATERIAL = 9,
    FPOCKET_RECIPE = 10,
    FPOCKET_NONE = 11,
}
