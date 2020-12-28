using System.Runtime.InteropServices;

namespace pkNX.Containers
{
    [StructLayout(LayoutKind.Sequential)]
    public class NSOHeader
    {
        public const int SIZE = 0x100;
        public const uint ExpectedMagic = 0x304F534E; // NSO0
        public bool Valid => Magic == ExpectedMagic;

        // Structure below

        public uint Magic;
        public uint Version;
        public uint Reserved;
        public NSOFlag Flags;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public SegmentHeader HeaderText;
        public int ModuleOffset;
        public SegmentHeader HeaderRO;
        public int ModuleFileSize;
        public SegmentHeader HeaderData;
        public int BssSize;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x20)]
        public byte[] DigestBuildID;

        public int SizeCompressedText;
        public int SizeCompressedRO;
        public int SizeCompressedData;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x1C)]
        public byte[] Padding;

        public RelativeExtent APIInfo;
        public RelativeExtent DynStr;
        public RelativeExtent DynSym;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x20)]
        public byte[] HashText;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x20)]
        public byte[] HashRO;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x20)]
        public byte[] HashData;
    }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
}