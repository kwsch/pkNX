using System.IO;

namespace pkNX.Containers;

public class AHTBEntry(ulong hash, ushort namelen, string name)
{
    public ulong Hash = hash;
    public ushort NameLength = namelen;
    public string Name = name;

    public AHTBEntry(BinaryReader br) : this(br.ReadUInt64(), br.ReadUInt16(), br.ReadStringBytesUntil(0))
    {
        //Debug.Assert(FnvHash.HashFnv1a_64(Name) == Hash);
        //Debug.Assert(Name.Length + 1 == NameLength); // Always null terminated
    }

    public void Write(BinaryWriter bw)
    {
        bw.Write((ulong)Hash);
        bw.Write((ushort)(Name.Length + 1));
        bw.Write(Name);
        bw.Write((byte)0); // \0 terminator
    }

    public override string ToString() => $"0x{Hash:X16}|{Name}";
}
