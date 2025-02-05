using System.Diagnostics;
using pkNX.Containers;

namespace pkNX.Structures.FlatBuffers.SV.Trinity;

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
    public byte[] GetUncompressedData() => CompressionType switch
    {  // TODO: What specific ID is zlib?
        3 => Oodle.Decompress(Data.Span, (long)UncompressedSize)!,  // Oodle
        0xFF => Data.ToArray(), // Uncompressed
        _ => throw new ArgumentException($"Unknown compression type {CompressionType}"),
    };

    public ReadOnlySpan<byte> GetUncompressedDataReadOnly() => CompressionType switch
    {  // TODO: What specific ID is zlib?
        3 => Oodle.Decompress(Data.Span, (long)UncompressedSize)!,  // Oodle
        0xFF => Data.ToArray(), // Uncompressed
        _ => throw new ArgumentException($"Unknown compression type {CompressionType}"),
    };
}
