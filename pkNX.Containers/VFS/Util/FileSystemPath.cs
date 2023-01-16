using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace pkNX.Containers.VFS;

public readonly struct FileSystemPath : IEquatable<FileSystemPath>, IComparable<FileSystemPath>
{
    public const char DirectorySeparator = '/';
    public static FileSystemPath Root { get; }

    public string Path { get; } = "/";

    public bool IsDirectory => Path[^1] == DirectorySeparator;

    public bool IsFile => !IsDirectory;

    public bool IsRoot => Path.Length == 1;

    public string EntityName
    {
        get
        {
            string name = Path;
            if (IsRoot)
                return string.Empty;
            int endOfName = name.Length;
            if (IsDirectory)
                endOfName--;
            int startOfName = name.LastIndexOf(DirectorySeparator, endOfName - 1, endOfName) + 1;
            return name[startOfName..endOfName];
        }
    }

    public FileSystemPath ParentPath
    {
        get
        {
            string parentPath = Path;
            if (IsRoot)
                throw new InvalidOperationException("There is no parent of root.");
            int lookaheadCount = parentPath.Length;
            if (IsDirectory)
                lookaheadCount--;
            int index = parentPath.LastIndexOf(DirectorySeparator, lookaheadCount - 1, lookaheadCount);
            Debug.Assert(index >= 0);
            parentPath = parentPath.Remove(index + 1);
            return new FileSystemPath(parentPath);
        }
    }

    static FileSystemPath()
    {
        Root = new FileSystemPath(DirectorySeparator.ToString());
    }

    private FileSystemPath(string path)
    {
        Path = path;
    }

    public static implicit operator FileSystemPath(string path)
    {
        var parsed = FileSystemPath.Parse(path);
        return parsed;
    }

    public static implicit operator string(FileSystemPath path)
    {
        return path.ToString();
    }

    public static bool IsRooted(string s)
    {
        if (s.Length == 0)
            return false;
        return s[0] == DirectorySeparator;
    }

    public static FileSystemPath Parse(string s)
    {
        if (s == null)
            throw new ArgumentNullException(nameof(s));
        if (!IsRooted(s))
            throw new UriFormatException($"Could not parse input \"{s}\": Path is not rooted.");
        if (s.Contains(string.Concat(DirectorySeparator, DirectorySeparator)))
            throw new UriFormatException($"Could not parse input \"{s}\": Path contains double directory-separators.");
        return new FileSystemPath(s);
    }

    public FileSystemPath AppendPath(string relativePath)
    {
        if (IsRooted(relativePath))
            throw new ArgumentException("The specified path should be relative.", nameof(relativePath));
        if (!IsDirectory)
            throw new InvalidOperationException("This FileSystemPath is not a directory.");
        return new FileSystemPath(Path + relativePath);
    }

    [Pure]
    public FileSystemPath AppendPath(FileSystemPath path)
    {
        if (!IsDirectory)
            throw new InvalidOperationException("This FileSystemPath is not a directory.");
        return new FileSystemPath(Path + path.Path[1..]);
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
        return IsDirectory && Path.Length != path.Path.Length && path.Path.StartsWith(Path);
    }

    [Pure]
    public bool IsChildOf(FileSystemPath path)
    {
        return path.IsParentOf(this);
    }

    [Pure]
    public FileSystemPath RemoveParent(FileSystemPath parent)
    {
        if (!parent.IsDirectory)
            throw new ArgumentException("The specified path can not be the parent of this path: it is not a directory.");
        if (!Path.StartsWith(parent.Path))
            throw new ArgumentException("The specified path is not a parent of this path.");
        return new FileSystemPath(Path.Remove(0, parent.Path.Length - 1));
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
        return segments.ToArray();
    }

    [Pure]
    public int CompareTo(FileSystemPath other)
    {
        return string.Compare(Path, other.Path, StringComparison.Ordinal);
    }

    [Pure]
    public override string ToString()
    {
        return Path;
    }

    [Pure]
    public override bool Equals(object? obj)
    {
        return obj is FileSystemPath path && Equals(path);
    }

    [Pure]
    public bool Equals(FileSystemPath other)
    {
        return other.Path.Equals(Path);
    }

    [Pure]
    public override int GetHashCode()
    {
        return Path.GetHashCode();
    }

    public static bool operator ==(FileSystemPath pathA, FileSystemPath pathB)
    {
        return pathA.Equals(pathB);
    }

    public static bool operator !=(FileSystemPath pathA, FileSystemPath pathB)
    {
        return !(pathA == pathB);
    }
}
