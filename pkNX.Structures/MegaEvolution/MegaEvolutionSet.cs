using System;
using static System.Buffers.Binary.BinaryPrimitives;

namespace pkNX.Structures;

public class MegaEvolutionSet(Memory<byte> Raw)
{
    public Span<byte> Data => Raw.Span;

    public const int SIZE = 8;

    public MegaEvolutionSet(ReadOnlySpan<byte> data) : this(data[..SIZE].ToArray().AsMemory())
    {
    }

    public static MegaEvolutionSet[] ReadArray(ReadOnlySpan<byte> data)
    {
        var count = data.Length / SIZE;
        var result = new MegaEvolutionSet[count];
        for (int i = 0; i < count; i++)
            result[i] = new MegaEvolutionSet(data[(i * SIZE)..]);
        return result;
    }

    public static byte[] WriteArray(MegaEvolutionSet[] data)
    {
        var result = new byte[SIZE * data.Length];
        var span = result.AsSpan();
        for (int i = 0; i < data.Length; i++)
            data[i].Data.CopyTo(span[(i * SIZE)..]);
        return result;
    }

    public int ToForm { get => ReadUInt16LittleEndian(Data); set => WriteUInt16LittleEndian(Data, (ushort)value); }
    public int Method {get => ReadUInt16LittleEndian(Data[2..]); set => WriteUInt16LittleEndian(Data[2..], (ushort)value); }
    public int Argument { get => ReadUInt16LittleEndian(Data[4..]); set => WriteUInt16LittleEndian(Data[4..], (ushort)value); }

    public void Clear() => Data.Clear();
    public byte[] Write() => Data.ToArray();
    public void Write(Span<byte> dest) => Data.CopyTo(dest);

    public void RemoveRestrictions()
    {
        if (Method != (int) MegaEvolutionMethod.None)
            Method = (int) MegaEvolutionMethod.NoRequirement;
    }
}
