using System;

namespace pkNX.Structures;

public static class CodePattern
{
    /// <summary>
    /// Finds a provided <see cref="pattern"/> within the supplied <see cref="array"/>.
    /// </summary>
    /// <param name="array">Array to look in</param>
    /// <param name="pattern">Pattern to look for</param>
    /// <param name="startIndex">Starting offset to look from</param>
    /// <param name="length">Amount of entries to look through</param>
    /// <returns>Index the pattern occurs at; if not found, returns -1.</returns>
    public static int IndexOfBytes(ReadOnlySpan<byte> array, ReadOnlySpan<byte> pattern, int startIndex = 0, int length = -1)
    {
        var span = array[startIndex..];
        if (length > 0 && length < span.Length)
            span = span[..length];
        return span.IndexOf(pattern) + startIndex;
    }

    /// <summary>
    /// <see cref="GameVersion.GG"/> byte pattern which precedes the TMHM list. This list is the tail end of item IDs for each TM(01->100).
    /// </summary>
    public static readonly byte[] TMHM_GG =
    [
        0xA0, 0x01, 0xA1, 0x01, 0xA2, 0x01, 0xA3, 0x01,
        0x6A, 0x02, 0x6B, 0x02, 0x6C, 0x02, 0xB2, 0x02,
        0xB3, 0x02, 0xB4, 0x02, 0xB5, 0x02, 0xB6, 0x02,
    ];
}
