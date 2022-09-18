﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using FlatSharp.Attributes;
using pkNX.Containers;

namespace pkNX.Structures.FlatBuffers;

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class EncounterSlot8a : ICloneable
{
    public override string ToString() => $"{(Species)Species}{(Form == 0 ? "" : $"-{Form}")}{(Oybn.IsOybnAny ? "*" : "")} - {BaseProbability}={ShinyLock}";

    [FlatBufferItem(00)] public int Species { get; set; }
    [FlatBufferItem(01)] public ulong SlotID { get; set; }
    [FlatBufferItem(02)] public int Gender { get; set; } = -1;
    [FlatBufferItem(03)] public int Form { get; set; }
    [FlatBufferItem(04)] public ShinyType8a ShinyLock { get; set; }
    [FlatBufferItem(05)] public AbilityType8a AbilityRandType { get; set; }
    [FlatBufferItem(06)] public NatureType8a Nature { get; set; } = NatureType8a.Random;
    [FlatBufferItem(07)] public int Height { get; set; } = -1;
    [FlatBufferItem(08)] public int Weight { get; set; } = -1;
    [FlatBufferItem(09)] public bool Field_09 { get; set; }
    [FlatBufferItem(10)] public bool Field_10 { get; set; }
    [FlatBufferItem(11)] public bool Field_11 { get; set; }
    [FlatBufferItem(12)] public bool Field_12 { get; set; }
    [FlatBufferItem(13)] public int GV_HP { get; set; } = -1;
    [FlatBufferItem(14)] public int GV_ATK { get; set; } = -1;
    [FlatBufferItem(15)] public int GV_DEF { get; set; } = -1;
    [FlatBufferItem(16)] public int GV_SPA { get; set; } = -1;
    [FlatBufferItem(17)] public int GV_SPD { get; set; } = -1;
    [FlatBufferItem(18)] public int GV_SPE { get; set; } = -1;
    [FlatBufferItem(19)] public int IV_HP { get; set; } = -1;
    [FlatBufferItem(20)] public int IV_ATK { get; set; } = -1;
    [FlatBufferItem(21)] public int IV_DEF { get; set; } = -1;
    [FlatBufferItem(22)] public int IV_SPA { get; set; } = -1;
    [FlatBufferItem(23)] public int IV_SPD { get; set; } = -1;
    [FlatBufferItem(24)] public int IV_SPE { get; set; } = -1;
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
    [FlatBufferItem(46)] public EncounterOybnTraits8a Oybn { get; set; } = new();

    public object Clone()
    {
        var slot = (EncounterSlot8a)MemberwiseClone();
        slot.Eligibility = (EncounterEligiblityTraits8a)Eligibility.Clone();
        slot.Oybn = (EncounterOybnTraits8a)Oybn.Clone();
        return slot;
    }

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
        // If the slot has defined an override factor, use it; otherwise, fall back on the default multiplier for (species, form).
        if (Eligibility.HasTimeModifier(time))
            return Eligibility.GetTimeMultiplier(time);
        if (default_mults.HasTimeModifier(time))
            return default_mults.GetTimeMultiplier(time);
        return 1.0f;
    }

    public float GetWeatherModifier(int weather, EncounterMultiplier8a default_mults)
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
