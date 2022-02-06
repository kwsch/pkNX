using System;
using System.IO;
using pkNX.Containers;
using pkNX.Structures.FlatBuffers;

namespace pkNX.Game
{
    public class GameManagerPLA : GameManager
    {
        public GameManagerPLA(GameLocation rom, int language) : base(rom, language) { }
        private string PathNPDM => Path.Combine(PathExeFS, "main.npdm");
        private string TitleID => BitConverter.ToUInt64(File.ReadAllBytes(PathNPDM), 0x470).ToString("X16");

        protected override void SetMitm()
        {
            var basePath = Path.GetDirectoryName(ROM.RomFS);
            var tid = ROM.ExeFS != null ? TitleID : "arceus";
            var redirect = Path.Combine(basePath, tid);
            FileMitm.SetRedirect(basePath, redirect);
        }

        private Learnset8a Learn;

        public override void Initialize()
        {
            base.Initialize();

            // initialize gametext
            ResetText();

            // initialize common structures
            var personal = GetFile(GameFile.PersonalStats)[0];
            var learn = this[GameFile.Learnsets][0];
            Learn = FlatBufferConverter.DeserializeFrom<Learnset8a>(learn);

            var move = this[GameFile.MoveStats];
            ((FolderContainer)move).Initialize();
            //Data = new GameData
            //{
            //    MoveData = new DataCache<IMove>(move)
            //    {
            //        Create = FlatBufferConverter.DeserializeFrom<Waza8a>,
            //        Write = z => FlatBufferConverter.SerializeFrom((Waza8a)z),
            //    },
            //    LevelUpData = new DataCache<Learnset>(Array.Empty<Learnset>())
            //    {
            //        Create = z => new Learnset8(z),
            //        Write = z => z.Write(),
            //    },
            //
            //    // folders
            //    PersonalData = new PersonalTable(personal, Game),
            //    EvolutionData = new DataCache<EvolutionSet>(GetFilteredFolder(GameFile.Evolutions))
            //    {
            //        Create = data => new EvolutionSet8(data),
            //        Write = evo => evo.Write(),
            //    },
            //};
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
            //personal[0] = Data.PersonalData.Table.SelectMany(z => z.Write()).ToArray();
            var learn = this[GameFile.Learnsets];
            //learn[0] = Learn.Entries.SelectMany(z => z.Write()).ToArray();
        }
    }
}