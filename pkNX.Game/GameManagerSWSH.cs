using System;
using System.IO;
using System.Linq;
using pkNX.Containers;
using pkNX.Structures;
using pkNX.Structures.FlatBuffers;

namespace pkNX.Game
{
    public class GameManagerSWSH : GameManager
    {
        public GameManagerSWSH(GameLocation rom, int language) : base(rom, language) { }
        private string GetTitleID() => BitConverter.ToString(File.ReadAllBytes(Path.Combine(PathExeFS, "main.npdm")).Skip(0x290).Take(0x08).Reverse().ToArray()).Replace("-", "");

        protected override void SetMitm()
        {
            var basePath = Path.GetDirectoryName(ROM.RomFS);
            var TitleID = PathExeFS != null ? GetTitleID() : "0100ABF008968000"; // no way to differentiate without exefs, so default to Sword
            var redirect = Path.Combine(basePath, TitleID);
            FileMitm.SetRedirect(basePath, redirect);
        }

        private FakeContainer Learn;

        public override void Initialize()
        {
            base.Initialize();

            // initialize gametext
            ResetText();

            // initialize common structures
            var personal = GetFilteredFolder(GameFile.PersonalStats, z => Path.GetFileNameWithoutExtension(z) == "personal_total");
            var learn = this[GameFile.Learnsets][0];
            var splitLearn = learn.Split(0x104);
            Learn = new FakeContainer(splitLearn);

            var move = this[GameFile.MoveStats];
            ((FolderContainer)move).Initialize();
            Data = new GameData
            {
                MoveData = new DataCache<IMove>(move)
                {
                    Create = FlatBufferConverter.DeserializeFrom<Waza8>,
                    Write = z => FlatBufferConverter.SerializeFrom((Waza8)z),
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

        public void ResetMoves() => GetFilteredFolder(GameFile.MoveStats);

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