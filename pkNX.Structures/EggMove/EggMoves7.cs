using System;

namespace pkNX.Structures;

public sealed class EggMoves7 : EggMoves
{
    private static readonly EggMoves7 None = new(Array.Empty<int>());
    public readonly int FormTableIndex;

    private EggMoves7(int[] moves, int formIndex = 0) : base(moves) => FormTableIndex = formIndex;

    private static EggMoves7 Get(byte[] data)
    {
        if (data.Length < 4 || data.Length % 2 != 0)
            return None;

        int formIndex = BitConverter.ToInt16(data, 0);
        int count = BitConverter.ToInt16(data, 2);
        var moves = new int[count];
        for (int i = 0; i < moves.Length; i++)
            moves[i] = BitConverter.ToInt16(data, 4 + (i * 2));
        return new EggMoves7(moves, formIndex);
    }

    public static EggMoves7[] GetArray(byte[][] entries)
    {
        EggMoves7[] data = new EggMoves7[entries.Length];
        for (int i = 0; i < data.Length; i++)
            data[i] = Get(entries[i]);
        return data;
    }

    private byte[] Set()
    {
        var data = new byte[2 + 2 + (2 *Moves.Length)];
        BitConverter.GetBytes((ushort)FormTableIndex).CopyTo(data, 0);
        BitConverter.GetBytes((ushort)Moves.Length).CopyTo(data, 2);
        for (int i = 0; i < Moves.Length; i++)
            BitConverter.GetBytes((ushort)Moves[i]).CopyTo(data, 4 + (2*i));
        return data;
    }

    public static byte[][] SetArray(EggMoves7[] entries)
    {
        byte[][] data = new byte[entries.Length][];
        for (int i = 0; i < data.Length; i++)
            data[i] = entries[i].Set();
        return data;
    }
}
