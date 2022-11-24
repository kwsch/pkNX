using System.ComponentModel;
using FlatSharp.Attributes;
// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedType.Global
#nullable disable

namespace pkNX.Structures.FlatBuffers;

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class DropItemDataArray : IFlatBufferArchive<DropItemData>
{
    [FlatBufferItem(0)] public DropItemData[] Table { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class DropItemData
{
    [FlatBufferItem(0)] public DevID DevId { get; set; }
    [FlatBufferItem(1)] public OneItemData Item1 { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class OneItemData
{
    [FlatBufferItem(0)] public ItemID ItemId { get; set; }
    [FlatBufferItem(1)] public sbyte Rate { get; set; }
    [FlatBufferItem(2)] public sbyte Num { get; set; }
}
