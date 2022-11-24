using FlatSharp.Attributes;
// ReSharper disable UnusedMember.Global

namespace pkNX.Structures.FlatBuffers;

[FlatBufferEnum(typeof(int))]
public enum AreaTag
{
    None = 0,
    NG_Encount = 1,
    Area_EncountLv = 2,
    NG_Item = 3,
    Area_CrashRock = 4,
    NG_CrashRock = 5,
    NG_ActionPoint = 6,
    NG_All = 7,
}
