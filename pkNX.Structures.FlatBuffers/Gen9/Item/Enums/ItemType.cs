using FlatSharp.Attributes;
// ReSharper disable UnusedMember.Global

namespace pkNX.Structures.FlatBuffers;

[FlatBufferEnum(typeof(int))]
public enum ItemType
{
    ITEMTYPE_POCKET = 0,
    ITEMTYPE_DRUG = 1,
    ITEMTYPE_EQUIP = 2,
    ITEMTYPE_NORMAL = 3,
    ITEMTYPE_BATTLE = 4,
    ITEMTYPE_BALL = 5,
    ITEMTYPE_MAIL = 6,
    ITEMTYPE_WAZA = 7,
    ITEMTYPE_NUTS = 8,
    ITEMTYPE_EVENT = 9,
    ITEMTYPE_RECIPE = 10,
    ITEMTYPE_CAPTURE = 11,
    ITEMTYPE_MATERIAL = 12,
    ITEMTYPE_NONE = 13,
}
