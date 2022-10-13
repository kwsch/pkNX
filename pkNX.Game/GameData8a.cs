using pkNX.Structures;
using pkNX.Structures.FlatBuffers;

namespace pkNX.Game;

public class GameData8a
{
    public IPersonalTable PersonalData { get; internal set; }
    public TableCache<PokeMiscTable8a, PokeMisc8a> PokeMiscData { get; internal set; }
    public TableCache<PokeAIArchive8a, PokeAI8a> SymbolBehaveData { get; internal set; }

    public DataCache<Waza8a> MoveData { get; internal set; }
    public TableCache<EvolutionTable8, EvolutionSet8a> EvolutionData { get; internal set; }
    public TableCache<Learnset8a, Learnset8aMeta> LevelUpData { get; internal set; }

    public TableCache<PokeDropItemArchive8a, PokeDropItem8a> FieldDrops { get; set; }
    public TableCache<PokeDropItemBattleArchive8a, PokeDropItemBattle8a> BattleDrops { get; set; }
    public TableCache<PokedexResearchTable, PokedexResearchTask> DexResearch { get; set; }
}
