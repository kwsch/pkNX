using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace pkNX.Structures
{
    public class NestHoleDistributionEncounter8Archive
    {
        public NestHoleDistributionEncounter8Table[] Tables { get; set; }
    }

    public class NestHoleDistributionEncounter8Table
    {
        public ulong TableID { get; set; }
        public uint GameVersion { get; set; }
        public byte Field_02 { get; set; }
        public byte Field_03 { get; set; }
        public NestHoleDistributionEncounter8[] Entries { get; set; }

        public IEnumerable<string> GetPrettySummary(IReadOnlyList<string> species, IReadOnlyList<string> items, IReadOnlyList<string> moves, IReadOnlyList<int> tmtrs,
            IReadOnlyList<INestHoleRewardTable> nest_drop_tables, IReadOnlyList<INestHoleRewardTable> nest_bonus_tables, IReadOnlyList<INestHoleRewardTable> dist_drop_tables, IReadOnlyList<INestHoleRewardTable> dist_bonus_tables, int index)
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

            IEnumerable<string> PrettySummary(NestHoleDistributionEncounter8 e)
            {
                var giga = e.IsGigantamax != 0 ? "Gigantamax " : string.Empty;
                var form = e.AltForm != 0 ? $"-{e.AltForm}" : string.Empty;
                var rank = $"{e.EncounterRank}-Star";
                yield return $"{rank} {giga}{species[e.Species]}{form}";
                yield return $"\tLv. {e.Level}";
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

                yield return $"\tMoves:";
                if (e.Move0 != 0) yield return $"\t\t- {moves[(int)e.Move0]}";
                if (e.Move1 != 0) yield return $"\t\t- {moves[(int)e.Move1]}";
                if (e.Move2 != 0) yield return $"\t\t- {moves[(int)e.Move2]}";
                if (e.Move3 != 0) yield return $"\t\t- {moves[(int)e.Move3]}";

                yield return "\tSelection Probabilities:";
                for (var i = 0; i < e.Probabilities.Length; i++)
                {
                    if (e.Probabilities[i] != 0)
                        yield return $"\t\t{i + 1}-Star Desired: {e.Probabilities[i]:00}%";
                }

                yield return "\tDrops:";
                foreach (var entry in GetOrderedDrops(drop_tables, e.DropTableID, e.EncounterRank - 1))
                    yield return $"\t\t{entry.Values[e.EncounterRank - 1],3}% {GetItemName(entry.ItemID)}";

                yield return "\tBonus Drops:";
                foreach (var entry in GetOrderedDrops(bonus_tables, e.BonusTableID, e.EncounterRank - 1))
                    yield return $"\t\t{entry.Values[e.EncounterRank - 1]} x {GetItemName(entry.ItemID)}";

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
                    return $"{items[(int)itemID]} {moves[tmtrs[100 + (int)itemID - 1130]]}";
                return items[(int)itemID];
            }
        }
    }

    public class NestHoleDistributionEncounter8
    {
        public int EntryIndex { get; set; }
        public int Species { get; set; }
        public int AltForm { get; set; }
        public int Level { get; set; }
        public ushort DynamaxLevel { get; set; }
        public uint Field_05 { get; set; }
        public uint Field_06 { get; set; }
        public uint Field_07 { get; set; }
        public uint Field_08 { get; set; }
        public uint Field_09 { get; set; }
        public uint Field_0A { get; set; }
        public byte Field_0B { get; set; }
        public int IsGigantamax { get; set; }
        public ulong DropTableID { get; set; }
        public ulong BonusTableID { get; set; }
        public int[] Probabilities { get; set; }
        public byte Field_10 { get; set; }
        public byte EncounterRank { get; set; }
        public byte Field_12 { get; set; }
        public byte Field_13 { get; set; }
        public byte Field_14 { get; set; }
        public byte Nature { get; set; }
        public int Field_16 { get; set; }
        public uint Move0 { get; set; }
        public uint Move1 { get; set; }
        public uint Move2 { get; set; }
        public uint Move3 { get; set; }
        public float DynamaxBoost { get; set; }
        public uint Field_1C { get; set; }
        public uint Field_1D { get; set; }
        public uint Field_1E { get; set; }
        public uint Field_1F { get; set; }
        public uint Field_20 { get; set; }
        public uint Field_21 { get; set; }
        public uint Field_22 { get; set; }
        public uint Field_23 { get; set; }
        public uint Field_24 { get; set; }
    }
}
