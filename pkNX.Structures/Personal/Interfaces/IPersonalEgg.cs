namespace pkNX.Structures;

public interface IPersonalEgg
{
    /// <summary>
    /// First Egg Group
    /// </summary>
    int EggGroup1 { get; set; }

    /// <summary>
    /// Second Egg Group
    /// </summary>
    int EggGroup2 { get; set; }
}

public interface IPersonalEgg_1 : IPersonalEgg
{
    /// <summary>
    /// Amount of Hatching Step Cycles required to hatch if in an egg.
    /// </summary>
    int HatchCycles { get; set; }
}

/// <summary>
/// PLA seems to be missing `HatchCycles` and adds `HatchedSpecies`
/// </summary>
public interface IPersonalEgg_2 : IPersonalEgg
{
    ushort HatchedSpecies { get; set; }
}

public static class PersonalEggExtensions
{
    /// <summary>
    /// Checks if the entry has either egg group equal to the input type.
    /// </summary>
    /// <param name="pi">Object reference</param>
    /// <param name="group">Egg group</param>
    /// <returns>Egg is present in entry</returns>
    public static bool IsEggGroup(this IPersonalEgg pi, int group) => pi.EggGroup1 == group || pi.EggGroup2 == group;
}
