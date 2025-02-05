namespace pkNX.Structures.FlatBuffers.SWSH;

public partial class NestHoleDistributionRewardTable : INestHoleRewardTable
{
    public IList<INestHoleReward> Rewards => (IList<INestHoleReward>)Entries;
}

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
