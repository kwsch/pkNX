using pkNX.Structures;
using pkNX.Structures.FlatBuffers.Arceus;

namespace pkNX.Game;

public class GameData8a
{
    public IPersonalTable PersonalData { get; init; } = null!;
    public DataCache<Waza> MoveData { get; init; } = null!;
    public TableCache<EvolutionTable, pkNX.Structures.FlatBuffers.Arceus.EvolutionSet> EvolutionData { get; init; } = null!;
    public TableCache<pkNX.Structures.FlatBuffers.Arceus.Learnset, LearnsetMeta> LevelUpData { get; init; } = null!;

    public TableCache<PokeDropItemArchive, PokeDropItem> FieldDrops { get; init; } = null!;
    public TableCache<PokeDropItemBattleArchive, PokeDropItemBattle> BattleDrops { get; init; } = null!;
    public TableCache<PokedexResearchTable, PokedexResearchTask> DexResearch { get; init; } = null!;
}
