using System;
using System.Collections.Generic;
using static System.Buffers.Binary.BinaryPrimitives;

namespace pkNX.Structures;

public static class ItemHash8
{
    public static Dictionary<int, ulong> GetItemHashTable(ReadOnlySpan<byte> data)
    {
        int count = ReadInt32LittleEndian(data);
        var hashes = new Dictionary<int, ulong>(count);
        data = data[4..];
        for (int i = 0; i < count; i++)
        {
            var hash = ReadUInt64LittleEndian(data);
            var key = ReadInt32LittleEndian(data[8..]);
            hashes.Add(key, hash);
            data = data[0x10..];
        }
        return hashes;
    }
}
