using FlatSharp.Attributes;
// ReSharper disable UnusedMember.Global

namespace pkNX.Structures.FlatBuffers;

[FlatBufferEnum(typeof(byte))]
public enum EncAttr : byte
{
    kusamura = 0,
    umi = 1,
    sabaku = 2,
    iwa = 3,
    mizu_umi = 4,
    kuramura = 5,
    suna_sunahama = 6,
    mizu_mizutamari = 7,
    doro = 8,
    tsuchi = 9,
    yuki_katai = 10,
    suna_sabaku = 11,
    yuki = 12,
    shiba_kare = 13,
    mizu_kawa = 14,
    mizu_ike = 15,
    kusamura_kare = 16,
    shiba = 17,
    dummy = 18,
}
