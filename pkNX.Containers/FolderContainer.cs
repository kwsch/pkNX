using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace pkNX.Containers;

public class FolderContainer : IFileContainer
{
    private readonly List<string> Paths = new();
    private readonly List<byte[]?> Data = new();
    private readonly List<bool> TrackModify = new();

    public string? FilePath { get; set; }

    public FolderContainer() { }
    public FolderContainer(IEnumerable<string> files) => AddFiles(files);

    public FolderContainer(string path) => FilePath = path;
    public FolderContainer(string path, Func<string, bool> filter) : this(path) => Initialize(filter);

    public IReadOnlyList<string> GetPaths() => Paths;
    public IReadOnlyList<string> GetFileNames() => Paths.Select(Path.GetFileName).ToList()!;

    public void Initialize(Func<string, bool>? filter = null)
    {
        if (Paths.Count > 0)
            return; // already initialized
        if (FilePath is null)
            throw new ArgumentException("No path specified to initialize from.");
        IEnumerable<string> files = Directory.GetFiles(FilePath, "*", SearchOption.AllDirectories);
        if (filter != null)
            files = files.Where(filter);

        AddFiles(files);
    }

    public void AddFile(string file, byte[]? data = null)
    {
        Paths.Add(file);
        Data.Add(data);
        TrackModify.Add(data != null);
    }

    public void AddFiles(IEnumerable<string> files)
    {
        foreach (var f in files)
            AddFile(f);
    }

    public byte[]? GetFileData(string file)
    {
        var index = GetFileIndex(file);
        if (index < 0)
            return null;
        string path = Paths[index];
        var data = Data[index] ??= FileMitm.ReadAllBytes(path);
        return (byte[])data.Clone();
    }

    public byte[] GetFileData(int index)
    {
        var data = Data[index] ??= FileMitm.ReadAllBytes(Paths[index]);
        return (byte[])data.Clone();
    }

    public byte[] this[int index]
    {
        get => GetFileData(index);
        set
        {
            var current = Data[index] ??= GetFileData(index);
            TrackModify[index] = !value.SequenceEqual(current);

            Data[index] = value;
        }
    }

    public void ResetIndex(int index)
    {
        Data[index] = null;
        TrackModify[index] = false;
    }

    public string GetFileName(int index) => Paths[index];
    public int GetFileIndex(string fileName) => Paths.FindIndex(z => Path.GetFileName(z) == fileName);

    public bool Modified
    {
        get => TrackModify.Contains(true);
        set { if (!value) CancelEdits(); }
    }

    public int Count => Paths.Count;

    public Task<byte[][]> GetFiles() => Task.FromResult(Paths.Select(FileMitm.ReadAllBytes).ToArray());
    public Task<byte[]> GetFile(int file, int subFile = 0) => Task.FromResult(this[file]);
    public Task SetFile(int file, byte[] value, int subFile = 0) => Task.FromResult(this[file] = value);
    public Task SaveAs(string path, ContainerHandler handler, CancellationToken token) => new(SaveAll, token);

    private void SaveAll()
    {
        for (int i = 0; i < Paths.Count; i++)
        {
            if (!TrackModify[i])
                continue;
            var data = Data[i];
            if (data == null)
                continue;
            FileMitm.WriteAllBytes(Paths[i], data);
        }
    }

    public void CancelEdits()
    {
        for (int i = 0; i < TrackModify.Count; i++)
        {
            TrackModify[i] = false;
            Data[i] = null;
        }
    }

    public void Dump(string path, ContainerHandler handler)
    {
        SaveAll(); // there's really nothing to dump, just save any modified
    }
}
