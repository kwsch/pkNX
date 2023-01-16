using System.IO;
using System.Collections.Generic;
using System;

namespace pkNX.Containers.VFS;

public interface IFileSystem : IDisposable
{
    IEnumerable<FileSystemPath> GetEntities(FileSystemPath path);
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
}
