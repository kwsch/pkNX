using pkNX.Structures;

namespace pkNX.Game
{
    /// <summary>
    /// Cached data reusable by multiple editors.
    /// </summary>
    public class GameData
    {
        public PersonalTable PersonalData { get; internal set; }
        public MegaEvolutionTable MegaEvolutionData { get; internal set; }

        public string[][] GameText { get; internal set; }
        public Move[] MoveData { get; internal set; }
        public EvolutionSet[] EvolutionData { get; internal set; }
        public Learnset[] LevelUpData { get; internal set; }
    }
}
