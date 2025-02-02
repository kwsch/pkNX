namespace pkNX.Structures.FlatBuffers.SWSH;

public partial class NestHoleRewardTable : INestHoleRewardTable
{
    public IList<INestHoleReward> Rewards => (IList<INestHoleReward>)Entries;
}

public partial class NestHoleReward : INestHoleReward
{
    public uint Item => ItemID;
    public override string ToString() => $"{EntryID:0} - {ItemID:0000}";
}

public interface INestHoleReward
{
    uint Item { get; }
    IList<uint> Values { get; }
}

public interface INestHoleRewardTable
{
    ulong TableID { get; set; }
    IList<INestHoleReward> Rewards { get; }
}
