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
    public class PlacementZone8TrainerHolder
    {
        [FlatBufferItem(00)] public PlacementZone8_F08 Field_00 { get; set; }
        [FlatBufferItem(01)] public float Field_01 { get; set; }
        [FlatBufferItem(02)] public ulong TrainerID { get; set; }
        [FlatBufferItem(03)] public ulong Hash_03 { get; set; }
        [FlatBufferItem(04)] public ulong MovementPath { get; set; }
        // 5 empty-table
        [FlatBufferItem(06)] public uint Field_06 { get; set; }
        [FlatBufferItem(07)] public PlacementZone8_F08_Nine Field_07 { get; set; }
        // 8
        // 9
        [FlatBufferItem(10)] public uint Field_10 { get; set; }
        [FlatBufferItem(11)] public uint Field_11 { get; set; }
        [FlatBufferItem(12)] public uint Field_12 { get; set; }
    }

    [FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
    public class PlacementZone8_F08_Nine
    {
        [FlatBufferItem(0)] public byte Field_00 { get; set; }
        [FlatBufferItem(1)] public byte Field_01 { get; set; }
        [FlatBufferItem(2)] public byte Field_02 { get; set; }
        // 3
        [FlatBufferItem(4)] public ulong Hash_04 { get; set; }
        [FlatBufferItem(5)] public byte Field_05 { get; set; }
        // 6
        [FlatBufferItem(7)] public ulong Hash_07 { get; set; }
        // 8? is this not the same as others?
        // 9?
    }

    [FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
    public class PlacementZone8_F08
    {
        [FlatBufferItem(0)] public PlacementZoneMetaTripleXYZ8 Field_00 { get; set; }
        [FlatBufferItem(1)] public ulong Hash_01 { get; set; }
        [FlatBufferItem(2)] public ulong Hash_02 { get; set; }
        [FlatBufferItem(3)] public ulong Hash_03 { get; set; }
        [FlatBufferItem(4)] public PlacementZone8_F08_IntFloat Field_04 { get; set; }
        // 5
        [FlatBufferItem(6)] public ulong Hash_06 { get; set; }
        [FlatBufferItem(7)] public PlacementZone8_F08_IntFloat Field_07 { get; set; }
    }

    [FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
    public class PlacementZone8_F08_IntFloat
    {
        [FlatBufferItem(0)] public int Field_00 { get; set; }
        // 1
        // 2
        // 3
        [FlatBufferItem(4)] public float Field_04 { get; set; }
    }
}
