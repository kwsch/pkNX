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
        public byte EncounterRate { get; set; }
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
                var giga = e.IsGigantamax ? "Gigantamax " : string.Empty;
                var form = e.AltForm != 0 ? $"-{e.AltForm}" : string.Empty;
                var rank = $"{e.MinRank + 1}-Star";
                yield return $"{rank} {giga}{species[e.Species]}{form}";
                yield return $"\tLv. {e.Level}";
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
                var dropTable = e.MinRank;
                foreach (var entry in GetOrderedDrops(drop_tables, e.DropTableID, dropTable))
                    yield return $"\t\t{entry.Values[e.MinRank],3}% {GetItemName(entry.ItemID)}";

                yield return "\tBonus Drops:";
                foreach (var entry in GetOrderedDrops(bonus_tables, e.BonusTableID, dropTable))
                    yield return $"\t\t{entry.Values[e.MinRank]} x {GetItemName(entry.ItemID)}";

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


        public IEnumerable<string> GetSummary(IReadOnlyList<string> species, int index)
        {
            foreach (var entry in Entries)
                yield return Summary(entry);
            yield return string.Empty;

            string Summary(NestHoleDistributionEncounter8 e)
            {
                var comment = $" // {species[e.Species]}{(e.AltForm == 0 ? string.Empty : "-" + e.AltForm)}";
                var gender = e.Gender == 0 ? string.Empty : $", Gender = {e.Gender - 1}";
                var altform = e.AltForm == 0 ? string.Empty : $", Form = {e.AltForm}";
                var giga = !e.IsGigantamax ? string.Empty : ", CanGigantamax = true";
                var moves = $", Moves = new[]{{ {e.Move0:000}, {e.Move1:000}, {e.Move2:000}, {e.Move3:000} }}";
                var shiny = e.ShinyLock switch
                {
                    0 => string.Empty,
                    1 => $", Shiny = Shiny.Never",
                    2 => $", Shiny = Shiny.Always",
                    _ => throw new Exception()
                };
                var ability = e.Ability switch
                {
                    0 => "A0", // 1
                    1 => "A1", // 2
                    2 => "A2", // H
                    3 => "A3", // 1/2 only
                    4 => "A4", // 1/2/H
                    _ => throw new Exception()
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
                return $"            new EncounterStatic8ND({e.Level:00},{e.DynamaxLevel:00},{flawless}) {{ Species = {e.Species:000}, Ability = {ability}{moves}{gender}{altform}{giga}{shiny} }},{comment}";
            }
        }

        public string GetSummarySimple()
        {
            var tableID = TableID.ToString("X16");
            var tableData = TableUtil.GetTable(Entries);

            return tableID + Environment.NewLine + tableData + Environment.NewLine;
        }
    }

    public class NestHoleDistributionEncounter8
    {
        public int EntryIndex { get; set; }
        public int Species { get; set; }
        public int AltForm { get; set; }
        public int Level { get; set; }
        public ushort DynamaxLevel { get; set; }
        public uint Field_05 { get; set; } // probably EVs
        public uint Field_06 { get; set; }
        public uint Field_07 { get; set; }
        public uint Field_08 { get; set; }
        public uint Field_09 { get; set; }
        public uint Field_0A { get; set; }
        public byte Ability { get; set; }
        public bool IsGigantamax { get; set; }
        public ulong DropTableID { get; set; }
        public ulong BonusTableID { get; set; }
        public int[] Probabilities { get; set; }
        public byte Gender { get; set; }
        public byte FlawlessIVs { get; set; }
        public byte ShinyLock { get; set; }
        public byte Field_13 { get; set; } // 3/4
        public byte Field_14 { get; set; } // 3/4/5 -- +1 for second entries
        public byte Nature { get; set; }
        public int Field_16 { get; set; }
        public uint Move0 { get; set; }
        public uint Move1 { get; set; }
        public uint Move2 { get; set; }
        public uint Move3 { get; set; }
        public float DynamaxBoost { get; set; }
        public uint Field_1C { get; set; }
        public uint Field_1D { get; set; }
        public uint Field_1E { get; set; } // Shield
        public uint Field_1F { get; set; } // % only if move
        public uint Field_20 { get; set; } // Move ID
        public uint Field_21 { get; set; } // Shield only if move
        public uint Field_22 { get; set; } // % only if move
        public uint Field_23 { get; set; } // Move ID
        public uint Field_24 { get; set; } // shield? only if move

        public int MinRank => Array.FindIndex(Probabilities, z => z != 0);
        public int MaxRank => Array.FindLastIndex(Probabilities, z => z != 0);
    }
}
