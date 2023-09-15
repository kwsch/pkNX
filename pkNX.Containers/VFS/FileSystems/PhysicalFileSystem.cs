using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace pkNX.Containers.VFS;

public class PhysicalFileSystem : IFileSystem
{
    public string PhysicalRoot { get; }

    public PhysicalFileSystem(string physicalRoot)
    {
        physicalRoot = Path.GetFullPath(physicalRoot);
        if (!physicalRoot.EndsWith(Path.DirectorySeparatorChar))
            physicalRoot += Path.DirectorySeparatorChar;
        PhysicalRoot = physicalRoot;

        if (!Directory.Exists(PhysicalRoot))
            throw new ArgumentException("The specified path does not exist.", nameof(physicalRoot));
    }

    public string GetPhysicalPath(FileSystemPath path)
    {
        return Path.GetFullPath(PhysicalRoot + path);
    }

    public FileSystemPath GetVirtualFilePath(string physicalPath)
    {
        if (!physicalPath.StartsWith(PhysicalRoot, StringComparison.InvariantCultureIgnoreCase))
            throw new ArgumentException("The specified path is not member of the PhysicalRoot.", nameof(physicalPath));

        string virtualPath = FileSystemPath.DirectorySeparator + physicalPath[PhysicalRoot.Length..].Replace(Path.DirectorySeparatorChar, FileSystemPath.DirectorySeparator);
        return FileSystemPath.Parse(virtualPath);
    }

    public FileSystemPath GetVirtualDirectoryPath(string physicalPath)
    {
        if (!physicalPath.StartsWith(PhysicalRoot, StringComparison.InvariantCultureIgnoreCase))
            throw new ArgumentException("The specified path is not member of the PhysicalRoot.", nameof(physicalPath));

        string virtualPath = FileSystemPath.DirectorySeparator + physicalPath[PhysicalRoot.Length..].Replace(Path.DirectorySeparatorChar, FileSystemPath.DirectorySeparator);
        if (!virtualPath.EndsWith(FileSystemPath.DirectorySeparator))
            virtualPath += FileSystemPath.DirectorySeparator;
        return FileSystemPath.Parse(virtualPath);
    }

    public IEnumerable<FileSystemPath> GetEntitiesInDirectory(FileSystemPath directory, Func<FileSystemPath, bool>? filter = null)
    {
        return GetDirectoriesInDirectory(directory, filter).Concat(GetFilesInDirectory(directory, filter));
    }

    public IEnumerable<FileSystemPath> GetDirectoriesInDirectory(FileSystemPath directory, Func<FileSystemPath, bool>? filter = null)
    {
        if (!directory.IsDirectory)
            throw new ArgumentException("This FileSystemPath is not a directory.", nameof(directory));

        var physicalPaths = Directory.GetDirectories(GetPhysicalPath(directory));
        var virtualPaths = physicalPaths.Select(GetVirtualDirectoryPath);

        if (filter == null)
            return virtualPaths;

        return virtualPaths.Where(filter);
    }

    public IEnumerable<FileSystemPath> GetFilesInDirectory(FileSystemPath directory, Func<FileSystemPath, bool>? filter = null)
    {
        if (!directory.IsDirectory)
            throw new ArgumentException("The specified path is not a directory.", nameof(directory));

        var physicalPaths = Directory.GetFiles(GetPhysicalPath(directory));
        var virtualPaths = physicalPaths.Select(GetVirtualFilePath);

        if (filter == null)
            return virtualPaths;

        return virtualPaths.Where(filter);
    }

    public bool Exists(FileSystemPath path)
    {
        var fullPath = GetPhysicalPath(path);
        return path.IsFile ? File.Exists(fullPath) : Directory.Exists(fullPath);
    }

    public Stream CreateFile(FileSystemPath path)
    {
        if (!path.IsFile)
            throw new ArgumentException("The specified path is not a file.", nameof(path));
        return File.Create(GetPhysicalPath(path));
    }

    public Stream OpenFile(FileSystemPath path, FileMode mode = FileMode.Open, FileAccess access = FileAccess.Read)
    {
        if (!path.IsFile)
            throw new ArgumentException("The specified path is not a file.", nameof(path));
        return File.Open(GetPhysicalPath(path), mode, access);
    }

    public void CreateDirectory(FileSystemPath path)
    {
        if (!path.IsDirectory)
            throw new ArgumentException("The specified path is not a directory.", nameof(path));
        Directory.CreateDirectory(GetPhysicalPath(path));
    }

    public void Delete(FileSystemPath path, DeleteMode mode = DeleteMode.TopMostLayer)
    {
        if (path.IsFile)
            File.Delete(GetPhysicalPath(path));
        else
            Directory.Delete(GetPhysicalPath(path), true);
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }
}
