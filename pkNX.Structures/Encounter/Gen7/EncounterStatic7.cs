using System;

namespace pkNX.Structures;

public sealed class EncounterStatic7 : EncounterStatic
{
    public const int SIZE = 0x38;
    public EncounterStatic7() : this(new byte[SIZE]) { }
    public EncounterStatic7(byte[] data) : base(data) { }

    public override Species Species
    {
        get => (Species)BitConverter.ToUInt16(Data, 0x0);
        set => BitConverter.GetBytes((ushort)value).CopyTo(Data, 0x0);
    }

    public override int Form
    {
        get => Data[0x2];
        set => Data[0x2] = (byte)value;
    }

    public override int Level
    {
        get => Data[0x3];
        set => Data[0x3] = (byte)value;
    }

    public override int HeldItem
    {
        get => Math.Max(0, (int)BitConverter.ToInt16(Data, 0x4));
        set => BitConverter.GetBytes((short)(value <= 0 ? -1 : value)).CopyTo(Data, 0x4);
    }

    public override Shiny Shiny
    {
        get => (Shiny) (Data[0x6] & 3);
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

    public bool Unk7_0
    {
        get => (Data[0x7] & 1) >> 0 == 1;
        set => Data[0x7] = (byte)((Data[0x7] & ~1) | (value ? 1 : 0));
    }

    public bool Unk7_1
    {
        get => (Data[0x7] & 2) >> 1 == 1;
        set => Data[0x7] = (byte)((Data[0x7] & ~2) | (value ? 2 : 0));
    }

    public int Map
    {
        get => BitConverter.ToInt16(Data, 0x8) - 1;
        set => BitConverter.GetBytes((short)(value + 1)).CopyTo(Data, 0x8);
    }

    public override int[] RelearnMoves
    {
        get => new int[]
        {
            BitConverter.ToUInt16(Data, 0xC),
            BitConverter.ToUInt16(Data, 0xE),
            BitConverter.ToUInt16(Data, 0x10),
            BitConverter.ToUInt16(Data, 0x12),
        };
        set
        {
            if (value.Length != 4)
                return;
            for (int i = 0; i < 4; i++)
                BitConverter.GetBytes((ushort)value[i]).CopyTo(Data, 0xC + (i * 2));
        }
    }

    public override Nature Nature
    {
        get => (Nature)Data[0x14];
        set => Data[0x14] = (byte)value;
    }

    public override int IV_HP  { get => (sbyte)Data[0x15]; set => Data[0x15] = (byte)value; }
    public override int IV_ATK { get => (sbyte)Data[0x16]; set => Data[0x16] = (byte)value; }
    public override int IV_DEF { get => (sbyte)Data[0x17]; set => Data[0x17] = (byte)value; }
    public override int IV_SPA { get => (sbyte)Data[0x18]; set => Data[0x18] = (byte)value; }
    public override int IV_SPD { get => (sbyte)Data[0x19]; set => Data[0x19] = (byte)value; }
    public override int IV_SPE { get => (sbyte)Data[0x1A]; set => Data[0x1A] = (byte)value; }

    public override int EV_HP  { get => (sbyte)Data[0x1B]; set => Data[0x1B] = (byte)value; }
    public override int EV_ATK { get => (sbyte)Data[0x1C]; set => Data[0x1C] = (byte)value; }
    public override int EV_DEF { get => (sbyte)Data[0x1D]; set => Data[0x1D] = (byte)value; }
    public override int EV_SPA { get => (sbyte)Data[0x1E]; set => Data[0x1E] = (byte)value; }
    public override int EV_SPD { get => (sbyte)Data[0x1F]; set => Data[0x1F] = (byte)value; }
    public override int EV_SPE { get => (sbyte)Data[0x20]; set => Data[0x20] = (byte)value; }

    public int Aura
    {
        get => Data[0x25];
        set => Data[0x25] = (byte)value;
    }

    public int Allies
    {
        get => Data[0x27];
        set => Data[0x27] = (byte)value;
    }

    public int Ally1
    {
        get => Data[0x28];
        set => Data[0x28] = (byte)value;
    }

    public int Ally2
    {
        get => Data[0x2C];
        set => Data[0x2C] = (byte)value;
    }

    public override bool IV3 => (sbyte)Data[0x15] < 0 && (sbyte)Data[0x15] + 1 == -3;
}
