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
public class Configure8aEntry
{
    [FlatBufferItem(00)] public string Name { get; set; } = string.Empty;
    [FlatBufferItem(01)] public ulong Hash { get; set; }
    [FlatBufferItem(02)] public int Value { get; set; } // none have this
    [FlatBufferItem(03)] public string DebugMin { get; set; } = string.Empty;
    [FlatBufferItem(04)] public string DebugMax { get; set; } = string.Empty;
    [FlatBufferItem(05)] public string[] Parameters { get; set; } = Array.Empty<string>();
    [FlatBufferItem(06)] public FlatDummyEntry[] UnusedArray { get; set; } = Array.Empty<FlatDummyEntry>(); // none have this

    public string ConfiguredValue
    {
        get => Parameters[0];
        set => Parameters[0] = value;
    }
}
