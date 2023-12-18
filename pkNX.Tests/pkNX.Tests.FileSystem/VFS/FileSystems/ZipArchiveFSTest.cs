using System;
using System.IO;
using System.Text;
using System.IO.Compression;
using pkNX.Containers.VFS;
using Xunit;
using FluentAssertions;

namespace pkNX.Tests;

public class ZipArchiveFSTest : IDisposable
{
    private readonly Stream zipStream = new MemoryStream();
    private readonly ZipArchiveFileSystem fileSystem;
    private const string fileContentString = "this is a file";

    //setup
    public ZipArchiveFSTest()
    {
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
        var expect = new[]
        {
            textfileAPath,
            directoryPath,
            scratchDirectoryPath,
        };
        expect.Should().Contain(fileSystem.GetEntitiesInDirectory(FileSystemPath.Root));
    }

    [Fact]
    public void GetEntitiesOfDirectoryTest()
    {
        var expect = new[]
        {
            fileInDirectoryPath,
        };
        expect.Should().Contain(fileSystem.GetEntitiesInDirectory(directoryPath));
    }

    [Fact]
    public void ExistsTest()
    {
        fileSystem.Exists(FileSystemPath.Root).Should().BeTrue();
        fileSystem.Exists(textfileAPath).Should().BeTrue();
        fileSystem.Exists(directoryPath).Should().BeTrue();
        fileSystem.Exists(fileInDirectoryPath).Should().BeTrue();
        fileSystem.Exists(FileSystemPath.Parse("/nonExistingFile")).Should().BeFalse();
        fileSystem.Exists(FileSystemPath.Parse("/nonExistingDirectory/")).Should().BeFalse();
        fileSystem.Exists(FileSystemPath.Parse("/directory/nonExistingFileInDirectory")).Should().BeFalse();
    }

    [Fact]
    public void CanReadFile()
    {
        using var file = fileSystem.OpenFile(textfileAPath, FileMode.Open, FileAccess.ReadWrite);
        var text = file.ReadAllText();
        text.Should().Be(fileContentString);
    }

    [Fact]
    public void CanWriteFile()
    {
        const string reviseTo = fileContentString + " and a new string";
        {
            using var file = fileSystem.OpenFile(textfileAPath, FileMode.Open, FileAccess.ReadWrite);
            var textBytes = Encoding.ASCII.GetBytes(reviseTo);
            file.Write(textBytes);
        }
        {
            using var file = fileSystem.OpenFile(textfileAPath, FileMode.Open, FileAccess.ReadWrite);
            var text = file.ReadAllText();
            text.Should().Be(reviseTo);
        }
    }

    [Fact]
    public void CanAddFile()
    {
        var fsp = FileSystemPath.Parse("/scratchdirectory/recentlyadded.txt");
        {
            using var file = fileSystem.CreateFile(fsp);
            var textBytes = "recently added"u8;
            file.Write(textBytes);
        }
        fileSystem.Exists(fsp).Should().BeTrue();
        {
            using var sameFile = fileSystem.OpenFile(fsp, FileMode.Open, FileAccess.ReadWrite);
            var text = sameFile.ReadAllText();
            text.Should().Be("recently added");
        }
    }

    [Fact]
    public void CanAddDirectory()
    {
        var fsp = FileSystemPath.Parse("/scratchdirectory/dir/");
        fileSystem.CreateDirectory(fsp);
        fileSystem.Exists(fsp).Should().BeTrue();
    }
}
