using System.ComponentModel;
using FlatSharp.Attributes;
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global
#nullable disable
#pragma warning disable CA1819 // Properties should not return arrays

namespace pkNX.Structures.FlatBuffers;

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class NestHoleReward8Archive : IFlatBufferArchive<NestHoleReward8Table>
{
    [FlatBufferItem(0)] public NestHoleReward8Table[] Table { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class NestHoleReward8Table : INestHoleRewardTable
{
    [FlatBufferItem(0)] public ulong TableID { get; set; }
    [FlatBufferItem(1)] public NestHoleReward8[] Entries { get; set; }

    [Browsable(false)]
    public INestHoleReward[] Rewards => Entries;
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class NestHoleReward8 : INestHoleReward
{
    [FlatBufferItem(0)] public uint EntryID { get; set; }
    [FlatBufferItem(1)] public uint ItemID { get; set; }
    [FlatBufferItem(2)] public uint[] Values { get; set; }

    public uint Item => ItemID;

    public override string ToString() => $"{EntryID:0} - {ItemID:0000}";
}

public interface INestHoleReward
{
    uint Item { get; }
    uint[] Values { get; set; }
}

public interface INestHoleRewardTable
{
    ulong TableID { get; set; }
    INestHoleReward[] Rewards { get; }
}
