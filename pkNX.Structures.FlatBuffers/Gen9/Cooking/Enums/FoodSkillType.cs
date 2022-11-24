using FlatSharp.Attributes;
// ReSharper disable UnusedMember.Global

namespace pkNX.Structures.FlatBuffers;

[FlatBufferEnum(typeof(int))]
public enum FoodSkillType
{
    NONE = 0,
    EGG = 1,
    CAPTURE = 2,
    EXP = 3,
    LOST_PROPERTY = 4,
    RAID = 5,
    ANOTHER_NAME = 6,
    RARE = 7,
    GIGANT = 8,
    MIINIMUM = 9,
    ENCOUNT = 10,
}
