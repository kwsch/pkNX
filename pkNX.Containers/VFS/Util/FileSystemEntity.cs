using System;

namespace pkNX.Containers.VFS;

public class FileSystemEntity : IEquatable<FileSystemEntity>
{
    public IFileSystem FileSystem { get; }
    public FileSystemPath Path { get; }
    public string Name => Path.EntityName;

    public FileSystemEntity(IFileSystem fileSystem, FileSystemPath path)
    {
        FileSystem = fileSystem;
        Path = path;
    }

    public override bool Equals(object? obj)
    {
        return obj is FileSystemEntity other && ((IEquatable<FileSystemEntity>)this).Equals(other);
    }

    public override int GetHashCode()
    {
        return FileSystem.GetHashCode() ^ Path.GetHashCode();
    }

    bool IEquatable<FileSystemEntity>.Equals(FileSystemEntity? other)
    {
        return FileSystem.Equals(other?.FileSystem) && Path.Equals(other.Path);
    }

    public static FileSystemEntity Create(IFileSystem fileSystem, FileSystemPath path)
    {
        if (path.IsFile)
            return new VirtualFile(fileSystem, path);

        return new VirtualDirectory(fileSystem, path);
    }
}

