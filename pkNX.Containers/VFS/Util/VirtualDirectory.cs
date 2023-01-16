using System;

namespace pkNX.Containers.VFS;

public class VirtualDirectory : FileSystemEntity, IEquatable<VirtualDirectory>
{
    public VirtualDirectory(IFileSystem fileSystem, FileSystemPath path) : base(fileSystem, path)
    {
        if (!path.IsDirectory)
            throw new ArgumentException("The specified path is no directory.", nameof(path));
    }

    public bool Equals(VirtualDirectory? other)
    {
        return ((IEquatable<FileSystemEntity>)this).Equals(other);
    }

    public override bool Equals(object? obj)
    {
        return Equals(obj as VirtualDirectory);
    }
}
