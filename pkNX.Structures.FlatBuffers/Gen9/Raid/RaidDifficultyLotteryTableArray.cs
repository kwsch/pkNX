using System.ComponentModel;
using FlatSharp.Attributes;
// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedType.Global
#nullable disable

namespace pkNX.Structures.FlatBuffers;

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class RaidDifficultyLotteryTableArray : IFlatBufferArchive<RaidDifficultyLotteryTable>
{
    [FlatBufferItem(0)] public RaidDifficultyLotteryTable[] Table { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class RaidDifficultyLotteryTable
{
    [FlatBufferItem(0)] public RaidLotteryParam Target01 { get; set; }
    [FlatBufferItem(1)] public RaidLotteryParam Target02 { get; set; }
    [FlatBufferItem(2)] public RaidLotteryParam Target03 { get; set; }
    [FlatBufferItem(3)] public RaidLotteryParam Target04 { get; set; }
    [FlatBufferItem(4)] public RaidLotteryParam Target05 { get; set; }
    [FlatBufferItem(5)] public RaidLotteryParam Target06 { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class RaidLotteryParam
{
    [FlatBufferItem(0)] public int Difficulty { get; set; }
    [FlatBufferItem(1)] public int Rate { get; set; }
}
