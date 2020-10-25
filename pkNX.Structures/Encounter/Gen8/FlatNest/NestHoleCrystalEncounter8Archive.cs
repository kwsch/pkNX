using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

#pragma warning disable CA1819 // Properties should not return arrays
namespace pkNX.Structures
{
    public class NestHoleCrystalEncounter8Archive
    {
        public NestHoleCrystalEncounter8Table[] Tables { get; set; }
    }

    public class NestHoleCrystalEncounter8Table
    {
        public ulong TableID { get; set; }
        public uint GameVersion { get; set; }
        public NestHoleCrystalEncounter8[] Entries { get; set; }

        public IEnumerable<string> GetPrettySummary(IReadOnlyList<string> species, IReadOnlyList<string> items, IReadOnlyList<string> moves, IReadOnlyList<int> tmtrs,
            IReadOnlyList<INestHoleRewardTable> nest_drop_tables, IReadOnlyList<INestHoleRewardTable> nest_bonus_tables, IReadOnlyList<INestHoleRewardTable> dist_drop_tables, IReadOnlyList<INestHoleRewardTable> dist_bonus_tables, int index)
        {
            var drop_tables = nest_drop_tables.Concat(dist_drop_tables).ToArray();
            var bonus_tables = nest_bonus_tables.Concat(dist_bonus_tables).ToArray();

            Debug.WriteLine(index);

            for (uint i = 0; i < Entries.Length; i++)
            {
                if (Entries[i].Species == 0)
                    continue;
                yield return $"Dynamax Crystal: {GetItemName(1279 + i)}";
                foreach (var line in PrettySummary(Entries[i]))
                    yield return $"\t{line}";
            }

            yield return string.Empty;

            IEnumerable<string> PrettySummary(NestHoleCrystalEncounter8 e)
            {
                var encounter_rank = GetEncounterRank(e.Level); // TODO: How is this actually encoded?
                var giga = e.IsGigantamax ? "Gigantamax " : string.Empty;
                var form = e.AltForm != 0 ? $"-{e.AltForm}" : string.Empty;
                var rank = $"{encounter_rank}-Star";
                yield return $"{rank} {giga}{species[e.Species]}{form}";
                yield return $"\tLv. {e.Level}";
                yield return $"\tDynamax Level: {e.DynamaxLevel}";
                yield return $"\tDynamax Boost: {e.DynamaxBoost:0.0}x";
                yield return $"\tIVs: {e.IV_Hp}/{e.IV_Atk}/{e.IV_Def}/{e.IV_SpAtk}/{e.IV_SpDef}/{e.IV_Spe}";
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
                    yield return $"\t\t{entry.Values[encounter_rank - 1],3}% {GetItemName(entry.ItemID)}";

                yield return "\tBonus Drops:";
                foreach (var entry in GetOrderedDrops(bonus_tables, e.BonusTableID, encounter_rank - 1))
                    yield return $"\t\t{entry.Values[encounter_rank - 1]} x {GetItemName(entry.ItemID)}";

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

        private static int GetEncounterRank(int level)
        {
            if (15 <= level && level <= 20)
                return 1;
            if (25 <= level && level <= 30)
                return 2;
            if (35 <= level && level <= 40)
                return 3;
            if (45 <= level && level <= 50)
                return 4;
            if (55 <= level && level <= 60)
                return 5;
            return 0;
        }

        public IEnumerable<string> GetSummary(IReadOnlyList<string> species, IReadOnlyList<string> items)
        {
            for (uint i = 0; i < Entries.Length; i++)
            {
                if (Entries[i].Species == 0)
                    continue;
                yield return Summary(Entries[i], i);
            }
            yield return string.Empty;

            string Summary(NestHoleCrystalEncounter8 e, uint x)
            {
                // Comment
                var crystal = items[(int)(1279 + x)];
                var form = e.AltForm != 0 ? $"-{e.AltForm}" : string.Empty;
                var gprefix = e.IsGigantamax ? "Gigantamax " : string.Empty;
                var comment = $"{crystal} {gprefix}{species[e.Species]}{form}";

                var ability = e.Ability switch
                {
                    0 => "A0", // 1
                    1 => "A1", // 2
                    2 => "A2", // H
                    3 => "A3", // 1/2 only
                    4 => "A4", // 1/2/H
                    _ => throw new Exception()
                };

                // Constructor
                var spec = $"Species = {e.Species:000}";
                var lvl = $", Level = {e.Level:00}";
                const string loc = ", Location = 126";
                var abil = $", Ability = {ability}";
                var dyna = $", DynamaxLevel = {e.DynamaxLevel}";
                var moves = $", Moves = new[] {{{e.Move0:000},{e.Move1:000},{e.Move2:000},{e.Move3:000}}}";
                var ivs = $", IVs = new[] {{{e.IV_Hp},{e.IV_Atk},{e.IV_Def},{e.IV_Spe},{e.IV_SpAtk},{e.IV_SpDef}}}";
                var altform = e.AltForm == 0 ? string.Empty : $", Form = {e.AltForm}";
                var giga = !e.IsGigantamax ? string.Empty : ", CanGigantamax = true";

                return $"            new EncounterStatic8NC {{ {spec}{lvl}{abil}{loc}{ivs}{dyna}{moves}{altform}{giga} }}, // {comment}";
            }
        }

        public string GetSummarySimple()
        {
            var tableID = TableID.ToString("X16");
            var tableData = TableUtil.GetTable(Entries);

            return tableID + Environment.NewLine + tableData + Environment.NewLine;
        }
    }

    public class NestHoleCrystalEncounter8
    {
        public int EntryIndex { get; set; }
        public int Species { get; set; }
        public int AltForm { get; set; }
        public int Level { get; set; }
        public byte DynamaxLevel { get; set; }
        public byte Ability { get; set; }
        public bool IsGigantamax { get; set; }
        public ulong DropTableID { get; set; }
        public ulong BonusTableID { get; set; }
        public byte Field_09 { get; set; }
        public byte Field_0A { get; set; }
        public byte Field_0B { get; set; }
        public byte Field_0C { get; set; }
        public byte Field_0D { get; set; }
        public byte Nature { get; set; }
        public short IV_Hp { get; set; }
        public short IV_Atk { get; set; }
        public short IV_Def { get; set; }
        public short IV_SpAtk { get; set; }
        public short IV_SpDef { get; set; }
        public short IV_Spe { get; set; }
        public uint Field_15 { get; set; }
        public uint Move0 { get; set; }
        public uint Move1 { get; set; }
        public uint Move2 { get; set; }
        public uint Move3 { get; set; }
        public float DynamaxBoost { get; set; }
        public uint Field_1B { get; set; }
        public uint Field_1C { get; set; }
        public uint Field_1D { get; set; } // Shield
        public uint Field_1E { get; set; } // % only if move
        public uint Field_1F { get; set; } // Move ID
        public uint Field_20 { get; set; } // Shield only if move
        public uint Field_21 { get; set; } // % only if move
        public uint Field_22 { get; set; } // Move ID
        public uint Field_23 { get; set; } // shield? only if move
    }
}
