using System;

namespace pkNX.Structures;

public abstract class StatPKM : IPokeData
{
    public abstract int Species { get; set; }
    public abstract int Form { get; set; }
    public abstract int Level { get; set; }
    public abstract int Nature { get; set; }

    public abstract int HeldItem { get; set; }
    public abstract int Gender { get; set; }
    public abstract int Ability { get; set; }

    public abstract int IV_HP { get; set; }
    public abstract int IV_ATK { get; set; }
    public abstract int IV_DEF { get; set; }
    public abstract int IV_SPA { get; set; }
    public abstract int IV_SPD { get; set; }
    public abstract int IV_SPE { get; set; }

    public abstract int EV_HP { get; set; }
    public abstract int EV_ATK { get; set; }
    public abstract int EV_DEF { get; set; }
    public abstract int EV_SPA { get; set; }
    public abstract int EV_SPD { get; set; }
    public abstract int EV_SPE { get; set; }

    public virtual ushort[] GetStats(IPersonalInfo p)
    {
        ushort[] Stats = new ushort[6];
        Stats[0] = (ushort)(((IV_HP + (2 * p.HP) + (EV_HP / 4) + 100) * Level / 100) + 10);
        Stats[1] = (ushort)(((IV_ATK + (2 * p.ATK) + (EV_ATK / 4)) * Level / 100) + 5);
        Stats[2] = (ushort)(((IV_DEF + (2 * p.DEF) + (EV_DEF / 4)) * Level / 100) + 5);
        Stats[4] = (ushort)(((IV_SPA + (2 * p.SPA) + (EV_SPA / 4)) * Level / 100) + 5);
        Stats[5] = (ushort)(((IV_SPD + (2 * p.SPD) + (EV_SPD / 4)) * Level / 100) + 5);
        Stats[3] = (ushort)(((IV_SPE + (2 * p.SPE) + (EV_SPE / 4)) * Level / 100) + 5);
        if (p.HP == 1)
            Stats[0] = 1;

        // Account for nature
        int incr = (Nature / 5) + 1;
        int decr = (Nature % 5) + 1;
        if (incr != decr)
        {
            Stats[incr] *= 11;
            Stats[incr] /= 10;
            Stats[decr] *= 9;
            Stats[decr] /= 10;
        }

        return Stats;
    }

    public int HiddenPowerType => 15 * ((IV_HP & 1) + (2 * (IV_ATK & 1)) + (4 * (IV_DEF & 1)) + (8 * (IV_SPE & 1)) + (16 * (IV_SPA & 1)) + (32 * (IV_SPD & 1))) / 63;

    public int GetIV(int index)
    {
        return index switch
        {
            0 => IV_HP,
            1 => IV_ATK,
            2 => IV_DEF,
            3 => IV_SPE,
            4 => IV_SPA,
            5 => IV_SPD,
            _ => throw new ArgumentOutOfRangeException(nameof(index))
        };
    }

    public void SetIV(int index, int value)
    {
        switch (index)
        {
            case 0: IV_HP = value; break;
            case 1: IV_ATK = value; break;
            case 2: IV_DEF = value; break;
            case 3: IV_SPE = value; break;
            case 4: IV_SPA = value; break;
            case 5: IV_SPD = value; break;
            default:
                throw new ArgumentOutOfRangeException(nameof(index));
        }
    }

    public int GetEV(int index)
    {
        return index switch
        {
            0 => EV_HP,
            1 => EV_ATK,
            2 => EV_DEF,
            3 => EV_SPE,
            4 => EV_SPA,
            5 => EV_SPD,
            _ => throw new ArgumentOutOfRangeException(nameof(index))
        };
    }

    public void SetEV(int index, int value)
    {
        switch (index)
        {
            case 0: EV_HP = value; break;
            case 1: EV_ATK = value; break;
            case 2: EV_DEF = value; break;
            case 3: EV_SPE = value; break;
            case 4: EV_SPA = value; break;
            case 5: EV_SPD = value; break;
            default:
                throw new ArgumentOutOfRangeException(nameof(index));
        }
    }

    public void SetHPIVs(int type)
    {
        for (int i = 0; i < 6; i++)
        {
            var val = (GetIV(i) & 0x1E) + hpivs[type, i];
            SetIV(i, val);
        }
    }

    private static readonly int[,] hpivs = {
        { 1, 1, 0, 0, 0, 0 }, // Fighting
        { 0, 0, 0, 0, 0, 1 }, // Flying
        { 1, 1, 0, 0, 0, 1 }, // Poison
        { 1, 1, 1, 0, 0, 1 }, // Ground
        { 1, 1, 0, 1, 0, 0 }, // Rock
        { 1, 0, 0, 1, 0, 1 }, // Bug
        { 1, 0, 1, 1, 0, 1 }, // Ghost
        { 1, 1, 1, 1, 0, 1 }, // Steel
        { 1, 0, 1, 0, 1, 0 }, // Fire
        { 1, 0, 0, 0, 1, 1 }, // Water
        { 1, 0, 1, 0, 1, 1 }, // Grass
        { 1, 1, 1, 0, 1, 1 }, // Electric
        { 1, 0, 1, 1, 1, 0 }, // Psychic
        { 1, 0, 0, 1, 1, 1 }, // Ice
        { 1, 0, 1, 1, 1, 1 }, // Dragon
        { 1, 1, 1, 1, 1, 1 }, // Dark
    };
}
