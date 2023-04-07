using System;
using System.IO;
using pkNX.Containers;
using pkNX.Structures.FlatBuffers.SV.Trinity;

namespace pkNX.Game;

public class GameManagerSV : GameManager, IFileInternal, IDisposable
{
    private readonly TrinityFileSystemManager Manager;
    private string PathNPDM => Path.Combine(PathExeFS, "main.npdm");
    private string TitleID => BitConverter.ToUInt64(File.ReadAllBytes(PathNPDM), 0x290).ToString("X16");

    public GameManagerSV(GameLocation rom, int language) : base(rom, language)
    {
        // TODO: Use GameFileMapping?

        // Open the trpfs
        var pathTrpfs = Path.Combine(PathRomFS, "arc/data.trpfs");
        var pathTrpfd = Path.Combine(PathRomFS, "arc/data.trpfd");
        Manager = new TrinityFileSystemManager(pathTrpfs, pathTrpfd);
    }

    public bool HasFile(string path) => Manager.HasFile(path);
    public bool HasFile(ulong hash) => Manager.HasFile(hash);
    public byte[] GetPackedFile(string path) => Manager.GetPackedFile(path);
    public byte[] GetPackedFile(ulong hash) => Manager.GetPackedFile(hash);

    /// <summary>
    /// Generally useful game data that can be used by multiple editors.
    /// </summary>
    public GameData Data { get; protected set; } = null!;

    protected override void SetMitm()
    {
        var basePath = Path.GetDirectoryName(ROM.RomFS);
        if (basePath is null)
            throw new InvalidDataException("Invalid ROMFS path.");
        var tid = ROM.ExeFS != null ? TitleID : "0100A3D008C5C000"; // no way to differentiate without exefs, so default to Scarlet
        var redirect = Path.Combine(basePath, tid);
        FileMitm.SetRedirect(basePath, redirect);
    }

    public override void Initialize()
    {
        base.Initialize();

        // initialize gametext
        //ResetText();

        // initialize common structures
        //var personal = GetFilteredFolder(GameFile.PersonalStats, z => Path.GetFileNameWithoutExtension(z) == "personal_total");
        //var learn = this[GameFile.Learnsets][0];
        //var splitLearn = learn.Split(0x104);
        //Learn = new FakeContainer(splitLearn);

        //var move = this[GameFile.MoveStats];
        //((FolderContainer)move).Initialize();
        Data = new GameData
        {
            //MoveData = new DataCache<IMove>(move)
            //{
            //    Create = FlatBufferConverter.DeserializeFrom<Waza8>,
            //    Write = z => FlatBufferConverter.SerializeFrom((Waza8)z),
            //},
            //LevelUpData = new DataCache<Learnset>(Learn)
            //{
            //    Create = z => new Learnset8(z),
            //    Write = z => z.Write(),
            //},

            // folders
            //PersonalData = new PersonalTable8SWSH(personal[0]),
            //EvolutionData = new DataCache<EvolutionSet>(GetFilteredFolder(GameFile.Evolutions))
            //{
            //    Create = data => new EvolutionSet8(data),
            //    Write = evo => evo.Write(),
            //},
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
        //var personal = this[GameFile.PersonalStats];
        //personal[0] = Data.PersonalData.Table.SelectMany(z => ((IPersonalInfoBin)z).Write()).ToArray();
        //var learn = this[GameFile.Learnsets];
        //learn[0] = Learn.Files.SelectMany(z => z).ToArray();
    }

    public void Dispose()
    {
        Manager.Dispose();
    }
}
