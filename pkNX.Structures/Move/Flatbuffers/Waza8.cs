namespace pkNX.Structures
{
    public class Waza8
    {
        public uint Version { get; set; }
        public uint MoveID { get; set; }
        public bool CanUseMove { get; set; }
        public byte Type { get; set; }
        public byte Quality { get; set; }
        public byte Category { get; set; }
        public byte Power { get; set; }
        public byte Accuracy { get; set; }
        public byte PP { get; set; }
        public byte Priority { get; set; }
        public byte HitMin { get; set; }
        public byte HitMax { get; set; }
        public ushort Inflict { get; set; }
        public byte InflictPercent { get; set; }
        public byte RawInflictCount { get; set; }
        public byte TurnMin { get; set; }
        public byte TurnMax { get; set; }
        public byte CritStage { get; set; }
        public byte Flinch { get; set; }
        public ushort EffectSequence { get; set; }
        public byte Recoil { get; set; }
        public byte RawHealing { get; set; }
        public byte RawTarget { get; set; }
        public byte Stat1 { get; set; }
        public byte Stat2 { get; set; }
        public byte Stat3 { get; set; }
        public byte Stat1Stage { get; set; }
        public byte Stat2Stage { get; set; }
        public byte Stat3Stage { get; set; }
        public byte Stat1Percent { get; set; }
        public byte Stat2Percent { get; set; }
        public byte Stat3Percent { get; set; }
        public byte GigantimaxPower { get; set; }
        public bool Flag_MakesContact { get; set; }
        public bool Flag_Charge { get; set; }
        public bool Flag_Recharge { get; set; }
        public bool Flag_Protect { get; set; }
        public bool Flag_Reflectable { get; set; }
        public bool Flag_Snatch { get; set; }
        public bool Flag_Mirror { get; set; }
        public bool Flag_Punch { get; set; }
        public bool Flag_Sound { get; set; }
        public bool Flag_Gravity { get; set; }
        public bool Flag_Defrost { get; set; }
        public bool Flag_DistanceTriple { get; set; }
        public bool Flag_Heal { get; set; }
        public bool Flag_IgnoreSubstitute { get; set; }
        public bool Flag_FailSkyBattle { get; set; }
        public bool Flag_AnimateAlly { get; set; }
        public bool Flag_Dance { get; set; }
        public bool Flag_18 { get; set; }

        public MoveInflictDuration InflictCount
        {
            get => (MoveInflictDuration)RawInflictCount;
            set => RawInflictCount = (byte)value;
        }

        public Heal Healing
        {
            get => (Heal)RawHealing;
            set => RawHealing = (byte)value;
        }

        public MoveTarget Target
        {
            get => (MoveTarget)RawTarget;
            set => RawTarget = (byte)value;
        }
    }
}
