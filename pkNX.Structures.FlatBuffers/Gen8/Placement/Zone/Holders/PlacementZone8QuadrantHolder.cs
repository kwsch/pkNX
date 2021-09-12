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
    // highplace?
    [FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
    public class PlacementZone8QuadrantHolder
    {
        [FlatBufferItem(00)] public PlacementZone8_F17 Field_00 { get; set; }
    }

    [FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
    public class PlacementZone8_F17
    {
        [FlatBufferItem(00)] public PlacementZoneMetaTripleXYZ8 Field_00 { get; set; }
        [FlatBufferItem(01)] public ulong Hash_01 { get; set; }
        [FlatBufferItem(02)] public PlacementZone8_F17_Sub Field_02 { get; set; }
    }

    [FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
    public class PlacementZone8_F17_Sub
    {
        [FlatBufferItem(00)] public uint Field_00 { get; set; }
        // 1
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
