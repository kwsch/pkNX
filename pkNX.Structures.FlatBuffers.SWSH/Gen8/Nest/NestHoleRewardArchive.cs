using System.ComponentModel;
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global
#nullable disable

namespace pkNX.Structures.FlatBuffers.SWSH;

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class NestHoleRewardArchive;

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class NestHoleRewardTable : INestHoleRewardTable
{
    public IList<INestHoleReward> Rewards => (IList<INestHoleReward>)Entries;
}

[TypeConverter(typeof(ExpandableObjectConverter))]
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
