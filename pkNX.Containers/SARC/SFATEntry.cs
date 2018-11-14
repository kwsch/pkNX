using System.IO;
using System.Text;

namespace pkNX.Containers
{
    /// <summary>
    /// <see cref="SARC"/> File Access Table (<see cref="SFAT"/>) Entry
    /// </summary>
    public class SFATEntry : LargeContainerEntry
    {
        public uint FileNameHash;
        public int FileNameOffset;

        public string FileName { get; private set; }

        private static uint GetHash(string name, int length, uint multiplier)
        {
            uint result = 0;
            for (int i = 0; i < length; i++)
                result = name[i] + (result * multiplier);
            return result;
        }

        public override int Length
        {
            get => End - Start;
            set { }
        }

        public SFATEntry() { }

        public SFATEntry(BinaryReader br, int DataOffset)
        {
            FileNameHash = br.ReadUInt32();
            FileNameOffset = br.ReadInt32();
            Start = br.ReadInt32();
            End = br.ReadInt32();
            ParentDataPosition = DataOffset;
        }

        public void Write(BinaryWriter bw)
        {
            bw.Write(FileNameHash);
            bw.Write(FileNameOffset);
            bw.Write(Start);
            bw.Write(End);
        }

        public string GetFileName(Stream parent, int StringOffset)
        {
            if (FileName != null)
                return FileName;

            var ofs = ((FileNameOffset & 0x00FFFFFF) * 4) + StringOffset;
            parent.Seek(ofs, SeekOrigin.Begin);
            var sb = new StringBuilder();

            for (char c = (char)parent.ReadByte(); c != 0; c = (char)parent.ReadByte())
                sb.Append(c);

            FileName = sb.Replace('/', Path.DirectorySeparatorChar).ToString();
            return FileName;
        }

        public void WriteFileName(Stream parent, int StringOffset)
        {
            FileNameOffset = (int)(parent.Position - StringOffset) / 4;

            var str = FileName.Replace(Path.DirectorySeparatorChar, '/');
            foreach (var b in str)
                parent.WriteByte((byte)b);
            parent.WriteByte(0); // \0
        }

        public void SetFileName(string value, uint hashMult)
        {
            FileName = value;
            FileNameHash = GetHash(value, value.Length, hashMult);
        }
    }
}