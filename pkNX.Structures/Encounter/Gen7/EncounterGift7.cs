using System;

namespace pkNX.Structures
{
    public class EncounterGift7 : EncounterGift
    {
        public const int SIZE = 0x14;

        public EncounterGift7(byte[] data) => Data = data;

        public override int Species { get => BitConverter.ToUInt16(Data, 0x0); set => BitConverter.GetBytes((ushort)value).CopyTo(Data, 0x0); }
        public override int Form { get => Data[0x2]; set => Data[0x2] = (byte)value; }
        public override int Level { get => Data[0x3]; set => Data[0x3] = (byte)value; }

        public override Shiny Shiny { get => (Shiny)Data[0x4]; set => Data[0x4] = (byte)value; }
        public override FixedGender Gender { get => (FixedGender)Data[0x5]; set => Data[0x5] = (byte)value; }
        public override int Ability { get => (sbyte)Data[0x6]; set => Data[0x6] = (byte)value; }
        public override Nature Nature { get => (Nature)Data[0x7]; set => Data[0x7] = (byte)value; }

        public override int HeldItem { get => BitConverter.ToUInt16(Data, 0x8); set => BitConverter.GetBytes((ushort)value).CopyTo(Data, 0x8); }

        public bool IsEgg { get => Data[0xA] == 1; set => Data[0xA] = (byte)(value ? 1 : 0); }

        public int SpecialMove { get => BitConverter.ToUInt16(Data, 0xC); set => BitConverter.GetBytes((ushort)value).CopyTo(Data, 0xC); }

        public override bool IV3 => (sbyte)Data[0xE] < 0 && (sbyte)Data[0xE] + 1 == -3;
        public override int IV_HP  { get; set; } = -1;
        public override int IV_ATK { get; set; } = -1;
        public override int IV_DEF { get; set; } = -1;
        public override int IV_SPE { get; set; } = -1;
        public override int IV_SPA { get; set; } = -1;
        public override int IV_SPD { get; set; } = -1;
    }
}