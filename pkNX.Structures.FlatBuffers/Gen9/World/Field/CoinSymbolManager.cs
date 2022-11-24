using System.ComponentModel;
using FlatSharp.Attributes;
// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedType.Global
#nullable disable

namespace pkNX.Structures.FlatBuffers;

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class CoinSymbolManager
{
    [FlatBufferItem(0)] public float WalkFetchRange { get; set; }
    [FlatBufferItem(1)] public float WalkReleaseRange { get; set; }
    [FlatBufferItem(2)] public float BoxFetchRange { get; set; }
    [FlatBufferItem(3)] public float BoxReleaseRange { get; set; }
    [FlatBufferItem(4)] public BoxRevival BoxRevival { get; set; }
    [FlatBufferItem(5)] public WalkRevival WalkRevival { get; set; }
    [FlatBufferItem(6)] public float WalkVoiceLength { get; set; }
    [FlatBufferItem(7)] public int WalkVoiceIntervalMin { get; set; }
    [FlatBufferItem(8)] public int WalkVoiceIntervalMax { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class BoxRevival
{
    [FlatBufferItem(0)] public int RevivalRate { get; set; }
    [FlatBufferItem(1)] public CoinQuantityData QuantityData { get; set; }
    [FlatBufferItem(2)] public CoinNumData NumData { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class WalkRevival
{
    [FlatBufferItem(0)] public int RevivalRate { get; set; }
    [FlatBufferItem(1)] public CoinQuantityData QuantityData { get; set; }
    [FlatBufferItem(2)] public CoinNumData NumData { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class CoinQuantityData
{
    [FlatBufferItem(0)] public CoinRevivalLotteryData Lottery00 { get; set; }
    [FlatBufferItem(1)] public CoinRevivalLotteryData Lottery01 { get; set; }
    [FlatBufferItem(2)] public CoinRevivalLotteryData Lottery02 { get; set; }
    [FlatBufferItem(3)] public CoinRevivalLotteryData Lottery03 { get; set; }
    [FlatBufferItem(4)] public CoinRevivalLotteryData Lottery04 { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class CoinNumData
{
    [FlatBufferItem(0)] public CoinRevivalLotteryData Lottery00 { get; set; }
    [FlatBufferItem(1)] public CoinRevivalLotteryData Lottery01 { get; set; }
    [FlatBufferItem(2)] public CoinRevivalLotteryData Lottery02 { get; set; }
    [FlatBufferItem(3)] public CoinRevivalLotteryData Lottery03 { get; set; }
    [FlatBufferItem(4)] public CoinRevivalLotteryData Lottery04 { get; set; }
    [FlatBufferItem(5)] public CoinRevivalLotteryData Lottery05 { get; set; }
    [FlatBufferItem(6)] public CoinRevivalLotteryData Lottery06 { get; set; }
    [FlatBufferItem(7)] public CoinRevivalLotteryData Lottery07 { get; set; }
    [FlatBufferItem(8)] public CoinRevivalLotteryData Lottery08 { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class CoinRevivalLotteryData
{
    [FlatBufferItem(0)] public int Rate { get; set; }
    [FlatBufferItem(1)] public int Num { get; set; }
}
