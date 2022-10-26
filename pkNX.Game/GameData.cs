using pkNX.Structures;

namespace pkNX.Game;

/// <summary>
/// Cached data reusable by multiple editors.
/// </summary>
public class GameData
{
    public IPersonalTable PersonalData { get; init; } = null!;
    public DataCache<MegaEvolutionSet[]> MegaEvolutionData { get; init; } = null!;

    public DataCache<IMove> MoveData { get; init; } = null!;
    public DataCache<EvolutionSet> EvolutionData { get; init; } = null!;
    public DataCache<Learnset> LevelUpData { get; init; } = null!;
}
