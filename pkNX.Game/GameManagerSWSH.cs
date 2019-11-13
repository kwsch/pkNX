using System.IO;
using System.Linq;
using pkNX.Containers;
using pkNX.Structures;

namespace pkNX.Game
{
    public class GameManagerSWSH : GameManager
    {
        public GameManagerSWSH(GameLocation rom, int language) : base(rom, language) { }
        private GameVersion ActualGame;
        public string TitleID => ActualGame == GameVersion.GP ? Sword : Shield;
        private const string Sword = "0100ABF008968000";
        private const string Shield = "01008DB008C2C000";

        protected override void SetMitm()
        {
            var basePath = Path.GetDirectoryName(ROM.RomFS);
            var swshfilepath = Path.Combine(ROM.RomFS, Path.Combine("bin", "SWSH_File"));
            bool sword = Directory.Exists(swshfilepath);
            ActualGame = sword ? GameVersion.SW : GameVersion.SH;
            var redirect = Path.Combine(basePath, TitleID);
            // get sword vs shield
            FileMitm.SetRedirect(basePath, redirect);
        }

        protected override void Initialize()
        {
            // initialize gametext
            GetFilteredFolder(GameFile.GameText, z => Path.GetExtension(z) == ".dat");

            // initialize common structures
            var personal = GetFilteredFolder(GameFile.PersonalStats, z => Path.GetFileNameWithoutExtension(z) == "personal_total");
            Data = new GameData
            {
               // MoveData = new DataCache<Move>(this[GameFile.MoveStats]) // mini
                //{
                    //Create = z => new Move7(z),
                    //Write = z => z.Write(),
                //},
                //LevelUpData = new DataCache<Learnset>(this[GameFile.Learnsets]) // gfpak
                //{
                    //Create = z => new Learnset6(z),
                    //Write = z => z.Write(),
                //},

                // folders;
                PersonalData = new PersonalTable(personal[0], Game),
                //MegaEvolutionData = new DataCache<MegaEvolutionSet[]>(GetFilteredFolder(GameFile.MegaEvolutions))
                //{
                    //Create = MegaEvolutionSet.ReadArray,
                    //Write = MegaEvolutionSet.WriteArray,
                //},
                EvolutionData = new DataCache<EvolutionSet>(GetFilteredFolder(GameFile.Evolutions))
                {
                    Create = (data) => new EvolutionSet7(data),
                    Write = evo => evo.Write(),
                },
            };
        }

        protected override void Terminate()
        {
            // Store Personal Data back in the file. Let the container detect if it is modified.
            var personal = this[GameFile.PersonalStats];
            personal[0] = Data.PersonalData.Table.SelectMany(z => z.Write()).ToArray();
        }
    }
}