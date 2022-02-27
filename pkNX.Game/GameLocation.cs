using System;
using System.IO;
using System.Linq;
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

            if (romfs == null && exefs == null)
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
        private const int FILECOUNT_LA = 18_370;
        private const int FILECOUNT_LA_01 = 18_371; // Ver. 1.0.1 (Day 1 Patch)
        private const int FILECOUNT_LA_10 = 19_095; // Ver. 1.1.0 (Daybreak)

        private static GameVersion GetGameFromCount(int fileCount, string romfs, string exefs)
        {
            string GetTitleID() => BitConverter.ToString(File.ReadAllBytes(Path.Combine(exefs, "main.npdm")).Skip(0x290).Take(0x08).Reverse().ToArray()).Replace("-", "");

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
                {
                    bool eevee = Directory.Exists(Path.Combine(romfs, "bin", "movies", "EEVEE_GO"));
                    if (eevee)
                        return GameVersion.GE;
                    return GameVersion.GP;
                }

                case FILECOUNT_SWSH:
                case FILECOUNT_SWSH_1:
                case FILECOUNT_SWSH_2:
                case FILECOUNT_SWSH_3:
                {
                    if (exefs == null)
                        return GameVersion.SWSH;
                    return GetTitleID() == "0100ABF008968000" ? GameVersion.SW : GameVersion.SH;
                }

                case FILECOUNT_LA or FILECOUNT_LA_01 or FILECOUNT_LA_10:
                    return GameVersion.PLA;

                default:
                    return GameVersion.Invalid;
            }
        }
    }
}
