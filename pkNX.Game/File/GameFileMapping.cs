using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using pkNX.Containers;
using pkNX.Structures;

namespace pkNX.Game;

/// <summary>
/// Handles file retrieval and lifetime management for a <see cref="GameLocation"/>'s <see cref="IFileContainer"/> data.
/// </summary>
public class GameFileMapping
{
    private readonly Dictionary<GameFile, IFileContainer> Cache = new();
    private readonly IReadOnlyCollection<GameFileReference> FileMap;

    public readonly ContainerHandler ProgressTracker = new();
    public readonly CancellationTokenSource TokenSource = new();

    private readonly GameLocation ROM;
    public GameFileMapping(GameLocation rom) => FileMap = GetMapping((ROM = rom).Game);

    internal IFileContainer GetFile(GameFile file, int language)
    {
        if (file is GameFile.GameText or GameFile.StoryText)
            file += language + 1; // shift to localized language

        if (Cache.TryGetValue(file, out var container))
            return container;

        var info = FileMap.FirstOrDefault(f => f.File == file);
        if (info == null)
            throw new ArgumentException($"Unknown {nameof(GameFile)} provided.", file.ToString());

        var basePath = info.Parent == ContainerParent.ExeFS ? ROM.ExeFS : ROM.RomFS;
        container = info.Get(basePath);
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
        GameVersion.GP => GG,
        GameVersion.GE => GG,
        GameVersion.GG => GG,
        GameVersion.SW => SWSH,
        GameVersion.SH => SWSH,
        GameVersion.SWSH => SWSH,
        GameVersion.PLA => PLA,
        _ => null,
    };

    #region Gen7

    /// <summary>
    /// Let's Go Pikachu &amp; Let's Go Eevee
    /// </summary>
    private static readonly GameFileReference[] GG =
    {
        new(GameFile.TrainerData, "bin", "trainer", "trainer_data"),
        new(GameFile.TrainerPoke, "bin", "trainer", "trainer_poke"),
        new(GameFile.TrainerClass, "bin", "trainer", "trainer_type"),

        new(GameFile.GameText0, 0, "bin", "message", "JPN", "common"),
        new(GameFile.GameText1, 1, "bin", "message", "JPN_KANJI", "common"),
        new(GameFile.GameText2, 2, "bin", "message", "English", "common"),
        new(GameFile.GameText3, 3, "bin", "message", "French", "common"),
        new(GameFile.GameText4, 4, "bin", "message", "Italian", "common"),
        new(GameFile.GameText5, 5, "bin", "message", "German", "common"),
        // 6 unused lang
        new(GameFile.GameText6, 7, "bin", "message", "Spanish", "common"),
        new(GameFile.GameText7, 8, "bin", "message", "Korean", "common"),
        new(GameFile.GameText8, 9, "bin", "message", "Simp_Chinese", "common"),
        new(GameFile.GameText9, 10, "bin", "message", "Trad_Chinese", "common"),

        new(GameFile.StoryText0, 0, "bin", "message", "JPN", "script"),
        new(GameFile.StoryText1, 1, "bin", "message", "JPN_KANJI", "script"),
        new(GameFile.StoryText2, 2, "bin", "message", "English", "script"),
        new(GameFile.StoryText3, 3, "bin", "message", "French", "script"),
        new(GameFile.StoryText4, 4, "bin", "message", "Italian", "script"),
        new(GameFile.StoryText5, 5, "bin", "message", "German", "script"),
        // 6 unused lang
        new(GameFile.StoryText6, 7, "bin", "message", "Spanish", "script"),
        new(GameFile.StoryText7, 8, "bin", "message", "Korean", "script"),
        new(GameFile.StoryText8, 9, "bin", "message", "Simp_Chinese", "script"),
        new(GameFile.StoryText9, 10, "bin", "message", "Trad_Chinese", "script"),

        new(GameFile.ItemStats, "bin", "pokelib", "item"),
        new(GameFile.Evolutions, "bin", "pokelib", "evolution"),
        new(GameFile.PersonalStats, "bin", "pokelib", "personal"),
        new(GameFile.MegaEvolutions, "bin", "pokelib", "mega_evolution"),
        new(GameFile.MoveStats, ContainerType.Mini, "bin", "pokelib", "waza", "waza_data.bin"),
        new(GameFile.EncounterStatic, ContainerType.SingleFile, "bin", "script_event_data", "event_encount.bin"),
        new(GameFile.EncounterTrade, ContainerType.SingleFile, "bin", "script_event_data", "field_trade_data.bin"),
        new(GameFile.EncounterGift, ContainerType.SingleFile, "bin", "script_event_data", "add_poke.bin"),
        new(GameFile.Learnsets, ContainerType.GFPack, "bin", "archive", "waza_oboe.gfpak"),

        new(GameFile.WildData1, ContainerType.SingleFile, "bin", "field", "param", "encount", "encount_data_p.bin"),
        new(GameFile.WildData2, ContainerType.SingleFile, "bin", "field", "param", "encount", "encount_data_e.bin"),
        new(GameFile.Shops, ContainerType.SingleFile, "bin", "app", "shop", "shop_data.bin"),

        // Cutscenes    bin\demo
        // Models       bin\archive\pokemon
        // pretty much everything is obviously named :)
        #endregion
    };

    #region Gen 8
    /// <summary>
    /// Sword
    /// </summary>
    private static readonly GameFileReference[] SWSH =
    {
        new(GameFile.TrainerData, "bin", "trainer", "trainer_data"),
        new(GameFile.TrainerPoke, "bin", "trainer", "trainer_poke"),
        new(GameFile.TrainerClass, "bin", "trainer", "trainer_type"),

        new(GameFile.GameText0, 0, "bin", "message", "JPN", "common"),
        new(GameFile.GameText1, 1, "bin", "message", "JPN_KANJI", "common"),
        new(GameFile.GameText2, 2, "bin", "message", "English", "common"),
        new(GameFile.GameText3, 3, "bin", "message", "French", "common"),
        new(GameFile.GameText4, 4, "bin", "message", "Italian", "common"),
        new(GameFile.GameText5, 5, "bin", "message", "German", "common"),
        // 6 unused lang
        new(GameFile.GameText6, 7, "bin", "message", "Spanish", "common"),
        new(GameFile.GameText7, 8, "bin", "message", "Korean", "common"),
        new(GameFile.GameText8, 9, "bin", "message", "Simp_Chinese", "common"),
        new(GameFile.GameText9, 10, "bin", "message", "Trad_Chinese", "common"),

        new(GameFile.StoryText0, 0, "bin", "message", "JPN", "script"),
        new(GameFile.StoryText1, 1, "bin", "message", "JPN_KANJI", "script"),
        new(GameFile.StoryText2, 2, "bin", "message", "English", "script"),
        new(GameFile.StoryText3, 3, "bin", "message", "French", "script"),
        new(GameFile.StoryText4, 4, "bin", "message", "Italian", "script"),
        new(GameFile.StoryText5, 5, "bin", "message", "German", "script"),
        // 6 unused lang
        new(GameFile.StoryText6, 7, "bin", "message", "Spanish", "script"),
        new(GameFile.StoryText7, 8, "bin", "message", "Korean", "script"),
        new(GameFile.StoryText8, 9, "bin", "message", "Simp_Chinese", "script"),
        new(GameFile.StoryText9, 10, "bin", "message", "Trad_Chinese", "script"),

        new(GameFile.ItemStats, ContainerType.SingleFile, "bin", "pml", "item", "item.dat"),
        new(GameFile.Evolutions, "bin", "pml", "evolution"),
        new(GameFile.EggMoves, "bin", "pml", "tamagowaza"),
        new(GameFile.PersonalStats, "bin", "pml", "personal"),
        new(GameFile.MoveStats, "bin", "pml", "waza"),
        new(GameFile.EncounterStatic, ContainerType.SingleFile, "bin", "script_event_data", "event_encount_data.bin"),
        new(GameFile.EncounterTrade, ContainerType.SingleFile, "bin", "script_event_data", "field_trade.bin"),
        new(GameFile.EncounterGift, ContainerType.SingleFile, "bin", "script_event_data", "add_poke.bin"),
        new(GameFile.Learnsets, ContainerType.SingleFile, "bin", "pml", "waza_oboe", "wazaoboe_total.bin"),

        new(GameFile.FacilityPokeNormal, ContainerType.SingleFile, "bin", "field", "param", "battle_tower", "battle_tower_poke_table.bin"),
        new(GameFile.FacilityTrainerNormal, ContainerType.SingleFile, "bin", "field", "param", "battle_tower", "battle_tower_trainer_table.bin"),

        new(GameFile.WildData, ContainerType.SingleFile, "bin", "archive", "field", "resident", "data_table.gfpak"),
        new(GameFile.NestData, ContainerType.SingleFile, "bin", "archive", "field", "resident", "data_table.gfpak"),

        new(GameFile.DynamaxDens, ContainerType.SingleFile, "bin", "appli", "chika", "data_table", "underground_exploration_poke.bin"),

        new(GameFile.Placement, ContainerType.SingleFile, "bin", "archive", "field", "resident", "placement.gfpak"),
        new(GameFile.Shops, ContainerType.SingleFile, "bin", "appli", "shop", "bin", "shop_data.bin"),
        new(GameFile.Rentals, ContainerType.SingleFile, "bin", "script_event_data", "rental.bin"),
        new(GameFile.SymbolBehave, ContainerType.SingleFile, "bin", "field", "param", "symbol_encount_mons_param", "symbol_encount_mons_param.bin"),

        // Cutscenes    bin\demo
        // Models       bin\archive\pokemon
        // pretty much everything is obviously named :)
    };

    /// <summary>
    /// Sword
    /// </summary>
    private static readonly GameFileReference[] PLA =
    {
        new(GameFile.TrainerData, "bin", "trainer"),

        new(GameFile.GameText0, 0, "bin", "message", "JPN", "common"),
        new(GameFile.GameText1, 1, "bin", "message", "JPN_KANJI", "common"),
        new(GameFile.GameText2, 2, "bin", "message", "English", "common"),
        new(GameFile.GameText3, 3, "bin", "message", "French", "common"),
        new(GameFile.GameText4, 4, "bin", "message", "Italian", "common"),
        new(GameFile.GameText5, 5, "bin", "message", "German", "common"),
        // 6 unused lang
        new(GameFile.GameText6, 7, "bin", "message", "Spanish", "common"),
        new(GameFile.GameText7, 8, "bin", "message", "Korean", "common"),
        new(GameFile.GameText8, 9, "bin", "message", "Simp_Chinese", "common"),
        new(GameFile.GameText9, 10, "bin", "message", "Trad_Chinese", "common"),

        new(GameFile.StoryText0, 0, "bin", "message", "JPN", "script"),
        new(GameFile.StoryText1, 1, "bin", "message", "JPN_KANJI", "script"),
        new(GameFile.StoryText2, 2, "bin", "message", "English", "script"),
        new(GameFile.StoryText3, 3, "bin", "message", "French", "script"),
        new(GameFile.StoryText4, 4, "bin", "message", "Italian", "script"),
        new(GameFile.StoryText5, 5, "bin", "message", "German", "script"),
        // 6 unused lang
        new(GameFile.StoryText6, 7, "bin", "message", "Spanish", "script"),
        new(GameFile.StoryText7, 8, "bin", "message", "Korean", "script"),
        new(GameFile.StoryText8, 9, "bin", "message", "Simp_Chinese", "script"),
        new(GameFile.StoryText9, 10, "bin", "message", "Trad_Chinese", "script"),

        new(GameFile.ItemStats, ContainerType.SingleFile, "bin", "pml", "item", "item.dat"),
        new(GameFile.Evolutions, ContainerType.SingleFile, "bin", "pml", "evolution", "evolution_data_total.evobin"),
        new(GameFile.PersonalStats, ContainerType.SingleFile, "bin", "pml", "personal", "personal_data_total.perbin"),
        new(GameFile.MoveStats, "bin", "pml", "waza"),
        new(GameFile.EncounterStatic, ContainerType.SingleFile, "bin", "pokemon", "data", "poke_event_encount.bin"),
        new(GameFile.EncounterTrade, ContainerType.SingleFile, "bin", "script_event_data", "field_trade.bin"),
        new(GameFile.EncounterGift, ContainerType.SingleFile, "bin", "pokemon", "data", "poke_add.bin"),
        new(GameFile.Learnsets, ContainerType.SingleFile, "bin", "pml", "waza_oboe", "waza_oboe_total.wazaoboe"),

        new(GameFile.Resident, ContainerType.GFPack, "bin", "archive", "field", "resident_release.gfpak"),

        new(GameFile.FieldDrops, ContainerType.SingleFile, "bin", "pokemon", "data", "poke_drop_item.bin"),
        new(GameFile.BattleDrops, ContainerType.SingleFile, "bin", "pokemon", "data", "poke_drop_item_battle.bin"),

        new(GameFile.DexResearch, ContainerType.SingleFile, "bin", "appli", "pokedex", "res_table", "pokedex_research_task_table.bin"),

        new(GameFile.EncounterRateTable, ContainerType.SingleFile, "bin", "pokemon", "data", "poke_encount.bin"),
        new(GameFile.PokeMisc, ContainerType.SingleFile, "bin", "pokemon", "data", "poke_misc.bin"),
        new(GameFile.Outbreak, ContainerType.SingleFile, "bin", "field", "encount", "huge_outbreak.bin"),
        new(GameFile.MoveShop, ContainerType.SingleFile, "bin", "appli", "wazaremember", "bin", "wazashop_table.bin"),
        new(GameFile.HaShop, ContainerType.SingleFile, "bin", "appli", "shop", "bin", "ha_shop_data.bin"),

        new(GameFile.ThrowPermissionSet     , ContainerType.SingleFile, "bin", "capture", "throw_permissionset_dictionary.bin"),
        new(GameFile.ThrowableParam         , ContainerType.SingleFile, "bin", "capture", "throwable_param_table.bin"),
        new(GameFile.ThrowableResource      , ContainerType.SingleFile, "bin", "capture", "throwable_resource_dictionary.bin"),
        new(GameFile.ThrowableResourceSet   , ContainerType.SingleFile, "bin", "capture", "throwable_resourceset_dictionary.bin"),

        new(GameFile.SymbolBehave               , ContainerType.SingleFile, "bin", "pokemon", "data", "poke_ai.bin"),

        new(GameFile.AICommonConfig             , ContainerType.SingleFile, "bin", "misc", "app_config", "ai_common_config.bin"),
        new(GameFile.AIExcitingConfig           , ContainerType.SingleFile, "bin", "misc", "app_config", "ai_exciting_config.bin"),
        new(GameFile.AIFieldWazaConfig          , ContainerType.SingleFile, "bin", "misc", "app_config", "ai_field_waza_config.bin"),
        new(GameFile.AISemiLegendConfig         , ContainerType.SingleFile, "bin", "misc", "app_config", "ai_semi_legend_config.bin"),
        new(GameFile.AITirednessConfig          , ContainerType.SingleFile, "bin", "misc", "app_config", "ai_tiredness_config.bin"),
        new(GameFile.AppConfigList              , ContainerType.SingleFile, "bin", "misc", "app_config", "app_config_list.bin"),
        new(GameFile.AppliHudConfig             , ContainerType.SingleFile, "bin", "misc", "app_config", "appli_hud_config.bin"),
        new(GameFile.AppliStaffrollConfig       , ContainerType.SingleFile, "bin", "misc", "app_config", "appli_staffroll_config.bin"),
        new(GameFile.AppliTipsConfig            , ContainerType.SingleFile, "bin", "misc", "app_config", "appli_tips_config.bin"),
        new(GameFile.BattleCommonConfig         , ContainerType.SingleFile, "bin", "misc", "app_config", "battle_common_config.bin"),
        new(GameFile.BattleEndConfig            , ContainerType.SingleFile, "bin", "misc", "app_config", "battle_end_config.bin"),
        new(GameFile.BattleInConfig             , ContainerType.SingleFile, "bin", "misc", "app_config", "battle_in_config.bin"),
        new(GameFile.BattleLogicConfig          , ContainerType.SingleFile, "bin", "misc", "app_config", "battle_logic_config.bin"),
        new(GameFile.BattleStartConfig          , ContainerType.SingleFile, "bin", "misc", "app_config", "battle_start_config.bin"),
        new(GameFile.BattleViewConfig           , ContainerType.SingleFile, "bin", "misc", "app_config", "battle_view_config.bin"),
        new(GameFile.BattleVsnsConfig           , ContainerType.SingleFile, "bin", "misc", "app_config", "battle_vsns_config.bin"),
        new(GameFile.BuddyBattleConfig          , ContainerType.SingleFile, "bin", "misc", "app_config", "buddy_battle_config.bin"),
        new(GameFile.BuddyConfig                , ContainerType.SingleFile, "bin", "misc", "app_config", "buddy_config.bin"),
        new(GameFile.BuddyDirectItemConfig      , ContainerType.SingleFile, "bin", "misc", "app_config", "buddy_direct_item_config.bin"),
        new(GameFile.BuddyGroupTalkConfig       , ContainerType.SingleFile, "bin", "misc", "app_config", "buddy_group_talk_config.bin"),
        new(GameFile.BuddyLandmarkConfig        , ContainerType.SingleFile, "bin", "misc", "app_config", "buddy_landmark_config.bin"),
        new(GameFile.BuddyNPCReactionConfig     , ContainerType.SingleFile, "bin", "misc", "app_config", "buddy_npc_reaction_config.bin"),
        new(GameFile.BuddyPlayerModeConfig      , ContainerType.SingleFile, "bin", "misc", "app_config", "buddy_player_mode_config.bin"),
        new(GameFile.BuddyWarpConfig            , ContainerType.SingleFile, "bin", "misc", "app_config", "buddy_warp_config.bin"),
        new(GameFile.CharacterBipedIkConfig     , ContainerType.SingleFile, "bin", "misc", "app_config", "character_biped_ik_config.bin"),
        new(GameFile.CharacterBlinkConfig       , ContainerType.SingleFile, "bin", "misc", "app_config", "character_blink_config.bin"),
        new(GameFile.CharacterControllerConfig  , ContainerType.SingleFile, "bin", "misc", "app_config", "character_controller_config.bin"),
        new(GameFile.CharacterLookAtConfig      , ContainerType.SingleFile, "bin", "misc", "app_config", "character_look_at_config.bin"),
        new(GameFile.CaptureConfig              , ContainerType.SingleFile, "bin", "misc", "app_config", "common_capture_config.bin"),
        new(GameFile.CommonGeneralConfig        , ContainerType.SingleFile, "bin", "misc", "app_config", "common_general_config.bin"),
        new(GameFile.CommonItemConfig           , ContainerType.SingleFile, "bin", "misc", "app_config", "common_item_config.bin"),
        new(GameFile.DemoConfig                 , ContainerType.SingleFile, "bin", "misc", "app_config", "demo_config.bin"),
        new(GameFile.EnvPokeVoiceConfig         , ContainerType.SingleFile, "bin", "misc", "app_config", "env_poke_voice_config.bin"),
        new(GameFile.EventBalloonrunConfig      , ContainerType.SingleFile, "bin", "misc", "app_config", "event_balloonrun_config.bin"),
        new(GameFile.EventBalloonthrowConfig    , ContainerType.SingleFile, "bin", "misc", "app_config", "event_balloonthrow_config.bin"),
        new(GameFile.EventBanditConfig          , ContainerType.SingleFile, "bin", "misc", "app_config", "event_bandit_config.bin"),
        new(GameFile.EventCullingConfig         , ContainerType.SingleFile, "bin", "misc", "app_config", "event_culling_config.bin"),
        new(GameFile.EventDitherConfig          , ContainerType.SingleFile, "bin", "misc", "app_config", "event_dither_config.bin"),
        new(GameFile.EventFarmConfig            , ContainerType.SingleFile, "bin", "misc", "app_config", "event_farm_config.bin"),
        new(GameFile.EventGameOverConfig        , ContainerType.SingleFile, "bin", "misc", "app_config", "event_game_over_config.bin"),
        new(GameFile.EventItemConfig            , ContainerType.SingleFile, "bin", "misc", "app_config", "event_item_config.bin"),
        new(GameFile.EventMkrgRewardConfig      , ContainerType.SingleFile, "bin", "misc", "app_config", "event_mkrg_reward_config.bin"),
        new(GameFile.EventQuestBoardConfig      , ContainerType.SingleFile, "bin", "misc", "app_config", "event_quest_board_config.bin"),
        new(GameFile.EventRestrictionBattle     , ContainerType.SingleFile, "bin", "misc", "app_config", "event_restriction_battle.bin"),
        new(GameFile.EventWork                  , ContainerType.SingleFile, "bin", "misc", "app_config", "event_work.bin"),
        new(GameFile.FieldAnimationFramerateConfig       , ContainerType.SingleFile, "bin", "misc", "app_config", "field_anime_framerate_config.bin"),
        new(GameFile.FieldAreaSpeedConfig                , ContainerType.SingleFile, "bin", "misc", "app_config", "field_area_speed_config.bin"),
        new(GameFile.FieldCameraConfig                   , ContainerType.SingleFile, "bin", "misc", "app_config", "field_camera_config.bin"),
        new(GameFile.FieldCaptureDirectorConfig          , ContainerType.SingleFile, "bin", "misc", "app_config", "field_capture_director_config.bin"),
        new(GameFile.FieldCharaViewerConfig              , ContainerType.SingleFile, "bin", "misc", "app_config", "field_chara_viewer_config.bin"),
        new(GameFile.FieldCommonConfig                   , ContainerType.SingleFile, "bin", "misc", "app_config", "field_common_config.bin"),
        new(GameFile.FieldDirectItemConfig               , ContainerType.SingleFile, "bin", "misc", "app_config", "field_direct_item_config.bin"),
        new(GameFile.FieldEnvConfig                      , ContainerType.SingleFile, "bin", "misc", "app_config", "field_env_config.bin"),
        new(GameFile.OutbreakConfig                      , ContainerType.SingleFile, "bin", "misc", "app_config", "field_huge_outbreak.bin"),
        new(GameFile.FieldItem                           , ContainerType.SingleFile, "bin", "misc", "app_config", "field_item.bin"),
        new(GameFile.FieldItemRespawn                    , ContainerType.SingleFile, "bin", "misc", "app_config", "field_item_respawn.bin"),
        new(GameFile.FieldLandmarkConfig                 , ContainerType.SingleFile, "bin", "misc", "app_config", "field_landmark_config.bin"),
        new(GameFile.FieldLandmarkInciteConfig           , ContainerType.SingleFile, "bin", "misc", "app_config", "field_landmark_incite_config.bin"),
        new(GameFile.FieldLockonConfig                   , ContainerType.SingleFile, "bin", "misc", "app_config", "field_lockon_config.bin"),
        new(GameFile.BallThrowConfig                     , ContainerType.SingleFile, "bin", "misc", "app_config", "field_my_poke_ball_config.bin"),
        new(GameFile.FieldMyPokeBallHitNoneTargetConfig  , ContainerType.SingleFile, "bin", "misc", "app_config", "field_my_poke_ball_hit_none_target_config.bin"),
        new(GameFile.FieldObstructionWazaConfig          , ContainerType.SingleFile, "bin", "misc", "app_config", "field_obstruction_waza_config.bin"),
        new(GameFile.FieldPokemonSlopeConfig             , ContainerType.SingleFile, "bin", "misc", "app_config", "field_pokemon_slope_config.bin"),
        new(GameFile.FieldQuestDestinationConfig         , ContainerType.SingleFile, "bin", "misc", "app_config", "field_quest_destination_config.bin"),
        new(GameFile.FieldShadowConfig                   , ContainerType.SingleFile, "bin", "misc", "app_config", "field_shadow_config.bin"),
        new(GameFile.FieldSpawnerConfig                  , ContainerType.SingleFile, "bin", "misc", "app_config", "field_spawner_config.bin"),
        new(GameFile.FieldThrowConfig                    , ContainerType.SingleFile, "bin", "misc", "app_config", "field_throw_config.bin"),
        new(GameFile.FieldThrowableAfterHitConfig        , ContainerType.SingleFile, "bin", "misc", "app_config", "field_throwable_after_hit_config.bin"),
        new(GameFile.FieldVigilanceBgmConfig             , ContainerType.SingleFile, "bin", "misc", "app_config", "field_vigilance_bgm_config.bin"),
        new(GameFile.FieldWeatheringConfig               , ContainerType.SingleFile, "bin", "misc", "app_config", "field_weathering_config.bin"),
        new(GameFile.FieldWildPokemonConfig              , ContainerType.SingleFile, "bin", "misc", "app_config", "field_wild_pokemon_config.bin"),
        new(GameFile.WormholeConfig                      , ContainerType.SingleFile, "bin", "misc", "app_config", "field_wormhole_config.bin"),
        new(GameFile.NPCAIConfig                   , ContainerType.SingleFile, "bin", "misc", "app_config", "npc_ai_config.bin"),
        new(GameFile.NPCControllerConfig           , ContainerType.SingleFile, "bin", "misc", "app_config", "npc_controller_config.bin"),
        new(GameFile.NPCCreaterConfig              , ContainerType.SingleFile, "bin", "misc", "app_config", "npc_creater_config.bin"),
        new(GameFile.NPCPokemonAIConfig            , ContainerType.SingleFile, "bin", "misc", "app_config", "npc_pokemon_ai_config.bin"),
        new(GameFile.NPCPopupConfig                , ContainerType.SingleFile, "bin", "misc", "app_config", "npc_popup_config.bin"),
        new(GameFile.NPCTalkTableConfig            , ContainerType.SingleFile, "bin", "misc", "app_config", "npc_talk_table_config.bin"),
        new(GameFile.PlayerCameraShakeConfig       , ContainerType.SingleFile, "bin", "misc", "app_config", "player_camera_shake_config.bin"),
        new(GameFile.PlayerCollisionConfig         , ContainerType.SingleFile, "bin", "misc", "app_config", "player_collision_config.bin"),
        new(GameFile.PlayerConfig                  , ContainerType.SingleFile, "bin", "misc", "app_config", "player_config.bin"),
        new(GameFile.PlayerControllerConfig        , ContainerType.SingleFile, "bin", "misc", "app_config", "player_controller_config.bin"),
        new(GameFile.PlayerFaceConfig              , ContainerType.SingleFile, "bin", "misc", "app_config", "player_face_config.bin"),
        new(GameFile.PokemonConfig                 , ContainerType.SingleFile, "bin", "misc", "app_config", "pokemon_config.bin"),
        new(GameFile.PokemonControllerConfig       , ContainerType.SingleFile, "bin", "misc", "app_config", "pokemon_controller_config.bin"),
        new(GameFile.EvolutionConfig               , ContainerType.SingleFile, "bin", "misc", "app_config", "pokemon_evolution_config.bin"),
        new(GameFile.PokemonFriendshipConfig       , ContainerType.SingleFile, "bin", "misc", "app_config", "pokemon_friendship_config.bin"),
        new(GameFile.ShinyRolls                    , ContainerType.SingleFile, "bin", "misc", "app_config", "pokemon_rare.bin"),
        new(GameFile.SizeScaleConfig               , ContainerType.SingleFile, "bin", "misc", "app_config", "pokemon_size_category_adjust_scale_config.bin"),
        new(GameFile.RideBasuraoCollisionConfig    , ContainerType.SingleFile, "bin", "misc", "app_config", "ride_basurao_collision_config.bin"),
        new(GameFile.RideBasuraoConfig             , ContainerType.SingleFile, "bin", "misc", "app_config", "ride_basurao_config.bin"),
        new(GameFile.RideChangeConfig              , ContainerType.SingleFile, "bin", "misc", "app_config", "ride_change_config.bin"),
        new(GameFile.RideCommonConfig              , ContainerType.SingleFile, "bin", "misc", "app_config", "ride_common_config.bin"),
        new(GameFile.RideNyuuraCollisionConfig     , ContainerType.SingleFile, "bin", "misc", "app_config", "ride_nyuura_collision_config.bin"),
        new(GameFile.RideNyuuraConfig              , ContainerType.SingleFile, "bin", "misc", "app_config", "ride_nyuura_config.bin"),
        new(GameFile.RideNyuuraControllerConfig    , ContainerType.SingleFile, "bin", "misc", "app_config", "ride_nyuura_controller_config.bin"),
        new(GameFile.RideOdoshishiCollisionConfig  , ContainerType.SingleFile, "bin", "misc", "app_config", "ride_odoshishi_collision_config.bin"),
        new(GameFile.RideOdoshishiConfig           , ContainerType.SingleFile, "bin", "misc", "app_config", "ride_odoshishi_config.bin"),
        new(GameFile.RideRingumaCollisionConfig    , ContainerType.SingleFile, "bin", "misc", "app_config", "ride_ringuma_collision_config.bin"),
        new(GameFile.RideRingumaConfig             , ContainerType.SingleFile, "bin", "misc", "app_config", "ride_ringuma_config.bin"),
        new(GameFile.RideRingumaControllerConfig   , ContainerType.SingleFile, "bin", "misc", "app_config", "ride_ringuma_controller_config.bin"),
        new(GameFile.RideWhooguruCollisionConfig   , ContainerType.SingleFile, "bin", "misc", "app_config", "ride_whooguru_collision_config.bin"),
        new(GameFile.RideWhooguruConfig            , ContainerType.SingleFile, "bin", "misc", "app_config", "ride_whooguru_config.bin"),
        new(GameFile.RideWhooguruControllerConfig  , ContainerType.SingleFile, "bin", "misc", "app_config", "ride_whooguru_controller_config.bin"),
        new(GameFile.SoundConfig                   , ContainerType.SingleFile, "bin", "misc", "app_config", "sound_config.bin"),
        new(GameFile.WaterMotion                   , ContainerType.SingleFile, "bin", "misc", "app_config", "water_motion.bin"),

        new(GameFile.NewHugeGroup           , ContainerType.SingleFile, "bin", "field", "encount", "new_huge_outbreak_group.bin"),
        new(GameFile.NewHugeGroupLottery    , ContainerType.SingleFile, "bin", "field", "encount", "new_huge_outbreak_group_lottery.bin"),
        new(GameFile.NewHugeLottery         , ContainerType.SingleFile, "bin", "field", "encount", "new_huge_outbreak_lottery.bin"),
        new(GameFile.NewHugeTimeLimit       , ContainerType.SingleFile, "bin", "field", "encount", "new_huge_outbreak_time_limit.bin"),
        // Cutscenes    bin\demo
        // Models       bin\archive\pokemon
        // pretty much everything is obviously named :)
    };
    #endregion
}
