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
        public static readonly GameVersion[] GameVersions = ((GameVersion[])Enum.GetValues(typeof(GameVersion))).Where(z => z < RB && z > 0).Reverse().ToArray();

        /// <summary>
        /// Indicates if the <see cref="GameVersion"/> value is a value used by the games or is an aggregate indicator.
        /// </summary>
        /// <param name="game">Game to check</param>
        public static bool IsValidSavedVersion(this GameVersion game) => 0 < game && game <= RB;

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
            if (Gen7.Contains(game))
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

            if (g1 == g2 || g1 == Any)
                return true;

            switch (g1)
            {
                case RB:
                    return g2 == RD || g2 == BU || g2 == GN;
                case Stadium:
                case EventsGBGen1:
                case VCEvents:
                case RBY:
                    return RB.Contains(g2) || g2 == YW;
                case Gen1:
                    return RBY.Contains(g2) || g2 == Stadium || g2 == EventsGBGen1 || g2 == VCEvents;

                case GS: return g2 == GD || g2 == SV;
                case Stadium2:
                case EventsGBGen2:
                case GSC:
                    return GS.Contains(g2) || g2 == C;
                case Gen2:
                    return GSC.Contains(g2) || g2 == Stadium2 || g2 == EventsGBGen2;
                case GBCartEraOnly:
                    return g2 == Stadium || g2 == Stadium2 || g2 == EventsGBGen1 || g2 == EventsGBGen2;

                case RS: return g2 == R || g2 == S;
                case RSE:
                    return RS.Contains(g2) || g2 == E;
                case FRLG: return g2 == FR || g2 == LG;
                case COLO:
                case XD: return g2 == CXD;
                case CXD: return g2 == COLO || g2 == XD;
                case RSBOX: return RS.Contains(g2) || g2 == E || FRLG.Contains(g2);
                case Gen3:
                    return RSE.Contains(g2) || FRLG.Contains(g2) || CXD.Contains(g2) || g2 == RSBOX;

                case DP: return g2 == D || g2 == P;
                case HGSS: return g2 == HG || g2 == SS;
                case DPPt:
                    return DP.Contains(g2) || g2 == Pt;
                case BATREV: return DP.Contains(g2) || g2 == Pt || HGSS.Contains(g2);
                case Gen4:
                    return DPPt.Contains(g2) || HGSS.Contains(g2) || g2 == BATREV;

                case BW: return g2 == B || g2 == W;
                case B2W2: return g2 == B2 || g2 == W2;
                case Gen5:
                    return BW.Contains(g2) || B2W2.Contains(g2);

                case XY: return g2 == X || g2 == Y;
                case ORAS: return g2 == OR || g2 == AS;
                case Gen6:
                    return XY.Contains(g2) || ORAS.Contains(g2);

                case SM:
                    return g2 == SN || g2 == MN;
                case USUM:
                    return g2 == US || g2 == UM;
                case GG:
                    return g2 == GP || g2 == GE || g2 == GO;
                case Gen7:
                    return SM.Contains(g2) || USUM.Contains(g2) || GG.Contains(g2);

                case Gen8:
                case SWSH:
                    return g2 == SW || g2 == SH;

                default: return false;
            }
        }

        /// <summary>
        /// List of possible <see cref="GameVersion"/> values within the provided <see cref="generation"/>.
        /// </summary>
        /// <param name="generation">Generation to look within</param>
        public static GameVersion[] GetVersionsInGeneration(int generation) => GameVersions.Where(z => z.GetGeneration() == generation).ToArray();
    }
}
