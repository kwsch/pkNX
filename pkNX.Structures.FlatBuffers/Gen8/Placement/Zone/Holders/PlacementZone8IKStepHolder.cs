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
    // IK_Step
    [FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
    public class PlacementZone8IKStepHolder
    {
        [FlatBufferItem(00)] public PlacementZone8_F25 Field_00 { get; set; }
        [FlatBufferItem(01)] public byte Field_01 { get; set; }
        // 2
        [FlatBufferItem(03)] public byte Field_03 { get; set; }
    }

    [FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
    public class PlacementZone8_F25
    {
        [FlatBufferItem(00)] public PlacementZoneMetaTripleXYZ8 Field_00 { get; set; }
        [FlatBufferItem(01)] public ulong Field_01 { get; set; }
        [FlatBufferItem(02)] public PlacementZone8_F25_X Field_02 { get; set; }
    }

    [FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
    public class PlacementZone8_F25_X
    {
        [FlatBufferItem(00)] public int Field_00 { get; set; }
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
        // 11
    }
}
