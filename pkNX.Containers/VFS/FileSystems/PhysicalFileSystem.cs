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

    public IEnumerable<FileSystemPath> GetEntityPaths(FileSystemPath path)
    {
        return GetDirectoryPaths(path).Concat(GetFilePaths(path));
    }

    public IEnumerable<FileSystemPath> GetDirectoryPaths(FileSystemPath path)
    {
        if (!path.IsDirectory)
            throw new ArgumentException("This FileSystemPath is not a directory.", nameof(path));

        var physicalPaths = Directory.GetDirectories(GetPhysicalPath(path));
        return physicalPaths.Select(GetVirtualDirectoryPath);
    }

    public IEnumerable<FileSystemPath> GetFilePaths(FileSystemPath path)
    {
        if (!path.IsDirectory)
            throw new ArgumentException("The specified path is not a directory.", nameof(path));

        var physicalPaths = Directory.GetFiles(GetPhysicalPath(path));
        return physicalPaths.Select(GetVirtualFilePath);
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

    public Stream OpenFile(FileSystemPath path, FileAccess access)
    {
        if (!path.IsFile)
            throw new ArgumentException("The specified path is not a file.", nameof(path));
        return File.Open(GetPhysicalPath(path), FileMode.Open, access);
    }

    public void CreateDirectory(FileSystemPath path)
    {
        if (!path.IsDirectory)
            throw new ArgumentException("The specified path is not a directory.", nameof(path));
        Directory.CreateDirectory(GetPhysicalPath(path));
    }

    public void Delete(FileSystemPath path)
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
