using System;
using static System.Buffers.Binary.BinaryPrimitives;

namespace pkNX.Structures;

public class Learnset8 : Learnset
{
    private const int SIZE = 0x104; // 65 entries * 4 bytes per (move, level)

    public Learnset8(ReadOnlySpan<byte> data)
    {
        int count = 0;
        for (; count < SIZE / 4; count++)
        {
            // 3rd byte of each (u16 move, u16 level) tuple
            if (data[(count * 4) + 3] == 0xFF)
                break;
        }

        Count = count;
        if (Count == 0)
        {
            Levels = Moves = [];
            return;
        }

        Moves = new int[Count];
        Levels = new int[Count];

        var span = data[..(Count * 4)];
        for (int i = 0; i < Count; i++)
        {
            var off = i * 4;
            Moves[i] = ReadInt16LittleEndian(span[off..]);
            Levels[i] = ReadInt16LittleEndian(span[(off + 2)..]);
        }
    }

    public override byte[] Write()
    {
        Count = Moves.Length;
        var buffer = new byte[SIZE];
        var span = buffer.AsSpan();
        int off = 0;
        for (int i = 0; i < Count; i++)
        {
            WriteInt16LittleEndian(span[off..], (short)Moves[i]);
            WriteInt16LittleEndian(span[(off + 2)..], (short)Levels[i]);
            off += 4;
        }
        // Fill remaining space with sentinel pairs (0xFFFF, 0xFFFF)
        while (off < SIZE)
        {
            WriteUInt32LittleEndian(span[off..], 0xFFFFFFFF);
            off += 4;
        }
        return buffer;
    }

    public byte[] WriteAsLearn6()
    {
        // For Learn6: write pairs then a single sentinel pair (0xFFFF, 0xFFFF)
        int len = (Moves.Length * 4) + 4; // + sentinel
        var buffer = new byte[len];
        var span = buffer.AsSpan();
        int off = 0;
        for (int i = 0; i < Moves.Length; i++)
        {
            WriteUInt16LittleEndian(span[off..], (ushort)Moves[i]);
            WriteUInt16LittleEndian(span[(off + 2)..], (ushort)Levels[i]);
            off += 4;
        }
        WriteUInt32LittleEndian(span[off..], 0xFFFFFFFF);
        return buffer;
    }
}
