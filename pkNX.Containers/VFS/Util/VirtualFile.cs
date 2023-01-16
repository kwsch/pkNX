using System;

namespace pkNX.Containers.VFS;

public class VirtualFile : FileSystemEntity, IEquatable<VirtualFile>
{
    public VirtualFile(IFileSystem fileSystem, FileSystemPath path) :
        base(fileSystem, path)
    {
        if (!path.IsFile)
            throw new ArgumentException("The specified path is no file.", nameof(path));
    }

    public bool Equals(VirtualFile? other)
    {
        return ((IEquatable<FileSystemEntity>)this).Equals(other);
    }

    public override bool Equals(object? obj)
    {
        return Equals(obj as VirtualFile);
    }
}

