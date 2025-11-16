using System;
using System.IO;
using System.Linq;
using static System.Buffers.Binary.BinaryPrimitives;

namespace pkNX.Structures;

public sealed class ZukanEvolutionTable
{
    private const int SIZE = 0x14;
    private readonly ushort[][] Table;

    public ZukanEvolutionTable(ReadOnlySpan<byte> data)
    {
        if (data.Length % SIZE != 0)
            throw new ArgumentException($"{nameof(data)} length should be a multiple of {SIZE}");

        Table = new ushort[data.Length / SIZE][];
        for (int i = 0; i < Table.Length; i++)
        {
            var evos = Table[i] = new ushort[(SIZE / 2) - 1];
            for (int j = 0; j < evos.Length; j++)
                evos[i] = ReadUInt16LittleEndian(data.Slice((i * SIZE) + (j * 2), 2));
        }
    }

    public byte[] Write()
    {
        using var ms = new MemoryStream();
        using var bw = new BinaryWriter(ms);
        foreach (var t in Table)
        {
            foreach (var e in t)
                bw.Write(e);
            bw.Write(t.Count(x => x != 0));
        }
        return ms.ToArray();
    }
}
