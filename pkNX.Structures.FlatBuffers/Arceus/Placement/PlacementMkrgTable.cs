using System;
using System.Collections.Generic;
using System.ComponentModel;
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
public class PlacementMkrgTable : IFlatBufferArchive<PlacementMkrgEntry>
{
    public byte[] Write() => FlatBufferConverter.SerializeFrom(this);

    [FlatBufferItem(0)] public PlacementMkrgEntry[] Table { get; set; } = Array.Empty<PlacementMkrgEntry>();
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class PlacementMkrgEntry
{
    [FlatBufferItem(0)] public string Identifier { get; set; } = string.Empty;
    [FlatBufferItem(1)] public ulong Hash_01 { get; set; }
    [FlatBufferItem(2)] public PlacementParameters8a[] Field_02 { get; set; } = Array.Empty<PlacementParameters8a>();
    [FlatBufferItem(3)] public float Field_03 { get; set; }
    [FlatBufferItem(4)] public float Field_04 { get; set; }

    public PlacementParameters8a Parameters
    {
        get { if (Field_02.Length != 1) throw new ArgumentException($"Invalid {nameof(Field_02)}"); return Field_02[0]; }
        set { if (Field_02.Length != 1) throw new ArgumentException($"Invalid {nameof(Field_02)}"); Field_02[0] = value; }
    }

    public override string ToString() => $"Mikaruge(\"{Identifier}\", 0x{Hash_01:X16}, {Parameters}, {Field_03}, {Field_04})";

    public IEnumerable<PlacementLocation8a> GetContainingLocations(IReadOnlyList<PlacementLocation8a> locations)
    {
        var result = new List<PlacementLocation8a>();
        foreach (var loc in locations)
        {
            if (!loc.IsNamedPlace)
                continue;
            if (loc.Contains(Parameters.Coordinates))
                result.Add(loc);
        }
        if (result.Count == 0)
            result.Add(locations.First(l => l.IsNamedPlace)); // base area name
        return result;
    }
}
