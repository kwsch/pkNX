using System;
using System.ComponentModel;
using System.Linq;
using FlatSharp.Attributes;
using pkNX.Containers;

// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global

namespace pkNX.Structures.FlatBuffers;

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class ArchiveContents8a : IFlatBufferArchive<ArchiveContent8a>
{
    public byte[] Write() => FlatBufferConverter.SerializeFrom(this);

    [FlatBufferItem(0)] public ArchiveContent8a[] Table { get; set; } = Array.Empty<ArchiveContent8a>();

    public ArchiveContent8a AddEntry()
    {
        var entry = new ArchiveContent8a();

        Table = Table.Append(entry);
        return entry;
    }

    public ArchiveContent8a AddEntry(string archiveFile, string[] internalArchiveFiles)
    {
        var entry = new ArchiveContent8a
        {
            ArchivePathHash = FnvHash.HashFnv1a_64(archiveFile),
            ArchiveContentPathHashes = internalArchiveFiles.Select(p => FnvHash.HashFnv1a_64(p)).ToArray(),
        };

        Table = Table.Append(entry);
        return entry;
    }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class ArchiveContent8a
{
    [FlatBufferItem(00)] public ulong ArchivePathHash { get; set; } // Eg. `bin/archive/pokemon/pm0025_00_00.gfpak` -> 0x9F0F82B368411FF
    [FlatBufferItem(01)] public ulong[] ArchiveContentPathHashes { get; set; } = Array.Empty<ulong>(); // Eg. `bin/pokemon/pm0025/pm0025_00_00/pm0025_00_00.trpokecfg` -> 0x132C0BBD4E633C28
}
