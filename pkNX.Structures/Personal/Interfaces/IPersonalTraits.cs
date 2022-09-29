namespace pkNX.Structures;

/// <summary>
/// Exposes info about personal trait details that an entity has.
/// </summary>
public interface IPersonalTraits
{
    /// <summary>
    /// Gender Ratio value determining if the entry is a fixed gender or bi-gendered.
    /// </summary>
    int Gender { get; set; }

    /// <summary>
    /// Experience-Level Growth Rate type
    /// </summary>
    int EXPGrowth { get; set; }

    /// <summary>
    /// Base Experience Yield factor
    /// </summary>
    int BaseEXP { get; set; }

    /// <summary>
    /// Catch Rate
    /// </summary>
    int CatchRate { get; set; }

    /// <summary>
    /// Initial Friendship when captured or received.
    /// </summary>
    int BaseFriendship { get; set; }

    /// <summary>
    /// Escape factor used for fleeing the Safari Zone or calling for help in SOS Battles.
    /// </summary>
    int EscapeRate { get; set; }

    /// <summary>
    /// Main color ID of the entry. The majority of the Pokémon's color is of this color, usually.
    /// </summary>
    int Color { get; set; }

    /// <summary>
    /// Height of the entry in meters (m).
    /// </summary>
    int Height { get; set; }

    /// <summary>
    /// Mass of the entry in kilograms (kg).
    /// </summary>
    int Weight { get; set; }
}

public enum FixedGenderType
{
    DualGender = -1,
    OnlyMale = 0,
    OnlyFemale = 1,
    Genderless = 2,
}

public static class PersonalTraitsExtensions
{
    public const int RatioMagicGenderless = 255;
    public const int RatioMagicFemale = 254;
    public const int RatioMagicMale = 0;

    public static FixedGenderType GetFixedGenderType(this IPersonalTraits info)
    {
        if (info.Gender == RatioMagicGenderless)
            return FixedGenderType.Genderless;
        if (info.Gender == RatioMagicFemale)
            return FixedGenderType.OnlyFemale;
        if (info.Gender == RatioMagicMale)
            return FixedGenderType.OnlyMale;
        return FixedGenderType.DualGender;
    }

    public static bool IsSingleGender(this IPersonalTraits info) => info.GetFixedGenderType() != FixedGenderType.DualGender;
    public static bool IsDualGender(this IPersonalTraits info) => info.GetFixedGenderType() == FixedGenderType.DualGender;
    public static bool IsGenderless(this IPersonalTraits info) => info.GetFixedGenderType() == FixedGenderType.Genderless;
    public static bool IsOnlyFemale(this IPersonalTraits info) => info.GetFixedGenderType() == FixedGenderType.OnlyFemale;
    public static bool IsOnlyMale(this IPersonalTraits info) => info.GetFixedGenderType() == FixedGenderType.OnlyMale;

    /// <summary>
    /// Gets a random valid gender for the entry.
    /// </summary>
    public static int RandomGender(this IPersonalTraits info)
    {
        var fix = info.GetFixedGenderType();
        return fix >= 0 ? (int)fix : Util.Rand.Next(2);
    }

    public static void SetIPersonalTraits(this IPersonalTraits self, IPersonalTraits other)
    {
        self.Gender = other.Gender;
        self.EXPGrowth = other.EXPGrowth;
        self.BaseEXP = other.BaseEXP;
        self.CatchRate = other.CatchRate;
        self.EscapeRate = other.EscapeRate;
        self.BaseFriendship = other.BaseFriendship;
        self.EscapeRate = other.EscapeRate;
        self.Color = other.Color;
        self.Height = other.Height;
        self.Weight = other.Weight;
    }
}