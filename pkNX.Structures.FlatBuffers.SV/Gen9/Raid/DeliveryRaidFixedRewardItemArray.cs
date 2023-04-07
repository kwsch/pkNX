using System;
using System.ComponentModel;
// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedType.Global

namespace pkNX.Structures.FlatBuffers.SV;

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class DeliveryRaidFixedRewardItemArray { }

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class DeliveryRaidFixedRewardItem
{
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
        _ => throw new ArgumentOutOfRangeException(nameof(index), index, null),
    };
}
