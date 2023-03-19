using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace pkNX.Structures.FlatBuffers.Arceus;

public static class EncounterTableUtil
{
    private const float SpawnerBias = 73; // lured unown and jet + run
    private const float WormholeBias = 15;
    private const float LandmarkBias = 15;

    public static IEnumerable<byte[]> GetEncounterDump(AreaInstance area,
        IReadOnlyDictionary<string, (string Name, int Index)> map, PokeMiscTable misc,
        NewHugeOutbreakGroupArchive nhoGroup,
        NewHugeOutbreakGroupLotteryArchive nhoLottery)
    {
        if (area.Locations.Count == 0)
            yield break;

        foreach (var table in area.Encounters)
        {
            var slots = table.Table.Where(z => z.ShinyLock is ShinyType.Random).ToList();
            if (slots.Count == 0)
                continue;

            foreach (var p in GetAreas(area, slots, table, map, misc, nhoGroup, nhoLottery))
                yield return p;
            foreach (var x in area.SubAreas)
            {
                foreach (var p in GetAreas(x, slots, table, map, misc, nhoGroup, nhoLottery))
                    yield return p;
            }
        }
    }

    private static IEnumerable<byte[]> GetAreas(AreaInstance area, IReadOnlyCollection<EncounterSlot> slots, EncounterTable table,
        IReadOnlyDictionary<string, (string Name, int Index)> map, PokeMiscTable misc,
        NewHugeOutbreakGroupArchive nhoGroup,
        NewHugeOutbreakGroupLotteryArchive nhoLottery)
    {
        if (area.Locations.Count == 0)
            yield break;

        int baseArea = map[area.Locations.First(z => z.IsNamedPlace).PlaceName].Index;
        // Spawners
        {
            var s = area.Spawners;
            var spawners = s.Where(z => z.UsesTable(table.TableID));
            var groups = spawners.GroupBy(GetSpawnerType);
            foreach (var g in groups)
            {
                if (g.Key is not (SpawnerType.Spawner or SpawnerType.SpawnerMass))
                    throw new Exception();
                var sl = g.SelectMany(z => z.GetIntersectingLocations(area.Locations, SpawnerBias));
                foreach (var a in GetAll(sl, g.Key))
                    yield return a;
            }
        }
        {
            var s = area.Spawners;
            var spawners = s.Where(z => nhoLottery.IsAreaGroup(z, nhoGroup, table.TableID));
            var groups = spawners.GroupBy(GetSpawnerType);
            foreach (var g in groups)
            {
                if (g.Key is not SpawnerType.SpawnerMMO)
                    throw new Exception();
                var sl = g.SelectMany(z => z.GetIntersectingLocations(area.Locations, SpawnerBias));
                foreach (var a in GetAll(sl, g.Key))
                    yield return a;
            }
        }

        // Wormholes
        {
            var s = area.Wormholes;
            var spawners = s.Where(z => z.UsesTable(table.TableID));

            // Since Wormholes can have different bonus level ranges, we defer uniqueness testing to the outer method. Just yield all spawners.
            foreach (var w in spawners)
            {
                var bmin = w.Field20Value.BonusLevelMin;
                var bmax = w.Field20Value.BonusLevelMax;

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

        IEnumerable<byte[]> GetAll(IEnumerable<PlacementLocation> places, SpawnerType type, int bmin = 0, int bmax = 0)
        {
            var temp = places.Select(z => z.PlaceName);
            var areas = temp.Select(z => map[z].Index).Distinct().ToList();
            if (areas.Count == 0)
                yield break;
            if (areas.Remove(baseArea) && areas.Count == 0)
                areas.Add(baseArea);
            else if (!areas.All(IsDungeonZone) && !areas.Contains(baseArea))
                areas.Add(baseArea);

            areas.Sort();
            yield return GetArea(areas, slots, table.MinLevel, table.MaxLevel, type, misc, bmin, bmax);
        }
    }

    private static SpawnerType GetSpawnerType(PlacementSpawner spawner)
    {
        var criteria = spawner.Parameters;
        if (criteria.ConditionID == Condition.Equal)
        {
            var arg = criteria.ConditionArg1;
            if (arg.StartsWith("FSYS_NEW_OUTBREAK"))
                return SpawnerType.SpawnerMMO;
            if (arg.StartsWith("WSYS_MASS_GANERATION"))
                return SpawnerType.SpawnerMass;
        }
        return SpawnerType.Spawner;
    }

    // 064 Seaside Hollow
    // 086 Wayward Cave
    // 095 Snowpoint Temple
    private static bool IsDungeonZone(int a) => a is 64 or 86 or 95;

    private static readonly int[] OybnSettings = { 15, 15, 15, 20, 20 };

    private static byte[] GetArea(IReadOnlyList<int> locations, IReadOnlyCollection<EncounterSlot> slots,
        int tableMinLevel, int tableMaxLevel, SpawnerType type,
        PokeMiscTable misc,
        int bonusMin = 0,
        int bonusMax = 0)
    {
        using var ms = new MemoryStream();
        using var bw = new BinaryWriter(ms);
        bw.Write((byte)locations.Count);
        foreach (var loc in locations)
            bw.Write((byte)loc);
        if (bw.BaseStream.Position % 2 != 0)
            bw.Write((byte)0);

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
                var miscEntry = misc.GetEntry((ushort)s.Species, (ushort)s.Form);
                var boostIndex = miscEntry.OybnLevelIndex;
                var boost = OybnSettings[boostIndex - 1];
                max += boost;
                min += boost;
                if (s.Oybn.Oybn2)
                    alpha = 2; // Oybn2 -- Master All Moves Possible?
                else if (s.Oybn.Oybn1)
                    alpha = 1; // Oybn1 -- Master only Alpha move?
            }

            if (min > max)
                min = max; // stupid Psyduck mass outbreak table.
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

    public static IEnumerable<string> GetUnownLines(AreaInstance area, IReadOnlyDictionary<string, (string Name, int Index)> map)
    {
        yield return $"Area: {area.AreaName}";
        foreach (var u in area.Unown.Concat(area.SubAreas.SelectMany(x => x.Unown)))
        {
            var contained = u.GetContainingLocations(area.Locations).First().PlaceName;
            var name = map[contained].Name;
            var p = u.Parameters;
            yield return $"Unown {u.Identifier} @ {u.Hash01:X16}_{u.Hash03:X16} {u.Flag} ({p.GetConditionSummary()}) @ {p.Coordinates.ToTriple()}, {area.AreaName} = {name}";
        }
    }

    public static IEnumerable<string> GetUnownLinesBias(AreaInstance area, IReadOnlyDictionary<string, (string Name, int Index)> map, float bias)
    {
        yield return $"Area: {area.AreaName}";
        foreach (var u in area.Unown.Concat(area.SubAreas.SelectMany(x => x.Unown)))
        {
            var contained = u.GetIntersectingLocations(area.Locations, bias);
            foreach (var c in contained)
            {
                var name = map[c.PlaceName].Name;
                var p = u.Parameters;
                yield return $"Unown {u.Identifier} @ {u.Hash01:X16}_{u.Hash03:X16} {u.Flag} ({p.GetConditionSummary()}) @ {p.Coordinates.ToTriple()}, {area.AreaName} = {name}";
            }
        }
    }

    public static IEnumerable<string> GetLines(EncounterMultiplierArchive multiplier_archive,
        PokeMiscTable misc, string[] speciesNames,
        AreaInstance area,
        NewHugeOutbreakGroupArchive nhoGroup,
        NewHugeOutbreakGroupLotteryArchive nhoLottery,
        IReadOnlyDictionary<string, (string Name, int Index)> map)
    {
        yield return $"Area: {area.AreaName}";

        foreach (var enctable in area.Encounters)
        {
            foreach (var line in GetTableSummary(enctable, multiplier_archive, speciesNames, misc, area, nhoGroup, nhoLottery, map))
                yield return $"\t{line}";

            yield return string.Empty;
        }
    }

    private static IEnumerable<string> GetUsedSpawnerSummary(EncounterTable t, AreaInstance area,
        IReadOnlyDictionary<string, (string Name, int Index)> valueTuples,
        NewHugeOutbreakGroupArchive nhoGroup,
        NewHugeOutbreakGroupLotteryArchive nhoLottery)
    {
        var usedBySpawners = area.Spawners.Where(z => z.UsesTable(t.TableID));
        var usedByWormholes = area.Wormholes.Where(z => z.UsesTable(t.TableID));
        var usedByLandmarkSpawns = area.LandItems.Where(z => z.UsesTable(t.TableID)).ToArray();
        var usedByLandmarks = area.LandMarks.Where(z => usedByLandmarkSpawns.Any(sz => z.UsesTable(sz.LandmarkItemSpawnTableID)));
        var usedByNHO = area.Spawners.Where(z => nhoLottery.IsAreaGroup(z, nhoGroup, t.TableID));

        foreach (var s in usedBySpawners)
        {
            var contained = s.GetContainingLocations(area.Locations).First().PlaceName;
            var name = valueTuples[contained].Name;
            var p = s.Parameters;
            yield return $"Spawner @ {s.NameSummary}_{s.Field01:X16} ({p.GetConditionSummary()}) @ {p.Coordinates.ToTriple()} ({s.MinSpawnCount}-{s.MaxSpawnCount}), {area.AreaName} = {name}";
        }
        foreach (var s in usedByNHO)
        {
            var contained = s.GetContainingLocations(area.Locations).First().PlaceName;
            var name = valueTuples[contained].Name;
            var p = s.Parameters;
            yield return $"SpawnerNHO @ {s.NameSummary}_{s.Field01:X16} ({p.GetConditionSummary()}) @ {p.Coordinates.ToTriple()} ({s.MinSpawnCount}-{s.MaxSpawnCount}), {area.AreaName} = {name}";
        }
        foreach (var s in usedByWormholes)
        {
            var contained = s.GetContainingLocations(area.Locations).First().PlaceName;
            var name = valueTuples[contained].Name;
            var p = s.Parameters;
            var c = s.Field20Value;
            yield return $"Wormhole: {s.NameSummary}_{s.Field01:X16} ({p.GetConditionSummary()}) [{c.BonusLevelMin}-{c.BonusLevelMax}] @ {p.Coordinates.ToTriple()} ({s.MinSpawnCount}-{s.MaxSpawnCount}), {area.AreaName} = {name}";
        }
        foreach (var s in usedByLandmarks)
        {
            var contained = s.GetContainingLocations(area.Locations).First().PlaceName;
            var name = valueTuples[contained].Name;
            var spawn = usedByLandmarkSpawns.First(sz => s.UsesTable(sz.LandmarkItemSpawnTableID));
            var p = s.Parameters;
            yield return $"Landmark: {s.NameSummary}_{s.Field01:X16}_{spawn.NameSummary} ({p.GetConditionSummary()}) @ {p.Coordinates.ToTriple()}, {area.AreaName} = {name}";
        }
    }

    private static IEnumerable<string> GetTableSummary(EncounterTable t,
        EncounterMultiplierArchive multiplier_archive, string[] speciesNames,
        PokeMiscTable misc,
        AreaInstance area,
        NewHugeOutbreakGroupArchive nhoGroup,
        NewHugeOutbreakGroupLotteryArchive nhoLottery,
        IReadOnlyDictionary<string, (string Name, int Index)> valueTuples)
    {
        yield return $"{t}:";

        var totalUses = 0;
        foreach (var line in GetUsedSpawnerSummary(t, area, valueTuples, nhoGroup, nhoLottery))
        {
            totalUses++;
            yield return $"\t{line}";
        }

        foreach (var subArea in area.SubAreas)
        {
            foreach (var line in GetUsedSpawnerSummary(t, subArea, valueTuples, nhoGroup, nhoLottery))
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

    private static IEnumerable<string> GetLines(IList<EncounterSlot> arr,
        EncounterMultiplierArchive multiplier_archive, IReadOnlyList<string> speciesNames, int lvMin, int lvMax,
        PokeMiscTable misc)
    {
        var dividedTables = EncounterDetail.GetEmpty();
        FillSlots(arr, multiplier_archive, dividedTables);
        EncounterDetail.Divide(dividedTables);

        var sym = EncounterDetail.AnalyzeSymmetry(dividedTables);
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
                yield return $"{(Time)time}/All Weather:";
                foreach (var line in GetEffectiveTableSummary(dividedTables[time, 0], speciesNames, lvMin, lvMax, misc))
                    yield return $"\t{line}";
            }
        }
        else if (sym.Time)
        {
            for (var weather = 0; weather < dividedTables.GetLength(1); weather++)
            {
                yield return $"Any Time/{(Weather)weather}:";
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
                    yield return $"{(Time)time}/All Weather:";
                    foreach (var line in GetEffectiveTableSummary(dividedTables[time, 0], speciesNames, lvMin, lvMax, misc))
                        yield return $"\t{line}";
                }
                else
                {
                    for (var weather = 0; weather < dividedTables.GetLength(1); weather++)
                    {
                        yield return $"{(Time)time}/{(Weather)weather}:";
                        foreach (var line in GetEffectiveTableSummary(dividedTables[time, weather], speciesNames, lvMin, lvMax, misc))
                            yield return $"\t{line}";
                    }
                }
            }
        }
    }

    private static void FillSlots(IEnumerable<EncounterSlot> arr, EncounterMultiplierArchive multArchive, List<EncounterDetail>[,] dividedTables)
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

                    var detail = new EncounterDetail(slot.BaseProbability * MultT * MultW, MultT, MultW, ctr, slot);
                    dividedTables[time, weather].Add(detail);
                }
            }

            ctr++;
        }
    }

    private static IEnumerable<string> GetEffectiveTableSummary(IReadOnlyList<EncounterDetail> table, IReadOnlyList<string> speciesNames, int lvMin, int lvMax, PokeMiscTable misc)
    {
        if (table.Count == 0)
        {
            yield return " - None";
            yield break;
        }

        foreach ((double rate, _, _, _, EncounterSlot? slot) in table)
        {
            string form = slot.Form == 0 ? string.Empty : $"-{slot.Form}";
            var spec_form = $"{speciesNames[slot.Species]}{form}";

            var summary = $"- {rate:00.00}%\t{spec_form,-12}";

            var (force, min, max) = slot.GetLevels(lvMin, lvMax);
            if (slot.Oybn.Oybn1 || slot.Oybn.Oybn2)
            {
                var miscEntry = misc.GetEntry((ushort)slot.Species, (ushort)slot.Form);
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

            if (slot.ShinyLock is not ShinyType.Random)
                summary += $"\tShinyLock={slot.ShinyLock}";

            if (slot.AbilityRandType is not AbilityType.Any12)
                summary += $"\tAbility={slot.AbilityRandType}";

            if (slot.Nature is not NatureType.Random)
                summary += $"\tNature={slot.Nature}";

            var gvs = new[] { slot.GVHP, slot.GVATK, slot.GVDEF, slot.GVSPA, slot.GVSPD, slot.GVSPE };
            if (gvs.Any(x => x != -1))
                summary += $"\tGVS={string.Join("/", gvs.Select(v => v != -1 ? v.ToString() : "*").ToArray())}";

            if (slot.NumPerfectIvs != 0)
                summary += $"\tPerfectIvs={slot.NumPerfectIvs}";

            var ivs = new[] { slot.IVHP, slot.IVATK, slot.IVDEF, slot.IVSPA, slot.IVSPD, slot.IVSPE };
            if (ivs.Any(x => x != -1))
                summary += $"\tIVS={string.Join("/", ivs.Select(v => v != -1 ? v.ToString() : "*").ToArray())}";

            var oybn = slot.Oybn;
            if (oybn.IsOybnAny)
            {
                if (oybn is { Oybn1: true, Oybn2: false, Field02: true, Field03: true })
                    summary += "\tOybn=Type1";
                else if (oybn is { Oybn1: true, Oybn2: true, Field02: true, Field03: true })
                    summary += "\tOybn=Type2";
                else
                    summary += $"\tOybn={{{(oybn.Oybn1 ? 1 : 0)},{(oybn.Oybn2 ? 1 : 0)},{(oybn.Field02 ? 1 : 0)},{(oybn.Field03 ? 1 : 0)}}}";
            }

            var elg = slot.Eligibility;
            if (slot.Eligibility.ConditionTypeID != ConditionType.None)
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

public static class Condition
{
    public const ulong Empty = 0;
    public const ulong GreaterThanEqual = 0x34F605C97C571896;
    public const ulong LessThanEqual = 0x488B1F9FBC949365;
    public const ulong NotEqual = 0x88C99E99F1FC6C55;
    public const ulong Equal = 0xB763CBE88B6F844F;
    public const ulong None = 0xCBF29CE484222645;
    public const ulong Between = 0xE5E34D77DC75E2EF;
}

public static class ConditionType
{
    public const ulong Empty = 0;
    public const ulong None = 0xCBF29CE484222645; // ""
    public const ulong BattleWin = 0xB2EE750DBCD8FFD6; // "battle_win" // UNUSED
    public const ulong EventEndFlagComparison = 0xB1D54CA3ACFD951C; // "event_end_flag_comparison"
    public const ulong FlagComparison = 0x3DDD5FEB4A6149; // "flag_comparison"
    public const ulong FlagComparisonMultiAnd = 0x1474C7C46A813955; // "flag_comparison_multi_and"
    public const ulong FlagComparisonMultiOr = 0x99A60B9D95356789; // "flag_comparison_multi_or"
    public const ulong Force = 0x344570C1301A7584; // "force"
    public const ulong IsRide = 0xE1DBDA285AC7E0C4; // "is_ride"
    public const ulong ItemNum = 0x4218EEEA9BFE8019; // "item_num"
    public const ulong MkrgCondition = 0xFDF49E06E61C30; // "mkrg_condition
    public const ulong MoonType = 0x9C765A1AAAC34605; // "moon_type"
    public const ulong MydressupParts = 0x6E4AB6860240F386; // "mydressup_parts"
    public const ulong NpcPassPokeForm = 0xDD65C7864951449B; // "npc_pass_poke_form"
    public const ulong NpcPassPokeIndividualNum = 0xCE6FB5F32B6297C5; // "npc_pass_poke_individual_num"
    public const ulong NpcPassPokeMonsNum = 0x26D399D556EE1C2D; // "npc_pass_poke_mons_num"
    public const ulong NpcPassPokeOybn = 0x769FC8C156B7CDBD; // "npc_pass_poke_oybn"
    public const ulong NpcPassPokeSeikaku = 0xF27E3B012D8F762C; // "npc_pass_poke_seikaku" // UNUSED
    public const ulong NpcPassPokeSex = 0xABE95BDC028692F3; // "npc_pass_poke_sex"
    public const ulong NpcPassPokeSizeAbsolute = 0xA6F824297967B404; // "npc_pass_poke_size_absolute" // UNUSED
    public const ulong NpcPassPokeSizeIndividual = 0x4CED96920003025E; // "npc_pass_poke_size_individual" // UNUSED
    public const ulong NpcPassPokeType = 0xC4BB08F0E0BB1979; // "npc_pass_poke_type"
    public const ulong NpcPassPokeWaza = 0xF26110F743861738; // "npc_pass_poke_waza"
    public const ulong PartyMonsNoNum = 0x0B830B240F28E61C; // "party_mons_no_num"
    public const ulong PartyPokeNum = 0x313CB8E1DEBB01DE; // "party_poke_num"
    public const ulong PhaseCondition = 0x8E77D62D1E38721E; // "phase_condition"
    public const ulong PlayerSex = 0xFA75906B85AE950F; // "player_sex"
    public const ulong PokeGetNum = 0x56E50D419BFB3A2C; // "poke_get_num"
    public const ulong PokeGetSelf = 0x7AAB4D6FEBAC4C0E; // "poke_get_self"
    public const ulong PokeResearchComplete = 0x5B017A2B0C07CD1A; // "poke_research_complete" // UNUSED
    public const ulong PokeTotalNum = 0x892E20D02629F1FA; // "poke_total_num"
    public const ulong Probability = 0x5E53C6367A2D36BC; // "probability"
    public const ulong ProgressWorkCondition = 0x43C4820F12B5385C; // "progress_work_condition"
    public const ulong ProgressWorkConditionHash = 0x409A7F367B037545; // "progress_work_condition_hash"
    public const ulong ProgressResidentWorkCondition = 0xD0D514A85FC0B50F; // "progress_resident_work_condition" // UNUSED
    public const ulong ProgressResidentWorkConditionHash = 0x1C5747049C3B1920; // "progress_resident_work_condition_hash"
    public const ulong QuestProgressCondition = 0x2041C674206FE0A7; // "quest_progress_condition"
    public const ulong QuestTaskClearCheck = 0x4BBF494C6CE53FEA; // "quest_task_clear_check" // UNUSED
    public const ulong ThrowItemCondition = 0x15C4DB49A2D05975; // "throw_item_condition" // UNUSED
    public const ulong TimeZone = 0x46EBE59F660B132B; // "time_zone"
    public const ulong TimeZoneAfternoon = 0x8E0F436D4E20E310; // "time_zone_afternoon"
    public const ulong TimeZoneNight = 0xD95B681BFE1BB762; // "time_zone_night"
    public const ulong WorkCondition = 0x2557279B949C81D6; // "work_condition"
    public const ulong ZukanResearchCondition = 0x854B2B1FDB9AC9B4; // "zukan_research_condition"
    public const ulong ZukanStageCondition = 0x9A193E701C7F9A61; // "zukan_stage_condition"
}
