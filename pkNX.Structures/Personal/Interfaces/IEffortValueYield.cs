using System;

namespace pkNX.Structures;

/// <summary>
/// Exposes details about yielding effort values on defeat/capture.
/// </summary>
public interface IEffortValueYield
{
    /// <summary>
    /// Amount of HP Effort Values to yield when defeating this entry.
    /// </summary>
    int EV_HP { get; set; }

    /// <summary>
    /// Amount of Attack Effort Values to yield when defeating this entry.
    /// </summary>
    int EV_ATK { get; set; }

    /// <summary>
    /// Amount of Defense Effort Values to yield when defeating this entry.
    /// </summary>
    int EV_DEF { get; set; }

    /// <summary>
    /// Amount of Speed Effort Values to yield when defeating this entry.
    /// </summary>
    int EV_SPE { get; set; }

    /// <summary>
    /// Amount of Special Attack Effort Values to yield when defeating this entry.
    /// </summary>
    int EV_SPA { get; set; }

    /// <summary>
    /// Amount of Special Defense Effort Values to yield when defeating this entry.
    /// </summary>
    int EV_SPD { get; set; }
}

public static class IIEffortValueYieldExtensions
{
    /// <summary>
    /// Gets the requested Base Stat value with the requested <see cref="index"/>.
    /// </summary>
    public static int GetEVYieldValue(this IEffortValueYield stats, int index) => index switch
    {
        0 => stats.EV_HP,
        1 => stats.EV_ATK,
        2 => stats.EV_DEF,
        3 => stats.EV_SPE,
        4 => stats.EV_SPA,
        5 => stats.EV_SPD,
        _ => throw new ArgumentOutOfRangeException(nameof(index)),
    };

    /// <summary>
    /// Sets the requested Base Stat value with the requested <see cref="index"/>.
    /// </summary>
    public static int SetEVYieldValue(this IEffortValueYield stats, int index, int value) => index switch
    {
        0 => stats.EV_HP = value,
        1 => stats.EV_ATK = value,
        2 => stats.EV_DEF = value,
        3 => stats.EV_SPE = value,
        4 => stats.EV_SPA = value,
        5 => stats.EV_SPD = value,
        _ => throw new ArgumentOutOfRangeException(nameof(index))
    };

    /// <summary>
    /// Gets the total number of base stats available.
    /// </summary>
    public static int GetNumEVs(this IEffortValueYield _) => 6;

    public static void ImportIEffortValueYield(this IEffortValueYield self, IEffortValueYield other)
    {
        for (int j = 0; j < other.GetNumEVs(); ++j)
            self.SetEVYieldValue(j, other.GetEVYieldValue(j));
    }
}
