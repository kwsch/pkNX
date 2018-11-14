using System;
using System.Collections.Generic;

namespace pkNX.Randomization
{
    public static class Util
    {
        public static Random Random { get; private set; } = new Random();

        public static void ReseedRand(int seed)
        {
            Random = new Random(seed);
            Structures.Util.Rand = Random;
        }

        public static uint Rand32() => (uint)Random.Next(1 << 30) << 2 | (uint)Random.Next(1 << 2);

        public static void Shuffle<T>(IList<T> array)
        {
            int n = array.Count;
            for (int i = 0; i < n; i++)
            {
                int r = i + (int)(Random.NextDouble() * (n - i));
                var t = array[r];
                array[r] = array[i];
                array[i] = t;
            }
        }
    }
}
