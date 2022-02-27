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
public class NewHugeOutbreakTimeLimitArchive8a : IFlatBufferArchive<NewHugeOutbreakTimeLimit8a>
{
    public byte[] Write() => FlatBufferConverter.SerializeFrom(this);

    [FlatBufferItem(0)] public NewHugeOutbreakTimeLimit8a[] Table { get; set; } = Array.Empty<NewHugeOutbreakTimeLimit8a>();
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class NewHugeOutbreakTimeLimit8a
{
    [FlatBufferItem(00)] public int Field_00 { get; set; } // not sure, is this instead pointing to 0x10 bytes 0-F?
    [FlatBufferItem(01)] public int Field_01 { get; set; }
    [FlatBufferItem(02)] public int[] Field_02 { get; set; } = Array.Empty<int>();
    [FlatBufferItem(03)] public int[] Field_03 { get; set; } = Array.Empty<int>();
}
