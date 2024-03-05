using System;

namespace pkNX.Structures;

public sealed class TrData6AO(byte[] data) : TrData6(data)
{
    private const int Size = 0x18;
    public override int SIZE => Size;
    public TrData6AO() : this(new byte[Size]) { }

    protected override int Format { get => BitConverter.ToUInt16(Data, 0x00); set => BitConverter.GetBytes((ushort)value).CopyTo(Data, 0x00); }
    public override int Class { get => BitConverter.ToUInt16(Data, 0x02); set => BitConverter.GetBytes((ushort)value).CopyTo(Data, 0x02); }
    public ushort Unused { get => BitConverter.ToUInt16(Data, 0x04); set => BitConverter.GetBytes(value).CopyTo(Data, 0x04); }
    public override BattleMode Mode { get => (BattleMode) Data[0x06]; set => Data[0x06] = (byte) value; }
    public override int NumPokemon { get => Data[0x07]; set => Data[0x07] = (byte)value; }
    public override int Item1 { get => BitConverter.ToUInt16(Data, 0x08); set => BitConverter.GetBytes((ushort)value).CopyTo(Data, 0x08); }
    public override int Item2 { get => BitConverter.ToUInt16(Data, 0x0A); set => BitConverter.GetBytes((ushort)value).CopyTo(Data, 0x0A); }
    public override int Item3 { get => BitConverter.ToUInt16(Data, 0x0C); set => BitConverter.GetBytes((ushort)value).CopyTo(Data, 0x0C); }
    public override int Item4 { get => BitConverter.ToUInt16(Data, 0x0E); set => BitConverter.GetBytes((ushort)value).CopyTo(Data, 0x0E); }
    public override uint AI { get => BitConverter.ToUInt32(Data, 0x10); set => BitConverter.GetBytes(value).CopyTo(Data, 0x10); }
    public override bool Heal { get => Data[0x14] == 1; set => Data[0x14] = value ? (byte)1 : (byte)0; }
    public override int Money { get => Data[0x15]; set => Data[0x15] = (byte)value; }
    public override int Gift { get => BitConverter.ToUInt16(Data, 0x16); set => BitConverter.GetBytes((ushort)value).CopyTo(Data, 0x16); }
}
