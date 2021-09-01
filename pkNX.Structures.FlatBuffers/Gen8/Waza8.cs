﻿using FlatSharp.Attributes;
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global
#nullable disable

namespace pkNX.Structures.FlatBuffers
{
    [FlatBufferTable]
    public class Waza8 : IMove
    {
        public byte[] Write() => FlatBufferConverter.SerializeFrom(this);

        [FlatBufferItem(0)]  public uint   Version { get; set; }
        [FlatBufferItem(1)]  public uint   MoveID { get; set; }
        [FlatBufferItem(2)]  public bool   CanUseMove { get; set; }
        [FlatBufferItem(3)]  public byte   FType { get; set; }
        [FlatBufferItem(4)]  public byte   FQuality { get; set; }
        [FlatBufferItem(5)]  public byte   FCategory { get; set; }
        [FlatBufferItem(6)]  public byte   FPower { get; set; }
        [FlatBufferItem(7)]  public byte   FAccuracy { get; set; }
        [FlatBufferItem(8)]  public byte   FPP { get; set; }
        [FlatBufferItem(9)]  public byte   FPriority { get; set; }
        [FlatBufferItem(10)] public byte   FHitMin { get; set; }
        [FlatBufferItem(11)] public byte   FHitMax { get; set; }
        [FlatBufferItem(12)] public ushort FInflict { get; set; }
        [FlatBufferItem(13)] public byte   FInflictPercent { get; set; }
        [FlatBufferItem(14)] public byte   FRawInflictCount { get; set; }
        [FlatBufferItem(15)] public byte   FTurnMin { get; set; }
        [FlatBufferItem(16)] public byte   FTurnMax { get; set; }
        [FlatBufferItem(17)] public byte   FCritStage { get; set; }
        [FlatBufferItem(18)] public byte   FFlinch { get; set; }
        [FlatBufferItem(19)] public ushort FEffectSequence { get; set; }
        [FlatBufferItem(20)] public byte   FRecoil { get; set; }
        [FlatBufferItem(21)] public byte   FRawHealing { get; set; }
        [FlatBufferItem(22)] public byte   FRawTarget { get; set; }
        [FlatBufferItem(23)] public byte   FStat1 { get; set; }
        [FlatBufferItem(24)] public byte   FStat2 { get; set; }
        [FlatBufferItem(25)] public byte   FStat3 { get; set; }
        [FlatBufferItem(26)] public byte   FStat1Stage { get; set; }
        [FlatBufferItem(27)] public byte   FStat2Stage { get; set; }
        [FlatBufferItem(28)] public byte   FStat3Stage { get; set; }
        [FlatBufferItem(29)] public byte   FStat1Percent { get; set; }
        [FlatBufferItem(30)] public byte   FStat2Percent { get; set; }
        [FlatBufferItem(31)] public byte   FStat3Percent { get; set; }
        [FlatBufferItem(32)] public byte   GigantamaxPower { get; set; }
        [FlatBufferItem(33)] public bool   Flag_MakesContact { get; set; }
        [FlatBufferItem(34)] public bool   Flag_Charge { get; set; }
        [FlatBufferItem(35)] public bool   Flag_Recharge { get; set; }
        [FlatBufferItem(36)] public bool   Flag_Protect { get; set; }
        [FlatBufferItem(37)] public bool   Flag_Reflectable { get; set; }
        [FlatBufferItem(38)] public bool   Flag_Snatch { get; set; }
        [FlatBufferItem(39)] public bool   Flag_Mirror { get; set; }
        [FlatBufferItem(40)] public bool   Flag_Punch { get; set; }
        [FlatBufferItem(41)] public bool   Flag_Sound { get; set; }
        [FlatBufferItem(42)] public bool   Flag_Gravity { get; set; }
        [FlatBufferItem(43)] public bool   Flag_Defrost { get; set; }
        [FlatBufferItem(44)] public bool   Flag_DistanceTriple { get; set; }
        [FlatBufferItem(45)] public bool   Flag_Heal { get; set; }
        [FlatBufferItem(46)] public bool   Flag_IgnoreSubstitute { get; set; }
        [FlatBufferItem(47)] public bool   Flag_FailSkyBattle { get; set; }
        [FlatBufferItem(48)] public bool   Flag_AnimateAlly { get; set; }
        [FlatBufferItem(49)] public bool   Flag_Dance { get; set; }
        [FlatBufferItem(50)] public bool   Flag_Metronome { get; set; }

        public int Type           { get => FType; set => FType = (byte)value; }
        public int Quality        { get => FQuality        ; set => FQuality        = (byte)value; }
        public int Category       { get => FCategory       ; set => FCategory       = (byte)value; }
        public int Power          { get => FPower          ; set => FPower          = (byte)value; }
        public int Accuracy       { get => FAccuracy       ; set => FAccuracy       = (byte)value; }
        public int PP             { get => FPP             ; set => FPP             = (byte)value; }
        public int Priority       { get => FPriority       ; set => FPriority       = (byte)value; }
        public int HitMin         { get => FHitMin         ; set => FHitMin         = (byte)value; }
        public int HitMax         { get => FHitMax         ; set => FHitMax         = (byte)value; }
        public int Inflict        { get => FInflict        ; set => FInflict        = (ushort)value; }
        public int InflictPercent { get => FInflictPercent ; set => FInflictPercent = (byte)value; }
        public int TurnMin        { get => FTurnMin        ; set => FTurnMin        = (byte)value; }
        public int TurnMax        { get => FTurnMax        ; set => FTurnMax        = (byte)value; }
        public int CritStage      { get => FCritStage      ; set => FCritStage      = (byte)value; }
        public int Flinch         { get => FFlinch         ; set => FFlinch         = (byte)value; }
        public int EffectSequence { get => FEffectSequence ; set => FEffectSequence = (ushort)value; }
        public int Recoil         { get => FRecoil         ; set => FRecoil         = (byte)value; }
        public int Stat1          { get => FStat1          ; set => FStat1          = (byte)value; }
        public int Stat2          { get => FStat2          ; set => FStat2          = (byte)value; }
        public int Stat3          { get => FStat3          ; set => FStat3          = (byte)value; }
        public int Stat1Stage     { get => FStat1Stage     ; set => FStat1Stage     = (byte)value; }
        public int Stat2Stage     { get => FStat2Stage     ; set => FStat2Stage     = (byte)value; }
        public int Stat3Stage     { get => FStat3Stage     ; set => FStat3Stage     = (byte)value; }
        public int Stat1Percent   { get => FStat1Percent   ; set => FStat1Percent   = (byte)value; }
        public int Stat2Percent   { get => FStat2Percent   ; set => FStat2Percent   = (byte)value; }
        public int Stat3Percent   { get => FStat3Percent   ; set => FStat3Percent   = (byte)value; }

        public MoveInflictDuration InflictCount
        {
            get => (MoveInflictDuration)FRawInflictCount;
            set => FRawInflictCount = (byte)value;
        }

        public Heal Healing
        {
            get => (Heal)FRawHealing;
            set => FRawHealing = (byte)value;
        }

        public MoveTarget Target
        {
            get => (MoveTarget)FRawTarget;
            set => FRawTarget = (byte)value;
        }
    }
}