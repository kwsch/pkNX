using System;
using System.IO;
using System.IO.Compression;

namespace pkNX.Containers;

public static class Zlib
{
    // Helper: Zlib Decompression
    public static byte[] Decompress(ReadOnlySpan<byte> compressed, int expectedLength)
    {
        if (expectedLength == 0)
            return [];

        // Copy span to array for MemoryStream
        using var ms = new MemoryStream(compressed.ToArray(), writable: false);
        using var zs = new ZLibStream(ms, CompressionMode.Decompress, leaveOpen: true);
        var result = new byte[expectedLength];
        int offset = 0;
        while (offset < expectedLength)
        {
            int read = zs.Read(result, offset, expectedLength - offset);
            if (read == 0)
                break; // stream ended early
            offset += read;
        }
        if (offset != expectedLength)
        {
            // If actual output shorter than expected, trim (avoid throwing to stay resilient)
            if (offset == 0)
                throw new InvalidDataException("Zlib decompression produced no data.");
            if (offset < expectedLength)
                Array.Resize(ref result, offset);
        }
        return result;
    }

    // Overload: Decompress into a caller-provided destination span, return bytes written
    public static int Decompress(ReadOnlySpan<byte> compressed, Span<byte> destination)
    {
        if (compressed.Length == 0)
            return 0;

        using var ms = new MemoryStream(compressed.ToArray(), writable: false);
        using var zs = new ZLibStream(ms, CompressionMode.Decompress, leaveOpen: true);
        int total = 0;
        while (total < destination.Length)
        {
            int read = zs.Read(destination[total..]);
            if (read == 0)
                break;
            total += read;
        }

        // If there is more data to decompress but destination is full, signal buffer too small.
        if (total == destination.Length)
        {
            Span<byte> probe = stackalloc byte[1];
            int more = zs.Read(probe);
            if (more > 0)
                throw new ArgumentException("Destination buffer too small for decompressed data.", nameof(destination));
        }

        return total;
    }

    // Helper: Zlib Compression

    public static byte[] Compress(ReadOnlySpan<byte> data)
    {
        if (data.Length == 0)
            return [];
        using var ms = new MemoryStream();
        using (var zs = new ZLibStream(ms, CompressionLevel.SmallestSize, leaveOpen: true))
            zs.Write(data);
        return ms.ToArray();
    }

    // Overload: Compress into a caller-provided destination span, return bytes written
    public static int Compress(ReadOnlySpan<byte> data, Span<byte> destination, CompressionLevel level = CompressionLevel.SmallestSize)
    {
        if (data.Length == 0)
            return 0;

        using var ms = new MemoryStream();
        using (var zs = new ZLibStream(ms, level, leaveOpen: true))
        {
            zs.Write(data);
        }

        // Copy resulting compressed bytes into destination
        int written = (int)ms.Length;
        if (written > destination.Length)
            throw new ArgumentException("Destination buffer too small for compressed data.", nameof(destination));

        if (ms.TryGetBuffer(out var segment))
        {
            segment.AsSpan(0, written).CopyTo(destination);
        }
        else
        {
            // Fallback if underlying buffer isn't directly accessible
            var arr = ms.ToArray();
            arr.AsSpan().CopyTo(destination);
        }

        return written;
    }
}
