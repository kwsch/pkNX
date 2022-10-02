using System;

namespace pkNX.Structures;

public sealed class EggMoves6 : EggMoves
{
    private static readonly EggMoves6 None = new(Array.Empty<int>());

    private EggMoves6(int[] moves) : base(moves) { }

    private static EggMoves6 Get(byte[] data)
    {
        if (data.Length < 2 || data.Length % 2 != 0)
            return None;

        int count = BitConverter.ToInt16(data, 0);
        var moves = new int[count];
        for (int i = 0; i < moves.Length; i++)
            moves[i] = BitConverter.ToInt16(data, 2 + (i * 2));
        return new EggMoves6(moves);
    }

    public static EggMoves6[] GetArray(byte[][] entries)
    {
        EggMoves6[] data = new EggMoves6[entries.Length];
        for (int i = 0; i < data.Length; i++)
            data[i] = Get(entries[i]);
        return data;
    }
}
