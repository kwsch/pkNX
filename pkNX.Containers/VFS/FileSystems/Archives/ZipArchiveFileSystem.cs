using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.IO.Compression;

namespace pkNX.Containers.VFS;

public class ZipArchiveFileSystem : IFileSystem
{
    public ZipArchive ZipArchive { get; private set; }

    public bool IsReadOnly => false;

    public static ZipArchiveFileSystem Open(Stream s)
    {
        return new ZipArchiveFileSystem(new ZipArchive(s, ZipArchiveMode.Update, true));
    }

    public static ZipArchiveFileSystem Create(Stream s)
    {
        return new ZipArchiveFileSystem(new ZipArchive(s, ZipArchiveMode.Create, true));
    }

    private ZipArchiveFileSystem(ZipArchive archive)
    {
        ZipArchive = archive;
    }
    public void Dispose()
    {
        ZipArchive.Dispose();
    }

    protected IEnumerable<ZipArchiveEntry> GetZipEntries()
    {
        return ZipArchive.Entries;
    }
    protected FileSystemPath ToPath(ZipArchiveEntry entry)
    {
        return FileSystemPath.Parse(FileSystemPath.DirectorySeparator + entry.FullName);
    }
    protected string ToEntryPath(FileSystemPath path)
    {
        // Remove heading '/' from path.
        return path.Path.TrimStart(FileSystemPath.DirectorySeparator);
    }

    protected ZipArchiveEntry? ToEntry(FileSystemPath path)
    {
        return ZipArchive.GetEntry(ToEntryPath(path));
    }
    public IEnumerable<FileSystemPath> GetEntities(FileSystemPath path)
    {
        return GetZipEntries().Select(ToPath).Where(path.IsParentOf)
            .Select(entryPath => entryPath.ParentPath == path
               ? entryPath
               : path.AppendDirectory(entryPath.RemoveParent(path).GetDirectorySegments().First()))
            .Distinct()
            .ToList();
    }

    public bool Exists(FileSystemPath path)
    {
        if (path.IsFile)
            return ToEntry(path) != null;
        return GetZipEntries()
            .Select(ToPath)
            .Any(entryPath => entryPath.IsChildOf(path) || entryPath.Equals(path));
    }

    public Stream CreateFile(FileSystemPath path)
    {
        var zae = ZipArchive.CreateEntry(ToEntryPath(path));
        return zae.Open();
    }

    public Stream OpenFile(FileSystemPath path, FileAccess access)
    {
        var entry = ZipArchive.GetEntry(ToEntryPath(path));
        return entry?.Open() ?? Stream.Null;
    }

    public void CreateDirectory(FileSystemPath path)
    {
        ZipArchive.CreateEntry(ToEntryPath(path));
    }

    public void Delete(FileSystemPath path)
    {
        var entry = ZipArchive.GetEntry(ToEntryPath(path));
        entry?.Delete();
    }
}
