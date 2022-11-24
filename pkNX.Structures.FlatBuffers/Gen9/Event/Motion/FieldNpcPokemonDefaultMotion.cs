using FlatSharp.Attributes;
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

namespace pkNX.Structures.FlatBuffers;

[FlatBufferEnum(typeof(uint))]
public enum FieldNpcPokemonDefaultMotion : uint
{
    Ground = 0,
    Water = 1,
    Sky = 2,
}
