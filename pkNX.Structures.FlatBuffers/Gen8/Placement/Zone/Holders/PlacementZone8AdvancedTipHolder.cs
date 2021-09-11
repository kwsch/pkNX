using System.ComponentModel;
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
    public class PlacementZone8AdvancedTipHolder
    {
        [FlatBufferItem(00)] public PlacementZone8AdvancedTip Field_00 { get; set; }
        // 1
        // 2
        [FlatBufferItem(03)] public ulong SignHash { get; set; }
    }

    [FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
    public class PlacementZone8AdvancedTip
    {
        [FlatBufferItem(00)] public PlacementZone8_F14 Field_00 { get; set; }
    }

    [FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
    public class PlacementZone8_F14
    {
        [FlatBufferItem(00)] public PlacementZoneMetaTripleXYZ8 Field_00 { get; set; }
        [FlatBufferItem(01)] public string Model { get; set; }
        // 3 empty table
        [FlatBufferItem(03)] public float Field_03 { get; set; }
        [FlatBufferItem(04)] public float Field_04 { get; set; }
        // 5 empty table
        // 6 empty table
        [FlatBufferItem(07)] public float Field_07 { get; set; }
        [FlatBufferItem(08)] public float Field_08 { get; set; }
        [FlatBufferItem(09)] public float Field_09 { get; set; }
        // 10
        [FlatBufferItem(11)] public PlacementZone8_F14_B Field_11 { get; set; }
        // 12
        [FlatBufferItem(13)] public PlacementZone8_F14_B Field_13 { get; set; }
        [FlatBufferItem(14)] public PlacementZone8_F14_Union Field_14 { get; set; }
    }

    [FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
    public class PlacementZone8_F14_B
    {
        [FlatBufferItem(00)] public uint Field_00 { get; set; }
        // 1
        // 2
        // 3
        // 4
        // 5
        [FlatBufferItem(06)] public float Field_06 { get; set; }
        // 7
        [FlatBufferItem(08)] public float Field_08 { get; set; }
        [FlatBufferItem(09)] public float Field_09 { get; set; }
        [FlatBufferItem(10)] public float Field_10 { get; set; }
    }

    // union
    [FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
    public class PlacementZone8_F14_Union
    {
        [FlatBufferItem(00)] public byte Field_00 { get; set; }
        [FlatBufferItem(01)] public PlacementZone8_F14_Sub Field_01 { get; set; }
        // seen: 1={default,float}
    }

    [FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
    public class PlacementZone8_F14_Sub
    {
        // 0 default
        [FlatBufferItem(06)] public float Field_01 { get; set; }
    }
}
