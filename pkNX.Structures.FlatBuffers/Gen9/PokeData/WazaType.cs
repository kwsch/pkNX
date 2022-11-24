using FlatSharp.Attributes;

namespace pkNX.Structures.FlatBuffers;

[FlatBufferEnum(typeof(int))]
public enum WazaType
{
    DEFAULT = 0,
    MANUAL = 1,
}
