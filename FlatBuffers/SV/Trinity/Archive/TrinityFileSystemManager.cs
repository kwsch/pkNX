using System.Diagnostics;
using FlatSharp;
using pkNX.Containers;

namespace pkNX.Structures.FlatBuffers.SV.Trinity;

/// <summary>
/// Wrapper object that extracts raw data from a Trinity Pack Filesystem file.
/// </summary>
public sealed class TrinityFileSystemManager : IDisposable, IFileInternal
{
    private readonly BinaryReader Reader;
    private readonly TrinityFileDescriptors FileData;
    private readonly TrinityFileSystemMetadata Meta;
    public const ulong MAGIC_ONEPACK = 0x004B4341_50454E4F; // ONEPACK\x00

    public TrinityFileSystemManager(string pathFs, string pathFd)
    {
        var trpfs = File.OpenRead(pathFs); // DO NOT DISPOSE UNTIL THIS OBJECT IS DISPOSED
        var br = new BinaryReader(trpfs);
        var totalLength = br.BaseStream.Length;

        // Read the trpfs meta -- check the file header first.
        var magic = br.ReadUInt64();
        Debug.Assert(magic == MAGIC_ONEPACK);

        // Locate the meta FlatBuffer
        var ofsMetaFB = br.ReadInt64();
        br.BaseStream.Seek(ofsMetaFB, SeekOrigin.Begin);

        // Extract the meta FlatBuffer
        var dataMetaFB = br.ReadBytes((int)(totalLength - ofsMetaFB));
        var meta = TrinityFileSystemMetadata.Serializer.Parse(dataMetaFB, FlatBufferDeserializationOption.GreedyMutable);

        // Read the trpfd
        var dataFd = File.ReadAllBytes(pathFd);
        var fd = TrinityFileDescriptors.Serializer.Parse(dataFd, FlatBufferDeserializationOption.GreedyMutable);
        Debug.Assert(meta.FileHashes.Count == fd.FileInfos.Count);

        Reader = br;
        FileData = fd;
        Meta = meta;
    }

    public void Dispose()
    {
        Reader.Dispose();
    }

    public int FileCount => FileData.FilePaths.Count;
    public string GetPackPath(int packIndex) => FileData.FilePaths[packIndex];

    public byte[] GetData(ulong offset, ulong length)
    {
        Reader.BaseStream.Seek((long)offset, SeekOrigin.Begin);
        return Reader.ReadBytes((int)length);
    }

    public void GetData(ulong offset, ulong length, Span<byte> data)
    {
        Reader.BaseStream.Seek((long)offset, SeekOrigin.Begin);
        _ = Reader.Read(data[..(int)length]);
    }

    public TrinityPak GetPak(int index)
    {
        var data = GetPakData(index);
        return TrinityPak.Serializer.Parse(data, FlatBufferDeserializationOption.GreedyMutable);
    }

    public ulong GetPakHash(int index) => FnvHash.HashFnv1a_64(GetPackPath(index));
    public ulong GetPakLength(int index) => FileData.FileInfos[index].FileSize;
    public ulong GetPakOffset(ulong hash) => Meta.GetFileOffset(hash);

    public byte[] GetPakData(int index)
    {
        var size = GetPakLength(index);
        var hash = GetPakHash(index);
        var offset = GetPakOffset(hash);
        return GetData(offset, size);
    }

    public byte[] GetPackedFile(ulong hash)
    {
        var index = FileData.GetSubFileIndex(hash);
        var pak = GetPak((int)index);
        var file = pak.GetFileData(hash);
        return file.Decompress();
    }

    public byte[] GetPackedFile(string path) => GetPackedFile(FnvHash.HashFnv1a_64(path));
    public bool HasFile(string path) => HasFile(FnvHash.HashFnv1a_64(path));
    public bool HasFile(ulong hash)
    {
        if (Meta.HasFile(hash))
            return true;
        return FileData.GetHasSubFile(hash);
    }
}
