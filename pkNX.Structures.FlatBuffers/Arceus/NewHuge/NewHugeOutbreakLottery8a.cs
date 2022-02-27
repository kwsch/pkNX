using System;
using System.ComponentModel;
using FlatSharp.Attributes;

// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global

namespace pkNX.Structures.FlatBuffers;

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class NewHugeOutbreakLotteryArchive8a : IFlatBufferArchive<NewHugeOutbreakLottery8a>
{
    public byte[] Write() => FlatBufferConverter.SerializeFrom(this);

    [FlatBufferItem(0)] public NewHugeOutbreakLottery8a[] Table { get; set; } = Array.Empty<NewHugeOutbreakLottery8a>();
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class NewHugeOutbreakLottery8a
{
    [FlatBufferItem(00)] public ulong Hash { get; set; }
    [FlatBufferItem(01)] public string Field_01 { get; set; } = string.Empty;
    [FlatBufferItem(02)] public int OutbreakChance { get; set; }
    [FlatBufferItem(03)] public int OutbreakTotalMin { get; set; }
    [FlatBufferItem(04)] public int OutbreakTotalMax { get; set; }
    [FlatBufferItem(05)] public int Field_05 { get; set; }
    [FlatBufferItem(06)] public int Field_06 { get; set; }
    [FlatBufferItem(07)] public int MinCountFirst { get; set; }
    [FlatBufferItem(08)] public int MaxCountFirst { get; set; }
    [FlatBufferItem(09)] public int MinCountSecond { get; set; }
    [FlatBufferItem(10)] public int MaxCountSecond { get; set; }
    [FlatBufferItem(11)] public int Field_11 { get; set; }
    [FlatBufferItem(12)] public int Field_12 { get; set; }
    [FlatBufferItem(13)] public int BerryChance { get; set; }
    [FlatBufferItem(14)] public int RollBonus { get; set; }
    [FlatBufferItem(15)] public string[] Field_15 { get; set; } = Array.Empty<string>();
    [FlatBufferItem(16)] public int[] Field_16 { get; set; } = Array.Empty<int>();
    [FlatBufferItem(17)] public int[] StarChanceParams { get; set; } = Array.Empty<int>();

    // Stars are checked before berries
    public int StarMin => StarChanceParams[0];
    public int StarMax => StarChanceParams[1];
}
