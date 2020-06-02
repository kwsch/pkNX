using System;

namespace pkNX.Structures
{
    public class EncounterGift6AO : EncounterGift
    {
        public const int SIZE = 0x24;
        public EncounterGift6AO(byte[] data = null) : base(data ?? new byte[SIZE]) { }

        public override int Species { get => BitConverter.ToUInt16(Data, 0x00); set => BitConverter.GetBytes((ushort)value).CopyTo(Data, 0x00); }
        public int Unk02 { get => BitConverter.ToUInt16(Data, 0x02); set => BitConverter.GetBytes((ushort)value).CopyTo(Data, 0x02); }
        public override int Form { get => Data[0x04]; set => Data[0x04] = (byte)value; }
        public override int Level { get => Data[0x05]; set => Data[0x05] = (byte)value; }
        public override int Ability { get => (sbyte)Data[0x06]; set => Data[0x06] = (byte)value; }
        public override Nature Nature { get => (Nature)Data[0x07]; set => Data[0x07] = (byte)value; }
        public override Shiny Shiny { get => (Shiny)Data[0x08]; set => Data[0x08] = (byte)value; }

        // padding?
        public int Unk09 { get => Data[0x09]; set => Data[0x09] = (byte)value; }
        public int Unk0A { get => Data[0x0A]; set => Data[0x0A] = (byte)value; }
        public int Unk0B { get => Data[0x0B]; set => Data[0x0B] = (byte)value; }

        public override int HeldItem { get => BitConverter.ToUInt16(Data, 0x0C); set => BitConverter.GetBytes((ushort)value).CopyTo(Data, 0x0C); }
        public override FixedGender Gender { get => (FixedGender)Data[0x10]; set => Data[0x10] = (byte)value; }

        // padding?
        public int Unk11 { get => (sbyte)Data[0x11]; set => Data[0x11] = (byte)value; }
        public short MetLocation { get => BitConverter.ToInt16(Data, 0x12); set => BitConverter.GetBytes(value).CopyTo(Data, 0x12); }
        public int Move { get => BitConverter.ToUInt16(Data, 0x14); set => BitConverter.GetBytes((ushort)value).CopyTo(Data, 0x14); }

        public override int IV_HP  { get => (sbyte)Data[0x16]; set => Data[0x16] = (byte)value; }
        public override int IV_ATK { get => (sbyte)Data[0x17]; set => Data[0x17] = (byte)value; }
        public override int IV_DEF { get => (sbyte)Data[0x18]; set => Data[0x18] = (byte)value; }
        public override int IV_SPA { get => (sbyte)Data[0x19]; set => Data[0x19] = (byte)value; }
        public override int IV_SPD { get => (sbyte)Data[0x1A]; set => Data[0x1A] = (byte)value; }
        public override int IV_SPE { get => (sbyte)Data[0x1B]; set => Data[0x1B] = (byte)value; }

        public int CNT_Cool  { get => (sbyte)Data[0x1C]; set => Data[0x1C] = (byte)value; }
        public int CNT_Beauty{ get => (sbyte)Data[0x1D]; set => Data[0x1D] = (byte)value; }
        public int CNT_Cute  { get => (sbyte)Data[0x1E]; set => Data[0x1E] = (byte)value; }
        public int CNT_Smart { get => (sbyte)Data[0x1F]; set => Data[0x1F] = (byte)value; }
        public int CNT_Tough { get => (sbyte)Data[0x20]; set => Data[0x20] = (byte)value; }
        public int CNT_Sheen { get => (sbyte)Data[0x21]; set => Data[0x21] = (byte)value; }

        public int Unk22 { get => (sbyte)Data[0x22]; set => Data[0x22] = (byte)value; }
    }
}