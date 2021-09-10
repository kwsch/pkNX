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
    public class PlacementZone8_F24Holder
    {
        [FlatBufferItem(00)] public PlacementZone8_F24 Field_00 { get; set; }
    }

    [FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
    public class PlacementZone8_F24
    {
        [FlatBufferItem(00)] public PlacementZoneMetaTripleXYZ8 Field_00 { get; set; }
        [FlatBufferItem(01)] public PlacementZone8_F24_IntFloat Field_01 { get; set; }
        [FlatBufferItem(02)] public float Field_02 { get; set; }
        // 3
        // 4
        // 5
        [FlatBufferItem(06)] public ulong Hash_06 { get; set; }
        // 7 empty table?
        [FlatBufferItem(08)] public PlacementZone8_F24_Table[] Hash_08 { get; set; }
        // 9
        [FlatBufferItem(10)] public float Field_10 { get; set; }
        // 11
        [FlatBufferItem(12)] public ulong Hash_12 { get; set; }
    }

    [FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
    public class PlacementZone8_F24_Table
    {
        [FlatBufferItem(00)] public ulong Hash_00 { get; set; }
        [FlatBufferItem(01)] public ulong Hash_01 { get; set; }
    }

    [FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
    public class PlacementZone8_F24_IntFloat
    {
        [FlatBufferItem(00)] public int Field_00 { get; set; }
        [FlatBufferItem(01)] public float Field_01 { get; set; }
        // 2
        // 3
        [FlatBufferItem(04)] public float Field_04 { get; set; }
    }
}
