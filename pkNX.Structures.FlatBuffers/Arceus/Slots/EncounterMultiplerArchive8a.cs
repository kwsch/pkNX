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
public class EncounterMultiplerArchive8a : IFlatBufferArchive<EncounterMultiplier8a>
{
    public byte[] Write() => FlatBufferConverter.SerializeFrom(this);

    [FlatBufferItem(0)] public EncounterMultiplier8a[] Table { get; set; } = Array.Empty<EncounterMultiplier8a>();

    public EncounterMultiplier8a GetEncounterMultiplier(EncounterSlot8a slot)
    {
        var result = Array.Find(Table, z => z.Species == slot.Species && z.Form == slot.Form);
        if (result == null)
            throw new ArgumentException($"Invalid Encounter Slot {slot.Species} - {slot.Form}");
        return result;
    }
}
