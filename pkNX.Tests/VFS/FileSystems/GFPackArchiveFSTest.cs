using System;
using System.IO;
using System.Linq;
using System.Text;
using System.IO.Compression;
using pkNX.Containers.VFS;
using Xunit;
using pkNX.Containers;
using pkNX.Structures.FlatBuffers;
using pkNX.Structures.FlatBuffers.Arceus;

namespace pkNX.Tests;

public class GFPackArchiveFSTest
{
    private Stream gfpackStream;
    private GFPackArchiveFileSystem fileSystem;
    private string fileContentString = "this is a file";

    //setup
    public GFPackArchiveFSTest()
    {
        /*gfpackStream = new MemoryStream();

        var fileContentBytes = Encoding.ASCII.GetBytes(fileContentString);

        using (var output = new GFPackArchive(gfpackStream, ArchiveOpenMode.Create, true))
        {
            var entry = output.CreateEntry("textfileA.txt");
            using (Stream stream = entry.Open())
                stream.Write(fileContentBytes);

            output.CreateEntry("directory/fileInDirectory.txt");
            output.CreateEntry("scratchdirectory/scratch");
        }

        gfpackStream.Position = 0;*/

        gfpackStream = File.Open("C:\\Users\\Admin\\Documents\\3ds wiiu Game hacks stuff\\switch\\pkNX\\Clean RomFS Dump\\romfs\\bin\\archive\\pokemon\\pm0025_00_00.gfpak", FileMode.Open, FileAccess.ReadWrite);

        fileSystem = GFPackArchiveFileSystem.Open(gfpackStream);
    }

    //teardown
    public void Dispose()
    {
        fileSystem.Dispose();
        gfpackStream.Dispose();
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
            }, fileSystem.GetEntitiesInDirectory(FileSystemPath.Root).ToArray());
    }

    [Fact]
    public void GetEntitiesOfDirectoryTest()
    {
        Assert.Equal(new[]
        {
                fileInDirectoryPath
            }, fileSystem.GetEntitiesInDirectory(directoryPath).ToArray());
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
        FileSystemPath pkmTestPath = "/bin/pokemon/pm0025/pm0025_00_00/pm0025_00_00.trpokecfg";

        var file = fileSystem.OpenFile(pkmTestPath, FileMode.Open, FileAccess.ReadWrite);
        var compressedBytes = file.ReadAllBytes();
        var decompressed = GFPack.Decompress(compressedBytes, 76, CompressionType.OodleKraken);
        var config = FlatBufferConverter.DeserializeFrom<PokeConfig>(decompressed);
        var text = file.ReadAllText();
        Assert.True(string.Equals(text, fileContentString));
    }

    [Fact]
    public void CanWriteFile()
    {
        var file = fileSystem.OpenFile(textfileAPath, FileMode.Open, FileAccess.ReadWrite);
        var textBytes = Encoding.ASCII.GetBytes(fileContentString + " and a new string");
        file.Write(textBytes);
        file.Close();


        file = fileSystem.OpenFile(textfileAPath, FileMode.Open, FileAccess.ReadWrite);
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

        file = fileSystem.OpenFile(fsp, FileMode.Open, FileAccess.ReadWrite);
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
