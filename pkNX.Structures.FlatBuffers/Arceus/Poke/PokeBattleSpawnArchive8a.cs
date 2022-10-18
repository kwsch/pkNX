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

/// <summary>
/// Seems to be a file left over from development. Only has test entries
/// </summary>
[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class PokeBattleSpawnArchive8a : IFlatBufferArchive<PokeBattleSpawn8a>
{
    public byte[] Write() => FlatBufferConverter.SerializeFrom(this);

    [FlatBufferItem(00)] public PokeBattleSpawn8a[] Table { get; set; } = Array.Empty<PokeBattleSpawn8a>();
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class PokeBattleSpawn8a
{
    [FlatBufferItem(00)] public string Field_00 { get; set; } = string.Empty;
    [FlatBufferItem(01)] public string[] Field_01 { get; set; } = Array.Empty<string>();
    [FlatBufferItem(02)] public string[] Field_02 { get; set; } = Array.Empty<string>();
    [FlatBufferItem(03)] public float Field_03 { get; set; }
    [FlatBufferItem(04)] public float Field_04 { get; set; }
    [FlatBufferItem(05)] public float Field_05 { get; set; }
}
