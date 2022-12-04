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
public class PokeFieldObstructionWaza8a
{
    [FlatBufferItem(00)] public string Field_00 { get; set; } = string.Empty;
    [FlatBufferItem(01)] public uint Field_01 { get; set; }
    [FlatBufferItem(02)] public uint Field_02 { get; set; } // None have this
    [FlatBufferItem(03)] public string Field_03 { get; set; } = string.Empty;
    [FlatBufferItem(04)] public string[] Field_04 { get; set; } = Array.Empty<string>();
    [FlatBufferItem(05)] public string Field_05 { get; set; } = string.Empty;
    [FlatBufferItem(06)] public uint Field_06 { get; set; }
    [FlatBufferItem(07)] public uint Field_07 { get; set; }
    [FlatBufferItem(08)] public string Field_08 { get; set; } = string.Empty; // Type: UInt32 AC (172) String string ()     ArrayType: Byte[0] UInt16[0] UInt32[0] Single[0] UInt64[0] Double[0] Object[0] String[0]
    [FlatBufferItem(09)] public float Field_09 { get; set; }
    [FlatBufferItem(10)] public uint Field_10 { get; set; } // None have this
    [FlatBufferItem(11)] public uint Field_11 { get; set; }
    [FlatBufferItem(12)] public float Field_12 { get; set; }
    [FlatBufferItem(13)] public float Field_13 { get; set; }
    [FlatBufferItem(14)] public float Field_14 { get; set; }
    [FlatBufferItem(15)] public float Field_15 { get; set; }
    [FlatBufferItem(16)] public uint Field_16 { get; set; } // None have this
    [FlatBufferItem(17)] public uint Field_17 { get; set; } // None have this
    [FlatBufferItem(18)] public uint Field_18 { get; set; } // None have this
    [FlatBufferItem(19)] public uint Field_19 { get; set; }
    [FlatBufferItem(20)] public uint Field_20 { get; set; }
    [FlatBufferItem(21)] public float Field_21 { get; set; }
    [FlatBufferItem(22)] public uint Field_22 { get; set; }
    [FlatBufferItem(23)] public string Field_23 { get; set; } = string.Empty; // Type: UInt32 84 (132) String string ()     ArrayType: Byte[0] UInt16[0] UInt32[0] Single[0] UInt64[0] Double[0] Object[0] String[0]
    [FlatBufferItem(24)] public string Field_24 { get; set; } = string.Empty; // Type: UInt32 78 (120) String string ()     ArrayType: Byte[0] UInt16[0] UInt32[0] Single[0] UInt64[0] Double[0] Object[0] String[0]
    [FlatBufferItem(25)] public uint Field_25 { get; set; } // None have this
    [FlatBufferItem(26)] public uint Field_26 { get; set; } // None have this
    [FlatBufferItem(27)] public uint Field_27 { get; set; } // None have this
    [FlatBufferItem(28)] public uint Field_28 { get; set; } // None have this
    [FlatBufferItem(29)] public uint Field_29 { get; set; }
    [FlatBufferItem(30)] public uint Field_30 { get; set; } // None have this
    [FlatBufferItem(31)] public bool Flag_31 { get; set; }
    [FlatBufferItem(32)] public bool Flag_32 { get; set; }
    [FlatBufferItem(33)] public float Field_33 { get; set; }
    [FlatBufferItem(34)] public float Field_34 { get; set; }
    [FlatBufferItem(35)] public bool Flag_35 { get; set; }
    [FlatBufferItem(36)] public bool Flag_36 { get; set; }
    [FlatBufferItem(37)] public string Field_37 { get; set; } = string.Empty; // Type: UInt32 60 (96) String string ()     ArrayType: Byte[0] UInt16[0] UInt32[0] Single[0] UInt64[0] Double[0] Object[0] String[0]
    [FlatBufferItem(38)] public uint Field_38 { get; set; }
    [FlatBufferItem(39)] public uint Field_39 { get; set; }
    [FlatBufferItem(40)] public uint Field_40 { get; set; } // None have this
    [FlatBufferItem(41)] public uint Field_41 { get; set; } // None have this
    [FlatBufferItem(42)] public uint Field_42 { get; set; } // None have this
    [FlatBufferItem(43)] public uint[] Field_43 { get; set; } = Array.Empty<uint>(); // Type: ArrayType: Byte[1] UInt16[1] UInt32[1] String[1]
    [FlatBufferItem(44)] public bool Flag_44 { get; set; }
    [FlatBufferItem(45)] public float Field_45 { get; set; }
    [FlatBufferItem(46)] public string Field_46 { get; set; } = string.Empty;
    [FlatBufferItem(47)] public float[] Field_47 { get; set; } = Array.Empty<float>();
    [FlatBufferItem(48)] public string Field_48 { get; set; } = string.Empty;
    [FlatBufferItem(49)] public float[] Field_49 { get; set; } = Array.Empty<float>();
    [FlatBufferItem(50)] public float Field_50 { get; set; }
}
