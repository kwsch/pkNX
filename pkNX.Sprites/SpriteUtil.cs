using System.Drawing;
using pkNX.Sprites.Properties;

namespace pkNX.Sprites
{
    public static class SpriteUtil
    {
        public static readonly SpriteBuilder3040 SB17 = new SpriteBuilder3040();
        public static readonly SpriteBuilder5668 SB8 = new SpriteBuilder5668();
        public static SpriteBuilder Spriter { get; set; } = SB17;

        public static Image GetBallSprite(int ball)
        {
            string resource = SpriteName.GetResourceStringBall(ball);
            return (Bitmap)Resources.ResourceManager.GetObject(resource) ?? Resources._ball4; // Poké Ball (default)
        }

        public static Image GetSprite(int species, int form, int gender, int item, bool isegg, bool shiny, int generation = -1)
        {
            return Spriter.GetSprite(species, form, gender, item, isegg, shiny, generation);
        }

        public static void Initialize(bool big) => Spriter = big ? (SpriteBuilder)SB8 : SB17;
    }
}
