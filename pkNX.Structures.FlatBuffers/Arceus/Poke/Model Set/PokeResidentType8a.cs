using FlatSharp.Attributes;

namespace pkNX.Structures.FlatBuffers;

[FlatBufferEnum(typeof(ulong))]
public enum PokeResidentType8a : ulong
{
    enigma = 0xC75DDBE25402B11C,
    residents_188 = 0xE0F6613235D0AEEE,
    residents_189 = 0xE0F6623235D0B0A1,
    residents_190 = 0xE0F9DF3235D3BADF,
};
