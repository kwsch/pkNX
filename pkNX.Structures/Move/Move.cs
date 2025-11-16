using System;

namespace pkNX.Structures;

public abstract class Move3DS(Memory<byte> raw) : IMove
{
    protected Span<byte> Data => raw.Span;
    protected abstract int SIZE { get; }

    public byte[] Write() => Data.ToArray();

    public abstract int Type { get; set; }
    public abstract int Quality { get; set; }
    public abstract int Category { get; set; }
    public abstract int Power { get; set; }
    public abstract int Accuracy { get; set; }
    public abstract int PP { get; set; }
    public abstract int Priority { get; set; }
    public abstract int HitMin { get; set; }
    public abstract int HitMax { get; set; }
    public abstract int Inflict { get; set; }
    public abstract int InflictPercent { get; set; }
    public abstract MoveInflictDuration InflictCount { get; set; }
    public abstract int TurnMin { get; set; }
    public abstract int TurnMax { get; set; }
    public abstract int CritStage { get; set; }
    public abstract int Flinch { get; set; }
    public abstract int EffectSequence { get; set; }
    public abstract int Recoil { get; set; }
    public abstract Heal Healing { get; set; }
    public abstract MoveTarget Target { get; set; }
    public abstract int Stat1 { get; set; }
    public abstract int Stat2 { get; set; }
    public abstract int Stat3 { get; set; }
    public abstract int Stat1Stage { get; set; }
    public abstract int Stat2Stage { get; set; }
    public abstract int Stat3Stage { get; set; }
    public abstract int Stat1Percent { get; set; }
    public abstract int Stat2Percent { get; set; }
    public abstract int Stat3Percent { get; set; }
}

public interface IMove
{
    byte[] Write();

    public int Type { get; set; }
    public int Quality { get; set; }
    public int Category { get; set; }
    public int Power { get; set; }
    public int Accuracy { get; set; }
    public int PP { get; set; }
    public int Priority { get; set; }
    public int HitMin { get; set; }
    public int HitMax { get; set; }
    public int Inflict { get; set; }
    public int InflictPercent { get; set; }
    public MoveInflictDuration InflictCount { get; set; }
    public int TurnMin { get; set; }
    public int TurnMax { get; set; }
    public int CritStage { get; set; }
    public int Flinch { get; set; }
    public int EffectSequence { get; set; }
    public int Recoil { get; set; }
    public Heal Healing { get; set; }
    public MoveTarget Target { get; set; }
    public int Stat1 { get; set; }
    public int Stat2 { get; set; }
    public int Stat3 { get; set; }
    public int Stat1Stage { get; set; }
    public int Stat2Stage { get; set; }
    public int Stat3Stage { get; set; }
    public int Stat1Percent { get; set; }
    public int Stat2Percent { get; set; }
    public int Stat3Percent { get; set; }
}
