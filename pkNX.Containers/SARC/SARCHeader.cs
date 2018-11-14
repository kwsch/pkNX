using System.IO;

namespace pkNX.Containers
{
    public class SARCHeader
    {
        public const string Identifier = nameof(SARC);

        /// <summary>
        /// The required <see cref="Magic"/> matches the first 4 bytes of the file data.
        /// </summary>
        public bool SigMatches => Magic == Identifier;

        public string Magic = Identifier;
        public ushort HeaderSize = 0x14;
        public ushort Endianness = 0xFFFE;
        public uint FileSize;
        public int DataOffset;
        public ushort Version = 0x0100;
        public ushort Reserved;

        public SARCHeader() { }

        public SARCHeader(BinaryReader Reader)
        {
            Magic = new string(Reader.ReadChars(4));
            HeaderSize = Reader.ReadUInt16();
            Endianness = Reader.ReadUInt16();
            FileSize = Reader.ReadUInt32();
            DataOffset = Reader.ReadInt32();
            Version = Reader.ReadUInt16();
            Reserved = Reader.ReadUInt16();
        }

        public void Write(BinaryWriter bw)
        {
            foreach (var c in Magic)
                bw.Write((byte)c);
            bw.Write(HeaderSize);
            bw.Write(FileSize);
            bw.Write(FileSize);
            bw.Write(DataOffset);
            bw.Write(Version);
            bw.Write(Reserved);
        }
    }
}
