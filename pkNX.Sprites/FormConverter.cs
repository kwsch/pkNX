using System.Collections.Generic;
using pkNX.Structures;

namespace pkNX.Sprites
{
    public static class FormConverter
    {
        public static bool IsTotemForm(int species, int form, int generation = 7)
        {
            if (generation != 7)
                return false;
            if (form == 0)
                return false;
            if (!Legal.Totem_USUM.Contains(species))
                return false;
            if (species == 778) // Mimikyu
                return form == 2 || form == 3;
            if (Legal.Totem_Alolan.Contains(species))
                return form == 2;
            return form == 1;
        }

        public static int GetTotemBaseForm(int species, int form)
        {
            if (species == 778) // Mimikyu
                return form - 2;
            return form - 1;
        }

        public static bool IsValidOutOfBoundsForme(int species, int form, int generation)
        {
            switch (species)
            {
                case 201: // Unown
                    return form < (generation == 2 ? 26 : 28); // A-Z : A-Z?!
                case 414: // Wormadam base form is kept
                    return form < 3;
                case 664:
                case 665: // Vivillon Pre-evolutions
                    return form < 18;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Checks if the species should have a drop-down selection visible for the form value.
        /// </summary>
        /// <param name="pi">Game specific personal info</param>
        /// <param name="species"> ID</param>
        public static bool HasFormSelection(PersonalInfo pi, int species)
        {
            if (HasFormeValuesNotIndicatedByPersonal.Contains(species))
                return true;

            int count = pi.FormeCount;
            return count > 1;
        }

        private static readonly HashSet<int> HasFormeValuesNotIndicatedByPersonal = new HashSet<int>
        {
            201, // Unown
            414, // Mothim (Burmy forme carried over, not cleared)
            664, 665, // Vivillon pre-evos
        };
    }
}
