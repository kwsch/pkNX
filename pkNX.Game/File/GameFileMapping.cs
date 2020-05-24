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
        private readonly Dictionary<GameFile, IFileContainer> Cache = new Dictionary<GameFile, IFileContainer>();
        private readonly IReadOnlyCollection<GameFileReference> FileMap;

        public readonly ContainerHandler ProgressTracker = new ContainerHandler();
        public readonly CancellationTokenSource TokenSource = new CancellationTokenSource();

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
                GameVersion.GG => GG,
                GameVersion.SW => SW,
                GameVersion.SH => SH,
                GameVersion.ORASDEMO => AO,
                GameVersion.ORAS => AO,
                GameVersion.SMDEMO => SMDEMO,
                _ => null
            };
        }

        #region Games
        private static readonly GameFileReference[] XY =
        {
            new GameFileReference(005, GameFile.MoveSprites),
            new GameFileReference(012, GameFile.Encounters),
            new GameFileReference(038, GameFile.TrainerData),
            new GameFileReference(039, GameFile.TrainerClass),
            new GameFileReference(040, GameFile.TrainerPoke),
            new GameFileReference(041, GameFile.MapGameRegion),
            new GameFileReference(042, GameFile.MapMatrix),

            new GameFileReference(072, GameFile.GameText0),
            new GameFileReference(073, GameFile.GameText1),
            new GameFileReference(074, GameFile.GameText2),
            new GameFileReference(075, GameFile.GameText3),
            new GameFileReference(076, GameFile.GameText4),
            new GameFileReference(077, GameFile.GameText5),
            new GameFileReference(078, GameFile.GameText6),
            new GameFileReference(079, GameFile.GameText7),
            new GameFileReference(080, GameFile.StoryText0),
            new GameFileReference(081, GameFile.StoryText1),
            new GameFileReference(082, GameFile.StoryText2),
            new GameFileReference(083, GameFile.StoryText3),
            new GameFileReference(084, GameFile.StoryText4),
            new GameFileReference(085, GameFile.StoryText5),
            new GameFileReference(086, GameFile.StoryText6),
            new GameFileReference(087, GameFile.StoryText7),

            new GameFileReference(104, GameFile.Wallpaper),
            new GameFileReference(165, GameFile.TitleScreen),
            new GameFileReference(203, GameFile.FacilityPokeNormal),
            new GameFileReference(204, GameFile.FacilityTrainerNormal),
            new GameFileReference(205, GameFile.FacilityPokeSuper),
            new GameFileReference(206, GameFile.FacilityTrainerSuper),
            new GameFileReference(212, GameFile.MoveStats),
            new GameFileReference(213, GameFile.EggMoves),
            new GameFileReference(214, GameFile.Learnsets),
            new GameFileReference(215, GameFile.Evolutions),
            new GameFileReference(216, GameFile.MegaEvolutions),
            new GameFileReference(218, GameFile.PersonalStats),
            new GameFileReference(220, GameFile.ItemStats),
        };

        private static readonly GameFileReference[] AO =
        {
            new GameFileReference(013, GameFile.Encounters),
            new GameFileReference(036, GameFile.TrainerData),
            new GameFileReference(037, GameFile.TrainerClass),
            new GameFileReference(038, GameFile.TrainerPoke),
            new GameFileReference(039, GameFile.MapGameRegion),
            new GameFileReference(040, GameFile.MapMatrix),

            new GameFileReference(071, GameFile.GameText0),
            new GameFileReference(072, GameFile.GameText1),
            new GameFileReference(073, GameFile.GameText2),
            new GameFileReference(074, GameFile.GameText3),
            new GameFileReference(075, GameFile.GameText4),
            new GameFileReference(076, GameFile.GameText5),
            new GameFileReference(077, GameFile.GameText6),
            new GameFileReference(078, GameFile.GameText7),
            new GameFileReference(079, GameFile.StoryText0),
            new GameFileReference(080, GameFile.StoryText1),
            new GameFileReference(081, GameFile.StoryText2),
            new GameFileReference(082, GameFile.StoryText3),
            new GameFileReference(083, GameFile.StoryText4),
            new GameFileReference(084, GameFile.StoryText5),
            new GameFileReference(085, GameFile.StoryText6),
            new GameFileReference(086, GameFile.StoryText7),

            new GameFileReference(103, GameFile.Wallpaper),
            new GameFileReference(152, GameFile.TitleScreen),
            new GameFileReference(182, GameFile.FacilityPokeNormal),
            new GameFileReference(183, GameFile.FacilityTrainerNormal),
            new GameFileReference(184, GameFile.FacilityPokeSuper),
            new GameFileReference(185, GameFile.FacilityTrainerSuper),
            new GameFileReference(189, GameFile.MoveStats),
            new GameFileReference(190, GameFile.EggMoves),
            new GameFileReference(191, GameFile.Learnsets),
            new GameFileReference(192, GameFile.Evolutions),
            new GameFileReference(193, GameFile.MegaEvolutions),
            new GameFileReference(195, GameFile.PersonalStats),
            new GameFileReference(197, GameFile.ItemStats),
        };
        #endregion

        #region Gen7
        private static readonly GameFileReference[] SMDEMO =
        {
            new GameFileReference(011, GameFile.MoveStats),
            new GameFileReference(012, GameFile.EggMoves),
            new GameFileReference(013, GameFile.Learnsets),
            new GameFileReference(014, GameFile.Evolutions),
            new GameFileReference(015, GameFile.MegaEvolutions),
            new GameFileReference(017, GameFile.PersonalStats),
            new GameFileReference(019, GameFile.ItemStats),

            new GameFileReference(030, GameFile.GameText0),
            new GameFileReference(031, GameFile.GameText1),
            new GameFileReference(032, GameFile.GameText2),
            new GameFileReference(033, GameFile.GameText3),
            new GameFileReference(034, GameFile.GameText4),
            new GameFileReference(035, GameFile.GameText5),
            new GameFileReference(036, GameFile.GameText6),
            new GameFileReference(037, GameFile.GameText7),
            new GameFileReference(038, GameFile.GameText8),
            new GameFileReference(039, GameFile.GameText9),
            new GameFileReference(040, GameFile.StoryText0),
            new GameFileReference(041, GameFile.StoryText1),
            new GameFileReference(042, GameFile.StoryText2),
            new GameFileReference(043, GameFile.StoryText3),
            new GameFileReference(044, GameFile.StoryText4),
            new GameFileReference(045, GameFile.StoryText5),
            new GameFileReference(046, GameFile.StoryText6),
            new GameFileReference(047, GameFile.StoryText7),
            new GameFileReference(048, GameFile.StoryText8),
            new GameFileReference(049, GameFile.StoryText9),

            new GameFileReference(076, GameFile.ZoneData),
            new GameFileReference(091, GameFile.WorldData),

            new GameFileReference(101, GameFile.TrainerClass),
            new GameFileReference(102, GameFile.TrainerData),
            new GameFileReference(103, GameFile.TrainerPoke),
        };

        private static readonly GameFileReference[] SM =
        {
            new GameFileReference(011, GameFile.MoveStats),
            new GameFileReference(012, GameFile.EggMoves),
            new GameFileReference(013, GameFile.Learnsets),
            new GameFileReference(014, GameFile.Evolutions),
            new GameFileReference(015, GameFile.MegaEvolutions),
            new GameFileReference(017, GameFile.PersonalStats),
            new GameFileReference(019, GameFile.ItemStats),

            new GameFileReference(030, GameFile.GameText0),
            new GameFileReference(031, GameFile.GameText1),
            new GameFileReference(032, GameFile.GameText2),
            new GameFileReference(033, GameFile.GameText3),
            new GameFileReference(034, GameFile.GameText4),
            new GameFileReference(035, GameFile.GameText5),
            new GameFileReference(036, GameFile.GameText6),
            new GameFileReference(037, GameFile.GameText7),
            new GameFileReference(038, GameFile.GameText8),
            new GameFileReference(039, GameFile.GameText9),
            new GameFileReference(040, GameFile.StoryText0),
            new GameFileReference(041, GameFile.StoryText1),
            new GameFileReference(042, GameFile.StoryText2),
            new GameFileReference(043, GameFile.StoryText3),
            new GameFileReference(044, GameFile.StoryText4),
            new GameFileReference(045, GameFile.StoryText5),
            new GameFileReference(046, GameFile.StoryText6),
            new GameFileReference(047, GameFile.StoryText7),
            new GameFileReference(048, GameFile.StoryText8),
            new GameFileReference(049, GameFile.StoryText9),

            new GameFileReference(077, GameFile.ZoneData),
            new GameFileReference(091, GameFile.WorldData),

            new GameFileReference(104, GameFile.TrainerClass),
            new GameFileReference(105, GameFile.TrainerData),
            new GameFileReference(106, GameFile.TrainerPoke),

            new GameFileReference(155, GameFile.EncounterStatic),

            new GameFileReference(267, GameFile.Pickup),

            new GameFileReference(277, GameFile.FacilityPokeNormal),
            new GameFileReference(278, GameFile.FacilityTrainerNormal),
            new GameFileReference(279, GameFile.FacilityPokeSuper),
            new GameFileReference(280, GameFile.FacilityTrainerSuper),
        };

        /// <summary>
        /// Ultra Sun &amp; Ultra Moon
        /// </summary>
        private static readonly GameFileReference[] UU =
        {
            new GameFileReference(011, GameFile.MoveStats),
            new GameFileReference(012, GameFile.EggMoves),
            new GameFileReference(013, GameFile.Learnsets),
            new GameFileReference(014, GameFile.Evolutions),
            new GameFileReference(015, GameFile.MegaEvolutions),
            new GameFileReference(017, GameFile.PersonalStats),
            new GameFileReference(019, GameFile.ItemStats),

            new GameFileReference(030, GameFile.GameText0),
            new GameFileReference(031, GameFile.GameText1),
            new GameFileReference(032, GameFile.GameText2),
            new GameFileReference(033, GameFile.GameText3),
            new GameFileReference(034, GameFile.GameText4),
            new GameFileReference(035, GameFile.GameText5),
            new GameFileReference(036, GameFile.GameText6),
            new GameFileReference(037, GameFile.GameText7),
            new GameFileReference(038, GameFile.GameText8),
            new GameFileReference(039, GameFile.GameText9),
            new GameFileReference(040, GameFile.StoryText0),
            new GameFileReference(041, GameFile.StoryText1),
            new GameFileReference(042, GameFile.StoryText2),
            new GameFileReference(043, GameFile.StoryText3),
            new GameFileReference(044, GameFile.StoryText4),
            new GameFileReference(045, GameFile.StoryText5),
            new GameFileReference(046, GameFile.StoryText6),
            new GameFileReference(047, GameFile.StoryText7),
            new GameFileReference(048, GameFile.StoryText8),
            new GameFileReference(049, GameFile.StoryText9),

            new GameFileReference(077, GameFile.ZoneData),
            new GameFileReference(091, GameFile.WorldData),

            new GameFileReference(105, GameFile.TrainerClass),
            new GameFileReference(106, GameFile.TrainerData),
            new GameFileReference(107, GameFile.TrainerPoke),

            new GameFileReference(159, GameFile.EncounterStatic),

            new GameFileReference(271, GameFile.Pickup),

            new GameFileReference(281, GameFile.FacilityPokeNormal),
            new GameFileReference(282, GameFile.FacilityTrainerNormal),
            new GameFileReference(283, GameFile.FacilityPokeSuper),
            new GameFileReference(284, GameFile.FacilityTrainerSuper),
        };

        /// <summary>
        /// Let's Go Pikachu &amp; Let's Go Eevee
        /// </summary>
        private static readonly GameFileReference[] GG =
        {
            new GameFileReference(GameFile.TrainerData, "bin", "trainer", "trainer_data"),
            new GameFileReference(GameFile.TrainerPoke, "bin", "trainer", "trainer_poke"),
            new GameFileReference(GameFile.TrainerClass, "bin", "trainer", "trainer_type"),

            new GameFileReference(GameFile.GameText0, 0, "bin", "message", "JPN", "common"),
            new GameFileReference(GameFile.GameText1, 1, "bin", "message", "JPN_KANJI", "common"),
            new GameFileReference(GameFile.GameText2, 2, "bin", "message", "English", "common"),
            new GameFileReference(GameFile.GameText3, 3, "bin", "message", "French", "common"),
            new GameFileReference(GameFile.GameText4, 4, "bin", "message", "Italian", "common"),
            new GameFileReference(GameFile.GameText5, 5, "bin", "message", "German", "common"),
            // 6 unused lang
            new GameFileReference(GameFile.GameText6, 7, "bin", "message", "Spanish", "common"),
            new GameFileReference(GameFile.GameText7, 8, "bin", "message", "Korean", "common"),
            new GameFileReference(GameFile.GameText8, 9, "bin", "message", "Simp_Chinese", "common"),
            new GameFileReference(GameFile.GameText9, 10, "bin", "message", "Trad_Chinese", "common"),

            new GameFileReference(GameFile.StoryText0, 0, "bin", "message", "JPN", "script"),
            new GameFileReference(GameFile.StoryText1, 1, "bin", "message", "JPN_KANJI", "script"),
            new GameFileReference(GameFile.StoryText2, 2, "bin", "message", "English", "script"),
            new GameFileReference(GameFile.StoryText3, 3, "bin", "message", "French", "script"),
            new GameFileReference(GameFile.StoryText4, 4, "bin", "message", "Italian", "script"),
            new GameFileReference(GameFile.StoryText5, 5, "bin", "message", "German", "script"),
            // 6 unused lang
            new GameFileReference(GameFile.StoryText6, 7, "bin", "message", "Spanish", "script"),
            new GameFileReference(GameFile.StoryText7, 8, "bin", "message", "Korean", "script"),
            new GameFileReference(GameFile.StoryText8, 9, "bin", "message", "Simp_Chinese", "script"),
            new GameFileReference(GameFile.StoryText9, 10, "bin", "message", "Trad_Chinese", "script"),

            new GameFileReference(GameFile.ItemStats, "bin", "pokelib", "item"),
            new GameFileReference(GameFile.Evolutions, "bin", "pokelib", "evolution"),
            new GameFileReference(GameFile.PersonalStats, "bin", "pokelib", "personal"),
            new GameFileReference(GameFile.MegaEvolutions, "bin", "pokelib", "mega_evolution"),
            new GameFileReference(GameFile.MoveStats, ContainerType.Mini, "bin", "pokelib", "waza", "waza_data.bin"),
            new GameFileReference(GameFile.EncounterStatic, ContainerType.SingleFile, "bin", "script_event_data", "event_encount.bin"),
            new GameFileReference(GameFile.EncounterTrade, ContainerType.SingleFile, "bin", "script_event_data", "field_trade_data.bin"),
            new GameFileReference(GameFile.EncounterGift, ContainerType.SingleFile, "bin", "script_event_data", "add_poke.bin"),
            new GameFileReference(GameFile.Learnsets, ContainerType.GFPack, "bin", "archive", "waza_oboe.gfpak"),

            new GameFileReference(GameFile.WildData1, ContainerType.SingleFile, "bin", "field", "param", "encount", "encount_data_p.bin"),
            new GameFileReference(GameFile.WildData2, ContainerType.SingleFile, "bin", "field", "param", "encount", "encount_data_e.bin"),

            // Cutscenes    bin\demo
            // Models       bin\archive\pokemon
            // pretty much everything is obviously named :)
        };

        /// <summary>
        /// Sword
        /// </summary>
        private static readonly GameFileReference[] SW =
        {
            new GameFileReference(GameFile.TrainerData, "bin", "trainer", "trainer_data"),
            new GameFileReference(GameFile.TrainerPoke, "bin", "trainer", "trainer_poke"),
            new GameFileReference(GameFile.TrainerClass, "bin", "trainer", "trainer_type"),

            new GameFileReference(GameFile.GameText0, 0, "bin", "message", "JPN", "common"),
            new GameFileReference(GameFile.GameText1, 1, "bin", "message", "JPN_KANJI", "common"),
            new GameFileReference(GameFile.GameText2, 2, "bin", "message", "English", "common"),
            new GameFileReference(GameFile.GameText3, 3, "bin", "message", "French", "common"),
            new GameFileReference(GameFile.GameText4, 4, "bin", "message", "Italian", "common"),
            new GameFileReference(GameFile.GameText5, 5, "bin", "message", "German", "common"),
            // 6 unused lang
            new GameFileReference(GameFile.GameText6, 7, "bin", "message", "Spanish", "common"),
            new GameFileReference(GameFile.GameText7, 8, "bin", "message", "Korean", "common"),
            new GameFileReference(GameFile.GameText8, 9, "bin", "message", "Simp_Chinese", "common"),
            new GameFileReference(GameFile.GameText9, 10, "bin", "message", "Trad_Chinese", "common"),

            new GameFileReference(GameFile.StoryText0, 0, "bin", "message", "JPN", "script"),
            new GameFileReference(GameFile.StoryText1, 1, "bin", "message", "JPN_KANJI", "script"),
            new GameFileReference(GameFile.StoryText2, 2, "bin", "message", "English", "script"),
            new GameFileReference(GameFile.StoryText3, 3, "bin", "message", "French", "script"),
            new GameFileReference(GameFile.StoryText4, 4, "bin", "message", "Italian", "script"),
            new GameFileReference(GameFile.StoryText5, 5, "bin", "message", "German", "script"),
            // 6 unused lang
            new GameFileReference(GameFile.StoryText6, 7, "bin", "message", "Spanish", "script"),
            new GameFileReference(GameFile.StoryText7, 8, "bin", "message", "Korean", "script"),
            new GameFileReference(GameFile.StoryText8, 9, "bin", "message", "Simp_Chinese", "script"),
            new GameFileReference(GameFile.StoryText9, 10, "bin", "message", "Trad_Chinese", "script"),

            new GameFileReference(GameFile.ItemStats, ContainerType.SingleFile, "bin", "pml", "item", "item.dat"),
            new GameFileReference(GameFile.Evolutions, "bin", "pml", "evolution"),
            new GameFileReference(GameFile.EggMoves, "bin", "pml", "tamagowaza"),
            new GameFileReference(GameFile.PersonalStats, "bin", "pml", "personal"),
            new GameFileReference(GameFile.MoveStats, "bin", "pml", "waza"),
            new GameFileReference(GameFile.EncounterStatic, ContainerType.SingleFile, "bin", "script_event_data", "event_encount_data.bin"),
            new GameFileReference(GameFile.EncounterTrade, ContainerType.SingleFile, "bin", "script_event_data", "field_trade.bin"),
            new GameFileReference(GameFile.EncounterGift, ContainerType.SingleFile, "bin", "script_event_data", "add_poke.bin"),
            new GameFileReference(GameFile.Learnsets, ContainerType.SingleFile, "bin", "pml", "waza_oboe", "wazaoboe_total.bin"),

            new GameFileReference(GameFile.FacilityPokeNormal, ContainerType.SingleFile, "bin", "field", "param", "battle_tower", "battle_tower_poke_table.bin"),
            new GameFileReference(GameFile.FacilityTrainerNormal, ContainerType.SingleFile, "bin", "field", "param", "battle_tower", "battle_tower_trainer_table.bin"),

            new GameFileReference(GameFile.WildData, ContainerType.SingleFile, "bin", "archive", "field", "resident", "data_table.gfpak"),
            new GameFileReference(GameFile.NestData, ContainerType.SingleFile, "bin", "archive", "field", "resident", "data_table.gfpak"),

            // Cutscenes    bin\demo
            // Models       bin\archive\pokemon
            // pretty much everything is obviously named :)
        };

        /// <summary>
        /// Shield
        /// </summary>
        private static readonly GameFileReference[] SH = SW;
        #endregion

        #region Split Versions
        private static readonly GameFileReference[] SN = SM.Concat(new[] {new GameFileReference(082, GameFile.Encounters)}).ToArray();
        private static readonly GameFileReference[] MN = SM.Concat(new[] {new GameFileReference(083, GameFile.Encounters)}).ToArray();
        private static readonly GameFileReference[] US = UU.Concat(new[] {new GameFileReference(082, GameFile.Encounters)}).ToArray();
        private static readonly GameFileReference[] UM = UU.Concat(new[] {new GameFileReference(083, GameFile.Encounters)}).ToArray();
        #endregion
    }
}
