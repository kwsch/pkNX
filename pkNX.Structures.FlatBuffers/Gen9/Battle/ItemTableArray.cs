using System.ComponentModel;
using FlatSharp.Attributes;
// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedType.Global
#nullable disable

namespace pkNX.Structures.FlatBuffers;

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class ItemTableArray : IFlatBufferArchive<ItemTable>
{
    [FlatBufferItem(0)] public ItemTable[] Table { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class ItemTable
{
    [FlatBufferItem(0)] public int ItemID { get; set; }
    [FlatBufferItem(1)] public int PlibID { get; set; }
}
