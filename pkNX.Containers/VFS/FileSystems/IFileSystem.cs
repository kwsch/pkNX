using System.IO;
using System.Collections.Generic;
using System;
using System.Linq;

namespace pkNX.Containers.VFS;

public enum DeleteMode
{
    TopMostLayer,
    TopMostWriteableLayer,
    AllWritable,
    All
}

public interface IFileSystem : IDisposable
{
    bool IsReadOnly => false;

    IEnumerable<FileSystemPath> GetEntitiesInDirectory(FileSystemPath directory, Func<FileSystemPath, bool>? filter = null);
    IEnumerable<FileSystemPath> GetDirectoriesInDirectory(FileSystemPath directory, Func<FileSystemPath, bool>? filter = null);
    IEnumerable<FileSystemPath> GetFilesInDirectory(FileSystemPath directory, Func<FileSystemPath, bool>? filter = null);

    /// <summary>
    /// Checks if the specified path exists in the filesystem.
    /// </summary>
    /// <param name="path">The path to check.</param>
    /// <returns>True if the path exists, false otherwise.</returns>
    bool Exists(FileSystemPath path);

    /// <summary>
    /// Opens a stream to the location of the specified file with the specified mode and access.
    /// </summary>
    /// <param name="path">The path and name of the file to open.</param>
    /// <param name="mode">A FileMode value that specifies whether a file is created if one does not exist, and determines whether the contents of existing files are retained or overwritten.</param>
    /// <param name="access">The access mode to use when opening the file.</param>
    /// <returns>A stream to the location of the opened file.</returns>
    Stream OpenFile(FileSystemPath path, FileMode mode = FileMode.Open, FileAccess access = FileAccess.Read);

    /// <summary>
    /// Creates or overwrites a file in the specified path
    /// </summary>
    /// <param name="path">The path and name of the file to create.</param>
    /// <returns>A stream to the location of the new file.</returns>
    public Stream CreateFile(FileSystemPath path) => OpenFile(path, FileMode.Create, FileAccess.Write);

    /// <summary>
    /// Opens a stream to the location of the specified file using FileMode.OpenOrCreate and FileAccess.Write.
    /// </summary>
    /// <param name="path">The path and name of the file to open.</param>
    /// <returns>A stream to the location of the opened file.</returns>
    public Stream OpenWrite(FileSystemPath path) => OpenFile(path, FileMode.OpenOrCreate, FileAccess.Write);

    /// <summary>
    /// Creates a directory in the specified path.
    /// </summary>
    /// <param name="path">The path and name of the directory to create.</param>
    void CreateDirectory(FileSystemPath path);

    /// <summary>
    /// Deletes the specified file or directory. Does not throw an exception if the specified file or directory does not exist.
    /// </summary>
    /// <param name="path">The path and name of the file or directory to delete.</param>
    /// <param name="mode">The method to use when deleting the file or directory.</param>
    void Delete(FileSystemPath path, DeleteMode mode = DeleteMode.TopMostLayer);

    public void Move(FileSystemPath sourcePath, IFileSystem destinationFileSystem, FileSystemPath destinationPath)
    {
        throw new NotImplementedException();
    }

    public void Copy(FileSystemPath sourcePath, IFileSystem destinationFileSystem, FileSystemPath destinationPath)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<FileSystemPath> GetEntitiesRecursive(FileSystemPath path, Func<FileSystemPath, bool>? filter = null)
    {
        if (!path.IsDirectory)
            throw new ArgumentException("The specified path is not a directory.");

        foreach (var entity in GetEntitiesInDirectory(path, filter))
        {
            yield return entity;

            if (!entity.IsDirectory)
                continue;

            foreach (var subEntity in GetEntitiesRecursive(entity, filter))
                yield return subEntity;
        }
    }

    public void CreateDirectoryRecursive(FileSystemPath path)
    {
        if (!path.IsDirectory)
            throw new ArgumentException("The specified path is not a directory.");

        var currentDirectoryPath = FileSystemPath.Root;
        foreach (var dirName in path.GetDirectorySegments())
        {
            currentDirectoryPath = currentDirectoryPath.AppendDirectory(dirName);

            if (!Exists(currentDirectoryPath))
                CreateDirectory(currentDirectoryPath);
        }
    }

    public string ReadAllText(FileSystemPath path)
    {
        if (!Exists(path))
            return string.Empty;

        using var stream = OpenFile(path);
        using var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }

    public void WriteAllText(FileSystemPath path, string content)
    {
        using var stream = OpenWrite(path);
        using var writer = new StreamWriter(stream);
        writer.Write(content);
    }
}

public static class IFileSystemExtensions
{
    public static ReadOnlyFileSystem AsReadOnlyFileSystem(this IFileSystem self) => new(self);

    public static RelativeFileSystem AsRelativeFileSystem(this IFileSystem self, PathTransformation toAbsolutePath, PathTransformation toRelativePath)
    {
        return new(self, toAbsolutePath, toRelativePath);
    }

    public static IEnumerable<IFileSystemEntity> GetEntities(this IFileSystem self, FileSystemPath path, Func<FileSystemPath, bool>? filter = null)
    {
        return self.GetEntitiesInDirectory(path, filter).Select(p => IFileSystemEntity.Create(self, p)).OrderBy(x => x.Path);
    }

    public static IEnumerable<VirtualDirectory> GetDirectories(this IFileSystem self, FileSystemPath path, Func<FileSystemPath, bool>? filter = null)
    {
        return self.GetDirectoriesInDirectory(path, filter).Select(p => VirtualDirectory.Create(self, p)).OrderBy(x => x.Path);
    }

    public static IEnumerable<VirtualFile> GetFiles(this IFileSystem self, FileSystemPath path, Func<FileSystemPath, bool>? filter = null)
    {
        return self.GetFilesInDirectory(path, filter).Select(p => VirtualFile.Create(self, p)).OrderBy(x => x.Path);
    }
}
