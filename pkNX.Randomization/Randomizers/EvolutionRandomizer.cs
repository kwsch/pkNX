using pkNX.Structures;

namespace pkNX.Randomization
{
    /// <summary>
    /// <see cref="EvolutionSet"/> randomizer.
    /// </summary>
    public class EvolutionRandomizer : Randomizer
    {
        private readonly EvolutionSet[] Evolutions;
        private readonly GameInfo Game;
        private readonly PersonalTable Personal;
        public readonly SpeciesRandomizer RandSpec;
        public readonly FormRandomizer RandForm;

        public EvolutionRandomizer(GameInfo game, EvolutionSet[] evolutions, PersonalTable t)
        {
            Game = game;
            Personal = t;
            Evolutions = evolutions;
            RandSpec = new SpeciesRandomizer(Game, t);
            RandForm = new FormRandomizer(t);
        }

        public override void Execute()
        {
            for (var i = 0; i < Evolutions.Length; i++)
            {
                var evo = Evolutions[i];
                if (Personal[i].HP == 0)
                    continue;
                Randomize(evo, i);
            }
        }

        public void ExecuteTrade()
        {
            for (var i = 0; i < Evolutions.Length; i++)
            {
                var evo = Evolutions[i];
                ReplaceTradeMethods(evo, i);
            }
        }

        public void ExecuteEvolveEveryLevel()
        {
            for (var i = 0; i < Evolutions.Length; i++)
            {
                var evo = Evolutions[i];
                if (Personal[i].HP == 0)
                    continue;
                Personal[i].EXPGrowth = (int)EXPGroup.Slow; // keep everything the same to preserve levels after evolving
                MakeEvolveEveryLevel(evo, i);
            }
        }

        private void Randomize(EvolutionSet evos, int species)
        {
            foreach (var evo in evos.PossibleEvolutions)
            {
                if (evo.Method != 0)
                {
                    evo.Species = RandSpec.GetRandomSpecies(evo.Species, species);
                    evo.Form = RandForm.GetRandomForme(evo.Species, false, false, true, Game.SWSH, Personal.Table);
                }
            }
        }

        private void ReplaceTradeMethods(EvolutionSet evos, int species)
        {
            for (var i = 0; i < evos.PossibleEvolutions.Length; i++)
            {
                var evo = evos.PossibleEvolutions[i];
                ReplaceTradeMethod(evo, species, i);
            }
        }

        private void ReplaceTradeMethod(EvolutionMethod evo, int species, int evoIndex)
        {
            switch (evo.Method)
            {
                case EvolutionType.Trade when Game.Generation == 6:
                    evo.Method = EvolutionType.LevelUp; // trade -> level up
                    evo.Argument = 30;
                    return;
                case EvolutionType.Trade when Game.Generation >= 7:
                    evo.Method = EvolutionType.LevelUp; // trade -> level up
                    evo.Level = 30;
                    return;
                case EvolutionType.TradeHeldItem:
                    evo.Method = EvolutionType.LevelUpHeldItemDay;
                    return;
                case EvolutionType.TradeShelmetKarrablast:
                    evo.Method = EvolutionType.LevelUpWithTeammate;
                    if (species == (int)Species.Karrablast)
                        evo.Argument = (int)Species.Shelmet; // Karrablast with Shelmet
                    if (species == (int)Species.Shelmet)
                        evo.Argument = (int)Species.Karrablast; // Shelmet with Karrablast
                    return;

                case EvolutionType.LevelUpVersion:
                    evo.Method = evoIndex == 0 ? EvolutionType.LevelUpECl5 : EvolutionType.LevelUpECgeq5;
                    evo.Argument = 0; // clear ver
                    return;
                case EvolutionType.LevelUpVersionDay:
                    evo.Method = EvolutionType.LevelUpFriendshipMorning;
                    evo.Argument = 0; // clear ver
                    return;
                case EvolutionType.LevelUpVersionNight:
                    evo.Method = EvolutionType.LevelUpFriendshipNight;
                    evo.Argument = 0; // clear ver
                    return;
            }
        }

        private static void MakeEvolveEveryLevel(EvolutionSet evos, int species)
        {
            var evoSet = evos.PossibleEvolutions;
            evoSet[0] = new EvolutionMethod
            {
                Argument = 0, // clear
                Form = 0, // randomized later
                Level = 1,
                Method = EvolutionType.LevelUp,
                Species = species, // randomized later
            };

            if (evoSet[1].HasData) // has other branched evolutions; remove them
            {
                for (int i = 1; i < evoSet.Length; i++)
                    evoSet[i] = new EvolutionMethod();
            }
        }
    }
}