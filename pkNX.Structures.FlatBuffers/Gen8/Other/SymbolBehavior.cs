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
    // symbol_encount_mons_param.bin
    [FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
    public class SymbolBehaveRoot : IFlatBufferArchive<SymbolBehave>
    {
        [FlatBufferItem(00)] public SymbolBehave[] Table { get; set; }
    }

    [FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
    public class SymbolBehave
    {
        public Species Species => (Species)SpeciesID;

        [FlatBufferItem(00)] public float Field_00 {get; set;}
        [FlatBufferItem(01)] public float Field_01 {get; set;}
        [FlatBufferItem(02)] public string ModelPart {get; set;}
        [FlatBufferItem(03)] public float Field_03 {get; set;}
        [FlatBufferItem(04)] public ulong Hash1 {get; set;}
        [FlatBufferItem(05)] public ulong Hash2 {get; set;}
        [FlatBufferItem(06)] public float HitboxRadius {get; set;}
        [FlatBufferItem(07)] public float Field_07 {get; set;}
        [FlatBufferItem(08)] public float Field_08 {get; set;} // unused default, assumed float
        [FlatBufferItem(09)] public float Field_09 {get; set;}
        [FlatBufferItem(10)] public int Form {get; set;}
        [FlatBufferItem(11)] public byte Field_11 {get; set;}
        [FlatBufferItem(12)] public byte Field_12 {get; set;} // unused default, assumed byte
        [FlatBufferItem(13)] public int SpeciesID {get; set;}
        [FlatBufferItem(14)] public byte Field_14 {get; set;} // unused default, assumed byte
        [FlatBufferItem(15)] public byte Field_15 {get; set;} // unused default, assumed byte
        [FlatBufferItem(16)] public float Field_16 {get; set;}
        [FlatBufferItem(17)] public float Field_17 {get; set;}
        [FlatBufferItem(18)] public int Field_18 {get; set;}
        [FlatBufferItem(19)] public float Field_19 {get; set;}
        [FlatBufferItem(20)] public float Field_20 {get; set;}
        [FlatBufferItem(21)] public float Field_21 {get; set;}
        [FlatBufferItem(22)] public string SpeciesNameJPN {get; set;}
        [FlatBufferItem(23)] public float Field_23 {get; set;}
        [FlatBufferItem(24)] public float Field_24 {get; set;}
        [FlatBufferItem(25)] public float Field_25 {get; set;}
        [FlatBufferItem(26)] public float Field_26 {get; set;}
        [FlatBufferItem(27)] public float GrassShakeRadius {get; set;}
        [FlatBufferItem(28)] public float Field_28 {get; set;} // unused default, assumed float
        [FlatBufferItem(29)] public int Field_29 {get; set;}
        [FlatBufferItem(30)] public int Field_30 {get; set;} // unused default, assumed int
        [FlatBufferItem(31)] public string Behavior {get; set;}
        [FlatBufferItem(32)] public int Field_32 {get; set;}
        [FlatBufferItem(33)] public int Field_33 {get; set;} // unused default, assumed int
        [FlatBufferItem(34)] public int Field_34 {get; set;} // unused default, assumed int
        [FlatBufferItem(35)] public int Field_35 {get; set;} // unused default, assumed int
        [FlatBufferItem(36)] public int Field_36 {get; set; } // unused default, assumed int
        [FlatBufferItem(37)] public float Field_37 {get; set;}
        [FlatBufferItem(38)] public float Field_38 {get; set;}
        [FlatBufferItem(39)] public float Field_39 {get; set;}
        [FlatBufferItem(40)] public float Field_40 {get; set;}
        [FlatBufferItem(41)] public float Field_41 {get; set;}
        [FlatBufferItem(42)] public float Field_42 {get; set;} // unused default, assumed float
        [FlatBufferItem(43)] public float Field_43 {get; set;} // unused default, assumed float
        [FlatBufferItem(44)] public float Field_44 {get; set;}
        [FlatBufferItem(45)] public float Field_45 {get; set;}
    }
}
