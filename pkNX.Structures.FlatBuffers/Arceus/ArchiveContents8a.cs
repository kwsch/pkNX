using System;
using System.ComponentModel;
using FlatSharp.Attributes;

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
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class ArchiveContent8a
{
    [FlatBufferItem(00)] public ulong Field_00 { get; set; }
    [FlatBufferItem(01)] public ulong[] Field_01 { get; set; } = Array.Empty<ulong>();
}
