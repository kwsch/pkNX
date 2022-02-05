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
    public class NestHoleCrystalEncounter8Archive : IFlatBufferArchive<NestHoleCrystalEncounter8Table>
    {
        [FlatBufferItem(0)] public NestHoleCrystalEncounter8Table[] Table { get; set; }
    }

    [FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
    public class NestHoleCrystalEncounter8Table
    {
        [FlatBufferItem(0)] public ulong TableID { get; set; }
        [FlatBufferItem(1)] public uint GameVersion { get; set; }
        [FlatBufferItem(2)] public NestHoleCrystalEncounter8[] Entries { get; set; }

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
                var form = e.Form != 0 ? $"-{e.Form}" : string.Empty;
                var rank = $"{encounter_rank}-Star";
                yield return $"{rank} {giga}{species[e.Species]}{form}";
                yield return $"\tLv. {e.Level}";
                yield return $"\tDynamax Level: {e.DynamaxLevel}";
                yield return $"\tDynamax Boost: {e.DynamaxBoost:0.0}x";
                yield return $"\tIVs: {e.IV_HP}/{e.IV_ATK}/{e.IV_DEF}/{e.IV_SPA}/{e.IV_SPD}/{e.IV_SPE}";
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
                    yield return $"\t\t{entry.Values[encounter_rank - 1],3}% {GetItemName(entry.Item)}";

                yield return "\tBonus Drops:";
                foreach (var entry in GetOrderedDrops(bonus_tables, e.BonusTableID, encounter_rank - 1))
                    yield return $"\t\t{entry.Values[encounter_rank - 1]} x {GetItemName(entry.Item)}";

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
                    _ => throw new Exception()
                };

                // Constructor
                var spec = $"Species = {e.Species:000}";
                var lvl = $", Level = {e.Level:00}";
                const string loc = ", Location = 126";
                var abil = $", Ability = {ability}";
                var dyna = $", DynamaxLevel = {e.DynamaxLevel}";
                var moves = $", Moves = new[] {{{e.Move0:000},{e.Move1:000},{e.Move2:000},{e.Move3:000}}}";
                var ivs = $", IVs = new[] {{{e.IV_HP},{e.IV_ATK},{e.IV_DEF},{e.IV_SPE},{e.IV_SPA},{e.IV_SPD}}}";
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

    [FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
    public class NestHoleCrystalEncounter8
    {
        [FlatBufferItem(00)] public int EntryIndex { get; set; }
        [FlatBufferItem(01)] public int Species { get; set; }
        [FlatBufferItem(02)] public int Form { get; set; }
        [FlatBufferItem(03)] public int Level { get; set; }
        [FlatBufferItem(04)] public byte DynamaxLevel { get; set; }
        [FlatBufferItem(05)] public byte Ability { get; set; }
        [FlatBufferItem(06)] public bool IsGigantamax { get; set; }
        [FlatBufferItem(07)] public ulong DropTableID { get; set; }
        [FlatBufferItem(08)] public ulong BonusTableID { get; set; }
        [FlatBufferItem(09)] public byte Field_09 { get; set; }
        [FlatBufferItem(10)] public byte Field_0A { get; set; }
        [FlatBufferItem(11)] public byte Field_0B { get; set; }
        [FlatBufferItem(12)] public byte Field_0C { get; set; }
        [FlatBufferItem(13)] public byte Field_0D { get; set; }
        [FlatBufferItem(14)] public byte Nature { get; set; }
        [FlatBufferItem(15)] public short IV_HP { get; set; }
        [FlatBufferItem(16)] public short IV_ATK { get; set; }
        [FlatBufferItem(17)] public short IV_DEF { get; set; }
        [FlatBufferItem(18)] public short IV_SPA { get; set; }
        [FlatBufferItem(19)] public short IV_SPD { get; set; }
        [FlatBufferItem(20)] public short IV_SPE { get; set; }
        [FlatBufferItem(21)] public uint Field_15 { get; set; }
        [FlatBufferItem(22)] public uint Move0 { get; set; }
        [FlatBufferItem(23)] public uint Move1 { get; set; }
        [FlatBufferItem(24)] public uint Move2 { get; set; }
        [FlatBufferItem(25)] public uint Move3 { get; set; }
        [FlatBufferItem(26)] public float DynamaxBoost { get; set; }
        [FlatBufferItem(27)] public uint Field_1B { get; set; }
        [FlatBufferItem(28)] public uint Field_1C { get; set; }
        [FlatBufferItem(29)] public uint Field_1D { get; set; } // Shield
        [FlatBufferItem(30)] public uint Field_1E { get; set; } // % only if move
        [FlatBufferItem(31)] public uint Field_1F { get; set; } // Move ID
        [FlatBufferItem(32)] public uint Field_20 { get; set; } // Shield only if move
        [FlatBufferItem(33)] public uint Field_21 { get; set; } // % only if move
        [FlatBufferItem(34)] public uint Field_22 { get; set; } // Move ID
        [FlatBufferItem(35)] public uint Field_23 { get; set; } // shield? only if move
    }
}
