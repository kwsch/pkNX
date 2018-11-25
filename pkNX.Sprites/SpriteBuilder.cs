using System.Collections.Generic;
using System.Drawing;
using pkNX.Sprites.Properties;

namespace pkNX.Sprites
{
    public static class SpriteBuilder
    {
        public static bool ShowEggSpriteAsItem { get; set; } = true;

        // Data Requests
        public static string GetResourceStringBall(int ball) => $"_ball{ball}";
        private const string ResourceSeparator = "_";
        private const string ResourcePikachuCosplay = "c"; // osplay
        private const string ResourceShiny = "s"; // hiny
        private const string ResourceGGStarter = "p"; //artner
        public static bool AllowShinySprite { get; set; } = true;

        public const int Generation = 7;

        /// <summary>
        /// Species that show their default Species sprite regardless of current form
        /// </summary>
        public static readonly HashSet<int> SpeciesDefaultFormSprite = new HashSet<int>
        {
            414, // Mothim
            493, // Arceus
            664, // Scatterbug
            665, // Spewpa
            773, // Silvally
            778, // Mimikyu
        };

        /// <summary>
        /// Species that show a Gender specific Sprite
        /// </summary>
        public static readonly HashSet<int> SpeciesGenderedSprite = new HashSet<int>
        {
            521, // Unfezant
            592, // Frillish
            593, // Jellicent
            668, // Pyroar
        };

        public static string GetResourceStringSprite(int species, int form, int gender, int generation = Generation, bool shiny = false)
        {
            if (SpeciesDefaultFormSprite.Contains(species)) // Species who show their default sprite regardless of Form
                form = 0;

            var sb = new System.Text.StringBuilder();
            { sb.Append(ResourceSeparator); sb.Append(species); }
            if (form > 0)
            { sb.Append(ResourceSeparator); sb.Append(form); }
            else if (gender == 1 && SpeciesGenderedSprite.Contains(species)) // Frillish & Jellicent, Unfezant & Pyroar
            { sb.Append(ResourceSeparator); sb.Append(gender); }

            if (species == 25 && form > 0 && generation == 6) // Cosplay Pikachu
                sb.Append(ResourcePikachuCosplay);
            else if ((species == 25 && form == 8) || (species == 133 && form == 1))
                sb.Append(ResourceGGStarter);

            if (shiny && AllowShinySprite)
                sb.Append(ResourceShiny);
            return sb.ToString();
        }

        public static Image GetSprite(int species, int form, int gender, int heldItem, bool isEgg, bool isShiny, int generation = -1)
        {
            if (species == 0)
                return Resources._0;

            var baseImage = GetBaseImage(species, form, gender, isShiny, generation);

            return GetSprite(baseImage, species, heldItem, isEgg, isShiny, generation);
        }

        public static Image GetSprite(Image baseSprite, int species, int heldItem, bool isEgg, bool isShiny, int generation = -1)
        {
            if (isEgg)
                baseSprite = LayerOverImageEgg(baseSprite, species, heldItem != 0);
            if (isShiny)
                baseSprite = LayerOverImageShiny(baseSprite);
            if (heldItem > 0)
                baseSprite = LayerOverImageItem(baseSprite, heldItem, generation);
            return baseSprite;
        }

        private static Image GetBaseImage(int species, int form, int gender, bool shiny, int generation)
        {
            var img = FormConverter.IsTotemForm(species, form)
                ? GetBaseImageTotem(species, form, gender, shiny, generation)
                : GetBaseImageDefault(species, form, gender, shiny, generation);
            return img ?? GetBaseImageFallback(species, form, gender, shiny, generation);
        }

        private static Image GetBaseImageTotem(int species, int form, int gender, bool shiny, int generation)
        {
            var baseform = FormConverter.GetTotemBaseForm(species, form);
            var file = GetResourceStringSprite(species, baseform, gender, generation, shiny);
            var baseImage = (Image)Resources.ResourceManager.GetObject(file);
            return ImageUtil.ToGrayscale(baseImage);
        }

        private static Image GetBaseImageDefault(int species, int form, int gender, bool shiny, int generation)
        {
            var file = GetResourceStringSprite(species, form, gender, generation, shiny);
            return (Image)Resources.ResourceManager.GetObject(file);
        }

        private static Image GetBaseImageFallback(int species, int form, int gender, bool shiny, int generation)
        {
            Image baseImage;
            if (shiny) // try again without shiny
            {
                var file = GetResourceStringSprite(species, form, gender, generation);
                baseImage = (Image)Resources.ResourceManager.GetObject(file);
                if (baseImage != null)
                    return baseImage;
            }

            // try again without form
            baseImage = (Image)Resources.ResourceManager.GetObject($"_{species}");
            if (baseImage == null) // failed again
                return Resources.unknown;
            return ImageUtil.LayerImage(baseImage, Resources.unknown, 0, 0, .5);
        }

        private static Image LayerOverImageItem(Image baseImage, int item, int generation)
        {
            Image itemimg = (Image)Resources.ResourceManager.GetObject($"item_{item}") ?? Resources.helditem;
            if (generation >= 2 && generation <= 4 && 328 <= item && item <= 419) // gen2/3/4 TM
                itemimg = Resources.item_tm;

            // Redraw
            int x = 22 + ((15 - itemimg.Width) / 2);
            if (x + itemimg.Width > baseImage.Width)
                x = baseImage.Width - itemimg.Width;
            int y = 15 + (15 - itemimg.Height);
            return ImageUtil.LayerImage(baseImage, itemimg, x, y);
        }

        private static Image LayerOverImageShiny(Image baseImage)
        {
            // Add shiny star to top left of image.
            var rare = Resources.rare_icon;
            return ImageUtil.LayerImage(baseImage, rare, 0, 0, 0.7);
        }

        private static Image LayerOverImageEgg(Image baseImage, int species, bool hasItem)
        {
            if (ShowEggSpriteAsItem && !hasItem)
                return LayerOverImageEggAsItem(baseImage, species);
            return LayerOverImageEggTransparentSpecies(baseImage, species);
        }

        private static Image GetEggSprite(int species) => species == 490 ? Resources._490_e : Resources.egg;

        private const double EggUnderLayerTransparency = 0.33;
        private const int EggOverLayerAsItemShiftX = 9;
        private const int EggOverLayerAsItemShiftY = 2;

        private static Image LayerOverImageEggTransparentSpecies(Image baseImage, int species)
        {
            // Partially transparent species.
            baseImage = ImageUtil.ChangeOpacity(baseImage, EggUnderLayerTransparency);
            // Add the egg layer over-top with full opacity.
            var egg = GetEggSprite(species);
            return ImageUtil.LayerImage(baseImage, egg, 0, 0);
        }

        private static Image LayerOverImageEggAsItem(Image baseImage, int species)
        {
            var egg = GetEggSprite(species);
            return ImageUtil.LayerImage(baseImage, egg, EggOverLayerAsItemShiftX, EggOverLayerAsItemShiftY); // similar to held item, since they can't have any
        }
    }
}
