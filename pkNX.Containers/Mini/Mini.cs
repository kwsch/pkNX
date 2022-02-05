using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace pkNX.Containers
{
    public class Mini : IFileContainer
    {
        public string Identifier { get; }
        public byte[][] Files { get; private set; }

        public Mini(byte[][] data, string ident)
        {
            Identifier = ident;
            Files = data;
            Backup = new byte[data.Length][];
            for (int i = 0; i < Backup.Length; i++)
                Backup[i] = (byte[])data[i].Clone();
        }

        public byte[] this[int index]
        {
            get => (byte[])Files[index].Clone();
            set
            {
                Modified |= !Files[index].SequenceEqual(value);
                Files[index] = value;
            }
        }

        public string? FilePath { get; set; }
        private readonly byte[][] Backup;

        public bool Modified { get; set; }
        public int Count => Files.Length;

        public Task<byte[]> GetFile(int file, int subFile = 0) => Task.FromResult(Files[file]);
        public Task SetFile(int file, byte[] value, int subFile = 0) => Task.FromResult(Files[file] = value);
        public Task<byte[][]> GetFiles() => Task.FromResult(Files);

        public void CancelEdits()
        {
            Modified = false;
            Files = new byte[Backup.Length][];
            for (int i = 0; i < Files.Length; i++)
                Files[i] = (byte[]) Backup[i].Clone();
        }

        public Task SaveAs(string path, ContainerHandler handler, CancellationToken token) => new(() =>
        {
            byte[] data = MiniUtil.PackMini(Files, Identifier);
            FileMitm.WriteAllBytes(path, data);
        }, token);

        public void Dump(string path, ContainerHandler handler)
        {
            string format = this.GetFileFormatString();
            Directory.CreateDirectory(path);

            handler.Initialize(Count);
            for (int i = 0; i < Count; i++)
            {
                var fn = Path.Combine(path, i.ToString(format) + ".bin");
                FileMitm.WriteAllBytes(fn, Files[i]);
                handler.StepFile(i + 1);
            }
        }
    }
}
