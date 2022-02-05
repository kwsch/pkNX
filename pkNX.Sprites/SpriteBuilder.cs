using System.Drawing;
using pkNX.Structures;
using pkNX.Sprites.Properties;

namespace pkNX.Sprites
{
    public abstract class SpriteBuilder
    {
        public static bool ShowEggSpriteAsItem { get; set; } = true;

        public abstract int Width { get; }
        public abstract int Height { get; }

        protected abstract int ItemShiftX { get; }
        protected abstract int ItemShiftY { get; }
        protected abstract int ItemMaxSize { get; }
        protected abstract int EggItemShiftX { get; }
        protected abstract int EggItemShiftY { get; }

        public abstract Bitmap Hover { get; }
        public abstract Bitmap View { get; }
        public abstract Bitmap Set { get; }
        public abstract Bitmap Delete { get; }
        public abstract Bitmap Transparent { get; }
        public abstract Bitmap Drag { get; }
        public abstract Bitmap UnknownItem { get; }
        public abstract Bitmap None { get; }
        public abstract Bitmap ItemTM { get; }
        public abstract Bitmap ItemTR { get; }

        private const double UnknownFormTransparency = 0.5;
        private const double ShinyTransparency = 0.7;
        private const double EggUnderLayerTransparency = 0.33;

        protected abstract string GetSpriteStringSpeciesOnly(int species);

        protected abstract string GetSpriteAll(int species, int form, int gender, bool shiny, bool gmax, int generation);
        protected abstract string GetItemResourceName(int item);
        protected abstract Bitmap Unknown { get; }
        protected abstract Bitmap GetEggSprite(int species);

        public Image GetSprite(int species, int form, int gender, int heldItem, bool isEgg, bool isShiny, bool isGigantamax, int generation = -1)
        {
            if (species == 0)
                return None;

            var baseImage = GetBaseImage(species, form, gender, isShiny, isGigantamax, generation);
            return GetSprite(baseImage, species, heldItem, isEgg, isShiny, isGigantamax, generation);
        }

        public Image GetSprite(Image baseSprite, int species, int heldItem, bool isEgg, bool isShiny, bool isGigantamax, int generation = -1, bool isBoxBGRed = false)
        {
            if (isEgg)
                baseSprite = LayerOverImageEgg(baseSprite, species, heldItem != 0);
            if (heldItem > 0)
                baseSprite = LayerOverImageItem(baseSprite, heldItem, generation);
            if (isShiny)
                baseSprite = LayerOverImageShiny(baseSprite);
            return baseSprite;
        }

        private Image GetBaseImage(int species, int form, int gender, bool shiny, bool gmax, int generation)
        {
            var img = FormConverter.IsTotemForm(species, form)
                        ? GetBaseImageTotem(species, form, gender, shiny, gmax, generation)
                        : GetBaseImageDefault(species, form, gender, shiny, gmax, generation);
            return img ?? GetBaseImageFallback(species, form, gender, shiny, gmax, generation);
        }

        private Image? GetBaseImageTotem(int species, int form, int gender, bool shiny, bool gmax, int generation)
        {
            var baseform = FormConverter.GetTotemBaseForm(species, form);
            var baseImage = GetBaseImageDefault(species, baseform, gender, shiny, gmax, generation);
            if (baseImage == null)
                return null;
            return ImageUtil.ToGrayscale(baseImage);
        }

        private Image? GetBaseImageDefault(int species, int form, int gender, bool shiny, bool gmax, int generation)
        {
            var file = GetSpriteAll(species, form, gender, shiny, gmax, generation);
            return (Image?)Resources.ResourceManager.GetObject(file);
        }

        private Image GetBaseImageFallback(int species, int form, int gender, bool shiny, bool gmax, int generation)
        {
            if (shiny) // try again without shiny
            {
                var img = GetBaseImageDefault(species, form, gender, false, gmax, generation);
                if (img != null)
                    return img;
            }

            // try again without form
            var baseImage = (Image?)Resources.ResourceManager.GetObject(GetSpriteStringSpeciesOnly(species));
            if (baseImage == null) // failed again
                return Unknown;
            return ImageUtil.LayerImage(baseImage, Unknown, 0, 0, UnknownFormTransparency);
        }

        private Image LayerOverImageItem(Image baseImage, int item, int generation)
        {
            Image itemimg = (Image?)Resources.ResourceManager.GetObject(GetItemResourceName(item)) ?? Resources.bitem_unk;
            if (item is >= 328 and <= 419) // gen2/3/4 TM
                itemimg = ItemTM;
            else if (item is >= 1130 and <= 1229) // Gen8 TR
                itemimg = ItemTR;

            // Redraw item in bottom right corner; since images are cropped, try to not have them at the edge
            int x = ItemShiftX + ((ItemMaxSize - itemimg.Width) / 2);
            if (x + itemimg.Width > baseImage.Width)
                x = baseImage.Width - itemimg.Width;
            int y = ItemShiftY + (ItemMaxSize - itemimg.Height);
            return ImageUtil.LayerImage(baseImage, itemimg, x, y);
        }

        private static Image LayerOverImageShiny(Image baseImage)
        {
            // Add shiny star to top left of image.
            var rare = Resources.rare_icon;
            return ImageUtil.LayerImage(baseImage, rare, 0, 0, ShinyTransparency);
        }

        private Image LayerOverImageEgg(Image baseImage, int species, bool hasItem)
        {
            if (ShowEggSpriteAsItem && !hasItem)
                return LayerOverImageEggAsItem(baseImage, species);
            return LayerOverImageEggTransparentSpecies(baseImage, species);
        }

        private Image LayerOverImageEggTransparentSpecies(Image baseImage, int species)
        {
            // Partially transparent species.
            baseImage = ImageUtil.ChangeOpacity(baseImage, EggUnderLayerTransparency);
            // Add the egg layer over-top with full opacity.
            var egg = GetEggSprite(species);
            return ImageUtil.LayerImage(baseImage, egg, 0, 0);
        }

        private Image LayerOverImageEggAsItem(Image baseImage, int species)
        {
            var egg = GetEggSprite(species);
            return ImageUtil.LayerImage(baseImage, egg, EggItemShiftX, EggItemShiftY); // similar to held item, since they can't have any
        }
    }

    /// <summary>
    /// 56 high, 68 wide sprite builder
    /// </summary>
    public class SpriteBuilder5668 : SpriteBuilder
    {
        public override int Height => 56;
        public override int Width => 68;

        protected override int ItemShiftX => 52;
        protected override int ItemShiftY => 24;
        protected override int ItemMaxSize => 32;
        protected override int EggItemShiftX => 9;
        protected override int EggItemShiftY => 2;

        protected override string GetSpriteStringSpeciesOnly(int species) => 'b' + $"_{species}";
        protected override string GetSpriteAll(int species, int form, int gender, bool shiny, bool gmax, int generation) => 'b' + SpriteName.GetResourceStringSprite(species, form, gender, generation, shiny, gmax);
        protected override string GetItemResourceName(int item) => 'b' + $"item_{item}";
        protected override Bitmap Unknown => Resources.b_unknown;
        protected override Bitmap GetEggSprite(int species) => species == (int)Species.Manaphy ? Resources.b_490_e : Resources.b_egg;

        public override Bitmap Hover => Resources.slotHover68;
        public override Bitmap View => Resources.slotView68;
        public override Bitmap Set => Resources.slotSet68;
        public override Bitmap Delete => Resources.slotDel68;
        public override Bitmap Transparent => Resources.slotTrans68;
        public override Bitmap Drag => Resources.slotDrag68;
        public override Bitmap UnknownItem => Resources.bitem_unk;
        public override Bitmap None => Resources.b_0;
        public override Bitmap ItemTM => Resources.bitem_tm;
        public override Bitmap ItemTR => Resources.bitem_tr;
    }
}
