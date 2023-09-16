using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace pkNX.Containers.VFS;

public class GFPackArchiveEntry : BaseArchiveEntry
{
    public GFPackFileHeader Header { get; init; }

    internal GFPackArchiveEntry(GFPackArchive archive, ulong uniqueId, GFPackFileHeader header)
        : base(archive, uniqueId)
    {
        Header = header;
        OffsetOfCompressedData = header.OffsetPacked;
        _compressedSize = header.SizeCompressed;
        _uncompressedSize = header.SizeDecompressed;
    }

    internal GFPackArchiveEntry(GFPackArchive archive, string entryPath)
        : base(archive, archive.GetUniqueIdentifierFromPath(entryPath))
    {
    }

    internal GFPackArchiveEntry(GFPackArchive archive, string entryPath, int compressionLevel)
        : base(archive, archive.GetUniqueIdentifierFromPath(entryPath))
    {
    }

    protected internal override bool LoadCompressedBytesIfNeeded()
    {
        throw new NotImplementedException();
    }

    protected override void ThrowIfUnsupportedCompressionType()
    {
        switch (Header.Type)
        {
            case CompressionType.None:
            case CompressionType.Lz4:
            case CompressionType.OodleKraken:
            case CompressionType.OodleLeviathan:
            case CompressionType.OodleMermaid:
            case CompressionType.OodleSelkie:
            case CompressionType.OodleHydra:
                break;
            case CompressionType.Zlib:
                throw new NotSupportedException(nameof(CompressionType.Zlib)); // not implemented
            default:
                throw new ArgumentOutOfRangeException(nameof(Header.Type));
        }
    }

    internal override void WriteAndFinishLocalEntry()
    {
        throw new NotImplementedException();
    }
}

public class GFPackArchive : BaseArchive
{
    public const ulong Magic = 0x4B434150_584C4647; // GFLXPACK
    public GFPackHeader Header { get; set; }
    public GFPackPointers Pointers { get; set; }
    public ulong[] HashAbsolute { get; set; }
    public FileHashFolder[] HashInFolder { get; set; }
    public GFPackFileHeader[] FileTable { get; set; }

    public GFPackArchive(Stream stream, ArchiveOpenMode openMode = ArchiveOpenMode.Read, bool leaveOpen = false) :
        base(stream, openMode, leaveOpen)
    {
        Initialize();
    }

    protected override void ReadHeader()
    {
        Debug.Assert(ArchiveReader != null);

        Header = ArchiveReader.ReadBytes(GFPackHeader.SIZE).ToClass<GFPackHeader>();
        Debug.Assert(Header.MAGIC == Magic);

        _expectedNumberOfEntries = Header.CountFiles;
    }

    protected override void ReadDirectory()
    {
        Debug.Assert(ArchiveReader != null);

        Pointers = new GFPackPointers(ArchiveReader, Header.CountFolders);
        Debug.Assert(Pointers.PtrHashPaths == ArchiveReader.BaseStream.Position);

        HashAbsolute = new ulong[Header.CountFiles];
        for (int i = 0; i < HashAbsolute.Length; i++)
            HashAbsolute[i] = ArchiveReader.ReadUInt64();

        HashInFolder = new FileHashFolder[Header.CountFolders];
        for (int f = 0; f < HashInFolder.Length; f++)
        {
            Debug.Assert(Pointers.PtrHashFolders[f] == ArchiveReader.BaseStream.Position);
            var table = HashInFolder[f] = new FileHashFolder
            {
                Folder = ArchiveReader.ReadBytes(FileHashIndex.SIZE).ToClass<FileHashFolderInfo>(),
            };
            table.Files = new FileHashIndex[table.Folder.FileCount];
            for (int i = 0; i < table.Files.Length; i++)
                table.Files[i] = ArchiveReader.ReadBytes(FileHashIndex.SIZE).ToClass<FileHashIndex>();
        }

        Debug.Assert(Pointers.PtrFileTable == ArchiveReader.BaseStream.Position);
        FileTable = new GFPackFileHeader[Header.CountFiles];
        for (int i = 0; i < FileTable.Length; i++)
            FileTable[i] = ArchiveReader.ReadBytes(GFPackFileHeader.SIZE).ToClass<GFPackFileHeader>();

        for (int i = 0; i < FileTable.Length; ++i)
        {
            AddEntry(new GFPackArchiveEntry(this, HashAbsolute[i], FileTable[i]));
        }
    }

    public override BaseArchiveEntry CreateEntry(string entryName, int compressionLevel = 9)
    {
        ThrowIfCantCreateEntry(entryName);

        BaseArchiveEntry entry = new GFPackArchiveEntry(this, entryName, compressionLevel);

        AddEntry(entry);

        return entry;
    }

    protected override void WriteFile()
    {
    }

    public override ulong GetUniqueIdentifierFromPath(string entryPath)
    {
        return FnvHash.HashFnv1a_64(entryPath);
    }
}
