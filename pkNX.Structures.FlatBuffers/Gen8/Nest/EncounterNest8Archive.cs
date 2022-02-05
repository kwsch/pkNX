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

namespace pkNX.Structures.FlatBuffers
{
    [FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
    public class EncounterNest8Archive : IFlatBufferArchive<EncounterNest8Table>
    {
        [FlatBufferItem(0)] public EncounterNest8Table[] Table { get; set; }
    }

    [FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
    public class EncounterNest8Table
    {
        [FlatBufferItem(0)] public ulong TableID { get; set; }
        [FlatBufferItem(1)] public int GameVersion { get; set; }
        [FlatBufferItem(2)] public EncounterNest8[] Entries { get; set; }

        public string GetSummarySimple()
        {
            var tableID = TableID.ToString("X16");
            var tableData = TableUtil.GetTable(Entries);

            return tableID + Environment.NewLine + tableData + Environment.NewLine;
        }

        public IEnumerable<string> GetSummary(IReadOnlyList<string> species, int index)
        {
            foreach (var entry in Entries)
            {
                foreach (var summary in Summary(entry))
                    yield return summary;
            }

            yield return string.Empty;

            IEnumerable<string> Summary(EncounterNest8 e)
            {
                var comment = $" // {species[e.Species]}{(e.Form == 0 ? string.Empty : "-" + e.Form)}";
                var gender = e.Gender == 0 ? string.Empty : $", Gender = {e.Gender - 1}";
                var altform = e.Form == 0 ? string.Empty : $", Form = {e.Form}";
                var giga = !e.IsGigantamax ? string.Empty : ", CanGigantamax = true";
                var ability = e.Ability switch
                {
                    2 => "A2",
                    3 => "A3",
                    4 => "A4",
                    _ => throw new Exception()
                };
                var flawless = e.FlawlessIVs;

                // calc min/max ranks
                int min = e.MinRank;
                int max = e.MaxRank;

                int curMin = -1;
                for (int i = min; i >= 0 && i <= max; i++)
                {
                    if (e.Probabilities[i] != 0)
                    {
                        if (curMin == -1)
                            curMin = i;

                        if (i == max)
                            yield return $"            new(Nest{index:000},{curMin},{i},{flawless}) {{ Species = {e.Species:000}, Ability = {ability}{gender}{altform}{giga} }},{comment}";
                    }
                    else if (curMin != -1)
                    {
                        yield return $"            new(Nest{index:000},{curMin},{i - 1},{flawless}) {{ Species = {e.Species:000}, Ability = {ability}{gender}{altform}{giga} }},{comment}";
                        curMin = -1;
                    }
                }
            }
        }

        public IEnumerable<string> GetPrettySummary(IReadOnlyList<string> species, IReadOnlyList<string> items, IReadOnlyList<string> moves, IReadOnlyList<int> tmtrs,
            IReadOnlyList<NestHoleReward8Table> drop_tables, IReadOnlyList<NestHoleReward8Table> bonus_tables, int index)
        {
            yield return $"Nest ID: {TableID}";
            Debug.WriteLine(index);

            foreach (var entry in Entries)
            {
                foreach (var line in PrettySummary(entry))
                    yield return $"\t{line}";
            }

            yield return string.Empty;

            IEnumerable<string> PrettySummary(EncounterNest8 e)
            {
                var giga = e.IsGigantamax ? "Gigantamax " : string.Empty;
                var form = e.Form != 0 ? $"-{e.Form}" : string.Empty;
                var rank = $"{e.MinRank + 1}-Star";
                yield return $"{rank} {giga}{species[e.Species]}{form}";
                yield return $"\tLv. {15 + (10 * e.MinRank)}-{20 + (10 * e.MaxRank)}";
                yield return $"\tGender: {new[] { "Random", "Male", "Female", "Genderless" }[e.Gender]}";

                var ability = e.Ability switch
                {
                    2 => "A2",
                    3 => "A3",
                    4 => "A4",
                    _ => throw new Exception()
                };
                yield return $"\tAbility: {ability}";
                yield return "\tSelection Probabilities:";
                for (var i = 0; i < e.Probabilities.Length; i++)
                {
                    if (e.Probabilities[i] != 0)
                        yield return $"\t\t{i+1}-Star Desired: {e.Probabilities[i]:00}%";
                }

                yield return "\tDrops:";
                foreach (var entry in GetOrderedDrops(drop_tables, e.DropTableID, e.FlawlessIVs))
                    yield return $"\t\t{entry.Values[e.FlawlessIVs],3}% {GetItemName(entry.Item)}";

                yield return "\tBonus Drops:";
                foreach (var entry in GetOrderedDrops(bonus_tables, e.BonusTableID, e.FlawlessIVs))
                    yield return $"\t\t{entry.Values[e.FlawlessIVs]} x {GetItemName(entry.Item)}";

                yield return string.Empty;
            }

            IEnumerable<INestHoleReward> GetOrderedDrops(IReadOnlyList<INestHoleRewardTable> rewards, ulong tableID, int encounterRank)
            {
                var table = rewards.First(t => t.TableID == tableID);
                var list = table.Rewards
                    .Where(d => encounterRank < d.Values.Length && d.Values[encounterRank] != 0)
                    .OrderByDescending(d => d.Values[encounterRank])
                    .ThenBy(d => GetItemName(d.Item));

                foreach (var entry in list)
                    yield return entry;
            }

            string GetItemName(uint itemID)
            {
                if (itemID is >= 1130 and < 1230) // TR
                    return $"{items[(int) itemID]} {moves[tmtrs[100 + (int) itemID - 1130]]}";
                return items[(int) itemID];
            }
        }
    }

    [FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
    public class EncounterNest8
    {
        [FlatBufferItem(00)] public int EntryIndex { get; set; }
        [FlatBufferItem(01)] public int Species { get; set; }
        [FlatBufferItem(02)] public int Form { get; set; }
        [FlatBufferItem(03)] public ulong LevelTableID { get; set; }
        [FlatBufferItem(04)] public byte Ability { get; set; }
        [FlatBufferItem(05)] public bool IsGigantamax { get; set; }
        [FlatBufferItem(06)] public ulong DropTableID { get; set; }
        [FlatBufferItem(07)] public ulong BonusTableID { get; set; }
        [FlatBufferItem(08)] public uint[] Probabilities { get; set; }
        [FlatBufferItem(09)] public byte Gender { get; set; }
        [FlatBufferItem(10)] public byte FlawlessIVs { get; set; }

        public Species SpeciesID => (Species)Species;

        public FixedAbility AbilityPermitted
        {
            get => (FixedAbility) Ability;
            set => Ability = (byte) value;
        }

        public int MinRank => Array.FindIndex(Probabilities, z => z != 0);
        public int MaxRank => Array.FindLastIndex(Probabilities, z => z != 0);

        public override string ToString() => $"{EntryIndex:00} - {Species:000}";
    }

    public enum FixedAbility
    {
        Ability1 = 0,
        Ability2 = 1,
        AbilityH = 2,
        Ability1_2 = 3,
        Any = 4,
    }
}
