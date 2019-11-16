using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace pkNX.Structures
{
    public class EncounterNest8Archive
    {
        public EncounterNest8Table[] Tables { get; set; }
    }

    public class EncounterNest8Table
    {
        public ulong TableID { get; set; }
        public int GameVersion { get; set; }
        public EncounterNest8[] Entries { get; set; }

        public string GetSummarySimple()
        {
            var tableID = TableID.ToString("X16");
            var tableData = TableUtil.GetTable(Entries);

            return tableID + Environment.NewLine + tableData + Environment.NewLine;
        }

        public IEnumerable<string> GetSummary(IReadOnlyList<string> species, int index)
        {
            foreach (var entry in Entries)
                yield return Summary(entry);
            yield return string.Empty;

            string Summary(EncounterNest8 e)
            {
                var comment = $" // {species[e.Species]}{(e.AltForm == 0 ? string.Empty : "-" + e.AltForm)}";
                var gender = e.Gender == 0 ? string.Empty : $", Gender = {e.Gender - 1}";
                var altform = e.AltForm == 0 ? string.Empty : $", Form = {e.AltForm}";
                var giga = !e.IsGigantamax ? string.Empty : ", CanGigantamax = true";
                var ability = e.Ability switch
                {
                    3 => " 4",
                    4 => "-1",
                    _ => throw new Exception()
                };

                // calc min/max ranks
                int min = Array.FindIndex(e.Probabilities, z => z != 0);
                int max = Array.FindLastIndex(e.Probabilities, z => z != 0);
                for (int i = min; i < max; i++)
                {
                    if (e.Probabilities[i] == 0)
                        throw new Exception();
                }
                var rank = e.EncounterRank;
                return $"            new EncounterStatic8N(Nest{index:00},{min},{max},{rank}) {{ Species = {e.Species:000}, Ability = {ability}{gender}{altform}{giga} }},{comment}";
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
                var form = e.AltForm != 0 ? $"-{e.AltForm}" : string.Empty;
                var rank = $"{e.EncounterRank + 1}-Star";
                yield return $"{rank} {giga}{species[e.Species]}{form}";
                yield return $"\tLv. {15 + (10 * e.EncounterRank)}-{20 + (10 * e.EncounterRank)}";
                yield return $"\tGender: {new[] { "Random", "Male", "Female", "Genderless" }[e.Gender]}";

                var ability = e.Ability switch
                {
                    3 => "Hidden",
                    4 => "Any",
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
                foreach (var entry in GetOrderedDrops(drop_tables, e.DropTableID, e.EncounterRank))
                    yield return $"\t\t{entry.Values[e.EncounterRank],3}% {GetItemName(entry.ItemID)}";

                yield return "\tBonus Drops:";
                foreach (var entry in GetOrderedDrops(bonus_tables, e.BonusTableID, e.EncounterRank))
                    yield return $"\t\t{entry.Values[e.EncounterRank]} x {GetItemName(entry.ItemID)}";

                yield return string.Empty;
            }

            IEnumerable<INestHoleReward> GetOrderedDrops(IReadOnlyList<INestHoleRewardTable> rewards, ulong tableID, int encounterRank)
            {
                var table = rewards.First(t => t.TableID == tableID);
                var list = table.Rewards
                    .Where(d => d.Values[encounterRank] != 0)
                    .OrderByDescending(d => d.Values[encounterRank])
                    .ThenBy(d => GetItemName(d.ItemID));

                foreach (var entry in list)
                    yield return entry;
            }

            string GetItemName(uint itemID)
            {
                if (1130 <= itemID && itemID < 1230) // TR
                    return $"{items[(int) itemID]} {moves[tmtrs[100 + (int) itemID - 1130]]}";
                return items[(int) itemID];
            }
        }
    }

    public class EncounterNest8
    {
        public int EntryIndex { get; set; }
        public int Species { get; set; }
        public int AltForm { get; set; }
        public ulong LevelTableID { get; set; }
        public int Ability { get; set; }
        public bool IsGigantamax { get; set; }
        public ulong DropTableID { get; set; }
        public ulong BonusTableID { get; set; }
        public int[] Probabilities { get; set; }
        public int Gender { get; set; }
        public int EncounterRank { get; set; }

        public FixedAbility AbilityPermitted
        {
            get => (FixedAbility) Ability;
            set => Ability = (int) value;
        }
    }

    public enum FixedAbility
    {
        Regular12 = 0,
        Only1 = 1,
        Only2 = 2,
        OnlyH = 3,
        Any = 4,
    }
}
