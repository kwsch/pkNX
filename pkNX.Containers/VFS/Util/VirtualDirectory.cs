using System;
using System.Collections.Generic;

namespace pkNX.Containers.VFS;

public readonly record struct VirtualDirectory(IFileSystem FileSystem, FileSystemPath Path) : IFileSystemEntity
{
    public string Name => Path.EntityName;
    public VirtualDirectory ParentDirectory => Create(FileSystem, Path.ParentPath);

    public void CopyTo(VirtualDirectory destination)
    {
        FileSystem.Copy(Path, destination.FileSystem, destination.Path.AppendDirectory(Name));
    }

    public void MoveTo(VirtualDirectory destination)
    {
        FileSystem.Move(Path, destination.FileSystem, destination.Path.AppendDirectory(Name));
    }

    internal static VirtualDirectory Create(IFileSystem fileSystem, FileSystemPath path)
    {
        if (!path.IsDirectory)
            throw new ArgumentException("The specified path is no directory.", nameof(path));

        return new VirtualDirectory(fileSystem, path);
    }

    public IEnumerable<FileSystemPath> GetEntityPaths(Func<FileSystemPath, bool>? filter = null)
    {
        return FileSystem.GetEntityPaths(Path, filter);
    }
}
