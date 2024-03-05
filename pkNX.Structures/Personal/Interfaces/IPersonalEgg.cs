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

public interface IPersonalEgg_v1 : IPersonalEgg
{
    /// <summary>
    /// Amount of Hatching Step Cycles required to hatch if in an egg.
    /// </summary>
    byte HatchCycles { get; set; }
}

/// <summary>
/// Since SWSH `HatchedSpecies` was added
/// </summary>
public interface IPersonalEgg_SWSH : IPersonalEgg_v1
{
    /// <summary>
    /// Species index that will be hatched from a egg of this species
    /// </summary>
    ushort HatchedSpecies { get; set; }
}

public interface IPersonalEgg_PLA : IPersonalEgg_SWSH;

public static class PersonalEggExtensions
{
    /// <summary>
    /// Checks if the entry has either egg group equal to the input type.
    /// </summary>
    /// <param name="pi">Object reference</param>
    /// <param name="group">Egg group</param>
    /// <returns>Egg is present in entry</returns>
    public static bool IsEggGroup(this IPersonalEgg pi, int group) => pi.EggGroup1 == group || pi.EggGroup2 == group;

    public static void ImportIPersonalEgg(this IPersonalEgg self, IPersonalEgg other)
    {
        self.EggGroup1 = other.EggGroup1;
        self.EggGroup2 = other.EggGroup2;

        if (self is IPersonalEgg_v1 self_1 && other is IPersonalEgg_v1 other_1)
        {
            self_1.HatchCycles = other_1.HatchCycles;
        }

        if (self is IPersonalEgg_SWSH self_2 && other is IPersonalEgg_SWSH other_2)
        {
            self_2.HatchedSpecies = other_2.HatchedSpecies;
        }
    }
}
