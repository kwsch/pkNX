using System;
using System.ComponentModel;
using FlatSharp.Attributes;

// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global

namespace pkNX.Structures.FlatBuffers;

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class PokeAIArchive8a : IFlatBufferArchive<PokeAI8a>
{
    public byte[] Write() => FlatBufferConverter.SerializeFrom(this);

    [FlatBufferItem(0)] public PokeAI8a[] Table { get; set; } = Array.Empty<PokeAI8a>();
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class PokeAI8a
{
    [FlatBufferItem(00)] public int Species { get; set; } // int
    [FlatBufferItem(01)] public int Form { get; set; } // int
    [FlatBufferItem(02)] public bool IsAlpha { get; set; } // bool
    [FlatBufferItem(03)] public string Behavior1 { get; set; } = string.Empty;
    [FlatBufferItem(04)] public string Behavior2 { get; set; } = string.Empty;
    [FlatBufferItem(05)] public string Behavior3 { get; set; } = string.Empty;
    [FlatBufferItem(06)] public bool Field_06 { get; set; } // bool
    [FlatBufferItem(07)] public ulong Field_07 { get; set; } // ulong
    [FlatBufferItem(08)] public bool Field_08 { get; set; } // bool, always true
    [FlatBufferItem(09)] public PokeAI8a_F09 Field_09 { get; set; } = new();
    [FlatBufferItem(10)] public bool Field_10 { get; set; } // bool, always true
    [FlatBufferItem(11)] public PokeAI8a_F09 Field_11 { get; set; } = new();
    [FlatBufferItem(12)] public bool Field_12 { get; set; } // bool, always true except for Darkrai
    [FlatBufferItem(13)] public PokeAI8a_F09 Field_13 { get; set; } = new();
    [FlatBufferItem(14)] public bool Field_14 { get; set; } // bool, always true
    [FlatBufferItem(15)] public PokeAI8a_F09 Field_15 { get; set; } = new();
    [FlatBufferItem(16)] public bool Field_16 { get; set; } // bool, always true
    [FlatBufferItem(17)] public PokeAI8a_F09 Field_17 { get; set; } = new();
    [FlatBufferItem(18)] public PokeAI8a_F18 Field_18 { get; set; } = new(); // only used by Kleavor-1
    [FlatBufferItem(19)] public PokeAI8a_F18 Field_19 { get; set; } = new(); // assumed same as above, none use
    [FlatBufferItem(20)] public PokeAI8a_F18 Field_20 { get; set; } = new(); // assumed same as above, none use
    [FlatBufferItem(21)] public PokeAI8a_F18 Field_21 { get; set; } = new(); // assumed same as above, only used by Kleavor-1 with fields 0,1,2
    [FlatBufferItem(22)] public string MoveEffect1 { get; set; } = string.Empty;
    [FlatBufferItem(23)] public string MoveEffect2 { get; set; } = string.Empty;
    [FlatBufferItem(24)] public PokeAI8a_F24 Field_24 { get; set; } = new();
    [FlatBufferItem(25)] public float Field_25 { get; set; } // float
    [FlatBufferItem(26)] public float Field_26 { get; set; } // float
    [FlatBufferItem(27)] public PlacementV3f8a Field_27 { get; set; } = new();
    [FlatBufferItem(28)] public PlacementV3f8a Field_28 { get; set; } = new();
    [FlatBufferItem(29)] public PlacementV3f8a Field_29 { get; set; } = new();
    [FlatBufferItem(30)] public PlacementV3f8a Field_30 { get; set; } = new();
    [FlatBufferItem(31)] public int Field_31 { get; set; } // int
    [FlatBufferItem(32)] public int Field_32 { get; set; } // int
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class PokeAI8a_F24
{
    [FlatBufferItem(00)] public float Field_00 { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class PokeAI8a_F18
{
    [FlatBufferItem(00)] public bool  Field_00 { get; set; }
    [FlatBufferItem(01)] public float Field_01 { get; set; }
    [FlatBufferItem(02)] public float Field_02 { get; set; }
    [FlatBufferItem(03)] public float Field_03 { get; set; }
    [FlatBufferItem(04)] public float Field_04 { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class PokeAI8a_F09
{
    [FlatBufferItem(00)] public PokeAI8_F08 Field_00 { get; set; } = new();
    [FlatBufferItem(01)] public PokeAI8_F08 Field_01 { get; set; } = new();
    [FlatBufferItem(02)] public PokeAI8_F08 Field_02 { get; set; } = new();
    [FlatBufferItem(03)] public PokeAI8_F08 Field_03 { get; set; } = new();
    [FlatBufferItem(04)] public PokeAI8_F08 Field_04 { get; set; } = new();
    [FlatBufferItem(05)] public PokeAI8_F08 Field_05 { get; set; } = new();
    [FlatBufferItem(06)] public PokeAI8_F08 Field_06 { get; set; } = new();
    [FlatBufferItem(07)] public PokeAI8_F08 Field_07 { get; set; } = new();
    [FlatBufferItem(08)] public PokeAI8_F08 Field_08 { get; set; } = new();
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class PokeAI8_F08
{
    [FlatBufferItem(00)] public ulong Hash { get; set; }
    [FlatBufferItem(01)] public string[] Field_01 { get; set; } = Array.Empty<string>();
}
