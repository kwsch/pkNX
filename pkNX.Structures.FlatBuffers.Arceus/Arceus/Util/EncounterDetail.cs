using System.Collections.Generic;
using System.Linq;

namespace pkNX.Structures.FlatBuffers.Arceus;

public record struct EncounterDetail(double Rate, double MultT, double MultW, int Unk, EncounterSlot Slot)
{
    private const int WeatherCount = (int)Weather.Count;
    private const int TimeCount = (int)Time.Count;

    public static List<EncounterDetail>[,] GetEmpty()
    {
        var result = new List<EncounterDetail>[TimeCount, WeatherCount];
        for (var t = 0; t < TimeCount; t++)
        {
            for (var w = 0; w < WeatherCount; w++)
                result[t, w] = [];
        }
        return result;
    }

    public static void Divide(List<EncounterDetail>[,] table)
    {
        for (var time = 0; time < TimeCount; time++)
        {
            for (var weather = 0; weather < WeatherCount; weather++)
            {
                table[time, weather] = [.. table[time, weather].OrderByDescending(xt => xt.Rate)];

                var totalRate = 0.0;
                foreach (var tup in table[time, weather])
                    totalRate += tup.Rate;

                for (var i = 0; i < table[time, weather].Count; i++)
                {
                    var e = table[time, weather][i];
                    var effective = (e.Rate / totalRate) * 100.0;
                    table[time, weather][i] = e with { Rate = effective };
                }
            }
        }
    }
    public static TableSymmetryResult AnalyzeSymmetry(List<EncounterDetail>[,] table)
    {
        var isWeatherSymmetric = true;
        var isTimeSymmetric = true;
        var weatherSymmetricPerTime = new[] { true, true, true, true };
        for (var time = 0; time < 4; time++)
        {
            for (var weather = 0; weather < (int)Weather.Count; weather++)
            {
                var ts = IsSameEffectiveTable(table[time, weather], table[0, weather]);
                var ws = IsSameEffectiveTable(table[time, weather], table[time, 0]);
                isTimeSymmetric &= ts;
                isWeatherSymmetric &= ws;
                weatherSymmetricPerTime[time] &= ws;
            }
        }

        return new TableSymmetryResult(isWeatherSymmetric, isTimeSymmetric, weatherSymmetricPerTime);
    }

    private static bool IsSameEffectiveTable(IReadOnlyList<EncounterDetail> lhs, IReadOnlyList<EncounterDetail> rhs)
    {
        if (lhs.Count != rhs.Count) return false;

        for (var i = 0; i < lhs.Count; i++)
        {
            if (lhs[i].Rate != rhs[i].Rate) return false;
            if (lhs[i].Unk != rhs[i].Unk) return false;
        }

        return true;
    }
}

public record struct TableSymmetryResult(bool Weather, bool Time, bool[] Complexed);
