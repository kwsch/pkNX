using System.IO;

namespace pkNX.Containers;

public class AHTBEntry
{
    public ulong Hash;
    public ushort NameLength;
    public string Name;

    public AHTBEntry(ulong hash, ushort namelen, string name)
    {
        Hash = hash;
        NameLength = namelen;
        Name = name;
    }

    public AHTBEntry(BinaryReader br)
    {
        Hash = br.ReadUInt64();
        NameLength = br.ReadUInt16();
        Name = br.ReadStringBytesUntil(0); // could use Length field, but they're always \0 terminated
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
