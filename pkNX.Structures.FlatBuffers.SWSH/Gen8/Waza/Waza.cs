namespace pkNX.Structures.FlatBuffers.SWSH
{
    public partial class Waza : IMove
    {
        public byte[] Write() => this.SerializeFrom();

        public MoveInflictDuration InflictCount { get => (MoveInflictDuration)RawInflictCount; set => RawInflictCount = (byte)value; }
        public Heal Healing { get => (Heal)RawHealing; set => RawHealing = (sbyte)value; }
        public MoveTarget Target { get => (MoveTarget)RawTarget; set => RawTarget = (byte)value; }

        int IMove.Type { get => Type ; set => Type = (byte)value; }
        int IMove.Quality { get => Quality ; set => Quality = (byte)value; }
        int IMove.Category { get => Category ; set => Category = (byte)value; }
        int IMove.Power { get => Power ; set => Power = (byte)value; }
        int IMove.Accuracy { get => Accuracy ; set => Accuracy = (byte)value; }
        int IMove.PP { get => PP ; set => PP = (byte)value; }
        int IMove.Priority { get => Priority; set => Priority= (sbyte)value; }
        int IMove.HitMin { get => HitMin; set => HitMin = (byte)value; }
        int IMove.HitMax { get => HitMax; set => HitMax = (byte)value; }
        int IMove.Inflict { get => Inflict; set => Inflict = (byte)value; }
        int IMove.InflictPercent { get => InflictPercent; set => InflictPercent = (byte)value; }
        int IMove.TurnMin { get => TurnMin; set => TurnMin = (byte)value; }
        int IMove.TurnMax { get => TurnMax; set => TurnMax = (byte)value; }
        int IMove.CritStage { get => CritStage; set => CritStage = (sbyte)value; }
        int IMove.Flinch { get => Flinch; set => Flinch = (byte)value; }
        int IMove.EffectSequence { get => EffectSequence; set => EffectSequence = (byte)value; }
        int IMove.Recoil { get => Recoil; set => Recoil = (sbyte)value; }
        int IMove.Stat1 { get => Stat1; set => Stat1 = (byte)value; }
        int IMove.Stat2 { get => Stat2; set => Stat2 = (byte)value; }
        int IMove.Stat3 { get => Stat3; set => Stat3 = (byte)value; }
        int IMove.Stat1Stage { get => Stat1Stage; set => Stat1Stage = (sbyte)value; }
        int IMove.Stat2Stage { get => Stat2Stage; set => Stat2Stage = (sbyte)value; }
        int IMove.Stat3Stage { get => Stat3Stage; set => Stat3Stage = (sbyte)value; }
        int IMove.Stat1Percent { get => Stat1Percent; set => Stat1Percent = (byte)value; }
        int IMove.Stat2Percent { get => Stat2Percent; set => Stat2Percent = (byte)value; }
        int IMove.Stat3Percent { get => Stat3Percent; set => Stat3Percent = (byte)value; }
    }
}
