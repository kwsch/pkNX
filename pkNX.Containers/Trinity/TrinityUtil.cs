using System;
using static System.Buffers.Binary.BinaryPrimitives;

namespace pkNX.Containers;

public static class TrinityUtil
{
    public static string GuessExtension(ReadOnlySpan<byte> data)
    {
        const string defaultExtension = "bin";
        if (data.Length < 8)
            return defaultExtension;
        var u32 = ReadUInt32LittleEndian(data);
        if (ReadUInt32LittleEndian(data[4..8]) == 0x53424642)
            return "bfbs";
        return u32 switch
        {
            AHTB.Magic => "ahtb",
            0x43524153 => "sarc",
            0x58544E42 => "bntx",
            0x63726173 => "sarc",
            _ => defaultExtension,
        };
    }
}
