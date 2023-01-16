using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace pkNX.Containers.VFS;

public class LayeredFileSystem : IFileSystem
{
    public IEnumerable<IFileSystem> FileSystems { get; }
    public LayeredFileSystem(IEnumerable<IFileSystem> fileSystems)
    {
        FileSystems = fileSystems.ToArray();
    }

    public LayeredFileSystem(params IFileSystem[] fileSystems)
    {
        FileSystems = fileSystems.ToArray();
    }

    public void Dispose()
    {
        foreach (var fs in FileSystems)
            fs.Dispose();

        GC.SuppressFinalize(this);
    }

    public IEnumerable<FileSystemPath> GetEntities(FileSystemPath path)
    {
        var entities = new SortedList<FileSystemPath, FileSystemPath>();
        foreach (var fs in FileSystems.Where(fs => fs.Exists(path)))
        {
            foreach (var entity in fs.GetEntities(path))
            {
                if (!entities.ContainsKey(entity))
                    entities.Add(entity, entity);
            }
        }
        return entities.Values;
    }

    public bool Exists(FileSystemPath path)
    {
        return FileSystems.Any(fs => fs.Exists(path));
    }

    public IFileSystem? GetFirst(FileSystemPath path)
    {
        return FileSystems.FirstOrDefault(fs => fs.Exists(path));
    }

    public Stream CreateFile(FileSystemPath path)
    {
        IFileSystem fs = GetFirst(path) ?? FileSystems.First();
        return fs.CreateFile(path);
    }

    public Stream OpenFile(FileSystemPath path, FileAccess access)
    {
        IFileSystem? fs = GetFirst(path);
        if (fs == null)
            throw new FileNotFoundException();
        return fs.OpenFile(path, access);
    }

    public void CreateDirectory(FileSystemPath path)
    {
        if (Exists(path))
            throw new ArgumentException("The specified directory already exists.");
        IFileSystem? fs = GetFirst(path.ParentPath);
        if (fs == null)
            throw new ArgumentException("The directory-parent does not exist.");
        fs.CreateDirectory(path);
    }

    public void Delete(FileSystemPath path)
    {
        foreach (var fs in FileSystems.Where(fs => fs.Exists(path)))
            fs.Delete(path);
    }
}
