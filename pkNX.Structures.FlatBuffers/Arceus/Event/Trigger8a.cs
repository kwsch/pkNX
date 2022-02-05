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
public class TriggerTable8a : IFlatBufferArchive<Trigger8a>
{
    public byte[] Write() => FlatBufferConverter.SerializeFrom(this);

    [FlatBufferItem(0)] public Trigger8a[] Table { get; set; } = Array.Empty<Trigger8a>();
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class Trigger8a
{
    [FlatBufferItem(00)] public Trigger8a_F00 Field_00 { get; set; } = new();
    [FlatBufferItem(01)] public Trigger8a_F01[] Field_01 { get; set; } = Array.Empty<Trigger8a_F01>();
    [FlatBufferItem(02)] public Trigger8a_F02[] Field_02 { get; set; } = Array.Empty<Trigger8a_F02>();
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class Trigger8a_F00
{
    [FlatBufferItem(00)] public ulong Field_00 { get; set; }
    [FlatBufferItem(01)] public ulong Field_01 { get; set; }
    [FlatBufferItem(02)] public string Field_02 { get; set; } = string.Empty;
    [FlatBufferItem(03)] public string Field_03 { get; set; } = string.Empty;
    [FlatBufferItem(04)] public string Field_04 { get; set; } = string.Empty; // assumed
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class Trigger8a_F01
{
    [FlatBufferItem(00)] public ulong Field_00 { get; set; }
    [FlatBufferItem(01)] public byte Field_01 { get; set; } // unk
    [FlatBufferItem(02)] public string Field_02 { get; set; } = string.Empty;
    [FlatBufferItem(03)] public string Field_03 { get; set; } = string.Empty; // assumed
    [FlatBufferItem(04)] public string Field_04 { get; set; } = string.Empty; // assumed
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class Trigger8a_F02
{
    [FlatBufferItem(00)] public ulong Field_00 { get; set; }
    [FlatBufferItem(01)] public string[] Field_01 { get; set; } = Array.Empty<string>();
    [FlatBufferItem(02)] public string[] Field_02 { get; set; } = Array.Empty<string>();
}
