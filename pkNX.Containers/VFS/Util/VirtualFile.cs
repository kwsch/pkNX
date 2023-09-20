using System;
using System.Buffers;
using System.IO;

namespace pkNX.Containers.VFS;

public readonly record struct VirtualFile(IFileSystem FileSystem, FileSystemPath Path) : IFileSystemEntity
{
    public string Name => Path.EntityName;
    public VirtualDirectory ParentDirectory => VirtualDirectory.Create(FileSystem, Path.ParentPath);

    public override string ToString() => Path.ToString();

    public (string, string) GetFileNameAndExtension() => Path.GetFileNameAndExtension();
    public string GetFileNameWithoutExtension() => Path.GetFileNameWithoutExtension();
    public string GetExtension() => Path.GetExtension();

    public Stream Open(FileMode mode = FileMode.Open, FileAccess access = FileAccess.Read)
    {
        return FileSystem.OpenFile(Path, mode, access);
    }

    public Stream OpenRead()
    {
        return FileSystem.OpenFile(Path, FileMode.Open, FileAccess.Read);
    }

    public Stream OpenWrite()
    {
        return FileSystem.OpenWrite(Path);
    }

    public void CopyTo(VirtualDirectory destination)
    {
        FileSystem.Copy(Path, destination.FileSystem, destination.Path.AppendFile(Name));
    }

    public void MoveTo(VirtualDirectory destination)
    {
        FileSystem.Move(Path, destination.FileSystem, destination.Path.AppendFile(Name));
    }

    public void Delete(DeleteMode mode = DeleteMode.TopMostLayer)
    {
        FileSystem.Delete(Path, mode);
    }

    public ReadOnlySpan<byte> ReadAllBytes()
    {
        using var stream = Open();
        return stream.ReadAllBytes();
    }

    public void ReadAllBytes(Span<byte> destination)
    {
        using var stream = Open();
        stream.ReadAllBytes(destination);
    }

    public string ReadAllText()
    {
        using var stream = Open();
        return stream.ReadAllText();
    }

    public void WriteAllBytes(ReadOnlySpan<byte> bytes)
    {
        using var stream = OpenWrite();
        stream.Write(bytes);
    }

    public void WriteAllBytes(IBinarySerializable source)
    {
        using var stream = OpenWrite();
        using var bw = new BinaryWriter(stream);
        source.Write(bw);
    }

    public void WriteAllText(string text)
    {
        using var stream = OpenWrite();
        using var writer = new StreamWriter(stream);
        writer.Write(text);
    }

    internal static VirtualFile Create(IFileSystem fileSystem, FileSystemPath path)
    {
        if (!path.IsFile)
            throw new ArgumentException("The specified path is not a file.", nameof(path));

        return new VirtualFile(fileSystem, path);
    }
}
