﻿using System.Drawing;
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

        private const double UnknownFormTransparency = 0.5;
        private const double ShinyTransparency = 0.7;
        private const double EggUnderLayerTransparency = 0.33;

        protected virtual string GetSpriteStringSpeciesOnly(int species) => $"_{species}";
        protected virtual string GetSpriteAll(int species, int form, int gender, bool shiny, int generation) => SpriteName.GetResourceStringSprite(species, form, gender, generation, shiny);
        protected virtual string GetItemResourceName(int item) => $"item_{item}";
        protected virtual Image Unknown => Resources.unknown;
        protected virtual Image GetEggSprite(int species) => species == 490 ? Resources._490_e : Resources.egg;

        public Image GetSprite(int species, int form, int gender, int heldItem, bool isEgg, bool isShiny, int generation = -1)
        {
            if (species == 0)
                return Resources._0;

            var baseImage = GetBaseImage(species, form, gender, isShiny, generation);
            return GetSprite(baseImage, species, heldItem, isEgg, isShiny, generation);
        }

        public Image GetSprite(Image baseSprite, int species, int heldItem, bool isEgg, bool isShiny, int generation = -1, bool isBoxBGRed = false)
        {
            if (isEgg)
                baseSprite = LayerOverImageEgg(baseSprite, species, heldItem != 0);
            if (heldItem > 0)
                baseSprite = LayerOverImageItem(baseSprite, heldItem, generation);
            if (isShiny)
                baseSprite = LayerOverImageShiny(baseSprite);
            return baseSprite;
        }

        private Image GetBaseImage(int species, int form, int gender, bool shiny, int generation)
        {
            var img = FormConverter.IsTotemForm(species, form)
                        ? GetBaseImageTotem(species, form, gender, shiny, generation)
                        : GetBaseImageDefault(species, form, gender, shiny, generation);
            return img ?? GetBaseImageFallback(species, form, gender, shiny, generation);
        }

        private Image GetBaseImageTotem(int species, int form, int gender, bool shiny, int generation)
        {
            var baseform = FormConverter.GetTotemBaseForm(species, form);
            var baseImage = GetBaseImageDefault(species, baseform, gender, shiny, generation);
            if (baseImage == null)
                return null;
            return ImageUtil.ToGrayscale(baseImage);
        }

        private Image GetBaseImageDefault(int species, int form, int gender, bool shiny, int generation)
        {
            var file = GetSpriteAll(species, form, gender, shiny, generation);
            return (Image)Resources.ResourceManager.GetObject(file);
        }

        private Image GetBaseImageFallback(int species, int form, int gender, bool shiny, int generation)
        {
            if (shiny) // try again without shiny
            {
                var img = GetBaseImageDefault(species, form, gender, false, generation);
                if (img != null)
                    return img;
            }

            // try again without form
            var baseImage = (Image)Resources.ResourceManager.GetObject(GetSpriteStringSpeciesOnly(species));
            if (baseImage == null) // failed again
                return Unknown;
            return ImageUtil.LayerImage(baseImage, Unknown, 0, 0, UnknownFormTransparency);
        }

        private Image LayerOverImageItem(Image baseImage, int item, int generation)
        {
            Image itemimg = (Image)Resources.ResourceManager.GetObject(GetItemResourceName(item)) ?? Resources.bitem_unk;
            if (328 <= item && item <= 419) // gen2/3/4 TM
                itemimg = Resources.bitem_tm;
            else if (1130 <= item && item <= 1229) // Gen8 TR
                itemimg = Resources.bitem_tr;

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
    /// 30 high, 40 wide sprite builder
    /// </summary>
    public class SpriteBuilder3040 : SpriteBuilder
    {
        public override int Height => 30;
        public override int Width => 40;

        protected override int ItemShiftX => 22;
        protected override int ItemShiftY => 15;
        protected override int ItemMaxSize => 15;
        protected override int EggItemShiftX => 9;
        protected override int EggItemShiftY => 2;

        public override Bitmap Hover => Resources.slotHover;
        public override Bitmap View => Resources.slotView;
        public override Bitmap Set => Resources.slotSet;
        public override Bitmap Delete => Resources.slotDel;
        public override Bitmap Transparent => Resources.slotTrans;
        public override Bitmap Drag => Resources.slotDrag;
        public override Bitmap UnknownItem => Resources.helditem; 
    }

    /// <summary>
    /// 56 high, 68 wide sprite builder
    /// </summary>
    public class SpriteBuilder5668 : SpriteBuilder
    {
        public override int Height => 56;
        public override int Width => 68;

        protected override int ItemShiftX => 52;
        protected override int ItemShiftY => 28;
        protected override int ItemMaxSize => 32;
        protected override int EggItemShiftX => 9;
        protected override int EggItemShiftY => 2;

        protected override string GetSpriteStringSpeciesOnly(int species) => 'b' + $"_{species}";
        protected override string GetSpriteAll(int species, int form, int gender, bool shiny, int generation) => 'b' + SpriteName.GetResourceStringSprite(species, form, gender, generation, shiny);
        protected override string GetItemResourceName(int item) => 'b' + $"item_{item}";
        protected override Image Unknown => Resources.b_0;
        protected override Image GetEggSprite(int species) => Resources.egg; // no manaphy egg sprite (yet)

        public override Bitmap Hover => Resources.slotHover68;
        public override Bitmap View => Resources.slotView68;
        public override Bitmap Set => Resources.slotSet68;
        public override Bitmap Delete => Resources.slotDel68;
        public override Bitmap Transparent => Resources.slotTrans68;
        public override Bitmap Drag => Resources.slotDrag68;
        public override Bitmap UnknownItem => Resources.bitem_unk;
    }
}
