﻿using System.ComponentModel;
using FlatSharp.Attributes;
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global
#nullable disable
#pragma warning disable CA1819 // Properties should not return arrays

namespace pkNX.Structures.FlatBuffers.Gen8.Other
{
    // script_id_record.bin
    [FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
    public class ScriptMeta
    {
        [FlatBufferItem(0)] public ScriptMetaEntry[] Commands { get; set; }
    }

    [FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
    public class ScriptMetaEntry
    {
        [FlatBufferItem(0)] public ulong Hash { get; set; }
        [FlatBufferItem(1)] public string PathAMX { get; set; }
        [FlatBufferItem(2)] public string Field2 { get; set; }
    }
}
