using System;

namespace pkNX.Structures
{
    public class EncounterTrade7b : EncounterTrade
    {
        private const int SIZE = 0x58;
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

        public override int IV_HP  { get => BitConverter.ToInt16(Data, 0x0E); set => BitConverter.GetBytes((short)value).CopyTo(Data, 0x0E); }
        public override int IV_ATK { get => BitConverter.ToInt16(Data, 0x10); set => BitConverter.GetBytes((short)value).CopyTo(Data, 0x10); }
        public override int IV_DEF { get => BitConverter.ToInt16(Data, 0x12); set => BitConverter.GetBytes((short)value).CopyTo(Data, 0x12); }
        public override int IV_SPA { get => BitConverter.ToInt16(Data, 0x14); set => BitConverter.GetBytes((short)value).CopyTo(Data, 0x14); }
        public override int IV_SPD { get => BitConverter.ToInt16(Data, 0x16); set => BitConverter.GetBytes((short)value).CopyTo(Data, 0x16); }
        public override int IV_SPE { get => BitConverter.ToInt16(Data, 0x18); set => BitConverter.GetBytes((short)value).CopyTo(Data, 0x18); }

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
