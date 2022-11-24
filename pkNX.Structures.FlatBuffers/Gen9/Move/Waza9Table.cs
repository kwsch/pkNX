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
public class Waza9Table : IFlatBufferArchive<Waza9>
{
    [FlatBufferItem(0)] public Waza9[] Table { get; set; } = Array.Empty<Waza9>();
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class Waza9
{
    public byte[] Write() => FlatBufferConverter.SerializeFrom(this);

    [FlatBufferItem(00)] public ushort MoveID { get; set; }
    [FlatBufferItem(01)] public bool CanUseMove { get; set; }
    [FlatBufferItem(02)] public byte Type { get; set; }
    [FlatBufferItem(03)] public byte Quality { get; set; }
    [FlatBufferItem(04)] public byte Category { get; set; }
    [FlatBufferItem(05)] public byte Power { get; set; }
    [FlatBufferItem(06)] public byte Accuracy { get; set; }
    [FlatBufferItem(07)] public byte PP { get; set; }
    [FlatBufferItem(08)] public sbyte Priority { get; set; }
    [FlatBufferItem(09)] public byte HitMax { get; set; }
    [FlatBufferItem(10)] public byte HitMin { get; set; }
    [FlatBufferItem(11)] public Waza9Inflict Inflict { get; set; } = new();
    [FlatBufferItem(12)] public byte CritStage { get; set; }
    [FlatBufferItem(13)] public byte Flinch { get; set; }
    [FlatBufferItem(14)] public ushort EffectSequence { get; set; }
    [FlatBufferItem(15)] public sbyte Recoil { get; set; }
    [FlatBufferItem(16)] public sbyte RawHealing { get; set; }
    [FlatBufferItem(17)] public byte RawTarget { get; set; }
    [FlatBufferItem(18)] public Waza9EntityStat StatAmps { get; set; } = new();
    [FlatBufferItem(19)] public Waza9Affinity Affinity { get; set; }
    [FlatBufferItem(20)] public bool Flag_MakesContact { get; set; }
    [FlatBufferItem(21)] public bool Flag_Charge { get; set; }
    [FlatBufferItem(22)] public bool Flag_Recharge { get; set; }
    [FlatBufferItem(23)] public bool Flag_Protect { get; set; }
    [FlatBufferItem(24)] public bool Flag_Reflectable { get; set; }
    [FlatBufferItem(25)] public bool Flag_Snatch { get; set; }
    [FlatBufferItem(26)] public bool Flag_Mirror { get; set; }
    [FlatBufferItem(27)] public bool Flag_Punch { get; set; }
    [FlatBufferItem(28)] public bool Flag_Sound { get; set; }
    [FlatBufferItem(29)] public bool Flag_Dance { get; set; }
    [FlatBufferItem(30)] public bool Flag_Gravity { get; set; }
    [FlatBufferItem(31)] public bool Flag_Defrost { get; set; }
    [FlatBufferItem(32)] public bool Flag_DistanceTriple { get; set; }
    [FlatBufferItem(33)] public bool Flag_Heal { get; set; }
    [FlatBufferItem(34)] public bool Flag_IgnoreSubstitute { get; set; }
    [FlatBufferItem(35)] public bool Flag_FailSkyBattle { get; set; }
    [FlatBufferItem(36)] public bool Flag_AnimateAlly { get; set; }
    [FlatBufferItem(37)] public bool Flag_Metronome { get; set; }
    [FlatBufferItem(38)] public bool Flag_FailEncore { get; set; }
    [FlatBufferItem(39)] public bool Flag_FailMeFirst { get; set; }
    [FlatBufferItem(40)] public bool Flag_FutureAttack { get; set; }
    [FlatBufferItem(41)] public bool Flag_Pressure { get; set; }
    [FlatBufferItem(42)] public bool Flag_Combo { get; set; }
    [FlatBufferItem(43)] public bool Flag_NoSleepTalk { get; set; }
    [FlatBufferItem(44)] public bool Flag_NoAssist { get; set; }
    [FlatBufferItem(45)] public bool Flag_FailCopycat { get; set; }
    [FlatBufferItem(46)] public bool Flag_FailMimic { get; set; }
    [FlatBufferItem(47)] public bool Flag_FailInstruct { get; set; }
    [FlatBufferItem(48)] public bool Flag_Powder { get; set; }
    [FlatBufferItem(49)] public bool Flag_Bite { get; set; }
    [FlatBufferItem(50)] public bool Flag_Bullet { get; set; }
    [FlatBufferItem(51)] public bool Flag_NoMultiHit { get; set; }
    [FlatBufferItem(52)] public bool Flag_NoEffectiveness { get; set; }
    [FlatBufferItem(53)] public bool Flag_SheerForce { get; set; }
    [FlatBufferItem(54)] public bool Flag_Slicing { get; set; }
    [FlatBufferItem(55)] public bool Flag_Wind { get; set; }
    [FlatBufferItem(56)] public bool Unknown_56 { get; set; }
    [FlatBufferItem(57)] public bool Unknown_57 { get; set; }
    [FlatBufferItem(58)] public bool Unknown_58 { get; set; }
    [FlatBufferItem(59)] public bool Unknown_59 { get; set; }
    [FlatBufferItem(60)] public bool Unknown_60 { get; set; }
    [FlatBufferItem(61)] public bool Unused_61 { get; set; }
    [FlatBufferItem(62)] public bool Unused_62 { get; set; }
    [FlatBufferItem(63)] public bool Unused_63 { get; set; }
    [FlatBufferItem(64)] public bool Unused_64 { get; set; }
    [FlatBufferItem(65)] public bool Unused_65 { get; set; }
    [FlatBufferItem(66)] public bool Unused_66 { get; set; }
    [FlatBufferItem(67)] public bool Unused_67 { get; set; }
    [FlatBufferItem(68)] public bool Unused_68 { get; set; }
    [FlatBufferItem(69)] public bool Unused_69 { get; set; }
    [FlatBufferItem(70)] public bool Unused_70 { get; set; }
    [FlatBufferItem(71)] public bool Flag_CantUseTwice { get; set; }
}
