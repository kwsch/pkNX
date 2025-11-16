using System;

namespace pkNX.Structures;

public abstract class TrainerData(Memory<byte> raw)
{
    public abstract int SIZE { get; }
    protected Span<byte> Data => raw.Span;

    public abstract int Class { get; set; }
    public abstract BattleMode Mode { get; set; }
    public abstract int NumPokemon { get; set; }
    public abstract int Item1 { get; set; }
    public abstract int Item2 { get; set; }
    public abstract int Item3 { get; set; }
    public abstract int Item4 { get; set; }

    public abstract uint AI { get; set; }
    public abstract bool Heal { get; set; }
    public abstract int Money { get; set; }
    public abstract int Gift { get; set; }

    // derived
    public bool HasAllyTrainer => (AI & 8) != 0;

    public byte[] Write() => Data.ToArray();
}
