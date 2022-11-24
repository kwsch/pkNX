using FlatSharp.Attributes;
// ReSharper disable UnusedMember.Global

namespace pkNX.Structures.FlatBuffers;

[FlatBufferEnum(typeof(short))]
public enum Biome : short
{
    NONE = 0,
    GRASS = 1,
    FOREST = 2,
    TOWN = 3,
    DESERT = 4,
    MOUNTAIN = 5,
    SNOW = 6,
    SWAMP = 7,
    LAKE = 8,
    RIVER = 9,
    OSEAN = 10,
    UNDERGROUND = 11,
    ROCKY = 12,
    CAVE = 13,
    BEACH = 14,
    FLOWER = 15,
    BAMBOO = 16,
    WASTELAND = 17,
    VOLCANO = 18,
    MINE = 19,
    OLIVE = 20,
    RUINS = 21,
    CAVE_WATER = 22,
}
