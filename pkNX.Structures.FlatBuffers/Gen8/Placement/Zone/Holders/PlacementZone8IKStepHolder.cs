using System;
using System.ComponentModel;
using FlatSharp.Attributes;
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global

namespace pkNX.Structures.FlatBuffers;

// IK_Step
[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class PlacementZone8IKStepHolder
{
    [FlatBufferItem(00)] public PlacementZone8_F25 Field_00 { get; set; } = new();
    [FlatBufferItem(01)] public byte Field_01 { get; set; }
    [FlatBufferItem(02)] public byte Field_02 { get => 0; set { if (value != 0) throw new ArgumentException("Not Observed"); } } // unused
    [FlatBufferItem(03)] public byte Field_03 { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class PlacementZone8_F25
{
    [FlatBufferItem(00)] public PlacementZoneMetaTripleXYZ8 Field_00 { get; set; } = new();
    [FlatBufferItem(01)] public ulong Field_01 { get; set; }
    [FlatBufferItem(02)] public PlacementZone8_F25_X Field_02 { get; set; } = new();
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class PlacementZone8_F25_X
{
    [FlatBufferItem(00)] public uint Field_00 { get; set; }
    [FlatBufferItem(01)] public uint Field_01 { get => 0; set { if (value != 0) throw new ArgumentException("Not Observed"); } } // unused

    [FlatBufferItem(02)] public float Field_02 { get => 0; set { if (value != 0) throw new ArgumentException("Not Observed"); } } // unused
    [FlatBufferItem(03)] public float Field_03 { get => 0; set { if (value != 0) throw new ArgumentException("Not Observed"); } } // unused
    [FlatBufferItem(04)] public float Field_04 { get => 0; set { if (value != 0) throw new ArgumentException("Not Observed"); } } // unused

    [FlatBufferItem(05)] public float Field_05 { get => 0; set { if (value != 0) throw new ArgumentException("Not Observed"); } } // unused
    [FlatBufferItem(06)] public float Field_06 { get; set; }
    [FlatBufferItem(07)] public float Field_07 { get => 0; set { if (value != 0) throw new ArgumentException("Not Observed"); } } // unused

    [FlatBufferItem(08)] public float Field_08 { get; set; }
    [FlatBufferItem(09)] public float Field_09 { get; set; }
    [FlatBufferItem(10)] public float Field_10 { get; set; }
}
