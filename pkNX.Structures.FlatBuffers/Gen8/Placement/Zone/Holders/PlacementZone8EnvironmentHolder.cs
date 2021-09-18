using System;
using System.ComponentModel;
using FlatSharp.Attributes;
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global
#pragma warning disable CA1819 // Properties should not return arrays

namespace pkNX.Structures.FlatBuffers
{
    [FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
    public class PlacementZone8EnvironmentHolder
    {
        [FlatBufferItem(00)] public PlacementZone8_F10 Field_00 { get; set; } = new();
    }

    [FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
    public class PlacementZone8_F10
    {
        [FlatBufferItem(00)] public PlacementZoneMetaTripleXYZ8 Field_00 { get; set; } = new();
        [FlatBufferItem(01)] public PlacementZone8_V3f[] Field_01 { get; set; } = Array.Empty<PlacementZone8_V3f>();
        [FlatBufferItem(02)] public string PlayName { get; set; } = "";
        [FlatBufferItem(03)] public string StopName { get; set; } = "";
        [FlatBufferItem(04)] public float Field_04 { get; set; }
        [FlatBufferItem(05)] public int Field_05 { get; set; }
        [FlatBufferItem(06)] public int Field_06 { get; set; }
    }
}
