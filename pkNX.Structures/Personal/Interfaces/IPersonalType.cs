namespace pkNX.Structures;

/// <summary>
/// Exposes info about <see cref="Types"/> that an entity has.
/// </summary>
public interface IPersonalType
{
    /// <summary>
    /// Primary Type
    /// </summary>
    Types Type1 { get; set; }

    /// <summary>
    /// Secondary Type
    /// </summary>
    Types Type2 { get; set; }
}

public static class PersonalTypeExtensions
{
    /// <summary>
    /// Checks if the entry has either type equal to the input type.
    /// </summary>
    /// <param name="detail">Object reference</param>
    /// <param name="type">Type</param>
    /// <returns>Typing is present in entry</returns>
    public static bool IsType(this IPersonalType detail, Types type)
    {
        return detail.Type1 == type || detail.Type2 == type;
    }

    /// <summary>
    /// Checks if the entry has either type equal to both input types.
    /// </summary>
    /// <remarks>Input order does not matter.</remarks>
    /// <param name="detail">Object reference</param>
    /// <param name="type1">Type 1</param>
    /// <param name="type2">Type 2</param>
    /// <returns>Typing is present in entry</returns>
    public static bool IsType(this IPersonalType detail, Types type1, Types type2)
    {
        return (detail.Type1 == type1 && detail.Type2 == type2) || (detail.Type1 == type2 && detail.Type2 == type1);
    }

    /// <summary>
    /// Checks to see if the provided Types match the entry's types.
    /// </summary>
    /// <remarks>Input order matters! If input order does not matter, use <see cref="IsType(IPersonalType, Types, Types)"/> instead.</remarks>
    /// <param name="detail">Object reference</param>
    /// <param name="type1">First type</param>
    /// <param name="type2">Second type</param>
    /// <returns>Typing is an exact match</returns>
    public static bool IsValidTypeCombination(this IPersonalType detail, Types type1, Types type2) => detail.Type1 == type1 && detail.Type2 == type2;

    public static void ImportIPersonalType(this IPersonalType self, IPersonalType other)
    {
        self.Type1 = other.Type1;
        self.Type2 = other.Type2;
    }
}
