using System.ComponentModel;
using System.Diagnostics;
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global
#nullable disable

namespace pkNX.Structures.FlatBuffers.SWSH;

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class NestHoleCrystalEncounterArchive { }

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class NestHoleCrystalEncounterTable
{
    public IEnumerable<string> GetPrettySummary(IReadOnlyList<string> species, IReadOnlyList<string> items, IReadOnlyList<string> moves, IReadOnlyList<ushort> tmtrs,
        IEnumerable<INestHoleRewardTable> nest_drop_tables, IEnumerable<INestHoleRewardTable> nest_bonus_tables, IEnumerable<INestHoleRewardTable> dist_drop_tables, IEnumerable<INestHoleRewardTable> dist_bonus_tables, int index)
    {
        var drop_tables = nest_drop_tables.Concat(dist_drop_tables).ToArray();
        var bonus_tables = nest_bonus_tables.Concat(dist_bonus_tables).ToArray();

        Debug.WriteLine(index);

        for (int i = 0; i < Entries.Count; i++)
        {
            if (Entries[i].Species == 0)
                continue;
            yield return $"Dynamax Crystal: {GetItemName(1279 + i)}";
            foreach (var line in PrettySummary(Entries[i]))
                yield return $"\t{line}";
        }

        yield return string.Empty;

        IEnumerable<string> PrettySummary(NestHoleCrystalEncounter e)
        {
            var encounter_rank = GetEncounterRank(e.Level); // TODO: How is this actually encoded?
            var giga = e.IsGigantamax ? "Gigantamax " : string.Empty;
            var form = e.Form != 0 ? $"-{e.Form}" : string.Empty;
            var rank = $"{encounter_rank}-Star";
            yield return $"{rank} {giga}{species[e.Species]}{form}";
            yield return $"\tLv. {e.Level}";
            yield return $"\tDynamax Level: {e.DynamaxLevel}";
            yield return $"\tDynamax Boost: {e.DynamaxBoost:0.0}x";
            yield return $"\tIVs: {e.IVHP}/{e.IVATK}/{e.IVDEF}/{e.IVSPA}/{e.IVSPD}/{e.IVSPE}";
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

            yield return "\tDrops:";
            foreach (var entry in GetOrderedDrops(drop_tables, e.DropTableID, encounter_rank - 1))
                yield return $"\t\t{entry.Values[encounter_rank - 1],3}% {GetItemName((int)entry.Item)}";

            yield return "\tBonus Drops:";
            foreach (var entry in GetOrderedDrops(bonus_tables, e.BonusTableID, encounter_rank - 1))
                yield return $"\t\t{entry.Values[encounter_rank - 1]} x {GetItemName((int)entry.Item)}";

            yield return string.Empty;
        }

        IEnumerable<INestHoleReward> GetOrderedDrops(IReadOnlyList<INestHoleRewardTable> rewards, ulong tableID, int encounterRank)
        {
            var table = rewards.First(t => t.TableID == tableID);
            var list = table.Rewards
                .Where(d => d.Values[encounterRank] != 0)
                .OrderByDescending(d => d.Values[encounterRank])
                .ThenBy(d => GetItemName((int)d.Item));

            foreach (var entry in list)
                yield return entry;
        }

        string GetItemName(int itemID)
        {
            if (itemID is >= 1130 and < 1230) // TR
                return $"{items[itemID]} {moves[tmtrs[100 + itemID - 1130]]}";
            return items[itemID];
        }
    }

    private static int GetEncounterRank(int level) => level switch
    {
        >= 15 and <= 20 => 1,
        >= 25 and <= 30 => 2,
        >= 35 and <= 40 => 3,
        >= 45 and <= 50 => 4,
        >= 55 and <= 60 => 5,
        _ => 0,
    };

    public IEnumerable<string> GetSummary(IReadOnlyList<string> species, IReadOnlyList<string> items)
    {
        for (int i = 0; i < Entries.Count; i++)
        {
            if (Entries[i].Species == 0)
                continue;
            yield return Summary(Entries[i], i);
        }
        yield return string.Empty;

        string Summary(NestHoleCrystalEncounter e, int x)
        {
            // Comment
            var crystal = items[1279 + x];
            var form = e.Form != 0 ? $"-{e.Form}" : string.Empty;
            var gprefix = e.IsGigantamax ? "Gigantamax " : string.Empty;
            var comment = $"{crystal} {gprefix}{species[e.Species]}{form}";

            var ability = e.Ability switch
            {
                0 => "A0", // 1
                1 => "A1", // 2
                2 => "A2", // H
                3 => "A3", // 1/2 only
                4 => "A4", // 1/2/H
                _ => throw new Exception(),
            };

            // Constructor
            var spec = $"Species = {e.Species:000}";
            var lvl = $", Level = {e.Level:00}";
            const string loc = ", Location = 126";
            var abil = $", Ability = {ability}";
            var dyna = $", DynamaxLevel = {e.DynamaxLevel}";
            var moves = $", Moves = new[] {{{e.Move0:000},{e.Move1:000},{e.Move2:000},{e.Move3:000}}}";
            var ivs = $", IVs = new[] {{{e.IVHP},{e.IVATK},{e.IVDEF},{e.IVSPE},{e.IVSPA},{e.IVSPD}}}";
            var altform = e.Form == 0 ? string.Empty : $", Form = {e.Form}";
            var giga = !e.IsGigantamax ? string.Empty : ", CanGigantamax = true";

            return $"            new() {{ {spec}{lvl}{abil}{loc}{ivs}{dyna}{moves}{altform}{giga} }}, // {comment}";
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
public partial class NestHoleCrystalEncounter { }
