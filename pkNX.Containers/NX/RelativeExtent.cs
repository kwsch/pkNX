using System.Runtime.InteropServices;

namespace pkNX.Containers;

[StructLayout(LayoutKind.Sequential)]
public class RelativeExtent
{
    public int RegionRODataOffset;
    public int RegionSize;
}
