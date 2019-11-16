using System;
using System.Collections.Generic;
using System.Linq;

namespace pkNX.Structures
{
    public static class EncounterTable8Util
    {
        public static IEnumerable<string> GetLines(EncounterArchive8 t, IReadOnlyDictionary<ulong, string> zone_names, string[] subtable_names, string[] species)
        {
            for (var i = 0; i < t.EncounterTables.Length; i++)
            {
                var enc = t.EncounterTables[i];
                yield return $"{i:000} - {zone_names[enc.ZoneID]}:";

                if (enc.SubTables.Length != 0)
                {
                    var j = 0;
                    const int NUM_WEATHER_TABLES = 9;
                    if (AllWeatherTablesIdentical(enc.SubTables, NUM_WEATHER_TABLES))
                    {
                        foreach (var line in GetSubTableSummary(enc.SubTables[0], "All Weather", species))
                            yield return $"\t{line}";
                        j = NUM_WEATHER_TABLES;
                    }

                    while (j < enc.SubTables.Length)
                    {
                        foreach (var line in GetSubTableSummary(enc.SubTables[j], subtable_names[j], species))
                            yield return $"\t{line}";
                        j++;
                    }
                }

                yield return string.Empty;
            }
        }

        private static IEnumerable<string> GetSubTableSummary(EncounterSubTable8 subtable, string name, string[] species)
        {
            if (subtable.LevelMin == 0 || subtable.LevelMax == 0) yield break;

            yield return $"{name} ({GetSubSummary(subtable.LevelMin, subtable.LevelMax)}):";
            foreach (var line in GetLines(subtable.Slots, species))
                yield return $"\t{line}";
        }

        private static bool AllWeatherTablesIdentical(EncounterSubTable8[] subtables, int numWeatherTables)
        {
            if (subtables.Length < numWeatherTables) throw new ArgumentException();
            var first_table = subtables[0];
            for (var i = 1; i < numWeatherTables; i++)
            {
                var cur_table = subtables[i];
                if (cur_table.LevelMin != first_table.LevelMin) return false;
                if (cur_table.LevelMax != first_table.LevelMax) return false;
                if (cur_table.Slots.Length != first_table.Slots.Length) return false;
                for (var j = 0; j < cur_table.Slots.Length; j++)
                {
                    if (cur_table.Slots[j].Species != first_table.Slots[j].Species) return false;
                    if (cur_table.Slots[j].Form != first_table.Slots[j].Form) return false;
                    if (cur_table.Slots[j].Probability != first_table.Slots[j].Probability) return false;
                }
            }

            return true;
        }

        private static string GetSubSummary(int min, int max) => $"Lv. {min}-{max}";

        private static IEnumerable<string> GetLines(IReadOnlyList<EncounterSlot8> arr, IReadOnlyList<string> species)
        {
            foreach (var slot in arr.OrderByDescending(sl => sl.Probability))
            {
                if (slot.Species == 0)
                    continue;
                string form = slot.Form == 0 ? string.Empty : $"-{slot.Form}";
                var spec_form = $"{species[slot.Species]}{form}";
                yield return $"- {spec_form,-12}\t{slot.Probability:00}%";
            }

            yield return string.Empty;
        }
    }
}
