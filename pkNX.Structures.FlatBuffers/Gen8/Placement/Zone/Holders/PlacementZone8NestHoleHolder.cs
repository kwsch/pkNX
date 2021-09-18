using System;
using System.ComponentModel;
using FlatSharp.Attributes;
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global

namespace pkNX.Structures.FlatBuffers
{
    [FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
    public class PlacementZone8NestHoleHolder
    {
        [FlatBufferItem(00)] public PlacementZone8_F21_A Field_00 { get; set; } = new();
        [FlatBufferItem(01)] public bool Field_01 { get; set; }
        [FlatBufferItem(02)] public int Field_02 { get; set; } // 0,2,6,270,64,12
        [FlatBufferItem(03)] public ulong Common { get; set; }
        [FlatBufferItem(04)] public ulong Rare { get; set; }
        [FlatBufferItem(05)] public bool Field_05 { get; set; }

        [Description("If a flag hash is specified, the savefile value must be true in order for the nest to be enabled.")]
        [FlatBufferItem(06)] public ulong EnableSpawns { get; set; }
    }

    [FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
    public class PlacementZone8_F21_A
    {
        [FlatBufferItem(00)] public PlacementZone8_F21_B Field_00 { get; set; } = new();
    }

    [FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
    public class PlacementZone8_F21_B
    {
        [FlatBufferItem(00)] public PlacementZoneMetaTripleXYZ8 Field_00 { get; set; } = new();
        [FlatBufferItem(01)] public string Field_01 { get; set; } = ""; // none have this
        [FlatBufferItem(02)] public string Field_02 { get; set; } = ""; // none have this
        [FlatBufferItem(03)] public float Field_03 { get; set; }
        [FlatBufferItem(04)] public float Field_04 { get; set; }
        [FlatBufferItem(05)] public string Field_05 { get; set; } = ""; // none have this
        [FlatBufferItem(06)] public string Field_06 { get; set; } = ""; // none have this
        [FlatBufferItem(07)] public float Field_07 { get => 0; set { if (value != 0) throw new ArgumentException("Not Observed"); } } // unused
        [FlatBufferItem(08)] public float Field_08 { get => 0; set { if (value != 0) throw new ArgumentException("Not Observed"); } } // unused
        [FlatBufferItem(09)] public float Field_09 { get => 0; set { if (value != 0) throw new ArgumentException("Not Observed"); } } // unused
        [FlatBufferItem(10)] public float Field_10 { get; set; }
        [FlatBufferItem(11)] public PlacementZone8_F21_IntFloat Field_11 { get; set; } = new();
        [FlatBufferItem(12)] public uint Field_12 { get => 0; set { if (value != 0) throw new ArgumentException("Not Observed"); } } // unused
        [FlatBufferItem(13)] public PlacementZone8_F21_IntFloat Field_13 { get; set; } = new();
        [FlatBufferItem(14)] public PlacementZone8_F21_BoolObject14 Field_14 { get; set; } = new();
    }

    [FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
    public class PlacementZone8_F21_IntFloat
    {
        [FlatBufferItem(00)] public int Field_00 { get; set; }
        [FlatBufferItem(01)] public float Field_01 { get; set; } // unused in all
        [FlatBufferItem(02)] public float Field_02 { get; set; } // unused in all
        [FlatBufferItem(03)] public float Field_03 { get; set; } // unused in all
        [FlatBufferItem(04)] public float Field_04 { get; set; }
    }

    [FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
    public class PlacementZone8_F21_BoolObject14
    {
        [FlatBufferItem(0)] public byte Type { get; set; }
        [FlatBufferItem(1)] public PlacementZone_F21_Inner Object { get; set; } = new();
    }

    [FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
    public class PlacementZone_F21_Inner
    {
        [FlatBufferItem(00)] public float Field_00 { get => 0; set { if (value != 0) throw new ArgumentException("Not Observed"); } } // unused
        [FlatBufferItem(01)] public float Field_01 { get; set; }
    }
}
