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
public class DressUpTable8a : IFlatBufferArchive<DressUpEntry8a>
{
    public byte[] Write() => FlatBufferConverter.SerializeFrom(this);

    [FlatBufferItem(0)] public DressUpEntry8a[] Table { get; set; } = Array.Empty<DressUpEntry8a>();
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class DressUpEntry8a
{
    [FlatBufferItem(00)] public string EntryName { get; set; } = string.Empty;
    [FlatBufferItem(01)] public uint Field_1 { get; set; }
    [FlatBufferItem(02)] public uint Field_2 { get; set; }
    [FlatBufferItem(03)] public DressupPartType8a PartHash { get; set; }
    [FlatBufferItem(04)] public uint Field_4 { get; set; }
    [FlatBufferItem(05)] public DressUpColorType8a ColorHash { get; set; }
    [FlatBufferItem(06)] public uint Field_6 { get; set; }
    [FlatBufferItem(07)] public uint Field_7 { get; set; }
    [FlatBufferItem(08)] public uint Field_8 { get; set; }
    [FlatBufferItem(09)] public DressUpUnlockFlagType8a UnlockFlag { get; set; }
    [FlatBufferItem(10)] public uint Field_10 { get; set; }
    [FlatBufferItem(11)] public uint Field_11 { get; set; }
    [FlatBufferItem(12)] public uint Field_12 { get; set; }
    [FlatBufferItem(13)] public uint Field_13 { get; set; }
    [FlatBufferItem(14)] public uint Field_14 { get; set; } //Never used
    [FlatBufferItem(15)] public string Field_15 { get; set; } = string.Empty; //Might be a string?
    [FlatBufferItem(16)] public uint Field_16 {get; set;}
    [FlatBufferItem(17)] public uint Field_17 {get; set;}
    [FlatBufferItem(18)] public uint Field_18 {get; set;}
    [FlatBufferItem(19)] public uint Field_19 {get; set;}
    [FlatBufferItem(20)] public string Field_20 { get; set; } = string.Empty; //Might be a string?
    [FlatBufferItem(21)] public uint Field_21 {get; set;}
    [FlatBufferItem(22)] public uint Field_22 {get; set;}
    [FlatBufferItem(23)] public uint Field_23 {get; set;}
    [FlatBufferItem(24)] public uint Field_24 {get; set;}
    [FlatBufferItem(25)] public DressUpHideFlagType8a HideHash_0 { get; set; }
    [FlatBufferItem(26)] public DressUpHideFlagType8a HideHash_1 { get; set; }
    [FlatBufferItem(27)] public string HairStyleName { get; set; } = string.Empty;
    [FlatBufferItem(28)] public string FaceName { get; set; } = string.Empty;
    [FlatBufferItem(29)] public string EyeBName { get; set; } = string.Empty;
    [FlatBufferItem(30)] public string EyeWName { get; set; } = string.Empty;
    [FlatBufferItem(31)] public string HeadwearName { get; set; } = string.Empty;
    [FlatBufferItem(32)] public string TopName { get; set; } = string.Empty;
    [FlatBufferItem(33)] public string BottomName { get; set; } = string.Empty;
    [FlatBufferItem(34)] public string DressName { get; set; } = string.Empty;
    [FlatBufferItem(35)] public string FootwearName { get; set; } = string.Empty;
    [FlatBufferItem(36)] public string BagName { get; set; } = string.Empty;
    [FlatBufferItem(37)] public string UnusedName { get; set; } = string.Empty;
    [FlatBufferItem(38)] public byte Field_38 { get; set; }
    [FlatBufferItem(39)] public string SlotName { get; set; } = string.Empty;
    [FlatBufferItem(40)] public string PartIndex0 { get; set; } = string.Empty;
    [FlatBufferItem(41)] public string PartIndex1 { get; set; } = string.Empty;
    [FlatBufferItem(42)] public string PartIndex2 { get; set; } = string.Empty;
    [FlatBufferItem(43)] public string PartIndex3 { get; set; } = string.Empty;
    [FlatBufferItem(44)] public string PartIndex4 { get; set; } = string.Empty;
    [FlatBufferItem(45)] public string ConfigMotionPath { get; set; } = string.Empty;
    [FlatBufferItem(46)] public string DefaultMotionPath { get; set; } = string.Empty;
    [FlatBufferItem(47)] public string DataPath { get; set; } = string.Empty;
    [FlatBufferItem(48)] public string DefaultPartName { get; set; } = string.Empty;
    [FlatBufferItem(49)] public string MotionPath { get; set; } = string.Empty;
    [FlatBufferItem(50)] public byte Field_50 { get; set; }
    [FlatBufferItem(51)] public uint Field_51 { get; set; } // Unused
    [FlatBufferItem(52)] public string CategoryName { get; set; } = String.Empty;
    [FlatBufferItem(53)] public string SubCategoryName { get; set; } = String.Empty;
 
}
