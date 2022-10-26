using System;

namespace pkNX.Structures;

public sealed class EncounterStatic7b : EncounterStatic
{
    public const int SIZE = 0x40;
    public EncounterStatic7b() : this(new byte[SIZE]) { }
    public EncounterStatic7b(byte[] data) : base(data) { }

    public ulong Hash => BitConverter.ToUInt64(Data, 0);

    public override Species Species
    {
        get => (Species)BitConverter.ToUInt16(Data, 0x08);
        set => BitConverter.GetBytes((ushort)value).CopyTo(Data, 0x08);
    }

    public override int Form { get => Data[0x0A]; set => Data[0x0A] = (byte)value; }
    public override int Level { get => Data[0x0B]; set => Data[0x0B] = (byte)value; }

    public override Shiny Shiny // 0 = 0x3..., 1 = 0x2, 2 = 0x1
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
    public override int Ability { get => Data[0x0F]; set => Data[0x0F] = (byte)value; }

    public uint[] Ptrs // 0x10-0x1F -- are these text line references?
    {
        get => new[]
        {
            BitConverter.ToUInt32(Data, 0x10),
            BitConverter.ToUInt32(Data, 0x14),
            BitConverter.ToUInt32(Data, 0x18),
            BitConverter.ToUInt32(Data, 0x1C),
        };
        set { }
    }

    public override int[] RelearnMoves // 0x20-0x27 -- these are actually just moves
    {
        get => new int[]
        {
            BitConverter.ToUInt16(Data, 0x20),
            BitConverter.ToUInt16(Data, 0x22),
            BitConverter.ToUInt16(Data, 0x24),
            BitConverter.ToUInt16(Data, 0x26),
        };
        set
        {
            if (value.Length != 4)
                return;
            for (int i = 0; i < 4; i++)
                BitConverter.GetBytes((ushort)value[i]).CopyTo(Data, 0x20 + (i * 2));
        }
    }

    public int V1
    {
        get => BitConverter.ToInt16(Data, 0x28);
        set => BitConverter.GetBytes((short)value).CopyTo(Data, 0x28);
    }

    public int V2
    {
        get => BitConverter.ToInt16(Data, 0x2A);
        set => BitConverter.GetBytes((short)value).CopyTo(Data, 0x2A);
    }

    // 0x2C-0x31
    public override int IV_HP  { get => (sbyte)Data[0x2C]; set => Data[0x2C] = (byte)value; }
    public override int IV_ATK { get => (sbyte)Data[0x2D]; set => Data[0x2D] = (byte)value; }
    public override int IV_DEF { get => (sbyte)Data[0x2E]; set => Data[0x2E] = (byte)value; }
    public override int IV_SPA { get => (sbyte)Data[0x2F]; set => Data[0x2F] = (byte)value; }
    public override int IV_SPD { get => (sbyte)Data[0x30]; set => Data[0x30] = (byte)value; }
    public override int IV_SPE { get => (sbyte)Data[0x31]; set => Data[0x31] = (byte)value; }

    // 0x32-0x37
    public override int EV_HP  { get => (sbyte)Data[0x32]; set => Data[0x32] = (byte)value; }
    public override int EV_ATK { get => (sbyte)Data[0x33]; set => Data[0x33] = (byte)value; }
    public override int EV_DEF { get => (sbyte)Data[0x34]; set => Data[0x34] = (byte)value; }
    public override int EV_SPA { get => (sbyte)Data[0x35]; set => Data[0x35] = (byte)value; }
    public override int EV_SPD { get => (sbyte)Data[0x36]; set => Data[0x36] = (byte)value; }
    public override int EV_SPE { get => (sbyte)Data[0x37]; set => Data[0x37] = (byte)value; }

    // Stat Boost levels/flags
    public int Boost_ATK { get => (sbyte)Data[0x38]; set => Data[0x38] = (byte)value; }
    public int Boost_DEF { get => (sbyte)Data[0x39]; set => Data[0x39] = (byte)value; }
    public int Boost_SPA { get => (sbyte)Data[0x3A]; set => Data[0x3A] = (byte)value; }
    public int Boost_SPD { get => (sbyte)Data[0x3B]; set => Data[0x3B] = (byte)value; }
    public int Boost_SPE { get => (sbyte)Data[0x3C]; set => Data[0x3C] = (byte)value; }

    public byte UnknownFlag3D { get => Data[0x3D]; set => Data[0x3D] = value; }
    public byte UnknownFlag3E { get => Data[0x3E]; set => Data[0x3E] = value; }
    public byte UnknownFlag3F { get => Data[0x3F]; set => Data[0x3F] = value; }

    public string Dump()
    {
        return $"new EncounterStatic {{ Species = {Species:000}, Level = {Level:00}, Location = -1, Ability = {Ability:0}, Shiny = Shiny.{Shiny} }},";
    }
}
