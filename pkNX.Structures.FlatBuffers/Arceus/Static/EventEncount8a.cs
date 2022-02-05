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
public class EventEncount8a
{
    [FlatBufferItem(0)] public string EncounterName { get; set; } = string.Empty;
    [FlatBufferItem(1)] public string Field_01 { get; set; } = string.Empty;
    [FlatBufferItem(2)] public int Field_02 { get; set; } // All Entries have empty
    [FlatBufferItem(3)] public EventEncountPoke8a[]? Table { get; set; } = Array.Empty<EventEncountPoke8a>();
}
