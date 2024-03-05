using pkNX.Containers;
using pkNX.Structures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using static pkNX.Structures.GameVersion;
using static pkNX.Game.GameFile;
using static pkNX.Containers.ContainerType;

namespace pkNX.Game;

/// <summary>
/// Handles file retrieval and lifetime management for a <see cref="GameLocation"/>'s <see cref="IFileContainer"/> data.
/// </summary>
public class GameFileMapping
{
    private readonly Dictionary<GameFile, IFileContainer> Cache = [];
    private readonly IReadOnlyCollection<GameFileReference> FileMap;

    public readonly ContainerHandler ProgressTracker = new();
    public readonly CancellationTokenSource TokenSource = new();

    private readonly GameLocation ROM;
    public GameFileMapping(GameLocation rom) => FileMap = GetMapping((ROM = rom).Game);

    internal IFileContainer GetFile(GameFile file, int language, GameManager gameManager)
    {
        if (file is GameText or StoryText)
            file += language + 1; // shift to localized language

        if (Cache.TryGetValue(file, out var container))
            return container;

        var info = FileMap.FirstOrDefault(f => f.File == file) ?? throw new ArgumentException($"Unknown {nameof(GameFile)} provided.", file.ToString());
        if (info.Type is SingleFileInternal && gameManager is IFileInternal irom)
        {
            var data = irom.GetPackedFile(info.RelativePath);
            container = new InternalFileContainer(data);
        }
        else
        {
            var basePath = (info.Parent == ContainerParent.ExeFS ? ROM.ExeFS : ROM.RomFS) ?? throw new ArgumentException($"No {info.Parent} found for {file}.");
            container = info.Get(basePath);
        }
        Cache.Add(file, container);
        return container;
    }

    internal void SaveAll()
    {
        foreach (var container in Cache)
        {
            var c = container.Value;
            if (c.Modified)
                c.SaveAs(c.FilePath!, ProgressTracker, TokenSource.Token).RunSynchronously();
        }
        var modified = Cache.Where(z => z.Value.Modified).ToArray();
        foreach (var m in modified)
            Cache.Remove(m.Key);
    }

    public static IReadOnlyCollection<GameFileReference> GetMapping(GameVersion game) => game switch
    {
        GP or GE or GG => FilesGG,
        SW or SH or SWSH => FilesSWSH,
        PLA => FilesPLA,
        SL or VL or SV => FilesSV,
        _ => throw new ArgumentOutOfRangeException(nameof(game), game, null),
    };

    #region Gen7

    /// <summary>
    /// Let's Go Pikachu &amp; Let's Go Eevee
    /// </summary>
    private static readonly GameFileReference[] FilesGG =
    [
        new(TrainerSpecData, "bin", "trainer", "trainer_data"),
        new(TrainerSpecPoke, "bin", "trainer", "trainer_poke"),
        new(TrainerSpecClass, "bin", "trainer", "trainer_type"),

        new(GameText0, 0, "bin", "message", "JPN", "common"),
        new(GameText1, 1, "bin", "message", "JPN_KANJI", "common"),
        new(GameText2, 2, "bin", "message", "English", "common"),
        new(GameText3, 3, "bin", "message", "French", "common"),
        new(GameText4, 4, "bin", "message", "Italian", "common"),
        new(GameText5, 5, "bin", "message", "German", "common"),
        // 6 unused lang
        new(GameText6, 7, "bin", "message", "Spanish", "common"),
        new(GameText7, 8, "bin", "message", "Korean", "common"),
        new(GameText8, 9, "bin", "message", "Simp_Chinese", "common"),
        new(GameText9, 10, "bin", "message", "Trad_Chinese", "common"),

        new(StoryText0, 0, "bin", "message", "JPN", "script"),
        new(StoryText1, 1, "bin", "message", "JPN_KANJI", "script"),
        new(StoryText2, 2, "bin", "message", "English", "script"),
        new(StoryText3, 3, "bin", "message", "French", "script"),
        new(StoryText4, 4, "bin", "message", "Italian", "script"),
        new(StoryText5, 5, "bin", "message", "German", "script"),
        // 6 unused lang
        new(StoryText6, 7, "bin", "message", "Spanish", "script"),
        new(StoryText7, 8, "bin", "message", "Korean", "script"),
        new(StoryText8, 9, "bin", "message", "Simp_Chinese", "script"),
        new(StoryText9, 10, "bin", "message", "Trad_Chinese", "script"),

        new(ItemStats, "bin", "pokelib", "item"),
        new(Evolutions, "bin", "pokelib", "evolution"),
        new(PersonalStats, "bin", "pokelib", "personal"),
        new(MegaEvolutions, "bin", "pokelib", "mega_evolution"),
        new(MoveStats, BinLinker, "bin", "pokelib", "waza", "waza_data.bin"),
        new(EncounterTableStatic, SingleFile, "bin", "script_event_data", "event_encount.bin"),
        new(EncounterTableTrade, SingleFile, "bin", "script_event_data", "field_trade_data.bin"),
        new(EncounterTableGift, SingleFile, "bin", "script_event_data", "add_poke.bin"),
        new(Learnsets, GameFreakPack, "bin", "archive", "waza_oboe.gfpak"),

        new(WildData1, SingleFile, "bin", "field", "param", "encount", "encount_data_p.bin"),
        new(WildData2, SingleFile, "bin", "field", "param", "encount", "encount_data_e.bin"),
        new(Shops, SingleFile, "bin", "app", "shop", "shop_data.bin"),

        // Cutscenes    bin\demo
        // Models       bin\archive\pokemon
        // pretty much everything is obviously named :)
    ];

    #endregion

    #region Gen 8
    /// <summary>
    /// Sword &amp; Shield
    /// </summary>
    private static readonly GameFileReference[] FilesSWSH =
    [
        new(TrainerSpecData, "bin", "trainer", "trainer_data"),
        new(TrainerSpecPoke, "bin", "trainer", "trainer_poke"),
        new(TrainerSpecClass, "bin", "trainer", "trainer_type"),

        new(GameText0, 0, "bin", "message", "JPN", "common"),
        new(GameText1, 1, "bin", "message", "JPN_KANJI", "common"),
        new(GameText2, 2, "bin", "message", "English", "common"),
        new(GameText3, 3, "bin", "message", "French", "common"),
        new(GameText4, 4, "bin", "message", "Italian", "common"),
        new(GameText5, 5, "bin", "message", "German", "common"),
        // 6 unused lang
        new(GameText6, 7, "bin", "message", "Spanish", "common"),
        new(GameText7, 8, "bin", "message", "Korean", "common"),
        new(GameText8, 9, "bin", "message", "Simp_Chinese", "common"),
        new(GameText9, 10, "bin", "message", "Trad_Chinese", "common"),

        new(StoryText0, 0, "bin", "message", "JPN", "script"),
        new(StoryText1, 1, "bin", "message", "JPN_KANJI", "script"),
        new(StoryText2, 2, "bin", "message", "English", "script"),
        new(StoryText3, 3, "bin", "message", "French", "script"),
        new(StoryText4, 4, "bin", "message", "Italian", "script"),
        new(StoryText5, 5, "bin", "message", "German", "script"),
        // 6 unused lang
        new(StoryText6, 7, "bin", "message", "Spanish", "script"),
        new(StoryText7, 8, "bin", "message", "Korean", "script"),
        new(StoryText8, 9, "bin", "message", "Simp_Chinese", "script"),
        new(StoryText9, 10, "bin", "message", "Trad_Chinese", "script"),

        new(ItemStats, SingleFile, "bin", "pml", "item", "item.dat"),
        new(Evolutions, "bin", "pml", "evolution"),
        new(GameFile.EggMoves, "bin", "pml", "tamagowaza"),
        new(PersonalStats, "bin", "pml", "personal"),
        new(MoveStats, "bin", "pml", "waza"),
        new(EncounterTableStatic, SingleFile, "bin", "script_event_data", "event_encount_data.bin"),
        new(EncounterTableTrade, SingleFile, "bin", "script_event_data", "field_trade.bin"),
        new(EncounterTableGift, SingleFile, "bin", "script_event_data", "add_poke.bin"),
        new(Learnsets, SingleFile, "bin", "pml", "waza_oboe", "wazaoboe_total.bin"),

        new(FacilityPokeNormal, SingleFile, "bin", "field", "param", "battle_tower", "battle_tower_poke_table.bin"),
        new(FacilityTrainerNormal, SingleFile, "bin", "field", "param", "battle_tower", "battle_tower_trainer_table.bin"),

        new(WildData, SingleFile, "bin", "archive", "field", "resident", "data_table.gfpak"),
        new(NestData, SingleFile, "bin", "archive", "field", "resident", "data_table.gfpak"),

        new(DynamaxDens, SingleFile, "bin", "appli", "chika", "data_table", "underground_exploration_poke.bin"),

        new(Placement, SingleFile, "bin", "archive", "field", "resident", "placement.gfpak"),
        new(Shops, SingleFile, "bin", "appli", "shop", "bin", "shop_data.bin"),
        new(Rentals, SingleFile, "bin", "script_event_data", "rental.bin"),
        new(SymbolBehave, SingleFile, "bin", "field", "param", "symbol_encount_mons_param", "symbol_encount_mons_param.bin"),

        // Cutscenes    bin\demo
        // Models       bin\archive\pokemon
        // pretty much everything is obviously named :)
    ];

    /// <summary>
    /// Legends: Arceus
    /// </summary>
    private static readonly GameFileReference[] FilesPLA =
    [
        new(TrainerSpecData, "bin", "trainer"),

        new(GameText0,  0, "bin", "message", "JPN", "common"),
        new(GameText1,  1, "bin", "message", "JPN_KANJI", "common"),
        new(GameText2,  2, "bin", "message", "English", "common"),
        new(GameText3,  3, "bin", "message", "French", "common"),
        new(GameText4,  4, "bin", "message", "Italian", "common"),
        new(GameText5,  5, "bin", "message", "German", "common"),
        // 6 unused lang
        new(GameText6,  7, "bin", "message", "Spanish", "common"),
        new(GameText7,  8, "bin", "message", "Korean", "common"),
        new(GameText8,  9, "bin", "message", "Simp_Chinese", "common"),
        new(GameText9, 10, "bin", "message", "Trad_Chinese", "common"),

        new(StoryText0, 0, "bin", "message", "JPN", "script"),
        new(StoryText1, 1, "bin", "message", "JPN_KANJI", "script"),
        new(StoryText2, 2, "bin", "message", "English", "script"),
        new(StoryText3, 3, "bin", "message", "French", "script"),
        new(StoryText4, 4, "bin", "message", "Italian", "script"),
        new(StoryText5, 5, "bin", "message", "German", "script"),
        // 6 unused lang
        new(StoryText6, 7, "bin", "message", "Spanish", "script"),
        new(StoryText7, 8, "bin", "message", "Korean", "script"),
        new(StoryText8, 9, "bin", "message", "Simp_Chinese", "script"),
        new(StoryText9, 10, "bin", "message", "Trad_Chinese", "script"),

        new(DexFormStorage         , SingleFile, "bin", "appli", "pokedex", "res_table", "pokedex_form_storage_index_table.bin"),
        new(DexRank                , SingleFile, "bin", "appli", "pokedex", "res_table", "pokedex_rank_table.bin"),
        new(DexResearch            , SingleFile, "bin", "appli", "pokedex", "res_table", "pokedex_research_task_table.bin"),
        new(PokemonResourceList    , SingleFile, "bin", "appli", "res_pokemon", "list", "pokemon_info_list.bin"),
        new(MoveShop               , SingleFile, "bin", "appli", "wazaremember", "bin", "wazashop_table.bin"),
        new(HaShop                 , SingleFile, "bin", "appli", "shop", "bin", "ha_shop_data.bin"),

        new(ArchiveFolder                         , "bin", "archive"),
        new(PokemonArchiveFolder                  , "bin", "archive", "pokemon"),
        new(Debug_SWSHPokemonArchiveFolder        , "bin", "archive", "pokemon", "SWSH"),
        new(Resident               , GameFreakPack, "bin", "archive", "field", "resident_release.gfpak"),
        new(archive_contents       , SingleFile   , "bin", "archive", "archive_contents.bin"),

        new(ThrowParam             , SingleFile, "bin", "capture", "throw_param_table.bin"),
        new(ThrowPermissionSet     , SingleFile, "bin", "capture", "throw_permissionset_dictionary.bin"),
        new(ThrowableParam         , SingleFile, "bin", "capture", "throwable_param_table.bin"),
        new(ThrowableResource      , SingleFile, "bin", "capture", "throwable_resource_dictionary.bin"),
        new(ThrowableResourceSet   , SingleFile, "bin", "capture", "throwable_resourceset_dictionary.bin"),

        new(Player1DressupTable    , SingleFile, "bin", "chara", "table", "dressup_table_p1.bin"),
        new(Player2DressupTable    , SingleFile, "bin", "chara", "table", "dressup_table_p2.bin"),

        new(Outbreak               , SingleFile, "bin", "field", "encount", "huge_outbreak.bin"),
        new(NewHugeGroup           , SingleFile, "bin", "field", "encount", "new_huge_outbreak_group.bin"),
        new(NewHugeGroupLottery    , SingleFile, "bin", "field", "encount", "new_huge_outbreak_group_lottery.bin"),
        new(NewHugeLottery         , SingleFile, "bin", "field", "encount", "new_huge_outbreak_lottery.bin"),
        new(NewHugeTimeLimit       , SingleFile, "bin", "field", "encount", "new_huge_outbreak_time_limit.bin"),

        new(AICommonConfig                 , SingleFile, "bin", "misc", "app_config", "ai_common_config.bin"),
        new(AIExcitingConfig               , SingleFile, "bin", "misc", "app_config", "ai_exciting_config.bin"),
        new(AIFieldWazaConfig              , SingleFile, "bin", "misc", "app_config", "ai_field_waza_config.bin"),
        new(AISemiLegendConfig             , SingleFile, "bin", "misc", "app_config", "ai_semi_legend_config.bin"),
        new(AITirednessConfig              , SingleFile, "bin", "misc", "app_config", "ai_tiredness_config.bin"),
        new(AppConfigList                  , SingleFile, "bin", "misc", "app_config", "app_config_list.bin"),
        new(AppliHudConfig                 , SingleFile, "bin", "misc", "app_config", "appli_hud_config.bin"),
        new(AppliStaffrollConfig           , SingleFile, "bin", "misc", "app_config", "appli_staffroll_config.bin"),
        new(AppliTipsConfig                , SingleFile, "bin", "misc", "app_config", "appli_tips_config.bin"),
        new(BattleCommonConfig             , SingleFile, "bin", "misc", "app_config", "battle_common_config.bin"),
        new(BattleEndConfig                , SingleFile, "bin", "misc", "app_config", "battle_end_config.bin"),
        new(BattleInConfig                 , SingleFile, "bin", "misc", "app_config", "battle_in_config.bin"),
        new(BattleLogicConfig              , SingleFile, "bin", "misc", "app_config", "battle_logic_config.bin"),
        new(BattleStartConfig              , SingleFile, "bin", "misc", "app_config", "battle_start_config.bin"),
        new(BattleViewConfig               , SingleFile, "bin", "misc", "app_config", "battle_view_config.bin"),
        new(BattleVsnsConfig               , SingleFile, "bin", "misc", "app_config", "battle_vsns_config.bin"),
        new(BuddyBattleConfig              , SingleFile, "bin", "misc", "app_config", "buddy_battle_config.bin"),
        new(BuddyConfig                    , SingleFile, "bin", "misc", "app_config", "buddy_config.bin"),
        new(BuddyDirectItemConfig          , SingleFile, "bin", "misc", "app_config", "buddy_direct_item_config.bin"),
        new(BuddyGroupTalkConfig           , SingleFile, "bin", "misc", "app_config", "buddy_group_talk_config.bin"),
        new(BuddyLandmarkConfig            , SingleFile, "bin", "misc", "app_config", "buddy_landmark_config.bin"),
        new(BuddyNPCReactionConfig         , SingleFile, "bin", "misc", "app_config", "buddy_npc_reaction_config.bin"),
        new(BuddyPlayerModeConfig          , SingleFile, "bin", "misc", "app_config", "buddy_player_mode_config.bin"),
        new(BuddyWarpConfig                , SingleFile, "bin", "misc", "app_config", "buddy_warp_config.bin"),
        new(CharacterBipedIkConfig         , SingleFile, "bin", "misc", "app_config", "character_biped_ik_config.bin"),
        new(CharacterBlinkConfig           , SingleFile, "bin", "misc", "app_config", "character_blink_config.bin"),
        new(CharacterControllerConfig      , SingleFile, "bin", "misc", "app_config", "character_controller_config.bin"),
        new(CharacterLookAtConfig          , SingleFile, "bin", "misc", "app_config", "character_look_at_config.bin"),
        new(CaptureConfig                  , SingleFile, "bin", "misc", "app_config", "common_capture_config.bin"),
        new(CommonGeneralConfig            , SingleFile, "bin", "misc", "app_config", "common_general_config.bin"),
        new(CommonItemConfig               , SingleFile, "bin", "misc", "app_config", "common_item_config.bin"),
        new(DemoConfig                     , SingleFile, "bin", "misc", "app_config", "demo_config.bin"),
        new(EnvPokeVoiceConfig             , SingleFile, "bin", "misc", "app_config", "env_poke_voice_config.bin"),
        new(EventBalloonrunConfig          , SingleFile, "bin", "misc", "app_config", "event_balloonrun_config.bin"),
        new(EventBalloonthrowConfig        , SingleFile, "bin", "misc", "app_config", "event_balloonthrow_config.bin"),
        new(EventBanditConfig              , SingleFile, "bin", "misc", "app_config", "event_bandit_config.bin"),
        new(EventCullingConfig             , SingleFile, "bin", "misc", "app_config", "event_culling_config.bin"),
        new(EventDitherConfig              , SingleFile, "bin", "misc", "app_config", "event_dither_config.bin"),
        new(EventFarmConfig                , SingleFile, "bin", "misc", "app_config", "event_farm_config.bin"),
        new(EventGameOverConfig            , SingleFile, "bin", "misc", "app_config", "event_game_over_config.bin"),
        new(EventItemConfig                , SingleFile, "bin", "misc", "app_config", "event_item_config.bin"),
        new(EventMkrgRewardConfig          , SingleFile, "bin", "misc", "app_config", "event_mkrg_reward_config.bin"),
        new(EventQuestBoardConfig          , SingleFile, "bin", "misc", "app_config", "event_quest_board_config.bin"),
        new(EventRestrictionBattle         , SingleFile, "bin", "misc", "app_config", "event_restriction_battle.bin"),
        new(EventWork                      , SingleFile, "bin", "misc", "app_config", "event_work.bin"),
        new(FieldAnimationFramerateConfig  , SingleFile, "bin", "misc", "app_config", "field_anime_framerate_config.bin"),
        new(FieldAreaSpeedConfig           , SingleFile, "bin", "misc", "app_config", "field_area_speed_config.bin"),
        new(FieldCameraConfig              , SingleFile, "bin", "misc", "app_config", "field_camera_config.bin"),
        new(FieldCaptureDirectorConfig     , SingleFile, "bin", "misc", "app_config", "field_capture_director_config.bin"),
        new(FieldCharaViewerConfig         , SingleFile, "bin", "misc", "app_config", "field_chara_viewer_config.bin"),
        new(FieldCommonConfig              , SingleFile, "bin", "misc", "app_config", "field_common_config.bin"),
        new(FieldDirectItemConfig          , SingleFile, "bin", "misc", "app_config", "field_direct_item_config.bin"),
        new(FieldEnvConfig                 , SingleFile, "bin", "misc", "app_config", "field_env_config.bin"),
        new(OutbreakConfig                 , SingleFile, "bin", "misc", "app_config", "field_huge_outbreak.bin"),
        new(FieldItem                      , SingleFile, "bin", "misc", "app_config", "field_item.bin"),
        new(FieldItemRespawn               , SingleFile, "bin", "misc", "app_config", "field_item_respawn.bin"),
        new(FieldLandmarkConfig            , SingleFile, "bin", "misc", "app_config", "field_landmark_config.bin"),
        new(FieldLandmarkInciteConfig      , SingleFile, "bin", "misc", "app_config", "field_landmark_incite_config.bin"),
        new(FieldLockonConfig              , SingleFile, "bin", "misc", "app_config", "field_lockon_config.bin"),
        new(BallThrowConfig                , SingleFile, "bin", "misc", "app_config", "field_my_poke_ball_config.bin"),
        new(FieldBallMissedConfig          , SingleFile, "bin", "misc", "app_config", "field_my_poke_ball_hit_none_target_config.bin"),
        new(FieldObstructionWazaConfig     , SingleFile, "bin", "misc", "app_config", "field_obstruction_waza_config.bin"),
        new(FieldPokemonSlopeConfig        , SingleFile, "bin", "misc", "app_config", "field_pokemon_slope_config.bin"),
        new(FieldQuestDestinationConfig    , SingleFile, "bin", "misc", "app_config", "field_quest_destination_config.bin"),
        new(FieldShadowConfig              , SingleFile, "bin", "misc", "app_config", "field_shadow_config.bin"),
        new(FieldSpawnerConfig             , SingleFile, "bin", "misc", "app_config", "field_spawner_config.bin"),
        new(FieldThrowConfig               , SingleFile, "bin", "misc", "app_config", "field_throw_config.bin"),
        new(FieldThrowableAfterHitConfig   , SingleFile, "bin", "misc", "app_config", "field_throwable_after_hit_config.bin"),
        new(FieldVigilanceBgmConfig        , SingleFile, "bin", "misc", "app_config", "field_vigilance_bgm_config.bin"),
        new(FieldWeatheringConfig          , SingleFile, "bin", "misc", "app_config", "field_weathering_config.bin"),
        new(FieldWildPokemonConfig         , SingleFile, "bin", "misc", "app_config", "field_wild_pokemon_config.bin"),
        new(WormholeConfig                 , SingleFile, "bin", "misc", "app_config", "field_wormhole_config.bin"),
        new(NPCAIConfig                    , SingleFile, "bin", "misc", "app_config", "npc_ai_config.bin"),
        new(NPCControllerConfig            , SingleFile, "bin", "misc", "app_config", "npc_controller_config.bin"),
        new(NPCCreaterConfig               , SingleFile, "bin", "misc", "app_config", "npc_creater_config.bin"),
        new(NPCPokemonAIConfig             , SingleFile, "bin", "misc", "app_config", "npc_pokemon_ai_config.bin"),
        new(NPCPopupConfig                 , SingleFile, "bin", "misc", "app_config", "npc_popup_config.bin"),
        new(NPCTalkTableConfig             , SingleFile, "bin", "misc", "app_config", "npc_talk_table_config.bin"),
        new(PlayerCameraShakeConfig        , SingleFile, "bin", "misc", "app_config", "player_camera_shake_config.bin"),
        new(PlayerCollisionConfig          , SingleFile, "bin", "misc", "app_config", "player_collision_config.bin"),
        new(PlayerConfig                   , SingleFile, "bin", "misc", "app_config", "player_config.bin"),
        new(PlayerControllerConfig         , SingleFile, "bin", "misc", "app_config", "player_controller_config.bin"),
        new(PlayerFaceConfig               , SingleFile, "bin", "misc", "app_config", "player_face_config.bin"),
        new(PokemonConfig                  , SingleFile, "bin", "misc", "app_config", "pokemon_config.bin"),
        new(PokemonControllerConfig        , SingleFile, "bin", "misc", "app_config", "pokemon_controller_config.bin"),
        new(EvolutionConfig                , SingleFile, "bin", "misc", "app_config", "pokemon_evolution_config.bin"),
        new(PokemonFriendshipConfig        , SingleFile, "bin", "misc", "app_config", "pokemon_friendship_config.bin"),
        new(ShinyRolls                     , SingleFile, "bin", "misc", "app_config", "pokemon_rare.bin"),
        new(SizeScaleConfig                , SingleFile, "bin", "misc", "app_config", "pokemon_size_category_adjust_scale_config.bin"),
        new(RideBasuraoCollisionConfig     , SingleFile, "bin", "misc", "app_config", "ride_basurao_collision_config.bin"),
        new(RideBasuraoConfig              , SingleFile, "bin", "misc", "app_config", "ride_basurao_config.bin"),
        new(RideChangeConfig               , SingleFile, "bin", "misc", "app_config", "ride_change_config.bin"),
        new(RideCommonConfig               , SingleFile, "bin", "misc", "app_config", "ride_common_config.bin"),
        new(RideNyuuraCollisionConfig      , SingleFile, "bin", "misc", "app_config", "ride_nyuura_collision_config.bin"),
        new(RideNyuuraConfig               , SingleFile, "bin", "misc", "app_config", "ride_nyuura_config.bin"),
        new(RideNyuuraControllerConfig     , SingleFile, "bin", "misc", "app_config", "ride_nyuura_controller_config.bin"),
        new(RideOdoshishiCollisionConfig   , SingleFile, "bin", "misc", "app_config", "ride_odoshishi_collision_config.bin"),
        new(RideOdoshishiConfig            , SingleFile, "bin", "misc", "app_config", "ride_odoshishi_config.bin"),
        new(RideRingumaCollisionConfig     , SingleFile, "bin", "misc", "app_config", "ride_ringuma_collision_config.bin"),
        new(RideRingumaConfig              , SingleFile, "bin", "misc", "app_config", "ride_ringuma_config.bin"),
        new(RideRingumaControllerConfig    , SingleFile, "bin", "misc", "app_config", "ride_ringuma_controller_config.bin"),
        new(RideWhooguruCollisionConfig    , SingleFile, "bin", "misc", "app_config", "ride_whooguru_collision_config.bin"),
        new(RideWhooguruConfig             , SingleFile, "bin", "misc", "app_config", "ride_whooguru_config.bin"),
        new(RideWhooguruControllerConfig   , SingleFile, "bin", "misc", "app_config", "ride_whooguru_controller_config.bin"),
        new(SoundConfig                    , SingleFile, "bin", "misc", "app_config", "sound_config.bin"),
        new(WaterMotion                    , SingleFile, "bin", "misc", "app_config", "water_motion.bin"),

        new(MoveStats                                                , "bin", "pml", "waza"),
        new(ItemStats                      , SingleFile, "bin", "pml", "item", "item.dat"),
        new(Evolutions                     , SingleFile, "bin", "pml", "evolution", "evolution_data_total.evobin"),
        new(PersonalStats                  , SingleFile, "bin", "pml", "personal", "personal_data_total.perbin"),
        new(Learnsets                      , SingleFile, "bin", "pml", "waza_oboe", "waza_oboe_total.wazaoboe"),

        new(EncounterTableGift             , SingleFile, "bin", "pokemon", "data", "poke_add.bin"),
        new(SymbolBehave                   , SingleFile, "bin", "pokemon", "data", "poke_ai.bin"),
        new(PokeBattleSpawn                , SingleFile, "bin", "pokemon", "data", "poke_battle_spawn.bin"),
        new(PokeBodyParticle               , SingleFile, "bin", "pokemon", "data", "poke_body_particle.bin"),
        new(PokeCaptureCollision           , SingleFile, "bin", "pokemon", "data", "poke_capture_collision.bin"),
        new(PokeDefaultLocator             , SingleFile, "bin", "pokemon", "data", "poke_default_locator.trloc"),
        new(FieldDrops                     , SingleFile, "bin", "pokemon", "data", "poke_drop_item.bin"),
        new(BattleDrops                    , SingleFile, "bin", "pokemon", "data", "poke_drop_item_battle.bin"),
        new(PokeEatingHabits               , SingleFile, "bin", "pokemon", "data", "poke_eating_habits.bin"),
        new(EncounterRateTable             , SingleFile, "bin", "pokemon", "data", "poke_encount.bin"),
        new(EncounterTableStatic           , SingleFile, "bin", "pokemon", "data", "poke_event_encount.bin"),
        new(MoveObstructionLegend          , SingleFile, "bin", "pokemon", "data", "poke_field_obstruction_waza_ns_legend.bin"),
        new(MoveObstructionLegendEffect    , SingleFile, "bin", "pokemon", "data", "poke_field_obstruction_waza_ns_legend_effect.bin"),
        new(MoveObstructionSE              , SingleFile, "bin", "pokemon", "data", "poke_field_obstruction_waza_se.bin"),
        new(MoveObstructionWild            , SingleFile, "bin", "pokemon", "data", "poke_field_obstruction_waza_wild.bin"),
        new(MoveObstructionWildEffect      , SingleFile, "bin", "pokemon", "data", "poke_field_obstruction_waza_wild_effect.bin"),
        new(MoveObstructionWater           , SingleFile, "bin", "pokemon", "data", "poke_field_obstruction_waza_wild_water.bin"),
        new(MoveObstructionWaterEffect     , SingleFile, "bin", "pokemon", "data", "poke_field_obstruction_waza_wild_water_effect.bin"),
        new(PokeMisc                       , SingleFile, "bin", "pokemon", "data", "poke_misc.bin"),
        new(NushiBattleSettings            , SingleFile, "bin", "pokemon", "data", "poke_nushi_battle_setting.bin"),
        new(PokemonResourceTable           , SingleFile, "bin", "pokemon", "table", "poke_resource_table.trpmcatalog"),

        new(EncounterTableTrade                 , SingleFile, "bin", "script_event_data", "field_trade.bin"), // Incorrect?

        // Cutscenes    bin\demo
        // Models       bin\archive\pokemon
        // pretty much everything is obviously named :)
    ];
    #endregion

    #region Gen9
    /// <summary>
    /// Scarlet &amp; Violet
    /// </summary>
    private static readonly GameFileReference[] FilesSV =
    [
        new(DataTrpfd, SingleFile, "arc", "data.trpfd"),
        new(DataTrpfs, SingleFile, "arc", "data.trpfs"),
        // new(TrainerSpecData, "bin", "trainer"),
        // 
        // new(GameText0,  0, "bin", "message", "JPN", "common"),
        // new(GameText1,  1, "bin", "message", "JPN_KANJI", "common"),
        // new(GameText2,  2, "bin", "message", "English", "common"),
        // new(GameText3,  3, "bin", "message", "French", "common"),
        // new(GameText4,  4, "bin", "message", "Italian", "common"),
        // new(GameText5,  5, "bin", "message", "German", "common"),
        // // 6 unused lang
        // new(GameText6,  7, "bin", "message", "Spanish", "common"),
        // new(GameText7,  8, "bin", "message", "Korean", "common"),
        // new(GameText8,  9, "bin", "message", "Simp_Chinese", "common"),
        // new(GameText9, 10, "bin", "message", "Trad_Chinese", "common"),
        // 
        // new(StoryText0, 0, "bin", "message", "JPN", "script"),
        // new(StoryText1, 1, "bin", "message", "JPN_KANJI", "script"),
        // new(StoryText2, 2, "bin", "message", "English", "script"),
        // new(StoryText3, 3, "bin", "message", "French", "script"),
        // new(StoryText4, 4, "bin", "message", "Italian", "script"),
        // new(StoryText5, 5, "bin", "message", "German", "script"),
        // // 6 unused lang
        // new(StoryText6, 7, "bin", "message", "Spanish", "script"),
        // new(StoryText7, 8, "bin", "message", "Korean", "script"),
        // new(StoryText8, 9, "bin", "message", "Simp_Chinese", "script"),
        // new(StoryText9, 10, "bin", "message", "Trad_Chinese", "script"),
        // 
        // new(MoveStats                                                , "bin", "pml", "waza"),
        // new(ItemStats                      , SingleFile, "bin", "pml", "item", "item.dat"),
        // new(Evolutions                     , SingleFile, "bin", "pml", "evolution", "evolution_data_total.evobin"),
        // new(PersonalStats                  , SingleFile, "bin", "pml", "personal", "personal_data_total.perbin"),
        // new(Learnsets                      , SingleFile, "bin", "pml", "waza_oboe", "waza_oboe_total.wazaoboe"),
        // 
        // new(EncounterTableGift                  , SingleFile, "bin", "pokemon", "data", "poke_add.bin"),
        // new(EncounterTableTrade                 , SingleFile, "bin", "script_event_data", "field_trade.bin"), // Incorrect?
    ];
    #endregion
}
