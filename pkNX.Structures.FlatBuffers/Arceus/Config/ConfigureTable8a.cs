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
public class ConfigArchive8a : IFlatBufferArchive<ConfigEntry8a>
{
    public byte[] Write() => FlatBufferConverter.SerializeFrom(this);

    [FlatBufferItem(0)] public ConfigEntry8a[] Table { get; set; } = Array.Empty<ConfigEntry8a>();
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class ConfigEntry8a
{
    [FlatBufferItem(00)] public string Name { get; set; } = string.Empty;
    [FlatBufferItem(01)] public ulong Hash { get; set; }
    [FlatBufferItem(02)] public int Field_02 { get; set; }
    [FlatBufferItem(03)] public string DebugMin { get; set; } = string.Empty;
    [FlatBufferItem(04)] public string DebugMax { get; set; } = string.Empty;
    [FlatBufferItem(05)] public string[] Parameters { get; set; } = Array.Empty<string>();
    [FlatBufferItem(06)] public string Description { get; set; } = string.Empty;

    public string ConfiguredValue
    {
        get => Parameters[0];
        set => Parameters[0] = value;
    }
}
