using System;
using System.Collections.Generic;
using System.IO;

namespace pkNX.Structures;

/// <summary>
/// Generation 6 Evolution Branch Entries
/// </summary>
public class EvolutionSet6 : EvolutionSet
{
    private const int ENTRY_SIZE = 6;
    private const int ENTRY_COUNT = 8;
    public const int SIZE = ENTRY_COUNT * ENTRY_SIZE;
    private static readonly HashSet<int> argEvos = new() { 6, 8, 16, 17, 18, 19, 20, 21, 22, 29, 30, 32, 33, 34 };

    public EvolutionSet6(byte[] data)
    {
        if (data.Length != SIZE)
            return;
        PossibleEvolutions = data.GetArray(GetEvo, SIZE);
    }

    private static EvolutionMethod GetEvo(byte[] data, int offset)
    {
        var method = (EvolutionType)BitConverter.ToUInt16(data, offset + 0);
        var level = (byte)BitConverter.ToUInt16(data, offset + 2);

        var evo = new EvolutionMethod
        {
            Method = method,
            Argument = (argEvos.Contains((int)method) ? (byte)0 : level), // Argument is used by both Level argument and Item/Move/etc. Clear if appropriate.
            Species = BitConverter.ToUInt16(data, offset + 4),
            Level = level,
        };

        return evo;
    }

    public override byte[] Write()
    {
        using MemoryStream ms = new MemoryStream();
        using BinaryWriter bw = new BinaryWriter(ms);
        foreach (EvolutionMethod evo in PossibleEvolutions)
        {
            bw.Write((ushort)evo.Method);
            bw.Write((ushort)evo.Argument);
            bw.Write((ushort)evo.Species);
        }
        return ms.ToArray();
    }
}
