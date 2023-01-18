using System;
using System.IO;

namespace pkNX.Containers.VFS;

public readonly record struct VirtualFile(IFileSystem FileSystem, FileSystemPath Path) : IFileSystemEntity
{
    public string Name => Path.EntityName;
    public VirtualDirectory ParentDirectory => VirtualDirectory.Create(FileSystem, Path.ParentPath);

    public Stream Open(FileAccess access)
    {
        return FileSystem.OpenFile(Path, access);
    }

    internal static VirtualFile Create(IFileSystem fileSystem, FileSystemPath path)
    {
        if (!path.IsFile)
            throw new ArgumentException("The specified path is no file.", nameof(path));

        return new VirtualFile(fileSystem, path);
    }
}
