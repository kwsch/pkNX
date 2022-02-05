using System.Collections.Generic;
using pkNX.Structures;

namespace pkNX.Game
{
    public static class TextMapping
    {
        public static IReadOnlyCollection<TextReference> GetMapping(GameVersion game)
        {
            return game switch
            {
                GameVersion.XY => XY,
                GameVersion.ORASDEMO => AO,
                GameVersion.ORAS => AO,
                GameVersion.SMDEMO => SMDEMO,
                GameVersion.SN => SM,
                GameVersion.MN => SM,
                GameVersion.US => USUM,
                GameVersion.UM => USUM,
                GameVersion.GP => GG,
                GameVersion.GE => GG,
                GameVersion.GG => GG,
                GameVersion.SW => SWSH,
                GameVersion.SH => SWSH,
                GameVersion.SWSH => SWSH,
                GameVersion.PLA => PLA,
                _ => null
            };
        }

        private static readonly TextReference[] XY =
        {
            new(005, TextName.Forms),
            new(013, TextName.MoveNames),
            new(015, TextName.MoveFlavor),
            new(017, TextName.Types),
            new(020, TextName.TrainerClasses),
            new(021, TextName.TrainerNames),
            new(022, TextName.TrainerText),
            new(034, TextName.AbilityNames),
            new(047, TextName.Natures),
            new(072, TextName.metlist_00000),
            new(080, TextName.SpeciesNames),
            new(096, TextName.ItemNames),
            new(099, TextName.ItemFlavor),
            new(130, TextName.MaisonTrainerNames),
            new(131, TextName.SuperTrainerNames),
            new(141, TextName.OPowerFlavor),
        };

        private static readonly TextReference[] AO =
        {
            new(005, TextName.Forms),
            new(014, TextName.MoveNames),
            new(016, TextName.MoveFlavor),
            new(018, TextName.Types),
            new(021, TextName.TrainerClasses),
            new(022, TextName.TrainerNames),
            new(023, TextName.TrainerText),
            new(037, TextName.AbilityNames),
            new(051, TextName.Natures),
            new(090, TextName.metlist_00000),
            new(098, TextName.SpeciesNames),
            new(114, TextName.ItemNames),
            new(117, TextName.ItemFlavor),
            new(153, TextName.MaisonTrainerNames),
            new(154, TextName.SuperTrainerNames),
            new(165, TextName.OPowerFlavor),
        };

        private static readonly TextReference[] SMDEMO =
        {
            new(020, TextName.ItemFlavor),
            new(021, TextName.ItemNames),
            new(026, TextName.SpeciesNames),
            new(030, TextName.metlist_00000),
            new(044, TextName.Forms),
            new(044, TextName.Natures),
            new(046, TextName.AbilityNames),
            new(049, TextName.TrainerText),
            new(050, TextName.TrainerNames),
            new(051, TextName.TrainerClasses),
            new(052, TextName.Types),
            new(054, TextName.MoveFlavor),
            new(055, TextName.MoveNames),
        };

        private static readonly TextReference[] SM =
        {
            new(035, TextName.ItemFlavor),
            new(036, TextName.ItemNames),
            new(055, TextName.SpeciesNames),
            new(067, TextName.metlist_00000),
            new(086, TextName.BattleRoyalNames),
            new(087, TextName.Natures),
            new(096, TextName.AbilityNames),
            new(099, TextName.BattleTreeNames),
            new(104, TextName.TrainerText),
            new(105, TextName.TrainerNames),
            new(106, TextName.TrainerClasses),
            new(107, TextName.Types),
            new(112, TextName.MoveFlavor),
            new(113, TextName.MoveNames),
            new(114, TextName.Forms),
            new(116, TextName.SpeciesClassifications),
            new(119, TextName.PokedexEntry1),
            new(120, TextName.PokedexEntry2)
        };

        private static readonly TextReference[] USUM =
        {
            new(039, TextName.ItemFlavor),
            new(040, TextName.ItemNames),
            new(060, TextName.SpeciesNames),
            new(072, TextName.metlist_00000),
            new(091, TextName.BattleRoyalNames),
            new(092, TextName.Natures),
            new(101, TextName.AbilityNames),
            new(104, TextName.BattleTreeNames),
            new(109, TextName.TrainerText),
            new(110, TextName.TrainerNames),
            new(111, TextName.TrainerClasses),
            new(112, TextName.Types),
            new(117, TextName.MoveFlavor),
            new(118, TextName.MoveNames),
            new(119, TextName.Forms),
            new(121, TextName.SpeciesClassifications),
            new(124, TextName.PokedexEntry1),
            new(125, TextName.PokedexEntry2)
        };

        private static readonly TextReference[] GG =
        {
            new("iteminfo.dat", TextName.ItemFlavor),
            new("itemname.dat", TextName.ItemNames),
            new("monsname.dat", TextName.SpeciesNames),
            new("place_name.dat", TextName.metlist_00000),
            new("seikaku.dat", TextName.Natures),
            new("tokusei.dat", TextName.AbilityNames),
            new("tokuseiinfo.dat", TextName.AbilityFlavor),
            new("trname.dat", TextName.TrainerNames),
            new("trtype.dat", TextName.TrainerClasses),
            new("trmsg.dat", TextName.TrainerText),
            new("typename.dat", TextName.Types),
            new("wazainfo.dat", TextName.MoveFlavor),
            new("wazaname.dat", TextName.MoveNames),
            new("zkn_form.dat", TextName.Forms),
            new("zkn_type.dat", TextName.SpeciesClassifications),
            new("zukan_comment_A.dat", TextName.PokedexEntry1),
        };

        private static readonly TextReference[] SWSH =
        {
            new("iteminfo.dat", TextName.ItemFlavor),
            new("itemname.dat", TextName.ItemNames),
            new("monsname.dat", TextName.SpeciesNames),
            new("place_name_indirect.dat", TextName.metlist_00000),
            new("place_name_spe.dat", TextName.metlist_30000),
            new("place_name_out.dat", TextName.metlist_40000),
            new("place_name_per.dat", TextName.metlist_60000),
            new("seikaku.dat", TextName.Natures),
            new("tokusei.dat", TextName.AbilityNames),
            new("tokuseiinfo.dat", TextName.AbilityFlavor),
            new("trname.dat", TextName.TrainerNames),
            new("trtype.dat", TextName.TrainerClasses),
            new("trmsg.dat", TextName.TrainerText),
            new("typename.dat", TextName.Types),
            new("wazainfo.dat", TextName.MoveFlavor),
            new("wazaname.dat", TextName.MoveNames),
            new("zkn_form.dat", TextName.Forms),
            new("zkn_type.dat", TextName.SpeciesClassifications),
            new("zukan_comment_A.dat", TextName.PokedexEntry1),
            new("zukan_comment_B.dat", TextName.PokedexEntry2),
            new("ribbon.dat", TextName.RibbonMark),
            new("poke_memory_feeling.dat", TextName.MemoryFeelings),
        };


        private static readonly TextReference[] PLA =
        {
            new("iteminfo.dat", TextName.ItemFlavor),
            new("itemname.dat", TextName.ItemNames),
            new("monsname.dat", TextName.SpeciesNames),
            new("place_name_indirect.dat", TextName.metlist_00000),
            new("place_name_spe.dat", TextName.metlist_30000),
            new("place_name_out.dat", TextName.metlist_40000),
            new("place_name_per.dat", TextName.metlist_60000),
            new("seikaku.dat", TextName.Natures),
            new("tokusei.dat", TextName.AbilityNames),
            new("tokuseiinfo.dat", TextName.AbilityFlavor),
            new("trname.dat", TextName.TrainerNames),
            new("trtype.dat", TextName.TrainerClasses),
            new("trmsg.dat", TextName.TrainerText),
            new("typename.dat", TextName.Types),
            new("wazainfo.dat", TextName.MoveFlavor),
            new("wazaname.dat", TextName.MoveNames),
            new("zkn_form.dat", TextName.Forms),
            new("zkn_type.dat", TextName.SpeciesClassifications),
            new("zukan_comment_A.dat", TextName.PokedexEntry1),
            new("zukan_comment_B.dat", TextName.PokedexEntry2),
            new("ribbon.dat", TextName.RibbonMark),
            new("poke_memory_feeling.dat", TextName.MemoryFeelings),
        };
    }
}