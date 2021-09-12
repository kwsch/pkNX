using System.ComponentModel;
using FlatSharp.Attributes;
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global
#nullable disable

namespace pkNX.Structures.FlatBuffers
{
    [FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
    public class PlacementZone8NestHoleHolder
    {
        [FlatBufferItem(00)] public PlacementZone8_F21_A Field_00 { get; set; }
        // 1
        [FlatBufferItem(02)] public int Field_02 { get; set; } // 0,2,6,270,64,12
        [FlatBufferItem(03)] public ulong Common { get; set; }
        [FlatBufferItem(04)] public ulong Rare { get; set; }
        // 5

        [Description("If a flag hash is specified, the savefile value must be true in order for the nest to be enabled.")]
        [FlatBufferItem(06)] public ulong EnableSpawns { get; set; }
    }

    [FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
    public class PlacementZone8_F21_A
    {
        [FlatBufferItem(00)] public PlacementZone8_F21_B Field_00 { get; set; }
    }

    [FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
    public class PlacementZone8_F21_B
    {
        [FlatBufferItem(00)] public PlacementZoneMetaTripleXYZ8 Field_00 { get; set; }
        // 1 empty table
        // 2 empty table
        [FlatBufferItem(03)] public float Field_03 { get; set; }
        [FlatBufferItem(04)] public float Field_04 { get; set; }
        // 5 empty table
        // 6 empty table
        // 7
        // 8
        // 9
        [FlatBufferItem(10)] public float Field_10 { get; set; }
        [FlatBufferItem(11)] public PlacementZone8_F21_IntFloat Field_11 { get; set; }
        // 12
        [FlatBufferItem(13)] public PlacementZone8_F21_IntFloat Field_13 { get; set; }
        [FlatBufferItem(14)] public PlacementZone8_F21_C Field_14 { get; set; }
    }

    [FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
    public class PlacementZone8_F21_IntFloat
    {
        [FlatBufferItem(00)] public int Field_00 { get; set; }
        // 1
        // 2
        // 3
        [FlatBufferItem(04)] public float Field_04 { get; set; }
    }

    // union
    [FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
    public class PlacementZone8_F21_C
    {
        [FlatBufferItem(00)] public byte Field_00 { get; set; }
        [FlatBufferItem(01)] public PlacementZone8_F21_D Field_01 { get; set; }
        // seen: 1={default,float}
    }

    [FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
    public class PlacementZone8_F21_D
    {
        // default
        [FlatBufferItem(01)] public float Field_01 { get; set; }
    }
}
