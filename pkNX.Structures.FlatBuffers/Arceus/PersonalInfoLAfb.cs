using System;
using System.Collections.Generic;
using System.ComponentModel;
using FlatSharp.Attributes;
using System.Linq;

// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global

namespace pkNX.Structures.FlatBuffers;

/// <summary>
/// <see cref="PersonalInfo"/> class with values from the <see cref="GameVersion.PLA"/> games.
/// </summary>
[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class PersonalTableLAfb : IFlatBufferArchive<PersonalInfoLAfb>
{
    [FlatBufferItem(0)] public PersonalInfoLAfb[] Table { get; set; } = Array.Empty<PersonalInfoLAfb>();
}

/// <summary>
/// <see cref="PersonalInfo"/> class with values from the <see cref="GameVersion.PLA"/> games.
/// </summary>
[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class PersonalInfoLAfb
{
    [FlatBufferItem(00)] public ushort Species { get; set; } // ushort
    [FlatBufferItem(01)] public ushort Form { get; set; } // ushort
    [FlatBufferItem(02)] public bool IsPresentInGame { get; set; } // byte
    [FlatBufferItem(03)] public byte Type1 { get; set; } // byte
    [FlatBufferItem(04)] public byte Type2 { get; set; } // byte
    [FlatBufferItem(05)] public ushort Ability1 { get; set; } // ushort
    [FlatBufferItem(06)] public ushort Ability2 { get; set; } // ushort
    [FlatBufferItem(07)] public ushort AbilityH { get; set; } // ushort
    [FlatBufferItem(08)] public byte Stat_HP { get; set; } // byte
    [FlatBufferItem(09)] public byte Stat_ATK { get; set; } // byte
    [FlatBufferItem(10)] public byte Stat_DEF { get; set; } // byte
    [FlatBufferItem(11)] public byte Stat_SPA { get; set; } // byte
    [FlatBufferItem(12)] public byte Stat_SPD { get; set; } // byte
    [FlatBufferItem(13)] public byte Stat_SPE { get; set; } // byte
    [FlatBufferItem(14)] public byte Gender { get; set; } // byte
    [FlatBufferItem(15)] public byte EXPGrowth { get; set; } // byte
    [FlatBufferItem(16)] public byte EvoStage { get; set; } // byte
    [FlatBufferItem(17)] public byte CatchRate { get; set; } // byte
    [FlatBufferItem(18)] public byte Field_18 { get; set; } // Always Default (0)
    [FlatBufferItem(19)] public byte Color { get; set; } // byte
    [FlatBufferItem(20)] public ushort Height { get; set; } // ushort
    [FlatBufferItem(21)] public ushort Weight { get; set; } // ushort
    [FlatBufferItem(22)] public uint TM_A { get; set; } // uint, not used by game
    [FlatBufferItem(23)] public uint TM_B { get; set; } // uint, not used by game
    [FlatBufferItem(24)] public uint TM_C { get; set; } // uint, not used by game
    [FlatBufferItem(25)] public uint TM_D { get; set; } // uint, not used by game
    [FlatBufferItem(26)] public uint TR_A { get; set; } // uint, not used by game
    [FlatBufferItem(27)] public uint TR_B { get; set; } // uint, not used by game
    [FlatBufferItem(28)] public uint TR_C { get; set; } // uint, not used by game
    [FlatBufferItem(29)] public uint TR_D { get; set; } // uint, not used by game
    [FlatBufferItem(30)] public uint TypeTutor { get; set; } // uint, not used by game
    [FlatBufferItem(31)] public ushort BaseEXP { get; set; } // ushort
    [FlatBufferItem(32)] public byte EV_HP { get; set; } // byte
    [FlatBufferItem(33)] public byte EV_ATK { get; set; } // byte
    [FlatBufferItem(34)] public byte EV_DEF { get; set; } // byte
    [FlatBufferItem(35)] public byte EV_SPA { get; set; } // byte
    [FlatBufferItem(36)] public byte EV_SPD { get; set; } // byte
    [FlatBufferItem(37)] public byte EV_SPE { get; set; } // byte
    [FlatBufferItem(38)] public ushort Item1 { get; set; } // ushort
    [FlatBufferItem(39)] public ushort Item2 { get; set; } // ushort
    [FlatBufferItem(40)] public ushort Item3 { get; set; } // Always Default (0)
    [FlatBufferItem(41)] public byte EggGroup1 { get; set; } // byte
    [FlatBufferItem(42)] public byte EggGroup2 { get; set; } // byte
    [FlatBufferItem(43)] public ushort HatchSpecies { get; set; } // ushort
    [FlatBufferItem(44)] public ushort LocalFormIndex { get; set; } // ushort
    [FlatBufferItem(45)] public bool Field_45 { get; set; } // byte
    [FlatBufferItem(46)] public ushort Field_46 { get; set; } // ushort
    [FlatBufferItem(47)] public byte Field_47 { get; set; } // byte
    [FlatBufferItem(48)] public byte BaseFriendship { get; set; } // byte
    [FlatBufferItem(49)] public ushort DexIndexHisui { get; set; } // ushort
    [FlatBufferItem(50)] public ushort DexIndexOther { get; set; } // ushort
    [FlatBufferItem(51)] public ushort DexIndexLocal1 { get; set; } // uint
    [FlatBufferItem(52)] public ushort DexIndexLocal2 { get; set; } // uint
    [FlatBufferItem(53)] public ushort DexIndexLocal3 { get; set; } // uint
    [FlatBufferItem(54)] public ushort DexIndexLocal4 { get; set; } // uint
    [FlatBufferItem(55)] public ushort DexIndexLocal5 { get; set; } // uint
    [FlatBufferItem(56)] public uint MoveShop1 { get; set; } // uint
    [FlatBufferItem(57)] public uint MoveShop2 { get; set; } // uint
}
