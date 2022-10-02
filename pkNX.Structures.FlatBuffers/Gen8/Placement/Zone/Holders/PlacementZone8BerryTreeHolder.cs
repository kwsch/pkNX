using System;
using System.ComponentModel;
using FlatSharp.Attributes;
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global

namespace pkNX.Structures.FlatBuffers;

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class PlacementZone8BerryTreeHolder
{
    [FlatBufferItem(00)] public PlacementZone8_F22_0 Field_00 { get; set; } = new(); // meta
    [FlatBufferItem(01)] public PlacementZone8BerryTreeRandom[] Field_01 { get; set; } = Array.Empty<PlacementZone8BerryTreeRandom>();
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class PlacementZone8_F22_0
{
    [FlatBufferItem(00)] public PlacementZone8_F22_0_0 Field_00 { get; set; } = new();
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class PlacementZone8_F22_0_0
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
    [FlatBufferItem(11)] public PlacementZone8_F22_Sub Field_11 { get; set; } = new();
    [FlatBufferItem(12)] public uint Field_12 { get => 0; set { if (value != 0) throw new ArgumentException("Not Observed"); } } // unused
    [FlatBufferItem(13)] public PlacementZone8_F22_Sub Field_13 { get; set; } = new();
    [FlatBufferItem(14)] public PlacementZone8_F22_BoolObject14 Field_14 { get; set; } = new();
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class PlacementZone8_F22_Sub
{
    [FlatBufferItem(00)] public uint Field_00 { get; set; }
    [FlatBufferItem(01)] public float Field_01 { get; set; } // unused in all
    [FlatBufferItem(02)] public float Field_02 { get; set; } // unused in all
    [FlatBufferItem(03)] public float Field_03 { get; set; } // unused in all
    [FlatBufferItem(04)] public float Field_04 { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class PlacementZone8_F22_BoolObject14
{
    [FlatBufferItem(0)] public byte Type { get; set; }
    [FlatBufferItem(1)] public PlacementZone_F22_Inner Object { get; set; } = new();
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class PlacementZone_F22_Inner
{
    [FlatBufferItem(00)] public float Field_00 { get => 0; set { if (value != 0) throw new ArgumentException("Not Observed"); } } // unused
    [FlatBufferItem(01)] public float Field_01 { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class PlacementZone8BerryTreeRandom
{
    [FlatBufferItem(0)] public ulong Hash { get; set; }
    [FlatBufferItem(1)] public uint Rate { get; set; }
    [FlatBufferItem(2)] public uint Quantity { get; set; } // always 1?
}
