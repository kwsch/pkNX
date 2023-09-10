using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using pkNX.Containers;
using pkNX.Containers.VFS;
using pkNX.Structures;
using pkNX.Structures.FlatBuffers;
using pkNX.Structures.FlatBuffers.Arceus;

namespace pkNX.Game;

public class GameManagerPLA : GameManager
{
    public GameManagerPLA(GameLocation rom, int language) : base(rom, language) { }
    private string PathNPDM => Path.Combine(PathExeFS, "main.npdm");
    private string TitleID => BitConverter.ToUInt64(File.ReadAllBytes(PathNPDM), 0x470).ToString("X16");

    /// <summary>
    /// Generally useful game data that can be used by multiple editors.
    /// </summary>
    public GameData8a Data { get; protected set; } = null!;

    public VirtualFileSystem VFS { get; private set; } = null!;

    protected override void SetMitm()
    {
        var basePath = Path.GetDirectoryName(ROM.RomFS);
        if (basePath is null)
            throw new InvalidDataException("Invalid RomFS path.");

        var tid = ROM.ExeFS != null ? TitleID : "arceus";
        var redirect = Path.Combine(basePath, tid);
        FileMitm.SetRedirect(basePath, redirect);

        var cleanRomFS = new PhysicalFileSystem(basePath + "/romfs/").AsReadOnlyFileSystem();
        var moddedRomFS = new PhysicalFileSystem(redirect + "/romfs/");

        var layeredFS = new LayeredFileSystem(moddedRomFS, cleanRomFS);
        VFS = new VirtualFileSystem(new MountPoint("/romfs/", layeredFS));
    }

    public override void Initialize()
    {
        base.Initialize();

        // initialize gametext
        ResetText();

        // initialize common structures
        ResetData();

        ItemConverter.ItemNames = GetStrings(TextName.ItemNames);

        // TODO: Get these from files
        AIBehaviourConverter.BehaviourNames = new HashSet<string> {
            "",
            "group_leader_wild_poke_coward_lv1",
            "group_leader_wild_poke_coward_lv1_fish_pm0550",
            "group_leader_wild_poke_coward_lv1_ghost",
            "group_leader_wild_poke_coward_lv2",
            "group_leader_wild_poke_coward_lv2_dist",
            "group_leader_wild_poke_coward_lv3",
            "group_leader_wild_poke_coward_pm0063",
            "group_leader_wild_poke_coward_pm0133",
            "group_leader_wild_poke_coward_siren",
            "group_leader_wild_poke_fortress_ew053_to_warlike",
            "group_leader_wild_poke_fortress_pm0224_to_warlike",
            "group_leader_wild_poke_mild",
            "group_leader_wild_poke_mild_koduck",
            "group_leader_wild_poke_mild_lv1_barrier",
            "group_leader_wild_poke_mild_lv1_to_coward",
            "group_leader_wild_poke_mild_lv1_to_coward_fish_vanish",
            "group_leader_wild_poke_mild_lv1_to_coward_pm0113",
            "group_leader_wild_poke_mild_lv1_to_coward_pm0363",
            "group_leader_wild_poke_mild_lv1_to_coward_tamazarashi",
            "group_leader_wild_poke_mild_lv1_to_warlike",
            "group_leader_wild_poke_mild_lv1_to_warlike_pm0100",
            "group_leader_wild_poke_mild_lv2_to_coward",
            "group_leader_wild_poke_mild_lv2_to_warlike",
            "group_leader_wild_poke_mild_lv3_to_coward_shaymin",
            "group_leader_wild_poke_mild_lv3_to_warlike",
            "group_leader_wild_poke_mild_pm0129",
            "group_leader_wild_poke_mild_pm0299",
            "group_leader_wild_poke_warlike_dive_lv2",
            "group_leader_wild_poke_warlike_fish_oyabun",
            "group_leader_wild_poke_warlike_fortress",
            "group_leader_wild_poke_warlike_fortress_fish",
            "group_leader_wild_poke_warlike_ghost_callhelp",
            "group_leader_wild_poke_warlike_lv1",
            "group_leader_wild_poke_warlike_lv1_fish_vanish",
            "group_leader_wild_poke_warlike_lv1_ghost",
            "group_leader_wild_poke_warlike_lv1_pm0075",
            "group_leader_wild_poke_warlike_lv2",
            "group_leader_wild_poke_warlike_lv2_callhelp",
            "group_leader_wild_poke_warlike_lv2_sly",
            "group_leader_wild_poke_warlike_lv3",
            "group_leader_wild_poke_warlike_lv3_pm0059",
            "group_leader_wild_poke_warlike_lv3_pm0445",
            "group_leader_wild_poke_warlike_lv3_pm0549",
            "group_leader_wild_poke_warlike_lv3_pm0899",
            "group_leader_wild_poke_warlike_lv3_pm0900",
            "group_leader_wild_poke_warlike_lv3_pm0901",
            "group_leader_wild_poke_warlike_lv3_quick",
            "group_leader_wild_poke_warlike_lv3_quick_uma",
            "group_leader_wild_poke_warlike_lv3_slow",
            "group_leader_wild_poke_warlike_lv3_slow_sleep",
            "group_leader_wild_poke_warlike_oyabun",
            "group_leader_wild_poke_warlike_oyabun_pm0059",
            "group_leader_wild_poke_warlike_oyabun_pm0063",
            "group_leader_wild_poke_warlike_oyabun_pm0299",
            "group_leader_wild_poke_warlike_oyabun_pm0445",
            "group_leader_wild_poke_warlike_oyabun_pm0549",
            "group_leader_wild_poke_warlike_oyabun_pm0899",
            "group_leader_wild_poke_warlike_oyabun_pm0900",
            "group_leader_wild_poke_warlike_oyabun_pm0901",
            "group_leader_wild_poke_warlike_pm0490",
            "group_leader_wild_poke_warlike_pm0550_fish",
            "group_leader_wild_poke_warlike_siren",
            "group_leader_wild_poke_warlike_swamp",
            "group_wild_poke_coward_callhelp_leader",
            "group_wild_poke_coward_lv1",
            "group_wild_poke_coward_lv1_fish_pm0550",
            "group_wild_poke_coward_lv1_ghost",
            "group_wild_poke_coward_lv2",
            "group_wild_poke_coward_lv3",
            "group_wild_poke_coward_pm0063",
            "group_wild_poke_coward_pm0133",
            "group_wild_poke_coward_siren",
            "group_wild_poke_fortress_ew053_to_warlike",
            "group_wild_poke_fortress_pm0224_to_warlike",
            "group_wild_poke_mild",
            "group_wild_poke_mild_lv1_barrier",
            "group_wild_poke_mild_lv1_koduck",
            "group_wild_poke_mild_lv1_to_coward",
            "group_wild_poke_mild_lv1_to_coward_fish_vanish",
            "group_wild_poke_mild_lv1_to_coward_pm0113",
            "group_wild_poke_mild_lv1_to_coward_pm0363",
            "group_wild_poke_mild_lv1_to_warlike",
            "group_wild_poke_mild_lv1_to_warlike_pm0100",
            "group_wild_poke_mild_lv2_to_coward",
            "group_wild_poke_mild_lv2_to_warlike",
            "group_wild_poke_mild_lv3_to_warlike",
            "group_wild_poke_mild_pm0129",
            "group_wild_poke_mild_pm0299",
            "group_wild_poke_warlike_fortress",
            "group_wild_poke_warlike_fortress_fish",
            "group_wild_poke_warlike_lv1",
            "group_wild_poke_warlike_lv1_fish_vanish",
            "group_wild_poke_warlike_lv1_ghost",
            "group_wild_poke_warlike_lv1_ghost_callhelp_leader",
            "group_wild_poke_warlike_lv1_pm0075",
            "group_wild_poke_warlike_lv2",
            "group_wild_poke_warlike_lv3_pm0059",
            "group_wild_poke_warlike_lv3_pm0445",
            "group_wild_poke_warlike_lv3_pm0549",
            "group_wild_poke_warlike_lv3_pm0899",
            "group_wild_poke_warlike_lv3_pm0900",
            "group_wild_poke_warlike_lv3_pm0901",
            "group_wild_poke_warlike_lv3_quick",
            "group_wild_poke_warlike_lv3_slow",
            "group_wild_poke_warlike_lv3_slow_sleep",
            "group_wild_poke_warlike_pm0489",
            "group_wild_poke_warlike_pm0550_fish",
            "group_wild_poke_warlike_siren",
            "group_wild_poke_warlike_swamp",
            "nsi_poke_2",
            "nsi_poke_3",
            "nsi_poke_4",
            "nsi_poke_5",
            "nsi_poke_6",
            "nsi_poke_7",
            "nsi_poke_8",
            "nsi_poke_9",
            "semi_legend_poke_pm0485",
            "semi_legend_poke_pm0488",
            "semi_legend_poke_pm0491",
            "semi_legend_poke_pm0641",
            "semi_legend_poke_pm0642",
            "semi_legend_poke_pm0645",
            "semi_legend_poke_pm0905",
        };
    }

    private void ResetData()
    {
        Data = new GameData8a
        {
            // Folders
            MoveData = GetMoves(),

            // Single Files
            PersonalData = new PersonalTable8LA(GetFile(GameFile.PersonalStats)),
            LevelUpData = new(GetFile(GameFile.Learnsets), z => z.Table),
            EvolutionData = new(GetFile(GameFile.Evolutions), z => z.Table),

            FieldDrops = new(GetFile(GameFile.FieldDrops), z => z.Table),
            BattleDrops = new(GetFile(GameFile.BattleDrops), z => z.Table),
            DexResearch = new(GetFile(GameFile.DexResearch), z => z.Table),
        };

        DropTableConverter.DropTableHashes = Data.FieldDrops.Table.Select(x => x.Hash).ToArray();
    }

    private DataCache<Waza> GetMoves()
    {
        var move = GetFilteredFolder(GameFile.MoveStats);
        return new DataCache<Waza>(move)
        {
            Create = FlatBufferConverter.DeserializeFrom<Waza>,
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
