using System.IO;

namespace pkNX.Containers
{
    public class LargeContainerEntry
    {
        public int Start { get; set; }
        public int End { get; set; }
        public virtual int Length { get; set; }
        public object? File { get; set; }
        public int ParentDataPosition { get; set; }

        public byte[] GetFileData(Stream parent)
        {
            parent.Seek(Start + ParentDataPosition, SeekOrigin.Begin);
            byte[] data = new byte[Length];
            _ = parent.Read(data, 0, Length);
            return data;
        }

        public void Write(Stream parent, Stream dest, int DataOffset)
        {
            switch (File)
            {
                case string f:
                    WriteFrom(dest, DataOffset, f);
                    break;
                case byte[] data:
                    WriteFrom(dest, DataOffset, data);
                    break;
                default:
                    WriteFrom(dest, DataOffset, parent);
                    break;
            }
        }

        private void WriteFrom(Stream dest, int DataOffset, string f)
        {
            Start = (int)dest.Position - DataOffset;
            using (var s = new FileStream(f, FileMode.Open, FileAccess.Read, FileShare.None))
                s.CopyTo(dest);
            End = (int)dest.Position - DataOffset;
            Length = End - Start;
        }

        private void WriteFrom(Stream dest, int DataOffset, byte[] data)
        {
            Start = (int)dest.Position - DataOffset;
            Length = data.Length;
            End = Start + Length;

            File = data;
            dest.Write(data, 0, data.Length);
        }

        private void WriteFrom(Stream dest, int DataOffset, Stream source)
        {
            Start = (int)dest.Position - DataOffset;
            End = Start + Length;

            source.Seek(Start + ParentDataPosition, SeekOrigin.Begin);
            source.CopyTo(dest, Length);
        }

        public void Dump(Stream parent, string path, int DataOffset)
        {
            if (File is string)
                return;

            using var file = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None);
            Write(parent, file, DataOffset);
        }
    }
}
