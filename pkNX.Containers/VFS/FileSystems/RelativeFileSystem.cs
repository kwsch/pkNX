using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;

namespace pkNX.Containers.VFS;

public delegate FileSystemPath PathTransformation(FileSystemPath arg);

public class RelativeFileSystem : IFileSystem
{
    public IFileSystem FileSystem { get; }
    public bool IsReadOnly => FileSystem.IsReadOnly;

    public PathTransformation ToAbsolutePath { get; }
    public PathTransformation ToRelativePath { get; }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public RelativeFileSystem(IFileSystem fileSystem, PathTransformation toAbsolutePath, PathTransformation toRelativePath)
    {
        FileSystem = fileSystem;
        ToAbsolutePath = toAbsolutePath;
        ToRelativePath = toRelativePath;
    }

    public void Dispose()
    {
        FileSystem.Dispose();
        GC.SuppressFinalize(this);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IEnumerable<FileSystemPath> GetEntityPaths(FileSystemPath path, Func<FileSystemPath, bool>? filter = null)
    {
        return FileSystem.GetEntityPaths(ToAbsolutePath(path), filter)
            .Select(p => ToRelativePath(p));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IEnumerable<FileSystemPath> GetDirectoryPaths(FileSystemPath path, Func<FileSystemPath, bool>? filter = null)
    {
        return FileSystem.GetDirectoryPaths(ToAbsolutePath(path), filter)
            .Select(p => ToRelativePath(p));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IEnumerable<FileSystemPath> GetFilePaths(FileSystemPath path, Func<FileSystemPath, bool>? filter = null)
    {
        return FileSystem.GetFilePaths(ToAbsolutePath(path), filter)
            .Select(p => ToRelativePath(p));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Exists(FileSystemPath path)
    {
        return FileSystem.Exists(ToAbsolutePath(path));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Stream OpenFile(FileSystemPath path, FileAccess access)
    {
        return FileSystem.OpenFile(ToAbsolutePath(path), access);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Stream CreateFile(FileSystemPath path)
    {
        return FileSystem.CreateFile(ToAbsolutePath(path));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void CreateDirectory(FileSystemPath path)
    {
        FileSystem.CreateDirectory(ToAbsolutePath(path));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Delete(FileSystemPath path)
    {
        FileSystem.Delete(ToAbsolutePath(path));
    }
}
