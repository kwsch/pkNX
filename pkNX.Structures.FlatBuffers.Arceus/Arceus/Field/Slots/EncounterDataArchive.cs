using System.ComponentModel;

// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global

namespace pkNX.Structures.FlatBuffers.Arceus;

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class EncounterDataArchive;

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class EncounterEligiblityTraits : IHasCondition, ISlotModifierTime, ISlotModifierWeather;

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class EncounterOybnTraits
{
    public bool IsOybnAny => Oybn1 || Oybn2 || Field02 || Field03;
}

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class EncounterTable
{
    // lazy init
    private static Dictionary<ulong, string>? _tableNameMap;
    private static IReadOnlyDictionary<ulong, string> GetTableNameMap() => _tableNameMap ??= GenerateTableMap();

    private static Dictionary<ulong, string> GenerateTableMap()
    {
        var result = new Dictionary<ulong, string>
        {
            [0xCBF29CE484222645] = "",
        };

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

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class EncounterSlot
{
    public override string ToString() => $"{(Species)Species}{(Form == 0 ? "" : $"-{Form}")}{(Oybn.IsOybnAny ? "*" : "")} - {BaseProbability}={ShinyLock}";

    public (bool forced, int min, int max) GetLevels(int min, int max)
    {
        bool force = false;
        if (OverrideMinLevel is > 0 and <= 100)
        {
            force = true;
            min = OverrideMinLevel;
        }
        if (OverrideMaxLevel is > 0 and <= 100)
        {
            force = true;
            max = OverrideMaxLevel;
        }
        return (force, min, max);
    }

    public float GetTimeModifier(int time, EncounterMultiplier default_mults)
    {
        // If the slot has defined an override factor, use it; otherwise, fall back on the default multiplier for (species, form).
        if (Eligibility.HasTimeModifier(time))
            return Eligibility.GetTimeMultiplier(time);
        if (default_mults.HasTimeModifier(time))
            return default_mults.GetTimeMultiplier(time);
        return 1.0f;
    }

    public float GetWeatherModifier(int weather, EncounterMultiplier default_mults)
    {
        // If the slot has defined an override factor, use it; otherwise, fall back on the default multiplier for (species, form).
        if (Eligibility.HasWeatherModifier(weather))
            return Eligibility.GetWeatherMultiplier(weather);
        if (default_mults.HasWeatherModifier(weather))
            return default_mults.GetWeatherMultiplier(weather);
        return 1.0f;
    }

    // lazy init
    private static Dictionary<ulong, string>? _slotNameMap;
    private static IReadOnlyDictionary<ulong, string> GetSlotNameMap() => _slotNameMap ??= GenerateTableMap();

    private static Dictionary<ulong, string> GenerateTableMap()
    {
        var result = new Dictionary<ulong, string>();
        for (var i = 0; i < 1000; i++)
        {
            var life = $"life{i:000}";
            var pm = $"pm{i:0000}";
            result[FnvHash.HashFnv1a_64(life)] = life;
            result[FnvHash.HashFnv1a_64(pm)] = pm;
        }
        for (var i = 0; i < 100; i++)
        {
            var poke = $"poke{i:00}";
            result[FnvHash.HashFnv1a_64(poke)] = poke;
        }
        return result;
    }

    public static string GetSlotName(ulong slotId)
    {
        var map = GetSlotNameMap();
        if (map.TryGetValue(slotId, out var name))
            return $"\"{name}\"";
        return $"0x{slotId:X16}";
    }

    public string SlotName => GetSlotName(SlotID);

    public void ClearMoves()
    {
        Move1 = Move2 = Move3 = Move4 = 0;
        Move1Mastered = Move2Mastered = Move3Mastered = Move4Mastered = false;
    }
}
