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
public class KeyAssignmentTable : IFlatBufferArchive<KeyAssignment>
{
    public byte[] Write() => FlatBufferConverter.SerializeFrom(this);

    [FlatBufferItem(0)] public KeyAssignment[] Table { get; set; } = Array.Empty<KeyAssignment>();
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class KeyAssignment
{
    [FlatBufferItem(00)] public string Field_00 { get; set; } = string.Empty;
    [FlatBufferItem(01)] public string[] Field_01 { get; set; } = Array.Empty<string>();
    [FlatBufferItem(02)] public byte Field_02 { get; set; }
}
