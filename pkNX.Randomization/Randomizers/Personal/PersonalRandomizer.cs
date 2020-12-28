using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using pkNX.Structures;

namespace pkNX.Randomization
{
    public class PersonalRandomizer : Randomizer
    {
        private const int tmcount = 100;
        private const int eggGroupCount = 16;
        private const int TypeCount = 18;

        private readonly GameInfo Game;
        private readonly PersonalTable Table;
        private readonly EvolutionSet[] Evolutions;

        public PersonalRandSettings Settings { get; set; } = new();

        public PersonalRandomizer(PersonalTable table, GameInfo game, EvolutionSet[] evolutions)
        {
            Game = game;
            Table = table;
            Evolutions = evolutions;
            if (File.Exists("bannedabilites.txt"))
            {
                var data = File.ReadAllLines("bannedabilities.txt");
                var list = new List<int>(BannedAbilities);
                list.AddRange(data.Select(z => Convert.ToInt32(z)));
                BannedAbilities = list;
            }
        }

        public override void Execute()
        {
            if (Settings.ModifyByEvolutions)
                RandomizeChains();
            else
                RandomizeAllSpecies();
        }

        private void RandomizeAllSpecies()
        {
            for (var species = 0; species <= Game.MaxSpeciesID; species++)
                RandomizeSpecies(species);
        }

        private bool[] processed = Array.Empty<bool>();

        private void RandomizeChains()
        {
            processed = new bool[Table.TableLength];
            for (var species = 0; species <= Game.MaxSpeciesID; species++)
            {
                for (int f = 0; f <= Table[species].FormeCount; f++)
                    RandomizeChain(species, f);
            }
        }

        private bool AlreadyProcessed(int index)
        {
            var p = processed;
            if (p.Length <= index)
                return false;
            if (p[index])
                return true;
            p[index] = true;
            return false;
        }

        private void RandomizeChain(int species, int forme)
        {
            var index = Table.GetFormeIndex(species, forme);
            if (AlreadyProcessed(index))
                return;
            processed[index] = true;

            var entry = Table[index];
            Randomize(entry, species);
            ProcessEvolutions(species, forme, index);
        }

        private void ProcessEvolutions(int species, int forme, int devolvedIndex)
        {
            var evoindex = GetEvolutionEntry(species, forme);
            var evos = Evolutions[evoindex];

            if (evos.PossibleEvolutions.Length == 1)
            {
                var evo = evos.PossibleEvolutions[0];
                var ei = Table.GetFormeIndex(evo.Species, evo.Form);

                if (AlreadyProcessed(evoindex))
                    return;
                RandomizeSingleChain(evo, devolvedIndex);
                ProcessEvolutions(evo.Species, evo.Form, ei);
            }
            else
            {
                foreach (var evo in evos.PossibleEvolutions)
                {
                    var ei = Table.GetFormeIndex(evo.Species, evo.Form);
                    if (AlreadyProcessed(evoindex))
                        return;
                    RandomizeSplitChain(evo, devolvedIndex);
                    ProcessEvolutions(evo.Species, evo.Form, ei);
                }
            }
        }

        private void RandomizeSingleChain(EvolutionMethod evo, int devolvedIndex)
        {
            var child = Table[devolvedIndex];
            var z = Table.GetFormeEntry(evo.Species, evo.Form);
            RandomizeFrom(z, child, evo.Species);
        }

        private void RandomizeSplitChain(EvolutionMethod evo, int devolvedIndex)
        {
            var child = Table[devolvedIndex];
            var z = Table.GetFormeEntry(evo.Species, evo.Form);
            RandomizeFrom(z, child, evo.Species);
        }

        private int GetEvolutionEntry(int species, int form)
        {
            if (Game.Generation < 7)
                return species;
            return Table.GetFormeIndex(species, form);
        }

        private void RandomizeSpecies(int species)
        {
            var entry = Table[species];
            Randomize(entry, species);
            var formeCount = entry.FormeCount;
            for (int forme = 1; forme <= formeCount; forme++)
            {
                entry = Table.GetFormeEntry(species, forme);
                Randomize(entry, species);
            }
        }

        public void RandomizeFrom(PersonalInfo z, PersonalInfo child, int species)
        {
            if (Settings.ModifyStats)
                RandomizeStats(z);
            if (Settings.ShuffleStats)
                RandomShuffledStats(z);

            if (Settings.ModifyTypes)
            {
                if (Settings.InheritType && Settings.InheritTypeSetting != ModifyState.All)
                {
                    var types = child.Types;
                    switch (Settings.InheritTypeSetting)
                    {
                        case ModifyState.Two when Rand.Next(100) < Settings.InheritTypeNeitherChance:
                            types = new[] { GetRandomType(), GetRandomType() };
                            break;
                        case ModifyState.Two when Rand.Next(100) < Settings.InheritTypeOnlyOneChance:
                        case ModifyState.One when Rand.Next(100) < Settings.InheritTypeOnlyOneChance:
                            types[Rand.Next(2)] = GetRandomType();
                            break;
                    }
                    if (Rand.Next(0, 100) < Settings.SameTypeChance)
                    {
                        int index = Rand.Next(2);
                        types[index ^ 1] = types[index];
                    }
                    z.Types = types;
                }
                else
                {
                    RandomizeTypes(z);
                }
            }

            if (Settings.ModifyAbility)
            {
                if (Settings.InheritAbility)
                {
                    var abils = child.Abilities;
                    GetRandomAbilities(abils, Settings.InheritAbilitySetting);
                    z.Abilities = abils;
                }
                else
                {
                    RandomizeAbilities(z);
                }
            }

            if (Settings.ModifyLearnsetTM || Settings.ModifyLearnsetHM)
            {
                if (Settings.InheritChildTM)
                    z.TMHM = child.TMHM;
                else
                    RandomizeTMHM(z);
            }

            if (Settings.ModifyLearnsetTypeTutors)
            {
                if (Settings.InheritChildSpecial)
                    z.TypeTutors = child.TypeTutors;
                else
                    RandomizeTypeTutors(z, species);
            }

            if (Settings.ModifyLearnsetMoveTutors)
            {
                if (Settings.InheritChildTutor)
                    z.SpecialTutors = child.SpecialTutors;
                else
                    RandomizeSpecialTutors(z);
            }

            if (Settings.ModifyEgg)
                z.EggGroups = child.EggGroups;

            if (Settings.ModifyHeldItems)
            {
                if (Settings.InheritHeldItem)
                    z.Items = child.Items;
                else
                    RandomizeHeldItems(z);
            }

            ExecuteCatchRate(z);
        }

        private void GetRandomAbilities(int[] abils, ModifyState setting)
        {
            switch (setting)
            {
                case ModifyState.All:
                    GetRandomAbilities(abils);
                    break;
                case ModifyState.Two when Rand.Next(100) < Settings.InheritAbilityNeitherChance:
                    GetRandomAbilities(abils, 1);
                    break;
                case ModifyState.Two when Rand.Next(100) < Settings.InheritAbilityOnlyOneChance:
                case ModifyState.One when Rand.Next(100) < Settings.InheritAbilityOnlyOneChance:
                    GetRandomAbilities(abils, 2);
                    break;
            }
            if (Rand.Next(100) < Settings.SameAbilityChance)
            {
                int index = Rand.Next(2);
                abils[index^1] = abils[index];
            }
        }

        public void Randomize(PersonalInfo z, int species)
        {
            if (Settings.ModifyStats)
                RandomizeStats(z);
            if (Settings.ShuffleStats)
                RandomShuffledStats(z);

            if (Settings.ModifyTypes)
                RandomizeTypes(z);

            if (Settings.ModifyAbility)
                RandomizeAbilities(z);

            if (Settings.ModifyLearnsetTM || Settings.ModifyLearnsetHM)
                RandomizeTMHM(z);

            if (Settings.ModifyLearnsetTypeTutors)
                RandomizeTypeTutors(z, species);

            if (Settings.ModifyLearnsetMoveTutors)
                RandomizeSpecialTutors(z);

            if (Settings.ModifyEgg)
                RandomizeEggGroups(z);

            if (Settings.ModifyHeldItems)
                RandomizeHeldItems(z);

            ExecuteCatchRate(z);
        }

        private void ExecuteCatchRate(PersonalInfo z)
        {
            if (Settings.CatchRate == CatchRate.Random)
                z.CatchRate = Rand.Next(3, 251); // Random Catch Rate between 3 and 250.
            else if (Settings.CatchRate == CatchRate.BSTScaled)
                z.CatchRate = GetBSTCatchRate(z.BST);
        }

        private static int GetBSTCatchRate(int BST)
        {
            var c = 11 * (Math.Sqrt(Math.Max(0, 600 - BST)));

            const int min = 3;
            return (int)Math.Min(255, min + c);
        }

        private void RandomizeTMHM(PersonalInfo z)
        {
            var tms = z.TMHM;

            if (Settings.ModifyLearnsetTM)
            {
                for (int j = 0; j < tmcount; j++)
                    tms[j] = Rand.Next(100) < Settings.LearnTMPercent;
            }

            if (Settings.ModifyLearnsetHM)
            {
                for (int j = tmcount; j < tms.Length; j++)
                    tms[j] = Rand.Next(100) < Settings.LearnTMPercent;
            }

            z.TMHM = tms;
        }

        private void RandomizeTypeTutors(PersonalInfo z, int species)
        {
            var t = z.TypeTutors;
            for (int i = 0; i < t.Length; i++)
                t[i] = Rand.Next(100) < Settings.LearnTypeTutorPercent;

            // Make sure Rayquaza can learn Dragon Ascent.
            if (!Game.XY && species == (int)Species.Rayquaza)
                t[7] = true;

            z.TypeTutors = t;
        }

        private void RandomizeSpecialTutors(PersonalInfo z)
        {
            var tutors = z.SpecialTutors;
            foreach (bool[] tutor in tutors)
            {
                for (int i = 0; i < tutor.Length; i++)
                    tutor[i] = Rand.Next(100) < Settings.LearnMoveTutorPercent;
            }

            z.SpecialTutors = tutors;
        }

        private void RandomizeAbilities(PersonalInfo z)
        {
            var abils = z.Abilities;
            GetRandomAbilities(abils, Settings.Ability);
            z.Abilities = abils;
        }

        private void GetRandomAbilities(int[] abils, int skip = 0)
        {
            for (int i = 0; i < abils.Length - skip; i++)
                abils[i] = GetRandomAbility();
        }

        private void RandomizeEggGroups(PersonalInfo z)
        {
            var egg = z.EggGroups;
            egg[0] = GetRandomEggGroup();
            egg[1] = Rand.Next(100) < Settings.SameEggGroupChance ? egg[0] : GetRandomEggGroup();
            z.EggGroups = egg;
        }

        private void RandomizeHeldItems(PersonalInfo z)
        {
            var item = z.Items;
            for (int j = 0; j < item.Length; j++)
                item[j] = GetRandomHeldItem();
            z.Items = item;
        }

        private void RandomizeTypes(PersonalInfo z)
        {
            var t = z.Types;
            t[0] = GetRandomType();
            t[1] = Rand.Next(0, 100) < Settings.SameTypeChance ? t[0] : GetRandomType();
            z.Types = t;
        }

        private void RandomizeStats(PersonalInfo z)
        {
            // Fiddle with Base Stats, don't muck with Shedinja.
            var stats = z.Stats;
            if (stats[0] == 1)
                return;
            int RandDeviation() => Rand.Next(Settings.StatDeviationMin, Settings.StatDeviationMax);
            for (int i = 0; i < stats.Length; i++)
            {
                if (!Settings.StatsToRandomize[i])
                    continue;

                var val = stats[i] * RandDeviation() / 100;
                stats[i] = Math.Max(1, Math.Min(255, val));
            }
            z.Stats = stats;
        }

        private static void RandomShuffledStats(PersonalInfo z)
        {
            // Fiddle with Base Stats, don't muck with Shedinja.
            var stats = z.Stats;
            if (stats[0] == 1)
                return;
            Util.Shuffle(stats);
            z.Stats = stats;
        }

        private int GetRandomType() => Rand.Next(0, TypeCount);
        private int GetRandomEggGroup() => Rand.Next(1, eggGroupCount);
        private int GetRandomHeldItem() => Game.HeldItems.Length > 1 ? Game.HeldItems[Rand.Next(1, Game.HeldItems.Length)] : 0;
        private readonly IList<int> BannedAbilities = Array.Empty<int>();

        private int GetRandomAbility()
        {
            const int WonderGuard = 25;
            while (true)
            {
                int newabil = Rand.Next(1, Game.MaxAbilityID + 1);
                if (newabil == WonderGuard && Settings.WonderGuard == Permissive.No)
                    continue;
                if (BannedAbilities.Contains(newabil))
                    continue;
                return newabil;
            }
        }
    }
}
