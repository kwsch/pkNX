using System.ComponentModel;
using FlatSharp.Attributes;
// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedType.Global
#nullable disable

namespace pkNX.Structures.FlatBuffers;

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class RaidRewardSlotArray : IFlatBufferArchive<RaidRewardSlot>
{
    [FlatBufferItem(0)] public RaidRewardSlot[] Table { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class RaidRewardSlot
{
    [FlatBufferItem(0)] public RaidRewardSlotInfo Slot01 { get; set; }
    [FlatBufferItem(1)] public RaidRewardSlotInfo Slot02 { get; set; }
    [FlatBufferItem(2)] public RaidRewardSlotInfo Slot03 { get; set; }
    [FlatBufferItem(3)] public RaidRewardSlotInfo Slot04 { get; set; }
    [FlatBufferItem(4)] public RaidRewardSlotInfo Slot05 { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class RaidRewardSlotInfo
{
    [FlatBufferItem(0)] public int SlotNum { get; set; }
    [FlatBufferItem(1)] public int Rate { get; set; }
}
