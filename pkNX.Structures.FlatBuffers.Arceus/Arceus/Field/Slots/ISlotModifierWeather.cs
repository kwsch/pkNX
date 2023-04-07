using System;

namespace pkNX.Structures.FlatBuffers.Arceus;

public interface ISlotModifierWeather
{
    float WeatherMultiplier1 {get; set;}
    float WeatherMultiplier2 {get; set;}
    float WeatherMultiplier3 {get; set;}
    float WeatherMultiplier4 {get; set;}
    float WeatherMultiplier5 {get; set;}
    float WeatherMultiplier6 {get; set;}
    float WeatherMultiplier7 {get; set;}
    float WeatherMultiplier8 {get; set;}
}

public static class SlotModifierWeatherExtensions
{
    public static float GetWeatherMultiplier(this ISlotModifierWeather x, int index) => index switch
    {
        0 => 1.0f, // "No Weather" results in no modification of rate for all species/forms
        1 => x.WeatherMultiplier1,
        2 => x.WeatherMultiplier2,
        3 => x.WeatherMultiplier3,
        4 => x.WeatherMultiplier4,
        5 => x.WeatherMultiplier5,
        6 => x.WeatherMultiplier6,
        7 => x.WeatherMultiplier7,
        8 => x.WeatherMultiplier8,
        _ => throw new ArgumentOutOfRangeException(nameof(index)),
    };

    public static bool HasWeatherModifier(this ISlotModifierWeather w, int index) => index != 0 && w.GetWeatherMultiplier(index) != -1.0f;
}
