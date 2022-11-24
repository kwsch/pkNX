using FlatSharp.Attributes;
// ReSharper disable UnusedMember.Global

namespace pkNX.Structures.FlatBuffers;

[FlatBufferEnum(typeof(int))]
public enum WorkType
{
    WORKTYPE_OTHER = 0,
    WORKTYPE_EffectPokemon = 1,
}
