﻿using System;
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
            if (file == GameFile.GameText || file == GameFile.StoryText)
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

        public static IReadOnlyCollection<GameFileReference> GetMapping(GameVersion game)
        {
            return game switch
            {
                GameVersion.SN => SN,
                GameVersion.MN => MN,
                GameVersion.US => US,
                GameVersion.UM => UM,
                GameVersion.XY => XY,
                GameVersion.GP => GG,
                GameVersion.GE => GG,
                GameVersion.GG => GG,
                GameVersion.SW => SWSH,
                GameVersion.SH => SWSH,
                GameVersion.SWSH => SWSH,
                GameVersion.ORASDEMO => AO,
                GameVersion.ORAS => AO,
                GameVersion.SMDEMO => SMDEMO,
                _ => null
            };
        }

        #region Games
        private static readonly GameFileReference[] XY =
        {
            new(005, GameFile.MoveSprites),
            new(012, GameFile.Encounters),
            new(038, GameFile.TrainerData),
            new(039, GameFile.TrainerClass),
            new(040, GameFile.TrainerPoke),
            new(041, GameFile.MapGameRegion),
            new(042, GameFile.MapMatrix),

            new(072, GameFile.GameText0),
            new(073, GameFile.GameText1),
            new(074, GameFile.GameText2),
            new(075, GameFile.GameText3),
            new(076, GameFile.GameText4),
            new(077, GameFile.GameText5),
            new(078, GameFile.GameText6),
            new(079, GameFile.GameText7),
            new(080, GameFile.StoryText0),
            new(081, GameFile.StoryText1),
            new(082, GameFile.StoryText2),
            new(083, GameFile.StoryText3),
            new(084, GameFile.StoryText4),
            new(085, GameFile.StoryText5),
            new(086, GameFile.StoryText6),
            new(087, GameFile.StoryText7),

            new(104, GameFile.Wallpaper),
            new(165, GameFile.TitleScreen),
            new(203, GameFile.FacilityPokeNormal),
            new(204, GameFile.FacilityTrainerNormal),
            new(205, GameFile.FacilityPokeSuper),
            new(206, GameFile.FacilityTrainerSuper),
            new(212, GameFile.MoveStats),
            new(213, GameFile.EggMoves),
            new(214, GameFile.Learnsets),
            new(215, GameFile.Evolutions),
            new(216, GameFile.MegaEvolutions),
            new(218, GameFile.PersonalStats),
            new(220, GameFile.ItemStats),
        };

        private static readonly GameFileReference[] AO =
        {
            new(013, GameFile.Encounters),
            new(036, GameFile.TrainerData),
            new(037, GameFile.TrainerClass),
            new(038, GameFile.TrainerPoke),
            new(039, GameFile.MapGameRegion),
            new(040, GameFile.MapMatrix),

            new(071, GameFile.GameText0),
            new(072, GameFile.GameText1),
            new(073, GameFile.GameText2),
            new(074, GameFile.GameText3),
            new(075, GameFile.GameText4),
            new(076, GameFile.GameText5),
            new(077, GameFile.GameText6),
            new(078, GameFile.GameText7),
            new(079, GameFile.StoryText0),
            new(080, GameFile.StoryText1),
            new(081, GameFile.StoryText2),
            new(082, GameFile.StoryText3),
            new(083, GameFile.StoryText4),
            new(084, GameFile.StoryText5),
            new(085, GameFile.StoryText6),
            new(086, GameFile.StoryText7),

            new(103, GameFile.Wallpaper),
            new(152, GameFile.TitleScreen),
            new(182, GameFile.FacilityPokeNormal),
            new(183, GameFile.FacilityTrainerNormal),
            new(184, GameFile.FacilityPokeSuper),
            new(185, GameFile.FacilityTrainerSuper),
            new(189, GameFile.MoveStats),
            new(190, GameFile.EggMoves),
            new(191, GameFile.Learnsets),
            new(192, GameFile.Evolutions),
            new(193, GameFile.MegaEvolutions),
            new(195, GameFile.PersonalStats),
            new(197, GameFile.ItemStats),
        };
        #endregion

        #region Gen7
        private static readonly GameFileReference[] SMDEMO =
        {
            new(011, GameFile.MoveStats),
            new(012, GameFile.EggMoves),
            new(013, GameFile.Learnsets),
            new(014, GameFile.Evolutions),
            new(015, GameFile.MegaEvolutions),
            new(017, GameFile.PersonalStats),
            new(019, GameFile.ItemStats),

            new(030, GameFile.GameText0),
            new(031, GameFile.GameText1),
            new(032, GameFile.GameText2),
            new(033, GameFile.GameText3),
            new(034, GameFile.GameText4),
            new(035, GameFile.GameText5),
            new(036, GameFile.GameText6),
            new(037, GameFile.GameText7),
            new(038, GameFile.GameText8),
            new(039, GameFile.GameText9),
            new(040, GameFile.StoryText0),
            new(041, GameFile.StoryText1),
            new(042, GameFile.StoryText2),
            new(043, GameFile.StoryText3),
            new(044, GameFile.StoryText4),
            new(045, GameFile.StoryText5),
            new(046, GameFile.StoryText6),
            new(047, GameFile.StoryText7),
            new(048, GameFile.StoryText8),
            new(049, GameFile.StoryText9),

            new(076, GameFile.ZoneData),
            new(091, GameFile.WorldData),

            new(101, GameFile.TrainerClass),
            new(102, GameFile.TrainerData),
            new(103, GameFile.TrainerPoke),
        };

        private static readonly GameFileReference[] SM =
        {
            new(011, GameFile.MoveStats),
            new(012, GameFile.EggMoves),
            new(013, GameFile.Learnsets),
            new(014, GameFile.Evolutions),
            new(015, GameFile.MegaEvolutions),
            new(017, GameFile.PersonalStats),
            new(019, GameFile.ItemStats),

            new(030, GameFile.GameText0),
            new(031, GameFile.GameText1),
            new(032, GameFile.GameText2),
            new(033, GameFile.GameText3),
            new(034, GameFile.GameText4),
            new(035, GameFile.GameText5),
            new(036, GameFile.GameText6),
            new(037, GameFile.GameText7),
            new(038, GameFile.GameText8),
            new(039, GameFile.GameText9),
            new(040, GameFile.StoryText0),
            new(041, GameFile.StoryText1),
            new(042, GameFile.StoryText2),
            new(043, GameFile.StoryText3),
            new(044, GameFile.StoryText4),
            new(045, GameFile.StoryText5),
            new(046, GameFile.StoryText6),
            new(047, GameFile.StoryText7),
            new(048, GameFile.StoryText8),
            new(049, GameFile.StoryText9),

            new(077, GameFile.ZoneData),
            new(091, GameFile.WorldData),

            new(104, GameFile.TrainerClass),
            new(105, GameFile.TrainerData),
            new(106, GameFile.TrainerPoke),

            new(155, GameFile.EncounterStatic),

            new(267, GameFile.Pickup),

            new(277, GameFile.FacilityPokeNormal),
            new(278, GameFile.FacilityTrainerNormal),
            new(279, GameFile.FacilityPokeSuper),
            new(280, GameFile.FacilityTrainerSuper),
        };

        /// <summary>
        /// Ultra Sun &amp; Ultra Moon
        /// </summary>
        private static readonly GameFileReference[] UU =
        {
            new(011, GameFile.MoveStats),
            new(012, GameFile.EggMoves),
            new(013, GameFile.Learnsets),
            new(014, GameFile.Evolutions),
            new(015, GameFile.MegaEvolutions),
            new(017, GameFile.PersonalStats),
            new(019, GameFile.ItemStats),

            new(030, GameFile.GameText0),
            new(031, GameFile.GameText1),
            new(032, GameFile.GameText2),
            new(033, GameFile.GameText3),
            new(034, GameFile.GameText4),
            new(035, GameFile.GameText5),
            new(036, GameFile.GameText6),
            new(037, GameFile.GameText7),
            new(038, GameFile.GameText8),
            new(039, GameFile.GameText9),
            new(040, GameFile.StoryText0),
            new(041, GameFile.StoryText1),
            new(042, GameFile.StoryText2),
            new(043, GameFile.StoryText3),
            new(044, GameFile.StoryText4),
            new(045, GameFile.StoryText5),
            new(046, GameFile.StoryText6),
            new(047, GameFile.StoryText7),
            new(048, GameFile.StoryText8),
            new(049, GameFile.StoryText9),

            new(077, GameFile.ZoneData),
            new(091, GameFile.WorldData),

            new(105, GameFile.TrainerClass),
            new(106, GameFile.TrainerData),
            new(107, GameFile.TrainerPoke),

            new(159, GameFile.EncounterStatic),

            new(271, GameFile.Pickup),

            new(281, GameFile.FacilityPokeNormal),
            new(282, GameFile.FacilityTrainerNormal),
            new(283, GameFile.FacilityPokeSuper),
            new(284, GameFile.FacilityTrainerSuper),
        };

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
        #endregion

        #region Split Versions
        private static readonly GameFileReference[] SN = SM.Concat(new[] {new GameFileReference(082, GameFile.Encounters)}).ToArray();
        private static readonly GameFileReference[] MN = SM.Concat(new[] {new GameFileReference(083, GameFile.Encounters)}).ToArray();
        private static readonly GameFileReference[] US = UU.Concat(new[] {new GameFileReference(082, GameFile.Encounters)}).ToArray();
        private static readonly GameFileReference[] UM = UU.Concat(new[] {new GameFileReference(083, GameFile.Encounters)}).ToArray();
        #endregion
    }
}
