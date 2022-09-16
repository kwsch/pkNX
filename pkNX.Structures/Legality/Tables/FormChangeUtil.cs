using System;
using System.Collections.Generic;

namespace pkNX.Structures;

/// <summary>
/// Option for checking how a move may be learned.
/// </summary>
public enum LearnOption
{
    /// <summary>
    /// Checks with rules assuming the move is in the current moveset.
    /// </summary>
    Current,

    /// <summary>
    /// Checks with rules assuming the move was known at any time while existing inside the game source it is being checked in.
    /// </summary>
    /// <remarks>
    /// Only relevant for memory checks.
    /// For not-transferable moves like Gen4/5 HM moves, no -- there's no point in checking them as they aren't requisites for anything.
    /// Evolution criteria is handled separately.
    /// </remarks>
    AtAnyTime,
}

/// <summary>
/// Logic for checking if an entity can freely change <see cref="PKM.Form"/>.
/// </summary>
public static class FormChangeUtil
{
    /// <summary>
    /// Checks if all forms should be iterated when checking for moves.
    /// </summary>
    /// <param name="species">Entity species</param>
    /// <param name="form">Entity form</param>
    /// <param name="generation">Generation we're checking in</param>
    /// <param name="option">Conditions we're checking with</param>
    public static bool ShouldIterateForms(ushort species, byte form, int generation, LearnOption option)
    {
        if (option is not LearnOption.Current)
            return FormChangeMoves.TryGetValue(species, out var func) && func((generation, form));
        return IterateAllForms(species);
    }

    private static bool IterateAllForms(ushort species) => FormChangeMovesRetain.Contains(species);

    /// <summary>
    /// Species that can change between their forms and get access to form-specific moves.
    /// </summary>
    private static readonly HashSet<ushort> FormChangeMovesRetain = new()
    {
        (int)Species.Deoxys,
        (int)Species.Giratina,
        (int)Species.Shaymin,
        (int)Species.Hoopa,
    };

    /// <summary>
    /// Species that can change between their forms and get access to form-specific moves.
    /// </summary>
    private static readonly Dictionary<ushort, Func<(int Generation, int Form), bool>> FormChangeMoves = new()
    {
        {(int)Species.Deoxys,   g => g.Generation >= 6},
        {(int)Species.Giratina, g => g.Generation >= 6},
        {(int)Species.Shaymin,  g => g.Generation >= 6},
        {(int)Species.Rotom,    g => g.Generation >= 6},
        {(int)Species.Hoopa,    g => g.Generation >= 6},
        {(int)Species.Tornadus, g => g.Generation >= 6},
        {(int)Species.Thundurus,g => g.Generation >= 6},
        {(int)Species.Landorus, g => g.Generation >= 6},
        {(int)Species.Urshifu,  g => g.Generation >= 8},
        {(int)Species.Enamorus, g => g.Generation >= 8},
        // Fuse
        {(int)Species.Kyurem,   g => g.Generation >= 6},
        {(int)Species.Necrozma, g => g.Generation >= 8},
        {(int)Species.Calyrex,  g => g.Generation >= 8},

        {(int)Species.Pikachu,  g => g.Generation == 6},
    };
}
