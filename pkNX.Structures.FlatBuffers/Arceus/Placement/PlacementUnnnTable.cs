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
public class PlacementUnnnTable : IFlatBufferArchive<PlacementUnnnEntry>
{
    public byte[] Write() => FlatBufferConverter.SerializeFrom(this);

    [FlatBufferItem(0)] public PlacementUnnnEntry[] Table { get; set; } = Array.Empty<PlacementUnnnEntry>();
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class PlacementUnnnEntry
{
    [FlatBufferItem(0)] public string Identifier { get; set; } = string.Empty;
    [FlatBufferItem(1)] public ulong Hash_01 { get; set; }
    [FlatBufferItem(2)] public PlacementParameters8a[] Field_03 { get; set; } = Array.Empty<PlacementParameters8a>();
    [FlatBufferItem(3)] public ulong Hash_03 { get; set; }
    [FlatBufferItem(4)] public int Number { get; set; }
    [FlatBufferItem(5)] public bool Flag { get; set; }

    public PlacementParameters8a Parameters
    {
        get { if (Field_03.Length != 1) throw new ArgumentException($"Invalid {nameof(Field_03)}"); return Field_03[0]; }
        set { if (Field_03.Length != 1) throw new ArgumentException($"Invalid {nameof(Field_03)}"); Field_03[0] = value; }
    }

    public IEnumerable<PlacementLocation8a> GetIntersectingLocations(IReadOnlyList<PlacementLocation8a> locations, float bias)
    {
        var c = Parameters.Coordinates;
        return GetIntersectingLocations(locations, bias, c, 0);
    }

    private static IEnumerable<PlacementLocation8a> GetIntersectingLocations(IReadOnlyList<PlacementLocation8a> locations, float bias, PlacementV3f8a c, float scalar)
    {
        var result = new List<PlacementLocation8a>();
        foreach (var loc in locations)
        {
            if (!loc.IsNamedPlace)
                continue;
            if (loc.IntersectsSphere(c.X, c.Y, c.Z, scalar + bias))
                result.Add(loc);
        }

        if (result.Count == 0)
            result.Add(locations.First(l => l.IsNamedPlace)); // base area name
        return result;
    }

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
