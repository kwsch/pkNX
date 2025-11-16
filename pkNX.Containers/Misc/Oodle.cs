using System;
using System.Runtime.InteropServices;

namespace pkNX.Containers;

/// <summary>
/// Oodle Compression and Decompression wrapper around the external dll.
/// </summary>
/// <remarks>
/// These methods are safely tuned for Span in order to minimize allocation.
/// </remarks>
public static class Oodle
{
    /// <summary>
    /// Oodle Library Path
    /// </summary>
    public const string OodleLibraryPath = "oo2core_8_win64";

    /// <summary>
    /// Oodle64 Decompression Method
    /// </summary>
    [DllImport(OodleLibraryPath, CallingConvention = CallingConvention.Cdecl)]
    private static extern long OodleLZ_Decompress(ref byte buffer, long bufferSize, ref byte result, long outputBufferSize,
        OodleFuzzSafe fuzz = OodleFuzzSafe.Yes,
        OodleCheckCrc crc = OodleCheckCrc.No,
        OodleVerbosity verbosity = OodleVerbosity.None,
        long context = 0, long e = 0, long callback = 0, long callback_ctx = 0, long scratch = 0, long scratch_size = 0,
        OodleThreadPhase threadPhase = OodleThreadPhase.Unthreaded);

    /// <summary>
    /// Oodle64 Compression Method
    /// </summary>
    [DllImport(OodleLibraryPath, CallingConvention = CallingConvention.Cdecl)]
    private static extern long OodleLZ_Compress(OodleFormat format, ref byte buffer, long bufferSize, ref byte result, OodleCompressionLevel level,
        long opts = 0, long context = 0, long unused = 0, long scratch = 0, long scratch_size = 0);

    /// <summary>
    /// Decompresses compressed data into a newly allocated array. Returns null on failure.
    /// </summary>
    /// <param name="input">Input Compressed Data</param>
    /// <param name="decompressedLength">Decompressed Size</param>
    /// <returns>Resulting Array if success, otherwise null.</returns>
    public static byte[]? Decompress(ReadOnlySpan<byte> input, long decompressedLength)
    {
        if (decompressedLength is < 0 or > int.MaxValue)
            throw new ArgumentOutOfRangeException(nameof(decompressedLength));
        if (decompressedLength == 0)
            return [];

        var result = new byte[(int)decompressedLength];
        return DecompressInternal(input, result) ? result : null;
    }

    /// <summary>
    /// Try to decompress into a caller-provided destination buffer.
    /// </summary>
    /// <param name="input">Compressed data.</param>
    /// <param name="destination">Destination buffer (must be large enough).</param>
    /// <param name="bytesWritten">Actual decompressed size on success.</param>
    public static bool TryDecompress(ReadOnlySpan<byte> input, Span<byte> destination, out int bytesWritten)
    {
        bytesWritten = 0;

        if (destination.Length == 0)
            return input.Length == 0;

        if (input.Length == 0)
            return false;

        ref byte src = ref MemoryMarshal.GetReference(input);
        ref byte dst = ref MemoryMarshal.GetReference(destination);

        long decoded = OodleLZ_Decompress(
            ref src,
            input.Length,
            ref dst,
            destination.Length,
            OodleFuzzSafe.Yes,
            OodleCheckCrc.No,
            OodleVerbosity.None,
            0, 0, 0, 0, 0, 0,
            OodleThreadPhase.Unthreaded);

        if (decoded <= 0 || decoded > destination.Length)
            return false;

        bytesWritten = (int)decoded;
        return true;
    }

    /// <summary>
    /// Compresses data and returns a Span over the allocated buffer (aligned length).
    /// </summary>
    /// <param name="input">Input Decompressed Data</param>
    /// <param name="compressedSize">Actual Compressed Data size</param>
    /// <param name="format">Compression format to use</param>
    /// <param name="level">Compression setting to use</param>
    /// <returns>Span of compressed data with remainder aligned working bytes.</returns>
    public static Span<byte> Compress(ReadOnlySpan<byte> input, out int compressedSize,
        OodleFormat format = OodleFormat.Kraken, OodleCompressionLevel level = OodleCompressionLevel.Optimal2)
    {
        if (input.Length == 0)
        {
            compressedSize = 0;
            return Span<byte>.Empty;
        }

        int maxSize = GetMaxCompressedSize(input.Length);
        var buffer = new byte[maxSize];
        var span = buffer.AsSpan();
        return CompressInternal(input, span, out compressedSize, format, level);
    }

    /// <summary>
    /// Try to compress into a caller-provided destination buffer.
    /// </summary>
    /// <param name="input">Uncompressed data.</param>
    /// <param name="destination">Destination buffer (size must be at least GetMaxCompressedSize(input.Length)).</param>
    /// <param name="compressedSize">Exact compressed byte size (without alignment padding).</param>
    /// <param name="format">Format.</param>
    /// <param name="level">Compression level.</param>
    /// <param name="compressedSlice">Slice (including alignment padding) on success.</param>
    /// <returns>True on success.</returns>
    public static bool TryCompress(ReadOnlySpan<byte> input, Span<byte> destination, out int compressedSize,
        out Span<byte> compressedSlice,
        OodleFormat format = OodleFormat.Kraken,
        OodleCompressionLevel level = OodleCompressionLevel.Optimal2)
    {
        compressedSize = 0;
        compressedSlice = default;

        if (input.Length == 0)
        {
            compressedSlice = destination[..0];
            return true;
        }

        if (destination.Length < GetMaxCompressedSize(input.Length))
            return false;

        ref byte src = ref MemoryMarshal.GetReference(input);
        ref byte dst = ref MemoryMarshal.GetReference(destination);

        long encoded = OodleLZ_Compress(format, ref src, input.Length, ref dst, level);
        if (encoded <= 0 || encoded > destination.Length)
            return false;

        compressedSize = (int)encoded;
        int aligned = Align4(compressedSize);
        if (aligned > destination.Length)
            return false;

        compressedSlice = destination[..aligned];
        return true;
    }

    private static bool DecompressInternal(ReadOnlySpan<byte> input, Span<byte> destination)
    {
        if (destination.Length == 0)
            return input.Length == 0;

        if (input.Length == 0)
            return false;

        ref byte src = ref MemoryMarshal.GetReference(input);
        ref byte dst = ref MemoryMarshal.GetReference(destination);

        long decoded = OodleLZ_Decompress(
            ref src,
            input.Length,
            ref dst,
            destination.Length,
            OodleFuzzSafe.Yes,
            OodleCheckCrc.No,
            OodleVerbosity.None,
            0, 0, 0, 0, 0, 0,
            OodleThreadPhase.Unthreaded);

        return decoded > 0 && decoded <= destination.Length;
    }

    private static Span<byte> CompressInternal(ReadOnlySpan<byte> input, Span<byte> destination, out int compressedSize,
        OodleFormat format, OodleCompressionLevel level)
    {
        if (input.Length == 0)
        {
            compressedSize = 0;
            return destination[..0];
        }

        ref byte src = ref MemoryMarshal.GetReference(input);
        ref byte dst = ref MemoryMarshal.GetReference(destination);

        long encoded = OodleLZ_Compress(format, ref src, input.Length, ref dst, level);
        if (encoded <= 0)
        {
            compressedSize = 0;
            return Span<byte>.Empty;
        }

        compressedSize = (int)encoded;
        int aligned = Align4(compressedSize);
        return destination[..aligned];
    }

    private static int Align4(int value) => (value + 3) & ~3;

    /// <summary>
    /// Gets the dimension required to compress the data.
    /// </summary>
    public static int GetMaxCompressedSize(int inputSize)
    {
        if (inputSize < 0)
            throw new ArgumentOutOfRangeException(nameof(inputSize));
        long val = inputSize + (274L * ((inputSize + 0x3FFFFL) / 0x40000L));
        if (val > int.MaxValue)
            throw new ArgumentOutOfRangeException(nameof(inputSize), "Computed size exceeds supported range.");
        return (int)val;
    }
}

public enum OodleFormat : uint
{
    LZH = 0,
    LZHLW = 1,
    LZNIB = 2,
    None = 3,
    LZB16 = 4,
    LZBLW = 5,
    LZA = 6,
    LZNA = 7,
    Kraken = 8,
    Mermaid = 9,
    BitKnit = 10,
    Selkie = 11,
    Hydra = 12,
    Leviathan = 13,
}

public enum OodleCompressionLevel : ulong
{
    None = 0,
    SuperFast = 1,
    VeryFast = 2,
    Fast = 3,
    Normal = 4,
    Optimal1 = 5,
    Optimal2 = 6,
    Optimal3 = 7,
    Optimal4 = 8,
    Optimal5 = 9,
}

public enum OodleFuzzSafe
{
    No = 0,
    Yes = 1,
}

public enum OodleCheckCrc
{
    No = 0,
    Yes = 1,
}

public enum OodleVerbosity
{
    None = 0,
    Max = 3,
}

[Flags]
public enum OodleThreadPhase
{
    Invalid = 0,
    ThreadPhase1 = 1,
    ThreadPhase2 = 2,

    Unthreaded = ThreadPhase1 | ThreadPhase2, // 3
}
