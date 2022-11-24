using System.ComponentModel;
using FlatSharp.Attributes;
// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedType.Global
#nullable disable

namespace pkNX.Structures.FlatBuffers;

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class GemSymbolLotteryTableArray : IFlatBufferArchive<GemSymbolLotteryTable>
{
    [FlatBufferItem(0)] public GemSymbolLotteryTable[] Table { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class GemSymbolLotteryTable
{
    [FlatBufferItem(00)] public string LotteryKey { get; set; }
    [FlatBufferItem(01)] public MoveType GemMoveType { get; set; }
    [FlatBufferItem(02)] public string TableKey0 { get; set; }
    [FlatBufferItem(03)] public string TableKey1 { get; set; }
    [FlatBufferItem(04)] public string TableKey2 { get; set; }
    [FlatBufferItem(05)] public string TableKey3 { get; set; }
    [FlatBufferItem(06)] public string TableKey4 { get; set; }
    [FlatBufferItem(07)] public string TableKey5 { get; set; }
    [FlatBufferItem(08)] public string TableKey6 { get; set; }
    [FlatBufferItem(09)] public string TableKey7 { get; set; }
    [FlatBufferItem(10)] public string TableKey8 { get; set; }
    [FlatBufferItem(11)] public string TableKey9 { get; set; }
}
