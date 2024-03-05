using System.ComponentModel;
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global

namespace pkNX.Structures.FlatBuffers.SWSH;

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class NestHoleDistributionRewardArchive;

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class NestHoleDistributionRewardTable : INestHoleRewardTable
{
    public IList<INestHoleReward> Rewards => (IList<INestHoleReward>)Entries;
}

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class NestHoleDistributionReward : INestHoleReward
{
    public uint Item => ItemID;

    public IList<uint> Values
    {
        get => [Value0, Value1, Value2, Value3, Value4];
        set
        {
            Value0 = (byte)value[0];
            Value1 = (byte)value[1];
            Value2 = (byte)value[2];
            Value3 = (byte)value[3];
            Value4 = (byte)value[4];
        }
    }
}
