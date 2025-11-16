using static pkNX.Structures.GameVersion;

namespace pkNX.Structures;

/// <summary>
/// Utility class for <see cref="GameVersion"/> logic.
/// </summary>
public static class GameUtil
{
    /// <summary>
    /// Gets a Version ID from the end of that Generation
    /// </summary>
    /// <param name="generation">Generation ID</param>
    /// <returns>Version ID from requested generation. If none, return <see cref="Invalid"/>.</returns>
    public static GameVersion GetVersion(int generation) => generation switch
    {
        1 => RBY,
        2 => C,
        3 => E,
        4 => SS,
        5 => W2,
        6 => AS,
        7 => UM,
        8 => SH,
        9 => VL,
        _ => Invalid,
    };

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
        if (Gen8.Contains(game)) return 8;
        if (Gen9.Contains(game)) return 9;
        return -1;
    }

    /// <summary>
    /// Gets the Generation the <see cref="GameVersion"/> belongs to.
    /// </summary>
    /// <param name="game">Game to retrieve the generation for</param>
    /// <returns>Generation ID</returns>
    public static int GetMaxSpeciesID(this GameVersion game)
    {
        if (Gen1.Contains(game)) return 151;
        if (Gen2.Contains(game)) return 251;
        if (Gen3.Contains(game)) return 384;
        if (Gen4.Contains(game)) return 493;
        if (Gen5.Contains(game)) return 649;
        if (Gen6.Contains(game)) return Legal.MaxSpeciesID_6;
        if (Gen7.Contains(game) || Gen7b.Contains(game))
        {
            if (SM.Contains(game))
                return 802;
            if (USUM.Contains(game))
                return 807;
            return Legal.MaxSpeciesID_7_GG;
        }
        if (Gen8.Contains(game))
        {
            if (SWSH.Contains(game))
                return Legal.MaxSpeciesID_8;
            return Legal.MaxSpeciesID_8a;
        }

        if (game is ZA)
            return Legal.MaxSpeciesID_9a;

        if (Gen9.Contains(game))
        {
            return Legal.MaxSpeciesID_9;
        }
        return -1;
    }

    /// <summary>
    /// Checks if the <see cref="g1"/> version (or subset versions) is equivalent to <see cref="g2"/>.
    /// </summary>
    /// <param name="g1">Version (set)</param>
    /// <param name="g2">Individual version</param>
    public static bool Contains(this GameVersion g1, int g2) => g1.Contains((GameVersion)g2);

    /// <summary>
    /// Checks if the <see cref="g1"/> version (or subset versions) is equivalent to <see cref="g2"/>.
    /// </summary>
    /// <param name="g1">Version (set)</param>
    /// <param name="g2">Individual version</param>
    public static bool Contains(this GameVersion g1, GameVersion g2)
    {
        if (g1 == g2 || g1 == Any)
            return true;

        return g1 switch
        {
            RB => g2 is RD or BU or GN,
            RBY or Stadium => RB.Contains(g2) || g2 == YW,
            Gen1 => RBY.Contains(g2) || g2 == Stadium,

            GS => g2 is GD or SV,
            GSC or Stadium2 => GS.Contains(g2) || g2 == C,
            Gen2 => GSC.Contains(g2) || g2 == Stadium2,

            RS => g2 is R or S,
            RSE => RS.Contains(g2) || g2 == E,
            FRLG => g2 is FR or LG,
            COLO or XD => g2 == CXD,
            CXD => g2 is COLO or XD,
            RSBOX => RS.Contains(g2) || g2 == E || FRLG.Contains(g2),
            Gen3 => RSE.Contains(g2) || FRLG.Contains(g2) || CXD.Contains(g2) || g2 == RSBOX,

            DP => g2 is D or P,
            HGSS => g2 is HG or SS,
            DPPt => DP.Contains(g2) || g2 == Pt,
            BATREV => DP.Contains(g2) || g2 == Pt || HGSS.Contains(g2),
            Gen4 => DPPt.Contains(g2) || HGSS.Contains(g2) || g2 == BATREV,

            BW => g2 is B or W,
            B2W2 => g2 is B2 or W2,
            Gen5 => BW.Contains(g2) || B2W2.Contains(g2),

            XY => g2 is X or Y,
            ORAS => g2 is OR or AS,

            Gen6 => XY.Contains(g2) || ORAS.Contains(g2),
            SM => g2 is SN or MN,
            USUM => g2 is US or UM,
            GG => g2 is GP or GE,
            Gen7 => SM.Contains(g2) || USUM.Contains(g2),
            Gen7b => GG.Contains(g2) || GO == g2,

            SWSH => g2 is SW or SH,
            PLA => g2 is PLA,
            Gen8 => SWSH.Contains(g2) || PLA.Contains(g2),

            SV => g2 is SL or VL,
            ZA => g2 is ZA,
            Gen9 => SV.Contains(g2) || ZA.Contains(g2),

            _ => false,
        };
    }
}
