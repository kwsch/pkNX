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
public class PlacementSpawner8a : ISlotTableConsumer
{
    [FlatBufferItem(0)] public ulong SpawnerID { get; set; }
    [FlatBufferItem(1)] public ulong Field_01 { get; set; }
    [FlatBufferItem(2)] public PlacementParameters8a[] Field_02 { get; set; } = { new() };
    [FlatBufferItem(3)] public string Shape { get; set; } = string.Empty; // only set by Wormhole spawners
    [FlatBufferItem(4)] public float Scalar { get; set; } // how big of a radius they can spawn in?
    [FlatBufferItem(5)] public PlacementV3f8a Field_05 { get; set; } = new(); // always default values
    [FlatBufferItem(6)] public PlacementV3f8a Field_06 { get; set; } = new(); // always default values
    [FlatBufferItem(7)] public int MinSpawnCount { get; set; }
    [FlatBufferItem(8)] public int MaxSpawnCount { get; set; }
    [FlatBufferItem(9)] public int Field_09 { get; set; } // 2 for a single tangela spawner, and 8 for a single lickitung spawner
    [FlatBufferItem(10)] public bool IsMassOutbreak { get; set; }
    [FlatBufferItem(11)] public bool IsWater { get; set; }
    [FlatBufferItem(12)] public bool IsSky { get; set; }
    [FlatBufferItem(13)] public ulong GroupID { get; set; }
    [FlatBufferItem(14)] public float Field_14 { get; set; }
    [FlatBufferItem(15)] public float Field_15 { get; set; }
    [FlatBufferItem(16)] public int ParentLink { get; set; } // 1 for Alpha parents, 2 for Children -- a [2] uses Behavior2 to alter [1]'s actions (run over to the player) to defend.
    [FlatBufferItem(17)] public float Field_17 { get; set; } // always -1
    [FlatBufferItem(18)] public float Field_18 { get; set; } // always -1
    [FlatBufferItem(19)] public float Field_19 { get; set; } // always -1
    [FlatBufferItem(20)] public PlacementSpawnerF208a[] Field_20 { get; set; } = { new() };
    [FlatBufferItem(21)] public PlacementSpawnerF218a[] Field_21 { get; set; } = { new() };
    [FlatBufferItem(22)] public string[] Field_22 { get; set; } = { string.Empty };

    public PlacementParameters8a Parameters
    {
        get { if (Field_02.Length != 1) throw new ArgumentException($"Invalid {nameof(Field_02)}"); return Field_02[0]; }
        set { if (Field_02.Length != 1) throw new ArgumentException($"Invalid {nameof(Field_02)}"); Field_02[0] = value; }
    }

    public PlacementSpawnerF208a Field_20_Value
    {
        get { if (Field_20.Length != 1) throw new ArgumentException($"Invalid {nameof(Field_20)}"); return Field_20[0]; }
        set { if (Field_20.Length != 1) throw new ArgumentException($"Invalid {nameof(Field_20)}"); Field_20[0] = value; }
    }

    public PlacementSpawnerF218a Field_21_Value
    {
        get { if (Field_21.Length != 1) throw new ArgumentException($"Invalid {nameof(Field_21)}"); return Field_21[0]; }
        set { if (Field_21.Length != 1) throw new ArgumentException($"Invalid {nameof(Field_21)}"); Field_21[0] = value; }
    }

    public string Field_22_Value
    {
        get { if (Field_22.Length != 1) throw new ArgumentException($"Invalid {nameof(Field_22)}"); return Field_22[0]; }
        set { if (Field_22.Length != 1) throw new ArgumentException($"Invalid {nameof(Field_22)}"); Field_22[0] = value; }
    }

    public IEnumerable<PlacementLocation8a> GetIntersectingLocations(IReadOnlyList<PlacementLocation8a> locations, float bias)
    {
        var c = Parameters.Coordinates;
        return GetIntersectingLocations(locations, bias, c, Scalar + bias);
    }

    private static IEnumerable<PlacementLocation8a> GetIntersectingLocations(IReadOnlyList<PlacementLocation8a> locations, float bias, PlacementV3f8a c, float scalar)
    {
        var result = new List<PlacementLocation8a>();
        foreach (var loc in locations)
        {
            if (!loc.IsNamedPlace)
                continue;
            if (loc.IntersectsSphere(c.X, c.Y, c.Z, scalar))
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

    private static IReadOnlyDictionary<ulong, string>? _spawnerNameMap;
    private static IReadOnlyDictionary<ulong, string> GetSpawnerNameMap() => _spawnerNameMap ??= GenerateSpawnerNameMap();

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
            result[FnvHash.HashFnv1a_64($"ex_{i:0000}")] = $"ex_{i:0000}";
            result[FnvHash.HashFnv1a_64($"eve_ex_{i:0000}")] = $"eve_ex_{i:0000}";

            result[FnvHash.HashFnv1a_64($"huge_{i:0000}")] = $"huge_{i:0000}";

            for (var j = 0; j < 20; j++)
            {
                result[FnvHash.HashFnv1a_64($"{i:0000}_{j:00}")] = $"{i:0000}_{j:00}";
            }
        }

        for (var i = 0; i < 100; i++)
        {
            result[FnvHash.HashFnv1a_64($"area00_{i:00}")] = $"area00_{i:00}";
            result[FnvHash.HashFnv1a_64($"poke{i:00}")] = $"poke{i:00}";
            result[FnvHash.HashFnv1a_64($"sky{i:00}")] = $"sky{i:00}";
            result[FnvHash.HashFnv1a_64($"lnd_no_{i:00}")] = $"lnd_no_{i:00}";
            result[FnvHash.HashFnv1a_64($"sky_{i:00}")] = $"sky_{i:00}";
            result[FnvHash.HashFnv1a_64($"ex_mkrg_{i:00}")] = $"ex_mkrg_{i:00}";
            result[FnvHash.HashFnv1a_64($"ex_unnn_{i:00}")] = $"ex_unnn_{i:00}";
            result[FnvHash.HashFnv1a_64($"ex_trs_{i:00}")] = $"ex_trs_{i:00}";
        }

        result[FnvHash.HashFnv1a_64($"ha_area01_s01_ev001")] = $"ha_area01_s01_ev001";
        result[FnvHash.HashFnv1a_64($"ha_area02_s02_ev001")] = $"ha_area02_s02_ev001";
        result[FnvHash.HashFnv1a_64($"ha_area02_s02_ev002")] = $"ha_area02_s02_ev002";
        result[FnvHash.HashFnv1a_64($"ha_area03_s03_ev001")] = $"ha_area03_s03_ev001";
        result[FnvHash.HashFnv1a_64($"ha_area04_ev001")] = $"ha_area04_ev001";
        result[FnvHash.HashFnv1a_64($"ha_area05_s03_ev001")] = $"ha_area05_s03_ev001";

        result[FnvHash.HashFnv1a_64($"area03_s04_ev001")] = $"area03_s04_ev001";
        result[FnvHash.HashFnv1a_64($"area03_s04_ev002")] = $"area03_s04_ev002";
        result[FnvHash.HashFnv1a_64($"area03_s04_ev003")] = $"area03_s04_ev003";
        result[FnvHash.HashFnv1a_64($"area03_s04_ev004")] = $"area03_s04_ev004";
        result[FnvHash.HashFnv1a_64($"area03_s04_ev005")] = $"area03_s04_ev005";

        // 1.0.2
        result[FnvHash.HashFnv1a_64($"ha_area01_s01_1000")] = $"ha_area01_s01_1000";
        result[FnvHash.HashFnv1a_64($"ha_area02_s02_1000")] = $"ha_area02_s02_1000";
        result[FnvHash.HashFnv1a_64($"ha_area05_s03_1000")] = $"ha_area05_s03_1000";


        for (var wh = 1; wh < 8; wh++)
        {
            for (var sub = 1; sub < 4; sub++)
            {
                for (var n = 1; n < 7; n++)
                {
                    result[FnvHash.HashFnv1a_64($"main_whSpawner{wh:00}{sub:00}_{n:00}")] = $"main_whSpawner{wh:00}{sub:00}_{n:00}";
                    result[FnvHash.HashFnv1a_64($"sub_whSpawner{wh:00}{sub:00}_{n:00}")] = $"sub_whSpawner{wh:00}{sub:00}_{n:00}";
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
    private static IReadOnlyDictionary<ulong, string>? _groupNameMap;
    private static IReadOnlyDictionary<ulong, string> GetGroupNameMap() => _groupNameMap ??= GenerateGroupNameMap();

    private static IReadOnlyDictionary<ulong, string> GenerateGroupNameMap()
    {
        var result = new Dictionary<ulong, string>();
        for (var i = 0; i < 100; i++)
        {
            var reg = $"grp{i:00}";
            var und = $"grp_{i:00}";
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

    public override string ToString() => $"Spawner({NameSummary}, 0x{Field_01:X16}, {Parameters}, \"{Shape}\", {Scalar}, {Field_05}, {Field_06}, {MinSpawnCount}, {MaxSpawnCount}, {Field_09}, {IsMassOutbreak}, {IsWater}, {IsSky}, /* Group = */ {GroupSummary}, {Field_14}, {Field_15}, {ParentLink}, {Field_17}, {Field_18}, {Field_19}, {Field_20_Value}, {Field_21_Value}, {Field_22_Value})";

    public bool UsesTable(ulong table) => Field_20_Value.EncounterTableID == table;
}
