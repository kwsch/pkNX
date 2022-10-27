using System;
using System.Collections.Generic;
using System.IO;
using static System.Buffers.Binary.BinaryPrimitives;

namespace pkNX.Structures;

public class EvolutionSet7 : EvolutionSet
{
    private const int ENTRY_SIZE = 8;
    public const int MAX_ENTRY_COUNT = 8;
    public const int SIZE = MAX_ENTRY_COUNT * ENTRY_SIZE;

    public EvolutionSet7(ReadOnlySpan<byte> data)
    {
        if (data.Length != SIZE)
            return;
        PossibleEvolutions = data.GetArray(ReadEvolution, ENTRY_SIZE);
    }

    public override byte[] Write()
    {
        using var ms = new MemoryStream();
        using var bw = new BinaryWriter(ms);
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

    private static EvolutionMethod ReadEvolution(ReadOnlySpan<byte> entry)
    {
        return new()
        {
            Method = (EvolutionType)entry[0],
            Argument = ReadUInt16LittleEndian(entry[2..]),
            Species = ReadUInt16LittleEndian(entry[4..]),
            Form = SByteToByte((sbyte)entry[6]),
            Level = entry[7],
        };
    }

    public static IReadOnlyList<EvolutionMethod[]> GetArray(BinLinkerAccessor data)
    {
        var evos = new EvolutionMethod[data.Length][];
        for (int i = 0; i < evos.Length; i++)
            evos[i] = data[i].GetArray(ReadEvolution, ENTRY_SIZE);
        return evos;
    }

    /// <summary>
    /// For evo set 7 sbyte is used for form, -1 means no forms are present.
    /// The remaining code expects to work with 0 for all base forms.
    /// This clamps the sbyte range to [0-128), removing the negative range.
    /// </summary>
    private static byte SByteToByte(sbyte b)
    {
        return (byte)Math.Max(b, (sbyte)0);
    }
}
