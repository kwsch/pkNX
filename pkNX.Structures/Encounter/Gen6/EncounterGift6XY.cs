using System;

namespace pkNX.Structures
{
    public class EncounterGift6XY : EncounterGift
    {
        public const int SIZE = 0x18;
        public EncounterGift6XY(byte[] data = null) : base(data ?? new byte[SIZE]) { }

        public override Species Species { get => (Species)BitConverter.ToUInt16(Data, 0x00); set => BitConverter.GetBytes((ushort)value).CopyTo(Data, 0x00); }
        public int _02 { get => BitConverter.ToUInt16(Data, 0x02); set => BitConverter.GetBytes((ushort)value).CopyTo(Data, 0x02); }
        public override int Form { get => Data[0x04]; set => Data[0x04] = (byte) value; }
        public override int Level { get => Data[0x05]; set => Data[0x05] = (byte)value; }
        public override int Ability { get => (sbyte)Data[0x06]; set => Data[0x06] = (byte)value; }
        public override Nature Nature { get => (Nature)Data[0x07]; set => Data[0x07] = (byte)value; }
        public override Shiny Shiny { get => (Shiny)Data[0x08]; set => Data[0x08] = (byte)value; }

        // padding
        public int _09 { get => Data[0x09]; set => Data[0x09] = (byte)value; }
        public int _0A { get => Data[0x0A]; set => Data[0x0A] = (byte)value; }
        public int _0B { get => Data[0x0B]; set => Data[0x0B] = (byte)value; }

        public override int HeldItem { get => BitConverter.ToUInt16(Data, 0x0C); set => BitConverter.GetBytes((ushort)value).CopyTo(Data, 0x0C); }
        public override FixedGender Gender { get => (FixedGender)Data[0x10]; set => Data[0x10] = (byte)value; }

        public override int IV_HP  { get => (sbyte)Data[0x11]; set => Data[0x11] = (byte)value; }
        public override int IV_ATK { get => (sbyte)Data[0x12]; set => Data[0x12] = (byte)value; }
        public override int IV_DEF { get => (sbyte)Data[0x13]; set => Data[0x13] = (byte)value; }
        public override int IV_SPA { get => (sbyte)Data[0x14]; set => Data[0x14] = (byte)value; }
        public override int IV_SPD { get => (sbyte)Data[0x15]; set => Data[0x15] = (byte)value; }
        public override int IV_SPE { get => (sbyte)Data[0x16]; set => Data[0x16] = (byte)value; }

        // padding
        public int Unk17 { get => (sbyte)Data[0x17]; set => Data[0x17] = (byte)value; }
    }
}