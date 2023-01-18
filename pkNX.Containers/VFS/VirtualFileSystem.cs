using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;

namespace pkNX.Containers.VFS;

public record MountPoint
{
    public FileSystemPath MountPath { get; }
    public IFileSystem FileSystem { get; }

    public FileSystemPath ToAbsolutePath(FileSystemPath path)
    {
        return MountPath.AppendPath(path);
    }

    public FileSystemPath ToRelativePath(FileSystemPath path)
    {
        return path.IsRoot ? path : path.MakeRelativeTo(MountPath);
    }

    public MountPoint(FileSystemPath mountPath, IFileSystem fileSystem)
    {
        MountPath = mountPath;
        FileSystem = fileSystem.AsRelativeFileSystem(ToAbsolutePath, ToRelativePath);
    }
}

public class VirtualFileSystem : IFileSystem
{
    public SortedSet<MountPoint> Mounts { get; }
    public bool IsReadOnly => Mounts.All(x => x.FileSystem.IsReadOnly);

    public VirtualFileSystem(IEnumerable<MountPoint> mounts)
    {
        Mounts = new SortedSet<MountPoint>(mounts);
    }

    public VirtualFileSystem(params MountPoint[] mounts) :
        this(mounts.AsEnumerable())
    { }

    public void Dispose()
    {
        foreach (var fs in Mounts.Select(x => x.FileSystem))
            fs.Dispose();

        GC.SuppressFinalize(this);
    }

    protected MountPoint GetMountPoint(FileSystemPath path)
    {
        return Mounts.First(mount => mount.MountPath == path || mount.MountPath.IsParentOf(path));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IEnumerable<FileSystemPath> GetEntityPaths(FileSystemPath path)
    {
        var mount = GetMountPoint(path);
        return mount.FileSystem.GetEntityPaths(path);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IEnumerable<FileSystemPath> GetDirectoryPaths(FileSystemPath path)
    {
        var mount = GetMountPoint(path);
        return mount.FileSystem.GetDirectoryPaths(path);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IEnumerable<FileSystemPath> GetFilePaths(FileSystemPath path)
    {
        var mount = GetMountPoint(path);
        return mount.FileSystem.GetFilePaths(path);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Exists(FileSystemPath path)
    {
        var mount = GetMountPoint(path);
        return mount.FileSystem.Exists(path);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Stream CreateFile(FileSystemPath path)
    {
        var mount = GetMountPoint(path);
        return mount.FileSystem.CreateFile(path);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Stream OpenFile(FileSystemPath path, FileAccess access)
    {
        var mount = GetMountPoint(path);
        return mount.FileSystem.OpenFile(path, access);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void CreateDirectory(FileSystemPath path)
    {
        var mount = GetMountPoint(path);
        mount.FileSystem.CreateDirectory(path);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Delete(FileSystemPath path)
    {
        var mount = GetMountPoint(path);
        mount.FileSystem.Delete(path);
    }
}
