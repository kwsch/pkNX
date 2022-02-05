using System;
using System.Linq;
using static pkNX.Structures.GameVersion;

namespace pkNX.Structures
{
    /// <summary>
    /// Utility class for <see cref="GameVersion"/> logic.
    /// </summary>
    public static class GameUtil
    {
        /// <summary>
        /// List of possible <see cref="GameVersion"/> values that are stored in PKM data.
        /// </summary>
        /// <remarks>Ordered roughly by most recent games first.</remarks>
        public static readonly GameVersion[] GameVersions = ((GameVersion[])Enum.GetValues(typeof(GameVersion))).Where(z => z is < RB and > 0).Reverse().ToArray();

        /// <summary>
        /// Indicates if the <see cref="GameVersion"/> value is a value used by the games or is an aggregate indicator.
        /// </summary>
        /// <param name="game">Game to check</param>
        public static bool IsValidSavedVersion(this GameVersion game) => game is > 0 and <= RB;

        /// <summary>Determines the Version Grouping of an input Version ID</summary>
        /// <param name="Version">Version of which to determine the group</param>
        /// <returns>Version Group Identifier or Invalid if type cannot be determined.</returns>
        public static GameVersion GetMetLocationVersionGroup(GameVersion Version)
        {
            return Version switch
            {
                // Sidegame
                CXD => CXD,
                GO => GO,
                // Gen1
                RBY => RBY,
                RD => RBY,
                BU => RBY,
                YW => RBY,
                GN => RBY,
                // Gen2
                GS => GSC,
                GD => GSC,
                SV => GSC,
                C => GSC,
                // Gen3
                R => RS,
                S => RS,
                E => E,
                FR => FR,
                LG => FR,
                // Gen4
                D => DP,
                P => DP,
                Pt => Pt,
                HG => HGSS,
                SS => HGSS,
                // Gen5
                B => BW,
                W => BW,
                B2 => B2W2,
                W2 => B2W2,
                // Gen6
                X => XY,
                Y => XY,
                OR => ORAS,
                AS => ORAS,
                // Gen7
                SN => SM,
                MN => SM,
                US => USUM,
                UM => USUM,
                GP => GG,
                GE => GG,
                _ => Invalid
            };
        }

        /// <summary>
        /// Gets a Version ID from the end of that Generation
        /// </summary>
        /// <param name="generation">Generation ID</param>
        /// <returns>Version ID from requested generation. If none, return <see cref="Invalid"/>.</returns>
        public static GameVersion GetVersion(int generation)
        {
            return generation switch
            {
                1 => RBY,
                2 => C,
                3 => E,
                4 => SS,
                5 => W2,
                6 => AS,
                7 => UM,
                _ => Invalid
            };
        }

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
            if (Gen8.Contains(game)) return Legal.MaxSpeciesID_8;
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
                Gen8 => SWSH.Contains(g2) || PLA == g2,
                _ => false,
            };
        }

        /// <summary>
        /// List of possible <see cref="GameVersion"/> values within the provided <see cref="generation"/>.
        /// </summary>
        /// <param name="generation">Generation to look within</param>
        public static GameVersion[] GetVersionsInGeneration(int generation) => GameVersions.Where(z => z.GetGeneration() == generation).ToArray();
    }
}
