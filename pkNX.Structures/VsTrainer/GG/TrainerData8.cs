using System;

namespace pkNX.Structures;

public sealed class TrainerData8 : TrainerData
{
    private const int Size = 0x14;
    public override int SIZE => Size;
    public TrainerData8() : this(new byte[Size]) { }
    public TrainerData8(byte[] data) : base(data) { }

    public override int Class { get => BitConverter.ToUInt16(Data, 0x00); set => BitConverter.GetBytes((ushort)value).CopyTo(Data, 0x00); }
    public override BattleMode Mode { get => (BattleMode)Data[2]; set => Data[2] = (byte)value; } // Not sure
    public override int NumPokemon { get => Data[3]; set => Data[3] = (byte)(value % 7); }
    public override int Item1 { get => BitConverter.ToUInt16(Data, 0x04); set => BitConverter.GetBytes((ushort)value).CopyTo(Data, 0x04); }
    public override int Item2 { get => BitConverter.ToUInt16(Data, 0x06); set => BitConverter.GetBytes((ushort)value).CopyTo(Data, 0x06); }
    public override int Item3 { get => BitConverter.ToUInt16(Data, 0x08); set => BitConverter.GetBytes((ushort)value).CopyTo(Data, 0x08); }
    public override int Item4 { get => BitConverter.ToUInt16(Data, 0x0A); set => BitConverter.GetBytes((ushort)value).CopyTo(Data, 0x0A); }

    public override uint AI { get => BitConverter.ToUInt32(Data, 0x0C); set => BitConverter.GetBytes(value).CopyTo(Data, 0x0C); }
    public override bool Heal { get => Data[0x10] == 1; set => Data[0x10] = value ? (byte)1 : (byte)0; } // unused?
    public override int Money { get => Data[0x11]; set => Data[0x11] = (byte)value; }

    public override int Gift { get => BitConverter.ToUInt16(Data, 0x12); set => BitConverter.GetBytes((ushort)value).CopyTo(Data, 0x12); } // unused?
}
