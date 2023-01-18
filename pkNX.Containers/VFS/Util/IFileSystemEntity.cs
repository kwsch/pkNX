namespace pkNX.Containers.VFS;

public interface IFileSystemEntity
{
    IFileSystem FileSystem { get; }
    FileSystemPath Path { get; }
    string Name { get; }
    VirtualDirectory ParentDirectory { get; }

    internal static IFileSystemEntity Create(IFileSystem fileSystem, FileSystemPath path)
    {
        if (path.IsFile)
            return VirtualFile.Create(fileSystem, path);

        return VirtualDirectory.Create(fileSystem, path);
    }
}

public static class IFileSystemEntityExtensions
{
    public static void Delete(this IFileSystemEntity e)
    {
        e.FileSystem.Delete(e.Path);
    }

    public static void Exists(this IFileSystemEntity e)
    {
        e.FileSystem.Exists(e.Path);
    }
}

