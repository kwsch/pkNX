using System;
using System.Collections.Generic;
using System.ComponentModel;
using pkNX.Containers;

// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global

namespace pkNX.Structures.FlatBuffers.Arceus;

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class LandmarkItemSpawnTable { }

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class LandmarkItemSpawn
{
    public bool UsesTable(ulong table) => EncounterTableID == table;

    public string NameSummary
    {
        get
        {
            var map = GetNameMap();
            if (map.TryGetValue(LandmarkItemSpawnTableID, out var name))
                return $"\"{name}\"";

            return $"0x{LandmarkItemSpawnTableID:X16}";
        }
    }

    private static IReadOnlyDictionary<ulong, string>? _spawnerNameMap;
    private static IReadOnlyDictionary<ulong, string> GetNameMap() => _spawnerNameMap ??= GenerateSpawnerNameMap();

    private static IReadOnlyDictionary<ulong, string> GenerateSpawnerNameMap()
    {
        var result = new Dictionary<ulong, string>
        {
            [FnvHash.HashFnv1a_64("hoge")] = "hoge",
        };

        var gimmicks = new[] { "no", "tree", "rock", "crystal", "snow", "box", "leaves_r", "leaves_g", "yachi" };
        foreach (var gimmick in gimmicks)
        {
            result[FnvHash.HashFnv1a_64($"{gimmick}")] = $"{gimmick}";
            for (var which = 0; which < 20; which++)
            {
                result[FnvHash.HashFnv1a_64($"{gimmick}{which:00}")] = $"{gimmick}{which:00}";
                result[FnvHash.HashFnv1a_64($"{gimmick}_{which:00}")] = $"{gimmick}_{which:00}";
                for (var i = 0; i < 100; i++)
                {
                    result[FnvHash.HashFnv1a_64($"{gimmick}_{which:00}_{i:00}")] = $"{gimmick}_{which:00}_{i:00}";
                    result[FnvHash.HashFnv1a_64($"{gimmick}_{which:00}_ex{i:00}")] = $"{gimmick}_{which:00}_ex{i:00}";
                    for (var j = 0; j < 3; j++)
                    {
                        result[FnvHash.HashFnv1a_64($"{gimmick}_{which:00}_{i:00}_{j:00}")] = $"{gimmick}_{which:00}_{i:00}_{j:00}";
                        result[FnvHash.HashFnv1a_64($"{gimmick}_{which:00}_ex{i:00}_{j:00}")] = $"{gimmick}_{which:00}_ex{i:00}_{j:00}";
                        for (var k = 0; k < 3; k++)
                        {
                            result[FnvHash.HashFnv1a_64($"{gimmick}_{which:00}_{i:00}{j:00}_{k:00}")] = $"{gimmick}_{which:00}_{i:00}{j:00}_{k:00}";
                        }
                    }
                }
            }

            for (var w1 = 0; w1 < 5; w1++)
            {
                for (var w2 = 0; w2 < 5; w2++)
                {
                    result[FnvHash.HashFnv1a_64($"{gimmick}_{w1:00}_{w2:00}")] = $"{gimmick}_{w1:00}_{w2:00}";
                    for (var i = 0; i < 100; i++)
                    {
                        result[FnvHash.HashFnv1a_64($"{gimmick}_{w1:00}_{w2:00}_{i:00}")] = $"{gimmick}_{w1:00}_{w2:00}_{i:00}";
                        for (var j = 0; j < 3; j++)
                        {
                            result[FnvHash.HashFnv1a_64($"{gimmick}_{w1:00}_{w2:00}_{i:00}_{j:00}")] = $"{gimmick}_{w1:00}_{w2:00}_{i:00}_{j:00}";
                            for (var k = 0; k < 3; k++)
                            {
                                result[FnvHash.HashFnv1a_64($"{gimmick}_{w1:00}_{w2:00}_{i:00}{j:00}_{k:00}")] = $"{gimmick}_{w1:00}_{w2:00}_{i:00}{j:00}_{k:00}";
                            }
                        }
                    }
                }
            }
        }

        return result;
    }
}

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class LandmarkItemSpawn_F03 { }
