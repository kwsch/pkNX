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
    public class PlacementZone8SpeciesHolder
    {
        [FlatBufferItem(00)] public PlacementZone8_F02 Field_00 { get; set; }
        [FlatBufferItem(01)] public PlacementZone8_F02_Field1 Field_01 { get; set; }
        [FlatBufferItem(02)] public uint Num_02 { get; set; } // Species?
        // 3
        // 4
        // 5
        // 6
        [FlatBufferItem(07)] public ulong Hash_07 { get; set; }
        [FlatBufferItem(08)] public ulong Hash_08 { get; set; }
        [FlatBufferItem(09)] public ulong Hash_09 { get; set; }
        // 10 empty-table
        [FlatBufferItem(11)] public float Field_11 { get; set; }
        [FlatBufferItem(12)] public PlacementZone8_F02_Nine Field_12 { get; set; }
        // 13
        [FlatBufferItem(14)] public int Field_14 { get; set; } // byte?
        [FlatBufferItem(15)] public byte Num_15 { get; set; }

        public override string ToString() => ((Species)Num_02).ToString();
    }

    [FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
    public class PlacementZone8_F02_Nine
    {
        [FlatBufferItem(00)] public byte Field_00 { get; set; }
        [FlatBufferItem(01)] public byte Field_01 { get; set; }
        [FlatBufferItem(02)] public byte Field_02 { get; set; }
        // 3
        [FlatBufferItem(04)] public ulong Hash_04 { get; set; }
        [FlatBufferItem(05)] public byte Field_05 { get; set; }
        // 6
        [FlatBufferItem(07)] public ulong Hash_07 { get; set; }
        [FlatBufferItem(08)] public int Field_08 { get; set; } // byte?
    }

    [FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
    public class PlacementZone8_F02
    {
        [FlatBufferItem(00)] public PlacementZoneMetaTripleXYZ8 Field_00 { get; set; }
        [FlatBufferItem(01)] public ulong Hash_01 { get; set; }
        [FlatBufferItem(02)] public ulong Hash_02 { get; set; }
        [FlatBufferItem(03)] public ulong Hash_03 { get; set; }
        [FlatBufferItem(04)] public ulong Hash_04 { get; set; }
        // 05 default
        // 06 default
        // 07 default
        // 08 default
        // 09 empty-object?
        // 10 default
        // 11 empty-object?
        [FlatBufferItem(12)] public ulong Hash_12 { get; set; }
    }

    [FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
    public class PlacementZone8_F02_Field1
    {
        [FlatBufferItem(00)] public PlacementZone8_F02_Inner Field_00 { get; set; }
    }

    [FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
    public class PlacementZone8_F02_Inner
    {
        [FlatBufferItem(0)] public PlacementZoneMetaTripleXYZ8 Field_00 { get; set; }
        [FlatBufferItem(1)] public ulong Hash_01 { get; set; }
        [FlatBufferItem(2)] public ulong Hash_02 { get; set; }
        [FlatBufferItem(3)] public ulong Hash_03 { get; set; }
        [FlatBufferItem(4)] public PlacementZone8_F02_IntFloat Field_04 { get; set; }
        [FlatBufferItem(5)] public byte Num_05 { get; set; }
        [FlatBufferItem(6)] public ulong Hash_06 { get; set; }
        [FlatBufferItem(7)] public PlacementZone8_F02_IntFloat Field_07 { get; set; }
    }

    [FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
    public class PlacementZone8_F02_IntFloat
    {
        [FlatBufferItem(00)] public int Field_00 { get; set; }
        // 1
        // 2
        [FlatBufferItem(03)] public float Field_03 { get; set; }
        [FlatBufferItem(04)] public float Field_04 { get; set; }
    }
}
