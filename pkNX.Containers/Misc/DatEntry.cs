using System.IO;

namespace pkNX.Containers;

public readonly struct DatEntry
{
    public readonly ulong Hash;
    public readonly int Value;

    public DatEntry(ulong hash, int value)
    {
        Hash = hash;
        Value = value;
    }

    public DatEntry(BinaryReader br)
    {
        Hash = br.ReadUInt64();
        Value = br.ReadInt32();
        br.ReadInt32();
    }

    public void Write(BinaryWriter bw)
    {
        bw.Write(Hash);
        bw.Write(Value);
        bw.Write(0);
    }

    public string Summary => $"{Hash:X16}\t{Value}";

    public override bool Equals(object? obj) => obj is DatEntry d && Equals(d);
    public bool Equals(DatEntry d) => d.Hash == Hash;
    public override int GetHashCode() => Hash.GetHashCode();
    public static bool operator ==(DatEntry left, DatEntry right) => left.Equals(right);
    public static bool operator !=(DatEntry left, DatEntry right) => !(left == right);
}
