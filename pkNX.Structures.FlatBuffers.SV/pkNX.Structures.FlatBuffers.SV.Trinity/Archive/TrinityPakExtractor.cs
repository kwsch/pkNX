using System;
using System.IO;
using FlatSharp;
using pkNX.Containers;

namespace pkNX.Structures.FlatBuffers.SV.Trinity;

public static class TrinityPakExtractor
{
    public const string DumpArchiveExtracted = "extracted";
    private const string DumpArchivePak = "paks";

    public static void DumpArchives(string pathRomFS, string outbasedir)
    {
        var pathFs = Path.Combine(pathRomFS, "arc", "data.trpfs");
        var pathFd = Path.Combine(pathRomFS, "arc", "data.trpfd");
        var reader = new TrinityFileSystemManager(pathFs, pathFd);
        Extract(outbasedir, reader);
        reader.Dispose();
    }

    private static void Extract(string outbasedir, TrinityFileSystemManager manager)
    {
        var outpakdir = Path.Combine(outbasedir, DumpArchivePak);
        var outexdir = Path.Combine(outbasedir, DumpArchiveExtracted);
        Directory.CreateDirectory(outbasedir);
        Directory.CreateDirectory(outpakdir);
        Directory.CreateDirectory(outexdir);

        var count = manager.FileCount;
        for (var i = 0; i < count; i++)
            ExportArc(manager, i, outpakdir, outexdir);
    }

    private static void ExportArc(TrinityFileSystemManager reader, int packIndex, string outpakdir, string outexdir)
    {
        var packFilePath = reader.GetPackPath(packIndex);
        var outpakpath = Path.Combine(outpakdir, packFilePath);
        var dirName = Path.GetDirectoryName(outpakpath);
        if (string.IsNullOrEmpty(dirName))
            throw new Exception($"{outpakpath} directory name is null");
        Directory.CreateDirectory(dirName);

        var data = reader.GetPakData(packIndex);
        File.WriteAllBytes(outpakpath, data);

        var outexpakdir = Path.Combine(outexdir, packFilePath);
        Directory.CreateDirectory(outexpakdir);
        var trpak = FlatBufferConverter.DeserializeFrom<TrinityPak>(data, FlatBufferDeserializationOption.GreedyMutable);
        ExtractPack(trpak, outexpakdir);
    }

    private static void ExtractPack(TrinityPak trpak, string outexpakdir)
    {
        for (var i = 0; i < trpak.Files.Count; i++)
            WriteFile(trpak.Files[i], trpak.Hashes[i], outexpakdir, i);
    }

    private static void WriteFile(TrinityPakFileData file, ulong hash, string outexpakdir, int fileIndex)
    {
        var decompressed = file.GetUncompressedDataReadOnly();
        var ext = GuessExtension(decompressed);

        var filepath = Path.Combine(outexpakdir, $"{fileIndex:0000} - {hash:X16}.{ext}");
        //Debug.WriteLine($"    * File, CompressionType={file.CompressionType}, CompressedSize={file.Data.Length:X}, UncompressedSize={file.UncompressedSize}, Field_00={file.Field_00}, Field_01={file.Field_01}");

        using var fs = File.Create(filepath);
        fs.Write(decompressed);
    }

    private static string GuessExtension(ReadOnlySpan<byte> data)
    {
        const string defaultExtension = "bin";
        if (data.Length < 4)
            return defaultExtension;
        var u32 = System.Buffers.Binary.BinaryPrimitives.ReadUInt32LittleEndian(data);
        return u32 switch
        {
            AHTB.Magic => "ahtb",
            0x43524153 => "sarc",
            0x58544E42 => "bntx",
            0x63726173 => "sarc",
            _ => defaultExtension,
        };
    }
}
