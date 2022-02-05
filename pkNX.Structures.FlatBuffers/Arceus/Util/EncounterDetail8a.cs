using System.Collections.Generic;
using System.Linq;

namespace pkNX.Structures.FlatBuffers;

public record struct EncounterDetail8a(double Rate, double MultT, double MultW, int Unk, EncounterSlot8a Slot)
{
    private const int WeatherCount = (int)Weather8a.Count;
    private const int TimeCount = (int)Time8a.Count;

    public static List<EncounterDetail8a>[,] GetEmpty()
    {
        var result = new List<EncounterDetail8a>[TimeCount, WeatherCount];
        for (var t = 0; t < TimeCount; t++)
        {
            for (var w = 0; w < WeatherCount; w++)
                result[t, w] = new List<EncounterDetail8a>();
        }
        return result;
    }

    public static void Divide(List<EncounterDetail8a>[,] table)
    {
        for (var time = 0; time < TimeCount; time++)
        {
            for (var weather = 0; weather < WeatherCount; weather++)
            {
                table[time, weather] = table[time, weather].OrderByDescending(xt => xt.Rate).ToList();

                var totalRate = 0.0;
                foreach (var tup in table[time, weather])
                    totalRate += tup.Rate;

                for (var i = 0; i < table[time, weather].Count; i++)
                {
                    var e = table[time, weather][i];
                    var effective = (e.Rate / totalRate) * 100.0;
                    var detail = new EncounterDetail8a(effective, e.MultT, e.MultW, e.Unk, e.Slot);
                    table[time, weather][i] = detail;
                }
            }
        }
    }
    public static TableSymmetryResult AnalyzeSymmetry(List<EncounterDetail8a>[,] table)
    {
        var isWeatherSymmetric = true;
        var isTimeSymmetric = true;
        var weatherSymmetricPerTime = new[] { true, true, true, true };
        for (var time = 0; time < 4; time++)
        {
            for (var weather = 0; weather < (int)Weather8a.Count; weather++)
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

    private static bool IsSameEffectiveTable(IReadOnlyList<EncounterDetail8a> lhs, IReadOnlyList<EncounterDetail8a> rhs)
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