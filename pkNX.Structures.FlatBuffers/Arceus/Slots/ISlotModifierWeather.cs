using System;

namespace pkNX.Structures.FlatBuffers;

public interface ISlotModifierWeather
{
    float WeatherMultiplier_1 { get; set; }
    float WeatherMultiplier_2 { get; set; }
    float WeatherMultiplier_3 { get; set; }
    float WeatherMultiplier_4 { get; set; }
    float WeatherMultiplier_5 { get; set; }
    float WeatherMultiplier_6 { get; set; }
    float WeatherMultiplier_7 { get; set; }
    float WeatherMultiplier_8 { get; set; }
}

public static class SlotModifierWeatherExtensions
{
    public static float GetWeatherMultiplier(this ISlotModifierWeather x, int index) => index switch
    {
        0 => 1.0f, // "No Weather" results in no modification of rate for all species/forms
        1 => x.WeatherMultiplier_1,
        2 => x.WeatherMultiplier_2,
        3 => x.WeatherMultiplier_3,
        4 => x.WeatherMultiplier_4,
        5 => x.WeatherMultiplier_5,
        6 => x.WeatherMultiplier_6,
        7 => x.WeatherMultiplier_7,
        8 => x.WeatherMultiplier_8,
        _ => throw new ArgumentOutOfRangeException(nameof(index)),
    };

    public static bool HasWeatherModifier(this ISlotModifierWeather w, int index) => index != 0 && w.GetWeatherMultiplier(index) != -1.0f;
}