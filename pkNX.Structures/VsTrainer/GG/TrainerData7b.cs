using System;
using static System.Buffers.Binary.BinaryPrimitives;

namespace pkNX.Structures;

public sealed class TrainerData7b(Memory<byte> data) : TrainerData(data)
{
    private const int Size = 0x17;
    public override int SIZE => Size;
    public TrainerData7b() : this(new byte[Size]) { }

    public override int Class { get => ReadUInt16LittleEndian(Data); set => WriteUInt16LittleEndian(Data, (ushort)value); }
    public override BattleMode Mode { get => (BattleMode)Data[2]; set => Data[2] = (byte)value; } // Not sure
    public override int NumPokemon { get => Data[3]; set => Data[3] = (byte)(value % 7); }
    public override int Item1 { get => ReadUInt16LittleEndian(Data[0x04..]); set => WriteUInt16LittleEndian(Data[0x04..], (ushort)value); }
    public override int Item2 { get => ReadUInt16LittleEndian(Data[0x06..]); set => WriteUInt16LittleEndian(Data[0x06..], (ushort)value); }
    public override int Item3 { get => ReadUInt16LittleEndian(Data[0x08..]); set => WriteUInt16LittleEndian(Data[0x08..], (ushort)value); }
    public override int Item4 { get => ReadUInt16LittleEndian(Data[0x0A..]); set => WriteUInt16LittleEndian(Data[0x0A..], (ushort)value); }
    public override uint AI { get => ReadUInt32LittleEndian(Data[0x0C..]); set => WriteUInt32LittleEndian(Data[0x0C..], value); }
    public override bool Heal { get => Data[0x10] == 1; set => Data[0x10] = value ? (byte)1 : (byte)0; } // unused?
    public override int Money { get => Data[0x11]; set => Data[0x11] = (byte)value; }
    // 12 unused
    public override int Gift { get => ReadUInt16LittleEndian(Data[0x14..]); set => WriteUInt16LittleEndian(Data[0x14..], (ushort)value); }
    public int GiftQuantity { get => Data[0x16]; set => Data[0x16] = (byte)value; }
}
