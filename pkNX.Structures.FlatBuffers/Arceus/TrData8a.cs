using System;
using System.ComponentModel;
using System.Linq;
using FlatSharp.Attributes;

// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global

namespace pkNX.Structures.FlatBuffers;

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class TrData8a
{
    [FlatBufferItem(00)] public ulong Hash_00 { get; set; } // ulong
    [FlatBufferItem(01)] public ulong Hash_01 { get; set; } // ulong
    [FlatBufferItem(02)] public ulong Hash_02 { get; set; } // ulong
    [FlatBufferItem(03)] public string Music  { get; set; } = string.Empty;
    [FlatBufferItem(04)] public string EE_04  { get; set; } = string.Empty;
    [FlatBufferItem(05)] public string EE_05  { get; set; } = string.Empty;
    [FlatBufferItem(06)] public string EE_06  { get; set; } = string.Empty;
    [FlatBufferItem(07)] public ulong Hash_07 { get; set; } // ulong
    [FlatBufferItem(08)] public ulong Hash_08 { get; set; } // ulong
    [FlatBufferItem(09)] public TrFloatQuad Field_09 { get; set; } = new();
    [FlatBufferItem(10)] public int  Field_10 { get; set; } // int
    [FlatBufferItem(11)] public int  Field_11 { get; set; } // int
    [FlatBufferItem(12)] public int  Field_12 { get; set; } // int
    [FlatBufferItem(13)] public byte Field_13 { get; set; } // UNUSED?
    [FlatBufferItem(14)] public byte Field_14 { get; set; } // UNUSED?
    [FlatBufferItem(15)] public byte Field_15 { get; set; } // UNUSED?
    [FlatBufferItem(16)] public byte Field_16 { get; set; } // byte
    [FlatBufferItem(17)] public byte Field_17 { get; set; } // byte
    [FlatBufferItem(18)] public byte Field_18 { get; set; } // byte
    [FlatBufferItem(19)] public byte Field_19 { get; set; } // UNUSED?
    [FlatBufferItem(20)] public byte Field_20 { get; set; } // byte
    [FlatBufferItem(21)] public byte Field_21 { get; set; } // byte
    [FlatBufferItem(22)] public TrPoke8a[] Team { get; set; } = Array.Empty<TrPoke8a>();

    public string TeamSummary => Environment.NewLine + string.Join(Environment.NewLine, Team.Select(z => z.ToString()));
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class TrPoke8a
{
    [FlatBufferItem(00)] public int Species { get; set; }
    [FlatBufferItem(01)] public int Form { get; set; }
    [FlatBufferItem(02)] public TrPoke8aMove Move_01 { get; set; } = new();
    [FlatBufferItem(03)] public TrPoke8aMove Move_02 { get; set; } = new();
    [FlatBufferItem(04)] public TrPoke8aMove Move_03 { get; set; } = new();
    [FlatBufferItem(05)] public TrPoke8aMove Move_04 { get; set; } = new();
    [FlatBufferItem(06)] public int Field_06 { get; set; }
    [FlatBufferItem(07)] public int Field_07 { get; set; }
    [FlatBufferItem(08)] public int Field_08 { get; set; }
    [FlatBufferItem(09)] public int Field_09 { get; set; }
    [FlatBufferItem(10)] public int Field_10 { get; set; }
    [FlatBufferItem(11)] public int Field_11 { get; set; }
    [FlatBufferItem(12)] public int Field_12 { get; set; }
    [FlatBufferItem(13)] public int Field_13 { get; set; }
    [FlatBufferItem(14)] public int Field_14 { get; set; }
    [FlatBufferItem(15)] public int Field_15 { get; set; }

    public override string ToString()
    {
        return $"{Species}{(Form == 0 ? "" : $"-{Form}")}|{Move_01}|{Move_02}|{Move_03}|{Move_04}|{Field_06}|{Field_07}|{Field_08}|{Field_09}|{Field_10}|{Field_11}|{Field_12}|{Field_13}|{Field_14}|{Field_15}";
    }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class TrPoke8aMove
{
    [FlatBufferItem(00)] public int  Move { get; set; }
    [FlatBufferItem(01)] public bool Flag { get; set; }
    public override string ToString() => $"{Move}{(Flag?"*":"")}";
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class TrFloatQuad
{
    [FlatBufferItem(00)] public float Float_00 { get; set; }
    [FlatBufferItem(01)] public float Float_01 { get; set; }
    [FlatBufferItem(02)] public float Float_02 { get; set; }
    [FlatBufferItem(03)] public float Float_03 { get; set; }

    public override string ToString() => $"({Float_00},{Float_01},{Float_02},{Float_03})";
}
