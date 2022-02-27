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
public class NewHugeOutbreakGroupLotteryArchive8a : IFlatBufferArchive<NewHugeOutbreakGroupLottery8a>
{
    public byte[] Write() => FlatBufferConverter.SerializeFrom(this);

    [FlatBufferItem(0)] public NewHugeOutbreakGroupLottery8a[] Table { get; set; } = Array.Empty<NewHugeOutbreakGroupLottery8a>();
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class NewHugeOutbreakGroupLottery8a
{
    [FlatBufferItem(00)] public ulong Field_00 { get; set; }
    [FlatBufferItem(01)] public NewHugeOutbreakGroupLotteryDetail8a[] Table1 { get; set; } = Array.Empty<NewHugeOutbreakGroupLotteryDetail8a>();
    [FlatBufferItem(02)] public NewHugeOutbreakGroupLotteryDetail8a[] Table2 { get; set; } = Array.Empty<NewHugeOutbreakGroupLotteryDetail8a>();
    [FlatBufferItem(03)] public NewHugeOutbreakGroupLotteryDetail8a[] Table3 { get; set; } = Array.Empty<NewHugeOutbreakGroupLotteryDetail8a>();
    [FlatBufferItem(04)] public ulong Field_02 { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class NewHugeOutbreakGroupLotteryDetail8a
{
    [FlatBufferItem(00)] public ulong Field_00 { get; set; }
    [FlatBufferItem(01)] public int Field_01 { get; set; }
}
