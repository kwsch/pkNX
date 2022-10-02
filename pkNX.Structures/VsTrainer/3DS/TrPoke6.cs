using System.IO;

namespace pkNX.Structures;

public class TrPoke6
{
    public byte IVs;
    public byte PID;
    public ushort Level;
    public ushort Species;
    public ushort Form;
    public int Ability;
    public int Gender;
    public int uBit;
    public ushort Item;
    public ushort[] Moves = new ushort[4];

    public TrPoke6(byte[] data, bool HasItem, bool HasMoves)
    {
        using var ms = new MemoryStream(data);
        using var br = new BinaryReader(ms);
        IVs = br.ReadByte();
        PID = br.ReadByte();
        Level = br.ReadUInt16();
        Species = br.ReadUInt16();
        Form = br.ReadUInt16();

        Ability = PID >> 4;
        Gender = PID & 3;
        uBit = (PID >> 3) & 1;

        if (HasItem)
            Item = br.ReadUInt16();

        if (!HasMoves)
            return;

        for (int i = 0; i < 4; i++)
            Moves[i] = br.ReadUInt16();
    }

    public byte[] Write(bool HasItem, bool HasMoves)
    {
        using var ms = new MemoryStream();
        using var bw = new BinaryWriter(ms);
        bw.Write(IVs);
        PID = (byte)(((Ability & 0xF) << 4) | ((uBit & 1) << 3) | (Gender & 0x7));
        bw.Write(PID);
        bw.Write(Level);
        bw.Write(Species);
        bw.Write(Form);

        if (HasItem)
            bw.Write(Item);
        if (!HasMoves)
            return ms.ToArray();

        foreach (ushort Move in Moves)
            bw.Write(Move);

        return ms.ToArray();
    }
}
