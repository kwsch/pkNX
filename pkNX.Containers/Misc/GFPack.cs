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
        private const ulong Magic = 0x4B434150_584C4647; // GFLXPACK

        // Overall structure: Header, metadata, and the raw compressed files
        public GFPackHeader Header { get; set; }
        public FileHashPath[] HashPaths { get; set; }
        public FileHashIndex[] HashIndexes { get; set; }
        public FileData[] FileTable { get; set; }
        public byte[][] CompressedFiles { get; set; }
        public byte[][] DecompressedFiles { get; set; }

        public GFPack(string path) : this(FileMitm.ReadAllBytes(path)) => FilePath = path;
        public GFPack(string[] directories, string parent = @"\bin") => LoadFiles(directories, parent);

        /// <summary>
        /// Initializes a packed <see cref="GFPack"/> object, and unpacks the data to accessible properties.
        /// </summary>
        /// <param name="data">Packed file</param>
        public GFPack(byte[] data)
        {
            using (var ms = new MemoryStream(data))
            using (var br = new BinaryReader(ms))
                ReadPack(br);
        }

        public GFPack(BinaryReader br) => ReadPack(br);

        private void ReadPack(BinaryReader br)
        {
            Header = br.ReadBytes(GFPackHeader.SIZE).ToClass<GFPackHeader>();
            Debug.Assert(Header.MAGIC == Magic);

            HashPaths = new FileHashPath[Header.CountFiles];
            for (int i = 0; i < HashPaths.Length; i++)
                HashPaths[i] = br.ReadBytes(FileHashPath.SIZE).ToClass<FileHashPath>();

            HashIndexes = new FileHashIndex[Header.CountFiles];
            for (int i = 0; i < HashIndexes.Length; i++)
                HashIndexes[i] = br.ReadBytes(FileHashIndex.SIZE).ToClass<FileHashIndex>();

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

        /// <summary>
        /// Intro bytes to the packed binary.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public class GFPackHeader
        {
            public const int SIZE = 0x40;
            public ulong MAGIC = Magic;
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

            /// <summary>
            /// Data offset for the <see cref="FileHashPath"/> table.
            /// </summary>
            public long PtrHashPaths;

            /// <summary>
            /// Data offset for the <see cref="FileHashIndex"/> table.
            /// </summary>
            public long PtrHashIndexes;

            /// <summary>
            /// Data offset for the <see cref="FileData"/> table.
            /// </summary>
            public long PtrFileTable;

            public ulong Reserved1;
            public ulong Reserved2;
        }

        [StructLayout(LayoutKind.Sequential)]
        public class FileHashPath
        {
            public const int SIZE = 0x08;

            /// <summary>
            /// Filename (with directory details) hash.
            /// </summary>
            public ulong HashFnv1aPathFull;

            public bool IsMatch(string fileName) => FnvHash.HashFnv1a_64(fileName) == HashFnv1aPathFull;
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

        public int GetIndexFull(ulong hash) => Array.FindIndex(HashPaths, z => z.HashFnv1aPathFull == hash);
        public int GetIndexFileName(ulong hash) => Array.FindIndex(HashIndexes, z => z.HashFnv1aPathFileName == hash);
        public int GetIndexFileName(string name) => Array.FindIndex(HashIndexes, z => z.IsMatch(name));

        public void LoadFiles(string[] directories, string parent, CompressionType type = CompressionType.Lz4)
        {
            var files = directories.SelectMany(Directory.GetFiles).ToArray();
            Header = new GFPackHeader { CountFolders = directories.Length, CountFiles = files.Length };

            HashPaths = new FileHashPath[files.Length];
            HashIndexes = new FileHashIndex[files.Length];
            FileTable = new FileData[files.Length];
            DecompressedFiles = new byte[files.Length][];
            CompressedFiles = new byte[files.Length][];
            for (var i = 0; i < files.Length; i++)
            {
                var file = files[i];
                var nameshort = Path.GetFileName(file);
                var namelong = file.Substring(file.IndexOf(parent, StringComparison.Ordinal));
                ulong hashShort = FnvHash.HashFnv1a_64(nameshort);
                ulong hashFull = FnvHash.HashFnv1a_64(namelong);

                HashPaths[i] = new FileHashPath {HashFnv1aPathFull = hashFull};
                HashIndexes[i] = new FileHashIndex {HashFnv1aPathFileName = hashShort, Index = i};
                FileTable[i] = new FileData {Type = type};
                DecompressedFiles[i] = FileMitm.ReadAllBytes(file);
            }
            Modified = true;
        }

        public byte[] Write()
        {
            using (var ms = new MemoryStream())
            using (var bw = new BinaryWriter(ms))
            {
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
                    // no padding
                }
                bw.BaseStream.Position = 0;
                WriteHeaderTableList(bw);
                return ms.ToArray();
            }
        }

        private static byte[] Decompress(byte[] encryptedData, int decryptedLength, CompressionType type)
        {
            switch (type)
            {
                case CompressionType.None:
                    return encryptedData;
                case CompressionType.Zlib:
                    return null; // not implemented
                default:
                    return LZ4.Decode(encryptedData, decryptedLength);
            }
        }

        private static byte[] Compress(byte[] decryptedData, CompressionType type)
        {
            switch (type)
            {
                case CompressionType.None:
                    return decryptedData;
                case CompressionType.Zlib:
                    return null; // not implemented
                default:
                    return LZ4.Encode(decryptedData);
            }
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
            Header.PtrHashPaths = bw.BaseStream.Position;
            bw.Write(HashPaths.ToBytesClass());
            Header.PtrHashIndexes = bw.BaseStream.Position;
            bw.Write(HashIndexes.ToBytesClass());
            Header.PtrFileTable = bw.BaseStream.Position;
            bw.Write(FileTable.ToBytesClass());
        }

        public string FilePath { get; set; }
        public bool Modified { get; set; }
        public Task<byte[][]> GetFiles() => Task.FromResult(DecompressedFiles);
        public Task<byte[]> GetFile(int file, int subFile = 0) => Task.FromResult(this[file]);
        public Task SetFile(int file, byte[] value, int subFile = 0) => Task.FromResult(this[file] = value);
        public Task SaveAs(string path, ContainerHandler handler, CancellationToken token) => new Task(() => FileMitm.WriteAllBytes(path, Write()), token);

        public void Dump(string path, ContainerHandler handler)
        {
            handler.Initialize(FileTable.Length);
            string format = this.GetFileFormatString();
            for (var i = 0; i < FileTable.Length; i++)
            {
                var hashFull = HashPaths[i].HashFnv1aPathFull;
                var fn = $"{i.ToString(format)} {hashFull:X16}.bin";
                var loc = Path.Combine(path ?? FilePath, fn);
                var data = DecompressedFiles[i];
                FileMitm.WriteAllBytes(loc, data);
                handler.StepFile(i, fileName: fn);
            }
        }
    }
}
