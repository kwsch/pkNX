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
    public class PlacementZone8WarpHolder
    {
        [FlatBufferItem(00)] public PlacementZoneWarp8 Field_00 { get; set; }
    }

    [FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
    public class PlacementZoneWarp8
    {
        [FlatBufferItem(00)] public PlacementZoneMetaTripleXYZ8 Field_00 { get; set; }
        [FlatBufferItem(01)] public ulong Hash_01 { get; set; }
        [FlatBufferItem(02)] public string OtherAreaName { get; set; }
        [FlatBufferItem(03)] public string ModelName { get; set; }
        // field 4 is pointing to an empty table?
        [FlatBufferItem(05)] public int Field_05 { get; set; }
        [FlatBufferItem(06)] public float Field_06 { get; set; }
        // field 7 is default
        [FlatBufferItem(08)] public ulong Hash_08 { get; set; }
        [FlatBufferItem(09)] public PlacementZoneWarpDetails8 SubMeta { get; set; }
        [FlatBufferItem(10)] public string SoundEffect1 { get; set; }
        [FlatBufferItem(11)] public string SoundEffect2 { get; set; }
    }

    [FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
    public class PlacementZoneWarpDetails8
    {
        [FlatBufferItem(00)] public int Field_00 { get; set; }
        [FlatBufferItem(01)] public float Field_01 { get; set; }
        // 2
        // 3
        // 4
        // 5
        // 6
        // 7
        [FlatBufferItem(08)] public float Field_08 { get; set; }
        [FlatBufferItem(09)] public float Field_09 { get; set; }
        [FlatBufferItem(10)] public float Field_10 { get; set; }
    }
}
