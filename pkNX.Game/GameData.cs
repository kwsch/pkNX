using pkNX.Structures;

namespace pkNX.Game;

/// <summary>
/// Cached data reusable by multiple editors.
/// </summary>
public class GameData
{
    public PersonalTable PersonalData { get; internal set; }
    public DataCache<MegaEvolutionSet[]> MegaEvolutionData { get; internal set; }

    public DataCache<IMove> MoveData { get; internal set; }
    public DataCache<EvolutionSet> EvolutionData { get; internal set; }
    public DataCache<Learnset> LevelUpData { get; internal set; }
}
