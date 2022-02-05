using System;
using System.ComponentModel;
using FlatSharp.Attributes;

// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global

namespace pkNX.Structures.FlatBuffers;

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class EncounterEligiblityTraits8a : IHasCondition8a
{
    [FlatBufferItem(0)] public ConditionType8a ConditionTypeID { get; set; }
    [FlatBufferItem(1)] public Condition8a ConditionID { get; set; }
    [FlatBufferItem(2)] public string ConditionArg1 { get; set; } = string.Empty;
    [FlatBufferItem(3)] public string ConditionArg2 { get; set; } = string.Empty;
    [FlatBufferItem(4)] public string ConditionArg3 { get; set; } = string.Empty;
    [FlatBufferItem(5)] public string ConditionArg4 { get; set; } = string.Empty;
    [FlatBufferItem(6)] public string ConditionArg5 { get; set; } = string.Empty;
    [FlatBufferItem(07)] public float TimeOfDayMultiplier_0 { get; set; }
    [FlatBufferItem(08)] public float TimeOfDayMultiplier_1 { get; set; }
    [FlatBufferItem(09)] public float TimeOfDayMultiplier_2 { get; set; }
    [FlatBufferItem(10)] public float TimeOfDayMultiplier_3 { get; set; }
    [FlatBufferItem(11)] public float WeatherMultiplier_1 { get; set; }
    [FlatBufferItem(12)] public float WeatherMultiplier_2 { get; set; }
    [FlatBufferItem(13)] public float WeatherMultiplier_3 { get; set; }
    [FlatBufferItem(14)] public float WeatherMultiplier_4 { get; set; }
    [FlatBufferItem(15)] public float WeatherMultiplier_5 { get; set; }
    [FlatBufferItem(16)] public float WeatherMultiplier_6 { get; set; }
    [FlatBufferItem(17)] public float WeatherMultiplier_7 { get; set; }
    [FlatBufferItem(18)] public float WeatherMultiplier_8 { get; set; }

    public float GetTimeMultiplier(int index) => index switch
    {
        0 => TimeOfDayMultiplier_0,
        1 => TimeOfDayMultiplier_1,
        2 => TimeOfDayMultiplier_2,
        3 => TimeOfDayMultiplier_3,
        _ => throw new ArgumentOutOfRangeException(nameof(index)),
    };

    public float GetWeatherMultiplier(int index) => index switch
    {
        0 => 1.0f, // "No Weather" results in no modification of rate for all species/forms
        1 => WeatherMultiplier_1,
        2 => WeatherMultiplier_2,
        3 => WeatherMultiplier_3,
        4 => WeatherMultiplier_4,
        5 => WeatherMultiplier_5,
        6 => WeatherMultiplier_6,
        7 => WeatherMultiplier_7,
        8 => WeatherMultiplier_8,
        _ => throw new ArgumentOutOfRangeException(nameof(index)),
    };

    public bool HasTimeModifier(int index) => GetTimeMultiplier(index) != -1.0f;
    public bool HasWeatherModifier(int index) => index != 0 && GetWeatherMultiplier(index) != -1.0f;
}
