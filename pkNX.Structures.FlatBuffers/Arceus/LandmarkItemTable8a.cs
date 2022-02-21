using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using FlatSharp.Attributes;
using pkNX.Containers;

// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global

namespace pkNX.Structures.FlatBuffers;

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class LandmarkItemTable8a : IFlatBufferArchive<LandmarkItem8a>
{
    public byte[] Write() => FlatBufferConverter.SerializeFrom(this);

    [FlatBufferItem(0)] public LandmarkItem8a[] Table { get; set; } = Array.Empty<LandmarkItem8a>();
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class LandmarkItem8a : ISlotTableConsumer
{
    [FlatBufferItem(00)] public string Field_00 { get; set; } = string.Empty;
    [FlatBufferItem(01)] public ulong Field_01 { get; set; }
    [FlatBufferItem(02)] public ulong LandmarkItemNameID { get; set; }
    [FlatBufferItem(03)] public PlacementParameters8a[] Field_03 { get; set; } = Array.Empty<PlacementParameters8a>();
    [FlatBufferItem(04)] public float Scalar { get; set; }
    [FlatBufferItem(05)] public FlatDummyObject Field_05 { get; set; } = new();
    [FlatBufferItem(06)] public FlatDummyObject Field_06 { get; set; } = new();
    [FlatBufferItem(07)] public FlatDummyObject Field_07 { get; set; } = new();
    [FlatBufferItem(08)] public ulong LandmarkItemSpawnTableID { get; set; }

    public PlacementParameters8a Parameters
    {
        get { if (Field_03.Length != 1) throw new ArgumentException($"Invalid {nameof(Field_03)}"); return Field_03[0]; }
        set { if (Field_03.Length != 1) throw new ArgumentException($"Invalid {nameof(Field_03)}"); Field_03[0] = value; }
    }

    public bool UsesTable(ulong table) => LandmarkItemSpawnTableID == table;

    public IEnumerable<PlacementLocation8a> GetIntersectingLocations(IReadOnlyList<PlacementLocation8a> locations, float bias)
    {
        var c = Parameters.Coordinates;
        return GetIntersectingLocations(locations, bias, c, Scalar);
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

    public string NameSummary
    {
        get
        {
            var map = GetNameMap();
            if (map.TryGetValue(LandmarkItemNameID, out var name))
                return $"\"{name}\" ({Field_00})";

            return $"0x{LandmarkItemNameID:X16}";
        }
    }

    private static IReadOnlyDictionary<ulong, string>? _landmarkItemNameMap;
    private static IReadOnlyDictionary<ulong, string> GetNameMap() => _landmarkItemNameMap ??= GenerateLandmarkItemNameMap();

    private static IReadOnlyDictionary<ulong, string> GenerateLandmarkItemNameMap()
    {
        var result = new Dictionary<ulong, string>();

        var gimmicks = new[] { "no", "tree", "rock", "crystal", "snow", "box", "leaves_r", "leaves_g", "yachi" };
        foreach (var gimmick in gimmicks)
        {
            result[FnvHash.HashFnv1a_64($"gimmick_{gimmick}")] = $"gimmick_{gimmick}";
            for (var which = 0; which < 20; which++)
            {
                result[FnvHash.HashFnv1a_64($"gimmick_{gimmick}{which:00}")] = $"gimmick_{gimmick}{which:00}";
                result[FnvHash.HashFnv1a_64($"gimmick_{gimmick}_{which:00}")] = $"gimmick_{gimmick}_{which:00}";
            }
        }

        for (var which = 0; which < 20; which++)
        {
            result[FnvHash.HashFnv1a_64($"gimmick_tree{which:00}_snow")] = $"gimmick_tree{which:00}_snow";
            foreach (var color in new[] { "blue", "pink", "yellow" })
            {
                result[FnvHash.HashFnv1a_64($"gimmick_tree{which:00}_{color}")] = $"gimmick_tree{which:00}_{color}";
                result[FnvHash.HashFnv1a_64($"gimmick_tree{which:00}_snow_{color}")] = $"gimmick_tree{which:00}_snow_{color}";
            }
        }

        return result;
    }
}
