using System;
using System.IO;

namespace pkNX.Containers
{
    public class GARCHeader
    {
        internal const uint MAGIC = 0x4E415243; // CRAG
        private const ushort VER_4 = (ushort)GARCVersion.VER_4;
        private const ushort VER_6 = (ushort)GARCVersion.VER_6;

        private readonly uint Magic = MAGIC;
        private readonly int HeaderSize;
        private readonly ushort Endianess = 0xFEFF;
        private readonly ushort Version;
        private readonly uint ChunkCount = 4;

        public int DataOffset { get; set; }
        public uint FileSize { get; }

        public readonly int ContentPadToNearest = 4; // Format 6 Only (4 bytes is standard in VER_4, and is not stored)
        public int ContentLargestUnpadded;
        public int ContentLargestPadded; // Format 6 Only

        public bool VER6 => Version == VER_6;
        public bool VER4 => Version == VER_4;

        public GARCHeader(GARCVersion version)
        {
            Version = version switch
            {
                GARCVersion.VER_6 => VER_6,
                GARCVersion.VER_4 => VER_4,
                _ => (ushort)version,
            };
            HeaderSize = VER6 ? 0x24 : 0x1C;
        }

        public GARCHeader(BinaryReader br)
        {
            Magic = br.ReadUInt32();
            HeaderSize = br.ReadInt32();
            Endianess = br.ReadUInt16();
            Version = br.ReadUInt16();
            ChunkCount = br.ReadUInt32();

            if (ChunkCount != 4)
                throw new FormatException($"Invalid GARC Chunk Count: {ChunkCount}");

            DataOffset = br.ReadInt32();
            FileSize = br.ReadUInt32();

            switch (Version)
            {
                case VER_4:
                    ContentLargestUnpadded = br.ReadInt32();
                    ContentPadToNearest = 4;
                    break;
                case VER_6:
                    ContentLargestPadded = br.ReadInt32();
                    ContentLargestUnpadded = br.ReadInt32();
                    ContentPadToNearest = br.ReadInt32();
                    break;
                default:
                    throw new FormatException($"Invalid GARC Version: 0x{Version:X4}");
            }
        }

        public void Write(BinaryWriter bw)
        {
            bw.Write(Magic);
            bw.Write(HeaderSize);
            bw.Write(Endianess);
            bw.Write(Version);
            bw.Write(ChunkCount);

            if (ChunkCount != 4)
                throw new FormatException($"Invalid GARC Chunk Count: {ChunkCount}");

            bw.Write(DataOffset);
            bw.Write(FileSize);

            switch (Version)
            {
                case VER_4:
                    bw.Write(ContentLargestUnpadded);
                    break;
                case VER_6:
                    bw.Write(ContentLargestPadded);
                    bw.Write(ContentLargestUnpadded);
                    bw.Write(ContentPadToNearest);
                    break;
                default:
                    throw new FormatException($"Invalid GARC Version: 0x{Version:X4}");
            }
        }
    }
}