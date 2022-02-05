﻿using System;
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
public class PlacementSearchItemTable : IFlatBufferArchive<PlacementSearchItem>
{
    public byte[] Write() => FlatBufferConverter.SerializeFrom(this);

    [FlatBufferItem(0)] public PlacementSearchItem[] Table { get; set; } = Array.Empty<PlacementSearchItem>();
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class PlacementSearchItem
{
    [FlatBufferItem(0)] public string Identifier { get; set; } = string.Empty;
    [FlatBufferItem(1)] public ulong Hash_01 { get; set; }
    [FlatBufferItem(2)] public PlacementParameters8a[] Field_02 { get; set; } = Array.Empty<PlacementParameters8a>();
    [FlatBufferItem(03)] public string Field_03 { get; set; } = string.Empty;
    [FlatBufferItem(04)] public string Field_04 { get; set; } = string.Empty;
    [FlatBufferItem(05)] public int Rate { get; set; }
    [FlatBufferItem(06)] public bool Field_06 { get; set; } // bool
    [FlatBufferItem(07)] public bool Field_07 { get; set; } // bool
    [FlatBufferItem(08)] public PlacementV3f8a Field_08 { get; set; } = new();
    [FlatBufferItem(09)] public byte Field_09 { get; set; } // bool
    [FlatBufferItem(10)] public PlacementV3f8a Field_10 { get; set; } = new();
    [FlatBufferItem(11)] public bool Field_11 { get; set; } // bool
    [FlatBufferItem(12)] public PlacementV3f8a Field_12 { get; set; } = new();
    [FlatBufferItem(13)] public bool Field_13 { get; set; } // bool
    [FlatBufferItem(14)] public PlacementV3f8a Field_14 { get; set; } = new();
    [FlatBufferItem(15)] public bool Field_15 { get; set; } // bool
    [FlatBufferItem(16)] public PlacementV3f8a Field_16 { get; set; } = new();
    [FlatBufferItem(17)] public bool Field_17 { get; set; } // bool
    [FlatBufferItem(18)] public PlacementV3f8a Field_18 { get; set; } = new();
    [FlatBufferItem(19)] public bool Field_19 { get; set; } // bool
    [FlatBufferItem(20)] public PlacementV3f8a Field_20 { get; set; } = new();
    [FlatBufferItem(21)] public bool Field_21 { get; set; } // bool
    [FlatBufferItem(22)] public PlacementV3f8a Field_22 { get; set; } = new();
    [FlatBufferItem(23)] public bool Field_23 { get; set; } // bool
    [FlatBufferItem(24)] public PlacementV3f8a Field_24 { get; set; } = new();
    [FlatBufferItem(25)] public bool Field_25 { get; set; } // bool
    [FlatBufferItem(26)] public PlacementV3f8a Field_26 { get; set; } = new();

    public PlacementParameters8a Parameters
    {
        get { if (Field_02.Length != 1) throw new ArgumentException($"Invalid {nameof(Field_02)}"); return Field_02[0]; }
        set { if (Field_02.Length != 1) throw new ArgumentException($"Invalid {nameof(Field_02)}"); Field_02[0] = value; }
    }

    public override string ToString() => $"SearchItem(\"{Identifier}\", 0x{Hash_01:X16}, {Parameters}, {Rate}, {Field_03}, {Field_04})";

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
