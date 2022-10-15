using System;
using System.IO;
using System.Linq;
using pkNX.Containers;
using pkNX.Structures;
using pkNX.Structures.FlatBuffers;

namespace pkNX.Game;

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
        if (basePath is null)
            throw new InvalidDataException("Invalid RomFS path.");
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

        ItemConverter.ItemNames = GetStrings(TextName.ItemNames);
    }

    private void ResetData()
    {
        Data = new GameData8a
        {
            // Folders
            MoveData = GetMoves(),

            // Single Files
            PersonalData = new PersonalTable8LA(GetFile(GameFile.PersonalStats)),
            LevelUpData = new(GetFile(GameFile.Learnsets)),
            EvolutionData = new(GetFile(GameFile.Evolutions)),

            FieldDrops = new(GetFile(GameFile.FieldDrops)),
            BattleDrops = new(GetFile(GameFile.BattleDrops)),
            DexResearch = new(GetFile(GameFile.DexResearch)),
        };

        DropTableConverter.DropTableHashes = Data.FieldDrops.Table.Select(x => x.Hash).ToArray();
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
    }
}
