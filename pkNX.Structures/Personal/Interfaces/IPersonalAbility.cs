using System;

namespace pkNX.Structures;

public interface IPersonalAbility
{
    int Ability1 { get; set; }
    int Ability2 { get; set; }
    int AbilityH { get; set; }
}

public static class IPersonalAbilityExtensions
{
    /// <summary>
    /// Gets the index of the <see cref="abilityID"/> within the specification's list of abilities.
    /// </summary>
    /// <param name="a"></param>
    /// <param name="abilityID">Ability ID</param>
    /// <returns>Ability Index or -1 if not found</returns>
    public static int GetIndexOfAbility(this IPersonalAbility a, int abilityID) => abilityID == a.Ability1 ? 0 : abilityID == a.Ability2 ? 1 : abilityID == a.AbilityH ? 2 : -1;

    public static bool GetIsAbility12Same(this IPersonalAbility pi) => pi.Ability1 == pi.Ability2;
    public static bool GetIsAbilityHiddenUnique(this IPersonalAbility pi) => pi.Ability1 != pi.AbilityH;
    public static bool GetIsAbilityPatchPossible(this IPersonalAbility pi) => pi.Ability1 != pi.AbilityH || pi.Ability2 != pi.AbilityH;

    public static void GetAbilities(this IPersonalAbility a, Span<int> result)
    {
        result[0] = a.Ability1;
        result[1] = a.Ability2;
        result[2] = a.AbilityH;
    }

    public static void SetAbilities(this IPersonalAbility a, Span<int> result)
    {
        a.Ability1 = result[0];
        a.Ability2 = result[1];
        a.AbilityH = result[2];
    }

    /// <summary>
    /// Gets the requested Ability value with the requested <see cref="index"/>.
    /// </summary>
    public static int GetAbilityAtIndex(this IPersonalAbility a, int index) => index switch
    {
        0 => a.Ability1,
        1 => a.Ability2,
        2 => a.AbilityH,
        _ => throw new ArgumentOutOfRangeException(nameof(index)),
    };

    /// <summary>
    /// Sets the requested Ability value with the requested <see cref="index"/>.
    /// </summary>
    public static int SetAbilityAtIndex(this IPersonalAbility a, int index, int value) => index switch
    {
        0 => a.Ability1 = value,
        1 => a.Ability2 = value,
        2 => a.AbilityH = value,
        _ => throw new ArgumentOutOfRangeException(nameof(index))
    };

    /// <summary>
    /// Gets the total number of abilities available.
    /// </summary>
    /// <remarks>Duplicate abilities still count separately.</remarks>
    public static int GetNumAbilities(this IPersonalAbility _) => 3;

    public static void SetIPersonalAbility(this IPersonalAbility self, IPersonalAbility other)
    {
        for (int j = 0; j < other.GetNumAbilities(); ++j)
            self.SetAbilityAtIndex(j, other.GetAbilityAtIndex(j));
    }
}
