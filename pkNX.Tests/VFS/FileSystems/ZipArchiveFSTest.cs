using System;
using System.IO;
using System.Linq;
using System.Text;
using System.IO.Compression;
using pkNX.Containers.VFS;
using Xunit;

namespace pkNX.Tests;

public class ZipArchiveFSTest
{
    private Stream zipStream;
    private ZipArchiveFileSystem fileSystem;
    private string fileContentString = "this is a file";

    //setup
    public ZipArchiveFSTest()
    {
        zipStream = new MemoryStream();

        var fileContentBytes = Encoding.ASCII.GetBytes(fileContentString);

        using (var zipOutput = new ZipArchive(zipStream, ZipArchiveMode.Create, true))
        {
            var entry = zipOutput.CreateEntry("textfileA.txt");
            using (Stream stream = entry.Open())
                stream.Write(fileContentBytes);

            zipOutput.CreateEntry("directory/fileInDirectory.txt");
            zipOutput.CreateEntry("scratchdirectory/scratch");
        }

        zipStream.Position = 0;
        fileSystem = ZipArchiveFileSystem.Open(zipStream);
    }

    //teardown
    public void Dispose()
    {
        fileSystem.Dispose();
        zipStream.Dispose();
    }

    private readonly FileSystemPath directoryPath = FileSystemPath.Parse("/directory/");
    private readonly FileSystemPath textfileAPath = FileSystemPath.Parse("/textfileA.txt");
    private readonly FileSystemPath fileInDirectoryPath = FileSystemPath.Parse("/directory/fileInDirectory.txt");
    private readonly FileSystemPath scratchDirectoryPath = FileSystemPath.Parse("/scratchdirectory/");

    [Fact]
    public void GetEntitiesOfRootTest()
    {
        Assert.Equal(new[]
        {
                textfileAPath,
                directoryPath,
                scratchDirectoryPath
            }, fileSystem.GetEntityPaths(FileSystemPath.Root).ToArray());
    }

    [Fact]
    public void GetEntitiesOfDirectoryTest()
    {
        Assert.Equal(new[]
        {
                fileInDirectoryPath
            }, fileSystem.GetEntityPaths(directoryPath).ToArray());
    }

    [Fact]
    public void ExistsTest()
    {
        Assert.True(fileSystem.Exists(FileSystemPath.Root));
        Assert.True(fileSystem.Exists(textfileAPath));
        Assert.True(fileSystem.Exists(directoryPath));
        Assert.True(fileSystem.Exists(fileInDirectoryPath));
        Assert.False(fileSystem.Exists(FileSystemPath.Parse("/nonExistingFile")));
        Assert.False(fileSystem.Exists(FileSystemPath.Parse("/nonExistingDirectory/")));
        Assert.False(fileSystem.Exists(FileSystemPath.Parse("/directory/nonExistingFileInDirectory")));
    }

    [Fact]
    public void CanReadFile()
    {
        var file = fileSystem.OpenFile(textfileAPath, FileAccess.ReadWrite);
        var text = file.ReadAllText();
        Assert.True(string.Equals(text, fileContentString));
    }

    [Fact]
    public void CanWriteFile()
    {
        var file = fileSystem.OpenFile(textfileAPath, FileAccess.ReadWrite);
        var textBytes = Encoding.ASCII.GetBytes(fileContentString + " and a new string");
        file.Write(textBytes);
        file.Close();


        file = fileSystem.OpenFile(textfileAPath, FileAccess.ReadWrite);
        var text = file.ReadAllText();
        Assert.True(string.Equals(text, fileContentString + " and a new string"));
    }

    [Fact]
    public void CanAddFile()
    {
        var fsp = FileSystemPath.Parse("/scratchdirectory/recentlyadded.txt");
        var file = fileSystem.CreateFile(fsp);
        var textBytes = "recently added"u8.ToArray();
        file.Write(textBytes);
        file.Close();

        Assert.True(fileSystem.Exists(fsp));

        file = fileSystem.OpenFile(fsp, FileAccess.ReadWrite);
        var text = file.ReadAllText();
        Assert.True(string.Equals(text, "recently added"));
    }

    [Fact]
    public void CanAddDirectory()
    {
        var fsp = FileSystemPath.Parse("/scratchdirectory/dir/");
        fileSystem.CreateDirectory(fsp);

        Assert.True(fileSystem.Exists(fsp));
    }
}
