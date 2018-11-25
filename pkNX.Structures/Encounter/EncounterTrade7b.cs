using System;

namespace pkNX.Structures
{
    public class EncounterTrade7b : EncounterTrade
    {
        public const int SIZE = 0x58;
        public EncounterTrade7b(byte[] data) => Data = data ?? new byte[SIZE];

        public ulong Hash1 => BitConverter.ToUInt64(Data, 0x00);
        public ulong Hash2 => BitConverter.ToUInt64(Data, 0x30);
        public ulong Hash3 => BitConverter.ToUInt64(Data, 0x50);

        public override int Species
        {
            get => BitConverter.ToUInt16(Data, 0x08);
            set => BitConverter.GetBytes((ushort)value).CopyTo(Data, 0x08);
        }

        public override int Form { get => Data[0x0A]; set => Data[0x0A] = (byte)value; }
        public override int Level { get => Data[0x0C]; set => Data[0x0C] = (byte)value; } // minimum level?

        public override Shiny Shiny // 0 = 0x3..., 1 = 0x2, 2 = 0x1
        {
            get => (Shiny)(Data[0x0D] & 3);
            set => Data[0x0D] = (byte)((Data[0x0D] & ~3) | ((byte)value & 3));
        }

        public override FixedGender Gender // 0 = Random, 1 = Male, 2 = Female, 3 = Panic
        {
            get => (FixedGender)(Data[0x0D] & 3);
            set => Data[0x0D] = (byte)((Data[0x0D] & ~3) | ((byte)value & 3));
        }

        private int GetIV(int index)
        {
            var val = BitConverter.ToUInt16(Data, 0xE + (2 * index));
            return val == 0x8000 ? -1 : val;
        }

        private void SetIV(int index, int value)
        {
            if ((uint) value > 31)
                value = 0x8000;
            BitConverter.GetBytes((ushort)value).CopyTo(Data, 0xE + (2 * index));
        }

        public override int IV_HP  { get => GetIV(0); set => SetIV(0, value); }
        public override int IV_ATK { get => GetIV(1); set => SetIV(1, value); }
        public override int IV_DEF { get => GetIV(2); set => SetIV(2, value); }
        public override int IV_SPA { get => GetIV(3); set => SetIV(3, value); }
        public override int IV_SPD { get => GetIV(4); set => SetIV(4, value); }
        public override int IV_SPE { get => GetIV(5); set => SetIV(5, value); }

        // AV randomness
        public int AV_HP  { get => Data[0x1A]; set => Data[0x1A] = (byte)value; }
        public int AV_ATK { get => Data[0x1C]; set => Data[0x1C] = (byte)value; }
        public int AV_DEF { get => Data[0x1E]; set => Data[0x1E] = (byte)value; }
        public int AV_SPA { get => Data[0x20]; set => Data[0x20] = (byte)value; }
        public int AV_SPD { get => Data[0x22]; set => Data[0x22] = (byte)value; }
        public int AV_SPE { get => Data[0x24]; set => Data[0x24] = (byte)value; }

        public int RequiredSpecies
        {
            get => BitConverter.ToUInt16(Data, 0x42);
            set => BitConverter.GetBytes((ushort)value).CopyTo(Data, 0x42);
        }

        public int RequiredForm
        {
            get => BitConverter.ToUInt16(Data, 0x44);
            set => BitConverter.GetBytes((ushort)value).CopyTo(Data, 0x44);
        }

        // what are these?
        public sbyte UV_HP  { get => (sbyte)Data[0x46]; set => Data[0x46] = (byte)value; }
        public sbyte UV_ATK { get => (sbyte)Data[0x47]; set => Data[0x47] = (byte)value; }
        public sbyte UV_DEF { get => (sbyte)Data[0x48]; set => Data[0x48] = (byte)value; }
        public sbyte UV_SPA { get => (sbyte)Data[0x49]; set => Data[0x49] = (byte)value; }
        public sbyte UV_SPD { get => (sbyte)Data[0x4A]; set => Data[0x4A] = (byte)value; }
        public sbyte UV_SPE { get => (sbyte)Data[0x4B]; set => Data[0x4B] = (byte)value; }
    }
}
