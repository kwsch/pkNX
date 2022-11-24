using System.ComponentModel;
using FlatSharp.Attributes;
// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedType.Global
#nullable disable

namespace pkNX.Structures.FlatBuffers;

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class RaidGemSetting
{
    [FlatBufferItem(0)] public MeteorSetting Meteor { get; set; }
    [FlatBufferItem(1)] public GemSetting Gem { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class MeteorSetting
{
    [FlatBufferItem(0)] public int ShowerInterval { get; set; }
    [FlatBufferItem(1)] public int QuantityMin { get; set; }
    [FlatBufferItem(2)] public int QuantityMax { get; set; }
    [FlatBufferItem(3)] public float DelayMin { get; set; }
    [FlatBufferItem(4)] public float DelayMax { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class GemSetting
{
    [FlatBufferItem(00)] public int MaxNumUnexplored { get; set; }
    [FlatBufferItem(01)] public int MaxNumExplored { get; set; }
    [FlatBufferItem(02)] public int MaxNumAnAreaUnexplored { get; set; }
    [FlatBufferItem(03)] public int MaxNumAnAreaExplored { get; set; }
    [FlatBufferItem(04)] public int AreaMax { get; set; }
    [FlatBufferItem(05)] public int GroupMax { get; set; }
    [FlatBufferItem(06)] public int GemMax { get; set; }
    [FlatBufferItem(07)] public int LotteryGroupMax { get; set; }
    [FlatBufferItem(08)] public int LotteryKwmMax { get; set; }
    [FlatBufferItem(09)] public float GemFetchLength { get; set; }
    [FlatBufferItem(10)] public float GemReleaseLength { get; set; }
    [FlatBufferItem(11)] public int LeaguePayDifficulty01 { get; set; }
    [FlatBufferItem(12)] public int LeaguePayDifficulty02 { get; set; }
    [FlatBufferItem(13)] public int LeaguePayDifficulty03 { get; set; }
    [FlatBufferItem(14)] public int LeaguePayDifficulty04 { get; set; }
    [FlatBufferItem(15)] public int LeaguePayDifficulty05 { get; set; }
    [FlatBufferItem(16)] public int LeaguePayDifficulty06 { get; set; }
    [FlatBufferItem(17)] public int LeaguePayDifficulty07 { get; set; }
}
