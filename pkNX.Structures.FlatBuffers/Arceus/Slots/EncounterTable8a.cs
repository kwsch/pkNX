using System;
using System.Collections.Generic;
using System.ComponentModel;
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
public class EncounterTable8a
{
    [FlatBufferItem(0)] public ulong TableID { get; set; }
    [FlatBufferItem(1)] public int MinLevel { get; set; }
    [FlatBufferItem(2)] public int MaxLevel { get; set; }
    [FlatBufferItem(3)] public EncounterSlot8a[] Table { get; set; } = Array.Empty<EncounterSlot8a>();

    // lazy init
    private static Dictionary<ulong, string>? _tableNameMap;
    private static IReadOnlyDictionary<ulong, string> GetTableNameMap() => _tableNameMap ??= GenerateTableMap();

    private static Dictionary<ulong, string> GenerateTableMap()
    {
        var result = new Dictionary<ulong, string>();

        result[0xCBF29CE484222645] = "";

        var prefixes = new[] { "eve", "fly", "gmk", "lnd", "mas", "oyb", "swm", "whl" };
        var kinds = new[] { "ex", "no", "ra" };
        foreach (var prefix in prefixes)
        {
            foreach (var kind in kinds)
            {
                for (var i = 0; i < 150; i++)
                {
                    var name = $"{prefix}_{kind}_{i:00}";
                    var hash = FnvHash.HashFnv1a_64(name);
                    result[hash] = name;
                }
            }
        }

        for (var area = 0; area < 6; area++)
        {
            for (var i = 0; i < 10; i++)
            {
                result[FnvHash.HashFnv1a_64($"sky_area{area}_{i:00}")] = $"sky_area{area}_{i:00}";
            }
        }

        result[FnvHash.HashFnv1a_64("eve_ex_16_b")] = "eve_ex_16_b";
        result[FnvHash.HashFnv1a_64("eve_ex_17_b")] = "eve_ex_17_b";
        result[FnvHash.HashFnv1a_64("eve_ex_18_b")] = "eve_ex_18_b";

        var gimmicks = new[] { "no", "tree", "rock", "crystal", "snow", "box" };
        foreach (var gimmick in gimmicks)
        {
            for (var i = 0; i < 100; i++)
            {
                result[FnvHash.HashFnv1a_64($"gmk_{gimmick}_{i:00}")] = $"gmk_{gimmick}_{i:00}";
                for (var j = 0; j < 3; j++)
                {
                    result[FnvHash.HashFnv1a_64($"gmk_{gimmick}_{i:00}_{j:00}")] = $"gmk_{gimmick}_{i:00}_{j:00}";
                    for (var k = 0; k < 3; k++)
                    {
                        result[FnvHash.HashFnv1a_64($"gmk_{gimmick}_{i:00}{j:00}_{k:00}")] = $"gmk_{gimmick}_{i:00}{j:00}_{k:00}";
                    }
                }
            }
        }

        return result;
    }

    public static string GetTableName(ulong tableId)
    {
        var map = GetTableNameMap();
        if (map.TryGetValue(tableId, out var name))
            return $"\"{name}\"";
        return $"0x{tableId:X16}";
    }

    public string TableName => GetTableName(TableID);

    public override string ToString() => $"{TableName} (Lv. {MinLevel}-{MaxLevel})";
}
