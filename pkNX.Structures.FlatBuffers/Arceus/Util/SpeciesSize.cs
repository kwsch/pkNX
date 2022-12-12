using FlatSharp.Attributes;

namespace pkNX.Structures.FlatBuffers;

[FlatBufferEnum(typeof(byte))]
public enum SpeciesSize : byte
{
    Size_S = 0,
    Size_M = 1,
    Size_L = 2,
    Size_XL = 3,
}
