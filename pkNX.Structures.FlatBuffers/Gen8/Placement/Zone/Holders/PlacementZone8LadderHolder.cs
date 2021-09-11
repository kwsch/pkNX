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
    public class PlacementZone8LadderHolder
    {
        [FlatBufferItem(00)] public PlacementZone8Ladder Field_00 { get; set; }
    }

    [FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
    public class PlacementZone8Ladder
    {
        [FlatBufferItem(0)] public PlacementZoneMetaTripleXYZ8 Field_00 { get; set; }
        [FlatBufferItem(1)] public PlacementZone8_F23_Sub Field_01 { get; set; }
        // 2
        [FlatBufferItem(3)] public int Field_03 { get; set; }
    }

    [FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
    public class PlacementZone8_F23_Sub
    {
        [FlatBufferItem(00)] public int Field_00 { get; set; }
        // 1
        // 2
        // 3
        [FlatBufferItem(04)] public float Field_04 { get; set; }
    }
}
