using System;
using System.Collections.Generic;
using System.IO;

namespace pkNX.Containers.VFS;

public class ReadOnlyFileSystem : IFileSystem
{
    public bool IsReadOnly => true;

    public IFileSystem FileSystem { get; }

    public ReadOnlyFileSystem(IFileSystem fileSystem)
    {
        FileSystem = fileSystem;
    }

    public void Dispose()
    {
        FileSystem.Dispose();
        GC.SuppressFinalize(this);
    }

    public IEnumerable<FileSystemPath> GetEntities(FileSystemPath path)
    {
        return FileSystem.GetEntities(path);
    }

    public bool Exists(FileSystemPath path)
    {
        return FileSystem.Exists(path);
    }

    public Stream OpenFile(FileSystemPath path, FileAccess access)
    {
        if (access != FileAccess.Read)
            throw new InvalidOperationException("This is a read-only filesystem.");
        return FileSystem.OpenFile(path, access);
    }

    public Stream CreateFile(FileSystemPath path)
    {
        throw new InvalidOperationException("This is a read-only filesystem.");
    }

    public void CreateDirectory(FileSystemPath path)
    {
        throw new InvalidOperationException("This is a read-only filesystem.");
    }

    public void Delete(FileSystemPath path)
    {
        throw new InvalidOperationException("This is a read-only filesystem.");
    }
}
