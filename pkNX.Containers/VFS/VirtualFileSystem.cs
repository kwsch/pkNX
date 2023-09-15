using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;

namespace pkNX.Containers.VFS;

public record MountPoint : IComparable<MountPoint>
{
    public FileSystemPath MountPath { get; }
    public IFileSystem FileSystem { get; }

    public MountPoint(FileSystemPath mountPath, IFileSystem fileSystem)
    {
        MountPath = mountPath;
        FileSystem = fileSystem.AsRelativeFileSystem(
            path => path.IsRoot ? path : path.MakeRelativeTo(MountPath),
            path => MountPath.AppendPath(path)
            );
    }

    public int CompareTo(MountPoint? other)
    {
        if (other == null)
            return 1;

        return MountPath.CompareTo(other.MountPath);
    }
}

public class VirtualFileSystem : IFileSystem
{
    public static VirtualFileSystem Current { get; private set; } = null!;
    public bool IsReadOnly => _mounts.All(x => x.FileSystem.IsReadOnly);

    private readonly SortedSet<MountPoint> _mounts;

    public VirtualFileSystem(IEnumerable<MountPoint> mounts) :
        this(mounts.ToArray())
    {
    }

    public VirtualFileSystem(params MountPoint[] mounts)
    {
        _mounts = new(mounts);
        Current = this;
    }

    public void Dispose()
    {
        foreach (var fs in _mounts.Select(x => x.FileSystem))
            fs.Dispose();

        GC.SuppressFinalize(this);
    }

    protected MountPoint GetMountPoint(FileSystemPath path)
    {
        return _mounts.First(mount => mount.MountPath == path || mount.MountPath.IsParentOf(path));
    }

    public bool IsMounted(FileSystemPath mountPath)
    {
        return _mounts.Any(mount => mount.MountPath == mountPath || mount.MountPath.IsParentOf(mountPath));
    }

    public void Mount(MountPoint mountPoint)
    {
        _mounts.Add(mountPoint);
    }

    public void UnMount(MountPoint mountPoint)
    {
        _mounts.Remove(mountPoint);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IEnumerable<FileSystemPath> GetEntitiesInDirectory(FileSystemPath directory, Func<FileSystemPath, bool>? filter = null)
    {
        var mount = GetMountPoint(directory);
        return mount.FileSystem.GetEntitiesInDirectory(directory, filter);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IEnumerable<FileSystemPath> GetDirectoriesInDirectory(FileSystemPath directory, Func<FileSystemPath, bool>? filter = null)
    {
        var mount = GetMountPoint(directory);
        return mount.FileSystem.GetDirectoriesInDirectory(directory, filter);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IEnumerable<FileSystemPath> GetFilesInDirectory(FileSystemPath directory, Func<FileSystemPath, bool>? filter = null)
    {
        var mount = GetMountPoint(directory);
        return mount.FileSystem.GetFilesInDirectory(directory, filter);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Exists(FileSystemPath path)
    {
        var mount = GetMountPoint(path);
        return mount.FileSystem.Exists(path);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Stream OpenFile(FileSystemPath path, FileMode mode = FileMode.Open, FileAccess access = FileAccess.Read)
    {
        var mount = GetMountPoint(path);
        return mount.FileSystem.OpenFile(path, mode, access);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Stream OpenWrite(FileSystemPath path) => OpenFile(path, FileMode.OpenOrCreate, FileAccess.Write);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Stream CreateFile(FileSystemPath path) => OpenFile(path, FileMode.Create, FileAccess.Write);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void CreateDirectory(FileSystemPath path)
    {
        var mount = GetMountPoint(path);
        mount.FileSystem.CreateDirectory(path);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Delete(FileSystemPath path, DeleteMode mode = DeleteMode.TopMostLayer)
    {
        var mount = GetMountPoint(path);
        mount.FileSystem.Delete(path, mode);
    }
}
