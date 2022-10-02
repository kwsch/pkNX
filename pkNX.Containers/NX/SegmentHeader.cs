using System.Runtime.InteropServices;

namespace pkNX.Containers;

[StructLayout(LayoutKind.Sequential)]
public class SegmentHeader
{
    public int FileOffset;
    public int MemoryOffset;
    public int DecompressedSize;
}
