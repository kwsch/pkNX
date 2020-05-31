using pkNX.Structures;
using System.Linq;
using static pkNX.Structures.Species;

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

            switch ((Species)species)
            {
                case Unown:
                case Sawsbuck:
                    return 31; // Random
                case Greninja when !AllowMega:
                    return 0;
                case Scatterbug:
                case Spewpa:
                case Vivillon:
                    return 30; // save file specific
                case Minior:
                    return Util.Random.Next(7);
                case Eternatus:
                    return 0;

                // Galarian Forms
                case Meowth when AllowGalarianForm: // Galarian Meowth is altform 2
                    return Util.Random.Next(3);
                case Darmanitan when AllowGalarianForm: // Galarian Darmanitan is altform 2, Galarian Zen is altform 3
                    return Util.Random.Next(4);
            }

            if (AllowAlolanForm && Legal.EvolveToAlolanForms.Contains(species))
                return Util.Random.Next(2);
            if (AllowGalarianForm && Legal.EvolveToGalarForms.Contains(species))
                return species == (int)Slowpoke ? 1 : Util.Random.Next(2);
            if (!Legal.BattleExclusiveForms.Contains(species) || AllowMega)
                return Util.Random.Next(stats[species].FormeCount); // Slot-Random
            return 0;
        }
    }
}
