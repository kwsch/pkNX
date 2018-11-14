using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace pkNX.Containers
{
    public class Mini : IFileContainer
    {
        public string Identifier { get; }
        public byte[][] Files { get; }

        public Mini(byte[][] data, string ident) { Identifier = ident; Files = data; }

        public byte[] this[int index]
        {
            get => Files[index];
            set
            {
                Files[index] = value;
                Modified = true;
            }
        }

        public string FilePath { get; set; }

        public bool Modified { get; set; }
        public int Count => Files.Length;

        public Task<byte[]> GetFile(int file, int subFile = 0) => new Task<byte[]>(()=>Files[file]);
        public Task SetFile(int file, byte[] value, int subFile = 0) => new Task(() => Files[file] = value);
        public Task<byte[][]> GetFiles() => new Task<byte[][]>(() => Files);

        public Task SaveAs(string path, ContainerHandler handler, CancellationToken token)
        {
            return new Task(() =>
            {
                byte[] data = MiniUtil.PackMini(Files, Identifier);
                File.WriteAllBytes(path, data);
            });
        }

        public void Dump(string path, ContainerHandler handler)
        {
            string format = this.GetFileFormatString();
            Directory.CreateDirectory(path);

            handler.Initialize(Count);
            for (int i = 0; i < Count; i++)
            {
                var fn = Path.Combine(path, i.ToString(format) + ".bin");
                File.WriteAllBytes(fn, Files[i]);
                handler.StepFile(i + 1);
            }
        }
    }
}
