using System;
using System.Collections.Generic;
using System.Linq;

namespace pkNX.Structures;

public static partial class Legal
{
    /// <summary>
    /// Multiplies the current level with a scaling factor, returning a modified level.
    /// </summary>
    /// <param name="level">Current Level.</param>
    /// <param name="factor">Modification factor.</param>
    /// <returns>Boosted (or reduced) level.</returns>
    public static int GetModifiedLevel(int level, double factor)
    {
        int newlvl = (int)(level * factor);
        return Math.Max(1, Math.Min(newlvl, 100));
    }

    public static int[] GetRandomItemList(GameVersion game)
    {
        if (GameVersion.XY.Contains(game))
            return Items_HeldXY.Concat(Items_Ball).Where(i => i != 0).ToArray();

        if (GameVersion.ORAS.Contains(game) || game == GameVersion.ORASDEMO)
            return Items_HeldAO.Concat(Items_Ball).Where(i => i != 0).ToArray();

        if (GameVersion.SM.Contains(game) || GameVersion.USUM.Contains(game))
            return HeldItemsBuy_SM.Select(i => (int)i).Concat(Items_Ball).Where(i => i != 0).ToArray();

        if (GameVersion.GG.Contains(game))
            return HeldItems_GG.Select(i => (int)i).Where(i => i != 0).ToArray();

        if (GameVersion.SWSH.Contains(game))
            return HeldItems_SWSH.Select(i => (int)i).Where(i => i != 0 && i != 1279 /*Dummy Items*/).ToArray();

        return new int[1];
    }

    public static Dictionary<int, int[]> GetMegaDictionary(GameVersion game)
    {
        if (GameVersion.XY.Contains(game))
            return MegaDictionaryXY;
        if (GameVersion.GG.Contains(game))
            return MegaDictionaryGG;
        return MegaDictionaryAO;
    }

    private static readonly Dictionary<int, int[]> MegaDictionaryXY = new()
    {
        {003, new[] {659}}, // Venusaur @ Venusaurite
        {006, new[] {660, 678}}, // Charizard @ Charizardite X/Y
        {009, new[] {661}}, // Blastoise @ Blastoisinite
        {065, new[] {679}}, // Alakazam @ Alakazite
        {094, new[] {656}}, // Gengar @ Gengarite
        {115, new[] {675}}, // Kangaskhan @ Kangaskhanite
        {127, new[] {671}}, // Pinsir @ Pinsirite
        {130, new[] {676}}, // Gyarados @ Gyaradosite
        {142, new[] {672}}, // Aerodactyl @ Aerodactylite
        {150, new[] {662, 663}}, // Mewtwo @ Mewtwonite X/Y
        {181, new[] {658}}, // Ampharos @ Ampharosite
        {212, new[] {670}}, // Scizor @ Scizorite
        {214, new[] {680}}, // Heracross @ Heracronite
        {229, new[] {666}}, // Houndoom @ Houndoominite
        {248, new[] {669}}, // Tyranitar @ Tyranitarite
        {257, new[] {664}}, // Blaziken @ Blazikenite
        {282, new[] {657}}, // Gardevoir @ Gardevoirite
        {303, new[] {681}}, // Mawile @ Mawilite
        {306, new[] {667}}, // Aggron @ Aggronite
        {308, new[] {665}}, // Medicham @ Medichamite
        {310, new[] {682}}, // Manectric @ Manectite
        {354, new[] {668}}, // Banette @ Banettite
        {359, new[] {677}}, // Absol @ Absolite
        {380, new[] {684}}, // Latias @ Latiasite
        {381, new[] {685}}, // Latios @ Latiosite
        {445, new[] {683}}, // Garchomp @ Garchompite
        {448, new[] {673}}, // Lucario @ Lucarionite
        {460, new[] {674}}, // Abomasnow @ Abomasite
    };

    private static readonly Dictionary<int, int[]> MegaDictionaryAO = new()
    {
        {003, new[] {659}}, // Venusaur @ Venusaurite
        {006, new[] {660, 678}}, // Charizard @ Charizardite X/Y
        {009, new[] {661}}, // Blastoise @ Blastoisinite
        {065, new[] {679}}, // Alakazam @ Alakazite
        {094, new[] {656}}, // Gengar @ Gengarite
        {115, new[] {675}}, // Kangaskhan @ Kangaskhanite
        {127, new[] {671}}, // Pinsir @ Pinsirite
        {130, new[] {676}}, // Gyarados @ Gyaradosite
        {142, new[] {672}}, // Aerodactyl @ Aerodactylite
        {150, new[] {662, 663}}, // Mewtwo @ Mewtwonite X/Y
        {181, new[] {658}}, // Ampharos @ Ampharosite
        {212, new[] {670}}, // Scizor @ Scizorite
        {214, new[] {680}}, // Heracross @ Heracronite
        {229, new[] {666}}, // Houndoom @ Houndoominite
        {248, new[] {669}}, // Tyranitar @ Tyranitarite
        {257, new[] {664}}, // Blaziken @ Blazikenite
        {282, new[] {657}}, // Gardevoir @ Gardevoirite
        {303, new[] {681}}, // Mawile @ Mawilite
        {306, new[] {667}}, // Aggron @ Aggronite
        {308, new[] {665}}, // Medicham @ Medichamite
        {310, new[] {682}}, // Manectric @ Manectite
        {354, new[] {668}}, // Banette @ Banettite
        {359, new[] {677}}, // Absol @ Absolite
        {380, new[] {684}}, // Latias @ Latiasite
        {381, new[] {685}}, // Latios @ Latiosite
        {445, new[] {683}}, // Garchomp @ Garchompite
        {448, new[] {673}}, // Lucario @ Lucarionite
        {460, new[] {674}}, // Abomasnow @ Abomasite

        {015, new[] {770}}, // Beedrill @ Beedrillite
        {018, new[] {762}}, // Pidgeot @ Pidgeotite
        {080, new[] {760}}, // Slowbro @ Slowbronite
        {208, new[] {761}}, // Steelix @ Steelixite
        {254, new[] {753}}, // Sceptile @ Sceptilite
        {260, new[] {752}}, // Swampert @ Swampertite
        {302, new[] {754}}, // Sableye @ Sablenite
        {319, new[] {759}}, // Sharpedo @ Sharpedonite
        {323, new[] {767}}, // Camerupt @ Cameruptite
        {334, new[] {755}}, // Altaria @ Altarianite
        {362, new[] {763}}, // Glalie @ Glalitite
        {373, new[] {769}}, // Salamence @ Salamencite
        {376, new[] {758}}, // Metagross @ Metagrossite
        // Rayquaza requires Dragon Ascent, no Held Item
        {428, new[] {768}}, // Lopunny @ Lopunnite
        {475, new[] {756}}, // Gallade @ Galladite
        {531, new[] {757}}, // Audino @ Audinite
        {719, new[] {764}}, // Diancie @ Diancite
    };

    private static readonly Dictionary<int, int[]> MegaDictionaryGG = new()
    {
        {003, new[] {659}}, // Venusaur @ Venusaurite
        {006, new[] {660, 678}}, // Charizard @ Charizardite X/Y
        {009, new[] {661}}, // Blastoise @ Blastoisinite
        {065, new[] {679}}, // Alakazam @ Alakazite
        {094, new[] {656}}, // Gengar @ Gengarite
        {115, new[] {675}}, // Kangaskhan @ Kangaskhanite
        {127, new[] {671}}, // Pinsir @ Pinsirite
        {130, new[] {676}}, // Gyarados @ Gyaradosite
        {142, new[] {672}}, // Aerodactyl @ Aerodactylite
        {150, new[] {662, 663}}, // Mewtwo @ Mewtwonite X/Y

        {015, new[] {770}}, // Beedrill @ Beedrillite
        {018, new[] {762}}, // Pidgeot @ Pidgeotite
        {080, new[] {760}}, // Slowbro @ Slowbronite
    };

    public static int[] GetBannedMoves(GameVersion infoGame, int moveCount)
    {
        if (!GameVersion.GG.Contains(infoGame))
            return Array.Empty<int>();

        return Enumerable.Range(0, moveCount).Except(AllowedMovesGG).ToArray();
    }

    public static int[] GetAllowedMoves(GameVersion infoGame, int moveCount)
    {
        if (GameVersion.GG.Contains(infoGame))
            return AllowedMovesGG;

        return Enumerable.Range(0, moveCount).ToArray();
    }

    public static readonly HashSet<int> BattleForms = new()
    {
        (int)Species.Castform,
        (int)Species.Cherrim,
        (int)Species.Darmanitan,
        (int)Species.Meloetta,
        (int)Species.Aegislash,
        (int)Species.Xerneas,
        (int)Species.Wishiwashi,
        (int)Species.Mimikyu,
        (int)Species.Cramorant,
        (int)Species.Eiscue,
        (int)Species.Morpeko,
        (int)Species.Zacian,
        (int)Species.Zamazenta,
        (int)Species.Eternatus,
    };

    public static readonly HashSet<int> BattleMegas = new()
    {
        // XY
        (int)Species.Venusaur, (int)Species.Charizard, (int)Species.Blastoise, (int)Species.Alakazam, (int)Species.Gengar,
        (int)Species.Kangaskhan, (int)Species.Pinsir, (int)Species.Gyarados, (int)Species.Aerodactyl, (int)Species.Mewtwo,
        (int)Species.Ampharos, (int)Species.Scizor, (int)Species.Heracross, (int)Species.Houndoom, (int)Species.Tyranitar,
        (int)Species.Blaziken, (int)Species.Gardevoir, (int)Species.Mawile, (int)Species.Aggron, (int)Species.Medicham,
        (int)Species.Manectric, (int)Species.Banette, (int)Species.Absol, (int)Species.Latias, (int)Species.Latios,
        (int)Species.Garchomp, (int)Species.Lucario, (int)Species.Abomasnow,

        // AO
        (int)Species.Beedrill, (int)Species.Pidgeot, (int)Species.Slowbro, (int)Species.Steelix,
        (int)Species.Sceptile, (int)Species.Swampert, (int)Species.Sableye, (int)Species.Sharpedo, (int)Species.Camerupt,
        (int)Species.Altaria, (int)Species.Glalie, (int)Species.Salamence, (int)Species.Metagross, (int)Species.Rayquaza,
        (int)Species.Lopunny, (int)Species.Gallade,
        (int)Species.Audino, (int)Species.Diancie,
    };

    public static readonly HashSet<int> BattlePrimals = new() { 382, 383 }; // Kyogre and Groudon
    public static readonly HashSet<int> BattleFusions = new() { 646, 800, 898 }; // Kyurem, Necrozma, Calyrex
    public static readonly HashSet<int> BattleExclusiveForms = new(BattleForms.Concat(BattleMegas.Concat(BattlePrimals).Concat(BattleFusions)));
}
