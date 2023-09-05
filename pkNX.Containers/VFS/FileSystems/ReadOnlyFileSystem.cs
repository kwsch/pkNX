using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;

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

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IEnumerable<FileSystemPath> GetEntityPaths(FileSystemPath path, Func<FileSystemPath, bool>? filter = null)
    {
        return FileSystem.GetEntityPaths(path, filter);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IEnumerable<FileSystemPath> GetDirectoryPaths(FileSystemPath path, Func<FileSystemPath, bool>? filter = null)
    {
        return FileSystem.GetDirectoryPaths(path, filter);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IEnumerable<FileSystemPath> GetFilePaths(FileSystemPath path, Func<FileSystemPath, bool>? filter = null)
    {
        return FileSystem.GetFilePaths(path, filter);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Exists(FileSystemPath path)
    {
        return FileSystem.Exists(path);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
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
