using System;
using System.IO;

namespace pkNX.Structures
{
    public class Learnset6 : Learnset
    {
        public Learnset6(byte[] data)
        {
            if (data.Length < 4 || data.Length % 4 != 0)
            { Count = 0; Levels = Moves = Array.Empty<int>(); return; }
            Count = (data.Length / 4) - 1;
            Moves = new int[Count];
            Levels = new int[Count];
            using var ms = new MemoryStream(data);
            using var br = new BinaryReader(ms);
            for (int i = 0; i < Count; i++)
            {
                Moves[i] = br.ReadInt16();
                Levels[i] = br.ReadInt16();
            }
        }

        public override byte[] Write()
        {
            Count = (ushort)Moves.Length;
            using MemoryStream ms = new MemoryStream();
            using BinaryWriter bw = new BinaryWriter(ms);
            for (int i = 0; i < Count; i++)
            {
                bw.Write((short)Moves[i]);
                bw.Write((short)Levels[i]);
            }
            bw.Write(-1);
            return ms.ToArray();
        }

        public static Learnset[] GetArray(byte[][] entries)
        {
            Learnset[] data = new Learnset[entries.Length];
            for (int i = 0; i < data.Length; i++)
                data[i] = new Learnset6(entries[i]);
            return data;
        }
    }
}