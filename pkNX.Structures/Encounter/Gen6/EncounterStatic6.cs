using System;

namespace pkNX.Structures.Encounter
{
    public sealed class EncounterStatic6 : EncounterStatic
    {
        private const int SIZE = 0xC;
        public EncounterStatic6(byte[] data) => Data = data ?? new byte[SIZE];

        public override int Species { get => BitConverter.ToUInt16(Data, 0x0); set => BitConverter.GetBytes((ushort)value).CopyTo(Data, 0x0); }
        public override int Form { get => Data[0x2]; set => Data[0x2] = (byte)value; }
        public override int Level { get => Data[0x3]; set => Data[0x3] = (byte)value; }

        public override int HeldItem
        {
            get => Math.Max(0, (int)BitConverter.ToInt16(Data, 0x4));
            set => BitConverter.GetBytes((short)(value <= 0 ? -1 : value)).CopyTo(Data, 0x4);
        }

        public override Shiny Shiny
        {
            get => (Shiny)(Data[0x6] & 3);
            set => Data[0x6] = (byte)((Data[0x6] & ~3) | ((byte)value & 3));
        }

        public override FixedGender Gender
        {
            get => (FixedGender)((Data[0x6] & 0x0C) >> 2);
            set => Data[0x6] = (byte)((Data[0x6] & ~0xC) | (((byte)value & 3) << 2));
        }

        public override int Ability
        {
            get => (Data[0x6] & 0x70) >> 4;
            set => Data[0x6] = (byte)((Data[0x6] & ~0x70) | ((value & 7) << 4));
        }

        public override bool IV3
        {
            get => (Data[0x7] & 1) >> 0 == 1;
            set => Data[0x7] = (byte)((Data[0x7] & ~1) | (value ? 1 : 0));
        }

        public bool IV3_1
        {
            get => (Data[0x7] & 2) >> 1 == 1;
            set => Data[0x7] = (byte)((Data[0x7] & ~2) | (value ? 2 : 0));
        }
    }
}
