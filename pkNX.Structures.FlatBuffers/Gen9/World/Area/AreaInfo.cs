using System.ComponentModel;
using FlatSharp.Attributes;
// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedType.Global
#nullable disable

namespace pkNX.Structures.FlatBuffers;

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class AreaInfo
{
    [FlatBufferItem(00)] public AreaType Type { get; set; }
    [FlatBufferItem(01)] public string LocationNameMain { get; set; }
    [FlatBufferItem(02)] public string LocationNameSub { get; set; }
    [FlatBufferItem(03)] public string Bgm { get; set; }
    [FlatBufferItem(04)] public string EnvBaseSound { get; set; }
    [FlatBufferItem(05)] public string WeatherTable { get; set; }
    [FlatBufferItem(06)] public string LightFileP1 { get; set; }
    [FlatBufferItem(07)] public string LightFileP2 { get; set; }
    [FlatBufferItem(08)] public string LightFileP3 { get; set; }
    [FlatBufferItem(09)] public float ShadowClipHeightOffset { get; set; }
    [FlatBufferItem(10)] public float ShadowClipHeightMinOffset { get; set; }
    [FlatBufferItem(11)] public bool DisableLightOffset { get; set; }
    [FlatBufferItem(12)] public bool NoDisplayLocationName { get; set; }
    [FlatBufferItem(13)] public bool NoRide { get; set; }
    [FlatBufferItem(14)] public bool NoFly { get; set; }
    [FlatBufferItem(15)] public bool NoPicnic { get; set; }
    [FlatBufferItem(16)] public bool NoPartner { get; set; }
    [FlatBufferItem(17)] public bool NoSpawnSection { get; set; }
    [FlatBufferItem(18)] public bool NoPokeExchange { get; set; }
    [FlatBufferItem(19)] public bool NoOcclusionCulling { get; set; }
    [FlatBufferItem(20)] public int MinEncLv { get; set; }
    [FlatBufferItem(21)] public int MaxEncLv { get; set; }
    [FlatBufferItem(22)] public EncBiome BaseBiome { get; set; }
    [FlatBufferItem(23)] public AreaTag Tag { get; set; }
    [FlatBufferItem(24)] public OverrideBiome OverrideBiome { get; set; }

    public int ActualMinLevel => MinEncLv != 0 ? MinEncLv : 1;
    public int ActualMaxLevel => MaxEncLv != 0 ? MaxEncLv : 100;
}
