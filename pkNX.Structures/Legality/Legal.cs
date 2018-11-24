using System.Collections.Generic;
using System.Linq;

namespace pkNX.Structures
{
    public static partial class Legal
    {
        public static int GetRandomForme(int species, bool mega, bool alola, PersonalInfo[] stats = null)
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
    }
}
