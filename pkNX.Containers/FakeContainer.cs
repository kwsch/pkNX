using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace pkNX.Containers
{
    public class FakeContainer : IFileContainer
    {
        public readonly byte[][] Files;
        private readonly byte[][] Backup;

        public FakeContainer(byte[][] files)
        {
            Files = files;
            Backup = new byte[files.Length][];
            for (int i = 0; i < Backup.Length; i++)
                Backup[i] = (byte[])files[i].Clone();
        }

        public string FilePath { get; set; } = string.Empty;
        public bool Modified { get; set; }
        public int Count => Files.Length;

        public byte[] this[int index]
        {
            get => Files[index];
            set => Files[index] = value;
        }

        public Task<byte[][]> GetFiles() => Task.FromResult(Files);
        public Task<byte[]> GetFile(int file, int subFile = 0) => Task.FromResult(this[file]);
        public Task SetFile(int file, byte[] value, int subFile = 0) => Task.FromResult(this[file] = value);
        public Task SaveAs(string path, ContainerHandler handler, CancellationToken token) => new Task(() => SaveAll(path, handler, token), token);

        public void SaveAll(string path, ContainerHandler handler, CancellationToken token)
        {
            handler.Initialize(Files.Length);
            for (int i = 0; i < Files.Length; i++)
            {
                if (token.IsCancellationRequested)
                    return;
                File.WriteAllBytes(Path.Combine(path, $"{i}.bin"), Files[i]);
                handler.StepFile(i);
            }
        }

        public void Dump(string path, ContainerHandler handler) => SaveAs(path, handler, CancellationToken.None);

        public void CancelEdits()
        {
            for (int i = 0; i < Files.Length; i++)
                Files[i] = (byte[])Backup[i].Clone();
        }
    }
}