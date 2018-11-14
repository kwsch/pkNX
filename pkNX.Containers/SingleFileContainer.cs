using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace pkNX.Containers
{
    public class SingleFileContainer : IFileContainer
    {
        public string FilePath { get; set; }
        public bool Modified { get; set; }
        public int Count => 1;

        public byte[] Data;
        public SingleFileContainer(byte[] data) => Data = data;
        public SingleFileContainer(BinaryReader br) => Data = br.ReadBytes((int) br.BaseStream.Length);

        public SingleFileContainer(string path)
        {
            FilePath = path;
            Data = File.ReadAllBytes(path);
        }

        public byte[] this[int index] { get => Data; set => Data = value; }
        public Task<byte[][]> GetFiles() => new Task<byte[][]>(() => new[] {Data});
        public Task<byte[]> GetFile(int file, int subFile = 0) => new Task<byte[]>(() => Data);
        public Task SetFile(int file, byte[] value, int subFile = 0) => new Task(() => Data = value);
        public Task SaveAs(string path, ContainerHandler handler, CancellationToken token) => new Task(() => Dump(path, handler));
        public void Dump(string path, ContainerHandler handler) => File.WriteAllBytes(path ?? FilePath, Data);
    }
}
