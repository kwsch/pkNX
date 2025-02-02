using System;
using System.ComponentModel;

// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global

namespace pkNX.Structures.FlatBuffers.SV.Trinity;

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class TrinityFileSystemMetadata
{
    public ulong GetFileOffset(ulong hashFnv)
    {
        var index = BinarySearch(hashFnv);
        if (index < 0)
            throw new ArgumentException(null, nameof(hashFnv));
        return FileOffsets[index];
    }

    public bool HasFile(ulong hashFnv) => BinarySearch(hashFnv) >= 0;

    private int BinarySearch(ulong hash)
    {
        var arr = FileHashes;
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
