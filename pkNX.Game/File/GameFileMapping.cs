using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using pkNX.Containers;
using pkNX.Structures;

namespace pkNX.Game
{
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
                    c.SaveAs(c.FilePath, ProgressTracker, TokenSource.Token).RunSynchronously();
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
            new(GameFile.SymbolBehave, ContainerType.SingleFile, "bin", "field", "param", "symbol_encount_mons_param", "symbol_encount_mons_param.bin")

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

            new(GameFile.EncounterRateTable, ContainerType.SingleFile, "bin", "pokemon", "data", "poke_encount.bin"),
            new(GameFile.PokeMisc, ContainerType.SingleFile, "bin", "pokemon", "data", "poke_misc.bin"),
            new(GameFile.Outbreak, ContainerType.SingleFile, "bin", "field", "encount", "huge_outbreak.bin"),
            new(GameFile.MoveShop, ContainerType.SingleFile, "bin", "appli", "wazaremember", "bin", "wazashop_table.bin"),

            new(GameFile.ThrowableParam, ContainerType.SingleFile, "bin", "capture", "throwable_param_table.bin"),
            new(GameFile.ThrowParam, ContainerType.SingleFile, "bin", "capture", "throw_param_table.bin"),
            new(GameFile.ThrowableResourceSet, ContainerType.SingleFile, "bin", "capture", "throwable_resourceset_dictionary.bin"),
            new(GameFile.ThrowableResource, ContainerType.SingleFile, "bin", "capture", "throwable_resource_dictionary.bin"),
            new(GameFile.ThrowPermissionSet, ContainerType.SingleFile, "bin", "capture", "throw_permissionset_dictionary.bin"),
            new(GameFile.HaShop, ContainerType.SingleFile, "bin", "appli", "shop", "bin", "ha_shop_data.bin"),

            new(GameFile.ShinyRolls, ContainerType.SingleFile, "bin", "misc", "app_config", "pokemon_rare.bin"),
            new(GameFile.WormholeConfig, ContainerType.SingleFile, "bin", "misc", "app_config", "field_wormhole_config.bin"),
            new(GameFile.CaptureConfig, ContainerType.SingleFile, "bin", "misc", "app_config", "common_capture_config.bin"),
            new(GameFile.BattleLogicConfig, ContainerType.SingleFile, "bin", "misc", "app_config", "battle_logic_config.bin"),
            new(GameFile.PlayerConfig, ContainerType.SingleFile, "bin", "misc", "app_config", "player_config.bin"),
            new(GameFile.EventFarmConfig, ContainerType.SingleFile, "bin", "misc", "app_config", "event_farm_config.bin"),
            new(GameFile.AppConfigList, ContainerType.SingleFile, "bin", "misc", "app_config", "app_config_list.bin"),
            new(GameFile.FieldLandmarkConfig, ContainerType.SingleFile, "bin", "misc", "app_config", "field_landmark_config.bin"),
            new(GameFile.BattleViewConfig, ContainerType.SingleFile, "bin", "misc", "app_config", "battle_view_config.bin"),
            new(GameFile.AICommonConfig, ContainerType.SingleFile, "bin", "misc", "app_config", "ai_common_config.bin"),
            new(GameFile.FieldSpawnerConfig, ContainerType.SingleFile, "bin", "misc", "app_config", "field_spawner_config.bin"),
            new(GameFile.OutbreakConfig, ContainerType.SingleFile, "bin", "misc", "app_config", "field_huge_outbreak.bin"),
            new(GameFile.EvolutionConfig, ContainerType.SingleFile, "bin", "misc", "app_config", "pokemon_evolution_config.bin"),
            new(GameFile.BallThrowConfig, ContainerType.SingleFile, "bin", "misc", "app_config", "field_my_poke_ball_config.bin"),
            new(GameFile.SizeScaleConfig, ContainerType.SingleFile, "bin", "misc", "app_config", "pokemon_size_category_adjust_scale_config.bin"),
            new(GameFile.SymbolBehave, ContainerType.SingleFile, "bin", "pokemon", "data", "poke_ai.bin"),

            new(GameFile.NewHugeGroup, ContainerType.SingleFile, "bin", "field", "encount", "new_huge_outbreak_group.bin"),
            new(GameFile.NewHugeGroupLottery, ContainerType.SingleFile, "bin", "field", "encount", "new_huge_outbreak_group_lottery.bin"),
            new(GameFile.NewHugeLottery, ContainerType.SingleFile, "bin", "field", "encount", "new_huge_outbreak_lottery.bin"),
            new(GameFile.NewHugeTimeLimit, ContainerType.SingleFile, "bin", "field", "encount", "new_huge_outbreak_time_limit.bin"),

            // Cutscenes    bin\demo
            // Models       bin\archive\pokemon
            // pretty much everything is obviously named :)
        };
        #endregion
    }
}
