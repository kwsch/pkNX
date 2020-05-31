using System.Collections.Generic;
using System.Text;
using pkNX.Structures;

namespace pkNX.Sprites
{
    public static class SpriteName
    {
        public static bool AllowShinySprite { get; set; } = true;

        private const string Separator = "_";
        private const string Cosplay = "c";
        private const string Shiny = "s";
        private const string GGStarter = "p";

        public static string GetResourceStringBall(int ball) => $"_ball{ball}";

        /// <summary>
        /// Gets the resource name of the Pokémon sprite.
        /// </summary>
        public static string GetResourceStringSprite(int species, int form, int gender, int generation = 8, bool shiny = false)
        {
            if (SpeciesDefaultFormSprite.Contains(species)) // Species who show their default sprite regardless of Form
                form = 0;

            var sb = new StringBuilder();
            { sb.Append(Separator); sb.Append(species); }

            if (form != 0)
            {
                sb.Append(Separator); sb.Append(form);

                if (species == (int)Species.Pikachu)
                {
                    if (generation == 6)
                        sb.Append(Cosplay);
                    else if (form == 8)
                        sb.Append(GGStarter);
                }
                else if (species == (int)Species.Eevee)
                {
                    if (form == 1)
                        sb.Append(GGStarter);
                }
            }
            else if (gender == 2 && SpeciesGenderedSprite.Contains(species))
            {
                sb.Append('f');
            }

            if (shiny && AllowShinySprite)
                sb.Append(Shiny);
            return sb.ToString();
        }

        /// <summary>
        /// Species that show their default Species sprite regardless of current form
        /// </summary>
        public static readonly HashSet<int> SpeciesDefaultFormSprite = new HashSet<int>
        {
            (int)Species.Mothim,
            (int)Species.Scatterbug,
            (int)Species.Spewpa,
            (int)Species.Rockruff,
            (int)Species.Mimikyu,
            (int)Species.Sinistea,
            (int)Species.Polteageist,
        };

        /// <summary>
        /// Species that show a Gender specific Sprite
        /// </summary>
        public static readonly HashSet<int> SpeciesGenderedSprite = new HashSet<int>
        {
            (int)Species.Hippopotas,
            (int)Species.Hippowdon,
            (int)Species.Unfezant,
            (int)Species.Frillish,
            (int)Species.Jellicent,
            (int)Species.Pyroar,
        };
    }
}
