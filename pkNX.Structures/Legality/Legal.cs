using System;
using System.Collections.Generic;
using System.Linq;

namespace pkNX.Structures
{
    public static partial class Legal
    {
        public static int GetRandomForme(int species, bool mega, bool alola, PersonalTable stats)
        {
            if (stats == null)
                return 0;
            if (stats[species].FormeCount <= 1)
                return 0;

            if (species == 658 && !mega) // Greninja
                return 0;
            if (species == 664 || species == 665 || species == 666) // vivillon
                return 30; // save file specific
            if (species == 774) // minior
                return Util.Rand.Next(7);

            if (alola && EvolveToAlolanForms.Contains(species))
                return Util.Rand.Next(2);
            if (stats.TableLength == 980 && (species == 25 || species == 133)) // gg tableB -- no starters, they crash trainer battles.
                return 0; // Pikachu
            if (!BattleExclusiveForms.Contains(species) || mega)
                return Util.Rand.Next(stats[species].FormeCount); // Slot-Random
            return 0;
        }

        /// <summary>
        /// Multiplies the current level with a scaling factor, returning a modified level.
        /// </summary>
        /// <param name="level">Current Level.</param>
        /// <param name="factor">Modification factor.</param>
        /// <returns>Boosted (or reduced) level.</returns>
        public static int GetModifiedLevel(int level, decimal factor)
        {
            int newlvl = (int)(level * factor);
            if (newlvl < 1)
                return 1;
            if (newlvl > 100)
                return 100;
            return newlvl;
        }

        public static int[] GetRandomItemList(GameVersion game)
        {
            if (GameVersion.ORAS.Contains(game) || game == GameVersion.ORASDEMO)
                return Items_HeldAO.Concat(Items_Ball).Where(i => i != 0).ToArray();

            if (GameVersion.XY.Contains(game))
                return Items_HeldXY.Concat(Items_Ball).Where(i => i != 0).ToArray();

            if (GameVersion.SM.Contains(game) || GameVersion.USUM.Contains(game))
                return HeldItemsBuy_SM.Select(i => (int)i).Concat(Items_Ball).Where(i => i != 0).ToArray();

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

        private static readonly Dictionary<int, int[]> MegaDictionaryXY = new Dictionary<int, int[]>
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

        private static readonly Dictionary<int, int[]> MegaDictionaryAO = new Dictionary<int, int[]>
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
            {428, new[] {768}}, // Lopunny @ Lopunnite
            {475, new[] {756}}, // Gallade @ Galladite
            {531, new[] {757}}, // Audino @ Audinite
            {719, new[] {764}}, // Diancie @ Diancite
        };

        private static readonly Dictionary<int, int[]> MegaDictionaryGG = new Dictionary<int, int[]>
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

        public static int[] GetBannedMoves(GameVersion infoGame, IEnumerable<Move> moves)
        {
            if (!GameVersion.GG.Contains(infoGame))
                return Array.Empty<int>();

            return Enumerable.Range(0, moves.Count()).Except(AllowedMovesGG).ToArray();
        }

        private static readonly int[] AllowedMovesGG =
        {
            000, 001, 002, 003, 004, 005, 006, 007, 008, 009,
            010, 011, 012, 013, 014, 015, 016, 017, 018, 019,
            020, 021, 022, 023, 024, 025, 026, 027, 028, 029,
            030, 031, 032, 033, 034, 035, 036, 037, 038, 039,
            040, 041, 042, 043, 044, 045, 046, 047, 048, 049,
            050, 051, 052, 053, 054, 055, 056, 057, 058, 059,
            060, 061, 062, 063, 064, 065, 066, 067, 068, 069,
            070, 071, 072, 073, 074, 075, 076, 077, 078, 079,
            080, 081, 082, 083, 084, 085, 086, 087, 088, 089,
            090, 091, 092, 093, 094, 095, 096, 097, 098, 099,
            100, 101, 102, 103, 104, 105, 106, 107, 108, 109,
            110, 111, 112, 113, 114, 115, 116, 117, 118, 119,
            120, 121, 122, 123, 124, 125, 126, 127, 128, 129,
            130, 131, 132, 133, 134, 135, 136, 137, 138, 139,
            140, 141, 142, 143, 144, 145, 146, 147, 148, 149,
            150, 151, 152, 153, 154, 155, 156, 157, 158, 159,
            160, 161, 162, 163, 164,

            182, 188, 200, 224, 227, 231, 242, 243, 247, 252,
            257, 261, 263, 269, 270, 276, 280, 281, 339, 347,
            355, 364, 369, 389, 394, 398, 399, 403, 404, 405,
            406, 417, 420, 430, 438, 446, 453, 483, 492, 499,
            503, 504, 525, 529, 583, 585, 605, 742
        };
    }
}
