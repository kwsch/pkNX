using System;
using System.IO;
using pkNX.Structures;

namespace pkNX.Game
{
    /// <summary>
    /// Environment Data where the Game's Data is located on the user's machine.
    /// </summary>
    public sealed class GameLocation
    {
        /// <summary>
        /// Location of the RomFS
        /// </summary>
        public string RomFS { get; }

        /// <summary>
        /// Location of the ExeFS.
        /// </summary>
        public string ExeFS { get; }

        /// <summary>
        /// Game version the files belong to.
        /// </summary>
        public GameVersion Game { get; }

        private GameLocation(string romfs, string exefs, GameVersion game)
        {
            Game = game;
            RomFS = romfs;
            ExeFS = exefs;
        }

        /// <summary>
        /// Determines the <see cref="GameVersion"/> of the input directory and detects the location of files for editing.
        /// </summary>
        /// <param name="dir">Directory the game data is in</param>
        /// <returns>New <see cref="GameLocation"/> object with references to file paths.</returns>
        public static GameLocation GetGame(string dir)
        {
            if (dir == null || !Directory.Exists(dir))
                return null;

            var dirs = Directory.GetDirectories(dir);
            var romfs = Array.Find(dirs, z => Path.GetFileName(z).StartsWith("rom", StringComparison.CurrentCultureIgnoreCase));
            var exefs = Array.Find(dirs, z => Path.GetFileName(z).StartsWith("exe", StringComparison.CurrentCultureIgnoreCase));

            if (romfs == null || exefs == null)
                return null;

            var files = Directory.GetFiles(romfs, "*", SearchOption.AllDirectories);
            var game = GetGameFromCount(files.Length, romfs, exefs);
            if (game == GameVersion.Invalid)
                return null;
            return new GameLocation(romfs, exefs, game);
        }

        private const int FILECOUNT_XY = 271;
        private const int FILECOUNT_ORASDEMO = 301;
        private const int FILECOUNT_ORAS = 299;
        private const int FILECOUNT_SMDEMO = 239;
        private const int FILECOUNT_SM = 311;
        private const int FILECOUNT_USUM = 333;
        private const int FILECOUNT_GG = 27818;
        private const int FILECOUNT_SWSH = 41702;
        private const int FILECOUNT_SWSH_1 = 41951; // Ver. 1.1.0 update (Galarian Slowpoke)
        private const int FILECOUNT_SWSH_2 = 46867; // Ver. 1.2.0 update (Isle of Armor)
        private const int FILECOUNT_SWSH_3 = 50494; // Ver. 1.3.0 update (Crown Tundra)

        private static GameVersion GetGameFromCount(int fileCount, string romfs, string exefs)
        {
            switch (fileCount)
            {
                case FILECOUNT_XY: return GameVersion.XY;
                case FILECOUNT_ORASDEMO: return GameVersion.ORASDEMO;
                case FILECOUNT_ORAS: return GameVersion.ORAS;
                case FILECOUNT_SMDEMO: return GameVersion.SMDEMO;
                case FILECOUNT_SM:
                {
                    var encdata = Path.Combine(romfs, "a", "0", "8", "2");
                    if (File.Exists(encdata) && new FileInfo(encdata).Length != 0)
                        return GameVersion.SN;
                    return GameVersion.MN;
                }
                case FILECOUNT_USUM:
                {
                    var encdata = Path.Combine(romfs, "a", "0", "8", "2");
                    if (File.Exists(encdata) && new FileInfo(encdata).Length != 0)
                        return GameVersion.US;
                    return GameVersion.UM;
                }
                case FILECOUNT_GG:
                    return GameVersion.GG;

                case FILECOUNT_SWSH:
                case FILECOUNT_SWSH_1:
                case FILECOUNT_SWSH_2:
                    return GameVersion.SW; // todo: differentiate between SW/SH
                case FILECOUNT_SWSH_3:
                {
                    // Unlike SM and USUM, the file structure between these two games are identical,
                    // so we identify the game by the size of the main binary.
                    var main = Path.Combine(exefs, "main");
                    if (new FileInfo(main).Length is 21163058 or 21163221) // 1.3.0 vs 1.3.1
                        return GameVersion.SW;
                    return GameVersion.SH;
                }
                default:
                    return GameVersion.Invalid;
            }
        }
    }
}
