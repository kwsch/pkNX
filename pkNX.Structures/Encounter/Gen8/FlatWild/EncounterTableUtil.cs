using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace pkNX.Structures
{
    public static class EncounterTable8Util
    {
        public static byte[][] GetBytes(IReadOnlyDictionary<ulong, byte> zone_loc, IReadOnlyDictionary<ulong, byte> zone_type, EncounterArchive8 t, bool hiddenTreeFix = false)
        {
            var result = new List<DumpableLocation>();
            foreach (var zone in t.EncounterTables)
            {
                var entry = GetDumpable(zone, zone_loc, zone_type);
                if (entry.Slots.Count == 0)
                    continue;
                result.Add(entry);
            }

            if (hiddenTreeFix)
            {
                // The Berry Trees in Bridge Field are right against the map boundary, and can be accessed on the adjacent Map ID (Stony Wilderness)
                // Copy the two Berry Tree encounters from Bridge to Stony, as these aren't overworld (wandering) crossover encounters.
                var bridge = result.Find(z => z.Location == 142);
                var stony = result.Find(z => z.Location == 144);

                foreach (var s in bridge.Slots.Where(z => z.EncounterType == SWSHEncounterType.Shaking_Trees))
                    stony.Slots.Add(s);
            }

            return result.ConvertAll(z => z.Serialize()).ToArray();
        }

        private static DumpableLocation GetDumpable(EncounterTable8 zone, IReadOnlyDictionary<ulong, byte> zoneLoc, IReadOnlyDictionary<ulong, byte> zoneType)
        {
            // Don't dump data that we can't correlate to a zone
            if (!zoneLoc.TryGetValue(zone.ZoneID, out var tmp))
                return DumpableLocation.Empty;

            // Try to get the table type. Skip inaccessible tables.
            if (!zoneType.TryGetValue(zone.ZoneID, out var slottype) || slottype == (byte)SWSHSlotType.Inaccessible)
                return DumpableLocation.Empty;

            byte locID = tmp;
            var list = new List<Slot8>();
            for (int i = 0; i < zone.SubTables.Length; i++)
            {
                var weather = (SWSHEncounterType)(1 << i);

                if (!IsPermittedWeather(locID, weather, slottype))
                    continue;

                var table = zone.SubTables[i];
                var min = table.LevelMin;
                var max = table.LevelMax;
                foreach (var s in table.Slots)
                {
                    if (s.Species == 0)
                        continue;

                    var s8 = new Slot8(s.Species, s.Form, min, max) {EncounterType = weather};
                    var match = list.Find(z => z.Equals(s8));
                    if (match == null)
                        list.Add(s8);
                    else
                        match.EncounterType |= weather;
                }
            }

            return new DumpableLocation(list, locID, slottype);
        }

        private static bool IsPermittedWeather(byte locID, SWSHEncounterType weather, byte slotType)
        {
            // Only keep fishing slots for any encounters that are FishingOnly.
            if (slotType == (byte)SWSHSlotType.OnlyFishing)
                return weather == SWSHEncounterType.Fishing;

            // Otherwise, keep all fishing and shaking tree encounters.
            if (weather == SWSHEncounterType.Shaking_Trees || weather == SWSHEncounterType.Fishing)
                return true;

            // If we didn't find the weather in the general table, only allow Normal.
            if (!WeatherbyArea.TryGetValue(locID, out var permit))
                permit = SWSHEncounterType.Normal;
            if (permit.HasFlag(weather))
                return true;

            // Check bleed conditions first.
            if (slotType is (byte)SWSHSlotType.SymbolMain or (byte)SWSHSlotType.SymbolMain2 or (byte)SWSHSlotType.SymbolMain3)
            {
                if (WeatherBleedSymbol.TryGetValue(locID, out permit) && permit.HasFlag(weather))
                    return true;
            }
            if (slotType == (byte)SWSHSlotType.Surfing)
            {
                if (WeatherBleedSymbolSurfing.TryGetValue(locID, out permit) && permit.HasFlag(weather))
                    return true;
            }
            if (slotType == (byte)SWSHSlotType.Sharpedo)
            {
                if (WeatherBleedSymbolSharpedo.TryGetValue(locID, out permit) && permit.HasFlag(weather))
                    return true;
            }
            if (slotType is (byte)SWSHSlotType.HiddenMain or (byte)SWSHSlotType.HiddenMain2 or (byte)SWSHSlotType.HiddenMain3)
            {
                if (WeatherBleedHiddenGrass.TryGetValue(locID, out permit) && permit.HasFlag(weather))
                    return true;
            }

            return false;
        }

        private class DumpableLocation
        {
            public static readonly DumpableLocation Empty = new(new(), 0, 0);

            public readonly List<Slot8> Slots;
            public readonly byte Location;
            public readonly byte SlotType;

            public DumpableLocation(List<Slot8> slots, byte location, byte slotType)
            {
                Slots = slots;
                Location = location;
                SlotType = slotType;
            }

            public byte[] Serialize() => SerializeSlot8(Location, Slots, SlotType);
        }

        private static byte[] SerializeSlot8(byte locID, IEnumerable<Slot8> list, byte slotType)
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
                bw.Write(slotType);

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

            All = Normal | Overcast | Raining | Thunderstorm | Intense_Sun | Snowing | Snowstorm | Sandstorm | Heavy_Fog,
            Stormy = Raining | Thunderstorm,
            Icy = Snowing | Snowstorm,
            All_IoA = Normal | Overcast | Stormy | Intense_Sun | Sandstorm | Heavy_Fog,         // IoA can have everything but snow
            All_CT = Normal | Overcast | Stormy | Intense_Sun | Icy | Heavy_Fog,                // CT can have everything but sand
            No_Sun_Sand = Normal | Overcast | Stormy | Icy | Heavy_Fog,                         // Everything but sand and sun
            All_Ballimere = Normal | Overcast | Stormy | Intense_Sun | Snowing | Heavy_Fog,     // All Ballimere Lake weather
        }

        private enum SWSHSlotType
        {
            SymbolMain,
            SymbolMain2,
            SymbolMain3,

            HiddenMain, // Table with the tree/fishing slots
            HiddenMain2,
            HiddenMain3,

            Surfing,
            Surfing2,
            Sky,
            Sky2,
            Ground,
            Ground2,
            Sharpedo,

            OnlyFishing,
            Inaccessible,
        }

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

        private static readonly Dictionary<int, SWSHEncounterType> WeatherbyArea = new()
        {
            { 68, SWSHEncounterType.Intense_Sun }, // Route 6
            { 88, SWSHEncounterType.Snowing }, // Route 8 (Steamdrift Way)
            { 90, SWSHEncounterType.Snowing }, // Route 9
            { 92, SWSHEncounterType.Snowing }, // Route 9 (Circhester Bay)
            { 94, SWSHEncounterType.Overcast }, // Route 9 (Outer Spikemuth)
            { 106, SWSHEncounterType.Snowstorm }, // Route 10
            { 122, SWSHEncounterType.All }, // Rolling Fields
            { 124, SWSHEncounterType.All }, // Dappled Grove
            { 126, SWSHEncounterType.All }, // Watchtower Ruins
            { 128, SWSHEncounterType.All }, // East Lake Axewell
            { 130, SWSHEncounterType.All }, // West Lake Axewell
            { 132, SWSHEncounterType.All }, // Axew's Eye
            { 134, SWSHEncounterType.All }, // South Lake Miloch
            { 136, SWSHEncounterType.All }, // Giant's Seat
            { 138, SWSHEncounterType.All }, // North Lake Miloch
            { 140, SWSHEncounterType.All }, // Motostoke Riverbank
            { 142, SWSHEncounterType.All }, // Bridge Field
            { 144, SWSHEncounterType.All }, // Stony Wilderness
            { 146, SWSHEncounterType.All }, // Dusty Bowl
            { 148, SWSHEncounterType.All }, // Giant's Mirror
            { 150, SWSHEncounterType.All }, // Hammerlocke Hills
            { 152, SWSHEncounterType.All }, // Giant's Cap
            { 154, SWSHEncounterType.All }, // Lake of Outrage
            { 164, SWSHEncounterType.Normal | SWSHEncounterType.Overcast | SWSHEncounterType.Stormy | SWSHEncounterType.Intense_Sun | SWSHEncounterType.Heavy_Fog }, // Fields of Honor
            { 166, SWSHEncounterType.Normal | SWSHEncounterType.Overcast | SWSHEncounterType.Stormy | SWSHEncounterType.Intense_Sun | SWSHEncounterType.Heavy_Fog }, // Soothing Wetlands
            { 168, SWSHEncounterType.All_IoA }, // Forest of Focus
            { 170, SWSHEncounterType.Normal | SWSHEncounterType.Overcast | SWSHEncounterType.Stormy | SWSHEncounterType.Intense_Sun | SWSHEncounterType.Heavy_Fog }, // Challenge Beach
            { 174, SWSHEncounterType.All_IoA }, // Challenge Road
            { 178, SWSHEncounterType.Normal | SWSHEncounterType.Overcast | SWSHEncounterType.Stormy | SWSHEncounterType.Intense_Sun | SWSHEncounterType.Heavy_Fog }, // Loop Lagoon
            { 180, SWSHEncounterType.All_IoA }, // Training Lowlands
            { 184, SWSHEncounterType.Normal | SWSHEncounterType.Overcast | SWSHEncounterType.Raining | SWSHEncounterType.Sandstorm | SWSHEncounterType.Intense_Sun | SWSHEncounterType.Heavy_Fog }, // Potbottom Desert
            { 186, SWSHEncounterType.Normal | SWSHEncounterType.Overcast | SWSHEncounterType.Stormy | SWSHEncounterType.Intense_Sun | SWSHEncounterType.Heavy_Fog }, // Workout Sea
            { 188, SWSHEncounterType.Normal | SWSHEncounterType.Overcast | SWSHEncounterType.Stormy | SWSHEncounterType.Intense_Sun | SWSHEncounterType.Heavy_Fog }, // Stepping-Stone Sea
            { 190, SWSHEncounterType.Normal | SWSHEncounterType.Overcast | SWSHEncounterType.Stormy | SWSHEncounterType.Intense_Sun | SWSHEncounterType.Heavy_Fog }, // Insular Sea
            { 192, SWSHEncounterType.Normal | SWSHEncounterType.Overcast | SWSHEncounterType.Stormy | SWSHEncounterType.Intense_Sun | SWSHEncounterType.Heavy_Fog }, // Honeycalm Sea
            { 194, SWSHEncounterType.Normal | SWSHEncounterType.Overcast | SWSHEncounterType.Stormy | SWSHEncounterType.Intense_Sun | SWSHEncounterType.Heavy_Fog }, // Honeycalm Island
            { 204, SWSHEncounterType.Normal | SWSHEncounterType.Overcast | SWSHEncounterType.Intense_Sun | SWSHEncounterType.Icy | SWSHEncounterType.Heavy_Fog }, // Slippery Slope
            { 208, SWSHEncounterType.Normal | SWSHEncounterType.Overcast | SWSHEncounterType.Intense_Sun | SWSHEncounterType.Icy | SWSHEncounterType.Heavy_Fog }, // Frostpoint Field
            { 210, SWSHEncounterType.All_CT }, // Giant's Bed
            { 212, SWSHEncounterType.All_CT }, // Old Cemetery
            { 214, SWSHEncounterType.Normal | SWSHEncounterType.Overcast | SWSHEncounterType.Intense_Sun | SWSHEncounterType.Icy | SWSHEncounterType.Heavy_Fog }, // Snowslide Slope
            { 216, SWSHEncounterType.Overcast }, // Tunnel to the Top
            { 218, SWSHEncounterType.Normal | SWSHEncounterType.Overcast | SWSHEncounterType.Intense_Sun | SWSHEncounterType.Icy | SWSHEncounterType.Heavy_Fog }, // Path to the Peak
            { 222, SWSHEncounterType.All_CT }, // Giant's Foot
            { 224, SWSHEncounterType.Overcast }, // Roaring-Sea Caves
            { 226, SWSHEncounterType.No_Sun_Sand }, // Frigid Sea
            { 228, SWSHEncounterType.All_CT }, // Three-Point Pass
            { 230, SWSHEncounterType.All_Ballimere }, // Ballimere Lake
            { 232, SWSHEncounterType.Overcast }, // Lakeside Cave
        };

        /// <summary>
        /// Weather types that may bleed into each location from adjacent locations for standard symbol encounter slots.
        /// </summary>
        private static readonly Dictionary<int, SWSHEncounterType> WeatherBleedSymbol = new()
        {
            { 166, SWSHEncounterType.All_IoA }, // Soothing Wetlands from Forest of Focus
            { 170, SWSHEncounterType.All_IoA }, // Challenge Beach from Forest of Focus
            { 182, SWSHEncounterType.All_IoA }, // Warm-Up Tunnel from Training Lowlands
            { 208, SWSHEncounterType.All_CT }, // Frostpoint Field from Giant's Bed
            { 216, SWSHEncounterType.Normal | SWSHEncounterType.Overcast | SWSHEncounterType.Intense_Sun | SWSHEncounterType.Icy | SWSHEncounterType.Heavy_Fog }, // Tunnel to the Top from Path to the Peak
            { 224, SWSHEncounterType.All_CT }, // Roaring-Sea Caves from Three-Point Pass
            { 230, SWSHEncounterType.All_CT }, // Ballimere Lake from Giant's Bed
            { 232, SWSHEncounterType.All_Ballimere }, // Lakeside Cave from Ballimere Lake
        };

        /// <summary>
        /// Weather types that may bleed into each location from adjacent locations for surfing symbol encounter slots.
        /// </summary>
        private static readonly Dictionary<int, SWSHEncounterType> WeatherBleedSymbolSurfing = new()
        {
            { 192, SWSHEncounterType.All_IoA }, // Honeycalm Sea from Training Lowlands
        };

        /// <summary>
        /// Weather types that may bleed into each location from adjacent locations for Sharpedo symbol encounter slots.
        /// </summary>
        private static readonly Dictionary<int, SWSHEncounterType> WeatherBleedSymbolSharpedo = new()
        {
            { 192, SWSHEncounterType.All_IoA }, // Honeycalm Sea from Training Lowlands
        };

        /// <summary>
        /// Weather types that may bleed into each location from adjacent locations, for standard hidden grass encounter slots.
        /// </summary>
        private static readonly Dictionary<int, SWSHEncounterType> WeatherBleedHiddenGrass = new()
        {
            { 166, SWSHEncounterType.All_IoA }, // Soothing Wetlands from Forest of Focus
            { 170, SWSHEncounterType.All_IoA }, // Challenge Beach from Forest of Focus
            { 208, SWSHEncounterType.All_CT }, // Frostpoint Field from Giant's Bed
            { 230, SWSHEncounterType.All_CT }, // Ballimere Lake from Giant's Bed
        };
    }
}
