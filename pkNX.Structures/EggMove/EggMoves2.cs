using System;
using System.Linq;

namespace pkNX.Structures;

public sealed class EggMoves2 : EggMoves
{
    private EggMoves2(byte[] data) : base(data.Select(i => (int)i).ToArray()) { }

    public static EggMoves[] GetArray(byte[] data, int count)
    {
        int[] ptrs = new int[count + 1];
        int baseOffset = (data[1] << 8 | data[0]) - (count * 2);
        for (int i = 1; i < ptrs.Length; i++)
        {
            var ofs = (i - 1) * 2;
            ptrs[i] = (data[ofs + 1] << 8 | data[ofs]) - baseOffset;
        }

        EggMoves[] entries = new EggMoves[count + 1];
        entries[0] = new EggMoves2([]);
        for (int i = 1; i < entries.Length; i++)
            entries[i] = new EggMoves2(data.Skip(ptrs[i]).TakeWhile(b => b != 0xFF).ToArray());

        return entries;
    }
}
