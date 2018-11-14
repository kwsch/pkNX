using System.Linq;

namespace pkNX.Structures
{
    public static partial class Legal
    {
        internal static int GetRandomForme(int species, bool mega, bool alola, PersonalInfo[] stats = null)
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
        internal static int GetModifiedLevel(int level, decimal factor)
        {
            int newlvl = (int)(level * factor);
            if (newlvl < 1)
                return 1;
            if (newlvl > 100)
                return 100;
            return newlvl;
        }

        internal static int[] GetRandomItemList(GameVersion game)
        {
            if (GameVersion.ORAS.Contains(game) || game == GameVersion.ORASDEMO)
                return Items_HeldAO.Concat(Items_Ball).Where(i => i != 0).ToArray();

            if (GameVersion.XY.Contains(game))
                return Items_HeldXY.Concat(Items_Ball).Where(i => i != 0).ToArray();

            if (GameVersion.SM.Contains(game) || GameVersion.USUM.Contains(game))
                return HeldItemsBuy_SM.Select(i => (int)i).Concat(Items_Ball).Where(i => i != 0).ToArray();

            return new int[1];
        }
    }
}
