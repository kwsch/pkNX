using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using FlatSharp.Attributes;

// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global

namespace pkNX.Structures.FlatBuffers;

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class EncounterMultiplierArchive8a : IFlatBufferArchive<EncounterMultiplier8a>
{
    public byte[] Write() => FlatBufferConverter.SerializeFrom(this);

    [FlatBufferItem(0)] public EncounterMultiplier8a[] Table { get; set; } = Array.Empty<EncounterMultiplier8a>();

    public EncounterMultiplier8a GetEncounterMultiplier(EncounterSlot8a slot)
    {
        var result = GetEntry((ushort)slot.Species, (ushort)slot.Form);
        if (result == null)
            throw new ArgumentException($"Invalid Encounter Slot {slot.Species} - {slot.Form}");
        return result;
    }

    public EncounterMultiplier8a GetEntry(ushort species, ushort form)
    {
        return Table.FirstOrDefault(x => x.Species == species && x.Form == form) ??
            new EncounterMultiplier8a { };
    }

    public bool HasEntry(ushort species, ushort form)
    {
        return Table.Any(x => x.Species == species && x.Form == form);
    }

    public EncounterMultiplier8a AddEntry(ushort species, ushort form)
    {
        Debug.Assert(!HasEntry(species, form), "The encounter rate table already contains an entry for the same species and form!");

        var entry = new EncounterMultiplier8a { Species = species, Form = form };
        Table = Table.Append(entry)
            .OrderBy(x => x.Species)
            .ThenBy(x => x.Form)
            .ToArray();
        return entry;
    }
}
