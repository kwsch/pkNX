using System;
using System.ComponentModel;
using FlatSharp.Attributes;
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global

namespace pkNX.Structures.FlatBuffers;

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class PlacementZone8PopupHolder
{
    [FlatBufferItem(00)] public PlacementZone8_F24 Field_00 { get; set; } = new();
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class PlacementZone8_F24
{
    [FlatBufferItem(00)] public PlacementZoneMetaTripleXYZ8 Field_00 { get; set; } = new();
    [FlatBufferItem(01)] public PlacementZone8_F24_IntFloat Field_01 { get; set; } = new();
    [FlatBufferItem(02)] public float Field_02 { get; set; }
    [FlatBufferItem(03)] public float Field_03 { get; set; }
    [FlatBufferItem(04)] public float Field_04 { get; set; }
    [FlatBufferItem(05)] public float Field_05 { get; set; }
    [FlatBufferItem(06)] public ulong Hash_06 { get; set; }
    [FlatBufferItem(07)] public string Field_07 { get; set; } = ""; // none have this
    [FlatBufferItem(08)] public PlacementZone8_F24_Table[] Hash_08 { get; set; } = Array.Empty<PlacementZone8_F24_Table>();
    [FlatBufferItem(09)] public float Field_09 { get; set; }
    [FlatBufferItem(10)] public float Field_10 { get; set; }
    [FlatBufferItem(11)] public float Field_11 { get; set; }
    [FlatBufferItem(12)] public ulong Hash_12 { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class PlacementZone8_F24_Table
{
    [FlatBufferItem(00)] public ulong Hash_00 { get; set; }
    [FlatBufferItem(01)] public ulong Hash_01 { get; set; }
    [FlatBufferItem(02)] public uint Field_02 { get; set; } // multiples of 10, usually +10 from previous entry.
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class PlacementZone8_F24_IntFloat
{
    [FlatBufferItem(00)] public int Field_00 { get; set; }
    [FlatBufferItem(01)] public float Field_01 { get; set; }
    [FlatBufferItem(02)] public float Field_02 { get; set; }
    [FlatBufferItem(03)] public float Field_03 { get; set; }
    [FlatBufferItem(04)] public float Field_04 { get; set; }
}
