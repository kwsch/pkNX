using FlatSharp.Attributes;
// ReSharper disable UnusedMember.Global

namespace pkNX.Structures.FlatBuffers;

[FlatBufferEnum(typeof(int))]
public enum SeasoningType
{
    NONE = 0,
    MAYONNAISE = 1,
    KETCHUP = 2,
    MUSTARD = 3,
    BUTTER = 4,
    PEANUT_BUTTER = 5,
    CHILI_SAUCE = 6,
    SALT = 7,
    PEPPER = 8,
    YOGHURT = 9,
    WHIPPED_CREAM = 10,
    CREAM_CHEESE = 11,
    BERRY_JAM = 12,
    MARMALADE = 13,
    OLIVE_OIL = 14,
    VINEGAR = 15,
    HORSE_RADISH = 16,
    CURRY_POWEDER = 17,
    WASABI_SAUCE = 18,
    TREASURE_SPICE_SWEET = 19,
    TREASURE_SPICE_SALTY = 20,
    TREASURE_SPICE_SOUR = 21,
    TREASURE_SPICE_BITTER = 22,
    TREASURE_SPICE_UMAMI = 23,
}
