namespace pkNX.Structures.FlatBuffers.Arceus;

public partial class PlacementSpawner : ISlotTableConsumer
{
    public PlacementParameters Parameters
    {
        get { if (Field02.Count != 1) throw new ArgumentException($"Invalid {nameof(Field02)}"); return Field02[0]; }
        set { if (Field02.Count != 1) throw new ArgumentException($"Invalid {nameof(Field02)}"); Field02[0] = value; }
    }

    public PlacementSpawnerF20 Field20Value
    {
        get { if (Field20.Count != 1) throw new ArgumentException($"Invalid {nameof(Field20)}"); return Field20[0]; }
        set { if (Field20.Count != 1) throw new ArgumentException($"Invalid {nameof(Field20)}"); Field20[0] = value; }
    }

    public PlacementSpawnerF21 Field21Value
    {
        get { if (Field21.Count != 1) throw new ArgumentException($"Invalid {nameof(Field21)}"); return Field21[0]; }
        set { if (Field21.Count != 1) throw new ArgumentException($"Invalid {nameof(Field21)}"); Field21[0] = value; }
    }

    public string PathToFollowID
    {
        get { if (PathToFollowIDs.Count != 1) throw new ArgumentException($"Invalid {nameof(PathToFollowIDs)}"); return PathToFollowIDs[0]; }
        set { if (PathToFollowIDs.Count != 1) throw new ArgumentException($"Invalid {nameof(PathToFollowIDs)}"); PathToFollowIDs[0] = value; }
    }

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

    private static IReadOnlyDictionary<ulong, string>? spawnerNameMap;
    private static IReadOnlyDictionary<ulong, string> GetSpawnerNameMap() => spawnerNameMap ??= GenerateSpawnerNameMap();

    private static IReadOnlyDictionary<ulong, string> GenerateSpawnerNameMap()
    {
        var result = new Dictionary<ulong, string>();

        for (var i = 0; i < 10000; i++)
        {
            result[FnvHash.HashFnv1a_64($"{i:0000}")] = $"{i:0000}";
            result[FnvHash.HashFnv1a_64($"1{i:0000}")] = $"1{i:0000}";
            result[FnvHash.HashFnv1a_64($"02{i:0000}")] = $"02{i:0000}";
            result[FnvHash.HashFnv1a_64($"03{i:0000}")] = $"03{i:0000}";
            result[FnvHash.HashFnv1a_64($"4{i:0000}")] = $"4{i:0000}";
            result[FnvHash.HashFnv1a_64($"5{i:0000}")] = $"5{i:0000}";
            result[FnvHash.HashFnv1a_64($"ev{i:0000}")] = $"ev{i:0000}";
            result[FnvHash.HashFnv1a_64($"ex{i:0000}")] = $"ex{i:0000}";
            result[FnvHash.HashFnv1a_64($"eveex{i:0000}")] = $"eveex{i:0000}";

            result[FnvHash.HashFnv1a_64($"huge{i:0000}")] = $"huge{i:0000}";

            for (var j = 0; j < 20; j++)
            {
                result[FnvHash.HashFnv1a_64($"{i:0000}{j:00}")] = $"{i:0000}{j:00}";
            }
        }

        for (var i = 0; i < 100; i++)
        {
            result[FnvHash.HashFnv1a_64($"area00{i:00}")] = $"area00{i:00}";
            result[FnvHash.HashFnv1a_64($"poke{i:00}")] = $"poke{i:00}";
            result[FnvHash.HashFnv1a_64($"sky{i:00}")] = $"sky{i:00}";
            result[FnvHash.HashFnv1a_64($"lndno{i:00}")] = $"lndno{i:00}";
            result[FnvHash.HashFnv1a_64($"sky{i:00}")] = $"sky{i:00}";
            result[FnvHash.HashFnv1a_64($"exmkrg{i:00}")] = $"exmkrg{i:00}";
            result[FnvHash.HashFnv1a_64($"exunnn{i:00}")] = $"exunnn{i:00}";
            result[FnvHash.HashFnv1a_64($"extrs{i:00}")] = $"extrs{i:00}";
        }

        result[FnvHash.HashFnv1a_64("haarea01s01ev001")] = "haarea01s01ev001";
        result[FnvHash.HashFnv1a_64("haarea02s02ev001")] = "haarea02s02ev001";
        result[FnvHash.HashFnv1a_64("haarea02s02ev002")] = "haarea02s02ev002";
        result[FnvHash.HashFnv1a_64("haarea03s03ev001")] = "haarea03s03ev001";
        result[FnvHash.HashFnv1a_64("haarea04ev001")] = "haarea04ev001";
        result[FnvHash.HashFnv1a_64("haarea05s03ev001")] = "haarea05s03ev001";

        result[FnvHash.HashFnv1a_64("area03s04ev001")] = "area03s04ev001";
        result[FnvHash.HashFnv1a_64("area03s04ev002")] = "area03s04ev002";
        result[FnvHash.HashFnv1a_64("area03s04ev003")] = "area03s04ev003";
        result[FnvHash.HashFnv1a_64("area03s04ev004")] = "area03s04ev004";
        result[FnvHash.HashFnv1a_64("area03s04ev005")] = "area03s04ev005";

        // 1.0.2
        result[FnvHash.HashFnv1a_64("haarea01s011000")] = "haarea01s011000";
        result[FnvHash.HashFnv1a_64("haarea02s021000")] = "haarea02s021000";
        result[FnvHash.HashFnv1a_64("haarea05s031000")] = "haarea05s031000";

        for (var wh = 1; wh < 8; wh++)
        {
            for (var sub = 1; sub < 4; sub++)
            {
                for (var n = 1; n < 7; n++)
                {
                    result[FnvHash.HashFnv1a_64($"mainwhSpawner{wh:00}{sub:00}{n:00}")] = $"mainwhSpawner{wh:00}{sub:00}{n:00}";
                    result[FnvHash.HashFnv1a_64($"subwhSpawner{wh:00}{sub:00}{n:00}")] = $"subwhSpawner{wh:00}{sub:00}{n:00}";
                }
            }
        }

        return result;
    }

    public string NameSummary
    {
        get
        {
            var map = GetSpawnerNameMap();
            if (map.TryGetValue(SpawnerID, out var name))
                return $"\"{name}\"";

            return $"0x{SpawnerID:X16}";
        }
    }

    // lazy init
    private static IReadOnlyDictionary<ulong, string>? groupNameMap;
    private static IReadOnlyDictionary<ulong, string> GetGroupNameMap() => groupNameMap ??= GenerateGroupNameMap();

    private static IReadOnlyDictionary<ulong, string> GenerateGroupNameMap()
    {
        var result = new Dictionary<ulong, string>();
        for (var i = 0; i < 100; i++)
        {
            var reg = $"grp{i:00}";
            var und = $"grp{i:00}";
            result[FnvHash.HashFnv1a_64(und)] = und;
            result[FnvHash.HashFnv1a_64(reg)] = reg;
        }
        result[0xCBF29CE484222645] = "";
        return result;
    }

    public string GroupSummary
    {
        get
        {
            var map = GetGroupNameMap();
            if (map.TryGetValue(GroupID, out var name))
                return $"\"{name}\"";

            return $"0x{GroupID:X16}";
        }
    }

    public override string ToString() => $"Spawner({NameSummary}, 0x{Field01:X16}, {Parameters}, \"{Shape}\", {Scalar}, {Field05}, {Field06}, {MinSpawnCount}, {MaxSpawnCount}, {Field09}, {IsMassOutbreak}, {IsWater}, {IsSky}, /* Group = */ {GroupSummary}, {Field14}, {Field15}, {ParentLink}, {Field17}, {Field18}, {Field19}, {Field20Value}, {Field21Value}, {PathToFollowID})";

    public bool UsesTable(ulong table) => Field20Value.EncounterTableID == table;
}
