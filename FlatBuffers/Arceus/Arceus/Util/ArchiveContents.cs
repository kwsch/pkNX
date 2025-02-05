namespace pkNX.Structures.FlatBuffers.Arceus;

public partial class ArchiveContents
{
    public ArchiveContent AddEntry()
    {
        var entry = new ArchiveContent
        {
            ArchiveContentPathHashes = [],
        };

        Table = Table.Append(entry).ToList();
        return entry;
    }

    public ArchiveContent AddEntry(string archiveFile, string[] internalArchiveFiles)
    {
        var entry = new ArchiveContent
        {
            ArchivePathHash = FnvHash.HashFnv1a_64(archiveFile),
            ArchiveContentPathHashes = internalArchiveFiles.Select(p => FnvHash.HashFnv1a_64(p)).ToArray(),
        };

        Table = Table.Append(entry).ToList();
        return entry;
    }
}
