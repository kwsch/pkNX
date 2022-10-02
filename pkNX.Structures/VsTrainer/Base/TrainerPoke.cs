namespace pkNX.Structures;

public abstract class TrainerPoke : StatPKM, IMoveset
{
    protected byte[] Data;

    public abstract int Friendship { get; set; }

    public abstract bool Shiny { get; set; }
    public abstract bool CanMegaEvolve { get; set; }
    public abstract bool CanDynamax { get; set; }

    public abstract int Move1 { get; set; }
    public abstract int Move2 { get; set; }
    public abstract int Move3 { get; set; }
    public abstract int Move4 { get; set; }

    public abstract uint IV32 { get; set; }

    public abstract int Rank { get; set; }

    public byte[] Write() => (byte[])Data.Clone();
    public abstract TrainerPoke Clone();

    #region Derived

    public int[] Moves
    {
        get => new[] { Move1, Move2, Move3, Move4 };
        set { if (value?.Length != 4) return; Move1 = value[0]; Move2 = value[1]; Move3 = value[2]; Move4 = value[3]; }
    }

    public int[] IVs
    {
        get => new[] { IV_HP, IV_ATK, IV_DEF, IV_SPE, IV_SPA, IV_SPD };
        set
        {
            if (value?.Length != 6) return;
            IV_HP = value[0]; IV_ATK = value[1]; IV_DEF = value[2];
            IV_SPE = value[3]; IV_SPA = value[4]; IV_SPD = value[5];
        }
    }

    public int[] EVs
    {
        get => new[] { EV_HP, EV_ATK, EV_DEF, EV_SPE, EV_SPA, EV_SPD };
        set
        {
            if (value?.Length != 6) return;
            EV_HP = value[0]; EV_ATK = value[1]; EV_DEF = value[2];
            EV_SPE = value[3]; EV_SPA = value[4]; EV_SPD = value[5];
        }
    }

    #endregion
}
