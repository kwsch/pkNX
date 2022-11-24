using System;
using System.ComponentModel;
using FlatSharp.Attributes;
// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedType.Global
#nullable disable

namespace pkNX.Structures.FlatBuffers;

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class DeliveryRaidLotteryRewardItemArray : IFlatBufferArchive<DeliveryRaidLotteryRewardItem>
{
    [FlatBufferItem(0)] public DeliveryRaidLotteryRewardItem[] Table { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class DeliveryRaidLotteryRewardItem
{
    [FlatBufferItem(00)] public ulong TableName { get; set; }
    [FlatBufferItem(01)] public RaidLotteryRewardItemInfo RewardItem00 { get; set; }
    [FlatBufferItem(02)] public RaidLotteryRewardItemInfo RewardItem01 { get; set; }
    [FlatBufferItem(03)] public RaidLotteryRewardItemInfo RewardItem02 { get; set; }
    [FlatBufferItem(04)] public RaidLotteryRewardItemInfo RewardItem03 { get; set; }
    [FlatBufferItem(05)] public RaidLotteryRewardItemInfo RewardItem04 { get; set; }
    [FlatBufferItem(06)] public RaidLotteryRewardItemInfo RewardItem05 { get; set; }
    [FlatBufferItem(07)] public RaidLotteryRewardItemInfo RewardItem06 { get; set; }
    [FlatBufferItem(08)] public RaidLotteryRewardItemInfo RewardItem07 { get; set; }
    [FlatBufferItem(09)] public RaidLotteryRewardItemInfo RewardItem08 { get; set; }
    [FlatBufferItem(10)] public RaidLotteryRewardItemInfo RewardItem09 { get; set; }
    [FlatBufferItem(11)] public RaidLotteryRewardItemInfo RewardItem10 { get; set; }
    [FlatBufferItem(12)] public RaidLotteryRewardItemInfo RewardItem11 { get; set; }
    [FlatBufferItem(13)] public RaidLotteryRewardItemInfo RewardItem12 { get; set; }
    [FlatBufferItem(14)] public RaidLotteryRewardItemInfo RewardItem13 { get; set; }
    [FlatBufferItem(15)] public RaidLotteryRewardItemInfo RewardItem14 { get; set; }
    [FlatBufferItem(16)] public RaidLotteryRewardItemInfo RewardItem15 { get; set; }
    [FlatBufferItem(17)] public RaidLotteryRewardItemInfo RewardItem16 { get; set; }
    [FlatBufferItem(18)] public RaidLotteryRewardItemInfo RewardItem17 { get; set; }
    [FlatBufferItem(19)] public RaidLotteryRewardItemInfo RewardItem18 { get; set; }
    [FlatBufferItem(20)] public RaidLotteryRewardItemInfo RewardItem19 { get; set; }
    [FlatBufferItem(21)] public RaidLotteryRewardItemInfo RewardItem20 { get; set; }
    [FlatBufferItem(22)] public RaidLotteryRewardItemInfo RewardItem21 { get; set; }
    [FlatBufferItem(23)] public RaidLotteryRewardItemInfo RewardItem22 { get; set; }
    [FlatBufferItem(24)] public RaidLotteryRewardItemInfo RewardItem23 { get; set; }
    [FlatBufferItem(25)] public RaidLotteryRewardItemInfo RewardItem24 { get; set; }
    [FlatBufferItem(26)] public RaidLotteryRewardItemInfo RewardItem25 { get; set; }
    [FlatBufferItem(27)] public RaidLotteryRewardItemInfo RewardItem26 { get; set; }
    [FlatBufferItem(28)] public RaidLotteryRewardItemInfo RewardItem27 { get; set; }
    [FlatBufferItem(29)] public RaidLotteryRewardItemInfo RewardItem28 { get; set; }
    [FlatBufferItem(30)] public RaidLotteryRewardItemInfo RewardItem29 { get; set; }

    public const int RewardItemCount = 30;

    // Get reward item from index
    public RaidLotteryRewardItemInfo GetRewardItem(int index) => index switch
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
        15 => RewardItem15,
        16 => RewardItem16,
        17 => RewardItem17,
        18 => RewardItem18,
        19 => RewardItem19,
        20 => RewardItem20,
        21 => RewardItem21,
        22 => RewardItem22,
        23 => RewardItem23,
        24 => RewardItem24,
        25 => RewardItem25,
        26 => RewardItem26,
        27 => RewardItem27,
        28 => RewardItem28,
        29 => RewardItem29,
        _ => throw new ArgumentOutOfRangeException(nameof(index), index, null),
    };
}
