using System.ComponentModel;
using FlatSharp.Attributes;
// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedType.Global
#nullable disable

namespace pkNX.Structures.FlatBuffers;

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class RestaurantShopDataArray : IFlatBufferArchive<RestaurantShopData>
{
    [FlatBufferItem(0)] public RestaurantShopData[] Table { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class RestaurantShopData
{
    [FlatBufferItem(0)] public string Shopid { get; set; }
    [FlatBufferItem(1)] public string Shopname { get; set; }
    [FlatBufferItem(2)] public string Shopsubname { get; set; }
    [FlatBufferItem(3)] public PayType Paykind { get; set; }
    [FlatBufferItem(4)] public string Menuid { get; set; }
}
