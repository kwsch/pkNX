using System;

namespace pkNX.Structures;

/// <summary>
/// Exposes details about base stat values.
/// </summary>
public interface IBaseStat
{
    /// <summary>
    /// Base HP
    /// </summary>
    int HP { get; set; }

    /// <summary>
    /// Base Attack
    /// </summary>
    int ATK { get; set; }

    /// <summary>
    /// Base Defense
    /// </summary>
    int DEF { get; set; }

    /// <summary>
    /// Base Special Attack
    /// </summary>
    int SPA { get; set; }

    /// <summary>
    /// Base Special Defense
    /// </summary>
    int SPD { get; set; }

    /// <summary>
    /// Base Speed
    /// </summary>
    int SPE { get; set; }
}

public static class IBaseStatExtensions
{
    /// <summary>
    /// Base Stat Total sum of all stats.
    /// </summary>
    public static int GetBaseStatTotal(this IBaseStat stats) => stats.HP + stats.ATK + stats.DEF + stats.SPA + stats.SPD + stats.SPE;

    /// <summary>
    /// Gets the requested Base Stat value with the requested <see cref="index"/>.
    /// </summary>
    public static int GetBaseStatValue(this IBaseStat stats, int index) => index switch
    {
        0 => stats.HP,
        1 => stats.ATK,
        2 => stats.DEF,
        3 => stats.SPE,
        4 => stats.SPA,
        5 => stats.SPD,
        _ => throw new ArgumentOutOfRangeException(nameof(index)),
    };

    /// <summary>
    /// Sets the requested Base Stat value with the requested <see cref="index"/>.
    /// </summary>
    public static void SetBaseStatValue(this IBaseStat stats, int index, int value)
    {
        switch (index)
        {
            case 0: stats.HP = value; return;
            case 1: stats.ATK = value; return;
            case 2: stats.DEF = value; return;
            case 3: stats.SPE = value; return;
            case 4: stats.SPA = value; return;
            case 5: stats.SPD = value; return;
            default: throw new ArgumentOutOfRangeException(nameof(index));
        };
    }

    /// <summary>
    /// Gets the total number of base stats available.
    /// </summary>
    public static int GetNumBaseStats(this IBaseStat _) => 6;

    public static void GetSortedStatIndexes(this IBaseStat pi, Span<(int Index, int Stat)> span)
    {
        for (int i = 0; i < span.Length; i++)
            span[i] = (i, pi.GetBaseStatValue(i));

        // Bubble sort based off Stat value
        // Higher stat values go to lower indexes in the span.
        for (int i = 0; i < span.Length - 1; i++)
        {
            for (int j = 0; j < span.Length - 1 - i; j++)
            {
                if (span[j].Stat < span[j + 1].Stat)
                    (span[j], span[j + 1]) = (span[j + 1], span[j]);
            }
        }
    }

    public static void SetIBaseStats(this IBaseStat self, IBaseStat other)
    {
        for (int j = 0; j < other.GetNumBaseStats(); ++j)
            self.SetBaseStatValue(j, other.GetBaseStatValue(j));
    }
}
