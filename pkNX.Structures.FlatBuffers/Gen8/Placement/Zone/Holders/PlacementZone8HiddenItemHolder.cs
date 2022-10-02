using System;
using System.ComponentModel;
using FlatSharp.Attributes;
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global
#pragma warning disable CA1819 // Properties should not return arrays

namespace pkNX.Structures.FlatBuffers;

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class PlacementZone8HiddenItemHolder
{
    [FlatBufferItem(00)] public PlacementZone8HiddenItem Field_00 { get; set; } = new();
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class PlacementZone8HiddenItem
{
    [FlatBufferItem(00)] public PlacementZoneMetaTripleXYZ8 Field_00 { get; set; } = new();
    [FlatBufferItem(01)] public PlacementZone8HiddenItemValue Field_01 { get; set; } = new();
    [FlatBufferItem(02)] public PlacementZone8HiddenItemChance[] Field_02 { get; set; } = Array.Empty<PlacementZone8HiddenItemChance>();
    [FlatBufferItem(03)] public int Field_03 { get; set; }
    [FlatBufferItem(04)] public uint Field_04 { get; set; }
    [FlatBufferItem(05)] public float Field_05 { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class PlacementZone8HiddenItemValue
{
    [FlatBufferItem(00)] public int Field_00 { get; set; }
    [FlatBufferItem(01)] public float Field_01 { get; set; } // unused
    [FlatBufferItem(02)] public float Field_02 { get; set; } // unused
    [FlatBufferItem(03)] public float Field_03 { get; set; } // unused
    [FlatBufferItem(04)] public float Field_04 { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class PlacementZone8HiddenItemChance
{
    [FlatBufferItem(00)] public ulong Hash { get; set; }
    [FlatBufferItem(01)] public int Chance { get; set; }
    [FlatBufferItem(02)] public int Quantity { get; set; }
}
