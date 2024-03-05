using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace pkNX.Containers.VFS;

public class LayeredFileSystem : IFileSystem
{
    private readonly IFileSystem[] _fileSystems;

    public LayeredFileSystem(IEnumerable<IFileSystem> fileSystems) :
        this(fileSystems.ToArray())
    { }

    public LayeredFileSystem(params IFileSystem[] fileSystems)
    {
        _fileSystems = fileSystems;

        Debug.Assert(_fileSystems.Any(), "At least one file system should be provided.");
        Debug.Assert(_fileSystems.Any(fs => !fs.IsReadOnly), "Should contain at least one writable filesystem.");
    }

    public void Dispose()
    {
        foreach (var fs in _fileSystems)
            fs.Dispose();

        GC.SuppressFinalize(this);
    }

    public IEnumerable<FileSystemPath> GetEntitiesInDirectory(FileSystemPath directory, Func<FileSystemPath, bool>? filter = null)
    {
        if (!directory.IsDirectory)
            throw new ArgumentException("This FileSystemPath is not a directory.", nameof(directory));

        var entities = new HashSet<FileSystemPath>();
        foreach (var fs in _fileSystems.Where(fs => fs.Exists(directory)))
            entities.UnionWith(fs.GetEntitiesInDirectory(directory, filter));
        return entities;
    }

    public IEnumerable<FileSystemPath> GetDirectoriesInDirectory(FileSystemPath directory, Func<FileSystemPath, bool>? filter = null)
    {
        if (!directory.IsDirectory)
            throw new ArgumentException("This FileSystemPath is not a directory.", nameof(directory));

        var directories = new HashSet<FileSystemPath>();
        foreach (var fs in _fileSystems.Where(fs => fs.Exists(directory)))
            directories.UnionWith(fs.GetDirectoriesInDirectory(directory, filter));
        return directories;
    }

    public IEnumerable<FileSystemPath> GetFilesInDirectory(FileSystemPath directory, Func<FileSystemPath, bool>? filter = null)
    {
        if (!directory.IsDirectory)
            throw new ArgumentException("This FileSystemPath is not a directory.", nameof(directory));

        var files = new HashSet<FileSystemPath>();
        foreach (var fs in _fileSystems.Where(fs => fs.Exists(directory)))
            files.UnionWith(fs.GetFilesInDirectory(directory, filter));
        return files;
    }

    public IFileSystem GetFirst()
    {
        return _fileSystems[0];
    }

    public IFileSystem GetFirstWritable()
    {
        return _fileSystems.First(fs => !fs.IsReadOnly);
    }

    public bool Exists(FileSystemPath path)
    {
        return _fileSystems.Any(fs => fs.Exists(path));
    }

    public IFileSystem? GetFirstWhereExists(FileSystemPath path)
    {
        return Array.Find(_fileSystems, fs => fs.Exists(path));
    }

    public IFileSystem? GetFirstWritableWhereExists(FileSystemPath path)
    {
        return Array.Find(_fileSystems, fs => !fs.IsReadOnly && fs.Exists(path));
    }

    private bool ValidateOpenMode(FileMode mode = FileMode.Open, FileAccess access = FileAccess.Read)
    {
        if (!access.HasFlag(FileAccess.Write) && mode is FileMode.Create or FileMode.CreateNew or FileMode.Truncate or FileMode.Append)
        {
            throw new ArgumentException($"File mode '{mode}' requires files to be accessed with write permission, but the access mode was '{access}'", nameof(access));
        }

        if (access.HasFlag(FileAccess.Read) && mode == FileMode.Append)
        {
            throw new ArgumentException("File mode 'Append' requires files to be accessed with in read/write permission.", nameof(access));
        }

        return true;
    }

    public Stream OpenFile(FileSystemPath path, FileMode mode = FileMode.Open, FileAccess access = FileAccess.Read)
    {
        if (!ValidateOpenMode(mode, access))
            return Stream.Null;

        switch (mode)
        {
            case FileMode.Open:
            {
                // just read from top layer
                var fs = GetFirstWhereExists(path) ?? throw new FileNotFoundException($"Could not find the file at the specified path: {path}", nameof(path));
                return fs.OpenFile(path, FileMode.Open, access);
            }
            case FileMode.OpenOrCreate:
            {
                // - Specifies that the operating system should open a file if it exists; otherwise, a new file should be created.
                // If the file is opened with FileAccess.Read, Read permission is required.
                // If the file access is FileAccess.Write, Write permission is required.
                // If the file is opened with FileAccess.ReadWrite, both Read and Write permissions are required.

                IFileSystem? fs = GetFirstWhereExists(path);
                if (fs != null)
                {
                    // The file exists, now we need to check if we can open it with the requested access
                    // readonly fs requires special handling for write requests.
                    // if readonly access is requested, we can open it if the fs is readonly
                    if (!fs.IsReadOnly)
                        return fs.OpenFile(path, FileMode.Open, access);

                    var readStream = fs.OpenFile(path);
                    if (access == FileAccess.Read)
                        return readStream;

                    // For write-only access, we can just create a new empty file
                    IFileSystem writableFs = GetFirstWritable();
                    writableFs.CreateDirectoryRecursive(path.ParentPath);
                    var writeStream = writableFs.OpenFile(path, FileMode.Create, access);

                    if (access == FileAccess.Write)
                        return writeStream;

                    // For read-write access, we need to first copy the file to a writable fs
                    readStream.CopyTo(writeStream);
                    writeStream.Seek(0, SeekOrigin.Begin);
                    readStream.Dispose();
                    return writeStream;
                }

                // The file does not exist, create it on the first writable fs
                return GetFirstWritable().CreateFile(path);
            }
            case FileMode.CreateNew:
            {
                // Specifies that the operating system should create a new file.
                // - Requires Write permission.
                // - Requires the file does not already exist.

                if (Exists(path))
                    throw new IOException($"File {path.Path} already exists.");

                return GetFirstWritable().CreateFile(path);
            }
            case FileMode.Create:
            {
                // - This requires Write permission.
                // - FileMode.Create is equivalent to requesting that if the file does not exist, use CreateNew; otherwise, use Truncate.
                // Specifies that the operating system should create a new file.
                // If the file already exists, it will be overwritten.
                IFileSystem? fs = GetFirstWhereExists(path);
                if (fs != null)
                    return fs.OpenFile(path, FileMode.Truncate, access);

                return GetFirstWritable().CreateFile(path);
            }

            case FileMode.Truncate:
            {
                // - Requires Write permission.
                // - Requires no Read access requested
                // - Requires existing file
                // When the file is opened, it should be truncated so that its size is zero bytes.

                IFileSystem? fs = GetFirstWhereExists(path);
                if (fs == null)
                    throw new FileNotFoundException($"Could not find the file at the specified path: {path}", nameof(path));

                return fs.OpenFile(path, FileMode.Truncate, access);
            }
            case FileMode.Append:
            {
                // Opens the file if it exists and seeks to the end of the file, or creates a new file.
                // This requires R/W permission.
                // Trying to seek to a position before the end of the file throws an IOException exception, and any attempt to read fails and throws a NotSupportedException exception.

                IFileSystem? fs = GetFirstWhereExists(path);
                if (fs == null)
                    throw new FileNotFoundException($"Could not find the file at the specified path: {path}", nameof(path));

                return fs.OpenFile(path, FileMode.Append, access);
            }

            default:
                throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
        }
    }

    public void CreateDirectory(FileSystemPath path)
    {
        if (Exists(path))
            throw new ArgumentException("The specified directory already exists.");
        IFileSystem? fs = GetFirstWhereExists(path.ParentPath);
        if (fs == null)
            throw new ArgumentException("The directory-parent does not exist.");
        fs.CreateDirectory(path);
    }

    public void Delete(FileSystemPath path, DeleteMode mode = DeleteMode.TopMostLayer)
    {
        switch (mode)
        {
            case DeleteMode.TopMostLayer:
                GetFirstWhereExists(path)?.Delete(path);
                break;
            case DeleteMode.TopMostWriteableLayer:
                GetFirstWritableWhereExists(path)?.Delete(path);
                break;
            case DeleteMode.AllWritable:
                foreach (var fs in _fileSystems.Where(fs => !fs.IsReadOnly && fs.Exists(path)))
                    fs.Delete(path);
                break;
            case DeleteMode.All:
                foreach (var fs in _fileSystems.Where(fs => fs.Exists(path)))
                    fs.Delete(path);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
        }
    }
}
