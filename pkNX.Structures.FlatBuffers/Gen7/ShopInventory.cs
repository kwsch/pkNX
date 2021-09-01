﻿using System.ComponentModel;
using System.Linq;
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
    [FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
    public class ShopInventory
    {
        [FlatBufferItem(0)] public Shop1[] Shop1 { get; set; }
        [FlatBufferItem(1)] public Shop2[] Shop2 { get; set; }
    }

    [FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
    public class Shop1
    {
        [FlatBufferItem(0)] public ulong Hash { get; set; }
        [FlatBufferItem(1)] public Inventory Inventory { get; set; }

        public override string ToString() => $"{Hash:X16} - {Inventory}";
    }

    [FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
    public class Shop2
    {
        [FlatBufferItem(0)] public ulong Hash { get; set; }
        [FlatBufferItem(1)] public Inventory[] Inventories { get; set; }

        public override string ToString() => $"{Hash:X16} - {string.Join(", ", Inventories.Select(z => z.ToString()))}";
    }

    [FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
    public class Inventory
    {
        [FlatBufferItem(0)] public int[] Items { get; set; }

        public override string ToString() => $"{string.Join(",", Items.Select(z => z.ToString()))}";
    }
}
