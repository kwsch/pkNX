using System;
using System.Collections.Generic;
using System.IO;

namespace pkNX.Structures
{
    /// <summary>
    /// Generation 6 Evolution Branch Entries
    /// </summary>
    public class EvolutionSet6 : EvolutionSet
    {
        private const int ENTRY_SIZE = 6;
        private const int ENTRY_COUNT = 8;
        public const int SIZE = ENTRY_COUNT * ENTRY_SIZE;
        private static readonly HashSet<int> argEvos = new() { 6, 8, 16, 17, 18, 19, 20, 21, 22, 29, 30, 32, 33, 34 };

        public EvolutionSet6(byte[] data)
        {
            if (data.Length != SIZE)
                return;
            PossibleEvolutions = data.GetArray(GetEvo, SIZE);
        }

        private static EvolutionMethod GetEvo(byte[] data, int offset)
        {
            var evo = new EvolutionMethod
            {
                Method = (EvolutionType)BitConverter.ToUInt16(data, offset + 0),
                Argument = BitConverter.ToUInt16(data, offset + 2),
                Species = BitConverter.ToUInt16(data, offset + 4),
                // Copy
                Level = BitConverter.ToUInt16(data, offset + 2),
            };

            // Argument is used by both Level argument and Item/Move/etc. Clear if appropriate.
            if (argEvos.Contains((int)evo.Method))
                evo.Level = 0;
            return evo;
        }

        public override byte[] Write()
        {
            using MemoryStream ms = new MemoryStream();
            using BinaryWriter bw = new BinaryWriter(ms);
            foreach (EvolutionMethod evo in PossibleEvolutions)
            {
                bw.Write((ushort)evo.Method);
                bw.Write((ushort)evo.Argument);
                bw.Write((ushort)evo.Species);
            }
            return ms.ToArray();
        }
    }
}