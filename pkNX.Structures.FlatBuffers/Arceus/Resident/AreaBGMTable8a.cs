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
public class AreaBGMTable8a
{
    [FlatBufferItem(00)] public ulong Hash { get; set; }
    [FlatBufferItem(01)] public string Name { get; set; } = string.Empty;
    [FlatBufferItem(02)] public AreaBGM8a[] Entries { get; set; } = Array.Empty<AreaBGM8a>();
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class AreaBGM8a
{
    [FlatBufferItem(00)] public int TimeStart { get; set; }
    [FlatBufferItem(01)] public int TimeEnd { get; set; }
    [FlatBufferItem(02)] public string StandardBGM { get; set; } = string.Empty;
    [FlatBufferItem(03)] public string FirstBGM { get; set; } = string.Empty;
    [FlatBufferItem(04)] public ulong FirstFlagHash { get; set; }  //A flag from system_flags
}
