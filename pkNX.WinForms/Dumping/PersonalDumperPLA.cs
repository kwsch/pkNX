using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using pkNX.Structures;
using pkNX.Structures.FlatBuffers;
#nullable disable // meh

namespace pkNX.WinForms
{
    public class PersonalDumperPLA
    {
        private void AddTRs(List<string> lines, IMovesInfo_1 pi, string specCode)
        {
            if (!(TMIndexes?.Count > 0))
                return;
            var tmhm = pi.TMHM;
            int count = 0;
            lines.Add("Legacy TRs:");
            for (int i = 0; i < 100; i++)
            {
                if (!tmhm[100 + i])
                    continue;
                var move = TMIndexes[100 + i];
                lines.Add($"- [TR{i:00}] {Moves[move]}");
                count++;

                MoveSpeciesLearn[move].Add(specCode);
            }
            if (count == 0)
                lines.Add("None!");
        }

        public bool HasAbilities { get; set; } = true;
        public bool HasItems { get; set; } = true;

        public IReadOnlyList<string> Abilities { private get; set; }
        public IReadOnlyList<string> Types { private get; set; }
        public IReadOnlyList<string> Items { private get; set; }
        public IReadOnlyList<string> Colors { private get; set; }
        public IReadOnlyList<string> EggGroups { private get; set; }
        public IReadOnlyList<string> ExpGroups { private get; set; }
        public IReadOnlyList<string> EntryNames { private get; set; }
        public IReadOnlyList<string> Moves { protected get; set; }
        public IReadOnlyList<string> Species { private get; set; }
        public IReadOnlyList<string> ZukanA { private get; set; }
        public IReadOnlyList<string> ZukanB { private get; set; }

        public Learnset8aMeta[] EntryLearnsets { private get; set; }
        public IReadOnlyList<EggMoves> EntryEggMoves { private get; set; }
        public EvolutionSet8a[] Evos { private get; set; }
        public IReadOnlyList<ushort> TMIndexes { protected get; set; }

        private static readonly string[] AbilitySuffix = { " (1)", " (2)", " (H)" };
        private static readonly string[] ItemPrefix = { "Item 1 (50%)", "Item 2 (5%)", "Item 3 (1%)" };

        public IReadOnlyList<List<string>> MoveSpeciesLearn { get; private set; }

        public PersonalDumperSettings Settings = new();

        public List<string> Dump(IPersonalTable table)
        {
            var lines = new List<string>();
            var ml = new List<string>[Moves.Count];
            for (int i = 0; i < ml.Length; i++)
                ml[i] = new List<string>();
            MoveSpeciesLearn = ml;

            for (ushort species = 0; species <= table.MaxSpeciesID; species++)
            {
                var spec = table[species];
                for (byte form = 0; form < spec.FormCount; form++)
                    AddDump(lines, table, species, form);
            }
            return lines;
        }

        public void AddDump(List<string> lines, IPersonalTable table, ushort species, byte form)
        {
            var index = table.GetFormIndex(species, form);
            var entry = table[index];
            string name = EntryNames[index];
            AddDump(lines, entry, index, name, species, form);
            lines.Add("");
        }

        private void AddDump(List<string> lines, IPersonalInfo pi, int entry, string name, int species, int form)
        {
            if (pi is IPersonalMisc_1 { IsPresentInGame: false })
                return;

            var specCode = pi.FormCount > 1 ? $"{Species[species]}-{form}" : $"{Species[species]}";

            if (Settings.Stats)
                AddPersonalLines(lines, pi, entry, name, specCode);
            if (Settings.Learn)
            {
                AddLearnsets(lines, specCode, species, form);
                AddLearnsetsLegacy(lines, specCode, species, form);
            }
            if (Settings.Evo)
                AddEvolutions(lines, species, form);
            if (Settings.Dex)
                AddZukan(lines, entry);
        }

        private void AddZukan(List<string> lines, int entry)
        {
            if (entry >= Species.Count)
                return;
            lines.Add(ZukanA[entry].Replace("\\n", " "));
            lines.Add(ZukanB[entry].Replace("\\n", " "));
        }

        protected virtual void AddTMs(List<string> lines, IMovesInfo_1 pi, string SpecCode)
        {
            var tmhm = pi.TMHM;
            int count = 0;
            lines.Add("Legacy TMs:");
            for (int i = 0; i < 100; i++)
            {
                if (!tmhm[i])
                    continue;
                var move = TMIndexes[i];
                lines.Add($"- [TM{i:00}] {Moves[move]}");
                count++;

                MoveSpeciesLearn[move].Add(SpecCode);
            }
            if (count == 0)
                lines.Add("None!");

            AddTRs(lines, pi, SpecCode);
        }

        protected virtual void AddArmorTutors(List<string> lines, IMovesInfo_2 pi, string SpecCode)
        {
            var shop = pi.SpecialTutors[0];
            int count = 0;
            lines.Add("Move Shop:");
            for (int i = 0; i < Math.Min(shop.Length, Legal.MoveShop8_LA.Length); i++)
            {
                if (!shop[i])
                    continue;
                var move = Legal.MoveShop8_LA[i];
                lines.Add($"- {Moves[move]}");
                count++;

                MoveSpeciesLearn[move].Add(SpecCode);
            }
            if (count == 0)
                lines.Add("None!");
        }

        private void AddLearnsetsLegacy(List<string> lines, string specCode, int species, int form)
        {
            var learn = Array.Find(EntryLearnsets, z => z.Species == species && z.Form == form);
            if (learn is null)
                return;


            lines.Add("Legacy Level Up Moves:");
            foreach (var x in learn.Mainline)
            {
                var move = x.Move;
                var level = x.Level;
                lines.Add($"- [{level:00}] {Moves[move]}");
                MoveSpeciesLearn[move].Add(specCode);
            }
        }

        private void AddLearnsets(List<string> lines, string specCode, int species, int form)
        {
            var learn = Array.Find(EntryLearnsets, z => z.Species == species && z.Form == form);
            if (learn is null)
                return;

            lines.Add("Level Up Moves:");
            foreach (var x in learn.Arceus)
            {
                var move = x.Move;
                var level = x.Level;
                var master = x.LevelMaster;
                lines.Add($"- [{level:00}] [{master:00}] {Moves[move]}");
                MoveSpeciesLearn[move].Add(specCode);
            }
        }

        private void AddEvolutions(List<string> lines, int species, int form)
        {
            var evo = Array.Find(Evos, z => z.Species == species && z.Form == form);
            if (evo?.Table is null)
                return;
            var evo2 = evo.Table.Where(z => z.Species != 0).ToArray();
            if (evo2.Length == 0)
                return;

            var msg = evo2.Select(z => $"Evolves into {Species[z.Species]}-{z.Form} @ {z.Level} ({z.Method}) [{z.Argument}]");
            lines.AddRange(msg);
        }

        private void AddPersonalLines(List<string> lines, IPersonalInfo pi, int entry, string name, string specCode)
        {
            Debug.WriteLine($"Dumping {specCode}");
            lines.Add("======");
            lines.Add($"{entry:000} - {name} (Stage: {pi.EvoStage})");
            lines.Add("======");
            if (pi is IPersonalMisc_1 { IsPresentInGame: false })
                lines.Add("Present: No");
            lines.Add($"Base Stats: {pi.HP}.{pi.ATK}.{pi.DEF}.{pi.SPA}.{pi.SPD}.{pi.SPE} (BST: {pi.GetBaseStatTotal()})");
            lines.Add($"EV Yield: {pi.EV_HP}.{pi.EV_ATK}.{pi.EV_DEF}.{pi.EV_SPA}.{pi.EV_SPD}.{pi.EV_SPE}");
            lines.Add($"Gender Ratio: {pi.Gender}");
            lines.Add($"Catch Rate: {pi.CatchRate}");

            if (HasAbilities)
            {
                var abils = new int[pi.GetNumAbilities()];
                pi.GetAbilities(abils);
                var msg = string.Join(" | ", abils.Select((z, j) => Abilities[z] + AbilitySuffix[j]));
                lines.Add($"Abilities: {msg}");
            }

            lines.Add(string.Format(pi.Type1 != pi.Type2
                ? "Type: {0} / {1}"
                : "Type: {0}", Types[(int)pi.Type1], Types[(int)pi.Type2]));

            if (HasItems)
            {
                var items = new int[pi.GetNumItems()];
                pi.GetItems(items);
                if (items.Distinct().Count() == 1)
                    lines.Add($"Items: {Items[pi.Item1]}");
                else
                    lines.AddRange(items.Select((z, j) => $"{ItemPrefix[j]}: {Items[z]}"));
            }

            lines.Add($"EXP Group: {ExpGroups[pi.EXPGrowth]}");
            lines.Add(string.Format(pi.EggGroup1 != pi.EggGroup2
                ? "Egg Group: {0} / {1}"
                : "Egg Group: {0}", EggGroups[pi.EggGroup1], EggGroups[pi.EggGroup2]));
            lines.Add($"Height: {(decimal)pi.Height / 100:00.00}m, Weight: {(decimal)pi.Weight / 10:000.0}kg, Color: {Colors[pi.Color]}");
        }
    }
}