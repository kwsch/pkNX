using System;

namespace pkNX.Structures
{
    public class EncounterTrade7b : EncounterTrade
    {
        public const int SIZE = 0x58;
        public EncounterTrade7b(byte[] data) => Data = data ?? new byte[SIZE];

        // game loops over all trades to find which one is being offered
        public ulong HashTradeID => BitConverter.ToUInt64(Data, 0x00);

        public override int Species
        {
            get => BitConverter.ToUInt16(Data, 0x08);
            set => BitConverter.GetBytes((ushort)value).CopyTo(Data, 0x08);
        }

        public override int Form { get => Data[0x0A]; set => Data[0x0A] = (byte)value; }
        public override int Level { get => Data[0x0C]; set => Data[0x0C] = (byte)value; } // minimum level?

        public override Shiny Shiny { get => Shiny.Random; set { } } // Ignored

        // Value randomness
        /* val:8
         * unused:7
         * randAny:1 (signed bit)
         */
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

        // Value randomness
        /* val:8
         * unused:6
         * randUpToVal:1
         * randAny:1
         */
        private int AVP_HP  { get => BitConverter.ToInt16(Data, 0x1A); set => BitConverter.GetBytes((short)value).CopyTo(Data, 0x1A); }
        private int AVP_ATK { get => BitConverter.ToInt16(Data, 0x1C); set => BitConverter.GetBytes((short)value).CopyTo(Data, 0x1C); }
        private int AVP_DEF { get => BitConverter.ToInt16(Data, 0x1E); set => BitConverter.GetBytes((short)value).CopyTo(Data, 0x1E); }
        private int AVP_SPA { get => BitConverter.ToInt16(Data, 0x20); set => BitConverter.GetBytes((short)value).CopyTo(Data, 0x20); }
        private int AVP_SPD { get => BitConverter.ToInt16(Data, 0x22); set => BitConverter.GetBytes((short)value).CopyTo(Data, 0x22); }
        private int AVP_SPE { get => BitConverter.ToInt16(Data, 0x24); set => BitConverter.GetBytes((short)value).CopyTo(Data, 0x24); }

        public int AV_HP  { get => AVP_HP  & 0xFF; set => AVP_HP  = (AVP_HP  & 0xFF00) | (byte)value; }
        public int AV_ATK { get => AVP_ATK & 0xFF; set => AVP_ATK = (AVP_ATK & 0xFF00) | (byte)value; }
        public int AV_DEF { get => AVP_DEF & 0xFF; set => AVP_DEF = (AVP_DEF & 0xFF00) | (byte)value; }
        public int AV_SPA { get => AVP_SPA & 0xFF; set => AVP_SPA = (AVP_SPA & 0xFF00) | (byte)value; }
        public int AV_SPD { get => AVP_SPD & 0xFF; set => AVP_SPD = (AVP_SPD & 0xFF00) | (byte)value; }
        public int AV_SPE { get => AVP_SPE & 0xFF; set => AVP_SPE = (AVP_SPE & 0xFF00) | (byte)value; }

        public bool AV_HPRand   { get => AVP_HP  == -32768; set => AVP_HP  = value ? 0x8000 : AVP_HP  & 0x7FFF; }
        public bool AV_ATKRand  { get => AVP_ATK == -32768; set => AVP_ATK = value ? 0x8000 : AVP_ATK & 0x7FFF; }
        public bool AV_DEFRand  { get => AVP_DEF == -32768; set => AVP_DEF = value ? 0x8000 : AVP_DEF & 0x7FFF; }
        public bool AV_SPARand  { get => AVP_SPA == -32768; set => AVP_SPA = value ? 0x8000 : AVP_SPA & 0x7FFF; }
        public bool AV_SPDRand  { get => AVP_SPD == -32768; set => AVP_SPD = value ? 0x8000 : AVP_SPD & 0x7FFF; }
        public bool AV_SPERand  { get => AVP_SPE == -32768; set => AVP_SPE = value ? 0x8000 : AVP_SPE & 0x7FFF; }

        public bool AV_HPRandUpTo  { get => (AVP_HP  & 0x4000) != 0; set => AVP_HP  = value ? 0x4000 | (AVP_HP  & 0xFF) : AVP_HP  & 0xBFFF; }
        public bool AV_ATKRandUpTo { get => (AVP_ATK & 0x4000) != 0; set => AVP_ATK = value ? 0x4000 | (AVP_ATK & 0xFF) : AVP_ATK & 0xBFFF; }
        public bool AV_DEFRandUpTo { get => (AVP_DEF & 0x4000) != 0; set => AVP_DEF = value ? 0x4000 | (AVP_DEF & 0xFF) : AVP_DEF & 0xBFFF; }
        public bool AV_SPARandUpTo { get => (AVP_SPA & 0x4000) != 0; set => AVP_SPA = value ? 0x4000 | (AVP_SPA & 0xFF) : AVP_SPA & 0xBFFF; }
        public bool AV_SPDRandUpTo { get => (AVP_SPD & 0x4000) != 0; set => AVP_SPD = value ? 0x4000 | (AVP_SPD & 0xFF) : AVP_SPD & 0xBFFF; }
        public bool AV_SPERandUpTo { get => (AVP_SPE & 0x4000) != 0; set => AVP_SPE = value ? 0x4000 | (AVP_SPE & 0xFF) : AVP_SPE & 0xBFFF; }

        public override FixedGender Gender
        {
            get
            {
                var val = BitConverter.ToInt16(Data, 0x26);
                if (val < 0)
                    return FixedGender.Random;
                return (FixedGender)(val + 1);
            }
            set
            {
                if (value == FixedGender.Random)
                    value = unchecked((FixedGender) 0x8000);
                else
                    value--;
                BitConverter.GetBytes((short) value).CopyTo(Data, 0x26);
            }
        }

        public override Nature Nature
        {
            get
            {
                var val = BitConverter.ToInt16(Data, 0x28);
                if (val < 0)
                    return Nature.Random;
                return (Nature) val;
            }
            set
            {
                if (value == Nature.Random)
                    value = unchecked((Nature)0x8000);
                BitConverter.GetBytes((short) value).CopyTo(Data, 0x28);
            }
        }

        public int OT_Gender { get => BitConverter.ToInt16(Data, 0x2A); set => BitConverter.GetBytes((short)value).CopyTo(Data, 0x2A); }
        public uint TrainerID { get => BitConverter.ToUInt32(Data, 0x2C); set => BitConverter.GetBytes(value).CopyTo(Data, 0x2C); }

        public ulong HashTradeStringOTName => BitConverter.ToUInt64(Data, 0x30);

        // 0x38-0x40 are languageIDs to figure out which language to use (do indexOf current save language -> language message file)

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

        // what are these? not referenced
        public sbyte UV_HP  { get => (sbyte)Data[0x46]; set => Data[0x46] = (byte)value; }
        public sbyte UV_ATK { get => (sbyte)Data[0x47]; set => Data[0x47] = (byte)value; }
        public sbyte UV_DEF { get => (sbyte)Data[0x48]; set => Data[0x48] = (byte)value; }
        public sbyte UV_SPA { get => (sbyte)Data[0x49]; set => Data[0x49] = (byte)value; }
        public sbyte UV_SPD { get => (sbyte)Data[0x4A]; set => Data[0x4A] = (byte)value; }
        public sbyte UV_SPE { get => (sbyte)Data[0x4B]; set => Data[0x4B] = (byte)value; }

        public ulong HashTradeStringNickname => BitConverter.ToUInt64(Data, 0x50);

        public int Ball => 4;
    }

    public enum OptionalTradeValue : short
    {
        Random = unchecked((short)0x8000),
        RandRange = 0x4000,
    }
}
