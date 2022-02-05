using FlatSharp.Attributes;

// ReSharper disable UnusedMember.Global

namespace pkNX.Structures.FlatBuffers;

[FlatBufferEnum(typeof(int))]
public enum NatureType8a
{
    Random = -1,

    Hardy,
    Lonely,
    Brave,
    Adamant,
    Naughty,
    Bold,

    Docile,
    Relaxed,
    Impish,
    Lax,
    Timid,
    Hasty,

    Serious,
    Jolly,
    Naive,
    Modest,
    Mild,
    Quiet,

    Bashful,
    Rash,
    Calm,
    Gentle,
    Sassy,
    Careful,

    Quirky,
}