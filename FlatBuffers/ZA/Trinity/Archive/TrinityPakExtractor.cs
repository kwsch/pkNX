using System.Buffers;
using pkNX.Containers;

namespace pkNX.Structures.FlatBuffers.ZA.Trinity;

public static class TrinityPakExtractor
{
    public const string DumpArchiveExtracted = "extracted";
    private const string DumpArchivePak = "paks";

    public static void DumpArchives(string pathRomFS, string dirRootDump)
    {
        var pathFs = Path.Combine(pathRomFS, "arc", "data.trpfs");
        var pathFd = Path.Combine(pathRomFS, "arc", "data.trpfd");
        var reader = new TrinityFileSystemManager(pathFs, pathFd);
        Extract(dirRootDump, reader);
        reader.Dispose();
    }

    private static void Extract(string dirRoot, TrinityFileSystemManager manager)
    {
        Directory.CreateDirectory(dirRoot);

        var dirPak = Path.Combine(dirRoot, DumpArchivePak);
        Directory.CreateDirectory(dirPak);

        var dirExtract = Path.Combine(dirRoot, DumpArchiveExtracted);
        Directory.CreateDirectory(dirExtract);

        var count = manager.FileCount;
        for (var i = 0; i < count; i++)
            ExportArc(manager, i, dirPak, dirExtract);
    }

    private static void ExportArc(TrinityFileSystemManager reader, int packIndex, string dirPak, string dirExtract)
    {
        var pool = ArrayPool<byte>.Shared;
        var hash = reader.GetPakHash(packIndex);
        var length = reader.GetPakLength(packIndex);
        var offset = reader.GetPakOffset(hash);
        var rent = pool.Rent((int)length);
        var data = rent.AsMemory(0, (int)length);
        try
        {
            reader.GetData(offset, length, data.Span);
            var packFilePath = reader.GetPackPath(packIndex);
            ExportPackFile(data.Span, dirPak, packFilePath);
            ExportPackExtract(data, dirExtract, packFilePath);
        }
        finally
        {
            data.Span.Clear();
            pool.Return(rent);
        }
    }

    private static void ExportPackFile(ReadOnlySpan<byte> data, string dir, string pakFilePath)
    {
        var fileName = Path.Combine(dir, pakFilePath);
        var dirName = Path.GetDirectoryName(fileName);
        if (string.IsNullOrEmpty(dirName))
            throw new Exception($"{fileName} directory name is null");
        Directory.CreateDirectory(dirName);

        File.WriteAllBytes(fileName, data);
    }

    private static void ExportPackExtract(Memory<byte> data, string dir, string pakFilePath)
    {
        var folder = Path.Combine(dir, pakFilePath);
        Directory.CreateDirectory(folder);
        var obj = FlatBufferConverter.DeserializeFrom<TrinityPak>(data);
        ExtractPack(obj, folder);
    }

    private static void ExtractPack(TrinityPak trpak, string dir)
    {
        for (var i = 0; i < trpak.Files.Count; i++)
            WriteFile(trpak.Files[i], trpak.Hashes[i], dir, i);
    }

    private static void WriteFile(TrinityPakFileData file, ulong hash, string dir, int fileIndex)
    {
        if (file.CompressionType is DataCompressionType.None)
        {
            WriteFile(file.Data.Span, hash, dir, fileIndex);
            return;
        }

        var pool = ArrayPool<byte>.Shared;
        var length = (int)file.UncompressedSize;
        var rent = pool.Rent(length);
        var decompressed = rent.AsSpan(0, length);
        file.DecompressTo(decompressed);
        WriteFile(decompressed, hash, dir, fileIndex);
        decompressed.Clear();
        pool.Return(rent);
    }

    private static void WriteFile(ReadOnlySpan<byte> decompressed, ulong hash, string dir, int fileIndex)
    {
        var ext = TrinityUtil.GuessExtension(decompressed);
        var filepath = Path.Combine(dir, $"{fileIndex:0000} - {hash:X16}.{ext}");
        File.WriteAllBytes(filepath, decompressed);
    }
}
