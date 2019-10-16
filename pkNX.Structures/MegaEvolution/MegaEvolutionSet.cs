using System;
using System.Diagnostics;
using System.Linq;

namespace pkNX.Structures
{
    public class MegaEvolutionSet
    {
        private readonly byte[] Data;
        private const int OFS_FORM = 0;
        private const int OFS_METHOD = 2;
        private const int OFS_ARGUMENT = 4;
        public const int SIZE = 8;

        public MegaEvolutionSet(byte[] data, int index)
        {
            Debug.Assert(data.Length % SIZE == 0);
            Data = new byte[SIZE];
            Array.Copy(data, index*SIZE, Data, 0, SIZE);
        }

        public static MegaEvolutionSet[] ReadArray(byte[] data)
        {
            var count = data.Length / SIZE;
            var result = new MegaEvolutionSet[count];
            for (int i = 0; i < count; i++)
                result[i] = new MegaEvolutionSet(data, i);
            return result;
        }

        public static byte[] WriteArray(MegaEvolutionSet[] data)
        {
            return data.SelectMany(z => z.Write()).ToArray();
        }

        public int ToForm
        {
            get => BitConverter.ToUInt16(Data, OFS_FORM);
            set => BitConverter.GetBytes((ushort) value).CopyTo(Data, OFS_FORM);
        }

        public int Method
        {
            get => BitConverter.ToUInt16(Data, OFS_METHOD);
            set => BitConverter.GetBytes((ushort)value).CopyTo(Data, OFS_METHOD);
        }

        public int Argument
        {
            get => BitConverter.ToUInt16(Data, OFS_ARGUMENT);
            set => BitConverter.GetBytes((ushort)value).CopyTo(Data, OFS_ARGUMENT);
        }

        public void Clear()
        {
            for (int i = 0; i < Data.Length; i++)
                Data[i] = 0;
        }

        public byte[] Write() => (byte[])Data.Clone();

        public void Write(byte[] data, int index) => Data.CopyTo(data, index * SIZE);

        public void RemoveRestrictions()
        {
            if (Method != (int) MegaEvolutionMethod.None)
                Method = (int) MegaEvolutionMethod.NoRequirement;
        }
    }
}
