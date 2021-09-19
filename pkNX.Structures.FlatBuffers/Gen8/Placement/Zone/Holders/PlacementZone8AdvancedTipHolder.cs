using System;
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
    public class PlacementZone8AdvancedTipHolder
    {
        [FlatBufferItem(00)] public PlacementZone8AdvancedTip Field_00 { get; set; }
        [FlatBufferItem(01)] public uint Field_01 { get => 0; set { if (value != 0) throw new ArgumentException("Not Observed"); } } // unused
        [FlatBufferItem(02)] public uint Field_02 { get => 0; set { if (value != 0) throw new ArgumentException("Not Observed"); } } // unused
        [FlatBufferItem(03)] public ulong SignHash { get; set; }
    }

    [FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
    public class PlacementZone8AdvancedTip
    {
        [FlatBufferItem(00)] public PlacementZone8_F14 Field_00 { get; set; }
    }

    [FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
    public class PlacementZone8_F14
    {
        [FlatBufferItem(00)] public PlacementZoneMetaTripleXYZ8 Field_00 { get; set; }
        [FlatBufferItem(01)] public string NameModel { get; set; }
        [FlatBufferItem(02)] public string NameAnimation { get; set; } // none have this
        [FlatBufferItem(03)] public float Field_03 { get; set; }
        [FlatBufferItem(04)] public float Field_04 { get; set; }
        [FlatBufferItem(05)] public string Field_05 { get; set; } // none have this
        [FlatBufferItem(06)] public string Field_06 { get; set; } // none have this
        [FlatBufferItem(07)] public float Field_07 { get; set; }
        [FlatBufferItem(08)] public float Field_08 { get; set; }
        [FlatBufferItem(09)] public float Field_09 { get; set; }
        [FlatBufferItem(10)] public float Field_10 { get => 0; set { if (value != 0) throw new ArgumentException("Not Observed"); } } // unused
        [FlatBufferItem(11)] public PlacementZone8_F14_B Field_11 { get; set; }
        [FlatBufferItem(12)] public uint Field_12 { get => 0; set { if (value != 0) throw new ArgumentException("Not Observed"); } } // unused
        [FlatBufferItem(13)] public PlacementZone8_F14_B Field_13 { get; set; }
        [FlatBufferItem(14)] public PlacementZone8_F14_Union Field_14 { get; set; }
    }

    [FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
    public class PlacementZone8_F14_B
    {
        [FlatBufferItem(00)] public uint Field_00 { get; set; } // 2
        [FlatBufferItem(01)] public float Field_01 { get => 0; set { if (value != 0) throw new ArgumentException("Not Observed"); } } // unused
        [FlatBufferItem(02)] public float Field_02 { get => 0; set { if (value != 0) throw new ArgumentException("Not Observed"); } } // unused
        [FlatBufferItem(03)] public float Field_03 { get; set; }
        [FlatBufferItem(04)] public float Field_04 { get => 0; set { if (value != 0) throw new ArgumentException("Not Observed"); } } // unused
        [FlatBufferItem(05)] public float Field_05 { get => 0; set { if (value != 0) throw new ArgumentException("Not Observed"); } } // unused
        [FlatBufferItem(06)] public float Field_06 { get; set; }
        [FlatBufferItem(07)] public float Field_07 { get => 0; set { if (value != 0) throw new ArgumentException("Not Observed"); } } // unused
        [FlatBufferItem(08)] public float Field_08 { get; set; }
        [FlatBufferItem(09)] public float Field_09 { get; set; }
        [FlatBufferItem(10)] public float Field_10 { get; set; }
    }

    [FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
    public class PlacementZone8_F14_Union
    {
        [FlatBufferItem(00)] public bool Field_00 { get; set; } = true;
        [FlatBufferItem(01)] public PlacementZone8_F14_Sub Field_01 { get; set; }
    }

    [FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
    public class PlacementZone8_F14_Sub
    {
        [FlatBufferItem(00)] public float Field_00 { get => 0; set { if (value != 0) throw new ArgumentException("Not Observed"); } } // unused
        [FlatBufferItem(01)] public float Field_01 { get; set; }
    }
}
