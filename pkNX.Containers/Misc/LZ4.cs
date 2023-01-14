using System;
using K4os.Compression.LZ4;

namespace pkNX.Containers;

/// <summary>
/// Simple wrapper for the NuGet dependency exposing byte[] return signatures.
/// </summary>
public static class LZ4
{
    public static int Decode(ReadOnlySpan<byte> data, Span<byte> result) => LZ4Codec.Decode(data, result);
    public static int Encode(ReadOnlySpan<byte> data, Span<byte> result) => LZ4Codec.Encode(data, result);

    public static byte[] Decode(ReadOnlySpan<byte> data, int decLength)
    {
        var result = new byte[decLength];
        int length = Decode(data, result);
        if (length != result.Length)
            throw new ArgumentException($"Decoded length mismatch: {length} vs expected {result.Length}");
        return result;
    }

    public static byte[] Encode(ReadOnlySpan<byte> data)
    {
        var result = new byte[LZ4Codec.MaximumOutputSize(data.Length)];
        int length = Encode(data, result);
        return result[..length];
    }
}
