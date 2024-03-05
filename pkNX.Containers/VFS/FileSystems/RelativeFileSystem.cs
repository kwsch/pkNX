using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;

namespace pkNX.Containers.VFS;

public delegate FileSystemPath PathTransformation(FileSystemPath arg);

[method: MethodImpl(MethodImplOptions.AggressiveInlining)]
public class RelativeFileSystem(
    IFileSystem fileSystem,
    PathTransformation toAbsolutePath,
    PathTransformation toRelativePath)
    : IFileSystem
{
    public IFileSystem FileSystem { get; } = fileSystem;
    public bool IsReadOnly => FileSystem.IsReadOnly;

    public PathTransformation ToAbsolutePath { get; } = toAbsolutePath;
    public PathTransformation ToRelativePath { get; } = toRelativePath;

    public void Dispose()
    {
        FileSystem.Dispose();
        GC.SuppressFinalize(this);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IEnumerable<FileSystemPath> GetEntitiesInDirectory(FileSystemPath directory, Func<FileSystemPath, bool>? filter = null)
    {
        return FileSystem.GetEntitiesInDirectory(ToAbsolutePath(directory), filter)
            .Select(p => ToRelativePath(p));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IEnumerable<FileSystemPath> GetDirectoriesInDirectory(FileSystemPath directory, Func<FileSystemPath, bool>? filter = null)
    {
        return FileSystem.GetDirectoriesInDirectory(ToAbsolutePath(directory), filter)
            .Select(p => ToRelativePath(p));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IEnumerable<FileSystemPath> GetFilesInDirectory(FileSystemPath directory, Func<FileSystemPath, bool>? filter = null)
    {
        return FileSystem.GetFilesInDirectory(ToAbsolutePath(directory), filter)
            .Select(p => ToRelativePath(p));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Exists(FileSystemPath path)
    {
        return FileSystem.Exists(ToAbsolutePath(path));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Stream OpenFile(FileSystemPath path, FileMode mode = FileMode.Open, FileAccess access = FileAccess.Read)
    {
        return FileSystem.OpenFile(ToAbsolutePath(path), mode, access);
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
    public void Delete(FileSystemPath path, DeleteMode mode = DeleteMode.TopMostLayer)
    {
        FileSystem.Delete(ToAbsolutePath(path), mode);
    }
}
