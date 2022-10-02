using System;
using System.Collections.Generic;
using System.IO;
using static System.Buffers.Binary.BinaryPrimitives;

namespace pkNX.Structures;

public class EvolutionSet8 : EvolutionSet
{
    private const int ENTRY_SIZE = 8;
    public const int MAX_ENTRY_COUNT = 9;
    public const int SIZE = MAX_ENTRY_COUNT * ENTRY_SIZE;

    public EvolutionSet8(ReadOnlySpan<byte> data)
    {
        if (data.Length != SIZE)
            return;
        PossibleEvolutions = data.GetArray(ReadEvolution, ENTRY_SIZE);
    }

    public override byte[] Write()
    {
        using MemoryStream ms = new();
        using BinaryWriter bw = new(ms);
        foreach (EvolutionMethod evo in PossibleEvolutions)
        {
            bw.Write((ushort)evo.Method);
            bw.Write((ushort)evo.Argument);
            bw.Write((ushort)evo.Species);
            bw.Write((sbyte)evo.Form);
            bw.Write((byte)evo.Level);
        }
        return ms.ToArray();
    }

    private static EvolutionMethod ReadEvolution(ReadOnlySpan<byte> entry) => new()
    {
        Method = (EvolutionType)ReadUInt16LittleEndian(entry),
        Argument = ReadUInt16LittleEndian(entry[2..]),
        Species = ReadUInt16LittleEndian(entry[4..]),
        Form = entry[6],
        Level = entry[7],
    };

    public static IReadOnlyList<EvolutionMethod[]> GetArray(BinLinkerAccessor data)
    {
        var evos = new EvolutionMethod[data.Length][];
        for (int i = 0; i < evos.Length; i++)
            evos[i] = data[i].GetArray(ReadEvolution, ENTRY_SIZE);
        return evos;
    }
}
