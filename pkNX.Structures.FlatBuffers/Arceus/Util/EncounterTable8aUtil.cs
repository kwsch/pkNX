using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace pkNX.Structures.FlatBuffers
{
    public static class EncounterTable8aUtil
    {
        private const float SpawnerBias = 73; // lured unown and jet + run
        private const float WormholeBias = 15;
        private const float LandmarkBias = 15;

        public static IEnumerable<byte[]> GetEncounterDump(AreaInstance8a area,
            IReadOnlyDictionary<string, (string Name, int Index)> map, PokeMiscTable8a misc)
        {
            if (area.Locations.Length == 0)
                yield break;

            foreach (var table in area.Encounters.Table)
            {
                var slots = table.Table.Where(z => z.ShinyLock is ShinyType8a.Random).ToList();
                if (slots.Count == 0)
                    continue;

                foreach (var p in GetAreas(area, slots, table, map, misc))
                    yield return p;
                foreach (var x in area.SubAreas)
                {
                    foreach (var p in GetAreas(x, slots, table, map, misc))
                        yield return p;
                }
            }
        }

        private static IEnumerable<byte[]> GetAreas(AreaInstance8a area, IReadOnlyCollection<EncounterSlot8a> slots, EncounterTable8a table, IReadOnlyDictionary<string, (string Name, int Index)> map, PokeMiscTable8a misc)
        {
            if (area.Locations.Length == 0)
                yield break;

            int baseArea = map[area.Locations.First(z => z.IsNamedPlace).PlaceName].Index;
            // Spawners
            {
                var s = area.Spawners;
                var spawners = s.Where(z => z.UsesTable(table.TableID));
                var sl = spawners.SelectMany(z => z.GetIntersectingLocations(area.Locations, SpawnerBias));
                foreach (var a in GetAll(sl, SpawnerType.Spawner))
                    yield return a;
            }

            // Wormholes
            {
                var s = area.Wormholes;
                var spawners = s.Where(z => z.UsesTable(table.TableID));

                // Since Wormholes can have different bonus level ranges, we defer uniqueness testing to the outer method. Just yield all spawners.
                foreach (var w in spawners)
                {
                    var bmin = w.Field_20_Value.BonusLevelMin;
                    var bmax = w.Field_20_Value.BonusLevelMax;

                    var sl = w.GetIntersectingLocations(area.Locations, WormholeBias);
                    foreach (var a in GetAll(sl, SpawnerType.Wormhole, bmin, bmax))
                        yield return a;
                }
            }

            // Landmarks
            {
                var items = area.LandItems;
                var lis = items.Where(z => z.UsesTable(table.TableID)).ToList();

                var marks = area.LandMarks;
                var li = marks.Where(z => lis.Any(sz => z.UsesTable(sz.LandmarkItemSpawnTableID)));
                var sl = li.SelectMany(z => z.GetIntersectingLocations(area.Locations, LandmarkBias));
                foreach (var a in GetAll(sl, SpawnerType.Landmark))
                    yield return a;
            }

            IEnumerable<byte[]> GetAll(IEnumerable<PlacementLocation8a> places, SpawnerType type, int bmin = 0, int bmax = 0)
            {
                var temp = places.Select(z => z.PlaceName);
                var areas = temp.Select(z => map[z].Index).Distinct().ToList();
                if (areas.Count == 0)
                    yield break;
                if (areas.Remove(baseArea) && areas.Count == 0)
                    areas.Add(baseArea);

                foreach (var a in areas)
                {
                    var parent = baseArea;
                    if (IsDungeonZone(a))
                        parent = a; // disallow crossover-out for dungeons
                    yield return GetArea(a, parent, slots, table.MinLevel, table.MaxLevel, type, misc, bmin, bmax);
                }
            }
        }

        // 064 Seaside Hollow
        // 086 Wayward Cave
        // 095 Snowpoint Temple
        private static bool IsDungeonZone(int a) => a is 64 or 86 or 95;

        private static readonly int[] OybnSettings = { 15, 15, 15, 20, 20 };

        private static byte[] GetArea(int location, int parentArea, IReadOnlyCollection<EncounterSlot8a> slots,
            int tableMinLevel, int tableMaxLevel, SpawnerType type,
            PokeMiscTable8a misc,
            int bonusMin = 0,
            int bonusMax = 0)
        {
            using var ms = new MemoryStream();
            using var bw = new BinaryWriter(ms);
            bw.Write((byte)location);
            bw.Write((byte)parentArea);
            bw.Write((byte)type);
            bw.Write((byte)slots.Count);
            foreach (var s in slots)
            {
                var (_, min, max) = s.GetLevels(tableMinLevel, tableMaxLevel);
                min += bonusMin;
                max += bonusMax;
                int alpha = 0;
                var oybn = s.Oybn;
                if (oybn.Oybn1 || oybn.Oybn2)
                {
                    var miscEntry = misc.GetEntry(s.Species, s.Form);
                    var boostIndex = miscEntry.OybnLevelIndex;
                    var boost = OybnSettings[boostIndex - 1];
                    max += boost;
                    min += boost;
                    if (s.Oybn.Oybn2)
                        alpha = 2; // Oybn2 -- Master All Moves Possible?
                    else if (s.Oybn.Oybn1)
                        alpha = 1; // Oybn1 -- Master only Alpha move?
                }

                var gender = s.Gender == -1 ? 2 : s.Gender;
                bw.Write((ushort)s.Species);
                bw.Write((byte)s.Form);
                bw.Write((byte)alpha);
                bw.Write((byte)min);
                bw.Write((byte)max);
                bw.Write((byte)gender);
                bw.Write((byte)s.NumPerfectIvs);
            }
            return ms.ToArray();
        }

        public static IEnumerable<string> GetUnownLines(AreaInstance8a area, IReadOnlyDictionary<string, (string Name, int Index)> map)
        {
            yield return $"Area: {area.AreaName}";
            foreach (var u in area.Unown.Concat(area.SubAreas.SelectMany(x => x.Unown)))
            {
                var contained = u.GetContainingLocations(area.Locations).First().PlaceName;
                var name = map[contained].Name;
                var p = u.Parameters;
                yield return $"Unown {u.Identifier} @ {u.Hash_01:X16}_{u.Hash_03:X16} {u.Flag} ({p.GetConditionSummary()}) @ {p.Coordinates.ToTriple()}, {area.AreaName} = {name}";
            }
        }

        public static IEnumerable<string> GetUnownLinesBias(AreaInstance8a area, IReadOnlyDictionary<string, (string Name, int Index)> map, float bias)
        {
            yield return $"Area: {area.AreaName}";
            foreach (var u in area.Unown.Concat(area.SubAreas.SelectMany(x => x.Unown)))
            {
                var contained = u.GetIntersectingLocations(area.Locations, bias);
                foreach (var c in contained)
                {
                    var name = map[c.PlaceName].Name;
                    var p = u.Parameters;
                    yield return $"Unown {u.Identifier} @ {u.Hash_01:X16}_{u.Hash_03:X16} {u.Flag} ({p.GetConditionSummary()}) @ {p.Coordinates.ToTriple()}, {area.AreaName} = {name}";
                }
            }
        }

        public static IEnumerable<string> GetLines(EncounterMultiplerArchive8a multiplier_archive,
            PokeMiscTable8a misc, string[] speciesNames,
            AreaInstance8a area, IReadOnlyDictionary<string, (string Name, int Index)> map)
        {
            yield return $"Area: {area.AreaName}";

            foreach (var enctable in area.Encounters.Table) {
                foreach (var line in GetTableSummary(enctable, multiplier_archive, speciesNames, misc, area, map))
                    yield return $"\t{line}";

                yield return string.Empty;
            }
        }

        private static IEnumerable<string> GetUsedSpawnerSummary(EncounterTable8a t, AreaInstance8a area, IReadOnlyDictionary<string, (string Name, int Index)> valueTuples)
        {
            var usedBySpawners = Array.FindAll(area.Spawners, z => z.UsesTable(t.TableID));
            var usedByWormholes = Array.FindAll(area.Wormholes, z => z.UsesTable(t.TableID));
            var usedByLandmarkSpawns = Array.FindAll(area.LandItems, z => z.UsesTable(t.TableID));
            var usedByLandmarks = Array.FindAll(area.LandMarks, z => usedByLandmarkSpawns.Any(sz => z.UsesTable(sz.LandmarkItemSpawnTableID)));

            foreach (var s in usedBySpawners)
            {
                var contained = s.GetContainingLocations(area.Locations).First().PlaceName;
                var name = valueTuples[contained].Name;
                var p = s.Parameters;
                yield return $"Spawner @ {s.NameSummary}_{s.Field_01:X16} ({p.GetConditionSummary()}) @ {p.Coordinates.ToTriple()}, {area.AreaName} = {name}";
            }
            foreach (var s in usedByWormholes)
            {
                var contained = s.GetContainingLocations(area.Locations).First().PlaceName;
                var name = valueTuples[contained].Name;
                var p = s.Parameters;
                var c = s.Field_20_Value;
                yield return $"Wormhole: {s.NameSummary}_{s.Field_01:X16} ({p.GetConditionSummary()}) [{c.BonusLevelMin}-{c.BonusLevelMax}] @ {p.Coordinates.ToTriple()}, {area.AreaName} = {name}";
            }
            foreach (var s in usedByLandmarks)
            {
                var contained = s.GetContainingLocations(area.Locations).First().PlaceName;
                var name = valueTuples[contained].Name;
                var spawn = usedByLandmarkSpawns.First(sz => s.UsesTable(sz.LandmarkItemSpawnTableID));
                var p = s.Parameters;
                yield return $"Landmark: {s.NameSummary}_{s.Field_01:X16}_{spawn.NameSummary} ({p.GetConditionSummary()}) @ {p.Coordinates.ToTriple()}, {area.AreaName} = {name}";
            }
        }

        private static IEnumerable<string> GetTableSummary(EncounterTable8a t,
            EncounterMultiplerArchive8a multiplier_archive, string[] speciesNames,
            PokeMiscTable8a misc,
            AreaInstance8a area, IReadOnlyDictionary<string, (string Name, int Index)> valueTuples)
        {
            yield return $"{t}:";

            var totalUses = 0;
            foreach (var line in GetUsedSpawnerSummary(t, area, valueTuples))
            {
                totalUses++;
                yield return $"\t{line}";
            }

            foreach (var subArea in area.SubAreas)
            {
                foreach (var line in GetUsedSpawnerSummary(t, subArea, valueTuples))
                {
                    totalUses++;
                    yield return $"\t{line}";
                }
            }

            if (totalUses == 0)
            {
                yield return "\tNo spawners? Check that this is used somewhere.";
            }

            foreach (var line in GetLines(t.Table, multiplier_archive, speciesNames, t.MinLevel, t.MaxLevel, misc))
                yield return $"\t{line}";
        }

        private static IEnumerable<string> GetLines(IReadOnlyList<EncounterSlot8a> arr,
            EncounterMultiplerArchive8a multiplier_archive, IReadOnlyList<string> speciesNames, int lvMin, int lvMax,
            PokeMiscTable8a misc)
        {
            var dividedTables = EncounterDetail8a.GetEmpty();
            FillSlots(arr, multiplier_archive, dividedTables);
            EncounterDetail8a.Divide(dividedTables);

            var sym = EncounterDetail8a.AnalyzeSymmetry(dividedTables);
            if (sym.Time && sym.Weather)
            {
                yield return "Any Time/All Weather:";
                foreach (var line in GetEffectiveTableSummary(dividedTables[0, 0], speciesNames, lvMin, lvMax, misc))
                    yield return $"\t{line}";
            }
            else if (sym.Weather)
            {
                for (var time = 0; time < dividedTables.GetLength(0); time++)
                {
                    yield return $"{(Time8a)time}/All Weather:";
                    foreach (var line in GetEffectiveTableSummary(dividedTables[time, 0], speciesNames, lvMin, lvMax, misc))
                        yield return $"\t{line}";
                }
            }
            else if (sym.Time)
            {
                for (var weather = 0; weather < dividedTables.GetLength(1); weather++)
                {
                    yield return $"Any Time/{(Weather8a)weather}:";
                    foreach (var line in GetEffectiveTableSummary(dividedTables[0, weather], speciesNames, lvMin, lvMax, misc))
                        yield return $"\t{line}";
                }
            }
            else
            {
                for (var time = 0; time < dividedTables.GetLength(0); time++)
                {
                    if (sym.Complexed[time])
                    {
                        yield return $"{(Time8a)time}/All Weather:";
                        foreach (var line in GetEffectiveTableSummary(dividedTables[time, 0], speciesNames, lvMin, lvMax, misc))
                            yield return $"\t{line}";
                    }
                    else
                    {
                        for (var weather = 0; weather < dividedTables.GetLength(1); weather++)
                        {
                            yield return $"{(Time8a)time}/{(Weather8a)weather}:";
                            foreach (var line in GetEffectiveTableSummary(dividedTables[time, weather], speciesNames, lvMin, lvMax, misc))
                                yield return $"\t{line}";
                        }
                    }
                }
            }
        }

        private static void FillSlots(IEnumerable<EncounterSlot8a> arr, EncounterMultiplerArchive8a multArchive, List<EncounterDetail8a>[,] dividedTables)
        {
            var ctr = 0;
            foreach (var slot in arr.OrderByDescending(sl => sl.BaseProbability))
            {
                var defaults = multArchive.GetEncounterMultiplier(slot);
                for (var time = 0; time < dividedTables.GetLength(0); time++)
                {
                    var MultT = slot.GetTimeModifier(time, defaults);
                    for (var weather = 0; weather < dividedTables.GetLength(1); weather++)
                    {
                        var MultW = slot.GetWeatherModifier(weather, defaults);
                        var rate = slot.BaseProbability * MultT * MultW;
                        if (rate == 0)
                            continue;

                        var detail = new EncounterDetail8a(slot.BaseProbability * MultT * MultW, MultT, MultW, ctr, slot);
                        dividedTables[time, weather].Add(detail);
                    }
                }

                ctr++;
            }
        }

        private static IEnumerable<string> GetEffectiveTableSummary(IReadOnlyList<EncounterDetail8a> table, IReadOnlyList<string> speciesNames, int lvMin, int lvMax, PokeMiscTable8a misc)
        {
            if (table.Count == 0)
            {
                yield return " - None";
                yield break;
            }

            foreach (var tup in table)
            {
                var rate = tup.Rate;
                var slot = tup.Slot;

                string form = slot.Form == 0 ? string.Empty : $"-{slot.Form}";
                var spec_form = $"{speciesNames[slot.Species]}{form}";

                var summary = $"- {rate:00.00}%\t{spec_form,-12}";

                var (force, min, max) = slot.GetLevels(lvMin, lvMax);
                if (slot.Oybn.Oybn1 || slot.Oybn.Oybn2)
                {
                    var miscEntry = misc.GetEntry(slot.Species, slot.Form);
                    var boostIndex = miscEntry.OybnLevelIndex;
                    var boost = OybnSettings[boostIndex - 1];
                    max += boost;
                    min += boost;
                    summary += $"\tAlphaLevel={min}-{max}";
                }
                else if (force)
                {
                    summary += $"\tOverrideLevel={min}-{max}";
                }

                if (slot.Gender != -1)
                    summary += $"\tGender={slot.Gender}";

                if (slot.ShinyLock is not ShinyType8a.Random)
                    summary += $"\tShinyLock={slot.ShinyLock}";

                if (slot.AbilityRandType is not AbilityType8a.Any12)
                    summary += $"\tAbility={slot.AbilityRandType}";

                if (slot.Nature is not NatureType8a.Random)
                    summary += $"\tNature={slot.Nature}";

                var gvs = new[] { slot.GV_HP, slot.GV_ATK, slot.GV_DEF, slot.GV_SPA, slot.GV_SPD, slot.GV_SPE };
                if (gvs.Any(x => x != -1))
                    summary += $"\tGVS={string.Join("/", gvs.Select(v => v != -1 ? v.ToString() : "*").ToArray())}";

                if (slot.NumPerfectIvs != 0)
                    summary += $"\tPerfectIvs={slot.NumPerfectIvs}";

                var ivs = new[] { slot.IV_HP, slot.IV_ATK, slot.IV_DEF, slot.IV_SPA, slot.IV_SPD, slot.IV_SPE };
                if (ivs.Any(x => x != -1))
                    summary += $"\tIVS={string.Join("/", ivs.Select(v => v != -1 ? v.ToString() : "*").ToArray())}";

                var oybn = slot.Oybn;
                if (oybn.IsOybnAny)
                {
                    if (oybn.Oybn1 && !oybn.Oybn2 && oybn.Field_02 && oybn.Field_03)
                        summary += "\tOybn=Type1";
                    else if (oybn.Oybn1 && oybn.Oybn2 && oybn.Field_02 && oybn.Field_03)
                        summary += "\tOybn=Type2";
                    else
                        summary += $"\tOybn={{{(oybn.Oybn1 ? 1 : 0)},{(oybn.Oybn2 ? 1 : 0)},{(oybn.Field_02 ? 1 : 0)},{(oybn.Field_03 ? 1 : 0)}}}";
                }

                var elg = slot.Eligibility;
                if (slot.Eligibility.ConditionTypeID != ConditionType8a.None)
                {
                    summary += $"\tConditionType={elg.GetConditionTypeSummary()}";
                    summary += $"\tCondition={elg.GetConditionSummary()}";
                }

                if (!string.IsNullOrEmpty(slot.Behavior1))
                    summary += $"\t{nameof(slot.Behavior1)}=\"{slot.Behavior1}\"";

                if (!string.IsNullOrEmpty(slot.Behavior2))
                    summary += $"\t{nameof(slot.Behavior2)}=\"{slot.Behavior2}\"";

                if (slot.SlotID != 0xCBF29CE484222645)
                    summary += $"\tSlotID={slot.SlotName}";

                yield return summary;
            }
        }
    }
}
