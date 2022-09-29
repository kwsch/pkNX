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
/// SWSH still has `HatchCycles` but adds `HatchedSpecies`
/// </summary>
public interface IPersonalEgg_2 : IPersonalEgg
{
    /// <summary>
    /// Amount of Hatching Step Cycles required to hatch if in an egg.
    /// </summary>
    int HatchCycles { get; set; }

    /// <summary>
    /// Species index that will be hatched from a egg of this species
    /// </summary>
    ushort HatchedSpecies { get; set; }
}

/// <summary>
/// PLA seems to be missing `HatchCycles`
/// </summary>
public interface IPersonalEgg_3 : IPersonalEgg
{
    /// <summary>
    /// Species index that will be hatched from a egg of this species
    /// </summary>
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

    public static void SetIPersonalEgg(this IPersonalEgg self, IPersonalEgg other)
    {
        self.EggGroup1 = other.EggGroup1;
        self.EggGroup2 = other.EggGroup2;

        if (self is IPersonalEgg_1 self_1 && other is IPersonalEgg_1 other_1)
        {
            self_1.HatchCycles = other_1.HatchCycles;
        }

        if (self is IPersonalEgg_2 self_2 && other is IPersonalEgg_2 other_2)
        {
            self_2.HatchCycles = other_2.HatchCycles;
            self_2.HatchedSpecies = other_2.HatchedSpecies;
        }

        if (self is IPersonalEgg_3 self_3 && other is IPersonalEgg_3 other_3)
        {
            self_3.HatchedSpecies = other_3.HatchedSpecies;
        }
    }
}
