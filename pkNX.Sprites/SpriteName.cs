using System.Collections.Generic;
using System.Text;
using pkNX.Structures;

namespace pkNX.Sprites
{
    public static class SpriteName
    {
        public static bool AllowShinySprite { get; set; } = true;
        public static bool AllowGigantamaxSprite { get; set; } = true;

        private const char Separator = '_';
        private const char Cosplay = 'c';
        private const char Shiny = 's';
        private const string Gigantamax = "gmax";
        private const char GGStarter = 'p';

        public static string GetResourceStringBall(int ball) => $"_ball{ball}";

        /// <summary>
        /// Gets the resource name of the Pokémon sprite.
        /// </summary>
        public static string GetResourceStringSprite(int species, int form, int gender, int generation = 8, bool shiny = false, bool gmax = false)
        {
            if (SpeciesDefaultFormSprite.Contains(species) && !gmax) // Species who show their default sprite regardless of Form
                form = 0;

            if (gmax && species is (int)Species.Toxtricity or (int)Species.Alcremie) // same sprites for all altform gmaxes
                form = 0;

            switch (form)
            {
                case 30 when species is >= (int)Species.Scatterbug and <= (int)Species.Vivillon: // save file specific
                    form = 0;
                    break;
                case 31 when species is (int)Species.Unown or (int)Species.Deerling or (int)Species.Sawsbuck: // Random
                    form = 0;
                    break;
            }

            var sb = new StringBuilder();
            sb.Append(Separator).Append(species);

            if (form != 0)
            {
                sb.Append(Separator)
                    .Append(form);

                if (species == (int)Species.Pikachu)
                {
                    if (generation == 6)
                    {
                        sb.Append(Cosplay);
                        gender = 2; // Cosplay Pikachu gift can only be Female, but personal entries are set to be either Gender
                    }
                    else if (form == 8)
                    {
                        sb.Append(GGStarter);
                    }
                }
                else if (species == (int)Species.Eevee)
                {
                    if (form == 1)
                        sb.Append(GGStarter);
                }
            }
            if (gender == 2 && SpeciesGenderedSprite.Contains(species) && !gmax)
            {
                sb.Append('f');
            }

            if (gmax && AllowGigantamaxSprite)
            {
                sb.Append(Separator);
                sb.Append(Gigantamax);
            }
            if (shiny && AllowShinySprite)
                sb.Append(Shiny);
            return sb.ToString();
        }

        /// <summary>
        /// Species that show their default Species sprite regardless of current form
        /// </summary>
        public static readonly HashSet<int> SpeciesDefaultFormSprite = new()
        {
            (int)Species.Mothim,
            (int)Species.Scatterbug,
            (int)Species.Spewpa,
            (int)Species.Rockruff,
            (int)Species.Mimikyu,
            (int)Species.Sinistea,
            (int)Species.Polteageist,
            (int)Species.Urshifu,
        };

        /// <summary>
        /// Species that show a Gender specific Sprite
        /// </summary>
        public static readonly HashSet<int> SpeciesGenderedSprite = new()
        {
            (int)Species.Pikachu,
            (int)Species.Hippopotas,
            (int)Species.Hippowdon,
            (int)Species.Unfezant,
            (int)Species.Frillish,
            (int)Species.Jellicent,
            (int)Species.Pyroar,
        };
    }
}
