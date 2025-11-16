using System;
using System.IO;
using static System.Buffers.Binary.BinaryPrimitives;

namespace pkNX.Structures;

public class Learnset6 : Learnset
{
    public Learnset6(Memory<byte> data) : this(data.Span)
    {
    }

    public Learnset6(Span<byte> data)
    {
        if (data.Length < 4 || data.Length % 4 != 0)
        { Count = 0; Levels = Moves = []; return; }
        Count = (data.Length / 4) - 1;
        Moves = new int[Count];
        Levels = new int[Count];

        int ofs = 0;
        for (int i = 0; i < Count; i++)
        {
            Moves[i] = ReadInt16LittleEndian(data.Slice(ofs, 2));
            Levels[i] = ReadInt16LittleEndian(data.Slice(ofs + 2, 2));
            ofs += 4;
        }
    }

    public override byte[] Write()
    {
        Count = (ushort)Moves.Length;
        using var ms = new MemoryStream();
        using var bw = new BinaryWriter(ms);
        for (int i = 0; i < Count; i++)
        {
            bw.Write((short)Moves[i]);
            bw.Write((short)Levels[i]);
        }
        bw.Write(-1);
        return ms.ToArray();
    }

    public static Learnset[] GetArray(byte[][] entries)
    {
        var data = new Learnset[entries.Length];
        for (int i = 0; i < data.Length; i++)
            data[i] = new Learnset6(entries[i].AsSpan());
        return data;
    }
}
