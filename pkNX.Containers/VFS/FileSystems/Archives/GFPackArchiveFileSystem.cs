using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace pkNX.Containers.VFS;

public class GFPackArchiveFileSystem : IFileSystem
{
    public GFPackArchive GfpakArchive { get; }

    public bool IsReadOnly => false;

    public static GFPackArchiveFileSystem Open(Stream s)
    {
        return new GFPackArchiveFileSystem(new GFPackArchive(s, ArchiveOpenMode.Update, true));
    }

    public static GFPackArchiveFileSystem Create(Stream s)
    {
        return new GFPackArchiveFileSystem(new GFPackArchive(s, ArchiveOpenMode.Create, true));
    }

    private GFPackArchiveFileSystem(GFPackArchive archive)
    {
        GfpakArchive = archive;
    }
    public void Dispose()
    {
        GfpakArchive.Dispose();
    }

    protected IEnumerable<GFPackArchiveEntry> GetGfpakEntries()
    {
        return GfpakArchive.Entries.Cast<GFPackArchiveEntry>();
    }
    protected FileSystemPath ToPath(GFPackArchiveEntry entry)
    {
        return FileSystemPath.Parse(FileSystemPath.DirectorySeparator + entry.UniqueIdentifier.ToString("X16"));
    }
    protected string ToEntryPath(FileSystemPath path)
    {
        // Remove heading '/' from path.
        return path.Path.TrimStart(FileSystemPath.DirectorySeparator);
    }

    protected GFPackArchiveEntry? ToEntry(FileSystemPath path)
    {
        return (GFPackArchiveEntry?)GfpakArchive.GetEntry(ToEntryPath(path));
    }

    public IEnumerable<FileSystemPath> GetEntitiesInDirectory(FileSystemPath directory, Func<FileSystemPath, bool>? filter = null)
    {
        var entries = GetGfpakEntries().Select(ToPath).Where(directory.IsParentOf);

        if (filter != null)
            entries = entries.Where(filter);

        return entries.Select(entryPath => entryPath.ParentPath == directory
                ? entryPath
                : directory.AppendDirectory(entryPath.MakeRelativeTo(directory).GetDirectorySegments().First()))
            .Distinct();
    }

    public IEnumerable<FileSystemPath> GetDirectoriesInDirectory(FileSystemPath directory, Func<FileSystemPath, bool>? filter = null)
    {
        if (!directory.IsDirectory)
            throw new ArgumentException("This FileSystemPath is not a directory.", nameof(directory));

        var entries = GetGfpakEntries()
            .Select(ToPath)
            .Where(p => directory.IsParentOf(p) && p.IsDirectory);

        if (filter != null)
            entries = entries.Where(filter);

        return entries.Select(entryPath =>
                entryPath.ParentPath == directory ? entryPath :
                directory.AppendDirectory(entryPath.MakeRelativeTo(directory).GetDirectorySegments().First()))
            .Distinct();
    }

    public IEnumerable<FileSystemPath> GetFilesInDirectory(FileSystemPath directory, Func<FileSystemPath, bool>? filter = null)
    {
        if (!directory.IsDirectory)
            throw new ArgumentException("The specified path is not a directory.", nameof(directory));

        var entries = GetGfpakEntries()
            .Select(ToPath)
            .Where(p => directory.IsParentOf(p) && p.IsFile);

        if (filter != null)
            entries = entries.Where(filter);

        return entries.Select(entryPath => entryPath.ParentPath == directory
                ? entryPath
                : directory.AppendDirectory(entryPath.MakeRelativeTo(directory).GetDirectorySegments().First()))
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
        var entry = GfpakArchive.CreateEntry(ToEntryPath(path));
        return entry.Open();
    }

    public Stream OpenFile(FileSystemPath path, FileMode mode, FileAccess access)
    {
        var entry = GfpakArchive.GetEntry(ToEntryPath(path));
        return entry?.Open() ?? Stream.Null;
    }

    public void CreateDirectory(FileSystemPath path)
    {
        GfpakArchive.CreateEntry(ToEntryPath(path));
    }

    public void Delete(FileSystemPath path, DeleteMode mode = DeleteMode.TopMostLayer)
    {
        var entry = GfpakArchive.GetEntry(ToEntryPath(path));
        entry?.Delete();
    }
}
