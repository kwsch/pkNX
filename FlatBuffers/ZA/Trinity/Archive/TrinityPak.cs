using pkNX.Containers;
using System.Diagnostics;

namespace pkNX.Structures.FlatBuffers.ZA.Trinity;

public partial class TrinityPak
{
    public TrinityPakFileData GetFileData(ulong hash)
    {
        var index = BinarySearch(hash);
        Debug.Assert((uint)index < Hashes.Count);
        return Files[index];
    }

    private int BinarySearch(ulong hash)
    {
        var arr = Hashes;
        var lo = 0;
        var hi = arr.Count - 1;
        while (lo <= hi)
        {
            var mid = lo + (hi - lo >> 1);
            var midVal = arr[mid];
            if (midVal < hash)
                lo = mid + 1;
            else if (midVal > hash)
                hi = mid - 1;
            else
                return mid;
        }
        return -1;
    }
}

public partial class TrinityPakFileData
{
    public byte[] Decompress() => CompressionType switch
    {
        DataCompressionType.None => Data.ToArray(), // Uncompressed
        DataCompressionType.Zlib => Zlib.Decompress(Data.Span, (int)UncompressedSize),
        DataCompressionType.Lz4 => LZ4.Decode(Data.Span, (int)UncompressedSize),
        >= DataCompressionType.OodleKraken and <= DataCompressionType.OodleHydra => Oodle.Decompress(Data.Span, (long)UncompressedSize)!,  // Oodle
        _ => throw new ArgumentException($"Unknown compression type {CompressionType}"),
    };

    public void DecompressTo(Span<byte> result)
    {
        if (CompressionType == DataCompressionType.None)
            Data.Span.CopyTo(result); // Uncompressed
        else if (CompressionType == DataCompressionType.Zlib)
            Zlib.Decompress(Data.Span, result);
        else if (CompressionType == DataCompressionType.Lz4)
            LZ4.Decode(Data.Span, result);
        else if (CompressionType is >= DataCompressionType.OodleKraken and <= DataCompressionType.OodleHydra)
            Oodle.TryDecompress(Data.Span, result, out _);
        else
            throw new ArgumentException($"Unknown compression type {CompressionType}");
    }

    public static ReadOnlySpan<byte> Compress(ReadOnlySpan<byte> data, DataCompressionType type, OodleCompressionLevel level = OodleCompressionLevel.Optimal2) => type switch
    {
        DataCompressionType.None => data,
        DataCompressionType.Zlib => Zlib.Compress(data),
        DataCompressionType.Lz4 => LZ4.Encode(data),
        DataCompressionType.OodleKraken => Oodle.Compress(data, out _, OodleFormat.Kraken, level),
        DataCompressionType.OodleLeviathan => Oodle.Compress(data, out _, OodleFormat.Leviathan, level),
        DataCompressionType.OodleMermaid => Oodle.Compress(data, out _, OodleFormat.Mermaid, level),
        DataCompressionType.OodleSelkie => Oodle.Compress(data, out _, OodleFormat.Selkie, level),
        DataCompressionType.OodleHydra => Oodle.Compress(data, out _, OodleFormat.Hydra, level),
        _ => throw new NotImplementedException($"Compression type {type} is not implemented."),
    };
}
