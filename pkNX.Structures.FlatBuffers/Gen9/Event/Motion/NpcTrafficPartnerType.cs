using FlatSharp.Attributes;
// ReSharper disable UnusedMember.Global

namespace pkNX.Structures.FlatBuffers;

[FlatBufferEnum(typeof(int))]
public enum NpcTrafficPartnerType
{
    None = 0,
    Npc = 1,
    Poke = 2,
}
