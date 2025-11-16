using System;
using static System.Buffers.Binary.BinaryPrimitives;

namespace pkNX.Structures;

public class Move7(Memory<byte> data) : Move3DS(data)
{
    private const int Size = 0x28;
    protected override int SIZE => Size;
    public Move7() : this(new byte[Size]) { }

    public override int Type { get => Data[0x00]; set => Data[0x00] = (byte)value; }
    public override int Quality { get => Data[0x01]; set => Data[0x01] = (byte)value; }
    public override int Category { get => Data[0x02]; set => Data[0x02] = (byte)value; }
    public override int Power { get => Data[0x03]; set => Data[0x03] = (byte)value; }
    public override int Accuracy { get => Data[0x04]; set => Data[0x04] = (byte)value; }
    public override int PP { get => Data[0x05]; set => Data[0x05] = (byte)value; }
    public override int Priority { get => Data[0x06]; set => Data[0x06] = (byte)value; }
    public override int HitMin { get => Data[0x07] & 0xF; set => Data[0x07] = (byte)(HitMax << 4 | value); }
    public override int HitMax { get => Data[0x07] >> 4; set => Data[0x07] = (byte)(value << 4 | HitMin); }
    public override int Inflict { get => ReadUInt16LittleEndian(Data[0x08..]); set => WriteUInt16LittleEndian(Data[0x08..], (ushort)value); }
    public override int InflictPercent { get => Data[0x0A]; set => Data[0x0A] = (byte)value; }
    public override MoveInflictDuration InflictCount { get => (MoveInflictDuration)Data[0x0B]; set => Data[0x0B] = (byte)value; }
    public override int TurnMin { get => Data[0x0C]; set => Data[0x0C] = (byte)value; }
    public override int TurnMax { get => Data[0x0D]; set => Data[0x0D] = (byte)value; }
    public override int CritStage { get => Data[0x0E]; set => Data[0x0E] = (byte)value; }
    public override int Flinch { get => Data[0x0F]; set => Data[0x0F] = (byte)value; }
    public override int EffectSequence { get => ReadUInt16LittleEndian(Data[0x10..]); set => WriteUInt16LittleEndian(Data[0x10..], (ushort)value); }
    public override int Recoil { get => Data[0x12]; set => Data[0x12] = (byte)value; }
    public override Heal Healing { get => (Heal)Data[0x13]; set => Data[0x13] = (byte)value; }
    public override MoveTarget Target { get => (MoveTarget)Data[0x14]; set => Data[0x14] = (byte)value; }
    public override int Stat1 { get => Data[0x15]; set => Data[0x15] = (byte)value; }
    public override int Stat2 { get => Data[0x16]; set => Data[0x16] = (byte)value; }
    public override int Stat3 { get => Data[0x17]; set => Data[0x17] = (byte)value; }
    public override int Stat1Stage { get => Data[0x18]; set => Data[0x18] = (byte)value; }
    public override int Stat2Stage { get => Data[0x19]; set => Data[0x19] = (byte)value; }
    public override int Stat3Stage { get => Data[0x1A]; set => Data[0x1A] = (byte)value; }
    public override int Stat1Percent { get => Data[0x1B]; set => Data[0x1B] = (byte)value; }
    public override int Stat2Percent { get => Data[0x1C]; set => Data[0x1C] = (byte)value; }
    public override int Stat3Percent { get => Data[0x1D]; set => Data[0x1D] = (byte)value; }

    public int ZMove { get => ReadUInt16LittleEndian(Data[0x1E..]); set => WriteUInt16LittleEndian(Data[0x1E..], (ushort)value); } // 32
    public int ZPower { get => Data[0x20]; set => Data[0x20] = (byte)value; } // 33
    public int ZEffect { get => Data[0x21]; set => Data[0x21] = (byte)value; } // 34

    public RefreshType RefreshAfflictType { get => (RefreshType)Data[0x22]; set => Data[0x22] = (byte)value; } // 35
    public int RefreshAfflictPercent { get => Data[0x23]; set => Data[0x23] = (byte)value; } // 36

    public MoveFlag7 Flags { get => (MoveFlag7)ReadUInt32LittleEndian(Data[0x24..]); set => WriteUInt32LittleEndian(Data[0x24..], (uint)value); } // 37-40
}
