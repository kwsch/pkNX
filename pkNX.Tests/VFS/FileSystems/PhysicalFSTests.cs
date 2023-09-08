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

    string AbsFilePath_W { get; set; }
    string AbsFilePath_R { get; set; }

    FileSystemPath FilePath_W { get; }
    FileSystemPath FilePath_R { get; }

    public PhysicalFSTests()
    {
        Root = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(Root);

        FilePath_W = FileSystemPath.Root.AppendFile("x");
        FilePath_R = FileSystemPath.Root.AppendFile("y");

        AbsFilePath_W = Path.Combine(Root, "x");
        AbsFilePath_R = Path.Combine(Root, "y");

        FileSystem = new PhysicalFileSystem(Root);

        var content = "asdf"u8.ToArray();
        using var stream = File.Create(AbsFilePath_R);
        stream.Write(content, 0, content.Length);
    }

    public void Dispose()
    {
        using (FileSystem) { }
        Directory.Delete(Root, true);
    }

    [Fact]
    public void CreateFile()
    {
        Assert.False(File.Exists(AbsFilePath_W));
        Assert.False(FileSystem.Exists(FilePath_W));

        var content = "asdf"u8.ToArray();
        using (var stream = FileSystem.CreateFile(FilePath_W))
        {
            stream.Write(content, 0, content.Length);
        }

        // File should contain content.
        Assert.Equal(content, File.ReadAllBytes(AbsFilePath_W));

        using (var stream = FileSystem.OpenFile(FilePath_W))
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
        Assert.False(File.Exists(AbsFilePath_W));
        Assert.False(FileSystem.Exists(FilePath_W));

        using (var stream = FileSystem.CreateFile(FilePath_W))
        {
            var content1 = "asdf"u8.ToArray();
            stream.Write(content1, 0, content1.Length);
        }

        // creating an existing file should truncate like open(O_CREAT).
        var content2 = "b"u8.ToArray();
        using (var stream = FileSystem.CreateFile(FilePath_W))
        {
            stream.Write(content2, 0, content2.Length);
        }
        Assert.Equal(content2, File.ReadAllBytes(AbsFilePath_W));
        using (var stream = FileSystem.OpenFile(FilePath_W))
        {
            Assert.Equal(content2, stream.ReadAllBytes());
        }
    }

    [Fact]
    public void CreateFile_Empty()
    {
        using (var stream = FileSystem.CreateFile(FilePath_W))
        {
        }

        Assert.Equal(Array.Empty<byte>(), File.ReadAllBytes(AbsFilePath_W));
        using (var stream = FileSystem.OpenFile(FilePath_W))
        {
            Assert.Equal(Array.Empty<byte>(), stream.ReadAllBytes());
        }
    }

    /*
Open("/path/to/a.file", "r") -> stream(read-only) to romfs/path/to/a.file
Open("/path/to/a.file", "w") -> error # a is read only
Open("/path/to/a.file", "rw") -> error # a is read only 
Open("/path/to/b.file", "r") -> stream(read-only) to overlayfs/path/to/b.file
Open("/path/to/b.file", "w") -> stream(write-only) to overlayfs/path/to/b.file
Open("/path/to/b.file", "rw") -> stream(rw) to overlayfs/path/to/b.file
Open("/path/to/c.file", "r") -> stream(read-only) to overlayfs/path/to/c.file
Open("/path/to/c.file", "w") -> stream(write-only) to overlayfs/path/to/c.file
Open("/path/to/c.file", "rw") -> stream(rw) to overlayfs/path/to/c.file
Open("/path/to/d.file", "r") -> error # d doesn't exist 
Open("/path/to/d.file", "w") -> error # d doesn't exist 
Open("/path/to/d.file", "rw") -> error # d doesn't exist
       
OpenOrCreate("/path/to/a.file", "r") -> Open("r") # a already exists, use open
OpenOrCreate("/path/to/a.file", "w") -> CreateNew("w") # a is read only -> write-only requested -> create new file
OpenOrCreate("/path/to/a.file", "rw") -> Copy(romfs/path/to/a.file, overlayfs/path/to/a.file) -> stream(rw) to overlayfs/path/to/a.file
OpenOrCreate("/path/to/b.file", "r") -> Open("r") # b already exists, use open
OpenOrCreate("/path/to/b.file", "w") -> Open("w") # b already exists, use open
OpenOrCreate("/path/to/b.file", "rw") -> Open("rw") # b already exists, use open
OpenOrCreate("/path/to/c.file", "r") -> Open("r") # c already exists, use open
OpenOrCreate("/path/to/c.file", "w") -> Open("w") # c already exists, use open 
OpenOrCreate("/path/to/c.file", "rw") -> Open("rw") # c already exists, use open
OpenOrCreate("/path/to/d.file", "r") -> error # d doesn't exist, can't call CreateNew()
OpenOrCreate("/path/to/d.file", "w") -> CreateNew("w") # d doesn't exist, create new file
OpenOrCreate("/path/to/d.file", "rw") -> CreateNew("rw") # d doesn't exist, create new file 
       
CreateNew("/path/to/a.file", "(r)w") -> error # a already exists
CreateNew("/path/to/b.file", "(r)w") -> error # b already exists
CreateNew("/path/to/c.file", "(r)w") -> error # c already exists
CreateNew("/path/to/d.file", "(r)w") -> stream("(r)w") to new file created in overlayfs/path/to/d.file 
       
Create("/path/to/a.file", "(r)w") -> Truncate("w") # a already exists, use truncate
Create("/path/to/b.file", "(r)w") -> Truncate("w") # b already exists, use truncate
Create("/path/to/c.file", "(r)w") -> Truncate("w") # c already exists, use truncate
Create("/path/to/d.file", "(r)w") -> CreateNew("(r)w") # d doesn't exist, use CreateNew
       
Truncate("/path/to/a.file", "w") -> error # a is read only
Truncate("/path/to/b.file", "w") -> stream(write-only) to overlayfs/path/to/b.file
Truncate("/path/to/c.file", "w") -> stream(write-only) to overlayfs/path/to/c.file 
Truncate("/path/to/d.file", "w") -> error # d doesn't exist
       
Append("/path/to/a.file", "rw") -> error # a is read only
Append("/path/to/b.file", "rw") -> stream(rw) to overlayfs/path/to/b.file
Append("/path/to/c.file", "rw") -> stream(rw) to overlayfs/path/to/c.file
Append("/path/to/d.file", "rw") -> error # d doesn't exist
     */

    // Write unit tests for all of the above.

    [Fact]
    public void OpenFile_ReadOnly()
    {
        using (var stream = FileSystem.OpenFile(FilePath_R, FileMode.Open, FileAccess.Read))
        {
            Assert.Equal("asdf"u8.ToArray(), stream.ReadAllBytes());
        }
    }

    [Fact]
    public void OpenFile_ReadOnly_Throws()
    {
        //Assert.Throws<UnauthorizedAccessException>(() => { using var _ = FileSystem.OpenFile(FilePath_R, FileMode.Open, FileAccess.Read); });
        //Assert.Throws<UnauthorizedAccessException>(() => { using var _ = FileSystem.OpenFile(FilePath_R, FileMode.Open, FileAccess.Write); });
        //Assert.Throws<UnauthorizedAccessException>(() => { using var _ = FileSystem.OpenFile(FilePath_R, FileMode.Open, FileAccess.ReadWrite); });
    }

    [Fact]
    public void OpenFile_WriteOnly()
    {
        using (var stream = FileSystem.OpenFile(FilePath_R, FileMode.Open, FileAccess.Write))
        {
            var content = "asdf"u8.ToArray();
            stream.Write(content, 0, content.Length);
        }

        Assert.Equal("asdf"u8.ToArray(), File.ReadAllBytes(AbsFilePath_R));
        using (var stream = FileSystem.OpenFile(FilePath_R))
        {
            Assert.Equal("asdf"u8.ToArray(), stream.ReadAllBytes());
        }
    }

    [Fact]
    public void OpenFile_ReadWrite()
    {
        using (var stream = FileSystem.OpenFile(FilePath_R, FileMode.Open, FileAccess.ReadWrite))
        {
            Assert.Equal("asdf"u8.ToArray(), stream.ReadAllBytes());
            stream.Position = 0;
            var content = "qwer"u8.ToArray();
            stream.Write(content, 0, content.Length);
        }

        Assert.Equal("qwer"u8.ToArray(), File.ReadAllBytes(AbsFilePath_R));
        using (var stream = FileSystem.OpenFile(FilePath_R))
        {
            Assert.Equal("qwer"u8.ToArray(), stream.ReadAllBytes());
        }
    }

    [Fact]
    public void OpenFile_CreateNew()
    {
        using (var stream = FileSystem.OpenFile(FilePath_W, FileMode.CreateNew, FileAccess.ReadWrite))
        {
            var content = "asdf"u8.ToArray();
            stream.Write(content, 0, content.Length);
        }

        Assert.Equal("asdf"u8.ToArray(), File.ReadAllBytes(AbsFilePath_W));
        using (var stream = FileSystem.OpenFile(FilePath_W))
        {
            Assert.Equal("asdf"u8.ToArray(), stream.ReadAllBytes());
        }
    }

    [Fact]
    public void OpenFile_CreateNew_Throws()
    {
        Assert.Throws<ArgumentException>(() => { using var _ = FileSystem.OpenFile(FilePath_W, FileMode.CreateNew, FileAccess.Read); });
    }


    [Fact]
    public void OpenFile_Create()
    {
        using (var stream = FileSystem.OpenFile(FilePath_W, FileMode.Create, FileAccess.ReadWrite))
        {
            var content = "asdf"u8.ToArray();
            stream.Write(content, 0, content.Length);
        }

        Assert.Equal("asdf"u8.ToArray(), File.ReadAllBytes(AbsFilePath_W));
        using (var stream = FileSystem.OpenFile(FilePath_W))
        {
            Assert.Equal("asdf"u8.ToArray(), stream.ReadAllBytes());
        }
    }

    [Fact]
    public void OpenFile_Create_Throws()
    {
        Assert.Throws<ArgumentException>(() => { using var _ = FileSystem.OpenFile(FilePath_W, FileMode.Create, FileAccess.Read); });
    }

    [Fact]
    public void OpenFile_Truncate()
    {
        using (var stream = FileSystem.OpenFile(FilePath_R, FileMode.Truncate, FileAccess.ReadWrite))
        {
            var content = "asdf"u8.ToArray();
            stream.Write(content, 0, content.Length);
        }

        Assert.Equal("asdf"u8.ToArray(), File.ReadAllBytes(AbsFilePath_R));
        using (var stream = FileSystem.OpenFile(FilePath_R))
        {
            Assert.Equal("asdf"u8.ToArray(), stream.ReadAllBytes());
        }
    }

    [Fact]
    public void OpenFile_Truncate_Throws()
    {
        Assert.Throws<ArgumentException>(() => { using var _ = FileSystem.OpenFile(FilePath_R, FileMode.Truncate, FileAccess.Read); });
    }

    [Fact]
    public void OpenFile_Append()
    {
        using (var stream = FileSystem.OpenFile(FilePath_W, FileMode.Append, FileAccess.Write))
        {
            var content = "asdf"u8.ToArray();
            stream.Write(content, 0, content.Length);
        }

        Assert.Equal("asdf"u8.ToArray(), File.ReadAllBytes(AbsFilePath_W));
        using (var stream = FileSystem.OpenFile(FilePath_W))
        {
            Assert.Equal("asdf"u8.ToArray(), stream.ReadAllBytes());
        }
    }

    [Fact]
    public void OpenFile_Append_Throws()
    {
        Assert.Throws<ArgumentException>(() => { using var _ = FileSystem.OpenFile(FilePath_W, FileMode.Append, FileAccess.Read); });
    }
}
