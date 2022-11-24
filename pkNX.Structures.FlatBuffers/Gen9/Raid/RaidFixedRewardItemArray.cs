using System;
using System.ComponentModel;
using FlatSharp.Attributes;
// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedType.Global
#nullable disable

namespace pkNX.Structures.FlatBuffers;

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class RaidFixedRewardItemArray : IFlatBufferArchive<RaidFixedRewardItem>
{
    [FlatBufferItem(0)] public RaidFixedRewardItem[] Table { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class RaidFixedRewardItem
{
    [FlatBufferItem(00)] public ulong TableName { get; set; }
    [FlatBufferItem(01)] public RaidFixedRewardItemInfo RewardItem00 { get; set; }
    [FlatBufferItem(02)] public RaidFixedRewardItemInfo RewardItem01 { get; set; }
    [FlatBufferItem(03)] public RaidFixedRewardItemInfo RewardItem02 { get; set; }
    [FlatBufferItem(04)] public RaidFixedRewardItemInfo RewardItem03 { get; set; }
    [FlatBufferItem(05)] public RaidFixedRewardItemInfo RewardItem04 { get; set; }
    [FlatBufferItem(06)] public RaidFixedRewardItemInfo RewardItem05 { get; set; }
    [FlatBufferItem(07)] public RaidFixedRewardItemInfo RewardItem06 { get; set; }
    [FlatBufferItem(08)] public RaidFixedRewardItemInfo RewardItem07 { get; set; }
    [FlatBufferItem(09)] public RaidFixedRewardItemInfo RewardItem08 { get; set; }
    [FlatBufferItem(10)] public RaidFixedRewardItemInfo RewardItem09 { get; set; }
    [FlatBufferItem(11)] public RaidFixedRewardItemInfo RewardItem10 { get; set; }
    [FlatBufferItem(12)] public RaidFixedRewardItemInfo RewardItem11 { get; set; }
    [FlatBufferItem(13)] public RaidFixedRewardItemInfo RewardItem12 { get; set; }
    [FlatBufferItem(14)] public RaidFixedRewardItemInfo RewardItem13 { get; set; }
    [FlatBufferItem(15)] public RaidFixedRewardItemInfo RewardItem14 { get; set; }

    public const int Count = 15;

    public RaidFixedRewardItemInfo GetReward(int index) => index switch
    {
        00 => RewardItem00,
        01 => RewardItem01,
        02 => RewardItem02,
        03 => RewardItem03,
        04 => RewardItem04,
        05 => RewardItem05,
        06 => RewardItem06,
        07 => RewardItem07,
        08 => RewardItem08,
        09 => RewardItem09,
        10 => RewardItem10,
        11 => RewardItem11,
        12 => RewardItem12,
        13 => RewardItem13,
        14 => RewardItem14,
        _ => throw new ArgumentOutOfRangeException(nameof(index))
    };
}
