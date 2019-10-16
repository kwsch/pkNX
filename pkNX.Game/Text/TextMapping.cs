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
                GameVersion.GG => GG,
                _ => null
            };
        }

        private static readonly TextReference[] XY =
        {
            new TextReference(005, TextName.Forms),
            new TextReference(013, TextName.MoveNames),
            new TextReference(015, TextName.MoveFlavor),
            new TextReference(017, TextName.Types),
            new TextReference(020, TextName.TrainerClasses),
            new TextReference(021, TextName.TrainerNames),
            new TextReference(022, TextName.TrainerText),
            new TextReference(034, TextName.AbilityNames),
            new TextReference(047, TextName.Natures),
            new TextReference(072, TextName.metlist_000000),
            new TextReference(080, TextName.SpeciesNames),
            new TextReference(096, TextName.ItemNames),
            new TextReference(099, TextName.ItemFlavor),
            new TextReference(130, TextName.MaisonTrainerNames),
            new TextReference(131, TextName.SuperTrainerNames),
            new TextReference(141, TextName.OPowerFlavor),
        };

        private static readonly TextReference[] AO =
        {
            new TextReference(005, TextName.Forms),
            new TextReference(014, TextName.MoveNames),
            new TextReference(016, TextName.MoveFlavor),
            new TextReference(018, TextName.Types),
            new TextReference(021, TextName.TrainerClasses),
            new TextReference(022, TextName.TrainerNames),
            new TextReference(023, TextName.TrainerText),
            new TextReference(037, TextName.AbilityNames),
            new TextReference(051, TextName.Natures),
            new TextReference(090, TextName.metlist_000000),
            new TextReference(098, TextName.SpeciesNames),
            new TextReference(114, TextName.ItemNames),
            new TextReference(117, TextName.ItemFlavor),
            new TextReference(153, TextName.MaisonTrainerNames),
            new TextReference(154, TextName.SuperTrainerNames),
            new TextReference(165, TextName.OPowerFlavor),
        };

        private static readonly TextReference[] SMDEMO =
        {
            new TextReference(020, TextName.ItemFlavor),
            new TextReference(021, TextName.ItemNames),
            new TextReference(026, TextName.SpeciesNames),
            new TextReference(030, TextName.metlist_000000),
            new TextReference(044, TextName.Forms),
            new TextReference(044, TextName.Natures),
            new TextReference(046, TextName.AbilityNames),
            new TextReference(049, TextName.TrainerText),
            new TextReference(050, TextName.TrainerNames),
            new TextReference(051, TextName.TrainerClasses),
            new TextReference(052, TextName.Types),
            new TextReference(054, TextName.MoveFlavor),
            new TextReference(055, TextName.MoveNames),
        };

        private static readonly TextReference[] SM =
        {
            new TextReference(035, TextName.ItemFlavor),
            new TextReference(036, TextName.ItemNames),
            new TextReference(055, TextName.SpeciesNames),
            new TextReference(067, TextName.metlist_000000),
            new TextReference(086, TextName.BattleRoyalNames),
            new TextReference(087, TextName.Natures),
            new TextReference(096, TextName.AbilityNames),
            new TextReference(099, TextName.BattleTreeNames),
            new TextReference(104, TextName.TrainerText),
            new TextReference(105, TextName.TrainerNames),
            new TextReference(106, TextName.TrainerClasses),
            new TextReference(107, TextName.Types),
            new TextReference(112, TextName.MoveFlavor),
            new TextReference(113, TextName.MoveNames),
            new TextReference(114, TextName.Forms),
            new TextReference(116, TextName.SpeciesClassifications),
            new TextReference(119, TextName.PokedexEntry1),
            new TextReference(120, TextName.PokedexEntry2)
        };

        private static readonly TextReference[] USUM =
        {
            new TextReference(039, TextName.ItemFlavor),
            new TextReference(040, TextName.ItemNames),
            new TextReference(060, TextName.SpeciesNames),
            new TextReference(072, TextName.metlist_000000),
            new TextReference(091, TextName.BattleRoyalNames),
            new TextReference(092, TextName.Natures),
            new TextReference(101, TextName.AbilityNames),
            new TextReference(104, TextName.BattleTreeNames),
            new TextReference(109, TextName.TrainerText),
            new TextReference(110, TextName.TrainerNames),
            new TextReference(111, TextName.TrainerClasses),
            new TextReference(112, TextName.Types),
            new TextReference(117, TextName.MoveFlavor),
            new TextReference(118, TextName.MoveNames),
            new TextReference(119, TextName.Forms),
            new TextReference(121, TextName.SpeciesClassifications),
            new TextReference(124, TextName.PokedexEntry1),
            new TextReference(125, TextName.PokedexEntry2)
        };

        private static readonly TextReference[] GG =
        {
            new TextReference("iteminfo.dat", TextName.ItemFlavor),
            new TextReference("itemname.dat", TextName.ItemNames),
            new TextReference("monsname.dat", TextName.SpeciesNames),
            new TextReference("place_name.dat", TextName.metlist_000000),
            new TextReference("seikaku.dat", TextName.Natures),
            new TextReference("tokusei.dat", TextName.AbilityNames),
            new TextReference("trname.dat", TextName.TrainerNames),
            new TextReference("trtype.dat", TextName.TrainerClasses),
            new TextReference("trmsg.dat", TextName.TrainerText),
            new TextReference("typename.dat", TextName.Types),
            new TextReference("wazainfo.dat", TextName.MoveFlavor),
            new TextReference("wazaname.dat", TextName.MoveNames),
            new TextReference("zkn_form.dat", TextName.Forms),
            new TextReference("zkn_type.dat", TextName.SpeciesClassifications),
            new TextReference("zukan_comment_A.dat", TextName.PokedexEntry1),
        };
    }
}