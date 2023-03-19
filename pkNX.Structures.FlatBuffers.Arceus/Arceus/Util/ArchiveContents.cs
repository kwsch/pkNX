using System.ComponentModel;

// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global

namespace pkNX.Structures.FlatBuffers.Arceus;

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class ArchiveContents
{
    public ArchiveContent AddEntry()
    {
        var entry = new ArchiveContent
        {
            ArchiveContentPathHashes = new List<ulong>(),
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

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class ArchiveContent { }
