using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace pkNX.Containers;

public class InternalFileContainer : IFileContainer
{
    public string? FilePath { get; set; }
    public bool Modified { get; set; }
    public int Count => 1;

    public byte[] Data = [];
    private byte[] Backup = [];
    public InternalFileContainer(byte[] data) => LoadData(data);

    private void LoadData(byte[] data) => Backup = (byte[]) (Data = data).Clone();

    public void CancelEdits()
    {
        Modified = false;
        Data = (byte[]) Backup.Clone();
    }

    public byte[] this[int index]
    {
        get => (byte[])Data.Clone();
        set
        {
            Modified |= !Data.SequenceEqual(value);
            Data = value;
        }
    }

    public Task<byte[][]> GetFiles() => Task.FromResult(new[] {this[0]});
    public Task<byte[]> GetFile(int file, int subFile = 0) => Task.FromResult(this[0]);
    public Task SetFile(int file, byte[] value, int subFile = 0) => Task.FromResult(Data = value);
    public Task SaveAs(string path, ContainerHandler handler, CancellationToken token) => new(() => Dump(path, handler), token);
    public void Dump(string? path, ContainerHandler handler) => FileMitm.WriteAllBytes(path ?? FilePath!, Data);
}
