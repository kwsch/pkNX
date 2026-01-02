using static pkNX.Structures.GameVersion;

namespace pkNX.Structures;

/// <summary>
/// Utility class for <see cref="GameVersion"/> logic.
/// </summary>
public static class GameUtil
{
    /// <summary>
    /// Gets the Generation the <see cref="GameVersion"/> belongs to.
    /// </summary>
    /// <param name="game">Game to retrieve the generation for</param>
    /// <returns>Generation ID</returns>
    public static int GetGeneration(this GameVersion game)
    {
        if (Gen1.Contains(game)) return 1;
        if (Gen2.Contains(game)) return 2;
        if (Gen3.Contains(game)) return 3;
        if (Gen4.Contains(game)) return 4;
        if (Gen5.Contains(game)) return 5;
        if (Gen6.Contains(game)) return 6;
        if (Gen7.Contains(game)) return 7;
        if (Gen7b.Contains(game)) return 7;
        if (SWSH.Contains(game)) return 8;
        if (game is PLA) return 8;
        if (BDSP.Contains(game)) return 8;
        if (SV.Contains(game)) return 9;
        if (game is ZA) return 9;
        return -1;
    }

    /// <summary>
    /// Checks if the <see cref="version"/> version (or subset versions) is equivalent to <see cref="g2"/>.
    /// </summary>
    /// <param name="version">Version (set)</param>
    /// <param name="g2">Individual version</param>
    public static bool Contains(this GameVersion version, GameVersion g2)
    {
        if (version == g2 || version == Any)
            return true;
        if (version.IsValidSavedVersion())
            return false;
        return version.ContainsFromLumped(g2);
    }

    /// <summary>
    /// Most recent game ID utilized by official games.
    /// </summary>
    internal const GameVersion HighestGameID = RB - 1;

    /// <summary>
    /// Indicates if the <see cref="GameVersion"/> value is a value used by the games or is an aggregate indicator.
    /// </summary>
    public static bool IsValidSavedVersion(this GameVersion version) => version is > 0 and <= HighestGameID;

    /// <summary>
    /// Checks if the <see cref="g1"/> version (or subset versions) is equivalent to <see cref="version1"/>.
    /// </summary>
    /// <param name="g1">Version (set)</param>
    /// <param name="version1">Individual version</param>
    public static bool ContainsFromLumped(this GameVersion g1, GameVersion version1) => g1 switch
    {
        RB => version1 is RD or BU or GN,
        RBY => version1 is RD or BU or GN or YW or RB,
        Stadium => version1 is RD or BU or GN or YW or RB or RBY,
        StadiumJ => version1 is RD or BU or GN or YW or RB or RBY,
        Gen1 => version1 is RD or BU or GN or YW or RB or RBY or Stadium,

        GS => version1 is GD or SI,
        GSC => version1 is GD or SI or C or GS,
        Stadium2 => version1 is GD or SI or C or GS or GSC,
        Gen2 => version1 is GD or SI or C or GS or GSC or Stadium2,

        RS => version1 is R or S,
        RSE => version1 is R or S or E or RS,
        FRLG => version1 is FR or LG,
        RSBOX => version1 is R or S or E or FR or LG,
        Gen3 => version1 is R or S or E or FR or LG or CXD or RSBOX or RS or RSE or FRLG,
        COLO => version1 is CXD,
        XD => version1 is CXD,

        DP => version1 is D or P,
        HGSS => version1 is HG or SS,
        DPPt => version1 is D or P or Pt or DP,
        Gen4 => version1 is D or P or Pt or HG or SS or BATREV or DP or HGSS or DPPt,

        BW => version1 is B or W,
        B2W2 => version1 is B2 or W2,
        Gen5 => version1 is B or W or B2 or W2 or BW or B2W2,

        XY => version1 is X or Y,
        ORAS => version1 is OR or AS,
        Gen6 => version1 is X or Y or OR or AS or XY or ORAS,

        SM => version1 is SN or MN,
        USUM => version1 is US or UM,
        Gen7 => version1 is SN or MN or US or UM or SM or USUM,

        GG => version1 is GP or GE,
        Gen7b => version1 is GP or GE or GO or GG,

        SWSH => version1 is SW or SH,
        BDSP => version1 is BD or SP,
        Gen8 => version1 is SW or SH or BD or SP or SWSH or BDSP or PLA,

        SV => version1 is SL or VL,
        Gen9 => version1 is SL or VL or SV or ZA,

        _ => false,
    };
}
