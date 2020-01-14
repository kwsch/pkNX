using pkNX.Structures;
using System.Linq;

namespace pkNX.Randomization
{
    public class FormRandomizer
    {
        private readonly PersonalTable Personal;

        public FormRandomizer(PersonalTable t)
        {
            Personal = t;
        }

        public bool AllowMega { get; set; } = false;
        public bool AllowAlolanForm { get; set; } = true;
        public bool AllowGalarianForm { get; set; } = true;

        public int GetRandomForme(int species, PersonalInfo[] stats = null)
        {
            if (stats == null)
                stats = Personal.Table;
            if (stats[species].FormeCount <= 1)
                return 0;

            switch (species)
            {
                case 201: // Unown
                case 586: // Sawsbuck
                    return 31; // Random
                case 658 when !AllowMega:
                    return 0;
                case 664:
                case 665:
                case 666: // Vivillon evo chain
                    return 30; // save file specific
                case 774: // Minior
                    return Util.Random.Next(7);
                case 890: // Eternatus
                    return 0;

                // Galarian Forms
                case 052 when AllowGalarianForm: // Galarian Meowth is altform 2
                    return Util.Random.Next(3);
                case 555 when AllowGalarianForm: // Galarian Darmanitan is altform 2, Galarian Zen is altform 3
                    return Util.Random.Next(4);
            }

            if (AllowAlolanForm && Legal.EvolveToAlolanForms.Contains(species))
                return Util.Random.Next(2);
            if (AllowGalarianForm && Legal.EvolveToGalarForms.Contains(species))
                return species == 079 ? 1 : Util.Random.Next(2);
            if (!Legal.BattleExclusiveForms.Contains(species) || AllowMega)
                return Util.Random.Next(stats[species].FormeCount); // Slot-Random
            return 0;
        }
    }
}
