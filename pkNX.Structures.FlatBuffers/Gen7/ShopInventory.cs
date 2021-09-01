using FlatSharp.Attributes;
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global
#nullable disable
#pragma warning disable CA1819 // Properties should not return arrays

namespace pkNX.Structures.FlatBuffers
{
    [FlatBufferTable]
    public class ShopInventory
    {
        [FlatBufferItem(0)] public Shop1[] Shop1 { get; set; }
        [FlatBufferItem(1)] public Shop2[] Shop2 { get; set; }
    }

    [FlatBufferTable]
    public class Shop1
    {
        [FlatBufferItem(0)] public ulong Hash { get; set; }
        [FlatBufferItem(1)] public Inventory Inventory { get; set; }
    }

    [FlatBufferTable]
    public class Shop2
    {
        [FlatBufferItem(0)] public ulong Hash { get; set; }
        [FlatBufferItem(1)] public Inventory[] Inventories { get; set; }
    }

    [FlatBufferTable]
    public class Inventory
    {
        [FlatBufferItem(0)] public int[] Items { get; set; }
    }
}
