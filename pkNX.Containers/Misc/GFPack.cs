using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace pkNX.Containers
{
    public class GFPack : IEnumerable<byte[]>, IFileContainer
    {
        public const ulong Magic = 0x4B434150_584C4647; // GFLXPACK

        // Overall structure: Header, metadata, and the raw compressed files
        public GFPackHeader Header { get; set; }
        public GFPackPointers Pointers { get; set; }
        public FileHashAbsolute[] HashAbsolute { get; set; }
        public FileHashFolder[] HashInFolder { get; set; }
        public FileData[] FileTable { get; set; }

        public byte[][] CompressedFiles { get; set; }
        public byte[][] DecompressedFiles { get; set; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public GFPack(string path) : this(FileMitm.ReadAllBytes(path)) => FilePath = path;
        public GFPack(string[] directories, string parent = @"\bin") => LoadFiles(directories, parent);

        /// <summary>
        /// Initializes a packed <see cref="GFPack"/> object, and unpacks the data to accessible properties.
        /// </summary>
        /// <param name="data">Packed file</param>
        public GFPack(byte[] data)
        {
            using var ms = new MemoryStream(data);
            using var br = new BinaryReader(ms);
            ReadPack(br);
        }

        public GFPack(BinaryReader br) => ReadPack(br);
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        private void ReadPack(BinaryReader br)
        {
            Header = br.ReadBytes(GFPackHeader.SIZE).ToClass<GFPackHeader>();
            Debug.Assert(Header.MAGIC == Magic);
            Pointers = new GFPackPointers(br, Header.CountFolders);

            Debug.Assert(Pointers.PtrHashPaths == br.BaseStream.Position);
            HashAbsolute = new FileHashAbsolute[Header.CountFiles];
            for (int i = 0; i < HashAbsolute.Length; i++)
                HashAbsolute[i] = br.ReadBytes(FileHashAbsolute.SIZE).ToClass<FileHashAbsolute>();

            HashInFolder = new FileHashFolder[Header.CountFolders];
            for (int f = 0; f < HashInFolder.Length; f++)
            {
                Debug.Assert(Pointers.PtrHashFolders[f] == br.BaseStream.Position);
                var table = HashInFolder[f] = new FileHashFolder
                {
                    Folder = br.ReadBytes(FileHashIndex.SIZE).ToClass<FileHashFolderInfo>()
                };
                table.Files = new FileHashIndex[table.Folder.FileCount];
                for (int i = 0; i < table.Files.Length; i++)
                    table.Files[i] = br.ReadBytes(FileHashIndex.SIZE).ToClass<FileHashIndex>();
            }

            Debug.Assert(Pointers.PtrFileTable == br.BaseStream.Position);
            FileTable = new FileData[Header.CountFiles];
            for (int i = 0; i < FileTable.Length; i++)
                FileTable[i] = br.ReadBytes(FileData.SIZE).ToClass<FileData>();

            CompressedFiles = new byte[Header.CountFiles][];
            for (int i = 0; i < CompressedFiles.Length; i++)
            {
                br.BaseStream.Position = FileTable[i].OffsetPacked;
                CompressedFiles[i] = br.ReadBytes(FileTable[i].SizeCompressed);
            }

            DecompressedFiles = new byte[Header.CountFiles][];
            for (int i = 0; i < DecompressedFiles.Length; i++)
                DecompressedFiles[i] = Decompress(CompressedFiles[i], FileTable[i].SizeDecompressed, FileTable[i].Type);
        }

        public IEnumerator<byte[]> GetEnumerator() => (IEnumerator<byte[]>)DecompressedFiles.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => DecompressedFiles.GetEnumerator();
        public int Count => Header.CountFiles;

        public byte[] this[int index]
        {
            get => (byte[])DecompressedFiles[index].Clone();
            set
            {
                Modified |= !DecompressedFiles[index].SequenceEqual(value);
                DecompressedFiles[index] = value;
            }
        }

        public int GetIndexFull(ulong hash) => Array.FindIndex(HashAbsolute, z => z.HashFnv1aPathFull == hash);
        public int GetIndexFull(string path) => GetIndexFull(FnvHash.HashFnv1a_64(path));

        public int GetIndexFileName(ulong hash)
        {
            foreach (var f in HashInFolder)
            {
                int index = f.GetIndexFileName(hash);
                if (index >= 0)
                    return f.Files[index].Index;
            }
            return -1;
        }

        public int GetIndexFileName(string name)
        {
            foreach (var f in HashInFolder)
            {
                int index = f.GetIndexFileName(name);
                if (index >= 0)
                    return f.Files[index].Index;
            }
            return -1;
        }

        public byte[] GetDataFileName(string name)
        {
            int index = GetIndexFileName(name);
            return DecompressedFiles[index];
        }

        public byte[] GetDataFull(ulong hash) => DecompressedFiles[GetIndexFull(hash)];

        public byte[] GetDataFullPath(string path) => GetDataFull(FnvHash.HashFnv1a_64(path));

        public void SetDataFileName(string name, byte[] data)
        {
            int index = GetIndexFileName(name);
            DecompressedFiles[index] = data;
        }

        public void SetDataFullPath(string path, byte[] data)
        {
            var hash = FnvHash.HashFnv1a_64(path);
            int index = GetIndexFull(hash);
            DecompressedFiles[index] = data;
        }

        public void LoadFiles(string[] directories, string parent, CompressionType type = CompressionType.Lz4)
        {
            var groups = directories.Select(Directory.GetFiles).ToArray();
            var files = groups.SelectMany(z => z).ToArray();
            Header = new GFPackHeader { CountFolders = directories.Length, CountFiles = files.Length };

            HashAbsolute = new FileHashAbsolute[files.Length];
            HashInFolder = new FileHashFolder[groups.Length];
            FileTable = new FileData[files.Length];
            DecompressedFiles = new byte[files.Length][];
            CompressedFiles = new byte[files.Length][];

            for (int f = 0; f < groups.Length; f++)
            {
                var folderFiles = groups[f];
                var folderName = Path.GetDirectoryName(directories[f]);
                var table = HashInFolder[f] = new FileHashFolder();
                table.Folder = new FileHashFolderInfo
                {
                    FileCount = folderFiles.Length,
                    HashFnv1aPathFolderName = FnvHash.HashFnv1a_64(folderName),
                };
                table.Files = new FileHashIndex[folderFiles.Length];
                for (int i = 0; i < folderFiles.Length; i++)
                {
                    var file = folderFiles[i];
                    int index = Array.IndexOf(files, folderFiles[i]);
                    var nameshort = Path.GetFileName(file);
                    ulong hashShort = FnvHash.HashFnv1a_64(nameshort);
                    table.Files[i] = new FileHashIndex { HashFnv1aPathFileName = hashShort, Index = index };
                }
            }

            for (var i = 0; i < files.Length; i++)
            {
                var file = files[i];
                var namelong = file[file.IndexOf(parent, StringComparison.Ordinal)..];
                ulong hashFull = FnvHash.HashFnv1a_64(namelong);

                HashAbsolute[i] = new FileHashAbsolute { HashFnv1aPathFull = hashFull };
                FileTable[i] = new FileData { Type = type };
                DecompressedFiles[i] = FileMitm.ReadAllBytes(file);
            }
            Modified = true;
        }

        public byte[] Write()
        {
            using var ms = new MemoryStream();
            using var bw = new BinaryWriter(ms);
            WriteHeaderTableList(bw);
            for (var i = 0; i < DecompressedFiles.Length; i++)
            {
                var entry = FileTable[i];
                var f = DecompressedFiles[i];
                var c = Compress(f, entry.Type);
                CompressedFiles[i] = c;

                // update entry details
                entry.SizeDecompressed = f.Length;
                entry.SizeCompressed = c.Length;
                entry.OffsetPacked = (int)bw.BaseStream.Position;

                bw.Write(c);
                while (bw.BaseStream.Position % 0x10 != 0) // pad to nearest 0x10 alignment
                    bw.Write((byte)0);
            }
            bw.BaseStream.Position = 0;
            WriteHeaderTableList(bw);
            return ms.ToArray();
        }

        private static byte[] Decompress(byte[] encryptedData, int decryptedLength, CompressionType type)
        {
            return type switch
            {
                CompressionType.None => encryptedData,
                CompressionType.Zlib => throw new NotSupportedException(nameof(CompressionType.Zlib)), // not implemented
                CompressionType.Lz4 => LZ4.Decode(encryptedData, decryptedLength),
                CompressionType.OodleKraken => Oodle.Decompress(encryptedData, decryptedLength)!,
                CompressionType.OodleLeviathan => Oodle.Decompress(encryptedData, decryptedLength)!,
                CompressionType.OodleMermaid => Oodle.Decompress(encryptedData, decryptedLength)!,
                CompressionType.OodleSelkie => Oodle.Decompress(encryptedData, decryptedLength)!,
                CompressionType.OodleHydra => Oodle.Decompress(encryptedData, decryptedLength)!,
                _ => throw new ArgumentOutOfRangeException(nameof(type)),
            };
        }

        private static byte[] Compress(byte[] decryptedData, CompressionType type)
        {
            return type switch
            {
                CompressionType.None => decryptedData,
                CompressionType.Zlib => throw new NotSupportedException(nameof(CompressionType.Zlib)), // not implemented
                CompressionType.Lz4 => LZ4.Encode(decryptedData),
                CompressionType.OodleKraken => Oodle.Compress(decryptedData, out _, OodleFormat.Kraken).ToArray(),
                CompressionType.OodleLeviathan => Oodle.Compress(decryptedData, out _, OodleFormat.Leviathan).ToArray(),
                CompressionType.OodleMermaid => Oodle.Compress(decryptedData, out _, OodleFormat.Mermaid).ToArray(),
                CompressionType.OodleSelkie => Oodle.Compress(decryptedData, out _, OodleFormat.Selkie).ToArray(),
                CompressionType.OodleHydra => Oodle.Compress(decryptedData, out _, OodleFormat.Hydra).ToArray(),
                _ => throw new ArgumentOutOfRangeException(nameof(type)),
            };
        }

        public void CancelEdits()
        {
            for (int i = 0; i < DecompressedFiles.Length; i++)
                DecompressedFiles[i] = Decompress(CompressedFiles[i], FileTable[i].SizeDecompressed, FileTable[i].Type);
            Modified = false;
        }

        private void WriteHeaderTableList(BinaryWriter bw)
        {
            bw.Write(Header.ToBytesClass());
            Pointers.Write(bw);
            Pointers.PtrHashPaths = bw.BaseStream.Position;
            foreach (var hp in HashAbsolute)
                bw.Write(hp.ToBytesClass());

            for (var f = 0; f < Pointers.PtrHashFolders.Length; f++)
            {
                Pointers.PtrHashFolders[f] = bw.BaseStream.Position;

                var folder = HashInFolder[f];
                folder.Folder.FileCount = folder.Files.Length;
                bw.Write(folder.Folder.ToBytesClass());
                foreach (var hi in folder.Files)
                    bw.Write(hi.ToBytesClass());
            }

            Pointers.PtrFileTable = bw.BaseStream.Position;
            foreach (var ft in FileTable)
                bw.Write(ft.ToBytesClass());
        }

        public string? FilePath { get; set; }
        public bool Modified { get; set; }
        public Task<byte[][]> GetFiles() => Task.FromResult(DecompressedFiles);
        public Task<byte[]> GetFile(int file, int subFile = 0) => Task.FromResult(this[file]);
        public Task SetFile(int file, byte[] value, int subFile = 0) => Task.FromResult(this[file] = value);
        public Task SaveAs(string path, ContainerHandler handler, CancellationToken token) => new(() => FileMitm.WriteAllBytes(path, Write()), token);

        public void Dump(string path, ContainerHandler handler)
        {
            handler.Initialize(FileTable.Length);
            string format = this.GetFileFormatString();

            foreach (var grp in HashInFolder)
            {
                var dirName = grp.Folder.HashFnv1aPathFolderName;
                foreach (var f in grp.Files)
                {
                    var index = f.Index;
                    var name = f.HashFnv1aPathFileName;

                    var fn = $"{index.ToString(format)} {name:X16}.bin";
                    var data = DecompressedFiles[f.Index];

                    var subfolder = HashInFolder.Length == 1 ? fn : Path.Combine(dirName.ToString("X16"), fn);
                    var loc = Path.Combine(path, subfolder);
                    FileMitm.WriteAllBytes(loc, data);
                }
            }
        }
    }

    /// <summary>
    /// Intro bytes to the packed binary.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public class GFPackHeader
    {
        public const int SIZE = 0x18;
        public ulong MAGIC = GFPack.Magic;
        public uint Version = 0x1000;
        public uint IsRelocated; // bit0

        /// <summary>
        /// Count of Files packed into the binary.
        /// </summary>
        public int CountFiles;

        /// <summary>
        /// Count of Folders packed into the binary.
        /// </summary>
        public int CountFolders;
    }

    public class GFPackPointers
    {
        /// <summary>
        /// Data offset for the <see cref="FileData"/> table.
        /// </summary>
        public long PtrFileTable; // array stored at end

        /// <summary>
        /// Data offset for the <see cref="FileHashAbsolute"/> table.
        /// </summary>
        public long PtrHashPaths; // array stored first

        /// <summary>
        /// Data offset for the <see cref="FileHashFolder"/> table, which has a leading <see cref="FileHashFolderInfo"/>.
        /// </summary>
        public long[] PtrHashFolders; // array stored in middle

        // immediately after the pointers are the arrays

        public GFPackPointers(BinaryReader br, int folderCount)
        {
            PtrFileTable = br.ReadInt64();
            PtrHashPaths = br.ReadInt64();
            PtrHashFolders = new long[folderCount];
            for (int i = 0; i < PtrHashFolders.Length; i++)
                PtrHashFolders[i] = br.ReadInt64();
        }

        public void Write(BinaryWriter bw)
        {
            bw.Write(PtrFileTable);
            bw.Write(PtrHashPaths);
            foreach (var table in PtrHashFolders)
                bw.Write(table);
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public class FileHashAbsolute
    {
        public const int SIZE = 0x08;

        /// <summary>
        /// Filename (with directory details) hash.
        /// </summary>
        public ulong HashFnv1aPathFull;

        public bool IsMatch(string fileName) => FnvHash.HashFnv1a_64(fileName) == HashFnv1aPathFull;
    }

    public class FileHashFolder
    {
        public FileHashFolderInfo Folder = new();
        public FileHashIndex[] Files = Array.Empty<FileHashIndex>();
        public int GetIndexFileName(ulong hash) => Array.FindIndex(Files, z => z.HashFnv1aPathFileName == hash);
        public int GetIndexFileName(string name) => Array.FindIndex(Files, z => z.IsMatch(name));
    }

    [StructLayout(LayoutKind.Sequential)]
    public class FileHashFolderInfo
    {
        public const int SIZE = 0x10;

        /// <summary>
        /// Filename (without directory details) hash.
        /// </summary>
        public ulong HashFnv1aPathFolderName;
        public int FileCount;
        public uint Padding = 0xCC;

        public bool IsMatch(string fileName) => FnvHash.HashFnv1a_64(fileName) == HashFnv1aPathFolderName;
    }

    [StructLayout(LayoutKind.Sequential)]
    public class FileHashIndex
    {
        public const int SIZE = 0x10;

        /// <summary>
        /// Filename (without directory details) hash.
        /// </summary>
        public ulong HashFnv1aPathFileName;
        public int Index;
        public uint Padding = 0xCC;

        public bool IsMatch(string fileName) => FnvHash.HashFnv1a_64(fileName) == HashFnv1aPathFileName;
    }

    [StructLayout(LayoutKind.Sequential)]
    public class FileData
    {
        public const int SIZE = 0x18;

        public ushort Level = 9; // quality?
        public CompressionType Type;
        public int SizeDecompressed;
        public int SizeCompressed;
        public int Padding = 0xCC;
        public int OffsetPacked;
        public uint unused;
    }

    public enum CompressionType : ushort
    {
        None = 0,
        Zlib = 1,
        Lz4 = 2,
        OodleKraken = 3,
        OodleLeviathan = 4,
        OodleMermaid = 5,
        OodleSelkie = 6,
        OodleHydra = 7,
    }
}
