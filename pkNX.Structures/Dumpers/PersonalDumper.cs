using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace pkNX.Structures
{
    public class PersonalDumperSWSH : PersonalDumper
    {
        protected override void AddTMs(List<string> lines, PersonalInfo pi, string specCode)
        {
            base.AddTMs(lines, pi, specCode);
            AddTRs(lines, pi, specCode);
        }

        private void AddTRs(List<string> lines, PersonalInfo pi, string specCode)
        {
            if (!(TMIndexes?.Count > 0))
                return;
            var tmhm = pi.TMHM;
            int count = 0;
            lines.Add("TRs:");
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
    }

    public class PersonalDumper
    {
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

        public IReadOnlyList<Learnset> EntryLearnsets { private get; set; }
        public IReadOnlyList<EggMoves> EntryEggMoves { private get; set; }
        public IReadOnlyList<EvolutionSet> Evos { private get; set; }
        public IReadOnlyList<int> TMIndexes { protected get; set; }

        private static readonly string[] AbilitySuffix = { " (1)", " (2)", " (H)" };
        private static readonly string[] ItemPrefix = { "Item 1 (50%)", "Item 2 (5%)", "Item 3 (1%)" };

        private static readonly int[] ArmorTutorIndexes = { 0x325, 0x327, 0x32C, 0x324, 0x323, 0x32D, 0x32B, 0x32A, 0x32F, 0x32E, 0x31D, 0x326, 0x320, 0x329, 0x31F, 0x328, 0x31E, 0x322 };

        public IReadOnlyList<List<string>> MoveSpeciesLearn { get; private set; }

        public List<string> Dump(PersonalTable table)
        {
            var lines = new List<string>();
            var ml = new List<string>[Moves.Count];
            for (int i = 0; i < ml.Length; i++)
                ml[i] = new List<string>();
            MoveSpeciesLearn = ml;

            for (int species = 0; species <= table.MaxSpeciesID; species++)
            {
                var spec = table[species];
                for (int form = 0; form < spec.FormeCount; form++)
                    AddDump(lines, table, species, form);
            }
            return lines;
        }

        public void AddDump(List<string> lines, PersonalTable table, int species, int form)
        {
            var index = table.GetFormeIndex(species, form);
            var entry = table[index];
            string name = EntryNames[index];
            AddDump(lines, entry, index, name, species, form);
            lines.Add("");
        }

        private void AddDump(List<string> lines, PersonalInfo pi, int entry, string name, int species, int form)
        {
            if (pi is PersonalInfoSWSH s && !s.IsPresentInGame)
                return;

            var specCode = pi.FormeCount > 1 ? $"{Species[species]}-{form}" : $"{Species[species]}";

            AddPersonalLines(lines, pi, entry, name, specCode);
            AddLearnsets(lines, entry, specCode);
            AddEggMoves(lines, species, form, specCode);
            AddTMs(lines, pi, specCode);
            AddArmorTutors(lines, pi, specCode);
            AddEvolutions(lines, entry);
            //AddZukan(lines, entry);
        }

        private void AddZukan(List<string> lines, int entry)
        {
            if (entry >= Species.Count)
                return;
            lines.Add(ZukanA[entry].Replace("\\n", " "));
            lines.Add(ZukanB[entry].Replace("\\n", " "));
        }

        protected virtual void AddTMs(List<string> lines, PersonalInfo pi, string SpecCode)
        {
            var tmhm = pi.TMHM;
            int count = 0;
            lines.Add("TMs:");
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
        }

        protected virtual void AddArmorTutors(List<string> lines, PersonalInfo pi, string SpecCode)
        {

            var armor = pi.SpecialTutors[0];
            int count = 0;
            lines.Add("Armor Tutors:");
            for (int i = 0; i < armor.Length; i++)
            {
                if (!armor[i])
                    continue;
                var move = ArmorTutorIndexes[i];
                lines.Add($"- {Moves[move]}");
                count++;

                MoveSpeciesLearn[move].Add(SpecCode);
            }
            if (count == 0)
                lines.Add("None!");
        }

        private void AddLearnsets(List<string> lines, int entry, string specCode)
        {
            if (!(EntryLearnsets?.Count > 0))
                return;
            var learn = EntryLearnsets[entry];
            if (learn.Moves.Length == 0)
                return;

            lines.Add("Level Up Moves:");
            for (int i = 0; i < learn.Moves.Length; i++)
            {
                var move = learn.Moves[i];
                var level = learn.Levels[i];
                lines.Add($"- [{level:00}] {Moves[move]}");
                MoveSpeciesLearn[move].Add(specCode);
            }
        }

        private void AddEggMoves(List<string> lines, int species, int form, string specCode)
        {
            if (!(EntryEggMoves?.Count > 0))
                return;
            var egg = EntryEggMoves[species];
            if (egg is EggMoves7 e7 && form > 0)
                egg = EntryEggMoves[e7.FormTableIndex + form - 1];
            if (egg.Moves.Length == 0)
                return;

            lines.Add("Egg Moves:");
            foreach (var move in egg.Moves)
            {
                lines.Add($"- {Moves[move]}");
                MoveSpeciesLearn[move].Add(specCode);
            }
        }

        private void AddEvolutions(List<string> lines, int entry)
        {
            if (!(Evos?.Count > 0))
                return;
            var evo = Evos[entry];
            var evo2 = evo.PossibleEvolutions.Where(z => z.Species != 0).ToArray();
            if (evo2.Length == 0)
                return;

            var msg = evo2.Select(z => $"Evolves into {Species[z.Species]}-{z.Form} @ {z.Level} ({z.Method}) [{z.Argument}]");
            lines.AddRange(msg);
        }

        private void AddPersonalLines(List<string> lines, PersonalInfo pi, int entry, string name, string specCode)
        {
            Debug.WriteLine($"Dumping {specCode}");
            lines.Add("======");
            lines.Add($"{entry:000} - {name} (Stage: {pi.EvoStage})");
            lines.Add("======");
            if (pi is PersonalInfoSWSH s)
            {
                if (s.DexID != 0)
                    lines.Add($"Galar Dex: #{s.DexID:000}");
                if (s.DexIDArmor != 0)
                    lines.Add($"Armor Dex: #{s.DexIDArmor:000}");
                if (s.DexIDCrown != 0)
                    lines.Add($"Crown Dex: #{s.DexIDCrown:000}");
                if (s.DexID == 0 && s.DexIDArmor == 0 && s.DexIDCrown == 0)
                    lines.Add("Galar Dex: Foreign");

                if (s.CanNotDynamax)
                    lines.Add("Can Not Dynamax!");
            }
            lines.Add($"Base Stats: {pi.HP}.{pi.ATK}.{pi.DEF}.{pi.SPA}.{pi.SPD}.{pi.SPE} (BST: {pi.BST})");
            lines.Add($"EV Yield: {pi.EV_HP}.{pi.EV_ATK}.{pi.EV_DEF}.{pi.EV_SPA}.{pi.EV_SPD}.{pi.EV_SPE}");
            lines.Add($"Gender Ratio: {pi.Gender}");
            lines.Add($"Catch Rate: {pi.CatchRate}");

            if (HasAbilities)
            {
                var abils = pi.Abilities;
                var msg = string.Join(" | ", abils.Select((z, j) => Abilities[z] + AbilitySuffix[j]));
                lines.Add($"Abilities: {msg}");
            }

            lines.Add(string.Format(pi.Type1 != pi.Type2
                ? "Type: {0} / {1}"
                : "Type: {0}", Types[pi.Type1], Types[pi.Type2]));

            if (HasItems)
            {
                var items = pi.Items;
                if (items.Distinct().Count() == 1)
                    lines.Add($"Items: {Items[pi.Items[0]]}");
                else
                    lines.AddRange(items.Select((z, j) => $"{ItemPrefix[j]}: {Items[z]}"));
            }

            lines.Add($"EXP Group: {ExpGroups[pi.EXPGrowth]}");
            lines.Add(string.Format(pi.EggGroup1 != pi.EggGroup2
                ? "Egg Group: {0} / {1}"
                : "Egg Group: {0}", EggGroups[pi.EggGroup1], EggGroups[pi.EggGroup2]));
            lines.Add($"Hatch Cycles: {pi.HatchCycles}");
            lines.Add($"Height: {(decimal)pi.Height / 100:00.00}m, Weight: {(decimal)pi.Weight / 10:000.0}kg, Color: {Colors[pi.Color]}");
        }
    }
}
