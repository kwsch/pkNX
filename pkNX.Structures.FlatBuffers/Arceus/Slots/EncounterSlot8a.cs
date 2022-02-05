using System.Collections.Generic;
using System.ComponentModel;
using FlatSharp.Attributes;
using pkNX.Containers;

namespace pkNX.Structures.FlatBuffers;

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class EncounterSlot8a
{
    [FlatBufferItem(00)] public int Species { get; set; }
    [FlatBufferItem(01)] public ulong SlotID { get; set; }
    [FlatBufferItem(02)] public int Gender { get; set; }
    [FlatBufferItem(03)] public int Form { get; set; }
    [FlatBufferItem(04)] public ShinyType8a ShinyLock { get; set; }
    [FlatBufferItem(05)] public AbilityType8a AbilityRandType { get; set; }
    [FlatBufferItem(06)] public NatureType8a Nature { get; set; }
    [FlatBufferItem(07)] public int Height { get; set; }
    [FlatBufferItem(08)] public int Weight { get; set; }
    [FlatBufferItem(09)] public bool Field_09 { get; set; }
    [FlatBufferItem(10)] public bool Field_10 { get; set; }
    [FlatBufferItem(11)] public bool Field_11 { get; set; }
    [FlatBufferItem(12)] public bool Field_12 { get; set; }
    [FlatBufferItem(13)] public int GV_HP { get; set; }
    [FlatBufferItem(14)] public int GV_ATK { get; set; }
    [FlatBufferItem(15)] public int GV_DEF { get; set; }
    [FlatBufferItem(16)] public int GV_SPA { get; set; }
    [FlatBufferItem(17)] public int GV_SPD { get; set; }
    [FlatBufferItem(18)] public int GV_SPE { get; set; }
    [FlatBufferItem(19)] public int IV_HP { get; set; }
    [FlatBufferItem(20)] public int IV_ATK { get; set; }
    [FlatBufferItem(21)] public int IV_DEF { get; set; }
    [FlatBufferItem(22)] public int IV_SPA { get; set; }
    [FlatBufferItem(23)] public int IV_SPD { get; set; }
    [FlatBufferItem(24)] public int IV_SPE { get; set; }
    [FlatBufferItem(25)] public int NumPerfectIvs { get; set; }
    [FlatBufferItem(26)] public string Behavior1 { get; set; } = string.Empty;
    [FlatBufferItem(27)] public string Behavior2 { get; set; } = string.Empty;
    [FlatBufferItem(28)] public byte Field_28_AffectsLottery { get; set; }
    [FlatBufferItem(29)] public int BaseProbability { get; set; }
    [FlatBufferItem(30)] public int OverrideMinLevel { get; set; }
    [FlatBufferItem(31)] public int OverrideMaxLevel { get; set; }
    [FlatBufferItem(32)] public int Field_32 { get; set; }
    [FlatBufferItem(33)] public bool HasMoveset { get; set; }
    [FlatBufferItem(34)] public int Move1 { get; set; }
    [FlatBufferItem(35)] public int Move2 { get; set; }
    [FlatBufferItem(36)] public int Move3 { get; set; }
    [FlatBufferItem(37)] public int Move4 { get; set; }
    [FlatBufferItem(38)] public bool Move1Mastered { get; set; }
    [FlatBufferItem(39)] public bool Move2Mastered { get; set; }
    [FlatBufferItem(40)] public bool Move3Mastered { get; set; }
    [FlatBufferItem(41)] public bool Move4Mastered { get; set; }
    [FlatBufferItem(42)] public string Unused { get; set; } = string.Empty;
    [FlatBufferItem(43)] public int Field_43_Func_1A25908 { get; set; }
    [FlatBufferItem(44)] public bool Field_44_SetsPropTo100Not8000 { get; set; }
    [FlatBufferItem(45)] public EncounterEligiblityTraits8a Eligibility { get; set; } = new();
    [FlatBufferItem(46)] public EncounterOybnTraits8a Oybn { get; set; } = new ();

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

    public float GetTimeModifier(int time, EncounterMultiplier8a default_mults)
    {
        if (Eligibility.HasTimeModifier(time))
            return Eligibility.GetTimeMultiplier(time);
        if (default_mults.HasTimeModifier(time))
            return default_mults.GetTimeMultiplier(time);
        return 1.0f;
    }
    public float GetWeatherModifier(int weather, EncounterMultiplier8a default_mults)
    {
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
}
