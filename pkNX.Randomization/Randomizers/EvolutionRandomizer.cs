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
        public readonly SpeciesRandomizer Randomizer;

        public EvolutionRandomizer(GameInfo game, EvolutionSet[] evolutions, PersonalTable t)
        {
            Game = game;
            Evolutions = evolutions;
            Randomizer = new SpeciesRandomizer(Game, t);
        }

        public override void Execute()
        {
            for (var i = 0; i < Evolutions.Length; i++)
            {
                var evo = Evolutions[i];
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

        private void Randomize(EvolutionSet evos, int species)
        {
            foreach (var evo in evos.PossibleEvolutions)
            {
                if (evo.Method != 0)
                {
                    evo.Species = Randomizer.GetRandomSpecies(evo.Species, species);
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
                case EvolutionType.TradeSpecies:
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
    }
}