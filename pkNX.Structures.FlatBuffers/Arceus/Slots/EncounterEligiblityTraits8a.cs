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
public class EncounterEligiblityTraits8a : IHasCondition8a, ISlotModifierTime, ISlotModifierWeather
{
    [FlatBufferItem(0)] public ConditionType8a ConditionTypeID { get; set; } = ConditionType8a.None;
    [FlatBufferItem(1)] public Condition8a ConditionID { get; set; } = Condition8a.None;
    [FlatBufferItem(2)] public string ConditionArg1 { get; set; } = string.Empty;
    [FlatBufferItem(3)] public string ConditionArg2 { get; set; } = string.Empty;
    [FlatBufferItem(4)] public string ConditionArg3 { get; set; } = string.Empty;
    [FlatBufferItem(5)] public string ConditionArg4 { get; set; } = string.Empty;
    [FlatBufferItem(6)] public string ConditionArg5 { get; set; } = string.Empty;
    [FlatBufferItem(07)] public float TimeOfDayMultiplier_0 { get; set; } = -1;
    [FlatBufferItem(08)] public float TimeOfDayMultiplier_1 { get; set; } = -1;
    [FlatBufferItem(09)] public float TimeOfDayMultiplier_2 { get; set; } = -1;
    [FlatBufferItem(10)] public float TimeOfDayMultiplier_3 { get; set; } = -1;
    [FlatBufferItem(11)] public float WeatherMultiplier_1 { get; set; } = -1;
    [FlatBufferItem(12)] public float WeatherMultiplier_2 { get; set; } = -1;
    [FlatBufferItem(13)] public float WeatherMultiplier_3 { get; set; } = -1;
    [FlatBufferItem(14)] public float WeatherMultiplier_4 { get; set; } = -1;
    [FlatBufferItem(15)] public float WeatherMultiplier_5 { get; set; } = -1;
    [FlatBufferItem(16)] public float WeatherMultiplier_6 { get; set; } = -1;
    [FlatBufferItem(17)] public float WeatherMultiplier_7 { get; set; } = -1;
    [FlatBufferItem(18)] public float WeatherMultiplier_8 { get; set; } = -1;
}
