using System;
using System.IO;
using System.Linq;
using pkNX.Containers;
using pkNX.Structures;
using pkNX.Structures.FlatBuffers;
using pkNX.Structures.FlatBuffers.SWSH;

namespace pkNX.Game;

public class GameManagerSWSH(GameLocation rom, int language) : GameManager(rom, language)
{
    private string npdmPath => Path.Combine(PathExeFS, "main.npdm");
    private string TitleID => BitConverter.ToUInt64(File.ReadAllBytes(npdmPath), 0x290).ToString("X16");

    /// <summary>
    /// Generally useful game data that can be used by multiple editors.
    /// </summary>
    public GameData Data { get; protected set; } = null!;

    private FakeContainer Learn = null!;

    protected override void SetMitm()
    {
        var basePath = Path.GetDirectoryName(ROM.RomFS);
        ArgumentNullException.ThrowIfNull(basePath);
        var tid = ROM.ExeFS != null ? TitleID : "0100ABF008968000"; // no way to differentiate without exefs, so default to Sword
        var redirect = Path.Combine(basePath, tid);
        FileMitm.SetRedirect(basePath, redirect);
    }

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
                Create = FlatBufferConverter.DeserializeFrom<Waza>,
                Write = z => ((Waza)z).SerializeFrom(),
            },
            LevelUpData = new DataCache<Learnset>(Learn)
            {
                Create = z => new Learnset8(z.Span),
                Write = z => z.Write(),
            },

            // folders
            PersonalData = new PersonalTable8SWSH(personal[0]),
            EvolutionData = new DataCache<EvolutionSet>(GetFilteredFolder(GameFile.Evolutions))
            {
                Create = data => new EvolutionSet8(data.Span),
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
        personal[0] = Data.PersonalData.Table.SelectMany(z => ((IPersonalInfoBin)z).Write()).ToArray();
        var learn = this[GameFile.Learnsets];
        learn[0] = Learn.Files.SelectMany(z => z).ToArray();
    }
}
