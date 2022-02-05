using System;
using pkNX.Structures;
using Util = pkNX.Randomization.Util;

namespace pkNX.Game
{
    public class TypeChartEditor
    {
        public byte[] Data;
        public int Width => (int)Math.Sqrt(Data.Length);
        public int Height => (int)Math.Sqrt(Data.Length);

        public TypeChartEditor(byte[] data) => Data = data;

        public void Randomize()
        {
            var rnd = Util.Random;
            for (int i = 0; i < Data.Length; i++)
            {
                var rv = rnd.Next(100);
                Data[i] = GetEffectiveness(rv);
            }
        }

        private static byte GetEffectiveness(int rv)
        {
            return rv switch
            {
                <  2 => (byte)TypeEffectiveness.Immune,  // 2%
                < 19 => (byte)TypeEffectiveness.NotVery, // 17%
                < 36 => (byte)TypeEffectiveness.Super,   // 17%
                   _ => (byte)TypeEffectiveness.Normal,
            };
        }

        private static readonly uint[] Colors =
        {
            0xFF000000,
            0, // unused
            0xFFFF0000,
            0, // unused
            0xFFFFFFFF,
            0, 0, 0, // unused
            0xFF008000,
        };

        public static byte[] GetTypeChartImageData(int itemsize, int itemsPerRow, byte[] vals, out int width, out int height)
        {
            width = itemsize * itemsPerRow;
            height = itemsize * vals.Length / itemsPerRow;
            var bmpData = new byte[4 * width * height];

            // loop over area
            for (int i = 0; i < vals.Length; i++)
            {
                int X = i % itemsPerRow;
                int Y = i / itemsPerRow;

                // Plop into image
                byte[] itemColor = BitConverter.GetBytes(Colors[vals[i]]);
                for (int x = 0; x < itemsize * itemsize; x++)
                {
                    var ofs = (((Y * itemsize) + (x % itemsize)) * width * 4) + (((X * itemsize) + (x / itemsize)) * 4);
                    Buffer.BlockCopy(itemColor, 0, bmpData, ofs, 4);
                }
            }
            // slap on a grid
            byte[] gridColor = BitConverter.GetBytes(0x17000000);
            for (int i = 0; i < width * height; i++)
            {
                if (i % itemsize == 0 || i / (itemsize * itemsPerRow) % itemsize == 0)
                {
                    var ofs = (i / (itemsize * itemsPerRow) * width * 4) + (i % (itemsize * itemsPerRow) * 4);
                    Buffer.BlockCopy(gridColor, 0, bmpData, ofs, 4);
                }
            }

            return bmpData;
        }
    }
}
