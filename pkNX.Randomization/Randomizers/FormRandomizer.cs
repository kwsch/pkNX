using pkNX.Structures;
using System;
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

        public int GetRandomForme(int species, bool mega, bool fused, bool alola, bool galar, PersonalInfo[] stats = null)
        {
            if (stats == null)
                stats = Personal.Table;
            if (stats[species].FormeCount <= 1)
                return 0;
            bool IsGen6 = Personal.MaxSpeciesID == 721;

            switch ((Species)species)
            {
                case Unown:
                case Deerling:
                case Sawsbuck:
                    return 31; // Random
                case Greninja when !mega:
                    return 0;
                case Scatterbug:
                case Spewpa:
                case Vivillon:
                    return 30; // save file specific
                case Zygarde when !IsGen6:
                    return Util.Random.Next(4); // Complete Forme is battle only
                case Minior:
                    return Util.Random.Next(7); // keep the core color a surprise

                case Meowth when galar:
                    return Util.Random.Next(3); // Kanto, Alola, Galar

                // some species have 1 invalid form among several other valid forms, handle them here
                case Pikachu when Personal.TableLength == 1181:
                case Slowbro when galar:
                case Darmanitan:
                {
                    int form = Util.Random.Next(stats[species].FormeCount - 1);
                    int banned = GetInvalidForm(species, galar, Personal);
                    if (form == banned)
                        form++;
                    return form;
                }
            }

            if (Personal.TableLength == 980 && (species == (int)Pikachu || species == (int)Eevee)) // gg tableB -- no starters, they crash trainer battles.
                return 0;
            if (alola && Legal.EvolveToAlolanForms.Contains(species))
                return Util.Random.Next(2);
            if (galar && Legal.EvolveToGalarForms.Contains(species))
                return Util.Random.Next(2);
            if (!Legal.BattleExclusiveForms.Contains(species) || mega || fused && Legal.BattleFusions.Contains(species))
                return Util.Random.Next(stats[species].FormeCount); // Slot-Random
            return 0;
        }

        public int GetInvalidForm(int species, bool galar, PersonalTable stats)
        {
            return species switch
            {
                (int)Pikachu when stats.TableLength == 1181 => 8, // LGPE Partner Pikachu
                (int)Slowbro when galar => 1, // Mega Slowbro
                (int)Darmanitan => 1, // Zen Mode (battle only)
                _ => throw new ArgumentOutOfRangeException(nameof(species))
            };
        }
    }
}
