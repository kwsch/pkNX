using System.ComponentModel;
using FlatSharp.Attributes;
// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedType.Global
#nullable disable

namespace pkNX.Structures.FlatBuffers;

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class FriendlyShopDataArray : IFlatBufferArchive<FriendlyShopData>
{
    [FlatBufferItem(0)] public FriendlyShopData[] Table { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class FriendlyShopData
{
    [FlatBufferItem(0)] public string Id { get; set; }
    [FlatBufferItem(1)] public string ShopName { get; set; }
    [FlatBufferItem(2)] public string ShopSubName { get; set; }
    [FlatBufferItem(3)] public SellType SellType { get; set; }
    [FlatBufferItem(4)] public PayType PayKind { get; set; }
    [FlatBufferItem(5)] public string Lineup { get; set; }
    [FlatBufferItem(6)] public ClerkType Clerktype { get; set; }
}
