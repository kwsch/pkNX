using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using FlatSharp.Attributes;
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global
#nullable disable

namespace pkNX.Structures.FlatBuffers.SWSH;

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class NestHoleDistributionEncounterArchive { }

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class NestHoleDistributionEncounterTable
{
    public IEnumerable<string> GetPrettySummary(IReadOnlyList<string> species, IReadOnlyList<string> items, IReadOnlyList<string> moves, IReadOnlyList<ushort> tmtrs,
        IEnumerable<INestHoleRewardTable> nest_drop_tables, IEnumerable<INestHoleRewardTable> nest_bonus_tables, IEnumerable<INestHoleRewardTable> dist_drop_tables, IEnumerable<INestHoleRewardTable> dist_bonus_tables, int index)
    {
        var drop_tables = nest_drop_tables.Concat(dist_drop_tables).ToArray();
        var bonus_tables = nest_bonus_tables.Concat(dist_bonus_tables).ToArray();

        yield return $"Nest ID: {TableID}";
        Debug.WriteLine(index);

        foreach (var entry in Entries)
        {
            foreach (var line in PrettySummary(entry))
                yield return $"\t{line}";
        }

        yield return string.Empty;

        IEnumerable<string> PrettySummary(NestHoleDistributionEncounter e)
        {
            if (!e.Exists)
                yield break;

            var giga = e.IsGigantamax ? "Gigantamax " : string.Empty;
            var form = e.Form != 0 ? $"-{e.Form}" : string.Empty;
            var rank = $"{e.MinRank + 1}-Star";
            yield return $"{rank} {giga}{species[e.Species]}{form}";
            yield return $"\tLv. {e.Level}";
            if (e.Field13 == 6 && e.Field14 == 6) // related to whether or not the raid boss can be caught; enums/bitflags?
                yield return "\tCatchable: No";
            if (e.ShinyLock == 1)
                yield return "\tShiny: Never";
            else if (e.ShinyLock == 2)
                yield return "\tShiny: Always";
            yield return $"\tDynamax Level: {e.DynamaxLevel}";
            yield return $"\tDynamax Boost: {e.DynamaxBoost:0.0}x";
            /*yield return $"\tGender: {new[] { "Random", "Male", "Female", "Genderless" }[e.Gender]}";

            var ability = e.Ability switch
            {
                3 => "Hidden",
                4 => "Any",
                _ => throw new Exception()
            };
            yield return $"\tAbility: {ability}";*/

            yield return "\tMoves:";
            if (e.Move0 != 0) yield return $"\t\t- {moves[(int)e.Move0]}";
            if (e.Move1 != 0) yield return $"\t\t- {moves[(int)e.Move1]}";
            if (e.Move2 != 0) yield return $"\t\t- {moves[(int)e.Move2]}";
            if (e.Move3 != 0) yield return $"\t\t- {moves[(int)e.Move3]}";

            yield return "\tSelection Probabilities:";
            for (var i = 0; i < e.Probabilities.Count; i++)
            {
                if (e.Probabilities[i] != 0)
                    yield return $"\t\t{i + 1}-Star Desired: {e.Probabilities[i]:00}%";
            }

            yield return "\tDrops:";
            var dropTable = e.MinRank;
            foreach (var entry in GetOrderedDrops(drop_tables, e.DropTableID, dropTable))
                yield return $"\t\t{entry.Values[e.MinRank],3}% {GetItemName(entry.Item)}";

            yield return "\tBonus Drops:";
            foreach (var entry in GetOrderedDrops(bonus_tables, e.BonusTableID, dropTable))
                yield return $"\t\t{entry.Values[e.MinRank]} x {GetItemName(entry.Item)}";

            yield return string.Empty;
        }

        IEnumerable<INestHoleReward> GetOrderedDrops(IReadOnlyList<INestHoleRewardTable> rewards, ulong tableID, int encounterRank)
        {
            var table = rewards.First(t => t.TableID == tableID);
            var list = table.Rewards
                .Where(d => d.Values[encounterRank] != 0)
                .OrderByDescending(d => d.Values[encounterRank])
                .ThenBy(d => GetItemName(d.Item));

            foreach (var entry in list)
                yield return entry;
        }

        string GetItemName(uint itemID)
        {
            if (itemID is >= 1130 and < 1230) // TR
                return $"{items[(int)itemID]} {moves[tmtrs[100 + (int)itemID - 1130]]}";
            return items[(int)itemID];
        }
    }

    public IEnumerable<string> GetSummary(IReadOnlyList<string> species, int index)
    {
        foreach (var entry in Entries)
        {
            if (entry.Exists)
                yield return Summary(entry, index);
        }
        yield return string.Empty;

        string Summary(NestHoleDistributionEncounter e, int encounterIndex)
        {
            var comment = $" // {species[e.Species]}{(e.Form == 0 ? string.Empty : "-" + e.Form)}";
            var gender = e.Gender == 0 ? string.Empty : $", Gender = {e.Gender - 1}";
            var altform = e.Form == 0 ? string.Empty : $", Form = {e.Form}";
            var istr = $", Index = {encounterIndex}";
            var giga = !e.IsGigantamax ? string.Empty : ", CanGigantamax = true";
            var moves = $", Moves = new({e.Move0:000}, {e.Move1:000}, {e.Move2:000}, {e.Move3:000})";
            var shiny = e.ShinyLock switch
            {
                0 => string.Empty,
                1 => ", Shiny = Shiny.Never",
                2 => ", Shiny = Shiny.Always",
                _ => throw new Exception(),
            };
            var ability = e.Ability switch
            {
                0 => "A0", // 1
                1 => "A1", // 2
                2 => "A2", // H
                3 => "A3", // 1/2 only
                4 => "A4", // 1/2/H
                _ => throw new Exception(),
            };

            // calc min/max ranks
            int min = e.MinRank;
            int max = e.MaxRank;
            for (int i = min; i < max; i++)
            {
                if (e.Probabilities[i] == 0)
                    throw new Exception();
            }
            var flawless = e.FlawlessIVs;
            var line = $"            new({e.Level:00},{e.DynamaxLevel:00},{flawless}) {{ Species = {e.Species:000}, Ability = {ability}{moves}{istr}{gender}{altform}{giga}{shiny} }},{comment}";
            if (e.Field13 == 6 && e.Field14 == 6)
                line = line.Insert(12, "//").Remove(10, 2); // comment out uncatchable encounters
            return line;
        }
    }

    public string GetSummarySimple()
    {
        var tableID = TableID.ToString("X16");
        var tableData = TableUtil.GetTable(Entries);

        return tableID + Environment.NewLine + tableData + Environment.NewLine;
    }
}

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class NestHoleDistributionEncounter
{
    public int MinRank => Array.FindIndex(Probabilities.ToArray(), z => z != 0);
    public int MaxRank => Array.FindLastIndex(Probabilities.ToArray(), z => z != 0);
    public bool Exists => Probabilities.Any(z => z != 0);

    public override string ToString() => $"[{MinRank},{MaxRank}] {(Species)Species}-{Form}";
}
