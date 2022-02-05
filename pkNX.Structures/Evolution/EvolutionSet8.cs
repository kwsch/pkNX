using System;
using System.IO;

namespace pkNX.Structures
{
    public class EvolutionSet8 : EvolutionSet
    {
        private const int ENTRY_SIZE = 8;
        private const int ENTRY_COUNT = 9;
        public const int SIZE = ENTRY_COUNT * ENTRY_SIZE;

        public EvolutionSet8(byte[] data)
        {
            if (data.Length != SIZE)
                return;
            PossibleEvolutions = data.GetArray(GetEvo, ENTRY_SIZE);
        }

        private static EvolutionMethod GetEvo(byte[] data, int offset) => new()
        {
            Method = (EvolutionType)BitConverter.ToUInt16(data, offset + 0),
            Argument = BitConverter.ToUInt16(data, offset + 2),
            Species = BitConverter.ToUInt16(data, offset + 4),
            Form = (sbyte)data[offset + 6],
            Level = data[offset + 7],
        };

        public override byte[] Write()
        {
            using MemoryStream ms = new();
            using BinaryWriter bw = new(ms);
            foreach (EvolutionMethod evo in PossibleEvolutions)
            {
                bw.Write((ushort)evo.Method);
                bw.Write((ushort)evo.Argument);
                bw.Write((ushort)evo.Species);
                bw.Write((sbyte)evo.Form);
                bw.Write((byte)evo.Level);
            }
            return ms.ToArray();
        }
    }
}