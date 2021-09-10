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
    public class PlacementZone8_F15Holder
    {
        [FlatBufferItem(00)] public PlacementZoneMetaTripleXYZ8 Field_00 { get; set; }
        [FlatBufferItem(01)] public ulong Hash_01 { get; set; }
        [FlatBufferItem(02)] public uint Field_02 { get; set; }
        [FlatBufferItem(03)] public uint Field_03 { get; set; }
        // 4
        [FlatBufferItem(05)] public PlacementZone8_V3f[] Field_05 { get; set; }
    }

    [FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
    public class PlacementZone8_V3f
    {
        [FlatBufferItem(00)] public float Field_00 { get; set; }
        [FlatBufferItem(01)] public float Field_01 { get; set; }
        [FlatBufferItem(02)] public float Field_02 { get; set; }
    }
}
