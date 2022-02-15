using System;
using System.IO;
using System.Linq;
using pkNX.Containers;
using pkNX.Structures;
using pkNX.Structures.FlatBuffers;

namespace pkNX.Game
{
    public class GameManagerPLA : GameManager
    {
        public GameManagerPLA(GameLocation rom, int language) : base(rom, language) { }
        private string PathNPDM => Path.Combine(PathExeFS, "main.npdm");
        private string TitleID => BitConverter.ToUInt64(File.ReadAllBytes(PathNPDM), 0x470).ToString("X16");

        /// <summary>
        /// Generally useful game data that can be used by multiple editors.
        /// </summary>
        public GameData8a Data { get; protected set; }

        protected override void SetMitm()
        {
            var basePath = Path.GetDirectoryName(ROM.RomFS);
            var tid = ROM.ExeFS != null ? TitleID : "arceus";
            var redirect = Path.Combine(basePath, tid);
            FileMitm.SetRedirect(basePath, redirect);
        }

        public override void Initialize()
        {
            base.Initialize();

            // initialize gametext
            ResetText();

            // initialize common structures
            ResetData();
        }

        private void ResetData()
        {
            var pbin = this[GameFile.PersonalStats][0];
            var personal = FlatBufferConverter.DeserializeFrom<PersonalTableLA>(pbin);
            var evos = this[GameFile.Evolutions][0];
            var evoTable = FlatBufferConverter.DeserializeFrom<EvolutionTable8>(evos);
            var learn = this[GameFile.Learnsets][0];
            var learnTable = FlatBufferConverter.DeserializeFrom<Learnset8a>(learn);

            Data = new GameData8a
            {
                // Folders
                MoveData = GetMoves(),

                // Custom
                PersonalData = new PersonalTable(personal.Table.Select(z => new PersonalInfoLA(z)).ToArray(), 905),

                // Single Files
                LevelUpData = new DataCache<Learnset8aMeta>(learnTable.Table),
                EvolutionData = new DataCache<EvolutionSet8a>(evoTable.Table),
            };
        }

        private DataCache<Waza8a> GetMoves()
        {
            var move = this[GameFile.MoveStats];
            ((FolderContainer)move).Initialize();
            return new DataCache<Waza8a>(move)
            {
                Create = FlatBufferConverter.DeserializeFrom<Waza8a>,
                Write = FlatBufferConverter.SerializeFrom,
            };
        }

        public void ResetMoves() => Data.MoveData.ClearAll();

        public void ResetText()
        {
            GetFilteredFolder(GameFile.GameText, z => Path.GetExtension(z) == ".dat");
        }

        protected override void Terminate()
        {
            // Store Personal Data back in the file. Let the container detect if it is modified.
            var personal = this[GameFile.PersonalStats];
            personal[0] = FlatBufferConverter.SerializeFrom(new PersonalTableLA { Table = Data.PersonalData.Table.Cast<PersonalInfoLA>().Select(z => z.FB).ToArray() } );
            var learn = this[GameFile.Learnsets];
            learn[0] = FlatBufferConverter.SerializeFrom(new Learnset8a { Table = Data.LevelUpData.LoadAll() });
            var evos = this[GameFile.Evolutions];
            evos[0] = FlatBufferConverter.SerializeFrom(new EvolutionTable8 { Table = Data.EvolutionData.LoadAll() });
        }
    }
}