using System.ComponentModel;
using FlatSharp.Attributes;
// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedType.Global
#nullable disable

namespace pkNX.Structures.FlatBuffers.EventSub012;

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class RewardTableArray : IFlatBufferArchive<RewardTable>
{
    [FlatBufferItem(0)] public RewardTable[] Table { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class RewardTable
{
    [FlatBufferItem(0)] public ItemID ItemId { get; set; }
    [FlatBufferItem(1)] public int ItemNum { get; set; }
    [FlatBufferItem(2)] public int LotteryWeight { get; set; }
}
