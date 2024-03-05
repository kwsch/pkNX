using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace pkNX.Containers.VFS;

public readonly record struct FileSystemPath : IComparable<FileSystemPath>
{
    public const char DirectorySeparator = '/';
    public static FileSystemPath Root { get; } = new(DirectorySeparator.ToString());

    public string Path { get; }

    public bool IsDirectory => Path.EndsWith(DirectorySeparator);
    public bool IsFile => !IsDirectory;
    public bool IsRoot => Path.Length == 1;

    public string EntityName
    {
        get
        {
            if (IsRoot)
                return string.Empty;

            int endOfName = Path.Length;
            if (IsDirectory)
                --endOfName;

            int startOfName = Path.LastIndexOf(DirectorySeparator, endOfName - 1, endOfName) + 1;
            return Path[startOfName..endOfName];
        }
    }

    public FileSystemPath ParentPath
    {
        get
        {
            if (IsRoot)
                throw new InvalidOperationException("There is no parent of root.");

            int endOfPath = Path.Length;
            if (IsDirectory)
                --endOfPath;

            endOfPath = Path.LastIndexOf(DirectorySeparator, endOfPath - 1, endOfPath) + 1;
            return new(Path[..endOfPath]);
        }
    }

    private FileSystemPath(string path)
    {
        Path = path;
    }

    public static implicit operator FileSystemPath(string path)
    {
        return Parse(path);
    }

    public static implicit operator string(FileSystemPath path)
    {
        return path.ToString();
    }

    public static bool IsRooted(string s)
    {
        return s.StartsWith(DirectorySeparator);
    }

    public static FileSystemPath Parse(string s)
    {
        if (!IsRooted(s))
            throw new UriFormatException($"Could not parse input \"{s}\": Path is not rooted.");
        if (s.Contains(string.Concat(DirectorySeparator, DirectorySeparator)))
            throw new UriFormatException($"Could not parse input \"{s}\": Path contains double directory-separators.");
        return new(s);
    }

    [Pure]
    public FileSystemPath AppendPath(string strPath)
    {
        if (!IsDirectory)
            throw new InvalidOperationException("This FileSystemPath is not a directory.");

        if (IsRooted(strPath))
            throw new ArgumentException("The specified path is a rooted path.", nameof(strPath));

        return new(Path + strPath);
    }

    [Pure]
    public FileSystemPath AppendPath(FileSystemPath path)
    {
        return AppendPath(path.Path[1..]);
    }

    [Pure]
    public FileSystemPath AppendDirectory(string directoryName)
    {
        if (directoryName.Contains(DirectorySeparator.ToString()))
            throw new ArgumentException("The specified name includes directory-separator(s).", nameof(directoryName));
        if (!IsDirectory)
            throw new InvalidOperationException("The specified FileSystemPath is not a directory.");
        return new FileSystemPath(Path + directoryName + DirectorySeparator);
    }

    [Pure]
    public FileSystemPath AppendFile(string fileName)
    {
        if (fileName.Contains(DirectorySeparator.ToString()))
            throw new ArgumentException("The specified name includes directory-separator(s).", nameof(fileName));
        if (!IsDirectory)
            throw new InvalidOperationException("The specified FileSystemPath is not a directory.");
        return new FileSystemPath(Path + fileName);
    }

    [Pure]
    public bool IsParentOf(FileSystemPath path)
    {
        if (!IsDirectory)
            throw new ArgumentException($"Path \"{Path}\" can not be a parent: it is not a directory.");

        return Path.Length != path.Path.Length && path.Path.StartsWith(Path);
    }

    [Pure]
    public bool IsChildOf(FileSystemPath path)
    {
        return path.IsParentOf(this);
    }

    [Pure]
    public FileSystemPath MakeRelativeTo(FileSystemPath parent)
    {
        if (Path == parent.Path)
            return Root;

        if (!IsChildOf(parent))
            throw new ArgumentException($"Path \"{parent}\" is not a parent of \"{Path}\".");

        int parentPathEnd = parent.Path.Length - 1;
        return new(Path[parentPathEnd..]);
    }

    [Pure]
    public FileSystemPath RemoveChild(FileSystemPath child)
    {
        if (!Path.EndsWith(child.Path))
            throw new ArgumentException("The specified path is not a child of this path.");
        return new FileSystemPath(Path[..(Path.Length - child.Path.Length + 1)]);
    }

    [Pure]
    public string GetExtension()
    {
        if (!IsFile)
            throw new ArgumentException("The specified FileSystemPath is not a file.");
        string name = EntityName;
        int extensionIndex = name.LastIndexOf('.');
        return extensionIndex <= 0 ? string.Empty : name[extensionIndex..];
    }

    [Pure]
    public FileSystemPath ChangeExtension(string extension)
    {
        if (!IsFile)
            throw new ArgumentException("The specified FileSystemPath is not a file.");
        string name = EntityName;
        int extensionIndex = name.LastIndexOf('.');
        if (extensionIndex < 0)
            return Parse(Path + extension);
        return ParentPath.AppendFile(name[..extensionIndex] + extension);
    }

    [Pure]
    public IEnumerable<string> GetDirectorySegments()
    {
        FileSystemPath path = this;
        if (IsFile)
            path = path.ParentPath;
        var segments = new LinkedList<string>();
        while (!path.IsRoot)
        {
            segments.AddFirst(path.EntityName);
            path = path.ParentPath;
        }
        return [.. segments];
    }

    [Pure]
    public int CompareTo(FileSystemPath other)
    {
        return string.CompareOrdinal(Path, other.Path);
    }

    [Pure]
    public override string ToString()
    {
        return Path;
    }
}
