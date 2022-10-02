using System.ComponentModel;
using FlatSharp.Attributes;
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global
#nullable disable
#pragma warning disable CA1819 // Properties should not return arrays

namespace pkNX.Structures.FlatBuffers;

// archive_contents.bin -- 7707 entry table (file, files)
[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class ArchiveContents : IFlatBufferArchive<ArchiveContent>
{
    [FlatBufferItem(00)] public ArchiveContent[] Table { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class ArchiveContent
{
    [FlatBufferItem(00)] public ulong Hash { get; set; }
    [FlatBufferItem(01)] public string Hashes { get; set; }
}
