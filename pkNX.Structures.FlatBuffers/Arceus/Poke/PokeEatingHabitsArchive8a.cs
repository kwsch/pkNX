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
public class PokeEatingHabitsArchive8a : IFlatBufferArchive<PokeEatingHabits8a>
{
    public byte[] Write() => FlatBufferConverter.SerializeFrom(this);

    [FlatBufferItem(00)] public PokeEatingHabits8a[] Table { get; set; } = Array.Empty<PokeEatingHabits8a>();
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class PokeEatingHabits8a
{
    [FlatBufferItem(00)] public uint ID { get; set; }
    [FlatBufferItem(01)] public uint Field_01 { get; set; }
    [FlatBufferItem(02)] public uint Field_02 { get; set; }
    [FlatBufferItem(03)] public uint Field_03 { get; set; }
    [FlatBufferItem(04)] public uint Field_04 { get; set; }
    [FlatBufferItem(05)] public uint Field_05 { get; set; }
    [FlatBufferItem(06)] public uint Field_06 { get; set; }
}
