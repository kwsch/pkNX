using System;
using System.Diagnostics;
using System.IO;
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
        var meta = TrinityFileSystemMetadata.Serializer.Parse(dataMetaFB, FlatBufferDeserializationOption.Progressive);

        // Read the trpfd
        var dataFd = File.ReadAllBytes(pathFd);
        var fd = TrinityFileDescriptors.Serializer.Parse(dataFd, FlatBufferDeserializationOption.Progressive);
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

    public TrinityPak GetPak(int index)
    {
        var data = GetPakData(index);
        return TrinityPak.Serializer.Parse(data);
    }

    public byte[] GetPakData(int index)
    {
        var path = GetPackPath(index);
        var hash = FnvHash.HashFnv1a_64(path);
        var offset = Meta.GetFileOffset(hash);
        var info = FileData.FileInfos[index];
        var size = info.FileSize;

        //Debug.WriteLine($"Found {index} at 0x{offset:X12}, Size={info.FileSize:X}, SubFileCount={info.FileCount}");

        return GetData(offset, size);
    }

    public byte[] GetPackedFile(ulong hash)
    {
        var index = FileData.GetSubFileIndex(hash);
        var pak = GetPak((int)index);
        var file = pak.GetFileData(hash);
        return file.GetUncompressedData();
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
