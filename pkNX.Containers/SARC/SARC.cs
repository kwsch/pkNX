using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace pkNX.Containers;

/// <summary>
/// Simple (?) ARChive
/// </summary>
public sealed class SARC : LargeContainer
{
    private const string Identifier = nameof(SARC);

    public SARCHeader Header { get; set; }
    public SFAT SFAT;
    public SFNT SFNT;

    public override int Count => SFAT.Entries.Count;

    /// <summary>
    /// The required <see cref="SARCHeader.Magic"/> matches the first 4 bytes of the file data.
    /// </summary>
    public bool SigMatches => Header.Magic == Identifier;

    /// <summary>
    /// Initializes an empty <see cref="SARC"/>.
    /// </summary>
    /// <param name="files">Files inside the <see cref="SARC"/>.</param>
    /// <param name="baseFolder">Root location of the archive</param>
    public SARC(IReadOnlyList<string> files, string baseFolder)
    {
        Header = new SARCHeader();
        SFAT = new SFAT(baseFolder, files);
        SFNT = new SFNT();
        Files = new byte[]?[SFAT.Entries.Count];
    }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    /// <summary>
    /// Initializes a <see cref="SARC"/> from a file location.
    /// </summary>
    /// <param name="path"></param>
    public SARC(string path) => OpenBinary(path);

    /// <summary>
    /// Initializes a <see cref="SARC"/> from a provided stream.
    /// </summary>
    /// <param name="fs"></param>
    public SARC(Stream fs) => OpenRead(new BinaryReader(fs));

    /// <summary>
    /// Initializes a <see cref="SARC"/> from a provided reader.
    /// </summary>
    /// <param name="br"></param>
    public SARC(BinaryReader br) => OpenRead(br);
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    /// <summary>
    /// Reads the contents of the <see cref="SARC"/> header and file info tables.
    /// </summary>
    protected override void Initialize()
    {
        if (Reader is null)
            throw new NullReferenceException(nameof(Reader));
        Header = new SARCHeader(Reader);
        if (!SigMatches)
            return;
        SFAT = new SFAT(Reader, Header.DataOffset);
        SFNT = new SFNT(Reader);
    }

    protected override Task Pack(BinaryWriter bw, ContainerHandler handler, CancellationToken token)
    {
        return Task.Run(() => PackSARC(bw, handler, token), token);
    }

    private void PackSARC(BinaryWriter bw, ContainerHandler handler, CancellationToken token)
    {
        StartPack();
        var count = SFAT.Entries.Count;
        handler.Initialize(count);
        var currentStringOffset = SFNT.StringOffset;

        WriteIntro(bw);
        for (int i = 0; i < count; i++)
        {
            var entr = SFAT.Entries[i];
            if (entr.FileName == null)
                entr.GetFileName(Reader!.BaseStream, (int)currentStringOffset);
            entr.WriteFileName(Reader!.BaseStream, (int)SFNT.StringOffset);
            while (Reader.BaseStream.Position % 4 != 0)
                bw.Write((byte)0);
        }
        for (int i = 0; i < count; i++)
        {
            if (token.IsCancellationRequested)
                return;
            WriteEntry(bw, i);
        }
        WriteIntro(bw, true);
        bw.Flush();
    }

    private static void StartPack() { } // do nothing?

    private void WriteIntro(BinaryWriter bw, bool finalPass = false)
    {
        Header.Write(bw);
        SFAT.Write(bw);
        SFNT.Write(bw);
        if (finalPass)
            Header.DataOffset = (int)bw.BaseStream.Position;
    }

    private void WriteEntry(BinaryWriter bw, int i)
    {
        throw new NotImplementedException();
    }

    protected override int GetFileOffset(int file, int subFile = 0)
    {
        var f = SFAT[file];
        return Header.DataOffset + f.Start;
    }

    public override byte[] GetEntry(int index, int subFile)
    {
        var f = SFAT[index];
        if (f.File is byte[] data)
            return data;

        data = f.GetFileData(Reader!.BaseStream);
        f.File = data; // cache for future fetches
        return data;
    }

    public override void Dump(string? path, ContainerHandler handler)
    {
        path ??= FilePath;
        if (File.Exists(path))
            path = Path.GetDirectoryName(path);
        ArgumentNullException.ThrowIfNull(path, nameof(path));

        var folder = FileName ?? Identifier;
        string dir = Path.Combine(path, folder);

        Directory.CreateDirectory(dir);
        var count = SFAT.Entries.Count;
        handler.Initialize(count);
        for (int i = 0; i < count; i++)
        {
            SFAT.Entries[i].Dump(Reader!.BaseStream, path, Header.DataOffset);
            handler.StepFile(i + 1);
        }
    }

    public static SARC? GetSARC(BinaryReader br)
    {
        if (br.BaseStream.Length < 20)
            return null;
        br.BaseStream.Position = 0;
        var ident = new string(br.ReadChars(4));
        if (ident != SARCHeader.Identifier)
            return null;
        return new SARC(br);
    }
}
