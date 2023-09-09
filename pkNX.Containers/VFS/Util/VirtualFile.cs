using System;
using System.IO;

namespace pkNX.Containers.VFS;

public readonly record struct VirtualFile(IFileSystem FileSystem, FileSystemPath Path) : IFileSystemEntity
{
    public string Name => Path.EntityName;
    public VirtualDirectory ParentDirectory => VirtualDirectory.Create(FileSystem, Path.ParentPath);

    public (string, string) GetFileNameAndExtension()
    {
        int lastPeriod = Name.LastIndexOf('.');
        return (Name[..lastPeriod], Name[(lastPeriod + 1)..]);
    }

    public string GetFileNameWithoutExtension()
    {
        int lastPeriod = Name.LastIndexOf('.');
        return Name[..lastPeriod];
    }

    public string GetExtension()
    {
        int lastPeriod = Name.LastIndexOf('.');
        return Name[(lastPeriod + 1)..];
    }

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
        using var memoryStream = new MemoryStream();
        stream.CopyTo(memoryStream);
        return memoryStream.ToArray();
    }

    public void ReadAllBytes(Span<byte> bytes)
    {
        using var stream = Open();
        int bytesRead = stream.Read(bytes);

        if (bytesRead != bytes.Length)
            throw new IOException("Could not read all bytes.");
    }

    public string ReadAllText()
    {
        using var stream = Open();
        using var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }

    public void WriteAllBytes(ReadOnlySpan<byte> bytes)
    {
        using var stream = OpenWrite();
        stream.Write(bytes);
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
