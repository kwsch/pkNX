using System.ComponentModel;
using FlatSharp.Attributes;
// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedType.Global
#nullable disable

namespace pkNX.Structures.FlatBuffers;

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class HiddenItemDataTableArray : IFlatBufferArchive<HiddenItemDataTable>
{
    [FlatBufferItem(0)] public HiddenItemDataTable[] Table { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class HiddenItemDataTable
{
    [FlatBufferItem(00)] public string TableId { get; set; }
    [FlatBufferItem(01)] public HiddenItemDataTableInfo Item1 { get; set; }
    [FlatBufferItem(02)] public HiddenItemDataTableInfo Item2 { get; set; }
    [FlatBufferItem(03)] public HiddenItemDataTableInfo Item3 { get; set; }
    [FlatBufferItem(04)] public HiddenItemDataTableInfo Item4 { get; set; }
    [FlatBufferItem(05)] public HiddenItemDataTableInfo Item5 { get; set; }
    [FlatBufferItem(06)] public HiddenItemDataTableInfo Item6 { get; set; }
    [FlatBufferItem(07)] public HiddenItemDataTableInfo Item7 { get; set; }
    [FlatBufferItem(08)] public HiddenItemDataTableInfo Item8 { get; set; }
    [FlatBufferItem(09)] public HiddenItemDataTableInfo Item9 { get; set; }
    [FlatBufferItem(10)] public HiddenItemDataTableInfo Item10 { get; set; }
}


[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class HiddenItemDataTableInfo
{
    [FlatBufferItem(0)] public ItemID ItemId { get; set; }
    [FlatBufferItem(1)] public int EmergePercent { get; set; }
    [FlatBufferItem(2)] public int DropCount { get; set; }
}
