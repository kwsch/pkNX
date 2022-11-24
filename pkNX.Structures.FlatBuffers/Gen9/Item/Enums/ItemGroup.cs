using FlatSharp.Attributes;
// ReSharper disable UnusedMember.Global

namespace pkNX.Structures.FlatBuffers;

[FlatBufferEnum(typeof(int))]
public enum ItemGroup
{
    ITEMGROUP_NONE = 0,
    ITEMGROUP_BALL = 1,
    ITEMGROUP_POCKET = 2,
    ITEMGROUP_NUTS = 3,
    ITEMGROUP_WAZA_MACHINE = 4,
    ITEMGROUP_HIDEN_MACHINE = 5,
    ITEMGROUP_JEWEL = 6,
    ITEMGROUP_MEGA_STONE = 7,
    ITEMGROUP_PIECE = 8,
    ITEMGROUP_BEADS = 9,
    ITEMGROUP_ROTOPON = 10,
    ITEMGROUP_HEART = 11,
    ITEMGROUP_RESEARCH = 12,
    ITEMGROUP_AMULET_ITEM = 13,
}
