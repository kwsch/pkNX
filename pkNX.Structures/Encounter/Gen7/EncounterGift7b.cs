using System;

namespace pkNX.Structures;

public class EncounterGift7b : EncounterGift
{
    public const int SIZE = 0x20;
    public EncounterGift7b(byte[] data = null) : base(data ?? new byte[SIZE]) { }

    public ulong Hash => BitConverter.ToUInt64(Data, 0);

    public override Species Species
    {
        get => (Species)BitConverter.ToUInt16(Data, 0x08);
        set => BitConverter.GetBytes((ushort)value).CopyTo(Data, 0x08);
    }

    public override int Form { get => Data[0x0A]; set => Data[0x0A] = (byte)value; }
    public override int Level { get => Data[0x0B]; set => Data[0x0B] = (byte)value; }

    public override Shiny Shiny // 0 = Random, 1 = Always, 2 = Never
    {
        get => (Shiny)(Data[0x0C] & 3);
        set => Data[0x0C] = (byte)((Data[0x0C] & ~3) | ((byte)value & 3));
    }

    public override FixedGender Gender // 0 = Random, 1 = Male, 2 = Female, 3 = Panic
    {
        get => (FixedGender)(Data[0x0D] & 3);
        set => Data[0x0D] = (byte)((Data[0x0D] & ~3) | ((byte)value & 3));
    }

    public override Nature Nature { get => (Nature)Data[0x0E]; set => Data[0x0E] = (byte)value; } // 25 = random (sets the nature rand to 1)
    public override int Ability { get => Data[0x0F]; set => Data[0x0F] = (byte)value; } // 0 = rand

    public int SpecialMove // sub_71002B7AD0 checks nonzero, pushes move
    {
        get => BitConverter.ToUInt16(Data, 0x10);
        set => BitConverter.GetBytes((ushort)value).CopyTo(Data, 0x10);
    }

    public override int IV_HP  { get => (sbyte)Data[0x12]; set => Data[0x12] = (byte)value; }
    public override int IV_ATK { get => (sbyte)Data[0x13]; set => Data[0x13] = (byte)value; }
    public override int IV_DEF { get => (sbyte)Data[0x14]; set => Data[0x14] = (byte)value; }
    public override int IV_SPA { get => (sbyte)Data[0x15]; set => Data[0x15] = (byte)value; }
    public override int IV_SPD { get => (sbyte)Data[0x16]; set => Data[0x16] = (byte)value; }
    public override int IV_SPE { get => (sbyte)Data[0x17]; set => Data[0x17] = (byte)value; }

    public int AV_HP  { get => (sbyte)Data[0x18]; set => Data[0x18] = (byte)value; }
    public int AV_ATK { get => (sbyte)Data[0x19]; set => Data[0x19] = (byte)value; }
    public int AV_DEF { get => (sbyte)Data[0x1A]; set => Data[0x1A] = (byte)value; }
    public int AV_SPA { get => (sbyte)Data[0x1B]; set => Data[0x1B] = (byte)value; }
    public int AV_SPD { get => (sbyte)Data[0x1C]; set => Data[0x1C] = (byte)value; }
    public int AV_SPE { get => (sbyte)Data[0x1D]; set => Data[0x1D] = (byte)value; }

    public override int[] RelearnMoves
    {
        get => new int[]
        {
            BitConverter.ToUInt16(Data, 0x18),
            BitConverter.ToUInt16(Data, 0x1A),
            BitConverter.ToUInt16(Data, 0x1C),
            BitConverter.ToUInt16(Data, 0x1E),
        };
        set
        {
            if (value?.Length != 4)
                return;
            for (int i = 0; i < 4; i++)
                BitConverter.GetBytes((ushort)value[i]).CopyTo(Data, 0x18 + (i * 2));
        }
    }
}
