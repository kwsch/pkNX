using System;
using System.ComponentModel;
using FlatSharp.Attributes;
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global
#pragma warning disable CA1819 // Properties should not return arrays

namespace pkNX.Structures.FlatBuffers
{
    [FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
    public class PlacementZone8FieldItemHolder
    {
        [FlatBufferItem(00)] public PlacementZone8FieldItem Field_00 { get; set; } = new();
    }

    [FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
    public class PlacementZone8FieldItem
    {
        [FlatBufferItem(00)] public PlacementZoneMetaTripleXYZ8 Field_00 { get; set; } = new();
        [FlatBufferItem(01)] public ulong Hash_01 { get; set; }
        [FlatBufferItem(02)] public string Field_02 { get; set; } = "";
        [FlatBufferItem(03)] public float Field_03 { get; set; }
        [FlatBufferItem(04)] public float Field_04 { get; set; }
        [FlatBufferItem(05)] public ulong Hash_05 { get; set; }
        [FlatBufferItem(06)] public ulong[] Flags { get; set; } = Array.Empty<ulong>();
        [FlatBufferItem(07)] public uint[] Items { get; set; } = Array.Empty<uint>();
        [FlatBufferItem(08)] public byte Quantity { get; set; }
        [FlatBufferItem(09)] public PlacementZone8FieldItem_A Field_09 { get; set; } = new();
    }

    [FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
    public class PlacementZone8FieldItem_A
    {
        [FlatBufferItem(00)] public bool Field_00 { get; set; }
        [FlatBufferItem(01)] public FlatDummyObject Field_01 { get; set; } = new();
    }
}
