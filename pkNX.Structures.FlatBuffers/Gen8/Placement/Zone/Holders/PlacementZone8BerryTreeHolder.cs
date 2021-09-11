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
    public class PlacementZone8BerryTreeHolder
    {
        [FlatBufferItem(00)] public PlacementZone8_F22_0 Field_00 { get; set; } // meta
        [FlatBufferItem(01)] public PlacementZone8BerryTreeRandom[] Field_01 { get; set; }
    }

    [FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
    public class PlacementZone8_F22_0
    {
        [FlatBufferItem(00)] public PlacementZone8_F22_0_0 Field_00 { get; set; }
    }

    // field0 has a hash for Berry Trees
    [FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
    public class PlacementZone8_F22_0_0
    {
        [FlatBufferItem(00)] public PlacementZoneMetaTripleXYZ8 Field_00 { get; set; }
        // 1 empty-table
        // 2 empty-table
        [FlatBufferItem(03)] public float Field_03 { get; set; }
        [FlatBufferItem(04)] public float Field_04 { get; set; }
        // 5 empty-table
        // 6 empty-table
        // 7
        // 8
        // 9
        [FlatBufferItem(11)] public PlacementZone8_F22_Sub Field_11 { get; set; }
        // 12
        [FlatBufferItem(13)] public PlacementZone8_F22_Sub Field_13 { get; set; }
        [FlatBufferItem(14)] public PlacementZone8_F22_Union14 Field_14 { get; set; }
    }

    [FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
    public class PlacementZone8_F22_Sub
    {
        [FlatBufferItem(00)] public int Field_00 { get; set; }
        // 1
        // 2
        // 3
        [FlatBufferItem(04)] public float Field_04 { get; set; }
    }

    [FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
    public class PlacementZone8_F22_Union14
    {
        [FlatBufferItem(0)] public byte Type { get; set; }
        [FlatBufferItem(1)] public uint Object { get; set; } // NOT IMPLEMENTED

        // seen:
        // 1: {default, float}
    }

    [FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
    public class PlacementZone8BerryTreeRandom
    {
        [FlatBufferItem(0)] public ulong Hash { get; set; }
        [FlatBufferItem(1)] public uint Field_01 { get; set; } // rate
        [FlatBufferItem(2)] public uint Field_02 { get; set; }
    }
}
