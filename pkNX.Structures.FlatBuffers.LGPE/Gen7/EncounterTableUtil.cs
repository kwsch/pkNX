using System.Collections.Generic;

namespace pkNX.Structures.FlatBuffers.LGPE;

public static class EncounterTableUtil
{
    public static IEnumerable<string> GetLines(EncounterArchive t, string[] metLocationNames, string[] species)
    {
        for (var i = 0; i < t.Table.Count; i++)
        {
            var enc = t.Table[i];
            yield return $"{i:000} - {metLocationNames[i]}";

            if (enc.GroundTableEncounterRate != 0)
            {
                yield return nameof(enc.GroundTable);
                yield return GetSubSummary(enc.GroundTableLevelMin, enc.GroundTableLevelMax, enc.GroundTableEncounterRate, enc.GroundSpawnCountMax);
                foreach (var line in GetLines(enc.GroundTable, species))
                    yield return line;
            }
            if (enc.WaterTableEncounterRate != 0)
            {
                yield return nameof(enc.WaterTable);
                yield return GetSubSummary(enc.WaterTableLevelMin, enc.WaterTableLevelMax, enc.WaterTableEncounterRate, enc.WaterSpawnCountMax);
                foreach (var line in GetLines(enc.WaterTable, species))
                    yield return line;
            }
            if (enc.OldRodTableEncounterRate != 0)
            {
                yield return nameof(enc.OldRodTable);
                yield return GetSubSummary(enc.OldRodTableLevelMin, enc.OldRodTableLevelMax, enc.OldRodTableEncounterRate);
                foreach (var line in GetLines(enc.OldRodTable, species))
                    yield return line;
            }
            if (enc.GoodRodTableEncounterRate != 0)
            {
                yield return nameof(enc.GoodRodTable);
                yield return GetSubSummary(enc.GoodRodTableLevelMin, enc.GoodRodTableLevelMax, enc.GoodRodTableEncounterRate);
                foreach (var line in GetLines(enc.GoodRodTable, species))
                    yield return line;
            }
            if (enc.SuperRodTableEncounterRate != 0)
            {
                yield return nameof(enc.SuperRodTable);
                yield return GetSubSummary(enc.SuperRodTableLevelMin, enc.SuperRodTableLevelMax, enc.SuperRodTableEncounterRate);
                foreach (var line in GetLines(enc.SuperRodTable, species))
                    yield return line;
            }
            if (enc.SkyTableEncounterRate != 0)
            {
                yield return nameof(enc.SkyTable);
                yield return GetSubSummary(enc.SkyTableLevelMin, enc.SkyTableLevelMax, enc.SkyTableEncounterRate, enc.SkySpawnCountMax);
                foreach (var line in GetLines(enc.SkyTable, species))
                    yield return line;
            }
            yield return string.Empty;
        }
    }

    private static string GetSubSummary(int min, int max, int rate) => $"lv{min}-{max}, rate: {rate}";
    private static string GetSubSummary(int min, int max, int rate, int count) => GetSubSummary(min, max, rate) + $", max count: {count}";

    private static IEnumerable<string> GetLines(IList<EncounterSlot> arr, IReadOnlyList<string> species)
    {
        for (var i = 0; i < arr.Count; i++)
        {
            var slot = arr[i];
            if (slot.Species == 0)
                continue;
            string form = slot.Form == 0 ? string.Empty : $"-{slot.Form}";
            yield return $"{i:00}\t{species[slot.Species]}{form}\t{slot.Probability}";
        }
        yield return string.Empty;
    }
}
