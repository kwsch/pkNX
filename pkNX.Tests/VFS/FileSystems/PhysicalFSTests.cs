using System;
using System.IO;
using System.Linq;
using System.Text;
using pkNX.Containers.VFS;
using Xunit;

namespace pkNX.Tests;

public class PhysicalFSTests : IDisposable
{
    string Root { get; set; }
    PhysicalFileSystem FileSystem { get; set; }
    string AbsoluteFileName { get; set; }

    string FileName { get; }
    FileSystemPath FileNamePath { get; }

    public PhysicalFSTests()
    {
        FileName = "x";
        FileNamePath = FileSystemPath.Root.AppendFile(FileName);
        Root = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(Root);
        AbsoluteFileName = Path.Combine(Root, FileName);
        FileSystem = new PhysicalFileSystem(Root);

    }

    public void Dispose()
    {
        using (FileSystem) { }
        Directory.Delete(Root, true);
    }

    [Fact]
    public void CreateFile()
    {
        Assert.False(File.Exists(AbsoluteFileName));
        Assert.False(FileSystem.Exists(FileNamePath));

        var content = "asdf"u8.ToArray();
        using (var stream = FileSystem.CreateFile(FileNamePath))
        {
            // File should exist at this point.
            Assert.True(FileSystem.Exists(FileNamePath));
            // File should also exist irl at this point.
            Assert.True(File.Exists(AbsoluteFileName));

            stream.Write(content, 0, content.Length);
        }

        // File should contain content.
        Assert.Equal(content, File.ReadAllBytes(AbsoluteFileName));

        using (var stream = FileSystem.OpenFile(FileNamePath, FileAccess.Read))
        {
            // Verify that EOF type stuff works.
            var readContent = new byte[2 * content.Length];
            Assert.Equal(content.Length, stream.Read(readContent, 0, readContent.Length));
            Assert.Equal(
                content,
                // trim to actual length.
                readContent.Take(content.Length).ToArray());

            // Trying to read beyond end of file should just return 0.
            Assert.Equal(0, stream.Read(readContent, 0, readContent.Length));
        }
    }

    [Fact]
    public void CreateFile_Exists()
    {
        Assert.False(File.Exists(AbsoluteFileName));
        Assert.False(FileSystem.Exists(FileNamePath));

        using (var stream = FileSystem.CreateFile(FileNamePath))
        {
            var content1 = "asdf"u8.ToArray();
            stream.Write(content1, 0, content1.Length);
        }

        // creating an existing file should truncate like open(O_CREAT).
        var content2 = "b"u8.ToArray();
        using (var stream = FileSystem.CreateFile(FileNamePath))
        {
            stream.Write(content2, 0, content2.Length);
        }
        Assert.Equal(content2, File.ReadAllBytes(AbsoluteFileName));
        using (var stream = FileSystem.OpenFile(FileNamePath, FileAccess.Read))
        {
            Assert.Equal(content2, stream.ReadAllBytes());
        }
    }

    [Fact]
    public void CreateFile_Empty()
    {
        using (var stream = FileSystem.CreateFile(FileNamePath))
        {
        }

        Assert.Equal(Array.Empty<byte>(), File.ReadAllBytes(AbsoluteFileName));
        using (var stream = FileSystem.OpenFile(FileNamePath, FileAccess.Read))
        {
            Assert.Equal(Array.Empty<byte>(), stream.ReadAllBytes());
        }
    }
}
