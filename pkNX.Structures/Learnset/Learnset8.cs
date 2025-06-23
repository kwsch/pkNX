using System.IO;

namespace pkNX.Structures;

public class Learnset8 : Learnset
{
    private const int SIZE = 0x104;

    public Learnset8(byte[] data)
    {
        // scan for count
        int count = 0;
        for (; count < SIZE / 4; count++)
        {
            if (data[(count * 4) + 3] == 0xFF) // check 3rd byte of each u16/u16 tuple, level is never > 255
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
        using var ms = new MemoryStream(data);
        using var br = new BinaryReader(ms);
        for (int i = 0; i < Count; i++)
        {
            Moves[i] = br.ReadInt16();
            Levels[i] = br.ReadInt16();
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
        while (bw.BaseStream.Length != SIZE)
            bw.Write(-1);
        return ms.ToArray();
    }

    public byte[] WriteAsLearn6()
    {
        using var ms = new MemoryStream();
        using var br = new BinaryWriter(ms);
        for (int j = 0; j < Moves.Length; j++)
        {
            br.Write((ushort)Moves[j]);
            br.Write((ushort)Levels[j]);
        }

        br.Write(-1);
        return ms.ToArray();
    }
}
