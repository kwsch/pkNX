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
public class PokemonRare8aTable : IFlatBufferArchive<PokemonRare8aEntry>
{
    public byte[] Write() => FlatBufferConverter.SerializeFrom(this);

    [FlatBufferItem(0)] public PokemonRare8aEntry[] Table { get; set; } = Array.Empty<PokemonRare8aEntry>();
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class PokemonRare8aEntry
{
    [FlatBufferItem(00)] public string Name { get; set; } = string.Empty;
    [FlatBufferItem(01)] public ulong Hash { get; set; }
    [FlatBufferItem(02)] public byte UnusedValue { get; set; } // none have this
    [FlatBufferItem(03)] public string Option { get; set; } = string.Empty;
    [FlatBufferItem(04)] public string Value { get; set; } = string.Empty;
    [FlatBufferItem(05)] public string[] Field_05 { get; set; } = Array.Empty<string>();
    [FlatBufferItem(06)] public FlatDummyEntry[] UnusedArray { get; set; } = Array.Empty<FlatDummyEntry>(); // none have this
}
