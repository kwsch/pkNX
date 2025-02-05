namespace pkNX.Structures.FlatBuffers.Arceus;

public partial class LandmarkItem : ISlotTableConsumer
{
    public PlacementParameters Parameters
    {
        get { if (Field03.Count != 1) throw new ArgumentException($"Invalid {nameof(Field03)}"); return Field03[0]; }
        set { if (Field03.Count != 1) throw new ArgumentException($"Invalid {nameof(Field03)}"); Field03[0] = value; }
    }

    public bool UsesTable(ulong table) => LandmarkItemSpawnTableID == table;

    public IEnumerable<PlacementLocation> GetIntersectingLocations(IList<PlacementLocation> locations, float bias)
    {
        var c = Parameters.Coordinates;
        return GetIntersectingLocations(locations, bias, c, Scalar);
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

    public bool IsTreeWithoutSpawns
    {
        get
        {
            var map = GetNameMap();
            return map.TryGetValue(LandmarkItemNameID, out var name) && name.StartsWith("gimmick_tree10");
        }
    }

    public string NameSummary
    {
        get
        {
            var map = GetNameMap();
            if (map.TryGetValue(LandmarkItemNameID, out var name))
                return $"\"{name}\" ({Field00})";

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
