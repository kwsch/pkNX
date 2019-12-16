using System;
using System.Collections.Generic;
using System.Linq;
using pkNX.Structures;

namespace pkNX.Randomization
{
    public class TrainerRandomizer : Randomizer
    {
        private readonly GameInfo Info;
        private readonly PersonalTable Personal;
        private readonly VsTrainer[] Trainers;
        private readonly int[] PossibleHeldItems;
        private readonly Dictionary<int, int[]> MegaDictionary;
        private readonly Dictionary<int, int> IndexFixedCount;
        private readonly IList<int> SpecialClasses;
        private readonly IList<int> CrashClasses;

        public GenericRandomizer<int> Class { get; set; }
        public LearnsetRandomizer Learn { get; set; }
        public SpeciesRandomizer RandSpec { get; set; }
        public MoveRandomizer RandMove { get; set; }
        public int ClassCount { get; set; }
        public Func<TrainerPoke> GetBlank { get; set; }
        public EvolutionSet[] Evos { get; set; }

        private TrainerRandSettings Settings;

        public TrainerRandomizer(GameInfo info, PersonalTable t, VsTrainer[] trainers, EvolutionSet[] evos)
        {
            Trainers = trainers;
            Info = info;
            Personal = t;
            Evos = evos;

            PossibleHeldItems = Legal.GetRandomItemList(Info.Game);
            MegaDictionary = Legal.GetMegaDictionary(Info.Game);
            IndexFixedCount = GetFixedCountIndexes(Info.Game);
            SpecialClasses = GetSpecialClasses(Info.Game);
            CrashClasses = GetCrashClasses(Info.Game);
        }

        public void Initialize(TrainerRandSettings settings)
        {
            Settings = settings;

            IEnumerable<int> classes = Enumerable.Range(0, ClassCount).Except(CrashClasses);
            if (Settings.SkipSpecialClasses)
                classes = classes.Except(SpecialClasses);
            Class = new GenericRandomizer<int>(classes.ToArray());
        }

        public override void Execute()
        {
            foreach (var tr in Trainers)
            {
                if (tr.Team.Count == 0)
                    continue;

                // Trainer
                if (Settings.RandomTrainerClass)
                    SetRandomClass(tr);
                if (Settings.ModifyTeamCount)
                    SetupTeamCount(tr);
                if (Settings.TrainerMaxAI)
                    MaximizeAIFlags(tr);

                // Team
                foreach (var pk in tr.Team)
                {
                    if (pk.Species == 0)
                        continue;
                    DetermineSpecies(pk);
                    UpdatePKMFromSettings(pk);
                }
            }
        }

        public static void MaximizeAIFlags(VsTrainer tr)
        {
            const TrainerAI max = (TrainerAI.Basic | TrainerAI.Strong | TrainerAI.Expert | TrainerAI.PokeChange);
            tr.Self.AI |= (int)max;
        }

        private void SetupTeamCount(VsTrainer tr)
        {
            bool special = IndexFixedCount.TryGetValue(tr.ID, out var count);
            special &= (count != 6 || Settings.ForceSpecialTeamCount6);
            int min = special ? count : Settings.TeamCountMin;
            int max = special ? count : Settings.TeamCountMax;

            var avgBST = (int)tr.Team.Average(pk => Personal[pk.Species].BST);
            int avgLevel = (int)tr.Team.Average(pk => pk.Level);
            var pinfo = Personal.Table.OrderBy(pk => Math.Abs(avgBST - pk.BST)).First();
            int avgSpec = Array.IndexOf(Personal.Table, pinfo);

            if (Settings.ForceDoubles && !(special && count % 2 == 1))
            {
                if (tr.Team.Count % 2 != 0)
                    tr.Team.Add(GetBlankPKM(avgLevel, avgSpec));
                tr.Self.AI |= (int)TrainerAI.Doubles;
                tr.Self.Mode = BattleMode.Doubles;
            }

            if (tr.Team.Count < min)
            {
                for (int p = tr.Team.Count; p < min; p++)
                    tr.Team.Add(GetBlankPKM(avgLevel, avgSpec));
            }
            else if (tr.Team.Count > max)
            {
                tr.Team.RemoveRange(max, tr.Team.Count - max);
            }
        }

        private void SetRandomClass(VsTrainer tr)
        {
            // ignore special classes
            if (Settings.SkipSpecialClasses && SpecialClasses.Contains(tr.Self.Class))
                return;

            if (CrashClasses.Contains(tr.Self.Class))
                return; // keep as is

            tr.Self.Class = Class.Next();
        }

        private void DetermineSpecies(IPokeData pk)
        {
            if (Settings.RandomizeTeam)
            {
                int Type = Settings.TeamTypeThemed ? Util.Random.Next(17) : -1;
                RandomizeSpecFormItem(pk, Type);

                pk.Gender = 0; // random
                pk.Nature = Util.Random.Next(25); // random
            }
        }

        private void RandomizeSpecFormItem(IPokeData pk, int Type)
        {
            if (pk is TrainerPoke7b p7b)
            {
                RandomizeSpecForm(p7b, Type);
                return;
            }

            // replaces Megas with another Mega (Dexio and Lysandre in USUM)
            if (MegaDictionary.Any(z => z.Value.Contains(pk.HeldItem)))
            {
                int[] mega = GetRandomMega(MegaDictionary, out int species);
                pk.Species = species;
                int index = Util.Random.Next(mega.Length);
                pk.HeldItem = mega[index];
                pk.Form = 0; // allow it to Mega Evolve naturally
            }
            else // every other pkm
            {
                pk.Species = RandSpec.GetRandomSpeciesType(pk.Species, Type);
                TryForceEvolve(pk);
                pk.Form = Legal.GetRandomForme(pk.Species, Settings.AllowRandomMegaForms, true, true, Personal);
            }
        }

        private void RandomizeSpecForm(TrainerPoke7b pk, int type)
        {
            bool isMega = pk.MegaFormChoice != 0;
            if (isMega)
            {
                int[] mega = GetRandomMega(MegaDictionary, out int species);
                pk.Species = species;
                pk.MegaFormChoice = Util.Random.Next(mega.Length);
                pk.CanMegaEvolve = true;
                pk.Form = Legal.GetRandomForme(pk.Species, Settings.AllowRandomMegaForms, true, false, Personal);
                return;
            }

            pk.MegaFormChoice = 0;
            pk.Species = RandSpec.GetRandomSpeciesType(pk.Species, type);
            TryForceEvolve(pk);
            pk.Form = Legal.GetRandomForme(pk.Species, Settings.AllowRandomMegaForms, true, false, Personal);
        }

        private void TryForceEvolve(IPokeData pk)
        {
            if (!Settings.ForceFullyEvolved || pk.Level < Settings.ForceFullyEvolvedAtLevel)
                return;

            var evos = Evos;
            int species = pk.Species;
            int form = pk.Form;

            int timesEvolved = TryForceEvolve(evos, ref species, ref form);
            if (timesEvolved == 0)
                return;
            pk.Species = species;
            pk.Form = form;
        }

        private int TryForceEvolve(IReadOnlyList<EvolutionSet> evos, ref int species, ref int form)
        {
            int timesEvolved = 0;
            do
            {
                var index = Personal.GetFormeIndex(species, form);
                var eSet = evos[index].PossibleEvolutions;
                int evoCount = eSet.Count(z => z.HasData);
                if (evoCount == 0)
                    break;
                ++timesEvolved;
                var next = Util.Random.Next(evoCount);
                var nextEvo = eSet[next];
                species = nextEvo.Species;
                form = nextEvo.Form >= 0 ? nextEvo.Form : form;
            }
            while (timesEvolved < 3); // prevent randomized evos from looping excessively
            return timesEvolved;
        }

        private void UpdatePKMFromSettings(TrainerPoke pk)
        {
            if (Settings.AllowRandomHeldItems)
                pk.HeldItem = PossibleHeldItems[Util.Random.Next(PossibleHeldItems.Length)];
            if (Settings.BoostLevel)
                BoostLevel(pk, Settings.LevelBoostRatio);
            if (Settings.RandomShinies)
                pk.Shiny = Util.Random.Next(0, 100 + 1) < Settings.ShinyChance;
            if (Settings.RandomAbilities)
                pk.Ability = Util.Random.Next(1, 4); // 1, 2, or H
            if (Settings.MaxIVs)
                pk.IVs = new[] { 31, 31, 31, 31, 31, 31 };
            if (Settings.MaxDynamaxLevel && pk is TrainerPoke8 c && c.DynamaxLevel != 0)
                c.DynamaxLevel = 10;

            RandomizeEntryMoves(pk);
        }

        public static void BoostLevel(IPokeData pk, double ratio)
        {
            pk.Level = Legal.GetModifiedLevel(pk.Level, ratio);
        }

        public void ModifyAllPokemon(Action<IPokeData> act)
        {
            if (act == null)
                throw new ArgumentException(nameof(act));

            foreach (var tr in Trainers.Where(z => z.Team.Count != 0))
            {
                foreach (var pk in tr.Team)
                {
                    if (pk.Species != 0)
                        act(pk);
                }
            }
        }

        public void ModifyAllTrainers(Action<VsTrainer> act)
        {
            if (act == null)
                throw new ArgumentException(nameof(act));

            foreach (var tr in Trainers.Where(z => z.Team.Count != 0))
            {
                act(tr);
            }
        }

        private void RandomizeEntryMoves(TrainerPoke pk)
        {
            switch (Settings.MoveRandType)
            {
                case MoveRandType.RandomMoves: // Random
                    pk.Moves = RandMove.GetRandomMoveset(pk.Species);
                    break;
                case MoveRandType.LevelUpMoves:
                    pk.Moves = Learn.GetCurrentMoves(pk.Species, pk.Form, pk.Level);
                    break;
                case MoveRandType.HighPowered:
                    pk.Moves = Learn.GetHighPoweredMoves(pk.Species, pk.Form);
                    break;
                case MoveRandType.MetronomeOnly: // Metronome
                    pk.Moves = new[] { 118, 0, 0, 0 };
                    break;
                default:
                    return;
            }

            // sanitize moves
            var moves = pk.Moves;
            if (RandMove.SanitizeMovesetForBannedMoves(moves, pk.Species))
                pk.Moves = moves;
        }

        private TrainerPoke GetBlankPKM(int avgLevel, int avgSpec)
        {
            var pk = GetBlank();
            pk.Species = RandSpec.GetRandomSpecies(avgSpec);
            pk.Level = avgLevel;
            return pk;
        }

        private static int[] GetRandomMega(Dictionary<int, int[]> megas, out int species)
        {
            int rnd = Util.Random.Next(megas.Count);
            species = megas.Keys.ElementAt(rnd);
            return megas.Values.ElementAt(rnd);
        }

        // 1 poke max
        private static readonly int[] royal = { 081, 082, 083, 084, 185 };

        // 3 poke max
        private static readonly int[] MultiBattle_GG =
        {
            007, 008, 020, 021, 024, 025, 032, 033, 050, 051, // Jessie & James
            028, 029, 030, 031, // Rival vs Archer & Grunt
        };

        // 3 poke max
        private static readonly int[] MultiBattle_SWSH =
        {
            156, 157, 158, 197, 198, 199, 225, 226, 227, 312, 313, 314, // Hop
            223, 224, // Sordward and Shielbert
        };

        private static Dictionary<int, int> GetFixedCountIndexes(GameVersion game)
        {
            if (GameVersion.XY.Contains(game))
                return Legal.ImportantTrainers_XY.ToDictionary(z => z, _ => 6);
            if (GameVersion.ORAS.Contains(game))
                return Legal.ImportantTrainers_ORAS.ToDictionary(z => z, _ => 6);
            if (GameVersion.SM.Contains(game))
                return Legal.ImportantTrainers_SM.ToDictionary(z => z, index => royal.Contains(index) ? 1 : 6);
            if (GameVersion.USUM.Contains(game))
                return Legal.ImportantTrainers_USUM.ToDictionary(z => z, index => royal.Contains(index) ? 1 : 6);
            if (GameVersion.GG.Contains(game))
                return Legal.ImportantTrainers_GG.ToDictionary(z => z, index => MultiBattle_GG.Contains(index) ? 3 : 6);
            if (GameVersion.SWSH.Contains(game))
                return Legal.ImportantTrainers_SWSH.ToDictionary(z => z, index => MultiBattle_SWSH.Contains(index) ? 3 : 6);
            return new Dictionary<int, int>();
        }

        private static readonly int[] CrashClasses_GG = Enumerable.Range(72, 382 - 72 + 1).ToArray();
        private static readonly int[] CrashClasses_SWSH = Enumerable.Range(94, 151 - 94 + 1).Concat(Enumerable.Range(219, 236 - 219 + 1)).Concat(Legal.UnusedClasses_SWSH).ToArray();

        private static int[] GetSpecialClasses(GameVersion game)
        {
            if (GameVersion.SWSH.Contains(game))
                return Legal.SpecialClasses_SWSH;
            if (GameVersion.GG.Contains(game))
                return Legal.SpecialClasses_GG;
            if (GameVersion.SM.Contains(game))
                return Legal.SpecialClasses_SM;
            if (GameVersion.USUM.Contains(game))
                return Legal.SpecialClasses_USUM;
            if (GameVersion.ORAS.Contains(game))
                return Legal.SpecialClasses_ORAS;
            if (GameVersion.XY.Contains(game))
                return Legal.SpecialClasses_XY;
            return Array.Empty<int>();
        }

        private static int[] GetCrashClasses(GameVersion game)
        {
            if (GameVersion.SWSH.Contains(game))
                return CrashClasses_SWSH;
            if (GameVersion.GG.Contains(game))
                return CrashClasses_GG;
            return Array.Empty<int>();
        }
    }
}