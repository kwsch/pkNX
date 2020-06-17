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
                case Deerling:
                case Sawsbuck:
                    return 31; // Random
                case Greninja when !AllowMega:
                case Eternatus:
                    return 0;
                case Scatterbug:
                case Spewpa:
                case Vivillon:
                    return 30; // save file specific
                case Minior:
                    return Util.Random.Next(7); // keep the core color a surprise

                // Galarian Forms
                case Meowth when AllowGalarianForm: // Kanto, Alola, Galar
                    return Util.Random.Next(3);
                case Darmanitan when AllowGalarianForm: // Standard, Zen, Galar Standard, Galar Zen
                    return Util.Random.Next(4);
                case Slowbro when AllowGalarianForm: // Slowbro-1 is Mega and Slowbro-2 is Galar, so only return 0 or 2
                    {
                        int form = Util.Random.Next(2);
                        if (form == 1)
                            form++;
                        return form;
                    }
            }

            if (AllowAlolanForm && Legal.EvolveToAlolanForms.Contains(species))
                return Util.Random.Next(2);
            if (AllowGalarianForm && Legal.EvolveToGalarForms.Contains(species))
                return Util.Random.Next(2);
            if (!Legal.BattleExclusiveForms.Contains(species) || AllowMega)
                return Util.Random.Next(stats[species].FormeCount); // Slot-Random
            return 0;
        }
    }
}
