using System.Collections.Generic;

namespace pkNX.Structures
{
    /// <summary>
    /// Represents all possible Mega Evolutions for all <see cref="PersonalInfo"/> entries in the <see cref="PersonalTable"/>.
    /// </summary>
    public sealed class MegaEvolutionTable
    {
        private readonly MegaEvolutionSet[][] Table;

        public MegaEvolutionTable(IList<byte[]> data)
        {
            Table = new MegaEvolutionSet[data.Count][];
            for (int i = 0; i < data.Count; i++)
            {
                int count = data[i].Length / MegaEvolutionSet.SIZE;
                Table[i] = new MegaEvolutionSet[count];
                for (int j = 0; j < count; j++)
                    Table[i][j] = new MegaEvolutionSet(data[i], j);
            }
        }

        private static byte[] Write(IList<MegaEvolutionSet> entry)
        {
            int count = entry.Count;
            byte[] data = new byte[MegaEvolutionSet.SIZE];
            for (int i = 0; i < count; i++)
                entry[i].Write(data, i);
            return data;
        }

        public MegaEvolutionSet[] this[int index]
        {
            get => Table[index];
            set => Table[index] = value;
        }

        /// <summary>
        /// Removes any restriction for Mega Evolution
        /// </summary>
        /// <remarks>
        /// <see cref="GameVersion.GG"/> uses <see cref="MegaEvolutionMethod.NoRequirement"/>; not supported in prior games and maybe not future games.
        /// This might not be useful in the long run.
        /// </remarks>
        public void RemoveRestrictions()
        {
            foreach (var t in Table)
            {
                foreach (var s in t)
                    s.RemoveRestrictions();
            }
        }

        /// <summary>
        /// Writes the entire <see cref="MegaEvolutionTable"/> to its component files.
        /// </summary>
        /// <returns>Pack-ready array of files.</returns>
        public byte[][] Write()
        {
            byte[][] data = new byte[Table.Length][];
            for (int i = 0; i < data.Length; i++)
                data[i] = Write(Table[i]);
            return data;
        }
    }
}
