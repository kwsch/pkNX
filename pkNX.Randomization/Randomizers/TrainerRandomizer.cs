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

        public LearnsetRandomizer Learn { get; set; }
        public SpeciesRandomizer RandSpec { get; set; }
        public MoveRandomizer RandMove { get; set; }
        public int ClassCount { get; set; }
        public IList<int> BannedMoves { get; set; }
        public Func<TrainerPoke> GetBlank { get; set; }
        public IList<int> FinalEvo { get; set; } = Array.Empty<int>();

        private TrainerRandSettings Settings;

        public TrainerRandomizer(GameInfo info, PersonalTable t, VsTrainer[] trainers)
        {
            Trainers = trainers;
            Info = info;
            Personal = t;

            PossibleHeldItems = Legal.GetRandomItemList(Info.Game);
            MegaDictionary = Legal.GetMegaDictionary(Info.Game);
            IndexFixedCount = GetFixedCountIndexes(Info.Game);
            SpecialClasses = GetSpecialClasses(Info.Game);
        }

        public void Initialize(TrainerRandSettings settings) => Settings = settings;

        public void SetBannedMoves(int[] moves)
        {
            var list = new List<int>(moves);
            list.AddRange(new[] { 165, 621, 464 }.Concat(Legal.Z_Moves)); // Struggle, Hyperspace Fury, Dark Void
            if (Settings.BanFixedDamageMoves)
                list.AddRange(MoveRandomizer.FixedDamageMoves);
            BannedMoves = list;
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
                SetupTeamCount(tr);
                if (Settings.TrainerMaxAI)
                    tr.Self.AI |= 7;

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

        private void SetupTeamCount(VsTrainer tr)
        {
            bool special = IndexFixedCount.TryGetValue(tr.ID, out var count);
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

            if (Settings.ForceSpecialTeamCount6 && special && count == 6)
            {
                for (int g = tr.Team.Count; g < 6; g++)
                    tr.Team.Add(GetBlankPKM(avgLevel, avgSpec));
            }
            else if (tr.Team.Count < min)
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
            if (Settings.SkipSpecialClasses && !SpecialClasses.Contains(tr.Self.Class))
            {
                int randClass() => Util.Random.Next(ClassCount);
                int rv;
                do
                {
                    rv = randClass();
                } while (SpecialClasses.Contains(rv)); // don't allow disallowed classes
                tr.Self.Class = rv;
            }

            // all classes
            else if (!Settings.SkipSpecialClasses)
            {
                int randClass() => Util.Random.Next(ClassCount);
                int rv;
                do
                {
                    rv = randClass();
                } while (rv == 082); // Lusamine 2 can crash multi battles, skip
                tr.Self.Class = rv;
            }
        }

        private void DetermineSpecies(IPokeData pk)
        {
            if (Settings.RandomizeTeam)
            {
                int Type = Settings.TeamTypeThemed ? Util.Random.Next(17) : -1;

                // replaces Megas with another Mega (Dexio and Lysandre in USUM)
                if (MegaDictionary.Any(z => z.Value.Contains(pk.HeldItem)))
                {
                    int[] mega = GetRandomMega(MegaDictionary, out int species);
                    pk.Species = species;
                    pk.HeldItem = mega[Util.Random.Next(mega.Length)];
                    pk.Form = 0; // allow it to Mega Evolve naturally
                }

                // every other pkm
                else
                {
                    pk.Species = RandSpec.GetRandomSpeciesType(pk.Species, Type);
                    pk.HeldItem = PossibleHeldItems[Util.Random.Next(PossibleHeldItems.Length)];
                    pk.Form = Legal.GetRandomForme(pk.Species, Settings.AllowRandomMegaForms, true, Personal.Table);
                }

                pk.Gender = 0; // random
                pk.Nature = Util.Random.Next(25); // random
            }

            if (Settings.ForceFullyEvolved && pk.Level >= Settings.ForceFullyEvolvedAtLevel && !FinalEvo.Contains(pk.Species))
            {
                int randFinalEvo() => Util.Random.Next(FinalEvo.Count);
                if (FinalEvo.Count != 0)
                    pk.Species = FinalEvo[randFinalEvo()];
                pk.Form = Legal.GetRandomForme(pk.Species, Settings.AllowRandomMegaForms, true, Personal.Table);
            }
        }

        private void UpdatePKMFromSettings(TrainerPoke pk)
        {
            if (Settings.BoostLevel)
                pk.Level = Legal.GetModifiedLevel(pk.Level, Settings.LevelBoostRatio);
            if (Settings.RandomShinies)
                pk.Shiny = Util.Random.Next(0, 100 + 1) < Settings.ShinyChance;
            if (Settings.RandomAbilities)
                pk.Ability = (int)Util.Rand32() % 4;
            if (Settings.MaxIVs)
                pk.IVs = new[] { 31, 31, 31, 31, 31, 31 };

            RandomizeEntryMoves(pk);
        }

        private void RandomizeEntryMoves(TrainerPoke pk)
        {
            switch (Settings.MoveRandType)
            {
                case MoveRandType.Random: // Random
                    pk.Moves = RandMove.GetRandomMoveset(pk.Species);
                    break;
                case MoveRandType.CurrentMoves:
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

        private static readonly int[] royal = { 081, 082, 083, 084, 185 };
        private static readonly Dictionary<int, int> FixedSM = royal.ToDictionary(z => z, _ => 1);

        private static Dictionary<int, int> GetFixedCountIndexes(GameVersion game)
        {
            if (GameVersion.SM.Contains(game) || GameVersion.USUM.Contains(game))
                return FixedSM;
            return new Dictionary<int, int>();
        }

        private static int[] GetSpecialClasses(GameVersion game)
        {
            return Array.Empty<int>();
        }
    }
}