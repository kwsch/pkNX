using System;
using System.Collections.Generic;

namespace pkNX.Containers.VFS;

public readonly record struct VirtualDirectory(IFileSystem FileSystem, FileSystemPath Path) : IFileSystemEntity
{
    public string Name => Path.EntityName;
    public VirtualDirectory ParentDirectory => Create(FileSystem, Path.ParentPath);


    internal static VirtualDirectory Create(IFileSystem fileSystem, FileSystemPath path)
    {
        if (!path.IsDirectory)
            throw new ArgumentException("The specified path is no directory.", nameof(path));

        return new VirtualDirectory(fileSystem, path);
    }
}
