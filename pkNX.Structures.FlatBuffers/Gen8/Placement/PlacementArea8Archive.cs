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
    public class PlacementArea8Archive : IFlatBufferArchive<PlacementZone8>
    {
        [FlatBufferItem(00)] public PlacementZone8[] Table { get; set; } = Array.Empty<PlacementZone8>();
        [FlatBufferItem(01)] public ulong Hash { get; set; }
        [FlatBufferItem(02)] public string Description { get; set; } = "";
        [FlatBufferItem(03)] public string OtherDescription { get; set; } = "";
        [FlatBufferItem(04)] public PlacementArea8_F04 Unknown { get; set; } = new();
        [FlatBufferItem(05)] public float Field_05 { get => 0; set { if (value != 0) throw new ArgumentException("Not Observed"); } } // unused
        [FlatBufferItem(06)] public float Field_06 { get; set; }
        [FlatBufferItem(07)] public float Field_07 { get; set; }
        [FlatBufferItem(08)] public float Field_08 { get => 0; set { if (value != 0) throw new ArgumentException("Not Observed"); } } // unused
        [FlatBufferItem(09)] public float Field_09 { get => 0; set { if (value != 0) throw new ArgumentException("Not Observed"); } } // unused
        [FlatBufferItem(10)] public float Field_10 { get => 0; set { if (value != 0) throw new ArgumentException("Not Observed"); } } // unused
        [FlatBufferItem(11)] public float Field_11 { get => 0; set { if (value != 0) throw new ArgumentException("Not Observed"); } } // unused
        [FlatBufferItem(12)] public float Field_12 { get; set; }
        [FlatBufferItem(13)] public float Field_13 { get; set; }
        [FlatBufferItem(14)] public float Field_14 { get; set; }
        [FlatBufferItem(15)] public float Field_15 { get; set; }
        [FlatBufferItem(16)] public float Field_16 { get; set; }
        [FlatBufferItem(17)] public float Field_17 { get; set; }
        [FlatBufferItem(18)] public float Field_18 { get => 0; set { if (value != 0) throw new ArgumentException("Not Observed"); } } // unused
        [FlatBufferItem(19)] public float Field_19 { get => 0; set { if (value != 0) throw new ArgumentException("Not Observed"); } } // unused
        [FlatBufferItem(20)] public float Field_20 { get; set; }
        [FlatBufferItem(21)] public float Field_21 { get; set; }
        [FlatBufferItem(22)] public float Field_22 { get; set; }
        [FlatBufferItem(23)] public float Field_23 { get; set; }
        [FlatBufferItem(24)] public PlacementArea8_F24 Field_24 { get; set; } = new();
        [FlatBufferItem(25)] public uint  Field_25 { get; set; } // 3000
        [FlatBufferItem(26)] public float Field_26 { get; set; }
        [FlatBufferItem(27)] public byte  Field_27 { get; set; }
        [FlatBufferItem(28)] public byte  Field_28 { get; set; }
        [FlatBufferItem(29)] public byte  Field_29 { get; set; } // present in a_d0101
        [FlatBufferItem(30)] public byte  Field_30 { get; set; }
        [FlatBufferItem(31)] public float Field_31 { get; set; }
        [FlatBufferItem(32)] public float Field_32 { get; set; }
        [FlatBufferItem(33)] public byte  Field_33 { get; set; }
    }
}
