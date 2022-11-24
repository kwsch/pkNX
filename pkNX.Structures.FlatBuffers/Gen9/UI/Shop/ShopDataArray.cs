using System.ComponentModel;
using FlatSharp.Attributes;
// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedType.Global
#nullable disable

namespace pkNX.Structures.FlatBuffers;

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class ShopDataArray : IFlatBufferArchive<ShopData>
{
    [FlatBufferItem(0)] public ShopData[] Table { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class ShopData
{
    [FlatBufferItem(0)] public string ShopId { get; set; }
    [FlatBufferItem(1)] public ShopKind ShopKind { get; set; }
}
