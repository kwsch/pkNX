using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace pkNX.Containers.VFS;

public class GfpakArchiveFileSystem : IFileSystem
{
    public GFPack GfpakArchive { get; }

    public bool IsReadOnly => false;

    public static GfpakArchiveFileSystem Open(Stream s)
    {
        return new GfpakArchiveFileSystem(new GfpakArchive(s, GfpakArchiveMode.Update, true));
    }

    public static GfpakArchiveFileSystem Create(Stream s)
    {
        return new GfpakArchiveFileSystem(new GfpakArchive(s, GfpakArchiveMode.Create, true));
    }

    private GfpakArchiveFileSystem(GFPack archive)
    {
        GfpakArchive = archive;
    }
    public void Dispose()
    {
        GfpakArchive.Dispose();
    }

    protected IEnumerable<GfpakArchiveEntry> GetGfpakEntries()
    {
        return GfpakArchive.Entries;
    }
    protected FileSystemPath ToPath(GfpakArchiveEntry entry)
    {
        return FileSystemPath.Parse(FileSystemPath.DirectorySeparator + entry.FullName);
    }
    protected string ToEntryPath(FileSystemPath path)
    {
        // Remove heading '/' from path.
        return path.Path.TrimStart(FileSystemPath.DirectorySeparator);
    }

    protected GfpakArchiveEntry? ToEntry(FileSystemPath path)
    {
        return GfpakArchive.GetEntry(ToEntryPath(path));
    }

    public IEnumerable<FileSystemPath> GetEntityPaths(FileSystemPath path)
    {
        return GetGfpakEntries()
            .Select(ToPath)
            .Where(path.IsParentOf)
            .Select(entryPath => entryPath.ParentPath == path
                ? entryPath
                : path.AppendDirectory(entryPath.MakeRelativeTo(path).GetDirectorySegments().First()))
            .Distinct();
    }

    public IEnumerable<FileSystemPath> GetDirectoryPaths(FileSystemPath path)
    {
        if (!path.IsDirectory)
            throw new ArgumentException("This FileSystemPath is not a directory.", nameof(path));

        return GetGfpakEntries()
            .Select(ToPath)
            .Where(p => path.IsParentOf(p) && p.IsDirectory)
            .Select(entryPath => entryPath.ParentPath == path
                ? entryPath
                : path.AppendDirectory(entryPath.MakeRelativeTo(path).GetDirectorySegments().First()))
            .Distinct();
    }

    public IEnumerable<FileSystemPath> GetFilePaths(FileSystemPath path)
    {
        if (!path.IsDirectory)
            throw new ArgumentException("The specified path is not a directory.", nameof(path));

        return GetGfpakEntries()
            .Select(ToPath)
            .Where(p => path.IsParentOf(p) && p.IsFile)
            .Select(entryPath => entryPath.ParentPath == path
                ? entryPath
                : path.AppendDirectory(entryPath.MakeRelativeTo(path).GetDirectorySegments().First()))
            .Distinct();
    }

    public bool Exists(FileSystemPath path)
    {
        if (path.IsFile)
            return ToEntry(path) != null;

        return GetGfpakEntries()
            .Select(ToPath)
            .Any(entryPath => entryPath.IsChildOf(path) || entryPath.Equals(path));
    }

    public Stream CreateFile(FileSystemPath path)
    {
        var zae = GfpakArchive.CreateEntry(ToEntryPath(path));
        return zae.Open();
    }

    public Stream OpenFile(FileSystemPath path, FileAccess access)
    {
        var entry = GfpakArchive.GetEntry(ToEntryPath(path));
        return entry?.Open() ?? Stream.Null;
    }

    public void CreateDirectory(FileSystemPath path)
    {
        GfpakArchive.CreateEntry(ToEntryPath(path));
    }

    public void Delete(FileSystemPath path)
    {
        var entry = GfpakArchive.GetEntry(ToEntryPath(path));
        entry?.Delete();
    }
}
