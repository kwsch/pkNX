using System;
using static System.Buffers.Binary.BinaryPrimitives;

namespace pkNX.Structures;

public sealed class TrainerData8(Memory<byte> data) : TrainerData(data)
{
    private const int Size = 0x14;
    public override int SIZE => Size;
    public TrainerData8() : this(new byte[Size]) { }

    public override int Class
    {
        get => ReadUInt16LittleEndian(Data[0x00..]);
        set => WriteUInt16LittleEndian(Data[0x00..], (ushort)value);
    }

    public override BattleMode Mode { get => (BattleMode)Data[2]; set => Data[2] = (byte)value; } // Not sure
    public override int NumPokemon { get => Data[3]; set => Data[3] = (byte)(value % 7); }
    public override int Item1 { get => ReadUInt16LittleEndian(Data[0x04..]); set => WriteUInt16LittleEndian(Data[0x04..], (ushort)value); }
    public override int Item2 { get => ReadUInt16LittleEndian(Data[0x06..]); set => WriteUInt16LittleEndian(Data[0x06..], (ushort)value); }
    public override int Item3 { get => ReadUInt16LittleEndian(Data[0x08..]); set => WriteUInt16LittleEndian(Data[0x08..], (ushort)value); }
    public override int Item4 { get => ReadUInt16LittleEndian(Data[0x0A..]); set => WriteUInt16LittleEndian(Data[0x0A..], (ushort)value); }

    public override uint AI { get => ReadUInt32LittleEndian(Data[0x0C..]); set => WriteUInt32LittleEndian(Data[0x0C..], value); }
    public override bool Heal { get => Data[0x10] == 1; set => Data[0x10] = value ? (byte)1 : (byte)0; } // unused?
    public override int Money { get => Data[0x11]; set => Data[0x11] = (byte)value; }

    public override int Gift { get => ReadUInt16LittleEndian(Data[0x12..]); set => WriteUInt16LittleEndian(Data[0x12..], (ushort)value); } // unused?
}
