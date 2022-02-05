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
public class ScriptIDRecordRelease : IFlatBufferArchive<ScriptIDRecord>
{
    public byte[] Write() => FlatBufferConverter.SerializeFrom(this);

    [FlatBufferItem(0)] public ScriptIDRecord[] Table { get; set; } = Array.Empty<ScriptIDRecord>();
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class ScriptIDRecord
{
    [FlatBufferItem(00)] public string Name { get; set; } = string.Empty;
    [FlatBufferItem(01)] public string Type { get; set; } = string.Empty;
    [FlatBufferItem(02)] public string Lua  { get; set; } = string.Empty;
    [FlatBufferItem(03)] public string Dat  { get; set; } = string.Empty;
}
