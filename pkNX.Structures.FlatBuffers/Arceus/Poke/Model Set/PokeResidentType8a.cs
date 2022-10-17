using FlatSharp.Attributes;

namespace pkNX.Structures.FlatBuffers;

[FlatBufferEnum(typeof(ulong))]
public enum PokeResidentType8a : ulong
{
    enigma = 0xC75DDBE25402B11C,
    unk_01 = 0xE0F6613235D0AEEE,
    unk_02 = 0xE0F9DF3235D3BADF,
    unk_03 = 0xE0F6623235D0B0A1,
};
