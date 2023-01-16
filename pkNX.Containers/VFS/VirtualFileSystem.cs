using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace pkNX.Containers.VFS;

public record MountPoint(FileSystemPath Path, IFileSystem FileSystem) : IComparable<MountPoint>
{
    public int CompareTo(MountPoint? other)
    {
        return other?.Path.CompareTo(Path) ?? 1;
    }
}

public class VirtualFileSystem : IFileSystem
{
    public bool IsReadOnly => Mounts.All(x => x.FileSystem.IsReadOnly);

    public SortedSet<MountPoint> Mounts { get; }

    public VirtualFileSystem(IEnumerable<MountPoint> mounts)
    {
        Mounts = new SortedSet<MountPoint>(mounts);
    }

    public VirtualFileSystem(params MountPoint[] mounts) :
        this(mounts.AsEnumerable())
    { }

    protected MountPoint Get(FileSystemPath path)
    {
        return Mounts.First(pair => pair.Path == path || pair.Path.IsParentOf(path));
    }

    public void Dispose()
    {
        foreach (var fs in Mounts.Select(x => x.FileSystem))
            fs.Dispose();

        GC.SuppressFinalize(this);
    }

    public IEnumerable<FileSystemPath> GetEntities(FileSystemPath path)
    {
        MountPoint point = Get(path);
        IEnumerable<FileSystemPath> entities = point.FileSystem.GetEntities(path.IsRoot ? path : path.RemoveParent(point.Path));
        return entities.Select(p => point.Path.AppendPath(p));
    }

    public bool Exists(FileSystemPath path)
    {
        var pair = Get(path);
        return pair.FileSystem.Exists(path.RemoveParent(pair.Path));
    }

    public Stream CreateFile(FileSystemPath path)
    {
        var pair = Get(path);
        return pair.FileSystem.CreateFile(path.RemoveParent(pair.Path));
    }

    public Stream OpenFile(FileSystemPath path, FileAccess access)
    {
        var pair = Get(path);
        return pair.FileSystem.OpenFile(path.RemoveParent(pair.Path), access);
    }

    public void CreateDirectory(FileSystemPath path)
    {
        var pair = Get(path);
        pair.FileSystem.CreateDirectory(path.RemoveParent(pair.Path));
    }

    public void Delete(FileSystemPath path)
    {
        var pair = Get(path);
        pair.FileSystem.Delete(path.RemoveParent(pair.Path));
    }
}
