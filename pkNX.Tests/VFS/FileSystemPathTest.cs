using System.Collections.Generic;
using System.Linq;
using System;
using pkNX.Containers.VFS;
using SharpFileSystem.Tests;
using Xunit;
// ReSharper disable ReturnValueOfPureMethodIsNotUsed

namespace pkNX.Tests;

/// <summary>
///This is a test class for FileSystemPathTest and is intended
///to contain all FileSystemPathTest Unit Tests
///</summary>
public class FileSystemPathTest
{
    private FileSystemPath[] _paths = { root, directoryA, fileA, directoryB, fileB };
    private IEnumerable<FileSystemPath> Directories { get { return _paths.Where(p => p.IsDirectory); } }
    private IEnumerable<FileSystemPath> Files { get { return _paths.Where(p => p.IsFile); } }

    private static readonly FileSystemPath directoryA = "/directorya/";
    private static FileSystemPath fileA = "/filea";
    private static FileSystemPath directoryB = "/directorya/directoryb/";
    private static FileSystemPath fileB = FileSystemPath.Parse("/directorya/fileb.txt");
    private static FileSystemPath root = FileSystemPath.Root;
    private FileSystemPath fileC;

    /// <summary>
    ///A test for Root
    ///</summary>
    [Fact]
    public void RootTest()
    {
        Assert.Equal(FileSystemPath.Parse("/"), root);
    }

    /// <summary>
    ///A test for ParentPath
    ///</summary>
    [Fact]
    public void ParentPathTest()
    {
        Assert.True(Directories.Where(d => d.GetDirectorySegments().Count() == 1)
                .All(d => d.ParentPath == root));

        Assert.DoesNotContain(Files, f => f.RemoveChild(root.AppendFile(f.EntityName)) != f.ParentPath);
        EAssert.Throws<InvalidOperationException>(() => Assert.Equal(root.ParentPath, root.ParentPath));
    }

    /// <summary>
    ///A test for IsRoot
    ///</summary>
    [Fact]
    public void IsRootTest()
    {
        Assert.True(root.IsRoot);
        Assert.False(directoryA.IsRoot);
        Assert.False(fileA.IsRoot);
    }

    /// <summary>
    ///A test for IsFile
    ///</summary>
    [Fact]
    public void IsFileTest()
    {

        Assert.True(fileA.IsFile);
        Assert.False(directoryA.IsFile);
        Assert.False(root.IsFile);
    }

    /// <summary>
    ///A test for IsDirectory
    ///</summary>
    [Fact]
    public void IsDirectoryTest()
    {
        Assert.True(directoryA.IsDirectory);
        Assert.True(root.IsDirectory);
        Assert.False(fileA.IsDirectory);
    }

    /// <summary>
    ///A test for EntityName
    ///</summary>
    [Fact]
    public void EntityNameTest()
    {
        Assert.Equal("filea", fileA.EntityName);
        Assert.Equal("fileb.txt", fileB.EntityName);
        Assert.Equal(string.Empty, root.EntityName);
    }

    /// <summary>
    ///A test for ToString
    ///</summary>
    [Fact]
    public void ToStringTest()
    {
        string s = "/directorya/";
        Assert.Equal(s, FileSystemPath.Parse(s).ToString());
        Assert.Equal(s, (string)FileSystemPath.Parse(s));

        Assert.Equal("C:\\" + s, "C:\\" + FileSystemPath.Parse(s));
    }

    /// <summary>
    ///A test for MakeRelativeTo
    ///</summary>
    [Fact]
    public void MakeRelativeToTest()
    {
        Assert.Equal(directoryB.MakeRelativeTo(directoryB), root);
        Assert.Equal(fileB.MakeRelativeTo(directoryA), FileSystemPath.Parse("/fileb.txt"));
        Assert.Equal(root.MakeRelativeTo(root), root);
        Assert.Equal(directoryB.MakeRelativeTo(root), directoryB);
        EAssert.Throws<ArgumentException>(() => fileB.MakeRelativeTo(FileSystemPath.Parse("/nonexistantparent/")));
        EAssert.Throws<ArgumentException>(() => fileB.MakeRelativeTo(FileSystemPath.Parse("/nonexistantparent")));
        EAssert.Throws<ArgumentException>(() => fileB.MakeRelativeTo(FileSystemPath.Parse("/fileb.txt")));
        EAssert.Throws<ArgumentException>(() => fileB.MakeRelativeTo(FileSystemPath.Parse("/directorya")));
    }

    /// <summary>
    ///A test for RemoveChild
    ///</summary>
    [Fact]
    public void RemoveChildTest()
    {
        Assert.Equal(fileB.RemoveChild(FileSystemPath.Parse("/fileb.txt")), directoryA);
        Assert.Equal(directoryB.RemoveChild(FileSystemPath.Parse("/directoryb/")), directoryA);
        Assert.Equal(directoryB.RemoveChild(directoryB), root);
        Assert.Equal(fileB.RemoveChild(fileB), root);
        EAssert.Throws<ArgumentException>(() => directoryA.RemoveChild(FileSystemPath.Parse("/nonexistantchild")));
        EAssert.Throws<ArgumentException>(() => directoryA.RemoveChild(FileSystemPath.Parse("/directorya")));
    }

    /// <summary>
    ///A test for Parse
    ///</summary>
    [Fact]
    public void ParseTest()
    {
        Assert.True(_paths.All(p => p == FileSystemPath.Parse(p.ToString())));
        Assert.True(_paths.All(p => p == p.ToString()));
        EAssert.Throws<UriFormatException>(() => FileSystemPath.Parse("thisisnotapath"));
        EAssert.Throws<UriFormatException>(() => FileSystemPath.Parse("/thisisainvalid//path"));
    }

    /// <summary>
    ///A test for IsRooted
    ///</summary>
    [Fact]
    public void IsRootedTest()
    {
        Assert.True(FileSystemPath.IsRooted("/filea"));
        Assert.True(FileSystemPath.IsRooted("/directorya/"));
        Assert.False(FileSystemPath.IsRooted("filea"));
        Assert.False(FileSystemPath.IsRooted("directorya/"));
        Assert.True(_paths.All(p => FileSystemPath.IsRooted(p.ToString())));
    }

    /// <summary>
    ///A test for IsParentOf
    ///</summary>
    [Fact]
    public void IsParentOfTest()
    {
        Assert.True(directoryA.IsParentOf(fileB));
        Assert.True(directoryA.IsParentOf(directoryB));
        Assert.True(root.IsParentOf(fileA));
        Assert.True(root.IsParentOf(directoryA));
        Assert.True(root.IsParentOf(fileB));
        Assert.True(root.IsParentOf(directoryB));

        EAssert.Throws<ArgumentException>(() => fileB.IsParentOf(directoryA));
        EAssert.Throws<ArgumentException>(() => fileA.IsParentOf(root));
        EAssert.Throws<ArgumentException>(() => fileB.IsParentOf(root));
        Assert.False(directoryB.IsParentOf(directoryA));
        Assert.False(directoryA.IsParentOf(root));
        Assert.False(directoryB.IsParentOf(root));
    }

    /// <summary>
    ///A test for IsChildOf
    ///</summary>
    [Fact]
    public void IsChildOfTest()
    {
        Assert.True(fileB.IsChildOf(directoryA));
        Assert.True(directoryB.IsChildOf(directoryA));
        Assert.True(fileA.IsChildOf(root));
        Assert.True(directoryA.IsChildOf(root));
        Assert.True(fileB.IsChildOf(root));
        Assert.True(directoryB.IsChildOf(root));

        EAssert.Throws<ArgumentException>(() => directoryA.IsChildOf(fileB));
        EAssert.Throws<ArgumentException>(() => root.IsChildOf(fileA));
        EAssert.Throws<ArgumentException>(() => root.IsChildOf(fileB));

        Assert.False(directoryA.IsChildOf(directoryB));
        Assert.False(root.IsChildOf(directoryA));
        Assert.False(root.IsChildOf(directoryB));
    }

    /// <summary>
    ///A test for GetExtension
    ///</summary>
    [Fact]
    public void GetExtensionTest()
    {
        Assert.Equal("", fileA.GetExtension());
        Assert.Equal(".txt", fileB.GetExtension());
        fileC = FileSystemPath.Parse("/directory.txt/filec");
        Assert.Equal("", fileC.GetExtension());
        EAssert.Throws<ArgumentException>(() => directoryA.GetExtension());
    }

    /// <summary>
    ///A test for GetFileNameAndExtension
    ///</summary>
    [Fact]
    public void GetFileNameAndExtensionTest()
    {
        Assert.Equal(("filea", ""), fileA.GetFileNameAndExtension());
        Assert.Equal(("fileb", ".txt"), fileB.GetFileNameAndExtension());
        fileC = FileSystemPath.Parse("/directory.txt/filec");
        Assert.Equal(("filec", ""), fileC.GetFileNameAndExtension());
        EAssert.Throws<ArgumentException>(() => directoryA.GetFileNameAndExtension());
    }

    /// <summary>
    ///A test for GetFileNameAndExtension
    ///</summary>
    [Fact]
    public void GetFileNameWithoutExtensionTest()
    {
        Assert.Equal("filea", fileA.GetFileNameWithoutExtension());
        Assert.Equal("fileb", fileB.GetFileNameWithoutExtension());
        fileC = FileSystemPath.Parse("/directory.txt/filec");
        Assert.Equal("filec", fileC.GetFileNameWithoutExtension());
        EAssert.Throws<ArgumentException>(() => directoryA.GetFileNameWithoutExtension());
    }

    /// <summary>
    ///A test for GetDirectorySegments
    ///</summary>
    [Fact]
    public void GetDirectorySegmentsTest()
    {
        Assert.Empty(root.GetDirectorySegments());
        Directories
            .Where(d => !d.IsRoot)
            .All(d => d.GetDirectorySegments().Count() == d.ParentPath.GetDirectorySegments().Count() - 1);
        Files.All(f => f.GetDirectorySegments().Count() == f.ParentPath.GetDirectorySegments().Count());
    }


    /// <summary>
    ///A test for CompareTo
    ///</summary>
    [Fact]
    public void CompareToTest()
    {
        foreach (var pa in _paths)
        {
            foreach (var pb in _paths)
                Assert.Equal(Math.Sign(pa.CompareTo(pb)), Math.Sign(string.Compare(pa.ToString(), pb.ToString(), StringComparison.Ordinal)));
        }
    }

    /// <summary>
    ///A test for ChangeExtension
    ///</summary>
    [Fact]
    public void ChangeExtensionTest()
    {
        foreach (var p in _paths.Where(p => p.IsFile))
            Assert.True(p.ChangeExtension(".exe").GetExtension() == ".exe");
        EAssert.Throws<ArgumentException>(() => directoryA.ChangeExtension(".exe"));
    }

    /// <summary>
    ///A test for AppendPath
    ///</summary>
    [Fact]
    public void AppendPathTest()
    {
        Assert.True(Directories.All(p => p.AppendPath(root) == p));
        Assert.True(Directories.All(p => p.AppendPath("") == p));

        var subpath = FileSystemPath.Parse("/dir/file");
        var subpathstr = "dir/file";
        foreach (var p in Directories)
        {
            Assert.True(p.AppendPath(subpath).ParentPath.ParentPath == p);
            Assert.True(p.AppendPath(subpathstr).ParentPath.ParentPath == p);

            Assert.True((p + subpath).ParentPath.ParentPath == p);
            Assert.True((p + subpathstr).ParentPath.ParentPath == p);

            foreach (var pb in _paths.Where(pb => !pb.IsRoot))
            {
                Assert.True(p.AppendPath(pb).IsChildOf(p));
                Assert.True((p + pb).IsChildOf(p));
            }
        }

        EAssert.Throws<InvalidOperationException>(() => fileA.AppendPath(subpath));
        EAssert.Throws<InvalidOperationException>(() => fileA.AppendPath(subpathstr));
        EAssert.Throws<ArgumentException>(() => directoryA.AppendPath("/rootedpath/"));
        EAssert.Throws<InvalidOperationException>(() => _ = fileA + subpath);
        EAssert.Throws<InvalidOperationException>(() => _ = fileA + subpathstr);
        EAssert.Throws<ArgumentException>(() => _ = directoryA + "/rootedpath/");
    }

    /// <summary>
    ///A test for AppendFile
    ///</summary>
    [Fact]
    public void AppendFileTest()
    {
        foreach (var d in Directories)
        {
            Assert.True(d.AppendFile("file").IsFile);
            Assert.True(d.AppendFile("file").EntityName == "file");
            Assert.True(d.AppendFile("file").ParentPath == d);
        }
        EAssert.Throws<InvalidOperationException>(() => fileA.AppendFile("file"));
        EAssert.Throws<ArgumentException>(() => directoryA.AppendFile("dir/file"));
    }

    /// <summary>
    ///A test for AppendDirectory
    ///</summary>
    [Fact]
    public void AppendDirectoryTest()
    {
        foreach (var d in Directories)
        {
            Assert.True(d.AppendDirectory("dir").IsDirectory);
            Assert.True(d.AppendDirectory("dir").EntityName == "dir");
            Assert.True(d.AppendDirectory("dir").ParentPath == d);
        }
        EAssert.Throws<InvalidOperationException>(() => fileA.AppendDirectory("dir"));
        EAssert.Throws<ArgumentException>(() => root.AppendDirectory("dir/dir"));
    }
}
