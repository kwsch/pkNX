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
public class PlacementZone8WarpHolder
{
    [FlatBufferItem(00)] public PlacementZoneWarp8 Field_00 { get; set; } = new();

    public override string ToString() => $"{Field_00.NameAreaOther} via {Field_00.NameModel}";
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class PlacementZoneWarp8
{
    [FlatBufferItem(00)] public PlacementZoneMetaTripleXYZ8 Field_00 { get; set; } = new();
    [FlatBufferItem(01)] public ulong Hash_01 { get; set; }
    [FlatBufferItem(02)] public string NameAreaOther { get; set; } = "";
    [FlatBufferItem(03)] public string NameModel { get; set; } = "";
    [FlatBufferItem(04)] public string NameAnimation { get; set; } = "";
    [FlatBufferItem(05)] public int Field_05 { get; set; }
    [FlatBufferItem(06)] public float Field_06 { get; set; }
    [FlatBufferItem(07)] public bool Field_07 { get; set; }
    [FlatBufferItem(08)] public ulong Hash_08 { get; set; }
    [FlatBufferItem(09)] public PlacementZoneWarpDetails8 SubMeta { get; set; } = new();
    [FlatBufferItem(10)] public string NameSoundEffect1 { get; set; } = "";
    [FlatBufferItem(11)] public string NameSoundEffect2 { get; set; } = "";
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class PlacementZoneWarpDetails8
{
    [FlatBufferItem(00)] public int Field_00 { get; set; }
    [FlatBufferItem(01)] public float Field_01 { get; set; }
    [FlatBufferItem(02)] public float Field_02 { get => 0; set { if (value != 0) throw new ArgumentException("Not Observed"); } } // unused
    [FlatBufferItem(03)] public float Field_03 { get; set; }
    [FlatBufferItem(04)] public float Field_04 { get => 0; set { if (value != 0) throw new ArgumentException("Not Observed"); } } // unused
    [FlatBufferItem(05)] public float Field_05 { get => 0; set { if (value != 0) throw new ArgumentException("Not Observed"); } } // unused
    [FlatBufferItem(06)] public float Field_06 { get => 0; set { if (value != 0) throw new ArgumentException("Not Observed"); } } // unused
    [FlatBufferItem(07)] public float Field_07 { get => 0; set { if (value != 0) throw new ArgumentException("Not Observed"); } } // unused
    [FlatBufferItem(08)] public float Field_08 { get; set; }
    [FlatBufferItem(09)] public float Field_09 { get; set; }
    [FlatBufferItem(10)] public float Field_10 { get; set; }
}
