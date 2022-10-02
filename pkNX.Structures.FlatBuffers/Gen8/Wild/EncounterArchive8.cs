using System.ComponentModel;
using FlatSharp.Attributes;
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global
#nullable disable
#pragma warning disable CA1819 // Properties should not return arrays

namespace pkNX.Structures.FlatBuffers;

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class EncounterArchive8
{
    [FlatBufferItem(0)] public uint Field_00 { get; set; }
    [FlatBufferItem(1)] public EncounterTable8[] EncounterTables { get; set; }
}

[FlatBufferTable]
public class EncounterTable8
{
    [FlatBufferItem(0)] public ulong ZoneID { get; set; }
    [FlatBufferItem(1)] public EncounterSubTable8[] SubTables { get; set; }
}

[FlatBufferTable]
public class EncounterSubTable8
{
    [FlatBufferItem(0)] public byte LevelMin { get; set; }
    [FlatBufferItem(1)] public byte LevelMax { get; set; }
    [FlatBufferItem(2)] public EncounterSlot8[] Slots { get; set; }
}

[FlatBufferTable]
public class EncounterSlot8
{
    [FlatBufferItem(0)] public byte Probability { get; set; }
    [FlatBufferItem(1)] public int Species { get; set; }
    [FlatBufferItem(2)] public byte Form { get; set; }
}
