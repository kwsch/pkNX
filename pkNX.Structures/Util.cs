using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pkNX.Structures
{
    public static class Util
    {
        public static Random Rand { get; set; } = new Random();
        internal static uint Rand32() => (uint)Rand.Next(1 << 30) << 2 | (uint)Rand.Next(1 << 2);

        private static T[] GetArray<T>(IReadOnlyList<byte[]> entries, Func<byte[], T> del)
        {
            var data = new T[entries.Count];
            for (int i = 0; i < data.Length; i++)
                data[i] = del(entries[i]);
            return data;
        }

        public static T[] GetArray<T>(byte[][] entries, Func<byte[], T> del, int size)
        {
            var data = new T[entries.Length / size];
            for (int i = 0; i < data.Length; i++)
                data[i] = del(entries[i]);
            return data;
        }

        public static T[] GetArray<T>(this byte[] entries, Func<byte[], int, T> del, int size)
        {
            if (entries == null || entries.Length < size)
                return Array.Empty<T>();

            var data = new T[entries.Length/size];
            for (int i = 0; i < data.Length; i+= size)
                data[i] = del(entries, i);
            return data;
        }

        public static T[] GetArray<T>(this Task<byte[][]> task, Func<byte[], T> del)
        {
            return GetArray(task.Result, del);
        }

        public static T[] GetArray<T>(this Task<byte[]> task, Func<byte[], int, T> del, int size) => GetArray(task.Result, del, size);

        public static string[] GetHexLines(byte[] data, int count = 4)
        {
            if (data == null)
                return Array.Empty<string>();

            // Generates an x-byte wide space separated string array; leftovers included at the end.
            string[] s = new string[(data.Length / count) + (data.Length % count > 0 ? 1 : 0)];
            for (int i = 0; i < s.Length; i++)
                s[i] = BitConverter.ToString(data.Skip(i * count).Take(count).ToArray()).Replace('-', ' ');
            return s;
        }

        public static string[] GetHexLines(uint[] data) => GetHexLines(GetBytes(data));

        public static byte[] GetBytes(uint[] data)
        {
            byte[] result = new byte[data.Length * 4];
            for (int i = 0; i < data.Length; i++)
            {
                int o = i * 4;
                var val = data[i];
                result[o + 0] = (byte)(val >> 0);
                result[o + 1] = (byte)(val >> 8);
                result[o + 2] = (byte)(val >> 16);
                result[o + 3] = (byte)(val >> 24);
            }
            return result;
        }
    }
}
