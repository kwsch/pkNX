using System.IO;
using System.Collections.Generic;
using System;
using System.Linq;

namespace pkNX.Containers.VFS;

public interface IFileSystem : IDisposable
{
    IEnumerable<FileSystemPath> GetEntityPaths(FileSystemPath path, Func<FileSystemPath, bool>? filter = null);
    IEnumerable<FileSystemPath> GetDirectoryPaths(FileSystemPath path, Func<FileSystemPath, bool>? filter = null);
    IEnumerable<FileSystemPath> GetFilePaths(FileSystemPath path, Func<FileSystemPath, bool>? filter = null);

    bool Exists(FileSystemPath path);
    Stream CreateFile(FileSystemPath path);
    Stream OpenFile(FileSystemPath path, FileAccess access);
    void CreateDirectory(FileSystemPath path);
    void Delete(FileSystemPath path);

    bool IsReadOnly => false;

    public string ReadAllText(FileSystemPath path)
    {
        if (!Exists(path))
            return string.Empty;

        using var stream = OpenFile(path, FileAccess.Read);
        using var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }

    public void WriteAllText(FileSystemPath path, string content)
    {
        if (!Exists(path))
        {
            CreateFile(path);
        }

        using var stream = OpenFile(path, FileAccess.Write);
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
        return self.GetEntityPaths(path, filter).Select(p => IFileSystemEntity.Create(self, p));
    }

    public static IEnumerable<VirtualDirectory> GetDirectories(this IFileSystem self, FileSystemPath path, Func<FileSystemPath, bool>? filter = null)
    {
        return self.GetDirectoryPaths(path, filter).Select(p => VirtualDirectory.Create(self, p));
    }

    public static IEnumerable<VirtualFile> GetFiles(this IFileSystem self, FileSystemPath path, Func<FileSystemPath, bool>? filter = null)
    {
        return self.GetFilePaths(path, filter).Select(p => VirtualFile.Create(self, p));
    }
}
