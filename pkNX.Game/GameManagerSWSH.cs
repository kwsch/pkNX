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
        public string TitleID => ActualGame == GameVersion.SW ? Sword : Shield;
        private const string Sword = "0100ABF008968000";
        private const string Shield = "01008DB008C2C000";

        protected override void SetMitm()
        {
            var basePath = Path.GetDirectoryName(ROM.RomFS);
            // todo var eeveevidpath = Path.Combine(ROM.RomFS, Path.Combine("bin", "movies", "EEVEE_GO"));
            bool shield = false; // todo Directory.Exists(eeveevidpath);
            ActualGame = shield ? GameVersion.SH : GameVersion.SW;
            var redirect = Path.Combine(basePath, TitleID);
            FileMitm.SetRedirect(basePath, redirect);
        }

        private FakeContainer Learn;

        protected override void Initialize()
        {
            // initialize gametext
            ResetText();

            // initialize common structures
            var personal = GetFilteredFolder(GameFile.PersonalStats, z => Path.GetFileNameWithoutExtension(z) == "personal_total");
            var learn = this[GameFile.Learnsets][0];
            var splitLearn = learn.Split(0x104);
            Learn = new FakeContainer(splitLearn);
            Data = new GameData
            {
                MoveData = new DataCache<Move>(this[GameFile.MoveStats]) // mini
                {
                    Create = z => new Move7(z),
                    Write = z => z.Write(),
                },
                LevelUpData = new DataCache<Learnset>(Learn)
                {
                    Create = z => new Learnset8(z),
                    Write = z => z.Write(),
                },

                // folders
                PersonalData = new PersonalTable(personal[0], Game),
                EvolutionData = new DataCache<EvolutionSet>(GetFilteredFolder(GameFile.Evolutions))
                {
                    Create = data => new EvolutionSet8(data),
                    Write = evo => evo.Write(),
                },
            };
        }

        public void ResetText()
        {
            GetFilteredFolder(GameFile.GameText, z => Path.GetExtension(z) == ".dat");
        }

        protected override void Terminate()
        {
            // Store Personal Data back in the file. Let the container detect if it is modified.
            var personal = this[GameFile.PersonalStats];
            personal[0] = Data.PersonalData.Table.SelectMany(z => z.Write()).ToArray();
            var learn = this[GameFile.Learnsets];
            learn[0] = Learn.Files.SelectMany(z => z).ToArray();
        }
    }
}