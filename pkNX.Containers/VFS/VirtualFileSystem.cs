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

    public FileSystemPath AddMountPoint(FileSystemPath path)
    {
        return MountPath.AppendPath(path);
    }

    public FileSystemPath RemoveMountPoint(FileSystemPath path)
    {
        return path.IsRoot ? path : path.MakeRelativeTo(MountPath);
    }

    public MountPoint(FileSystemPath mountPath, IFileSystem fileSystem)
    {
        MountPath = mountPath;
        FileSystem = fileSystem.AsRelativeFileSystem(RemoveMountPoint, AddMountPoint);
    }
}

public class VirtualFileSystem : IFileSystem
{
    public static VirtualFileSystem Current { get; private set; } = null!;

    public SortedSet<MountPoint> Mounts { get; }
    public bool IsReadOnly => Mounts.All(x => x.FileSystem.IsReadOnly);

    public VirtualFileSystem(IEnumerable<MountPoint> mounts)
    {
        Mounts = new SortedSet<MountPoint>(mounts);
        Current = this;
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
