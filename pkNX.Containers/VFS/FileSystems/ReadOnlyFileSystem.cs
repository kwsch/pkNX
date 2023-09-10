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
    public IEnumerable<FileSystemPath> GetEntitiesInDirectory(FileSystemPath directory, Func<FileSystemPath, bool>? filter = null)
    {
        return FileSystem.GetEntitiesInDirectory(directory, filter);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IEnumerable<FileSystemPath> GetDirectoriesInDirectory(FileSystemPath directory, Func<FileSystemPath, bool>? filter = null)
    {
        return FileSystem.GetDirectoriesInDirectory(directory, filter);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IEnumerable<FileSystemPath> GetFilesInDirectory(FileSystemPath directory, Func<FileSystemPath, bool>? filter = null)
    {
        return FileSystem.GetFilesInDirectory(directory, filter);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Exists(FileSystemPath path)
    {
        return FileSystem.Exists(path);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Stream OpenFile(FileSystemPath path, FileMode mode = FileMode.Open, FileAccess access = FileAccess.Read)
    {
        if (access != FileAccess.Read)
            throw new UnauthorizedAccessException("This is a read-only filesystem.");
        return FileSystem.OpenFile(path, mode, access);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Stream CreateFile(FileSystemPath path)
    {
        throw new UnauthorizedAccessException("This is a read-only filesystem.");
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void CreateDirectory(FileSystemPath path)
    {
        throw new UnauthorizedAccessException("This is a read-only filesystem.");
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Delete(FileSystemPath path, DeleteMode mode = DeleteMode.TopMostLayer)
    {
        throw new UnauthorizedAccessException("This is a read-only filesystem.");
    }
}
