using System.ComponentModel;
using System.Diagnostics;

// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global

namespace pkNX.Structures.FlatBuffers.SV.Trinity;

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class TrinityFileDescriptors
{
    public ulong GetSubFileIndex(ulong hash)
    {
        var index = BinarySearch(hash);
        Debug.Assert((uint)index < SubFileHashes.Count);
        return SubFileInfos[index].Index;
    }

    public bool GetHasSubFile(ulong hash)
    {
        var index = BinarySearch(hash);
        return index >= 0;
    }

    private int BinarySearch(ulong hash)
    {
        var arr = SubFileHashes;
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

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class TrinityFileDescriptorSubFileInfo
{
}

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class TrinityFileDescriptorInfo
{
}

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class TrinityFileDescriptorSubFileUnknown
{
}
