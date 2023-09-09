namespace pkNX.Containers.VFS;

public interface IFileSystemEntity
{
    IFileSystem FileSystem { get; }
    FileSystemPath Path { get; }
    string Name { get; }
    VirtualDirectory ParentDirectory { get; }

    public void CopyTo(IFileSystem destinationFileSystem, FileSystemPath destinationPath)
    {
        FileSystem.Copy(Path, destinationFileSystem, destinationPath);
    }

    public void MoveTo(IFileSystem destinationFileSystem, FileSystemPath destinationPath)
    {
        FileSystem.Move(Path, destinationFileSystem, destinationPath);
    }

    public void Delete(DeleteMode mode = DeleteMode.TopMostLayer)
    {
        FileSystem.Delete(Path, mode);
    }

    public void Exists()
    {
        FileSystem.Exists(Path);
    }

    internal static IFileSystemEntity Create(IFileSystem fileSystem, FileSystemPath path)
    {
        if (path.IsFile)
            return VirtualFile.Create(fileSystem, path);

        return VirtualDirectory.Create(fileSystem, path);
    }
}

