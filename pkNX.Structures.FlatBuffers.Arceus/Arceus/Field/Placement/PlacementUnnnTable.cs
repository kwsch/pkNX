using System.ComponentModel;

// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global

namespace pkNX.Structures.FlatBuffers.Arceus;

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class PlacementUnnnTable;

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class PlacementUnnnEntry
{
    public PlacementParameters Parameters
    {
        get { if (Field03.Count != 1) throw new ArgumentException($"Invalid {nameof(Field03)}"); return Field03[0]; }
        set { if (Field03.Count != 1) throw new ArgumentException($"Invalid {nameof(Field03)}"); Field03[0] = value; }
    }

    public IEnumerable<PlacementLocation> GetIntersectingLocations(IList<PlacementLocation> locations, float bias)
    {
        var c = Parameters.Coordinates;
        return GetIntersectingLocations(locations, bias, c, 0);
    }

    private static IEnumerable<PlacementLocation> GetIntersectingLocations(IList<PlacementLocation> locations, float bias, Vec3f c, float scalar)
    {
        var result = new List<PlacementLocation>();
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

    public IEnumerable<PlacementLocation> GetContainingLocations(IList<PlacementLocation> locations)
    {
        var result = new List<PlacementLocation>();
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
