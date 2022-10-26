using pkNX.Structures;
using pkNX.Structures.FlatBuffers;

namespace pkNX.Game;

public class GameData8a
{
    public IPersonalTable PersonalData { get; init; } = null!;
    public DataCache<Waza8a> MoveData { get; init; } = null!;
    public TableCache<EvolutionTable8, EvolutionSet8a> EvolutionData { get; init; } = null!;
    public TableCache<Learnset8a, Learnset8aMeta> LevelUpData { get; init; } = null!;

    public TableCache<PokeDropItemArchive8a, PokeDropItem8a> FieldDrops { get; init; } = null!;
    public TableCache<PokeDropItemBattleArchive8a, PokeDropItemBattle8a> BattleDrops { get; init; } = null!;
    public TableCache<PokedexResearchTable, PokedexResearchTask> DexResearch { get; init; } = null!;
}
