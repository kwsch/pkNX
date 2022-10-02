using System;
using System.Linq;
using pkNX.Containers;
using pkNX.Structures;

namespace pkNX.Game;

public class TMEditorGG
{
    private readonly int Offset;
    private readonly NSO NSO;
    private readonly byte[] Data;

    private const int count = 60;

    public TMEditorGG(byte[] data)
    {
        NSO = new NSO(data);
        Data = NSO.DecompressedRO;

        // tm list is stored immediately after TM item index list
        var pattern = CodePattern.TMHM_GG;
        Offset = CodePattern.IndexOfBytes(Data, pattern, 0x200_000);
        if (Valid)
            Offset += pattern.Length;
    }

    public ushort[] GetMoves()
    {
        var moves = new ushort[count];
        for (int i = 0; i < moves.Length; i++)
            moves[i] = BitConverter.ToUInt16(Data, Offset + (2 * i));
        return moves;
    }

    public void SetMoves(ushort[] finalMoves)
    {
        var result = finalMoves.SelectMany(BitConverter.GetBytes).ToArray();
        result.CopyTo(Data, Offset);
    }

    public bool Valid => Offset > 0;
    public byte[] Write() => NSO.Write();
}