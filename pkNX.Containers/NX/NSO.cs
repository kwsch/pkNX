using System.IO;
using System.Linq;
using System.Security.Cryptography;

namespace pkNX.Containers
{
    public class NSO
    {
        public NSOHeader Header { get; private set; }

        public byte[] CompressedText { get; set; }
        public byte[] CompressedRO { get; set; }
        public byte[] CompressedData { get; set; }

        public byte[] DecompressedText { get; set; }
        public byte[] DecompressedRO { get; set; }
        public byte[] DecompressedData { get; set; }

        public bool ValidText => Hash(DecompressedText).SequenceEqual(Header.HashText);
        public bool ValidRO => Hash(DecompressedText).SequenceEqual(Header.HashText);
        public bool ValidData => Hash(DecompressedText).SequenceEqual(Header.HashText);

        public decimal CompressionRatioText => (decimal)DecompressedText.Length / CompressedText.Length;
        public decimal CompressionRatioRO => (decimal)DecompressedRO.Length / CompressedRO.Length;
        public decimal CompressionRatioData => (decimal)DecompressedData.Length / CompressedData.Length;

        public NSO(BinaryReader br) => ReadHeader(br);

        public NSO(byte[] data)
        {
            using var ms = new MemoryStream(data);
            using var br = new BinaryReader(ms);
            ReadHeader(br);
        }

        private void ReadHeader(BinaryReader br)
        {
            if (br.BaseStream.Length < NSOHeader.SIZE)
                return;
            Header = br.ReadBytes(NSOHeader.SIZE).ToClass<NSOHeader>();

            // seek around to decode
            CompressedText = GetCompressedSegment(br, Header.HeaderText, Header.SizeCompressedText);
            CompressedRO = GetCompressedSegment(br, Header.HeaderRO, Header.SizeCompressedRO);
            CompressedData = GetCompressedSegment(br, Header.HeaderData, Header.SizeCompressedData);

            Decompress();
        }

        public static byte[] GetCompressedSegment(BinaryReader br, SegmentHeader h, int sizeCompressed)
        {
            br.BaseStream.Position = h.FileOffset;
            return br.ReadBytes(sizeCompressed);
        }

        public static byte[] GetDecompressedSegment(BinaryReader br, SegmentHeader h, int sizeCompressed)
        {
            byte[] data = GetCompressedSegment(br, h, sizeCompressed);
            return LZ4.Decode(data, h.DecompressedSize);
        }

        public static byte[] Hash(byte[] data)
        {
            using var method = SHA256.Create();
            return method.ComputeHash(data);
        }

        private void Decompress()
        {
            DecompressedText = Header.Flags.HasFlagFast(NSOFlag.CompressedText)
                ? LZ4.Decode(CompressedText, Header.HeaderText.DecompressedSize)
                : CompressedText;
            DecompressedRO = Header.Flags.HasFlagFast(NSOFlag.CompressedRO)
                ? LZ4.Decode(CompressedRO, Header.HeaderRO.DecompressedSize)
                : CompressedRO;
            DecompressedData = Header.Flags.HasFlagFast(NSOFlag.CompressedData)
                ? LZ4.Decode(CompressedData, Header.HeaderData.DecompressedSize)
                : CompressedData;
        }

        private void Compress()
        {
            CompressedText = Header.Flags.HasFlagFast(NSOFlag.CompressedText)
                ? LZ4.Encode(DecompressedText)
                : DecompressedText;
            Header.SizeCompressedText = CompressedText.Length;
            Header.HeaderText.DecompressedSize = DecompressedText.Length;
            Header.HashText = Hash(DecompressedText);

            CompressedRO = Header.Flags.HasFlagFast(NSOFlag.CompressedRO)
                ? LZ4.Encode(DecompressedRO)
                : DecompressedRO;
            Header.SizeCompressedRO = CompressedRO.Length;
            Header.HeaderRO.DecompressedSize = DecompressedRO.Length;
            Header.HashRO = Hash(DecompressedRO);

            CompressedData = Header.Flags.HasFlagFast(NSOFlag.CompressedData)
                ? LZ4.Encode(DecompressedData)
                : DecompressedData;
            Header.SizeCompressedData = CompressedData.Length;
            Header.HeaderData.DecompressedSize = DecompressedData.Length;
            Header.HashData = Hash(DecompressedData);
        }

        public byte[] Write()
        {
            Compress();
            using var ms = new MemoryStream();
            using var bw = new BinaryWriter(ms);
            bw.Write(Header.ToBytesClass());
            while (bw.BaseStream.Position != Header.HeaderText.FileOffset)
                bw.Write((byte)0); // match layout for previous example

            // text
            Header.HeaderText.FileOffset = (int)bw.BaseStream.Position;
            bw.Write(CompressedText);

            // ro
            Header.HeaderRO.FileOffset = (int)bw.BaseStream.Position;
            bw.Write(CompressedRO);

            // data
            Header.HeaderData.FileOffset = (int)bw.BaseStream.Position;
            bw.Write(CompressedData);

            bw.BaseStream.Position = 0;
            bw.Write(Header.ToBytesClass());

            return ms.ToArray();
        }
    }
}
