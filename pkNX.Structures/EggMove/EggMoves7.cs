using System;
using System.IO;

namespace pkNX.Structures
{
    public class EggMoves7 : EggMoves
    {
        public EggMoves7(byte[] data)
        {
            if (data.Length < 2 || data.Length % 2 != 0)
            { Count = 0; Moves = Array.Empty<int>(); return; }

            using BinaryReader br = new BinaryReader(new MemoryStream(data));
            FormTableIndex = br.ReadUInt16();
            Count = br.ReadUInt16();
            Moves = new int[Count];
            for (int i = 0; i < Count; i++)
                Moves[i] = br.ReadUInt16();
        }

        public override byte[] Write()
        {
            Count = Moves.Length;
            using MemoryStream ms = new MemoryStream();
            using BinaryWriter bw = new BinaryWriter(ms);
            bw.Write((ushort)FormTableIndex);
            bw.Write((ushort)Count);
            for (int i = 0; i < Count; i++)
                bw.Write((ushort)Moves[i]);

            return ms.ToArray();
        }
    }
}