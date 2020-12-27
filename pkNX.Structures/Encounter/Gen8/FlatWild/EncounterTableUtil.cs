using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace pkNX.Structures
{
    public static class EncounterTable8Util
    {
        public static byte[][] GetBytes(IReadOnlyDictionary<ulong, byte> zone_loc, EncounterArchive8 t)
        {
            var result = new List<byte[]>();
            foreach (var zone in t.EncounterTables)
            {
                var entry = GetZoneBytes(zone, zone_loc);
                if (entry.Length != 0)
                    result.Add(entry);
            }

            return result.ToArray();
        }

        private static byte[] GetZoneBytes(EncounterTable8 zone, IReadOnlyDictionary<ulong, byte> zoneLoc)
        {
            // Don't dump data that we can't correlate to a zone
            if (!zoneLoc.TryGetValue(zone.ZoneID, out var tmp))
                return Array.Empty<byte>();

            byte locID = tmp;
            var list = new List<Slot8>();
            for (int i = 0; i < zone.SubTables.Length; i++)
            {
                var weather = (SWSHEncounterType)(1 << i);
                var table = zone.SubTables[i];
                var min = table.LevelMin;
                var max = table.LevelMax;
                foreach (var s in table.Slots)
                {
                    var s8 = new Slot8(s.Species, s.Form, min, max) {EncounterType = weather};
                    var match = list.Find(z => z.Equals(s8));
                    if (match == null)
                        list.Add(s8);
                    else
                        match.EncounterType |= weather;
                }
            }

            return SerializeSlot8(locID, list);
        }

        private static byte[] SerializeSlot8(byte locID, IEnumerable<Slot8> list)
        {
            using var ms = new MemoryStream();
            using var bw = new BinaryWriter(ms);
            int ctr = 0;
            bw.Write(locID);
            bw.Write((byte) 0); // count tmp
            var groups = list.GroupBy(z => (int) z.EncounterType | (z.Min << 16) | (z.Max << 24));
            foreach (var g in groups)
            {
                var slots = g.ToArray();
                var type = g.Key & 0xFFFF;
                var min = (g.Key >> 16) & 0xFF;
                var max = g.Key >> 24;
                bw.Write((ushort) type);
                bw.Write((byte) min);
                bw.Write((byte) max);
                bw.Write((byte) slots.Length);
                bw.Write((byte) 0);

                foreach (var slot in slots)
                    bw.Write((ushort)(slot.Species | (slot.Form << 11)));
                ctr += slots.Length;
            }

            bw.BaseStream.Seek(1, SeekOrigin.Begin);
            bw.Write((byte) ctr);
            return ms.ToArray();
        }

        [Flags]
        private enum SWSHEncounterType
        {
            None,
            Normal = 1,
            Overcast = 1 << 1,
            Raining = 1 << 2,
            Thunderstorm = 1 << 3,
            Intense_Sun = 1 << 4,
            Snowing = 1 << 5,
            Snowstorm = 1 << 6,
            Sandstorm = 1 << 7,
            Heavy_Fog = 1 << 8,
            Shaking_Trees = 1 << 9,
            Fishing = 1 << 10,
        };

        private class Slot8 : IEquatable<Slot8>
        {
            public readonly int Species;
            public readonly int Form;
            public readonly int Min;
            public readonly int Max;
            public SWSHEncounterType EncounterType;

            public Slot8(int s, int f, int n, int x)
            {
                Species = s;
                Form = f;
                Min = n;
                Max = x;
            }

            public bool Equals(Slot8 other)
            {
                if (other is null) return false;
                if (ReferenceEquals(this, other)) return true;
                return Species == other.Species && Form == other.Form && Min == other.Min && Max == other.Max;
            }

            public override bool Equals(object obj)
            {
                if (obj is null) return false;
                if (ReferenceEquals(this, obj)) return true;
                return obj.GetType() == GetType() && Equals((Slot8) obj);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    var hashCode = Species;
                    hashCode = (hashCode * 397) ^ Form;
                    hashCode = (hashCode * 397) ^ Min;
                    hashCode = (hashCode * 397) ^ Max;
                    return hashCode;
                }
            }
        }

        public static IEnumerable<string> GetLines(EncounterArchive8 t, IReadOnlyDictionary<ulong, string> zone_names, string[] subtable_names, string[] species)
        {
            for (var i = 0; i < t.EncounterTables.Length; i++)
            {
                var enc = t.EncounterTables[i];
                bool known = zone_names.TryGetValue(enc.ZoneID, out var zoneName);
                if (!known)
                    zoneName = enc.ZoneID.ToString("X16");
                yield return $"{i:000} - {zoneName}:";

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
