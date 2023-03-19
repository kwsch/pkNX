using System;
using System.ComponentModel;
// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedType.Global

namespace pkNX.Structures.FlatBuffers.SV;

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class DeliveryRaidLotteryRewardItem
{
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
        _ => throw new ArgumentOutOfRangeException(nameof(index), index, null)
    };
}
