using System;

namespace pkNX.Structures;

public sealed class TrData6XY : TrData6
{
    public override int SIZE => 0x14;
    public TrData6XY(byte[] trData = null) : base(trData) { }

    protected override int Format { get => Data[0x00]; set => Data[0x00] = (byte) value; }
    public override int Class { get => Data[0x01]; set => Data[0x01] = (byte)value; }
    public override BattleMode Mode { get => (BattleMode)Data[0x02]; set => Data[0x02] = (byte)value; }
    public override int NumPokemon { get => Data[0x03]; set => Data[0x03] = (byte)value; }
    public override int Item1 { get => BitConverter.ToUInt16(Data, 0x04); set => BitConverter.GetBytes((ushort)value).CopyTo(Data, 0x04); }
    public override int Item2 { get => BitConverter.ToUInt16(Data, 0x06); set => BitConverter.GetBytes((ushort)value).CopyTo(Data, 0x06); }
    public override int Item3 { get => BitConverter.ToUInt16(Data, 0x08); set => BitConverter.GetBytes((ushort)value).CopyTo(Data, 0x08); }
    public override int Item4 { get => BitConverter.ToUInt16(Data, 0x0A); set => BitConverter.GetBytes((ushort)value).CopyTo(Data, 0x0A); }
    public override uint AI { get => BitConverter.ToUInt32(Data, 0x0C); set => BitConverter.GetBytes(value).CopyTo(Data, 0x0C); }
    public override bool Heal { get => Data[0x10] == 1; set => Data[0x10] = value ? (byte)1 : (byte)0; }
    public override int Money { get => Data[0x11]; set => Data[0x11] = (byte)value; }
    public override int Gift { get => BitConverter.ToUInt16(Data, 0x12); set => BitConverter.GetBytes((ushort)value).CopyTo(Data, 0x12); }
}
