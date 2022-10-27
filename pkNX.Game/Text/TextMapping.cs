using System.Collections.Generic;
using pkNX.Structures;
using static pkNX.Game.TextName;

namespace pkNX.Game;

public static class TextMapping
{
    public static IReadOnlyCollection<TextReference> GetMapping(GameVersion game) => game switch
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
        _ => throw new System.ArgumentOutOfRangeException($"No text mapping for {game}"),
    };

    private static readonly TextReference[] XY =
    {
        new(005, Forms),
        new(013, MoveNames),
        new(015, MoveFlavor),
        new(017, TypeNames),
        new(020, TrainerClasses),
        new(021, TrainerNames),
        new(022, TrainerText),
        new(034, AbilityNames),
        new(047, Natures),
        new(072, metlist_00000),
        new(080, SpeciesNames),
        new(096, ItemNames),
        new(099, ItemFlavor),
        new(130, MaisonTrainerNames),
        new(131, SuperTrainerNames),
        new(141, OPowerFlavor),
    };

    private static readonly TextReference[] AO =
    {
        new(005, Forms),
        new(014, MoveNames),
        new(016, MoveFlavor),
        new(018, TypeNames),
        new(021, TrainerClasses),
        new(022, TrainerNames),
        new(023, TrainerText),
        new(037, AbilityNames),
        new(051, Natures),
        new(090, metlist_00000),
        new(098, SpeciesNames),
        new(114, ItemNames),
        new(117, ItemFlavor),
        new(153, MaisonTrainerNames),
        new(154, SuperTrainerNames),
        new(165, OPowerFlavor),
    };

    private static readonly TextReference[] SMDEMO =
    {
        new(020, ItemFlavor),
        new(021, ItemNames),
        new(026, SpeciesNames),
        new(030, metlist_00000),
        new(044, Forms),
        new(044, Natures),
        new(046, AbilityNames),
        new(049, TrainerText),
        new(050, TrainerNames),
        new(051, TrainerClasses),
        new(052, TypeNames),
        new(054, MoveFlavor),
        new(055, MoveNames),
    };

    private static readonly TextReference[] SM =
    {
        new(035, ItemFlavor),
        new(036, ItemNames),
        new(055, SpeciesNames),
        new(067, metlist_00000),
        new(086, BattleRoyalNames),
        new(087, Natures),
        new(096, AbilityNames),
        new(099, BattleTreeNames),
        new(104, TrainerText),
        new(105, TrainerNames),
        new(106, TrainerClasses),
        new(107, TypeNames),
        new(112, MoveFlavor),
        new(113, MoveNames),
        new(114, Forms),
        new(116, SpeciesClassifications),
        new(119, PokedexEntry1),
        new(120, PokedexEntry2),
    };

    private static readonly TextReference[] USUM =
    {
        new(039, ItemFlavor),
        new(040, ItemNames),
        new(060, SpeciesNames),
        new(072, metlist_00000),
        new(091, BattleRoyalNames),
        new(092, Natures),
        new(101, AbilityNames),
        new(104, BattleTreeNames),
        new(109, TrainerText),
        new(110, TrainerNames),
        new(111, TrainerClasses),
        new(112, TypeNames),
        new(117, MoveFlavor),
        new(118, MoveNames),
        new(119, Forms),
        new(121, SpeciesClassifications),
        new(124, PokedexEntry1),
        new(125, PokedexEntry2),
    };

    private static readonly TextReference[] GG =
    {
        new("iteminfo.dat", ItemFlavor),
        new("itemname.dat", ItemNames),
        new("monsname.dat", SpeciesNames),
        new("place_name.dat", metlist_00000),
        new("seikaku.dat", Natures),
        new("tokusei.dat", AbilityNames),
        new("tokuseiinfo.dat", AbilityFlavor),
        new("trname.dat", TrainerNames),
        new("trtype.dat", TrainerClasses),
        new("trmsg.dat", TrainerText),
        new("typename.dat", TypeNames),
        new("wazainfo.dat", MoveFlavor),
        new("wazaname.dat", MoveNames),
        new("zkn_form.dat", Forms),
        new("zkn_type.dat", SpeciesClassifications),
        new("zukan_comment_A.dat", PokedexEntry1),
    };

    private static readonly TextReference[] SWSH =
    {
        new("iteminfo.dat", ItemFlavor),
        new("itemname.dat", ItemNames),
        new("monsname.dat", SpeciesNames),
        new("place_name_indirect.dat", metlist_00000),
        new("place_name_spe.dat", metlist_30000),
        new("place_name_out.dat", metlist_40000),
        new("place_name_per.dat", metlist_60000),
        new("seikaku.dat", Natures),
        new("tokusei.dat", AbilityNames),
        new("tokuseiinfo.dat", AbilityFlavor),
        new("trname.dat", TrainerNames),
        new("trtype.dat", TrainerClasses),
        new("trmsg.dat", TrainerText),
        new("typename.dat", TypeNames),
        new("wazainfo.dat", MoveFlavor),
        new("wazaname.dat", MoveNames),
        new("zkn_form.dat", Forms),
        new("zkn_type.dat", SpeciesClassifications),
        new("zukan_comment_A.dat", PokedexEntry1),
        new("zukan_comment_B.dat", PokedexEntry2),
        new("ribbon.dat", RibbonMark),
        new("poke_memory_feeling.dat", MemoryFeelings),
    };

    private static readonly TextReference[] PLA =
    {
        new("iteminfo.dat", ItemFlavor),
        new("itemname.dat", ItemNames),
        new("monsname.dat", SpeciesNames),
        new("place_name_indirect.dat", metlist_00000),
        new("place_name_spe.dat", metlist_30000),
        new("place_name_out.dat", metlist_40000),
        new("place_name_per.dat", metlist_60000),
        new("seikaku.dat", Natures),
        new("tokusei.dat", AbilityNames),
        new("tokuseiinfo.dat", AbilityFlavor),
        new("trname.dat", TrainerNames),
        new("trtype.dat", TrainerClasses),
        new("trmsg.dat", TrainerText),
        new("typename.dat", TypeNames),
        new("wazainfo.dat", MoveFlavor),
        new("wazaname.dat", MoveNames),
        new("zkn_form.dat", Forms),
        new("zkn_type.dat", SpeciesClassifications),
        new("zukan_comment_A.dat", PokedexEntry1),
        new("zukan_comment_B.dat", PokedexEntry2),
        new("ribbon.dat", RibbonMark),
        new("poke_memory_feeling.dat", MemoryFeelings),
    };
}
