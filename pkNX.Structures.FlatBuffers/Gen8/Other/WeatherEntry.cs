using System.ComponentModel;
using FlatSharp.Attributes;
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global
#nullable disable

namespace pkNX.Structures.FlatBuffers;

// field\param\weather\weather_data.bin
// field\param\weather\weather_data_alt.bin
[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class WeatherTable : IFlatBufferArchive<WeatherEntry>
{
    [FlatBufferItem(0)] public WeatherEntry[] Table { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class WeatherEntry
{
    [FlatBufferItem(00)] public byte Field_00 { get; set; } // all except 0
    [FlatBufferItem(01)] public int[] Field_01 { get; set; } // unused in main, used in _alt
    [FlatBufferItem(02)] public string[] Field_02 { get; set; }
    [FlatBufferItem(03)] public float Field_03 { get; set; } // entry 2 3 4 5 6 7 8
    [FlatBufferItem(04)] public float Field_04 { get; set; } // entry 2 3 4 5 6 7 8
    [FlatBufferItem(05)] public float Field_05 { get; set; } // entry 2 3 4 5 6 7 8 9 10 11 12 14
    [FlatBufferItem(06)] public float Field_06 { get; set; } // unused
    [FlatBufferItem(07)] public float Field_07 { get; set; } // entry 2 3
    [FlatBufferItem(08)] public float Field_08 { get; set; } // entry 2 3 7
    [FlatBufferItem(09)] public uint Field_09 { get; set; }
    [FlatBufferItem(10)] public uint Field_10 { get; set; }
    [FlatBufferItem(11)] public float Field_11 { get; set; } // entry 1 2 3 4 5 7 8 10 11 12 13 14
    [FlatBufferItem(12)] public PentaFloat Field_12 { get; set; }
    [FlatBufferItem(13)] public float[] Field_13 { get; set; }
    [FlatBufferItem(14)] public float[] Field_14 { get; set; }
    [FlatBufferItem(15)] public QuadFloatSet Field_15 { get; set; }
    [FlatBufferItem(16)] public QuadFloatSet Field_16 { get; set; }
    [FlatBufferItem(17)] public PentaFloat Field_17 { get; set; }
    [FlatBufferItem(18)] public float Field_18 { get; set; } // entry 1 2 3 4 5 7 8 10 11 12 13 14
    [FlatBufferItem(19)] public float Field_19 { get; set; }
    [FlatBufferItem(20)] public float Field_20 { get; set; }
    [FlatBufferItem(21)] public float[] Field_21 { get; set; }
    [FlatBufferItem(22)] public float[] Field_22 { get; set; }
    [FlatBufferItem(23)] public int[] Field_23 { get; set; }
    [FlatBufferItem(24)] public byte Field_24 { get; set; } // entry 9 10 11 12 13 14
    [FlatBufferItem(25)] public uint Field_25 { get; set; } // unused
    [FlatBufferItem(26)] public uint Field_26 { get; set; } // unused
    [FlatBufferItem(27)] public byte Field_27 { get; set; } // entry 6
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class PentaFloat
{
    [FlatBufferItem(00)] public float Field_00 { get; set; }
    [FlatBufferItem(01)] public float Field_01 { get; set; }
    [FlatBufferItem(02)] public float Field_02 { get; set; }
    [FlatBufferItem(03)] public float Field_03 { get; set; }
    [FlatBufferItem(04)] public float Field_04 { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class QuadFloatSet
{
    [FlatBufferItem(00)] public float[] Field_00 { get; set; }
    [FlatBufferItem(01)] public float[] Field_01 { get; set; }
    [FlatBufferItem(02)] public float[] Field_02 { get; set; }
    [FlatBufferItem(03)] public float[] Field_03 { get; set; }
}
