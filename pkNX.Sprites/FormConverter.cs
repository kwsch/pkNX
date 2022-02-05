using System.Collections.Generic;
using pkNX.Structures;
using static pkNX.Structures.Species;

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
            if (species == (int)Mimikyu)
                return form is 2 or 3;
            if (Legal.Totem_Alolan.Contains(species))
                return form == 2;
            return form == 1;
        }

        public static int GetTotemBaseForm(int species, int form)
        {
            if (species == (int)Mimikyu)
                return form - 2;
            return form - 1;
        }

        public static bool IsValidOutOfBoundsForme(int species, int form, int generation)
        {
            return (Species) species switch
            {
                Unown => form < (generation == 2 ? 26 : 28), // A-Z : A-Z?!
                Mothim => form < 3, // Wormadam base form is kept
                Scatterbug or Spewpa => form < 18,
                _ => false
            };
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

        private static readonly HashSet<int> HasFormeValuesNotIndicatedByPersonal = new()
        {
            (int)Unown,
            (int)Mothim, // Burmy forme carried over, not cleared
            (int)Scatterbug,
            (int)Spewpa, // Vivillon pre-evos
        };
    }
}
