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
    // Trigger tiles?
    [FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
    public class PlacementZone8_F07Holder
    {
        [FlatBufferItem(0)] public PlacementZone8_F07 Field_00 { get; set; }
    }

    [FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
    public class PlacementZone8_F07
    {
        [FlatBufferItem(0)] public PlacementZoneDeepX8 Field_00 { get; set; }
        [FlatBufferItem(1)] public ulong Field_01 { get; set; }

        [FlatBufferItem(3)] public PlacementZoneDeepY8 Field_03 { get; set; }
    }

    [FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
    public class PlacementZoneDeepX8
    {
        [FlatBufferItem(00)] public float Field_00 { get; set; }

        [FlatBufferItem(02)] public float Field_02 { get; set; }

        [FlatBufferItem(06)] public float Field_06 { get; set; }
        [FlatBufferItem(07)] public float Field_07 { get; set; }
        [FlatBufferItem(08)] public float Field_08 { get; set; }
        [FlatBufferItem(09)] public ulong Field_09 { get; set; }
        [FlatBufferItem(10)] public ulong Field_10 { get; set; }
        [FlatBufferItem(11)] public ulong Field_11 { get; set; }
    }

    [FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
    public class PlacementZoneDeepY8
    {
        [FlatBufferItem(00)] public uint Field_00 { get; set; }

        [FlatBufferItem(04)] public float Field_04 { get; set; }

        [FlatBufferItem(06)] public float Field_06 { get; set; }

        [FlatBufferItem(08)] public float Field_08 { get; set; }
        [FlatBufferItem(09)] public float Field_09 { get; set; }
        [FlatBufferItem(10)] public float Field_10 { get; set; }
    }
}
