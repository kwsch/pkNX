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
    // trainer signs?
    [FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
    public class PlacementZone8_F09Holder
    {
        [FlatBufferItem(00)] public PlacementZoneMetaTripleXYZ8 Field_00 { get; set; }
        [FlatBufferItem(01)] public float Field_01 { get; set; }
        [FlatBufferItem(02)] public float Field_02 { get; set; }
        [FlatBufferItem(03)] public float Field_03 { get; set; }
        [FlatBufferItem(04)] public float Field_04 { get; set; }
        [FlatBufferItem(05)] public ulong Field_05 { get; set; }
        [FlatBufferItem(06)] public PlacementZone8_F09 Field_06 { get; set; }
        [FlatBufferItem(07)] public PlacementZone8_F09_Union Field_07 { get; set; }
    }

    [FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
    public class PlacementZone8_F09
    {
        [FlatBufferItem(00)] public uint Field_00 { get; set; }
        [FlatBufferItem(06)] public float Field_06 { get; set; }
    }

    // union?
    [FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
    public class PlacementZone8_F09_Union
    {
        [FlatBufferItem(00)] public byte Field_00 { get; set; }
        [FlatBufferItem(06)] public PlacementZone8_F09_Sub Field_06 { get; set; }
    }

    [FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
    public class PlacementZone8_F09_Sub
    {
        // 0 default
        [FlatBufferItem(1)] public float Field_01 { get; set; }
    }
}
