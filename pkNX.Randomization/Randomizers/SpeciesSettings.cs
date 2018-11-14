using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using pkNX.Structures;

namespace pkNX.Randomization
{
    /// <summary>
    /// Settings for what Species are permitted during randomization.
    /// </summary>
    public class SpeciesSettings : RandSettings
    {
        private const string General = nameof(General);
        private const string Misc = nameof(Misc);

        /// <summary>Allows Generation 1 species when randomizing.</summary>
        [Category(General), Description("Allows Generation 1 species when randomizing.")]
        public bool Gen1 { get; set; } = true;

        /// <summary>Allows Generation 2 species when randomizing.</summary>
        [Category(General), Description("Allows Generation 2 species when randomizing.")]
        public bool Gen2 { get; set; } = true;

        /// <summary>Allows Generation 3 species when randomizing.</summary>
        [Category(General), Description("Allows Generation 3 species when randomizing.")]
        public bool Gen3 { get; set; } = true;

        /// <summary>Allows Generation 4 species when randomizing.</summary>
        [Category(General), Description("Allows Generation 4 species when randomizing.")]
        public bool Gen4 { get; set; } = true;

        /// <summary>Allows Generation 5 species when randomizing.</summary>
        [Category(General), Description("Allows Generation 5 species when randomizing.")]
        public bool Gen5 { get; set; } = true;

        /// <summary>Allows Generation 6 species when randomizing.</summary>
        [Category(General), Description("Allows Generation 6 species when randomizing.")]
        public bool Gen6 { get; set; } = true;

        /// <summary>Allows Generation 7 species when randomizing.</summary>
        [Category(General), Description("Allows Generation 7 species when randomizing.")]
        public bool Gen7 { get; set; } = true;

        /// <summary>Allows Legendary species when randomizing.</summary>
        [Category(Misc), Description("Allows Legendary species when randomizing.")]
        public bool Legends { get; set; } = false;

        /// <summary>Allows Event-only species when randomizing.</summary>
        [Category(Misc), Description("Allows Event-only species when randomizing.")]
        public bool Events { get; set; } = false;

        /// <summary>Allows Shedinja as a random species when randomizing.</summary>
        [Category(Misc), Description("Allows Shedinja as a random species when randomizing.")]
        public bool Shedinja { get; set; } = false;

        /// <summary>Requires the randomized species to be in the same EXP Group as the original species.</summary>
        [Category(Misc), Description("Requires the randomized species to be in the same EXP Group as the original species. Note: might not be used by all randomizers.")]
        public bool EXPGroup { get; set; } = false;

        /// <summary>Requires the randomized species to have a similar Base Stat Total as the original species.</summary>
        [Category(Misc), Description("Requires the randomized species to have a similar Base Stat Total as the original species. Note: might not be used by all randomizers.")]
        public bool BST { get; set; } = true;

        /// <summary>Requires the randomized species to have a similar typing as the original species.</summary>
        [Category(Misc), Description("Requires the randomized species to have a similar typing as the original species. Note: might not be used by all randomizers.")]
        public bool Type { get; set; } = false;

        /// <summary>
        /// Gets an array of Species according to the specified settings.
        /// </summary>
        /// <param name="maxSpecies">Max Species ID</param>
        /// <param name="generation"></param>
        /// <returns></returns>
        public int[] GetSpecies(int maxSpecies, int generation)
        {
            var list = new List<int>();
            if (Gen1) AddGen1Species(list, maxSpecies);
            if (Gen2) AddGen2Species(list, maxSpecies);
            if (Gen3) AddGen3Species(list, maxSpecies);
            if (Gen4) AddGen4Species(list, maxSpecies);
            if (Gen5) AddGen5Species(list, maxSpecies);
            if (Gen6) AddGen6Species(list, maxSpecies);
            if (Gen7) AddGen7Species(list, maxSpecies);

            if (generation == 7 && Gen1 && maxSpecies <= Legal.MaxSpeciesID_7_GG)
                AddGGEvents(list);

            return list.Count == 0 ? GetSpeciesAll(maxSpecies) : list.ToArray();
        }

        private int[] GetSpeciesAll(int maxSpecies) => Enumerable.Range(1, maxSpecies).ToArray();

        private void AddGen1Species(List<int> list, int maxSpecies)
        {
            if (maxSpecies <= 0)
                return;
            list.AddRange(Enumerable.Range(1, 143)); // Bulbasaur - Snorlax
            list.AddRange(Enumerable.Range(147, 3)); // Dratini - Dragonite

            if (Legends)
            {
                list.AddRange(Enumerable.Range(144, 3)); // Birds
                list.Add(150); // Mewtwo
            }
            if (Events) list.Add(151); // Mew
        }

        private void AddGen2Species(List<int> list, int maxSpecies)
        {
            if (maxSpecies <= 151)
                return;
            list.AddRange(Enumerable.Range(152, 91)); // Chikorita - Blissey
            list.AddRange(Enumerable.Range(246, 3)); // Larvitar - Tyranitar

            if (Legends)
            {
                list.AddRange(Enumerable.Range(243, 3)); // Beasts
                list.AddRange(Enumerable.Range(249, 2)); // Lugia & Ho-Oh
            }
            if (Events) list.Add(251); // Celebi
        }

        private void AddGen3Species(List<int> list, int maxSpecies)
        {
            if (maxSpecies <= 251)
                return;
            list.AddRange(Enumerable.Range(252, 40)); // Treecko - Ninjask
            list.AddRange(Enumerable.Range(293, 84)); // Whismur - Metagross
            if (Shedinja) list.Add(292); // Shedinja
            if (Legends) list.AddRange(Enumerable.Range(377, 8)); // Regi, Lati, Mascot
            if (Events) list.AddRange(Enumerable.Range(385, 2)); // Jirachi/Deoxys
        }

        private void AddGen4Species(List<int> list, int maxSpecies)
        {
            if (maxSpecies <= 386)
                return;
            list.AddRange(Enumerable.Range(387, 93)); // Turtwig - Rotom
            if (Legends) list.AddRange(Enumerable.Range(480, 9)); // Sinnoh Legends
            if (Events) list.AddRange(Enumerable.Range(489, 5)); // Phione, Manaphy, Darkrai, Shaymin, Arceus
        }

        private void AddGen5Species(List<int> list, int maxSpecies)
        {
            if (maxSpecies <= 493)
                return;
            list.AddRange(Enumerable.Range(495, 143)); // Snivy - Volcarona
            if (Legends) list.AddRange(Enumerable.Range(638, 9)); // Unova Legends
            if (Events) list.Add(494); list.AddRange(Enumerable.Range(647, 3)); // Victini, Keldeo, Meloetta, Genesect
        }

        private void AddGen6Species(List<int> list, int maxSpecies)
        {
            if (maxSpecies <= 649)
                return;
            list.AddRange(Enumerable.Range(650, 66)); // Chespin - Noivern
            if (Legends) list.AddRange(Enumerable.Range(716, 3)); // Kalos Legends
            if (Events) list.AddRange(Enumerable.Range(719, 3)); // Diancie, Hoopa, Volcanion
        }

        private void AddGen7Species(List<int> list, int maxSpecies)
        {
            if (maxSpecies <= 721)
                return;
            list.AddRange(Enumerable.Range(722, 50)); // Rowlet - Pyukumuku
            list.AddRange(Enumerable.Range(774, 11)); // Minior - Kommo-o

            if (Legends)
            {
                list.AddRange(Enumerable.Range(772, 2)); // Type: Null, Silvally
                list.AddRange(Enumerable.Range(785, 16)); // Tapus, Legends, UBs
            }
            if (Events) list.AddRange(Enumerable.Range(801, 2)); // Magearna, Marshadow

            if (maxSpecies >= Legal.MaxSpeciesID_7_USUM) // USUM
            {
                if (Legends) list.AddRange(Enumerable.Range(803, 4)); // Poipole, Naganadel, Stakataka, Blacephalon
                if (Events) list.Add(807); // Zeraora
            }
            if (maxSpecies >= Legal.MaxSpeciesID_7_GG) // USUM
            {
                if (Events)
                    AddGGEvents(list);
            }
        }

        private void AddGGEvents(List<int> list)
        {
            list.AddRange(Enumerable.Range(808, Legal.MaxSpeciesID_7_GG - Legal.MaxAbilityID_7_USUM)); // *
        }
    }
}
