using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace pkNX.Containers;

internal class FATBEntry
{
    private readonly uint Vector;
    public readonly bool IsFolder;
    public readonly FATBSubEntry[] SubEntries;

    private FATBEntry()
    {
        var sub = SubEntries = new FATBSubEntry[32];
        for (int i = 0; i < sub.Length; i++)
            sub[i] = new FATBSubEntry();
    }

    public FATBEntry(string file) : this()
    {
        IsFolder = false;
        Vector = 1;
        SubEntries[0].Exists = true;
        SubEntries[0].File = file;
    }

    public FATBEntry(IEnumerable<string> files) : this()
    {
        IsFolder = true;
        Vector = 0;
        foreach (var f in files)
        {
            var fn = Path.GetFileNameWithoutExtension(f);
            if (!int.TryParse(fn, out var val) || val >= SubEntries.Length)
                continue;
            if (FATBSubEntry.GetFileNumber(val) != fn)
                continue;

            Vector |= (uint)(1 << val);

            SubEntries[val].Exists = true;
            SubEntries[val].File = f;
        }
    }

    public FATBEntry(BinaryReader br, int DataOffset) : this()
    {
        Vector = br.ReadUInt32();

        int ctr = 0;
        for (int b = 0; b < 32; b++)
        {
            SubEntries[b].Exists = (Vector & 1 << b) != 0;
            if (!SubEntries[b].Exists)
                continue;
            SubEntries[b].Start = br.ReadInt32();
            SubEntries[b].End = br.ReadInt32();
            SubEntries[b].Length = br.ReadInt32();
            SubEntries[b].ParentDataPosition = DataOffset;
            ctr++;
        }
        IsFolder = ctr > 1;
    }

    public void Write(BinaryWriter bw)
    {
        bw.Write(Vector);
        foreach (var s in SubEntries.Where(s => s.Exists))
        {
            bw.Write(s.Start);
            bw.Write(s.End);
            bw.Write(s.Length);
        }
    }

    public void WriteEntries(BinaryWriter bw, Stream originalStream, int padToNearest, int DataOffset, ref int max, ref int maxPad)
    {
        var dest = bw.BaseStream;

        bw.Write(Vector);
        foreach (var entry in SubEntries.Where(z => z.Exists))
        {
            entry.Write(originalStream, dest, DataOffset);

            int padding = GetPadding(entry.Length, padToNearest);
            if (max < entry.Length)
            {
                max = entry.Length;
                maxPad = entry.Length + padding;
            }

            while (padding-- > 0)
                bw.Write((byte)0xFF);
        }
    }

    private static int GetPadding(int length, int padTo)
    {
        var remain = length % padTo;
        return remain == 0 ? 0 : padTo - remain;
    }

    public void Dump(string path, int index, string format, Stream parent, int DataOffset)
    {
        var fn = index.ToString(format);
        if (!IsFolder)
        {
            var loc = Path.Combine(path, fn + ".bin");
            SubEntries[0].Dump(parent, loc, DataOffset);
            return;
        }

        path = Path.Combine(path, fn);
        Directory.CreateDirectory(path);
        for (var i = 0; i < SubEntries.Length; i++)
        {
            var entry = SubEntries[i];
            if (!entry.Exists)
                continue;
            fn = i.ToString("D2");
            var loc = Path.Combine(path, fn + ".bin");
            entry.Dump(parent, loc, DataOffset);
        }
    }
}
